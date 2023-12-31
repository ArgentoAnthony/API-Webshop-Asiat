﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API_Webshop_Asiat.Tools
{
    public class JwtTokenService
    {
        public int? GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);

            var claims = jsonToken.Claims;

            var id = claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

            return id != null ? int.Parse(id) : null;
        }
    }
}
