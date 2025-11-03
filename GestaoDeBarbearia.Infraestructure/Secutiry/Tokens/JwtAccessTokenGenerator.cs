using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Secutiry.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace GestaoDeBarbearia.Infraestructure.Secutiry.Tokens;
public class JwtAccessTokenGenerator : IAccessTokenGenerator
{
    private readonly uint _expirationTimeMinutes;
    private readonly ECDsaSecurityKey _privateKey;

    public JwtAccessTokenGenerator(uint expirationTimeMinutes, string privateKey)
    {
        _expirationTimeMinutes = expirationTimeMinutes;

        // Cria o objeto ECDsa a partir da chave privada 
        var ecdsa = ECDsa.Create();
        ecdsa.ImportFromPem(privateKey);

        _privateKey = new ECDsaSecurityKey(ecdsa);
    }

    public string Generate(User user)
    {
        //++++++++++++++++++++++++++++++++++++++++++++
        // Descreve como o token JWT deve ser gerado
        //++++++++++++++++++++++++++++++++++++++++++++

        // Aqui guardamos as informações que serão armazenadas no token

        /*
        NÃO É OBRIGATÓRIO UTILIZAR O ClaimTypes, É APENAS UMA STRING,
        PODEMOS DIGITAR UMA STRING LIVREMENTE -> ex:

           new Claim("nome", user.Name)
        */

        List<Claim> claims = new()
        {
            new("nome", user.Name),
        };

        var claimsIdentity = new ClaimsIdentity(claims);

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Configura as credenciais em um objeto para indicar como o token deve ser gerado
        // 
        // criptografia -> ES256 com uma chave pública e privada
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = claimsIdentity,
            Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
            SigningCredentials = new SigningCredentials(_privateKey, SecurityAlgorithms.EcdsaSha256),
        };

        // Cria um handler para criar o token e devolver ele do método

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }

}
