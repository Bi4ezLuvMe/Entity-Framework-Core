using Microsoft.Data.SqlClient;

using SqlConnection con = new SqlConnection(@"Server=DESKTOP-745T20N\SQLEXPRESS;Database=MinionsDB;Integrated Security=true;TrustServerCertificate=True;");

string[] inputMinion = Console.ReadLine().Split(" ");
string[] inputVillain = Console.ReadLine().Split(" ");
string minionName = inputMinion[1];
int minionAge = int.Parse(inputMinion[2]);
string minionTown = inputMinion[3];
string villainName = inputVillain[1];
int minionTownId = 0;

con.Open();
//Town

string selectTown = " select name" +
    " from Towns" +
    " where name = @name";
SqlCommand cmdTown = new(selectTown, con);
cmdTown.Parameters.AddWithValue("@name", minionTown);
using (SqlDataReader townReader = cmdTown.ExecuteReader())
{
    if (!townReader.Read())
    {
        string insertTown = "Insert into Towns(Name)VALUES" +
            " (@name)";
        SqlCommand cmdInsertTown = new(insertTown, con);
        cmdInsertTown.Parameters.AddWithValue("@name", minionTown);
        townReader.Close();
        using (SqlDataReader execInsertTownCmd = cmdInsertTown.ExecuteReader())
        {
            Console.WriteLine($"Town {minionTown} was added to the database.");
        }
    }
}
string selectTownId= "select Id " +
    "from Towns " +
    "where Name = @name";
SqlCommand cmdSelectTownId = new(selectTownId, con);
cmdSelectTownId.Parameters.AddWithValue("@name", minionTown);
using (SqlDataReader selectTownReader = cmdSelectTownId.ExecuteReader())
{
    if (!selectTownReader.Read())
    {
        Console.WriteLine("Kur");
    }
    minionTownId = int.Parse(selectTownReader[0].ToString());
}

//Villain

string selectVillain = " select name" +
               " from Villains" +
               " where name = @name";
SqlCommand cmdVillain = new(selectVillain, con);
cmdVillain.Parameters.AddWithValue("@name", villainName);
using (SqlDataReader villainReader = cmdVillain.ExecuteReader())
{
    if (!villainReader.Read())
    {
        string insertVillain = "Insert into Villains(Name,EvilnessFactorId) values" +
            "(@name , 4)";
        SqlCommand cmdInsertVillain = new(insertVillain, con);
        cmdInsertVillain.Parameters.AddWithValue("@name", villainName);
        villainReader.Close();
        using (SqlDataReader execInsertVillainCmd = cmdInsertVillain.ExecuteReader())
        {
            Console.WriteLine($"Villain {villainName} was added to the database.");
        }
    }
}

//Minion

string selectMinion = " select Name" +
               " from Minions" +
               " where Name = @name";
SqlCommand cmdMinion = new(selectMinion, con);
cmdMinion.Parameters.AddWithValue("@name", minionName);
using (SqlDataReader minionReader = cmdMinion.ExecuteReader())
{
    if (!minionReader.Read())
    {
        string insertMinion = "Insert into Minions(Name,Age,TownId) VALUES " +
            "(@name,@age,@townId)";
        SqlCommand cmdInsertMinion = new(insertMinion, con);
        cmdInsertMinion.Parameters.AddWithValue("@age", minionAge);
        cmdInsertMinion.Parameters.AddWithValue("@name", minionName);
        cmdInsertMinion.Parameters.AddWithValue("@townId", minionTownId);
        using (SqlDataReader execInsertMinionCmd = cmdInsertMinion.ExecuteReader())
        {
            Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
        }
    }
}