namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
        {
            return "kur";
        }

        public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
        {
            var medicience = context.Medicines
            .Where(x=>(int)x.Category == medicineCategory&&x.Pharmacy.IsNonStop)
            .OrderBy(x => x.Price)
                      .ThenBy(x => x.Name)
                      .ToList()
                      .Select(x => new
                      {
                          Name = x.Name,
                          Price = x.Price.ToString("F2"),
                          Pharmacy = new
                          {
                              Name = x.Pharmacy.Name,
                              PhoneNumber = x.Pharmacy.PhoneNumber
                          }
                      });
            
            return JsonConvert.SerializeObject(medicience, Formatting.Indented);
        }
    }
}
