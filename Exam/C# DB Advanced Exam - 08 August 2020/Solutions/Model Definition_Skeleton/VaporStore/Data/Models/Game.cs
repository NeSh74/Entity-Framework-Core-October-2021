namespace VaporStore.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Game
    {
        public Game()
        {
            this.Purchases = new HashSet<Purchase>();
            this.GameTags = new List<GameTag>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; } //by default required

        public DateTime ReleaseDate { get; set; }

        public int DeveloperId { get; set; }//by default required

        public virtual Developer Developer { get; set; } //given required by DeveloperId

        public int GenreId { get; set; }

        public virtual Genre Genre { get; set; }

        public ICollection<Purchase> Purchases { get; set; }

        public ICollection<GameTag> GameTags { get; set; }
    }
}