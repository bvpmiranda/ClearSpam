namespace ClearSpam.Services
{
    public interface ICryptography
    {
        string Encrypt(string value);
        string Decrypt(string value);
    }
}
