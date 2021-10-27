using System;
using Microsoft.Data.SqlClient;
using MyClasses;

namespace CSharpDbDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            ////1.ExecuteNonQuery()
            //using (var connection = new SqlConnection(
            //    "Server=.; Integrated Security=true; Database=SoftUni"))
            //{
            //    connection.Open();
            //    SqlCommand sqlCommand = new SqlCommand(
            //       "UPDATE Employees SET Salary = Salary + 0.12 WHERE FirstName LIKE 'N%'",
            //    connection);
            //    var rowsAffected = sqlCommand.ExecuteNonQuery(); 
            //    Console.WriteLine(rowsAffected);

            ////2.ExecuteScalar()
            //using (var connection = new SqlConnection(
            //    "Server=.; Integrated Security=true; Database=SoftUni"))
            //{
            //    connection.Open();
            //    SqlCommand sqlCommand = new SqlCommand(
            //       // "SELECT COUNT (*) FROM Employees",
            //       "SELECT SUM(Salary)/12.0 FROM Employees",
            //        connection);

            //    //var rowsAffected = (int)sqlCommand.ExecuteScalar();
            //    var rowsAffected = (decimal)sqlCommand.ExecuteScalar();
            //    Console.WriteLine(rowsAffected);

            //3. ExecuteReader()

            //using (var connection = new SqlConnection(
            //"Server=.; Integrated Security=true; Database=SoftUni"))
            //{
            //    connection.Open();
            //    SqlCommand sqlCommand = new SqlCommand(
            //        "SELECT d.Name, COUNT(*) AS Count " +
            //    "FROM Employees e " +
            //    "JOIN Departments d ON e.DepartmentID = d.DepartmentID " +
            //    "GROUP BY d.Name ",
            //     connection);

            //    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
            //    {
            //        while (sqlDataReader.Read())
            //        {
            //            var data = sqlDataReader["Name"] as string;
            //            Console.WriteLine($"{sqlDataReader["Name"]} => {sqlDataReader["Count"]}");
            //        }
            //    }
            //}

            //Injection

            Console.Write("Please enter username: "); // ' OR -- ; awight1' --
            var username = Console.ReadLine();
            Console.Write("Please enter password: ");
            var password = Console.ReadLine();

            using (var connection = new SqlConnection(
            "Server=.; Integrated Security=true; Database=Service"))
            {
                connection.Open();

                SqlCommand sqlCommand = new SqlCommand(
                    $"SELECT COUNT (*) FROM Users WHERE Username = '@Username' AND Password =  '@Password';", connection);
                sqlCommand.Parameters.Add(new SqlParameter("@Username", username));
                sqlCommand.Parameters.Add(new SqlParameter("@Password", password));
                var usersCount = (int)sqlCommand.ExecuteScalar();
                if (usersCount > 0)
                {
                    Console.WriteLine("Access granted! Welcome!");
                }
                else
                {
                    Console.WriteLine("Access denied. :(");
                }
            }
        }
    }
}
