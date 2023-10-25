using Microsoft.AspNetCore.Mvc;

namespace UniversityBRDataAPI.Controllers;

[ApiController]
[Route("unibr-data")]
public class UniversityBRDataController : ControllerBase
{
    private static readonly string[] UniversityNames = new[]
    {
        "UNI A", "UNI B", "UNI C", "UNI D", "UNI E", "UNI F", "UNI G", "UNI H", "UNI I", "UNI J"
    };

    private static readonly string[] States = new[]
    {
        "RJ", "SP", "ES", "BA", "SC", "AM", "PA", "GO", "MT", "MA"
    };

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
}
