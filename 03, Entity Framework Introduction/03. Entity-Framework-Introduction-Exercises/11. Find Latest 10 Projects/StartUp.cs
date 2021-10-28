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

            string result = GetLatestProjects(db);

            Console.WriteLine(result);
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var lastTenStartedProjects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .Select(p => new
                {
                    ProjectName = p.Name,
                    Description = p.Description,
                    StartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                })

                .OrderBy(p => p.ProjectName)
                .ToList();

            foreach (var project in lastTenStartedProjects)
            {
                sb.AppendLine($"{project.ProjectName}" +
                              $"{Environment.NewLine}" +
                              $"{project.Description}" +
                              $"{Environment.NewLine}" +
                              $"{project.StartDate}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
