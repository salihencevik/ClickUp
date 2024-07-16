using System.Security.Cryptography; 

namespace ClickUpApp.Nuget.Helper
{
    public static class PasswordHelper
    {
        private const int _saltSize = 128 / 8; // 128 bits
        private const int _keySize = 256 / 8; // 256 bits
        private const int _iterations = 10000;
        private static readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA256;
        private const char _delimiter = ':'; 

        public static string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(_saltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, _iterations, HashAlgorithmName.SHA256, _keySize);

            return string.Join(_delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        public static bool VerifyPassword(string passwordHash, string inputPassword)
        {
            var elements = passwordHash.Split(_delimiter);
            var salt = Convert.FromBase64String(elements[0]);
            var hash = Convert.FromBase64String(elements[1]);

            var hashInput = Rfc2898DeriveBytes.Pbkdf2(inputPassword, salt, _iterations, HashAlgorithmName.SHA256, _keySize);

            return CryptographicOperations.FixedTimeEquals(hash, hashInput);
        }  
    }
}
