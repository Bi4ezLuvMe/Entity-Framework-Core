using Microsoft.Data.SqlClient;
using System.Security.Cryptography;

using SqlConnection con = new SqlConnection(@"Server=DESKTOP-745T20N\SQLEXPRESS;Database=MinionsDB;Integrated Security=true;TrustServerCertificate=True;");
con.Open();
int[]ids = Console.ReadLine().Split().Select(int.Parse).ToArray();
foreach (var item in ids)
{
    SqlCommand idsForChange = new("UPDATE minions SET Age = Age + 1, Name = LOWER(LEFT(Name, 1)) + RIGHT(Name, LEN(Name) - 1) WHERE Id = @id", con);
    idsForChange.Parameters.AddWithValue("@id", item);
    using SqlDataReader reader = idsForChange.ExecuteReader();
}
SqlCommand output = new("SELECT Name,Age FROM Minions", con);
using (SqlDataReader reader = output.ExecuteReader())
{
    while (reader.Read())
    {
        Console.WriteLine($"{reader[0]} {reader[1]}");
    }
}