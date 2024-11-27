using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ApiGestorProductos.Services.Interfaces;
using ApiGestorProductos.Services;
using ApiGestorProductos.Repositories.Interfaces;
using ApiGestorProductos.Repositories;
using ApiGestorProductos.Data;

var builder = WebApplication.CreateBuilder(args);

// Configura servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Gestor Productos", Version = "v1" });
});

// Configuración de CORS para permitir solicitudes solo desde localhost
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:5027", "https://localhost:7009") // Puertos Default, cambiar de ser necesario.
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Configuración de DbContext para SQL Server
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

// Configuración global de manejo de excepciones
// Agregar middleware para manejar excepciones no controladas
app.UseExceptionHandler("/error");  // Redirige a un controlador de error personalizado si ocurre una excepción no controlada

// Habilitar CORS con la política configurada
app.UseCors("AllowLocalhost");

app.UseAuthorization();

app.MapControllers();

// Iniciar la aplicación
app.Run();
