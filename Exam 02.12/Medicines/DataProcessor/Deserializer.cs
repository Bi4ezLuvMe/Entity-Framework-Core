namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ImportDtos;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

        public static string ImportPatients(MedicinesContext context, string jsonString)
        {
            StringBuilder sb = new();
            var patients = JsonConvert.DeserializeObject<List<ImportPatientsDTO>>(jsonString);
            List<Patient> validPatients = new();
            int count = 0;
            foreach (var patient in patients)
            {
                if (!IsValid(patient))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Patient p = new()
                {
                    FullName = patient.FullName,
                    AgeGroup =(AgeGroup) patient.AgeGroup,
                    Gender = (Gender)patient.Gender
                };
                foreach (var medicineId in patient.Medicines)
                {
                    if (p.PatientsMedicines.Any(x=>x.MedicineId==medicineId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    p.PatientsMedicines.Add(new PatientMedicine
                    {
                        MedicineId = medicineId,
                    });
                    count++;
                }
                sb.AppendLine($"Successfully imported patient - {p.FullName} with {p.PatientsMedicines.Count()} medicines.");
                validPatients.Add(p);
            }
            context.Patients.AddRange(validPatients);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportPharmacies(MedicinesContext context, string xmlString)
        {
            StringBuilder sb = new();
            XmlSerializer serializer = new XmlSerializer(typeof(List<ImportPharmaciesDTO>), new XmlRootAttribute("Pharmacies"));
            List<ImportPharmaciesDTO> pharmaciesDTOs = new();
            List<Pharmacy> pharmacies = new();
            int count = 0;
            using (StringReader reader = new(xmlString))
            {
                pharmaciesDTOs = (List<ImportPharmaciesDTO>)serializer.Deserialize(reader);
            }
            foreach (var pharmacy in pharmaciesDTOs)
            {
                if (!IsValid(pharmacy))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (pharmacy.IsNonStop != "true" && pharmacy.IsNonStop != "false")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Pharmacy p = new()
                {
                    Name = pharmacy.Name,
                    PhoneNumber = pharmacy.PhoneNumber,
                    IsNonStop = pharmacy.IsNonStop == "true"?true:false
                };
                foreach (var medicines in pharmacy.Medicines)
                {
                    if (!IsValid(medicines))
                    {
                        sb.AppendLine(ErrorMessage); continue;
                    }
                    DateTime ProductionDate;
                    bool ProductionDateIsValid = DateTime.TryParseExact(medicines.ProductionDate,
                        "yyyy-MM-dd", CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out ProductionDate);
                    if (!ProductionDateIsValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    DateTime ExpiryDate;
                    bool ExpiryDateIsValid = DateTime.TryParseExact(medicines.ExpiryDate,
                        "yyyy-MM-dd", CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out ExpiryDate);
                    if (!ExpiryDateIsValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (ProductionDate == ExpiryDate||ExpiryDate<ProductionDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Medicine m = new()
                    {
                        Name = medicines.Name,
                        Price = medicines.Price,
                        ProductionDate = ProductionDate,
                        ExpiryDate = ExpiryDate,
                        Producer = medicines.Producer,
                        Category = (Category)medicines.Category
                    };
                    var medicinesduplicated = p.Medicines.Where(x => x.Name == medicines.Name && x.Producer == medicines.Producer);
                    if (medicinesduplicated.Count() == 1)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    p.Medicines.Add(m);
                    
                    count++;
                }
                sb.AppendLine($"Successfully imported pharmacy - {p.Name} with {p.Medicines.Count()} medicines.");
                pharmacies.Add(p);
               
            }
            context.Pharmacies.AddRange(pharmacies);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
