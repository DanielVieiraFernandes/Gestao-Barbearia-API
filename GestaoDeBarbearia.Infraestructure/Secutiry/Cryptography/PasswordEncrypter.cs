using GestaoDeBarbearia.Domain.Secutiry.Cryptography;
using BC = BCrypt.Net.BCrypt;

namespace GestaoDeBarbearia.Infraestructure.Secutiry.Cryptography;
public class PasswordEncrypter : IPasswordEncrypter
{
    /// <summary>
    /// Número de salts para gerar o hash da senha (quanto maior, exige mais processamento)
    /// </summary>
    private const int NUMBER_OF_SALT = 8;

    public string Encrypt(string password) => BC.HashPassword(password, NUMBER_OF_SALT);

    public bool Verify(string password, string passwordHash) => BC.Verify(password, passwordHash);
}
