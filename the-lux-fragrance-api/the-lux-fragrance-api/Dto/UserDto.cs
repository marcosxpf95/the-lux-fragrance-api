namespace the_lux_fragrance_api.Dto;

public record UserDto
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}