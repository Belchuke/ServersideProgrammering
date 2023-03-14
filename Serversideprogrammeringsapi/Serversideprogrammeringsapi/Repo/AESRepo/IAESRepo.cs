using Serversideprogrammeringsapi.Models;

namespace Serversideprogrammeringsapi.Repo.AESRepo
{
    public interface IAESRepo
    {
        string GenerateINstring();

        AESEncryptResult Encrypt(string text);

        string Decrypt(string cipherText, string iv);
    }
}
