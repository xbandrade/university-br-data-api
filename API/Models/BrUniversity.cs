using System.ComponentModel.DataAnnotations;
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
    public ICollection<string> WebPages { get; set; } = new List<string>();

    public ICollection<string> Domains { get; set; } = new List<string>();
}
