using GestaoDeBarbearia.Domain.Repositories;
using GestaoDeBarbearia.Infraestructure.Repositories;
using GestaoDeBarbearia.Infraestructure.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace GestaoDeBarbearia.Infraestructure;
public static class DependencyInjectionExtension
{
    public static void AddInfra(this IServiceCollection services)
    {
        // DBFunctions é uma classe que contém métodos úteis para manipulação do banco de dados
        services.AddScoped<DBFunctions>();

        // Injeção de dependência dos repositórios
        services.AddScoped<IScheduleRepository, ScheduleRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
    }
}
