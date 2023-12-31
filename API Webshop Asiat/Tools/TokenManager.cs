using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Webshop_DAL.Models;

namespace API_Webshop_Asiat.Tools
{
    public class TokenManager
    {
        public static string _secretKey = "igflkjsbdcùpksdjgugx!'éqsdkqsdffgdsssqdµfùm$^sqdµùm12356D122klnxù$^:'";

        public string GenerateToken(User user)
        {

            // 1 : Générer la clé de signature
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_secretKey));
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha512);

            // 2 : Données du token et de l'user
            Claim[] claims = new[] {

        new Claim(ClaimTypes.Sid, user.Id.ToString()),
        new Claim(ClaimTypes.Role, user.Role)
    };

            // 3 : Construction du token
            JwtSecurityToken jwt = new(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.Now.AddDays(1),
                issuer: "webshop.com"
            );

            JwtSecurityTokenHandler jwtHandler = new();
            return jwtHandler.WriteToken(jwt);
        }
    }
}
