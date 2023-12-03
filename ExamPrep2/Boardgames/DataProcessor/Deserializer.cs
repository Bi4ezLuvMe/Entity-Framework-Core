namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Reflection.Metadata.Ecma335;
    using System.Text;
    using System.Xml.Serialization;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ImportDto;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
           XmlSerializer serializer = new XmlSerializer(typeof(List<ImportCreatorsDTO>), new XmlRootAttribute("Creators"));
            List<Creator>validCreators = new List<Creator>();
            List<ImportCreatorsDTO>creatorsDTOs = new List< ImportCreatorsDTO >();
            using(StringReader reader = new StringReader(xmlString))
            {
                creatorsDTOs = (List<ImportCreatorsDTO>)serializer.Deserialize(reader);
            }
            foreach (var creator in creatorsDTOs)
            {
                if (!IsValid(creator))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Creator c = new Creator()
                {
                    FirstName = creator.FirstName,
                    LastName = creator.LastName,
                };
                foreach (var boardgame in creator.BoardGames)
                {
                    if (!IsValid(boardgame))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Boardgame bg = new Boardgame()
                    {
                        Name = boardgame.Name,
                        Rating = boardgame.Rating,
                        YearPublished = boardgame.YearPublished,
                        CategoryType = (CategoryType)boardgame.CategoryType,
                        Mechanics = boardgame.Mechanics
                    };
                    c.Boardgames.Add(bg);
                }
                sb.AppendLine($"Successfully imported creator – {c.FirstName} {c.LastName} with {c.Boardgames.Count()} boardgames.");
                validCreators.Add(c);
            }
            context.Creators.AddRange(validCreators);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportSellersDTO[] sellerDtos = JsonConvert.DeserializeObject<ImportSellersDTO[]>(jsonString);

            List<Seller> sellers = new List<Seller>();

            foreach (ImportSellersDTO sellerDto in sellerDtos)
            {
                if (!IsValid(sellerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Seller s = new Seller()
                {
                    Name = sellerDto.Name,
                    Address = sellerDto.Address,
                    Country = sellerDto.Country,
                    Website = sellerDto.Website,
                };

                foreach (int boardgameId in sellerDto.BoardGames.Distinct())
                {
                    Boardgame b = context.Boardgames.Find(boardgameId);
                    if (b == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    s.BoardgamesSellers.Add(new BoardgameSeller()
                    {
                        Boardgame = b
                    });
                }
                sellers.Add(s);
                sb.AppendLine(String.Format(SuccessfullyImportedSeller, s.Name, s.BoardgamesSellers.Count));
            }
            context.Sellers.AddRange(sellers);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
