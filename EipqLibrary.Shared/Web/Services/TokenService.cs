using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using EipqLibrary.Shared.Models;
using EipqLibrary.Shared.SharedSettings;
using EipqLibrary.Shared.Web.Dtos.Tokens;
using EipqLibrary.Shared.Web.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace EipqLibrary.Shared.Web.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly TokenSettings _tokenSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(
            JwtSettings jwtSettings,
            TokenSettings tokenSettings,
            TokenValidationParameters tokenValidationParameters,
            IHttpContextAccessor httpContextAccessor)
        {
            _jwtSettings = jwtSettings;
            _tokenSettings = tokenSettings;
            _tokenValidationParameters = tokenValidationParameters.Clone();
            _tokenValidationParameters.ValidateLifetime = false;
            _httpContextAccessor = httpContextAccessor;
        }

        private string CreateToken(
            ClaimsIdentity subject,
            byte[] key,
            DateTime expiryDate)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                Expires = expiryDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public TokenInfo CreateToken(UserTokenInfo user, string deviceId, string loginProvider = null)
        {
            var expiryDate = DateTime.UtcNow.AddMinutes(_tokenSettings.AccessTokenLifetimeInMinutes);
            var tokenId = Guid.NewGuid().ToString();

            var claims = GetClaimsForUser(user);
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, tokenId));
            claims.Add(new Claim("deviceId", deviceId));

            if (loginProvider != null)
            {
                claims.Add(new Claim("provider", loginProvider));
            }

            var token = CreateToken(
                new ClaimsIdentity(claims),
                Encoding.ASCII.GetBytes(_jwtSettings.JwtTokenSecret),
                expiryDate);

            return new TokenInfo
            {
                Token = token,
                ExpiryDate = expiryDate,
                Id = tokenId
            };
        }

        private ICollection<Claim> GetClaimsForUser(UserTokenInfo user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Id)
            };

            if (user.Roles != null)
            {
                claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r)));
            }

            return claims;
        }

        public bool TryGetPrincipalFromToken(string token, out ClaimsPrincipal principal)
        {
            principal = null;
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                if (IsJwtTokenValid(validatedToken))
                {
                    return true;
                }
            }
            catch
            {
                // ignored
            } // we don't really care what's wrong with token

            return false;
        }

        private static bool IsJwtTokenValid(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                       StringComparison.InvariantCultureIgnoreCase);
        }

        public string GetTokenJti(ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(JwtRegisteredClaimNames.Jti);
        }

        public string GetUserId(ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public string GetDeviceId(ClaimsPrincipal principal)
        {
            return principal.FindFirstValue("deviceId");
        }

        public JwtSecurityToken DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            SecurityToken jsonToken;
            try
            {
                jsonToken = handler.ReadToken(token);
            }
            catch (Exception)
            {
                throw new CustomExceptions.BadDataException("Invalid access token");
            }

            var tokenS = jsonToken as JwtSecurityToken;
            if (tokenS == null)
            {
                throw new InvalidOperationException("Unable to parse the given token");
            }

            return tokenS;
        }

        public string CurrentUserId => GetUserId(_httpContextAccessor?.HttpContext?.User);

        public string CurrentDeviceId => GetDeviceId(_httpContextAccessor?.HttpContext?.User);

        public string CurrentTokenJti => GetTokenJti(_httpContextAccessor?.HttpContext?.User);
    }
}
