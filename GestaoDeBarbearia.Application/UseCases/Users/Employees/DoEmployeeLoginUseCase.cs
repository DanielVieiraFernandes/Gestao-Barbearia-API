using GestaoDeBarbearia.Communication.Requests;
using GestaoDeBarbearia.Communication.Responses;
using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Repositories;
using GestaoDeBarbearia.Domain.Secutiry.Cryptography;
using GestaoDeBarbearia.Domain.Secutiry.Tokens;
using GestaoDeBarbearia.Exception.ExceptionsBase;

namespace GestaoDeBarbearia.Application.UseCases.Users.Employees;
public class DoEmployeeLoginUseCase
{
    private readonly IEmployeesRepository employeesRepository;
    private readonly IPasswordEncrypter passwordEncrypter;
    private readonly IAccessTokenGenerator accessTokenGenerator;
    public DoEmployeeLoginUseCase(
        IEmployeesRepository employeesRepository,
        IPasswordEncrypter passwordEncrypter,
        IAccessTokenGenerator accessTokenGenerator)
    {
        this.employeesRepository = employeesRepository;
        this.passwordEncrypter = passwordEncrypter;
        this.accessTokenGenerator = accessTokenGenerator;
    }

    public async Task<ResponseLoginJson> Execute(RequestLoginJson request)
    {
        Employee? user = await employeesRepository.GetByEmail(request.Email);

        if (user is null)
            throw new InvalidLoginException();

        bool passwordIsValid = passwordEncrypter.Verify(request.Password, user.Password);

        if (!passwordIsValid)
            throw new InvalidLoginException();

        return new ResponseLoginJson { Token = accessTokenGenerator.Generate(user) };

    }
}
