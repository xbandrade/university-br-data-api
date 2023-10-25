using MySqlConnector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace UniversityBRDataAPI;
public class DataPopulator
{
    private readonly string APIUrl = "http://universities.hipolabs.com/search?name=&country=brazil";
    private readonly string connectionString;
    private readonly UniversityDBContext _dbContext;

    public DataPopulator(string connectionString, UniversityDBContext dbContext)
    {
        this.connectionString = connectionString;
        _dbContext = dbContext;
    }

    public async Task PopulateDatabase()
    {
        using MySqlConnection connection = new(connectionString);
        try
        {
            Console.WriteLine("Updating DB");
            string createDatabaseQuery = "CREATE DATABASE IF NOT EXISTS BrUniversityAPI";
            connection.Open();
            using MySqlCommand createDatabaseCommand = new(createDatabaseQuery, connection);
            createDatabaseCommand.ExecuteNonQuery();
            string useDatabaseQuery = "USE BrUniversityAPI";
            using MySqlCommand useDatabaseCommand = new(useDatabaseQuery, connection);
            useDatabaseCommand.ExecuteNonQuery();

            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS University (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    Name VARCHAR(255) UNIQUE,
                    State VARCHAR(255),
                    WebPages VARCHAR(255),
                    Domains VARCHAR(255)
                );";

            using MySqlCommand createTableCommand = new(createTableQuery, connection);
            createTableCommand.ExecuteNonQuery();

            using HttpClient client = new();
            HttpResponseMessage response = await client.GetAsync(APIUrl);
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var rawJsonData = JArray.Parse(data);
                foreach (var item in rawJsonData)
                {
                    string universityName = item["name"]?.ToString() ?? "";
                    var universityExists = _dbContext.Universities.FirstOrDefault(u => u.Name == universityName);
                    if (universityExists != null)
                    {
                        continue;
                    }
                    var university = new BrUniversity
                    {
                        Name = universityName,
                        State = item["state-province"]?.ToString(),
                    };

                    List<string>? universityWebPages = new();
                    List<string>? universityDomains = new();
                    if (item["web_pages"] is JArray webPagesArray)
                    {
                        foreach (var webPage in webPagesArray)
                        {
                            universityWebPages.Add(webPage.ToString());
                        }
                    }
                    if (item["domains"] is JArray domainsArray)
                    {
                        foreach (var domain in domainsArray)
                        {
                            universityDomains.Add(domain.ToString());
                        }
                    }
                    string concatenatedWebPages = universityWebPages != null ? string.Join(", ", universityWebPages) : "";
                    string concatenatedDomains = universityDomains != null ? string.Join(", ", universityDomains) : "";

                    string insertUniversityQuery = "INSERT INTO University (Name, State, WebPages, Domains) " +
                                                "VALUES (@Name, @State, @WebPages, @Domains)";
                    using MySqlCommand command = new(insertUniversityQuery, connection);
                    command.Parameters.AddWithValue("@Name", university.Name);
                    command.Parameters.AddWithValue("@State", university.State ?? string.Empty);
                    command.Parameters.AddWithValue("@WebPages", concatenatedWebPages);
                    command.Parameters.AddWithValue("@Domains", concatenatedDomains);
                    command.ExecuteNonQuery();
                }
                Console.WriteLine("Database updated successfully!");
            }
            else
            {
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
