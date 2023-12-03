using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boardgames.Data.Models
{
    public class Creator
    {
        public Creator()
        {
            Boardgames = new List<Boardgame>();
        }
        [Key]
        public int Id { get; set; }
        [MinLength(2),MaxLength(7),Required]
        public string FirstName { get; set; }
        [MinLength(2), MaxLength(7), Required]
        public string LastName { get; set; }
        public ICollection<Boardgame> Boardgames { get; set; }
    }
}
