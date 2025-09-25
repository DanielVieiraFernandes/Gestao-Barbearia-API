using GestaoDeBarbearia.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace GestaoDeBarbearia.Application;
public static class DependecyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ScheduleServiceUseCase>();
        services.AddScoped<ConfirmScheduleUseCase>();
        services.AddScoped<FetchAppointmentsUseCase>();
        services.AddScoped<MarkServiceAsCompletedUseCase>();
        services.AddScoped<GetAppointmentByIdUseCase>();
    }
}
