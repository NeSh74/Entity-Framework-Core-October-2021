namespace SoftJail.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;


    public class Department
    {
        public Department()
        {
            this.Cells = new HashSet<Cell>();
        }
        public int Id { get; set; }

        [Required]
        [MaxLength(25)]
        public string Name { get; set; }

        public ICollection<Cell> Cells { get; set; }
    }
}