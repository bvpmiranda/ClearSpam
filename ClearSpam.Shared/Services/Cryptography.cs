using ClearSpam.Infrastructure;

namespace ClearSpam.Services
{
    public class Cryptography : ICryptography
    {
        private SimpleAes simpleAes;

        public Cryptography()
        {
            simpleAes = new SimpleAes();
        }

        public string Decrypt(string value)
        {
            return simpleAes.DecryptString(value);
        }

        public string Encrypt(string value)
        {
            return simpleAes.EncryptToString(value);
        }
    }
}
