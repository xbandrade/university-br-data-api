using System;
using System.Net.Http;
using System.Threading.Tasks;
using MySqlConnector;


namespace UniversityBRDataAPI;
public class DataPopulator
{
    private readonly string APIUrl;
    private readonly string connectionString;

    public DataPopulator(string apiUrl, string connectionString)
    {
        this.APIUrl = apiUrl;
        this.connectionString = connectionString;
    }

  public async Task PopulateDatabase()
    {
        using MySqlConnection connection = new(connectionString);
        try
        {
            Console.WriteLine("Populating DB");
            string createDatabaseQuery = "CREATE DATABASE IF NOT EXISTS BrUniversityAPI";
            connection.Open();
            using MySqlCommand createDatabaseCommand = new(createDatabaseQuery, connection);
            createDatabaseCommand.ExecuteNonQuery();
            string useDatabaseQuery = "USE BrUniversityAPI";
            using MySqlCommand useDatabaseCommand = new(useDatabaseQuery, connection);
            useDatabaseCommand.ExecuteNonQuery();

            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS UniversityData (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    Name VARCHAR(255) UNIQUE,
                    State VARCHAR(255),
                    WebPages JSON,
                    Domains JSON
                )";
            using MySqlCommand createTableCommand = new(createTableQuery, connection);
            createTableCommand.ExecuteNonQuery();

            using HttpClient client = new();
            HttpResponseMessage response = await client.GetAsync(APIUrl);
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var parsedData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BrUniversity>>(data);

                if (parsedData != null && parsedData.Any())
                {
                    foreach (var item in parsedData)
                    {
                        string webPagesJson = Newtonsoft.Json.JsonConvert.SerializeObject(item.WebPages);
                        string domainsJson = Newtonsoft.Json.JsonConvert.SerializeObject(item.Domains);
                        string insertQuery = "INSERT INTO UniversityData (Name, State, WebPages, Domains) " +
                                            "VALUES (@Name, @State, @WebPages, @Domains)";
                        using MySqlCommand command = new(insertQuery, connection);

                        command.Parameters.AddWithValue("@Name", item.Name);
                        command.Parameters.AddWithValue("@State", item.State);
                        command.Parameters.AddWithValue("@WebPages", webPagesJson);
                        command.Parameters.AddWithValue("@Domains", domainsJson);

                        command.ExecuteNonQuery();
                    }
                    Console.WriteLine("Database populated successfully!");
                }
                else {
                    Console.WriteLine("No valid data was retrieved from the API.");
                }
            }
            else {
                Console.WriteLine($"An error occurred while fetching data from the API: {response.StatusCode}");
                Console.WriteLine($"Response content: {await response.Content.ReadAsStringAsync()}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while populating the database: {ex.Message}");
        }
    }
}
