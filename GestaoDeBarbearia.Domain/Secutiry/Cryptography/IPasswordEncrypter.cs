namespace GestaoDeBarbearia.Domain.Secutiry.Cryptography;
public interface IPasswordEncrypter
{
    string Encrypt(string password);
    bool Verify(string password, string passwordHash);
}
