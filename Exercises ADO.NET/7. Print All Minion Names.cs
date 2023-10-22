using Microsoft.Data.SqlClient;
using System.Security.Cryptography;

using SqlConnection con = new SqlConnection(@"Server=DESKTOP-745T20N\SQLEXPRESS;Database=MinionsDB;Integrated Security=true;TrustServerCertificate=True;");
con.Open();
SqlCommand getAllMinionNames = new("SELECT Name FROM Minions", con);
List<string> minions = new List<string>();
using (SqlDataReader reader = getAllMinionNames.ExecuteReader())
{
    while (reader.Read())
    {
        minions.Add(reader.GetString(0));
    }
}

for (int i = 0; i < minions.Count/2; i++)
{
        Console.WriteLine(minions.ElementAt(i));
        Console.WriteLine(minions.ElementAt(minions.Count-1-i));
}
if (minions.Count % 2 != 0)
{
    Console.WriteLine(minions.ElementAt(minions.Count/2));
}