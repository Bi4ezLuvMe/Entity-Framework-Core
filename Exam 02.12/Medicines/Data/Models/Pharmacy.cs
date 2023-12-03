using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.Data.Models
{
    public class Pharmacy
    {
        public Pharmacy()
        {
            Medicines = new List<Medicine>();
        }
        [Key]
        public int Id { get; set; }
        [MinLength(2),MaxLength(50),Required]
        public string Name { get; set; }
        [StringLength(14),Required,RegularExpression("\\(\\d{3}\\) \\d{3}-\\d{4}")]
        public string PhoneNumber  { get; set; }
        [Required]
        public bool IsNonStop  { get; set; }
        public ICollection<Medicine> Medicines { get; set; }
    }
}
