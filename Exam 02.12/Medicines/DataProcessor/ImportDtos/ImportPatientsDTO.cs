using Medicines.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.DataProcessor.ImportDtos
{
    public class ImportPatientsDTO
    {
        [MinLength(5), MaxLength(100), Required]
        public string FullName { get; set; }
        [Required,Range(0,2)]
        public int AgeGroup { get; set; }
        [Required,Range(0,1)]
        public int Gender { get; set; }
        [Required]
        public int[] Medicines { get; set; }
    }
}
