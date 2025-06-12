using Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;
using Aplicacion.Repositorio;
using Infraestructura.Repositorios;
using Aplicacion.Servicios.Casos;
using Aplicacion.Servicios;
using Aplicacion.Casos;
using FluentValidation.AspNetCore;
using Aplicacion.Validaciones;
using API.Middlewares;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using Aplicacion.Servicios.Auth;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using System.Security.Claims;



//Configuración de Servicios (DI)
var builder = WebApplication.CreateBuilder(args);

//aqui permitimos 
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


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
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddHttpContextAccessor();



//🔹 Validaciones (FluentValidation)
builder.Services.AddControllers()
.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CrearCasoRequestValidator>())
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations(); //  Esto es clave

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Jurídica",
        Version = "v1",
        Description = "Documentación oficial de la API"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ejemplo: Bearer {tu_token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }

    });
    c.UseInlineDefinitionsForEnums(); //Esto activa los enums como dropdown en Swagger

});


// 1. CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:7266") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Autenticación con JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"])),
            // ✅ Esta línea es la clave
            NameClaimType = ClaimTypes.Name,
            RoleClaimType = ClaimTypes.Role
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});


builder.Services.AddEndpointsApiExplorer(); // Necesario para Swagger



var app = builder.Build();
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Jurídica v1");
    c.DocumentTitle = "Documentación API Jurídica";
    c.RoutePrefix = "swagger"; // <- esto asegura que cargue en /swagger
});
app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseDefaultFiles();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("PermitirFrontend"); // ESTO ACTIVA CORS
app.UseAuthentication(); // JWT primero
app.UseAuthorization();
app.MapControllers();

app.Run();
