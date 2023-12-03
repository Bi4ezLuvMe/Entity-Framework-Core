using Medicines.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.Data.Models
{
    public class Medicine
    {
        public Medicine()
        {
            PatientsMedicines = new List<PatientMedicine>();
        }
        [Key]
        public int Id { get; set; }
        [MinLength(3),MaxLength(150),Required]
        public string Name { get; set; }
        [Range(0.01,1000),Required]
        public decimal Price { get; set; }
        [Required]
        public Category Category { get; set; }
        [Required]
        public DateTime ProductionDate  { get; set; }
        [Required]
        public DateTime ExpiryDate  { get; set; }
        [MinLength(3),MaxLength(1000),Required]
        public string Producer { get; set; }
        [Required]
        public int PharmacyId  { get; set; }
        [ForeignKey("PharmacyId")]
        public Pharmacy Pharmacy { get; set; }
        public ICollection<PatientMedicine> PatientsMedicines { get; set; }
    }
}
