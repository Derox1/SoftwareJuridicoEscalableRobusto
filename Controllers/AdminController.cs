using Dominio.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infraestructura.Persistencia; // Asegúrate de tener esto

namespace API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context; // Paso 1

        public AdminController(AppDbContext context) // Paso 2
        {
            _context = context;
        }

        // 🔐 Solo usuarios con rol "Admin" pueden acceder
        [Authorize(Roles = "Admin")]
        [HttpGet("solo-admins")]
        public IActionResult SoloAdmins()
        {
            var username = User.Identity?.Name;
            return Ok($"🎉 Bienvenido {username}, accediste como Admin.");
        }



        //// ✅ ASIGNAR ROL A UN USUARIO (POST)

        //[Authorize(Roles = "Admin")]
        //[HttpPost("usuarios/{usuarioId}/roles")]
        //public async Task<IActionResult> AsignarRolAUsuario(int usuarioId, [FromBody] string nombreRol)
        //{
        //    var usuario = await _context.Usuarios
        //        .Include(u => u.UsuarioRoles)
        //        .FirstOrDefaultAsync(u => u.Id == usuarioId);

        //    if (usuario == null)
        //        return NotFound("Usuario no encontrado");

        //    var rol = await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == nombreRol);
        //    if (rol == null)
        //        return NotFound("Rol no válido");

        //    var yaTieneRol = usuario.UsuarioRoles.Any(ur => ur.RolId == rol.Id);
        //    if (yaTieneRol)
        //        return BadRequest("El usuario ya tiene este rol");

        //    usuario.UsuarioRoles.Add(new UsuarioRol
        //    {
        //        UsuarioId = usuario.Id,
        //        RolId = rol.Id
        //    });

        //    await _context.SaveChangesAsync();

        //    return Ok($"✅ Rol '{nombreRol}' asignado correctamente al usuario.");
        //}


        //// ✅ ESTE ES TU NUEVO MÉTODO, CORRECTAMENTE UBICADO
        //[Authorize(Roles = "Admin")]
        //[HttpGet("usuarios/{usuarioId}/roles")]
        //public async Task<IActionResult> ObtenerRolesDeUsuario(int usuarioId)
        //{
        //    var usuario = await _context.Usuarios
        //        .Include(u => u.UsuarioRoles)
        //        .ThenInclude(ur => ur.Rol)
        //        .FirstOrDefaultAsync(u => u.Id == usuarioId);

        //    if (usuario == null)
        //        return NotFound("Usuario no encontrado");

        //    var roles = usuario.UsuarioRoles
        //        .Select(ur => ur.Rol.Nombre)
        //        .ToList();

        //    return Ok(roles);



        //}
        //[Authorize(Roles = "Admin")]
        //[HttpDelete("usuarios/{usuarioId}/roles/{nombreRol}")]
        //public async Task<IActionResult> QuitarRolAUsuario(int usuarioId, [FromRoute] string nombreRol)
        //{
        //    var usuario = await _context.Usuarios
        //        .Include(u => u.UsuarioRoles)
        //        .FirstOrDefaultAsync(u => u.Id == usuarioId);

        //    if (usuario == null)
        //        return NotFound(new { detail = "Usuario no encontrado" });

        //    var rol = await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == nombreRol);
        //    if (rol == null)
        //        return NotFound(new { detail = "Rol no válido" });

        //    var relacion = usuario.UsuarioRoles.FirstOrDefault(ur => ur.RolId == rol.Id);
        //    if (relacion == null)
        //        return BadRequest(new { detail = "El usuario no tiene asignado ese rol" });

        //    _context.Remove(relacion);
        //    await _context.SaveChangesAsync();

        //    return Ok(new { mensaje = $"Rol '{nombreRol}' quitado correctamente del usuario." });
        //}

    }
}
    
