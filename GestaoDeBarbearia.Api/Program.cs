using GestaoDeBarbearia.Api.Filters;
using GestaoDeBarbearia.Api.Middleware;
using GestaoDeBarbearia.Application;
using GestaoDeBarbearia.Infraestructure;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc(op => op.Filters.Add(typeof(ExceptionFilter)));
builder.Services.AddRouting(op =>
{
    op.LowercaseUrls = true;
    op.LowercaseQueryStrings = true;
});

// Faz a Injeção de dependência das classes dentro do projeto de Application
// UseCases
builder.Services.AddApplication();

// Faz a injeção de dependência das classes dentro do projeto de Infraestructure
// Repositories
builder.Services.AddInfra();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
