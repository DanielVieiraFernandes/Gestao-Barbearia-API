using GestaoDeBarbearia.Domain.Repositories;
using GestaoDeBarbearia.Domain.Secutiry.Cryptography;
using GestaoDeBarbearia.Domain.Secutiry.Tokens;
using GestaoDeBarbearia.Infraestructure.Repositories;
using GestaoDeBarbearia.Infraestructure.Secutiry.Cryptography;
using GestaoDeBarbearia.Infraestructure.Secutiry.Tokens;
using GestaoDeBarbearia.Infraestructure.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GestaoDeBarbearia.Infraestructure;
public static class DependencyInjectionExtension
{
    public static void AddInfra(this IServiceCollection services, IConfiguration configuration)
    {
        // DBFunctions é uma classe que contém métodos úteis para manipulação do banco de dados
        services.AddScoped<DatabaseQueryBuilder>();

        // Injeção de dependência dos repositórios
        services.AddScoped<ISchedulesRepository, SchedulesRepository>();
        services.AddScoped<IProductsRepository, ProductsRepository>();
        services.AddScoped<ISalesRepository, SalesRepository>();
        services.AddScoped<IImpostosRepository, ImpostosRepository>();
        services.AddScoped<IExpensesRepository, ExpensesRepository>();
        services.AddScoped<IServicesRepository, ServicesRepository>();
        services.AddScoped<IEmployeesRepository, EmployeesRepository>();

        // Injeção de dependência das classes referentes a segurança da API
        // Criptografia e Tokens

        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpiresMinutes");
        var privateKey = configuration.GetValue<string>("Settings:Jwt:PrivateKey");

        if (privateKey is null || privateKey.Trim().Length == 0)
            throw new System.Exception("Chave de criptografia não fornecida");

        services.AddScoped<IAccessTokenGenerator>(config =>
        new JwtAccessTokenGenerator(expirationTimeMinutes, privateKey));

        services.AddScoped<IPasswordEncrypter, PasswordEncrypter>();
    }
}
