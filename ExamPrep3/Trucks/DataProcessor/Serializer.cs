namespace Trucks.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using System.Text;
    using System.Xml.Serialization;
    using Trucks.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            XmlSerializer serializer = new(typeof(List<ExportDespatchersDTO>), new XmlRootAttribute("Despatchers"));
            StringBuilder sb = new();
            using StringWriter writer = new StringWriter(sb);
            XmlSerializerNamespaces namespaces = new();
            namespaces.Add("", "");
            List<ExportDespatchersDTO> despatchers = context.Despatchers.OrderByDescending(x => x.Trucks.Count)
     .ThenBy(x => x.Name)
     .Where(x => x.Trucks.Any())
     .Select(x => new ExportDespatchersDTO
     {
         DespatcherName = x.Name,
         Trucks = (List<ExportTrucksDTO>)x.Trucks
             .OrderBy(truck => truck.RegistrationNumber)
             .Select(truck => new ExportTrucksDTO
             {
                 RegistrationNumber = truck.RegistrationNumber,
                 Make = truck.MakeType
             }).ToList()
     })
     .ToList();
            foreach (var despatcher in despatchers)
            {
                despatcher.TrucksCount = despatcher.Trucks.Count;
            }
            serializer.Serialize(writer, despatchers,namespaces);
            return sb.ToString();
        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            var clients = context
                 .Clients
                 .Where(x => x.ClientsTrucks.Any(x => x.Truck.TankCapacity >= capacity))
                 .ToArray()
                 .Select(x => new
                 {
                     Name = x.Name,
                     Trucks = x.ClientsTrucks
                     .Where(x => x.Truck.TankCapacity >= capacity)
                     .OrderBy(x => x.Truck.MakeType)
                     .ThenByDescending(x => x.Truck.CargoCapacity)
                     .Select(x => new
                     {
                         TruckRegistrationNumber = x.Truck.RegistrationNumber,
                         VinNumber = x.Truck.VinNumber,
                         TankCapacity = x.Truck.TankCapacity,
                         CargoCapacity = x.Truck.CargoCapacity,
                         CategoryType = x.Truck.CategoryType.ToString(),
                         MakeType = x.Truck.MakeType.ToString()
                     })
                 })
                 .OrderByDescending(x => x.Trucks.Count())
                 .ThenBy(x => x.Name)
                 .Take(10)
                 .ToArray();
            return JsonConvert.SerializeObject(clients, Formatting.Indented);
        }
    }
}
