using System.ComponentModel.DataAnnotations;

namespace VaporStore.DataProcessor.Dto.Import
{
    using System;
    using System.Collections.Generic;
    using System.Text;


    public class UserJsonInputModel
    {
        //text, which has two words, consisting of Latin letters.Both start with an upper letter and are followed by lower letters.The two words are separated by a single space (ex. "John Smith") (required)
        [Required]
        [RegularExpression("[A-Z][a-z]{2,} [A-Z][a-z]{2,}")]
        public string FullName { get; set; }

        //text with length[3, 20] (required)
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Username { get; set; }

        //text(required)
        [Required]
        public string Email { get; set; }

        //integer in the range[3, 103] (required)
        [Range(3,103)]
        public int Age { get; set; }

        //[MinLength(1)]
        public IEnumerable<CardJsonInputModel> Cards { get; set; }
    }
}
