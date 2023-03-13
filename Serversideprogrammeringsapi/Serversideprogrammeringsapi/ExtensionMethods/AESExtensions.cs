using System.Security.Cryptography;
using System.Text;

namespace Serversideprogrammeringsapi.ExtensionMethods
{
    public static class AESExtensions
    {
        public static string Encrypt(this string text, string key, string iv)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
            byte[] plainBytes = Encoding.UTF8.GetBytes(text);

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = ivBytes;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        byte[] cipherBytes = memoryStream.ToArray();
                        return Convert.ToBase64String(cipherBytes);
                    }
                }
            }
        }


        public static string Decrypt(this string cipherText, string key, string iv)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = ivBytes;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        byte[] plainBytes = new byte[cipherBytes.Length];
                        int decryptedByteCount = cryptoStream.Read(plainBytes, 0, plainBytes.Length);
                        return Encoding.UTF8.GetString(plainBytes, 0, decryptedByteCount);
                    }
                }
            }
        }
    }
}
