using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footballers.Data.Models
{
    public class TeamFootballer
    {
        [Required]
        public int TeamId { get; set; }
        [Required]
        public int FootballerId  { get; set; }
        [ForeignKey("TeamId")]
        public Team Team { get; set; }
        [ForeignKey("FootballerId")]
        public Footballer Footballer { get; set; }
    }
}
