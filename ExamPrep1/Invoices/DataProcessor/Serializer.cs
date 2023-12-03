namespace Invoices.DataProcessor
{
    using AutoMapper.QueryableExtensions;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.Text;
    using System.Text.Json.Serialization;
    using System.Xml.Serialization;
    using Invoices.DataProcessor.ExportDto;
    using Invoices.DataProcessor.ImportDto;
    using AutoMapper;

    public class Serializer
    {
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<InvoicesProfile>();
            });
            StringBuilder sb = new();
            XmlSerializer serializer = new(typeof(List<ClientExportDto>),new XmlRootAttribute("Clients"));
            using StringWriter sw = new(sb);
            XmlSerializerNamespaces namespaces = new();
            namespaces.Add(string.Empty, string.Empty);
            ClientExportDto[] clientsDtos = context
               .Clients
               .Where(c => c.Invoices.Any(ci => ci.IssueDate >= date))
               .ProjectTo<ClientExportDto>(config)
               .OrderByDescending(c => c.InvoicesCount)
               .ThenBy(c => c.Name)
               .ToArray();

            serializer.Serialize(sw, clientsDtos, namespaces);
            return sb.ToString().TrimEnd();
        }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
        {
            var products = context
                   .Products
                   .Where(x =>
                   x.ProductsClients.Any(x => x.Client.Name.Length >= nameLength))
                   .ToList()
                   .Select(x => new
                   {
                       Name = x.Name,
                       Price = x.Price,
                       Category = x.CategoryType.ToString(),
                       Clients = x.ProductsClients
                       .Where(x=>x.Client.Name.Length>=nameLength)
                       .ToList()
                       .OrderBy(x => x.Client.Name)
                       .Select(c => new
                       {
                           Name = c.Client.Name,
                           NumberVat = c.Client.NumberVat
                       })
                       .ToList()
                   })
                     .OrderByDescending(x => x.Clients.Count())
                   .ThenBy(x => x.Name)
                   .Take(5)
                   .ToList();
            return JsonConvert.SerializeObject(products, Formatting.Indented);
        }
    }
}