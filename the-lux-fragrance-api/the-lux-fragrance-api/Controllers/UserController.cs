using Microsoft.AspNetCore.Mvc;
using the_lux_fragrance_api.Dto;
using the_lux_fragrance_api.Helpers;
using the_lux_fragrance_api.Mappings;
using the_lux_fragrance_api.Service.Interface;

namespace the_lux_fragrance_api.Controllers;

[Route("api")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly JwtService _jwtService;

    public UserController(IUserService userService, JwtService jwtService)
    {
        _userService = userService;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] UserDto userDto)
    {
        var user = userDto.ToModel();
        _userService.CriarUser(user);
        return Ok("success");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserDto userDto)
    {
        var user = _userService.GetByEmail(userDto.Email);

        if (user == null) return BadRequest(new { message = "Email or password is incorrect" });

        if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password))
        {
            return BadRequest(new { message = "Email or password is incorrect" });
        }

        var jwt = _jwtService.Generate(user.Id);

        Response.Cookies.Append("jwt", jwt, new CookieOptions
        {
            HttpOnly = true,
        });

        return Ok(new
        {
            jwt
        });
    }

    [HttpGet("user")]
    public IActionResult User()
    {
        try
        {
            var jwt = Request.Cookies["jwt"];

            var token = _jwtService.Verify(jwt);

            int userId = int.Parse(token.Issuer);

            var user = _userService.GetById(userId);

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