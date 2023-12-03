namespace Boardgames.DataProcessor
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            return "Kur";
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellers = context.Sellers
                .Where(x=>x.BoardgamesSellers.Any(x=>x.Boardgame.YearPublished>=year&&x.Boardgame.Rating<=rating))
                .ToArray()
                .Select(x => new
                {
                    Name = x.Name,
                    Website = x.Website,
                    Boardgames = x.BoardgamesSellers.Where(x => x.Boardgame.YearPublished >= year && x.Boardgame.Rating <= rating).OrderByDescending(x=>x.Boardgame.Rating).ThenBy(x=>x.Boardgame.Name).Select(x => new
                    {
                        Name = x.Boardgame.Name,
                        Rating = x.Boardgame.Rating,
                        Mechanics = x.Boardgame.Mechanics,
                        Category = x.Boardgame.CategoryType.ToString()
                    })
                })
                .ToArray()
                .OrderByDescending(x=>x.Boardgames.Count())
                .ThenBy(x=>x.Name)
                .Take(5)
                .ToArray();
            return JsonConvert.SerializeObject(sellers, Formatting.Indented);
        }
    }
}