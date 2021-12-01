using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace VaporStore.Data.Models
{
    using System.Text;

    public class Genre
    {
        public Genre()
        {
            this.Games = new HashSet<Game>();
        }
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}