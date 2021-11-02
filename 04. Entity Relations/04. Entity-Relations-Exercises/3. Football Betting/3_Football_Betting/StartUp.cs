using Microsoft.EntityFrameworkCore;
using P03_FootballBetting.Data;

namespace P03_FootballBetting
{
    using System;

    class StartUp
    {
        static void Main(string[] args)
        {
            //FootballBettingContext dbContext = new FootballBettingContext();

            //dbContext.Database.EnsureCreated();

            //Console.WriteLine("Db creates successfully!");
            //Console.WriteLine("Do you want to delete the database(Y/N)?");
            //string result = Console.ReadLine();
            //if (result == "Y")
            //{
            //    dbContext.Database.EnsureDeleted();
            //}

            FootballBettingContext db = new FootballBettingContext();

            db.Database.Migrate();
            Console.WriteLine("Db creates successfully!");
        }
    }
}
