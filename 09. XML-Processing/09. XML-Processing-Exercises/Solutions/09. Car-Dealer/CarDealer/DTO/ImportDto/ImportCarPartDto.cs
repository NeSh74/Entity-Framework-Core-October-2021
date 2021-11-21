using System.Xml.Serialization;
using CarDealer.Models;

namespace CarDealer.DTO.ImportDto
{
    [XmlType("partId")]
    public class ImportCarPartDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
