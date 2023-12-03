using Medicines.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.Data.Models
{
    public class Patient
    {
        public Patient()
        {
            PatientsMedicines = new List<PatientMedicine>();
        }
        [Key]
        public int Id { get; set; }
        [MinLength(5),MaxLength(100),Required]
        public string FullName { get; set; }
        [Required]
        public AgeGroup AgeGroup { get; set; }
        [Required]
        public Gender Gender { get; set; }
        public ICollection<PatientMedicine> PatientsMedicines  { get; set; }
    }
}
