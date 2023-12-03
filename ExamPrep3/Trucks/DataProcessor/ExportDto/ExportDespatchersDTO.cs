using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Trucks.DataProcessor.ExportDto
{
    [XmlType("Despatcher")]
    public class ExportDespatchersDTO
    {
        [XmlElement("DespatcherName")]
        public string DespatcherName { get; set; }
        [XmlAttribute("TrucksCount")]
        public int TrucksCount { get; set; }
        [XmlArray("Trucks")]
        public List<ExportTrucksDTO> Trucks { get; set; }
    }
}
