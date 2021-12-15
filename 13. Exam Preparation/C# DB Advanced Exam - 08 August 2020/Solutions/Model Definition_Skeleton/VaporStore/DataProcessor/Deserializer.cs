using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using VaporStore.Data.Models;
using VaporStore.DataProcessor.Dto.Import;

namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Data;

    public static class Deserializer
    {
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var output = new StringBuilder();
            var games = JsonConvert.DeserializeObject<IEnumerable<GameImportJsonModel>>(jsonString);

            foreach (var jsonGame in games)
            {
                if (!IsValid(jsonGame) || jsonGame.Tags.Count() == 0) //!jsonGame.Tags.Any()
                {
                    output.AppendLine("Invalid Data");
                    continue;
                }

                var genre = context.Genres.FirstOrDefault(x => x.Name == jsonGame.Genre)
                ?? new Genre { Name = jsonGame.Genre };

                //var genre = context.Genres.FirstOrDefault(x => x.Name == jsonGame.Genre);
                //if (genre == null)
                //{
                //    genre = new Genre { Name = jsonGame.Genre };
                //}

                var developer = context.Developers.FirstOrDefault(x => x.Name == jsonGame.Developer)
                                ?? new Developer { Name = jsonGame.Developer };
                var game = new Game
                {
                    Name = jsonGame.Name,
                    Genre = genre,
                    Developer = developer,
                    Price = jsonGame.Price,
                    ReleaseDate = jsonGame.ReleaseDate.Value,
                };

                foreach (var jsonTag in jsonGame.Tags)
                {
                    var tag = context.Tags.FirstOrDefault(x => x.Name == jsonTag)
                        ?? new Tag { Name = jsonTag };
                    game.GameTags.Add(new GameTag { Tag = tag });
                }
                context.Games.Add(game);
                context.SaveChanges();
                output.AppendLine($"Added {jsonGame.Name} ({jsonGame.Genre}) with {jsonGame.Tags.Count()} tags");
            }

            return output.ToString();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            var output = new StringBuilder();
            var users = JsonConvert.DeserializeObject<IEnumerable<UserJsonInputModel>>(jsonString);

            foreach (var jsonUser in users)
            {
                if (!IsValid(jsonUser) || !jsonUser.Cards.All(IsValid))
                {
                    output.AppendLine("Invalid Data");
                    continue;
                }

                //foreach (var card in jsonUser.Cards)
                //{
                //    if (!IsValid(card))
                //    {
                //        output.AppendLine("Invalid Data");
                //        continue;
                //    }
                //}

                var user = new User
                {
                    Age = jsonUser.Age,
                    Email = jsonUser.Email,
                    FullName = jsonUser.FullName,
                    Username = jsonUser.Username,
                    Cards = jsonUser.Cards.Select(x => new Card
                    {
                        Cvc = x.CVC,
                        Number = x.Number,
                        Type = x.Type.Value
                    })
                        .ToList()
                };

                context.Users.Add(user);

                output.AppendLine($"Imported {jsonUser.Username} with {jsonUser.Cards.Count()} cards");
            }
            context.SaveChanges();
            return output.ToString();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            var output = new StringBuilder();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PurchaseXmlInputModel[]), new XmlRootAttribute("Purchases"));

            var purchases = (PurchaseXmlInputModel[])xmlSerializer.Deserialize(new StringReader(xmlString));

            foreach (var xmlPurchase in purchases)
            {
                if (!IsValid(xmlPurchase))
                {
                    output.AppendLine("Invalid Data");
                    continue;
                }

                bool parseDate = DateTime.TryParseExact(
                    xmlPurchase.Date, "dd/MM/yyyy HH:mm",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var date);

                if (!parseDate)
                {
                    output.AppendLine("Invalid Data");
                    continue;
                }

                var purchase = new Purchase
                {
                    Date = date,
                    Type = xmlPurchase.Type.Value,
                    ProductKey = xmlPurchase.Key,
                    Card = context.Cards.FirstOrDefault(x => x.Number == xmlPurchase.Card),
                    Game = context.Games.FirstOrDefault(x => x.Name == xmlPurchase.GameName)
                };
               
                context.Purchases.Add(purchase);

                var username = 
                    context
                        .Users
                        .Where(x => x.Id == purchase.Card.UserId)
                        .Select(x=>x.Username).FirstOrDefault();

                output.AppendLine($"Imported {xmlPurchase.GameName} for {username}");
            }

            context.SaveChanges();
            return output.ToString();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}