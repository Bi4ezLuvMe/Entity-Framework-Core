using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType("Creator")]
    public class ImportCreatorsDTO
    {
        [MinLength(2), MaxLength(7), Required,XmlElement("FirstName")]
        public string FirstName { get; set; }
        [MinLength(2), MaxLength(7), Required,XmlElement("LastName")]
        public string LastName { get; set; }
        [XmlArray("Boardgames")]
        public ImportBoardGamesDTO[] BoardGames { get; set; }
    }
}
