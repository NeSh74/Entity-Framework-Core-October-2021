using System.Globalization;
using SoftUni.Models;

namespace SoftUni
{
    using SoftUni.Data;
    using System;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        static void Main()
        {
            SoftUniContext db = new SoftUniContext();

            string result = RemoveTown(db);

            Console.WriteLine(result);
        }

        public static string RemoveTown(SoftUniContext context)
        {
            Address[] seattleAddresses = context
                .Addresses
                .Where(a => a.Town.Name == "Seattle")
                .ToArray();

            Employee[] employeesInSeattle = context
                .Employees
                .ToArray() 
                .Where(e => seattleAddresses.Any(a => a.AddressId == e.AddressId))
                .ToArray();

            foreach (Employee employee in employeesInSeattle)
            {
                employee.AddressId = null;
            }

            context.Addresses.RemoveRange(seattleAddresses);

            Town seattleTown = context
                .Towns
                .First(t => t.Name == "Seattle");

            context.Towns.Remove(seattleTown);

            context.SaveChanges();

            return $"{seattleAddresses.Length} addresses in Seattle were deleted";
        }
    }
}


