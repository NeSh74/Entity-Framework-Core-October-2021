using System;
using System.Linq;
using EfCoreDemo.Models;

namespace EfCoreDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new ApplicationDbContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var department = new Department { Name = "HR" };
            var footballClub = new Club { Name = "Ritane" };
            var sewingClub = new Club { Name = "Shiene" };

            for (int i = 0; i < 10; i++)
            {
                //db.Employees.Add(new Employee
                var employee = new Employee()
                {
                    Egn = "123456789" + i,
                    FirstName = "Niki_" + i,
                    LastName = "Kostov",
                    StartWorkDay = new DateTime(2010 + i, 1, 1),
                    Salary = 100 + i,
                    Department = department,
                };
                employee.ClubParticipations.Add(footballClub);
                employee.ClubParticipations.Add(sewingClub);
                db.Employees.Add(employee);
            }

            db.SaveChanges();

            //var employeesInHR = db.Departments.Where(x => x.Name == "HR").Select(x => x.Employees.Count()).FirstOrDefault();
            var employeesInHR = db.Departments.Where(x => x.Name == "HR").Select(x => x.Employees.Max(e => e.Egn)).FirstOrDefault();

            Console.WriteLine(employeesInHR);

            var niki2Clubs = db.Employees
                .Where(x => x.FirstName == "Niki_2")
                .Select(x => x.ClubParticipations.Select(c => c.Name))
                .FirstOrDefault();

            foreach (var club in niki2Clubs)
            {
                Console.WriteLine(club);
            }
        }
    }
}
