using Microsoft.Data.SqlClient;
namespace _19._10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using SqlConnection dbCon = new SqlConnection(@"Server=DESKTOP-745T20N\SQLEXPRESS;Database=MinionsDB;Integrated Security=true;TrustServerCertificate=True; User Id = ; Password = ;");
            dbCon.Open();
            string query = "select v.Name, Count(mv.MinionId)\r\nfrom MinionsVillains as mv join Villains as v on mv.VillainId = v.Id join Minions as m on mv.MinionId = m.Id\r\ngroup by v.Name\r\nhaving Count(mv.MinionId) >3\r\norder by Count(mv.MinionId) desc";
            SqlCommand cmd = new(query, dbCon);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"{reader[0]} - {reader[1]}");
            }
            
        }
    }
}
