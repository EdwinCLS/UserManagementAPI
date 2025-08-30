using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// 🔧 Registrar servicios
builder.Services.AddControllers();            // Habilita los controladores
builder.Services.AddEndpointsApiExplorer();   // Para Swagger
// Documentación interactiva
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingresa el token en el formato: Bearer {token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});             

var app = builder.Build();


// 🌐 Configurar middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();    // Redirige a HTTPS
app.UseMiddleware<ExceptionHandlingMiddleware>(); // Middleware de manejo de excepciones
app.UseMiddleware<TokenAuthenticationMiddleware>(); // Middleware de autenticación
app.UseMiddleware<RequestResponseLoggingMiddleware>(); // Middleware de logging
app.MapControllers();         // Mapea los controladores automáticamente

app.Run();