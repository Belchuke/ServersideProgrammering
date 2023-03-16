using Serversideprogrammeringsapi.Models;

namespace Serversideprogrammeringsapi.Repo.AESRepo
{
    public interface IAESRepo
    {

        AESEncryptResult Encrypt(string text);

        string Decrypt(byte[] cipherText, byte[] iv);
    }
}
