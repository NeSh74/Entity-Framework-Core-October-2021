using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
            //contex.Database.EnsureDeleted();
            //contex.Database.EnsureCreated();

            //string usersJasonString = File.ReadAllText("../../../Datasets/users.json");
            //string productsJasonString = File.ReadAllText("../../../Datasets/products.json");
            //string categoriesJasonString = File.ReadAllText("../../../Datasets/categories.json");
            //string categoryProductsJasonString = File.ReadAllText("../../../Datasets/categories-products.json");

            //Console.WriteLine(ImportUsers(contex, usersJasonString));
            //Console.WriteLine(ImportProducts(contex, productsJasonString));
            //Console.WriteLine(ImportCategories(contex, categoriesJasonString));
            //Console.WriteLine(ImportCategoryProducts(contex, categoryProductsJasonString));

            Console.WriteLine(GetProductsInRange(contex));
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

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            IEnumerable<CategoryInputDto> categories = JsonConvert.DeserializeObject<IEnumerable<CategoryInputDto>>(inputJson)
                .Where(x => !string.IsNullOrEmpty(x.Name));

            InitializeMapper();

            var mappedCategories = mapper.Map<IEnumerable<Category>>(categories);

            context.Categories.AddRange(mappedCategories);
            context.SaveChanges();

            return $"Successfully imported {mappedCategories.Count()}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            IEnumerable<CategoryProductInputDto> categoryProducts = JsonConvert.DeserializeObject<IEnumerable<CategoryProductInputDto>>(inputJson);

            InitializeMapper();

            var mappedCategoryProducts = mapper.Map<IEnumerable<CategoryProduct>>(categoryProducts);

            context.CategoryProducts.AddRange(mappedCategoryProducts);
            context.SaveChanges();

            return $"Successfully imported {mappedCategoryProducts.Count()}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .OrderBy(x => x.Price)
                .Select(x => new
                {
                    Name = x.Name,
                    Price = x.Price,
                    Seller = $"{x.Seller.FirstName} {x.Seller.LastName}"
                })
                .ToList();

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = contractResolver
            };

            string productsAsJson = JsonConvert.SerializeObject(products, jsonSettings);

            return productsAsJson;
        }

        private static void InitializeMapper()
        {
            var mapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile<ProductShopProfile>(); });

            mapper = new Mapper(mapperConfiguration);
        }
    }
}