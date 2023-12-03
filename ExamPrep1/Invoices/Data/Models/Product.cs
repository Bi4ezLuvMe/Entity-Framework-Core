using Invoices.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Data.Models
{
    public class Product
    {
        public Product()
        {
            ProductsClients = new List<ProductClient>();
        }
        [Key]
        public int Id { get; set; }
        [MaxLength(30),Required]
        public string Name { get; set; }
        [MaxLength(1000),Required]
        public decimal Price { get; set; }
        [Required]
        public CategoryType CategoryType { get; set; }
        public ICollection<ProductClient> ProductsClients { get; set; }
    }
}
