using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Dtos.Input;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        private static IMapper mapper;

        public static void Main(string[] args)
        {
            ProductShopContext contex = new ProductShopContext();
            contex.Database.EnsureDeleted();
            contex.Database.EnsureCreated();

            string usersJasonString = File.ReadAllText("../../../Datasets/users.json");
            string productsJasonString = File.ReadAllText("../../../Datasets/products.json");

            Console.WriteLine(ImportUsers(contex, usersJasonString));
            Console.WriteLine(ImportProducts(contex, productsJasonString));
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            IEnumerable<UserInputDto> users = JsonConvert.DeserializeObject<IEnumerable<UserInputDto>>(inputJson);

            InitializeMapper();

            IEnumerable<User> mappedUsers = mapper.Map<IEnumerable<User>>(users);
            //IEnumerable<User> mappedUsers = users
            //    .Select(x => x.MapToDomainUser())
            //    .ToList();
            context.Users.AddRange(mappedUsers);
            context.SaveChanges();

            return $"Successfully imported {mappedUsers.Count()}";
        }


        //public static class UserMappings
        //{
        //    //public static User MapToDomainUser(this UserInputDto userDto)
        //    //{
        //    //    return new User
        //    //    {
        //    //        Age = userDto.Age,
        //    //        FirstName = userDto.FirstName,
        //    //        LastName = userDto.LastName
        //    //    };
        //    //}
        //}

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            IEnumerable<ProductInputDto> products = JsonConvert.DeserializeObject<IEnumerable<ProductInputDto>>(inputJson);

            InitializeMapper();

            var mappedProducts = mapper.Map<IEnumerable<Product>>(products);

            context.Products.AddRange(mappedProducts);
            context.SaveChanges();

            return $"Successfully imported {mappedProducts.Count()}";
        }

        private static void InitializeMapper()
        {
            var mapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile<ProductShopProfile>(); });

            mapper = new Mapper(mapperConfiguration);
        }
    }
}