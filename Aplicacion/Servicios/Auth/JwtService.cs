using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Aplicacion.Servicios.Auth
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config; // Se guarda la configuración que llega por inyección
        }

        // Este método genera un token JWT válido para el usuario y el rol dados.
        public string GenerarToken(string email, string rol)
        {
            // 1. Obtener configuración JWT desde appsettings.json
            var jwt = _config.GetSection("Jwt");

            // 2. Crear la clave secreta para firmar el token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]));

            // 3. Configurar cómo se firma el token (algoritmo)
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 4. Agregar los "claims" personalizados al token (info del usuario)
            var claims = new[]
            {
            new Claim(ClaimTypes.Email, email),     // Quién es el usuario
            new Claim(ClaimTypes.Role, rol)           // Qué rol tiene (Admin, Cliente, etc)
        };

            // 5. Crear el objeto JWT con toda la información configurada
            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],                        // Quién genera el token
                audience: jwt["Audience"],                    // Para quién es válido el token
                claims: claims,                               // Claims definidos arriba
                expires: DateTime.Now.AddMinutes(
                    double.Parse(jwt["ExpiresInMinutes"])),   // Tiempo de expiración
                signingCredentials: creds                     // Firma con clave secreta
            );

            // 6. Serializar el token a texto plano (formato JWT que se envía al frontend)
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
