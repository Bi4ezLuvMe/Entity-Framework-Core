namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            StringBuilder sb = new();
            XmlSerializer serializer = new XmlSerializer(typeof(List<ImportDespatchersDTO>),new XmlRootAttribute("Despatchers"));
            List<ImportDespatchersDTO> despatchersDTOs = new();
            List<Despatcher> despatchers = new();
            int count = 0;
            using (StringReader reader  = new(xmlString))
            {
                despatchersDTOs = (List<ImportDespatchersDTO>)serializer.Deserialize(reader);
            }
            foreach (var despatcher in despatchersDTOs)
            {
                if (!IsValid(despatcher))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Despatcher d = new()
                {
                    Name = despatcher.Name,
                    Position = despatcher.Position
                };
                foreach (var truck in despatcher.Trucks)
                {
                    if (!IsValid(truck))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Truck t = new()
                    {
                        RegistrationNumber = truck.RegistrationNumber,
                        VinNumber =truck.VinNumber,
                        TankCapacity = truck.TankCapacity,
                        CargoCapacity = truck.CargoCapacity,
                        CategoryType = (CategoryType)truck.CategoryType,
                        MakeType = (MakeType)truck.MakeType,
                        DespatcherId = d.Id
                    };
                    d.Trucks.Add(t);
                    count++;
                }
                sb.AppendLine(String.Format(SuccessfullyImportedDespatcher,d.Name,d.Trucks.Count()));
                despatchers.Add(d);
            }
            context.Despatchers.AddRange(despatchers);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            StringBuilder sb = new();
            List<ImportClientsDTO> clientsDTOs = JsonConvert.DeserializeObject<List<ImportClientsDTO>>(jsonString);
            List<Client> clients = new();
            int count = 0;
            foreach (var client in clientsDTOs)
            {
                if (!IsValid(client))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (client.Type == "usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Client c = new()
                {
                    Name = client.Name,
                    Nationality = client.Nationality,
                    Type = client.Type
                };
                foreach (var truckId in client.Trucks.Distinct())
                {
                    Truck truck = context.Trucks.FirstOrDefault(x=>x.Id==truckId);
                    if(truck == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    c.ClientsTrucks.Add(new ClientTruck()
                    {
                        Truck = truck
                    });
                    count++;
                }
                sb.AppendLine(String.Format(SuccessfullyImportedClient,c.Name,c.ClientsTrucks.Count()));
                clients.Add(c);
            }
            context.Clients.AddRange(clients);
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