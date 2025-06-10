using the_lux_fragrance_api.Dto;
using the_lux_fragrance_api.Models;

namespace the_lux_fragrance_api.Mappings;

public static class UserMapping
{
    public static User ToModel(this UserDto userDto) => new User
    {
        Name = userDto.Name,
        Email = userDto.Email,
        Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
    };
}