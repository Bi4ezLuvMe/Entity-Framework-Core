using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Footballers.Data.Models
{
    public class Team
    {
        public Team()
        {
            TeamsFootballers = new List<TeamFootballer>();
        }
        [Key]
        public int Id { get; set; }
        [Required,MinLength(3),MaxLength(40),RegularExpression("[A-Z*a-z*1-9*.*\\-*]")]
        public string Name { get; set; }
        [Required,MinLength(2),MaxLength(40)]
        public string Nationality  { get; set; }
        [Required]
        public int Trophies  { get; set; }
        public ICollection<TeamFootballer> TeamsFootballers { get; set; }
    }
}
