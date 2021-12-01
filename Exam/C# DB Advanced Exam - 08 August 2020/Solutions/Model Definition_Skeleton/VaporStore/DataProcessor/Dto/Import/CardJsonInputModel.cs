namespace VaporStore.DataProcessor.Dto.Import
{
    using System.ComponentModel.DataAnnotations;
    using VaporStore.Data.Models.Enums;

    public class CardJsonInputModel
    {
        //text, which consists of 4 pairs of 4 digits, separated by spaces(ex. “1234 5678 9012 3456”) (required)
        [Required]
        [RegularExpression("[0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}")]
        public string Number { get; set; }

        //text, which consists of 3 digits(ex. “123”) (required)
        [Required]
        [RegularExpression("[0-9]{3}")]
        public string CVC { get; set; }

        [Required]
        public CardType? Type { get; set; }
    }
}
