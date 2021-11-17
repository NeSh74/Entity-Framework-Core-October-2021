using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var contex = new CarDealerContext();
            //contex.Database.EnsureDeleted();
            //contex.Database.EnsureCreated();

            //var suppliersJson = File.ReadAllText("../../../Datasets/suppliers.json");
            //var partsJson = File.ReadAllText("../../../Datasets/parts.json");
            //var carsJson = File.ReadAllText("../../../Datasets/cars.json");
            //var customersJson = File.ReadAllText("../../../Datasets/customers.json");
            //var salesJson = File.ReadAllText("../../../Datasets/sales.json");
            //ImportSuppliers(contex, suppliersJson);
            //ImportParts(contex, partsJson);
            //ImportCars(contex, carsJson);
            //ImportCustomers(contex, customersJson);
            //ImportSales(contex, salesJson);
            //GetOrderedCustomers(contex);
            //GetCarsFromMakeToyota(contex);
            //GetLocalSuppliers(contex);
            //GetCarsWithTheirListOfParts(contex);
            //GetCarsWithTheirListOfParts(contex);
            var result = GetSalesWithAppliedDiscount(contex);

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

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var suppliedIds = context
                .Suppliers.Select(x => x.Id)
                .ToArray();

            var parts = JsonConvert
                .DeserializeObject<IEnumerable<Part>>(inputJson)
                .Where(s => suppliedIds.Contains(s.SupplierId))
                .ToList();

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count()}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carsDto = JsonConvert
                .DeserializeObject<IEnumerable<CarInputModel>>(inputJson);

            //var cars = carsDto.Select(c => new Car
            //{
            //    Make = c.Make,
            //    Model = c.Model,
            //    TravelledDistance = c.TravelledDistance
            //});

            var listOfCars = new List<Car>();

            foreach (var car in carsDto)
            {
                var currentCar = new Car
                {
                    Make = car.Make,
                    Model = car.Model,
                    TravelledDistance = car.TravelledDistance
                };
                foreach (var partId in car?.PartsId.Distinct())
                {
                    currentCar.PartCars.Add(new PartCar
                    {
                        PartId = partId
                    });
                }
                listOfCars.Add(currentCar);
            }

            context.Cars.AddRange(listOfCars);
            context.SaveChanges();

            return $"Successfully imported {listOfCars.Count}.";
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            //var customers = JsonConvert.DeserializeObject<Customer[]>(inputJson);

            //context.Customers.AddRange(customers);
            //context.SaveChanges();

            //return $"Successfully imported {customers.Length}.";

            var customersDto = JsonConvert.DeserializeObject<IEnumerable<ImportCustomerInputModel>>(inputJson);
            
            var customers = customersDto.Select(x => new Customer()
            {
                Name = x.Name,
                BirthDate = x.BirthDate,
                IsYoungDriver = x.IsYoungDriver
            }).ToList();

            context.AddRange(customers);
            context.SaveChanges();
            var result = $"Successfully imported {customers.Count()}.";
            return result;
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            //var salesDto = JsonConvert.DeserializeObject<IEnumerable<ImportSalesInputModel>>(inputJson);
            //var sales = salesDto.Select(x => new Sale()
            //{
            //    Discount = x.Discount,
            //    CustomerId = x.CustomerId,
            //    CarId = x.CarId
            //}).ToList();

            //context.AddRange(sales);
            //context.SaveChanges();
            //var result = $"Successfully imported {sales.Count()}.";
            //return result;

            var sales = JsonConvert.DeserializeObject<Sale[]>(inputJson);

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Length}.";
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context
                .Customers
                .OrderBy(x => x.BirthDate)
                .ThenBy(x => x.IsYoungDriver)
                .Select(x => new
                {
                    x.Name,
                    BirthDate = x.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    x.IsYoungDriver
                })
                .ToList();

            var json = JsonConvert.SerializeObject(customers, Formatting.Indented);
            return json;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context
                .Cars
                .Where(x => x.Make == "Toyota")
                .Select(x => new
                {
                    x.Id,
                    x.Make,
                    x.Model,
                    x.TravelledDistance
                })
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .ToList();

            var json = JsonConvert.SerializeObject(cars, Formatting.Indented);
            return json;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var supplier = context
                .Suppliers
                .Where(x => x.IsImporter == false)//.Where(s => !s.IsImporter)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    PartsCount = x.Parts.Count(p => p.SupplierId == x.Id)
                })
                .ToList();

            var json = JsonConvert.SerializeObject(supplier, Formatting.Indented);
            return json;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context
                .Cars
                .Select(x => new
                {
                    car = new
                    {
                        x.Make,
                        x.Model,
                        x.TravelledDistance,
                    },
                    parts = x.PartCars
                        .Where(p => p.CarId == x.Id)
                        .Select(p => new
                        {
                            p.Part.Name,
                            Price = p.Part.Price.ToString("F2")
                        }).ToList()
                })
                .ToList();

            var jsonOutput = JsonConvert.SerializeObject(cars, Formatting.Indented);
            return jsonOutput;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var sales = context
                .Sales
                .Select(x => new
                {
                    x.CustomerId,
                    SpentMoney = x.Car.PartCars.Sum(z => z.Part.Price)
                });

            var customers = context
                .Customers
                .Where(c => sales.Count(s => s.CustomerId == c.Id) > 0)
                .Select(x => new
                {
                    FullName = x.Name,
                    BoughtCars = sales.Count(s => s.CustomerId == x.Id),
                    SpentMoney = sales.Where(z => z.CustomerId == x.Id).Sum(s => s.SpentMoney)
                })
                .OrderByDescending(x => x.SpentMoney)
                .ThenByDescending(x => x.BoughtCars)
                .ToList();

            var formatting = JsonSerializerSettings();
            var json = JsonConvert.SerializeObject(customers, formatting);

            return json;
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context
                .Sales
                .Include(x => x.Car)
                .Select(x => new
                {
                    car = new
                    {
                        x.Car.Make,
                        x.Car.Model,
                        x.Car.TravelledDistance
                    },
                    customerName = x.Customer.Name,
                    Discount = x.Discount.ToString("F2"),
                    price = x.Car.PartCars.Sum(c => c.Part.Price).ToString("F2"),
                    priceWithDiscount = (x.Car.PartCars.Sum(c => c.Part.Price) - (x.Car.PartCars.Sum(c => c.Part.Price) * (x.Discount) / 100.0m)).ToString("F2"),
                })
                .Take(10)
                .ToList();

            var jsonOutput = JsonConvert.SerializeObject(sales, Formatting.Indented);
            return jsonOutput;
        }

        private static JsonSerializerSettings JsonSerializerSettings()
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver();
            contractResolver.NamingStrategy = new CamelCaseNamingStrategy();
            var formatting = new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented,
            };
            return formatting;
        }
    }
}