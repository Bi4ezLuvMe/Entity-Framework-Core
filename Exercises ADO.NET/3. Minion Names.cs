using System;
using System.Data.SqlClient;
 
namespace _01.InitialSetup
{
    class GetMinionNames
    {
        static void Main()
        {
            int input = int.Parse(Console.ReadLine());
 
            string connectionString = "Server=.; Database=MinionsDB; Trusted_Connection=True;";
            SqlConnection connection = new SqlConnection(connectionString);
 
            GetVillainName(input, connection);
            GetMinions(input, connection);
        }
 
        static void GetMinions(int vilainID, SqlConnection connection)
        {
            connection.Open();
 
            string selectionCommand = "SELECT m.Name, " +
                                        "m.Age " +
                                        "FROM [dbo].[Minions] AS m " +
                                        "INNER JOIN [dbo].[MinionsVillainsConnections] AS mv " +
                                        "ON mv.MinionID = m.MinionID " +
                                        "INNER JOIN [dbo].[Villains] AS v " +
                                        "ON v.VillainID = mv.VillainID " +
                                        "WHERE v.VillainID = @villainID";
 
            SqlCommand command = new SqlCommand(selectionCommand, connection);
            command.Parameters.AddWithValue("@villainID", vilainID);
 
            SqlDataReader reader = command.ExecuteReader();
 
            using (reader)
            {
 
                while (reader.Read())
                {
                    if (reader.FieldCount == 0)
                    {
                        Console.WriteLine("<no minions>");
                    }
 
                    else
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write("{0} ", reader[i]);
                        }
                        Console.WriteLine();
                    }
                }
            }
 
            connection.Close();
        }
 
        static void GetVillainName(int villainID, SqlConnection connection)
        {
            connection.Open();
 
            string selectionCommand = "SELECT v.Name " +
                                           "FROM [dbo].[Villains] AS v " +
                                           "WHERE v.VillainID = @villainID";
 
            SqlCommand command = new SqlCommand(selectionCommand, connection);
            command.Parameters.AddWithValue("@villainID", villainID);
 
            SqlDataReader reader = command.ExecuteReader();
 
            using (reader)
            {
                if (!reader.Read())
                {
                    Console.WriteLine("No villain with ID {0} exists in the database.", villainID);
                }
 
                else
                {
                    Console.WriteLine("Villain: {0}", reader[0]);                  
                }
            }
              
            connection.Close();
        }
    }
}