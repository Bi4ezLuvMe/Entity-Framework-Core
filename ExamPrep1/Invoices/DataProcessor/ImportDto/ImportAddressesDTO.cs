using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ImportDto
{
    [XmlType("Address")]
    public class ImportAddressesDTO
    {
        [MaxLength(20),Required,XmlElement("StreetName"),MinLength(10)]
        public string StreetName { get; set; }
        [Required,XmlElement("StreetNumber")]
        public int StreetNumber { get; set; }
        [Required, XmlElement("PostCode")]
        public string PostCode { get; set; }
        [MaxLength(15), Required,XmlElement("City"),MinLength(5)]
        public string City { get; set; }
        [MaxLength(15), Required, XmlElement("Country"),MinLength(5)]
        public string Country { get; set; }
    }
}
