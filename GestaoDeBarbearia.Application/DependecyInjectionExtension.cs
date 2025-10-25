using GestaoDeBarbearia.Application.UseCases.Appointments;
using GestaoDeBarbearia.Application.UseCases.Billing.Reports.Excel;
using GestaoDeBarbearia.Application.UseCases.Expenses;
using GestaoDeBarbearia.Application.UseCases.Products;
using GestaoDeBarbearia.Application.UseCases.Sales;
using Microsoft.Extensions.DependencyInjection;

namespace GestaoDeBarbearia.Application;
public static class DependecyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        // Casos de uso de agendamentos
        services.AddScoped<ScheduleServiceUseCase>();
        services.AddScoped<ConfirmScheduleUseCase>();
        services.AddScoped<FetchAppointmentsUseCase>();
        services.AddScoped<MarkServiceAsCompletedUseCase>();
        services.AddScoped<GetAppointmentByIdUseCase>();

        // Casos de uso de produtos
        services.AddScoped<CreateProductUseCase>();
        services.AddScoped<GetProductByIdUseCase>();
        services.AddScoped<FetchProductsUseCase>();
        services.AddScoped<UpdateProductUseCase>();

        // Casos de uso de vendas
        services.AddScoped<RegisterSaleUseCase>();
        services.AddScoped<FetchSalesUseCase>();

        // Casos de uso de relatório
        services.AddScoped<GenerateBillingReportUseCase>();

        // Casos de uso de despesas
        services.AddScoped<RegisterExpenseUseCase>();
        services.AddScoped<ExpensePaymentUseCase>();
        services.AddScoped<GetExpenseByIdUseCase>();
        services.AddScoped<FetchAllExpensesUseCase>();
    }
}
