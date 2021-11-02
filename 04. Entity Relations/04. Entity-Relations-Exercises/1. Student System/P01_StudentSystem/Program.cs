using System;
using P01_StudentSystem.Data;

namespace P01_StudentSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            var contex = new StudentSystemContext();
            contex.Database.EnsureDeleted();
            contex.Database.EnsureCreated();
        }
    }
}
