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

            string result = GetEmployeesWithSalaryOver50000(db);

            Console.WriteLine(result);
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context
                .Employees
                .Where(e => e.Salary > 50000)
                .OrderBy(e => e.FirstName)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}