using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boardgames.DataProcessor.ImportDto
{
    public class ImportSellersDTO
    {
        [MinLength(5), MaxLength(20), Required]
        public string Name { get; set; }
        [Required, MinLength(2), MaxLength(30)]
        public string Address { get; set; }
        [Required]
        public string Country { get; set; }
        [Required,RegularExpression(@"(www\.[a-zA-Z0-9\-]{2,256}\.com)")]
        public string Website { get; set; }
        [Required]
        public int[] BoardGames { get; set; }
    }
}
