using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace the_lux_fragrance_api.Helpers;

public class JwtService
{
    private string secureKey = "sua-chave-secreta-bem-longa-1234567890";

    public string Generate(int id)
    {
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey));
        var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
        
        var header = new JwtHeader(credentials);

        var payload = new JwtPayload(id.ToString(), null, null, null, DateTime.Today.AddDays(1));
        var secutiryToken = new JwtSecurityToken(header, payload);
        
        return new JwtSecurityTokenHandler().WriteToken(secutiryToken);
    }

    public JwtSecurityToken Verify(string? jwt)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
         var key = Encoding.ASCII.GetBytes(secureKey);
        tokenHandler.ValidateToken(jwt, new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false
        }, out SecurityToken validatedToken);
        
        return ((JwtSecurityToken)validatedToken);
    }
}