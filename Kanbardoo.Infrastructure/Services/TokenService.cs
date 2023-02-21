using Kanbardoo.Application.Contracts;
using Kanbardoo.Domain.Authorization;
using Kanbardoo.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Kanbardoo.Infrastructure.Services;
public class TokenService : ICreateToken
{
    private readonly SymmetricSecurityKey _symmetricSecurityKey;

    public TokenService(IConfiguration configuration)
    {
        _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenSecretKey"]!));
    }

    public string Create(KanUser user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(KanClaimName.ID, user.ID.ToString()),
        };

        foreach (var claim in user.GetClaimList())
        {
            claims.Add(new Claim(claim.Name, JsonConvert.SerializeObject(claim.Value)));
        }

        SigningCredentials credentials = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);

        var jwtToken = new JwtSecurityToken(
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: credentials);

        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return token;
    }
}
