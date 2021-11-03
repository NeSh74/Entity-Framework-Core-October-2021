using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDemo.Models
{
    [Table("People", Schema = "company")]
    [Index("Egn", IsUnique = true)]
    public class Employee
    {
        public Employee()
        {
            //this.ClubParticipations = new HashSet<EmployeeInClub>();
            this.ClubParticipations = new HashSet<Club>();
        }
        // [Key] //for case of EID
        //Id/EmployeeId
        public int Id { get; set; } //EID

        public string Egn { get; set; }

        [Column("FN", Order = 6)]
        [Required] //not null
        [MaxLength(25)]
        public string FirstName { get; set; }

        [Required] //not null
        [MaxLength(35)]
        public string LastName { get; set; }

        [NotMapped]
        public string FullName => $"{this.FirstName} {this.LastName}";

        [Column("StartedOn", TypeName = "date")]

        public DateTime? StartWorkDay { get; set; }

        public decimal? Salary { get; set; }

        // public int DID { get; set; } //optional DepartmentId
        public int DepartmentId { get; set; } //optional DepartmentId

        // [ForeignKey(nameof(DID))]
        //[InverseProperty("Employees")]
        //[ForeignKey("DepartmentId")]
        public Department Department { get; set; } //required

        [ForeignKey("Address")]
        public int? AddressId { get; set; }

        public Address Address { get; set; }

        //public ICollection<EmployeeInClub> ClubParticipations { get; set; }
        public ICollection<Club> ClubParticipations { get; set; }
    }
}
