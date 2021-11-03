using System;
using System.Collections.Generic;
using System.Text;

namespace EfCoreDemo.Models
{
    public class Club
    {
        public Club()
        {
            //this.Employees = new HashSet<EmployeeInClub>();
            this.Employees = new HashSet<Employee>();
        }
        public int Id { get; set; }

        public string  Name { get; set; }

        //public ICollection<EmployeeInClub> Employees { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
