using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace UniversityBRDataAPI;

public class BrUniversity
{
    [Key]
    public int Id { get; set; }

    public string? Name { get; set; }

    [JsonProperty("state-province")]
    public string? State { get; set; }

    [JsonProperty("web_pages")]
    public string? WebPages { get; set; }

    [JsonProperty("domains")]
    public string? Domains { get; set; }
}