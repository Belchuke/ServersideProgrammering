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

        public AESEncryptResult Encrypt(string text)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = _KeyBytes;

                byte[] iv = new byte[16];

                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(iv);
                }

                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cryptoStream))
                        {
                            sw.Write(text);
                        }

                        byte[] result = memoryStream.ToArray();

                        return new AESEncryptResult()
                        {
                            EncryptedText = result,
                            IV = aes.IV,
                        };
                    }
                }
            }
        }

        public string Decrypt(byte[] cipherText, byte[] iv)
        {
            string plaintext = null;

            using (Aes aes = Aes.Create())
            {
                aes.Key = _KeyBytes;
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            plaintext = sr.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

    }
}
