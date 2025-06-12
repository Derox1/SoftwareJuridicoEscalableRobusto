using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Aplicacion.Servicios.Auth;
using Aplicacion.DTO;
using Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IJwtService _jwtService;
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context,  IJwtService jwtService)
    {
        _jwtService = jwtService;
        _context = context;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            /* aqui usamos EF CORE YA QUE ES CONSULTA SIMPLE */
            var usuario = await _context.Usuarios
                .Include(u => u.UsuarioRoles)
                    .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.Email == dto.Email && u.Contraseña == dto.Password);

            if (usuario == null)
                return Unauthorized("Credenciales inválidas");

            // Obtener roles desde la relación UsuarioRoles
            var roles = usuario.UsuarioRoles.Select(ur => ur.Rol.Nombre).ToList();

            // Generar token con los nuevos parámetros
            var token = _jwtService.GenerarToken(usuario.Email, usuario.Id, roles);

            return Ok(new { token });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno: {ex.Message}");
        }
    }
}
