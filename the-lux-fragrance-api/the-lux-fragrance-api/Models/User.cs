using System.Text.Json.Serialization;

namespace the_lux_fragrance_api.Models;

public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    [JsonIgnore] public string? Password { get; set; }
}
