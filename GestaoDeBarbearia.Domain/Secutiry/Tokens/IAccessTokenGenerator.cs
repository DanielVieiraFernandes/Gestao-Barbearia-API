using GestaoDeBarbearia.Domain.Entities;

namespace GestaoDeBarbearia.Domain.Secutiry.Tokens;
public interface IAccessTokenGenerator
{
    string Generate(User user);
}
