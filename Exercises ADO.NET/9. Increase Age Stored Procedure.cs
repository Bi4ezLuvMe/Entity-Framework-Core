using Microsoft.Data.SqlClient;
using System.Security.Cryptography;

using SqlConnection con = new SqlConnection(@"Server=DESKTOP-745T20N\SQLEXPRESS;Database=MinionsDB;Integrated Security=true;TrustServerCertificate=True;");
con.Open();
int id = int.Parse(Console.ReadLine());
SqlCommand getOlder = new("CREATE PROCEDURE usp_GetOlder(@Id INT) AS BEGIN UPDATE Minions SET Age = Age+1 WHERE Id = @Id END EXEC usp_GetOlder @Id = @id", con);
getOlder.Parameters.AddWithValue("@id", id);
using (SqlDataReader reader = getOlder.ExecuteReader())
{
    if (reader.Read()) { }
}
SqlCommand getMinion = new("SELECT Name,Age FROM Minions WHERE Id = @Id", con);
getMinion.Parameters.AddWithValue("@Id", id);
using (SqlDataReader reader = getMinion.ExecuteReader())
{
    if (reader.Read())
    {
        Console.WriteLine($"{reader.GetString(0)} - {reader.GetInt32(1)} years old");
    }
}