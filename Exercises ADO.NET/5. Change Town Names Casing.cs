using Microsoft.Data.SqlClient;

using SqlConnection con = new SqlConnection(@"Server=DESKTOP-745T20N\SQLEXPRESS;Database=MinionsDB;Integrated Security=true;TrustServerCertificate=True;");

con.Open();

string country = Console.ReadLine();

SqlCommand getCountryIdCmd = new("select Id FROM Countries WHERE Name = @name", con);

int countryId = 0;

getCountryIdCmd.Parameters.AddWithValue("@name", country);

using (SqlDataReader getCountryIdReader = getCountryIdCmd.ExecuteReader())
{

    if (getCountryIdReader.Read())
    {

        countryId = getCountryIdReader.GetInt32(0); 

    }

}

SqlCommand updateCountryNames = new("UPDATE Towns SET Name = UPPER(Name) WHERE CountryCode = @countrycode", con);

updateCountryNames.Parameters.AddWithValue("@countrycode", countryId);

using (SqlDataReader updateCounrtiesReader = updateCountryNames.ExecuteReader())
{

}

SqlCommand selectTheNewCounrtyNames = new("SELECT Name FROM Towns WHERE CountryCode = @countrycode",con);

selectTheNewCounrtyNames.Parameters.AddWithValue("@countrycode", countryId);

List<string> countryNames = new();
using (SqlDataReader readTheNewNames = selectTheNewCounrtyNames.ExecuteReader())
{

    while (readTheNewNames.Read())
    {

        countryNames.Add(readTheNewNames[0].ToString());

    }

}
if(countryNames.Count == 0) 
{
    Console.WriteLine("No town names were affected.");
}
else
{
    Console.WriteLine($"{countryNames.Count} town names were affected.");
    Console.WriteLine("[" + String.Join(", ", countryNames) + "]");
}
