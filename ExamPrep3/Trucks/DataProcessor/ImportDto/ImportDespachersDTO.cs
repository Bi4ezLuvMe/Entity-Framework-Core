using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Despatcher")]
    public class ImportDespatchersDTO
    {
        [Required, MinLength(2), MaxLength(40),XmlElement("Name")]
        public string Name { get; set; }
        [Required,MinLength(1), XmlElement("Position")]
        public string Position { get; set; }
        [Required]
        public ImportTrucksDTO[] Trucks { get; set; }
    }
}
