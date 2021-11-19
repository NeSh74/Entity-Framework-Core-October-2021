using System.Xml.Serialization;

namespace CarDealer.DTO.ImportDto
{
    [XmlType("Supplier")]

    public class ImportSupplierDto
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("IsImporter")]
        public bool IsImporter { get; set; }
    }
}
