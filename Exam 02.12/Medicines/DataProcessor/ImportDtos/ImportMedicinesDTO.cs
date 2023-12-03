using Medicines.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType("Medicine")]
    public class ImportMedicinesDTO
    {
        [MinLength(3), MaxLength(150), Required,XmlElement("Name")]
        public string Name { get; set; }
        [Range(0.01, 1000), Required,XmlElement("Price")]
        public decimal Price { get; set; }
        [Range(0,4),XmlAttribute("category"),Required]
        public int Category { get; set; }
        [Required,XmlElement("ProductionDate")]
        public string ProductionDate { get; set; }
        [Required,XmlElement("ExpiryDate")]
        public string ExpiryDate { get; set; }
        [MinLength(3), MaxLength(100), Required,XmlElement("Producer")]
        public string Producer { get; set; }
    }
}
