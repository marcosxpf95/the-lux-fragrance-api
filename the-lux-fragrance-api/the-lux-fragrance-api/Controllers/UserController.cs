using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using the_lux_fragrance_api.Dto;
using the_lux_fragrance_api.Helpers;
using the_lux_fragrance_api.Mappings;
using the_lux_fragrance_api.Service.Interface;

namespace the_lux_fragrance_api.Controllers;

[Route("api")]
[ApiController]
public class UserController(IUserService userService, JwtService jwtService) : ControllerBase
{
    [HttpPost("register")]
    public IActionResult Register([FromBody] UserDto userDto)
    {
        var user = userDto.ToModel();
        userService.CriarUser(user);
        return Ok("success");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserDto userDto)
    {
        var user = userService.GetByEmail(userDto.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password))
        {
            return BadRequest(new { message = "Email or password is incorrect" });
        }
        
        var jwt = jwtService.Generate(user.Id);

        Response.Cookies.Append("jwt", jwt!, new CookieOptions
        {
            HttpOnly = true,
        });

        return Ok(new
        {
            jwt
        });
    }

    [HttpGet("user")]
    public new IActionResult User()
    {
        try
        {
            var jwt = Request.Cookies["jwt"];

            var token = jwtService.Verify(jwt);

            if (token == null) return BadRequest(new { message = "Token is incorrect" });
            
            int userId = int.Parse(token.Issuer);

            var user = userService.GetById(userId);

            return Ok(user);

        }
        catch (Exception _)
        {
            return Unauthorized();
        }
    }

    [HttpPost("logout")]
    public IActionResult LogOut()
    {
        Response.Cookies.Delete("jwt");
        return Ok("Success");
    }
}