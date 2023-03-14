using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Serversideprogrammeringsapi.Env;
using Serversideprogrammeringsapi.Models;
using System.Security.Cryptography;
using System.Text;

namespace Serversideprogrammeringsapi.Repo.AESRepo
{
    public class AESRepo : IAESRepo
    {
        private readonly string _AESKey;
        private readonly byte[] _KeyBytes;

        public AESRepo()
        {
            _AESKey = EnvHandler.GetAESKey();
            _KeyBytes = Encoding.UTF8.GetBytes(_AESKey);
        }

        public string GenerateINstring()
        {
            byte[] data = new byte[32]; // 32 bytes = 256 bits
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(data);
            }

            return Convert.ToBase64String(data);
        }

        public AESEncryptResult Encrypt(string text)
        {
            string IV = GenerateINstring();

            byte[] ivBytes = Encoding.UTF8.GetBytes(IV);
            byte[] plainBytes = Encoding.UTF8.GetBytes(text);

            using (Aes aes = Aes.Create())
            {
                aes.Key = _KeyBytes;
                aes.IV = ivBytes;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        byte[] cipherBytes = memoryStream.ToArray();

                        return new AESEncryptResult()
                        {
                            EncryptedText = Convert.ToBase64String(cipherBytes),
                            IV = IV,
                        };
                    }
                }
            }
        }

        public string Decrypt(string cipherText, string iv)
        {
            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = _KeyBytes;
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
