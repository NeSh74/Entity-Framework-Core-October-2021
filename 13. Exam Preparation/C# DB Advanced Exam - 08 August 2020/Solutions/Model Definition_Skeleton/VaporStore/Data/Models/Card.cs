namespace VaporStore.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using VaporStore.Data.Models.Enums;

    public class Card
    {
        public Card()
        {
            this.Purchases = new HashSet<Purchase>();
        }
        public int Id { get; set; }

        [Required]
        //[MaxLength(19)]
        public string Number { get; set; }

        [Required]
        //[MaxLength(3)]
        public string Cvc { get; set; }

        public CardType Type { get; set; }//by default Required

        public int UserId { get; set; }

        public User User { get; set; }

        public ICollection<Purchase> Purchases { get; set; }

    }
}
////•	Id – integer, Primary Key
////•	Number – text, which consists of 4 pairs of 4 digits, separated by spaces (ex. “1234 5678 9012 3456”) 
////    •	Cvc – text, which consists of 3 digits (ex. “123”) 
////    •	UserId – integer, foreign key(required)
///
