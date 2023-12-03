using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boardgames.Data.Models
{
    public class BoardgameSeller
    {
        [Required]
        public int BoardgameId { get; set; }
        [Required]
        public int SellerId { get; set; }
        [ForeignKey("BoardgameId")]
        public Boardgame Boardgame { get; set; }
        [ForeignKey("SellerId")]
        public Seller Seller { get; set; }
    }
}
