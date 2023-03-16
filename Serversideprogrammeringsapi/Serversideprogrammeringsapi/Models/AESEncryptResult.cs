namespace Serversideprogrammeringsapi.Models
{
    public class AESEncryptResult
    {
        public byte[] EncryptedText { get; set; }
        public byte[] IV { get; set; }
    }
}
