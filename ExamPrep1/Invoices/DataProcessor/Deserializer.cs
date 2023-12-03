namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";


        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            XmlSerializer serializer = new(typeof(List<ImportClientsDTO>),new XmlRootAttribute("Clients"));
            StringBuilder sb = new();
            List<ImportClientsDTO> clients = new List<ImportClientsDTO>();
            List<Client> validClients = new();
            using (StringReader reader = new(xmlString))
            {
                clients =(List<ImportClientsDTO>) serializer.Deserialize(reader);
            }
            foreach (var client in clients)
            {
                if (!IsValid(client))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Client currClient = new()
                {
                    Name = client.Name,
                    NumberVat = client.NumberVat
                };
                foreach (var address in client.Addresses)
                {
                    if (!IsValid(address))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Address address1 = new()
                    {
                        StreetName = address.StreetName,
                        StreetNumber = address.StreetNumber,
                        PostCode = address.PostCode,
                        City = address.City,
                        Country = address.Country
                    };
                    currClient.Addresses.Add(address1);
                }
                validClients.Add(currClient);
                sb.AppendLine($"Successfully imported client {currClient.Name}.");
            }
            context.Clients.AddRange(validClients);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            var invoices = JsonConvert.DeserializeObject<List<InvoiceImportDto>>(jsonString);
            List<Invoice> validInvoices = new();
            StringBuilder sb = new();
            foreach (var invoice in invoices)
            {
                if (!IsValid(invoice))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
               if(invoice.DueDate == DateTime.ParseExact("01/01/0001", "dd/MM/yyyy", CultureInfo.InvariantCulture) || invoice.IssueDate == DateTime.ParseExact("01/01/0001", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Invoice i = new Invoice()
                {
                    Number = invoice.Number,
                    IssueDate = invoice.IssueDate,
                    DueDate = invoice.DueDate,
                    CurrencyType = invoice.CurrencyType,
                    Amount = invoice.Amount,
                    ClientId = invoice.ClientId
                };
                if (i.DueDate < i.IssueDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                validInvoices.Add(i);
                sb.AppendLine($"Successfully imported invoice with number {i.Number}.");
            }
            context.Invoices.AddRange(validInvoices);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ProductImportDto[] productDtos = JsonConvert.DeserializeObject<ProductImportDto[]>(jsonString);

            List<Product> products = new List<Product>();

            foreach (ProductImportDto productDto in productDtos)
            {
                if (!IsValid(productDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Product p = new Product()
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    CategoryType = productDto.CategoryType,
                };

                foreach (int clientId in productDto.Clients.Distinct())
                {
                    Client c = context.Clients.Find(clientId);
                    if (c == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    p.ProductsClients.Add(new ProductClient()
                    {
                        Client = c
                    });
                }
                products.Add(p);
                sb.AppendLine(String.Format(SuccessfullyImportedProducts, p.Name, p.ProductsClients.Count));
            }
            context.Products.AddRange(products);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    } 
}
