using Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;
using Aplicacion.Repositorio;
using Infraestructura.Repositorios;
using Aplicacion.Servicios.Casos;
using Aplicacion.Servicios;
using Aplicacion.Casos;


var builder = WebApplication.CreateBuilder(args);


//Conexion a la base de datos 
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ICasoRepository, CasoRepository>();
builder.Services.AddScoped<ListarCasosService>();
builder.Services.AddScoped<ActualizarCasoService>();
builder.Services.AddScoped<FormateadorNombreService>();
builder.Services.AddScoped<CrearCasoService>();
builder.Services.AddScoped<CerrarCasoService>();
builder.Services.AddScoped<EliminarCasoService>();






// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
