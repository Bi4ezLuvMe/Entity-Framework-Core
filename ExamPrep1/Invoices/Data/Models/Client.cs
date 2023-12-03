using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Data.Models
{
    public class Client
    {
        public Client()
        {
            Invoices = new List<Invoice>();
            Addresses = new List<Address>();
            ProductsClients = new List<ProductClient>();
        }
        [Key]
        public int Id { get; set; }
        [MaxLength(25),Required,MinLength(10)]
        public string Name { get; set; }
        [MaxLength(15),Required,MinLength(15)]
        public string NumberVat { get; set; }
        public ICollection<Invoice> Invoices { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<ProductClient> ProductsClients { get; set; }
    }
}
