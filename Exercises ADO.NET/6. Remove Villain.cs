using Microsoft.Data.SqlClient;

using SqlConnection con = new SqlConnection(@"Server=DESKTOP-745T20N\SQLEXPRESS;Database=MinionsDB;Integrated Security=true;TrustServerCertificate=True;");
con.Open();
int villainId = int.Parse(Console.ReadLine());
string villainName = String.Empty;
SqlCommand getVillainNameCmd = new("SELECT Name FROM Villains WHERE Id = @id", con);
getVillainNameCmd.Parameters.AddWithValue("@id", villainId);
using (SqlDataReader reader = getVillainNameCmd.ExecuteReader())
{
    if (!reader.Read())
    {
        Console.WriteLine("No such villain was found.");
        return;
    }
        villainName = reader.GetString(0);
}

SqlCommand numOfMinions = new("SELECT COUNT(MinionId) FROM MinionsVillains AS MV JOIN Villains AS V ON MV.VillainId = V.Id JOIN Minions AS M ON MV.MinionId = M.Id WHERE MV.VillainId = @vilionid", con);
numOfMinions.Parameters.AddWithValue("@vilionid", villainId);

using (SqlDataReader reader = numOfMinions.ExecuteReader())
{
    if (reader.Read())
    {
        Console.WriteLine($"{reader.GetInt32(0)} minions were released.");
    }
}

SqlCommand freeMinions = new("DELETE FROM MinionsVillains WHERE VillainId = @id", con);
freeMinions.Parameters.AddWithValue("@id", villainId);
using (SqlDataReader reader = freeMinions.ExecuteReader())
{
    if (reader.Read());
}

SqlCommand deleteVillain = new("DELETE FROM Villains WHERE Id = @id", con);
deleteVillain.Parameters.AddWithValue("@id", villainId);

using (SqlDataReader reader = deleteVillain.ExecuteReader())
{
    if (reader.Read())
    {
        Console.WriteLine($"{villainName} was deleted.");
    }
}