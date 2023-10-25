using Microsoft.AspNetCore.Mvc;

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
        return CreatedAtRoute("GetUniversityByPk", new { pk = university.Id }, university);
    }
}
