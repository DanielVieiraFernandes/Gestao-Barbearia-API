using GestaoDeBarbearia.Communication.Requests;
using GestaoDeBarbearia.Communication.Responses;
using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Repositories;
using GestaoDeBarbearia.Domain.Secutiry.Cryptography;
using GestaoDeBarbearia.Domain.Secutiry.Tokens;

namespace GestaoDeBarbearia.Application.UseCases.Users.Employees;
public class RegisterEmployeeUseCase
{
    private readonly IAccessTokenGenerator accessTokenGenerator;
    private readonly IEmployeesRepository employeesRepository;
    private readonly IPasswordEncrypter passwordEcrypter;

    public RegisterEmployeeUseCase(IEmployeesRepository employeesRepository,
        IAccessTokenGenerator accessTokenGenerator,
        IPasswordEncrypter passwordEncrypter)
    {
        this.employeesRepository = employeesRepository;
        this.accessTokenGenerator = accessTokenGenerator;
        this.passwordEcrypter = passwordEncrypter;
    }
    public async Task<ResponseCreatedUserJson> Execute(RequestEmployeeJson request)
    {
        Employee employee = new()
        {
            Name = request.Name,
            Email = request.Email,
            Position = request.Position,
            Salary = request.Salary,
            Telephone = request.Telephone,
        };

        var passwordHashed = passwordEcrypter.Encrypt(request.Password);

        employee.Password = passwordHashed;

        await employeesRepository.Create(employee);

        return new ResponseCreatedUserJson
        {
            Token = accessTokenGenerator.Generate(employee)
        };

    }

}
