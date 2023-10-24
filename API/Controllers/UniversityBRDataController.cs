using Microsoft.AspNetCore.Mvc;

namespace UniversityBRDataAPI.Controllers;

[ApiController]
[Route("uni-br-data")]
public class UniversityBRDataController : ControllerBase
{
    private static readonly string[] UniversityNames = new[]
    {
        "UNI A", "UNI B", "UNI C", "UNI D", "UNI E", "UNI F", "UNI G", "UNI H", "UNI I", "UNI J"
    };

    private readonly ILogger<UniversityBRDataController> _logger;

    public UniversityBRDataController(ILogger<UniversityBRDataController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetUniversityData")]
    public IEnumerable<UniversityBRData> Get()
    {
        return Enumerable.Range(1, 5).Select(index => 
        {
            int randomIndex = Random.Shared.Next(UniversityNames.Length);
            string universityName = UniversityNames[randomIndex];
            string webPage = $"http://www.{universityName.Replace(" ", "").ToLower()}.com";
            string domains = $"{universityName.Replace(" ", "").ToLower()}.com";
            return new UniversityBRData
            {
                Name = universityName,
                WebPages = new List<string> { webPage },
                Domains = new List<string> { domains }
            };
        })
        .ToArray();
    }
}
