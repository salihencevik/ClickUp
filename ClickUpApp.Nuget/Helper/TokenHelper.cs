using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using ClickUpApp.Nuget.Dto;

namespace ClickUpApp.Nuget.Helper
{
    public static class TokenHelper
    {
        private const int _saltSize = 128 / 8; // 128 bits
        private const int _keySize = 256 / 8; // 256 bits
        private const int _iterations = 10000;
        private static readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA256;
        private const char _delimiter = ':';

        public static TokenDto CreateToken(JwtSecurityToken jwtSecurityToken, DateTime accessTokenExpiration, DateTime refreshTokenExpiration)
        {
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return new TokenDto
            {
                AccessToken = token,
                AccessTokenExpiration = accessTokenExpiration
            };
        } 

        public static void ValidateToken(this UserAuthToken data, string environment)
        {
            if (data == null || data.Id == 0)
            {
                throw new Exception("InvalidUser");
            }

            var remainingTokenTime = GetTokenRemainingTime(data);

            if (environment != "DEV" && remainingTokenTime.HasValue && remainingTokenTime.Value < DateTime.Now)
            {
                throw new Exception("TokenExpired");
            }
        }

        /// <summary>
        /// Returns remaining time
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static DateTime? GetTokenRemainingTime(UserAuthToken data)
        {
            if (data == null)
            {
                return null;
            }

            return new DateTime(data.Ticks);
        }
    }
}
