namespace VaporStore.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public User()
        {
            this.Cards = new HashSet<Card>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        public int Age { get; set; }

        public virtual ICollection<Card> Cards { get; set; }
    }
}

//•	Username – text with length [3,  
//    •	FullName – text, which has two words, consisting of Latin letters. Both start with an upper letter and are followed by lower letters. The two words are separated by a single space (ex. "John Smith") 
//    •	Age – integer in the range[3, 103] 
//    •	Cards – collection of type Card
