using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType("Pharmacy")]
    public class ImportPharmaciesDTO
    {
        [MinLength(2), MaxLength(50), Required,XmlElement("Name")]
        public string Name { get; set; }
        [StringLength(14), Required, RegularExpression("\\(\\d{3}\\) \\d{3}-\\d{4}"),XmlElement("PhoneNumber")]
        public string PhoneNumber { get; set; }
        [XmlAttribute("non-stop")]
        public string IsNonStop { get; set; }
        [XmlArray("Medicines")]
        public List<ImportMedicinesDTO> Medicines { get; set; }
    }
}
