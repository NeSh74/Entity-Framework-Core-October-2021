using System.Collections.Generic;

namespace EfCoreDemo.Models
{
    public class Department
    {
        public Department()
        {
            this.Employees = new HashSet<Employee>();
        }

        public int Id { get; set; }

        public string  Name { get; set; }

        //inverse poperty
        public ICollection<Employee> Employees { get; set; } //optional
    }
}
