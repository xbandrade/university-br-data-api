using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace UniversityBRDataAPI.Controllers;

[ApiController]
[Route("unibr-data")]
public class UniversityBRDataController : ControllerBase
{
    private readonly UniversityDBContext _dbContext;
    private readonly ILogger<UniversityBRDataController> _logger;

    public UniversityBRDataController(UniversityDBContext dbContext, ILogger<UniversityBRDataController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet(Name = "GetUniversityData")]
    public IEnumerable<BrUniversity> Get()
    {
        Console.WriteLine("Retrieving universities from database");
        var universities = _dbContext.Universities.ToList();
        return universities;
    }

    [HttpGet("{pk}", Name = "GetUniversityByPk")]
    public IActionResult Get(int pk)
    {
        Console.WriteLine($"Retrieving university with PK {pk} from database");
        var university = _dbContext.Universities.FirstOrDefault(u => u.Id == pk);
        if (university == null)
        {
            return NotFound();
        }
        _logger.LogInformation($"Retrieved university with PK {pk} from database");
        return Ok(university);
    }

    [HttpPost(Name = "CreateUniversityData")]
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

    [HttpPost("update-data", Name = "UpdateUniversityData")]
    public async Task<IActionResult> UpdateUniversityData()
    {
        try
        {
            
            string uniAPIUrl = "http://universities.hipolabs.com/search?name=&country=brazil";
            string relativePath = @".\Env\ConnectionString.txt";
            string fullPath = Path.Combine(Environment.CurrentDirectory, relativePath);
            string connectionString = System.IO.File.ReadAllText(fullPath);
            var dataPopulator = new DataPopulator(uniAPIUrl, connectionString, _dbContext);
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
