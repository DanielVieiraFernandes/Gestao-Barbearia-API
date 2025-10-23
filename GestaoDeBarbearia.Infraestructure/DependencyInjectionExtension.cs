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
        services.AddScoped<ISchedulesRepository, SchedulesRepository>();
        services.AddScoped<IProductsRepository, ProductsRepository>();
        services.AddScoped<ISalesRepository, SalesRepository>();
        services.AddScoped<IImpostosRepository, ImpostosRepository>();
        services.AddScoped<IExpensesRepository, ExpensesRepository>();
        services.AddScoped<IServicesRepository, ServicesRepository>();
    }
}
