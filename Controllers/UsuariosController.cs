using Aplicacion.Excepciones;
using Aplicacion.Usuarios.Commands;
using Aplicacion.Usuarios.Queries;
using Infraestructura.Persistencia;
using MediatR;
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

        //invocamos mediator para la contraseña 
        private readonly IMediator _mediator;


        public UsuariosController(AppDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<IActionResult> ObtenerUsuarios()
        {
            var resultado = await _mediator.Send(new ObtenerUsuariosQuery());
            return Ok(resultado);
        }


        [HttpGet("{usuarioId}/roles")]
        public async Task<IActionResult> ObtenerRolesAsignados(int usuarioId)
        {
            /*// 🧠 Se utiliza MediatR para enviar una Query que representa la solicitud de obtener roles.
            // Esto delega la responsabilidad al Handler correspondiente (en la capa de Aplicación).*/
            //throw new NotFoundException("Este usuario no existe en la base de datos (prueba).");
            var roles = await _mediator.Send(new ObtenerRolesPorUsuarioQuery(usuarioId));
            return Ok(roles);
        }


        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CrearUsuario([FromBody] CrearUsuarioCommand comando)
        {
            try
            {
                var id = await _mediator.Send(comando);
                return CreatedAtAction(nameof(CrearUsuario), new { id = id }, new { id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { detail = "❌ Error al crear usuario desde el controlador", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] ActualizarUsuarioCommand comando)
        {
            comando.Id = id; // Asigna el ID que vino por ruta
            await _mediator.Send(comando);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            await _mediator.Send(new EliminarUsuarioCommand(id));
            return NoContent();
        }


    }

}
