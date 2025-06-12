using Infraestructura.Persistencia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Select(u => new { u.Id, u.Nombre })
                .ToListAsync();

            return Ok(usuarios);
        }
      
        [HttpGet("{usuarioId}/roles")]
        public async Task<IActionResult> ObtenerRolesAsignados(int usuarioId)
        {
            var roles = await _context.UsuarioRoles
                .Where(ur => ur.UsuarioId == usuarioId)
                .Include(ur => ur.Rol)
                .Select(ur => new { ur.Rol.Nombre })
                .ToListAsync();

            return Ok(roles);
        }

        [HttpDelete("{usuarioId}/roles/{nombreRol}")]
        public async Task<IActionResult> QuitarRol(int usuarioId, string nombreRol)
        {
            // ✅ Validación de emergencia (opcional, profesional)
            if (usuarioId == 1 && nombreRol == "Admin")
            {
                return BadRequest(new { detail = "No se puede quitar el rol Admin del usuario principal." });
            }

            //Esta asegura que "admin", "Admin " o " ADMIN" sean interpretados igual ✅
            var rol = await _context.Roles
                .FirstOrDefaultAsync(r => r.Nombre.ToLower() == nombreRol.ToLower().Trim());
            if (rol == null)
                return NotFound(new { detail = "Rol no encontrado" });

            var usuarioRol = await _context.UsuarioRoles
                .FirstOrDefaultAsync(ur => ur.UsuarioId == usuarioId && ur.RolId == rol.Id);

            if (usuarioRol == null)
                return NotFound(new { detail = "El usuario no tiene ese rol asignado" });

            _context.UsuarioRoles.Remove(usuarioRol);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpPost("{usuarioId}/roles/{nombreRol}")]
        public async Task<IActionResult> AsignarRol(int usuarioId, string nombreRol)
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario == null)
                return NotFound(new { detail = "Usuario no encontrado" });

            var rol = await _context.Roles
                .FirstOrDefaultAsync(r => r.Nombre.ToLower() == nombreRol.ToLower().Trim());
            if (rol == null)
                return NotFound(new { detail = "Rol no encontrado" });

            var yaAsignado = await _context.UsuarioRoles
                .AnyAsync(ur => ur.UsuarioId == usuarioId && ur.RolId == rol.Id);
            if (yaAsignado)
                return Conflict(new { detail = "El usuario ya tiene este rol asignado" });

            _context.UsuarioRoles.Add(new()
            {
                UsuarioId = usuarioId,
                RolId = rol.Id
            });

            await _context.SaveChangesAsync();
            return Ok();
        }


    }

}
