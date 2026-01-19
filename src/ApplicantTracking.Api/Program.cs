using ApplicantTracking.Infrastructure.Persistence;
using ApplicantTracking.Infrastructure.Repositories;
using ApplicantTracking.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuração do DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlServer"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,              // número máximo de tentativas
            maxRetryDelay: TimeSpan.FromSeconds(10), // tempo máximo entre tentativas
            errorNumbersToAdd: null        // erros adicionais que podem disparar retry
        )
    ));

// Registro dos repositórios e UoW
builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();
builder.Services.AddScoped<ITimelineRepository, TimelineRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// MediatR (descobre todos os Handlers na camada Application)
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<ApplicantTracking.Application.Queries.GetCandidatesQuery>());

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ApplicantTracking API",
        Version = "v1",
        Description = "API para gerenciamento de candidatos e auditoria de ações"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApplicantTracking API v1");
        c.RoutePrefix = string.Empty; // abre Swagger direto na raiz (http://localhost:5000)
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
