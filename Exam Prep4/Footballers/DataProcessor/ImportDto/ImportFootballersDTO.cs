using Footballers.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Footballer")]
    public class ImportFootballersDTO
    {
        [Required, MinLength(2), MaxLength(40),XmlElement("Name")]
        public string Name { get; set; }
        [Required,XmlElement("ContractStartDate")]
        public DateTime ContractStartDate { get; set; }
        [Required,XmlElement("ContractEndDate")]
        public DateTime ContractEndDate { get; set; }
        [Required,XmlElement("PositionType")]
        public int PositionType { get; set; }
        [Required,XmlElement("BestSkillType")]
        public int BestSkillType { get; set; }
       
    }
}
