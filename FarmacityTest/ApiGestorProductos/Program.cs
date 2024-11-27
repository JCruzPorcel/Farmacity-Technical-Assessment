using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Backend.Data;
using Backend.Services;
using Backend.Repositories;
using Backend.Services.Interfaces;
using Backend.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configura servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Gestor Productos", Version = "v1" });
});

// Configuraci�n de CORS para permitir solicitudes solo desde localhost
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        var frontendUrl = builder.Configuration["FRONTEND_URL"]; // Obtiene la URL del frontend desde las configuraciones o variables de entorno
        if (!string.IsNullOrEmpty(frontendUrl))
        {
            policy.WithOrigins(frontendUrl)
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        }
        else
        {
            // En caso de no encontrar la variable de entorno, lanzar un error.
            throw new InvalidOperationException("La variable de entorno 'FRONTEND_URL' no est� definida. No se puede configurar CORS.");
        }
    });
});


// Configuraci�n de DbContext para SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Registrar los servicios y repositorios
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();

var app = builder.Build();

// Configura el pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gestor Productos V1");
        c.RoutePrefix = "swagger";
    });
}

// Configuraci�n global de manejo de excepciones
// Agregar middleware para manejar excepciones no controladas
app.UseExceptionHandler("/error");  // Redirige a un controlador de error personalizado si ocurre una excepci�n no controlada

// Habilitar CORS con la pol�tica configurada
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

// Iniciar la aplicaci�n
app.Run();
