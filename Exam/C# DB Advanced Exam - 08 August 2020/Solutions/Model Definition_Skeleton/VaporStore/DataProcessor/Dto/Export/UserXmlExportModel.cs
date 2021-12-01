namespace VaporStore.DataProcessor.Dto.Export
{
    using System.Xml.Serialization;

    [XmlType("User")]
    public class UserXmlExportModel
    {
        [XmlAttribute("username")]
        public string Username { get; set; }

        [XmlArray]
        public PurchaseXmlExportModel[] Purchases { get; set; }

        public decimal TotalSpent { get; set; }
    }
}
