using System.Text;
using System.Security.Cryptography; 

namespace ClickUpApp.Nuget.Helper
{
    /// <summary>
    /// Cryptolog
    /// </summary> 
    public static class Cryptolog
    {                                     
        private readonly static string protectorWord = "@TS_AU!T*_TOKEN&";

        /// <summary>
        /// Encrypt given data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Encrypt(string data)
        {
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(data);
            using (var aes = Aes.Create())
            {
                aes.Key = UTF8Encoding.UTF8.GetBytes(protectorWord);
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = aes.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                aes.Clear();
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
        }

        /// <summary>
        /// Decrypt token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string Decrypt(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("TokenNotProvided");
            }

            string decryptedToken = string.Empty;
            byte[] inputArray = Convert.FromBase64String(token);
            using (var aes = Aes.Create())
            {
                aes.Key = UTF8Encoding.UTF8.GetBytes(protectorWord);
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = aes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                aes.Clear();
                decryptedToken = UTF8Encoding.UTF8.GetString(resultArray);
            }

            if (string.IsNullOrEmpty(decryptedToken))
            {
                throw new Exception("Not Valid Token");
            }

            return decryptedToken;
        } 
    }
}
