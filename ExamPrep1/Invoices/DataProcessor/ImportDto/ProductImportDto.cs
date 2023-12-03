using Invoices.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.DataProcessor.ImportDto
{
    public class ProductImportDto
    {
        [MinLength(9), MaxLength(30), Required]
        public string Name { get; set; } = null!;
        [Range(5,1000),Required]
        public decimal Price { get; set; }
        [Range(0,4),Required]
        public CategoryType CategoryType { get; set; }
        public int[] Clients { get; set; } = null!;
    }
}
