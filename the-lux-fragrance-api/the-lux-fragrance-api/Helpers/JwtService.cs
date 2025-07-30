using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace the_lux_fragrance_api.Helpers;

public sealed class JwtService(IConfiguration configuration)
{
    private readonly string? _secureKey = configuration["Jwt:SecretKey"];

    public string? Generate(int id)
    {
        if (_secureKey == null) return null;
        
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secureKey));
        var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
        
        var header = new JwtHeader(credentials);

        var payload = new JwtPayload(
            configuration["Jwt:Issuer"], 
            null, 
            null,
            null, 
            DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationInMinutes")));
        
        var secutiryToken = new JwtSecurityToken(header, payload);
        
        return new JwtSecurityTokenHandler().WriteToken(secutiryToken);
    }

    public JwtSecurityToken? Verify(string? jwt)
    {
        if (_secureKey == null) return null;
        var key = Encoding.ASCII.GetBytes(_secureKey);
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(jwt, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = false
            }, out SecurityToken validatedToken);

            return ((JwtSecurityToken)validatedToken);
        }
        catch (SecurityTokenException)
        {
            // Token validation failed (e.g., signature invalid, expired)
            return null;
        }
        catch (Exception)
        {
            // Other unexpected errors during validation
            return null;
        }
    }
}