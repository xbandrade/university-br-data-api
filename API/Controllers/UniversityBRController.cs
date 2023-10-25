using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace UniversityBRDataAPI.Controllers;

[ApiController]
[Route("uni-br")]
public class UniversityBRDataController : ControllerBase
{
    private readonly UniversityDBContext _dbContext;
    private readonly ILogger<UniversityBRDataController> _logger;

    public UniversityBRDataController(UniversityDBContext dbContext, ILogger<UniversityBRDataController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet("search", Name = "GetUniversityData")]
    [SwaggerOperation(Summary = "Get data from all universities",
        Description = "This endpoint retrieves data from every university in the database. " +
                      "If the database is initially empty, it will try to populate it first.")]
    public async Task<IActionResult> Get(int page = 1, int pageSize = 10)
    {
        try
        {
            var universitiesQuery = _dbContext.Universities;
            var dbHasData = universitiesQuery.Any();
        }
        catch (MySqlConnector.MySqlException)
        {
            try
            {
                string relativePath = @".\Env\ConnectionString.txt";
                string fullPath = Path.Combine(Environment.CurrentDirectory, relativePath);
                string connectionString = System.IO.File.ReadAllText(fullPath);
                var dataPopulator = new DataPopulator(connectionString, _dbContext);
                await dataPopulator.PopulateDatabase();
                _logger.LogInformation("Database populated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating data: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        var universities = _dbContext.Universities 
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new
            {
                Id = u.Id,
                Name = u.Name,
                State = u.State
            })
            .ToList();
        string? baseUrl = Url.Action("GetUniversityData");
        int totalItems = _dbContext.Universities.Count();
        int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
        if (page < 1)
        {
            page = 1;
        }
        else if (page > totalPages)
        {
            page = totalPages;
        }
        var result = new
        {
            CurrentPage = page,
            TotalItems = totalItems,
            TotalPages = totalPages,
            PreviousPage = page > 1 ? $"{baseUrl}?page={page - 1}&pageSize={pageSize}" : null,
            NextPage = page < totalPages ? $"{baseUrl}?page={page + 1}&pageSize={pageSize}" : null,
            Data = universities,
        };
        return Ok(result);
    }

    [HttpGet("search/{pk}", Name = "GetUniversityByPk")]
    [SwaggerOperation(Summary = "Get data from a specific university",
        Description = "This endpoint retrieves data from a specific university in the database.")]
    public IActionResult Get(int pk)
    {
        try
        {
            var universitiesQuery = _dbContext.Universities;
            var dbHasData = universitiesQuery.Any();
        }
        catch (MySqlConnector.MySqlException)
        {
            return NotFound();
        }
        Console.WriteLine($"Retrieving university with PK {pk} from database");
        var university = _dbContext.Universities.FirstOrDefault(u => u.Id == pk);
        if (university == null)
        {
            return NotFound();
        }
        _logger.LogInformation($"Retrieved university with PK {pk} from database");
        return Ok(university);
    }

    [HttpPost("create", Name = "CreateUniversityData")]
    [SwaggerOperation(Summary = "Create a new entry for university data",
        Description = "This endpoint creates a new entry for university data in the database.")]
    public IActionResult CreateUniversity([FromBody] BrUniversity university)
    {
        if (university == null)
        {
            return BadRequest();
        }
        _dbContext.Universities.Add(university);
        _dbContext.SaveChanges();
        _logger.LogInformation($"Created new university with PK {university.Id} in database");
        return CreatedAtRoute("GetUniversityByPk", new { pk = university.Id }, university);
    }

    [HttpPost("update-db", Name = "UpdateUniversityData")]
    [SwaggerOperation(Summary = "Update the university database",
        Description = "This endpoint populates the university database if it is empty, " +
                      "and updates it with new data from the base API if there are any new universities.")]
    public async Task<IActionResult> UpdateUniversityData()
    {
        try
        {
            string relativePath = @".\Env\ConnectionString.txt";
            string fullPath = Path.Combine(Environment.CurrentDirectory, relativePath);
            string connectionString = System.IO.File.ReadAllText(fullPath);
            var dataPopulator = new DataPopulator(connectionString, _dbContext);
            await dataPopulator.PopulateDatabase();
            return Ok(new { message = "Data updated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating data: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
    
}
