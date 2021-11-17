using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var contex = new CarDealerContext();
            contex.Database.EnsureDeleted();
            contex.Database.EnsureCreated();

            var json = File.ReadAllText("../../../Datasets/suppliers.json");
            var result = ImportSuppliers(contex, json);

            Console.WriteLine(result);
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            //suppliers.json
            var supplierDtos = JsonConvert.DeserializeObject<IEnumerable<ImportSupllierInputDto>>(inputJson);
            var suppliers = supplierDtos.Select(x => new Supplier
            {
                Name = x.Name,
                IsImporter = x.IsImporter
            })
                .ToList();
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}.";
        }
    }
}