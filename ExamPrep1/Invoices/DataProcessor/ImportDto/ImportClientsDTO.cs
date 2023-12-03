using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ImportDto
{
    [XmlType("Client")]
    public class ImportClientsDTO
    {
        [XmlElement("Name"), MaxLength(25), Required,MinLength(10)]
        public string Name { get; set; }
        [XmlElement("NumberVat"), MaxLength(15), Required,MinLength(10)]
        public string NumberVat { get; set; }
        [XmlArray("Addresses"),Required]
        public ImportAddressesDTO[] Addresses { get; set; }
    }
}
