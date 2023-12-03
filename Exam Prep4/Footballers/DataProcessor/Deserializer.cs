namespace Footballers.DataProcessor
{
    using Footballers.Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
           XmlSerializer serializer = new XmlSerializer(typeof(List<ImportCoachesDTO>),new XmlRootAttribute("Coaches"));
            StringBuilder sb = new();
            List<ImportCoachesDTO> coachesDTOs = new List<ImportCoachesDTO>();
            List<Coach>coaches = new List<Coach>();
            int count = 0;
            using (StringReader reader = new(xmlString))
            {
                coachesDTOs = (List<ImportCoachesDTO>)serializer.Deserialize(reader);
            }
            foreach (var coach in coachesDTOs)
            {
                if (!IsValid(coach))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Coach c = new()
                {
                    Name = coach.Name,
                    Nationality = coach.Nationality
                };
                foreach (var footballer in coach.Footballers)
                {
                    if (!IsValid(footballer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Footballer f = new()
                    {
                        Name = footballer.Name,
                        ContractStartDate = footballer.ContractStartDate,
                        ContractEndDate = footballer.ContractEndDate,
                        BestSkillType = (BestSkillType)footballer.BestSkillType,
                        PositionType = (PositionType)footballer.PositionType
                    };
                    c.Footballers.Add(f);
                    count++;
                }
                sb.AppendLine($"Successfully imported coach – {c.Name} with {c.Footballers.Count} footballers.");
                coaches.Add(c);
            }
            context.Coaches.AddRange(coaches);
            //context.SaveChanges();
            //return sb.ToString().TrimEnd();
            return $"{coaches.Count} coaches with {count} footballers";
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            throw new NotImplementedException();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
