using Aplicacion.Casos;
using Aplicacion.DTO;
using Aplicacion.DTOs;
using Aplicacion.Servicios.Casos;
using Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;
using Aplicacion.Repositorio;
using Aplicacion.Servicios;

namespace API.Controllers
{
    //
    [ApiController]
    [Route("api/[controller]")]
    public class CasosController : ControllerBase
    {
        private readonly ListarCasosService _listarCasosService;
        private readonly CrearCasoService _crearCasoService;
        private readonly CerrarCasoService _cerrarCasosService;
        private readonly ActualizarCasoService _actualizarCasoService;
        private readonly EliminarCasoService _eliminarCasoService;


        public CasosController(ListarCasosService listarCasosService,
                              CrearCasoService crearCasosService,
                              CerrarCasoService cerrarCasoService,
                              ActualizarCasoService actualizarCasoService,
                              EliminarCasoService eliminarCasoService)
        {
            _listarCasosService = listarCasosService;
            _crearCasoService = crearCasosService;
            _actualizarCasoService = actualizarCasoService;
            _cerrarCasosService = cerrarCasoService;
            _eliminarCasoService = eliminarCasoService;
        }
        // GET /api/casos

        [HttpGet]
        public async Task<ActionResult<List<Caso>>> ObtenerCasos()
        {
            var casos = await _listarCasosService.EjecutarAsync();
            return Ok(casos);
        }

        // POST /api/casos
        [HttpPost]
        public async Task<IActionResult> CrearCaso([FromBody] CrearCasoRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Titulo) || string.IsNullOrWhiteSpace(request.NombreCliente))
                return BadRequest("El título y el nombre del cliente son obligatorios.");
           //validaciones de entrada del cliente antes de pasar 
            if (string.IsNullOrWhiteSpace(request.Descripcion) || (request.Descripcion.Length < 10))
                return BadRequest("La descripción debe tener al menos 10 caracteres.");
            var resultado = await _crearCasoService.EjecutarAsync(request);
            return StatusCode(201);
          
         }
        // PUT /api/casos/{id}/cerrar
        [HttpPut("{id}/cerrar")]
        public async Task<IActionResult>CerrarCaso(int id)
        {
            try
            {
                var resultado = await _cerrarCasosService.EjecutarAsync(id);

                if (resultado != "caso cerrado correctamente") ;
                return BadRequest();
            }
            catch (InvalidOperationException ex )
            {
                return NotFound(ex.Message); // "El caso no existe."    
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCaso(int id, [FromBody] ActualizarCasoRequest request)
        {
            try
            {
                var resultado = await _actualizarCasoService.EjecutarAsync(id, request);

                if (resultado == "El caso no existe.")
                    return NotFound(resultado);

                if (resultado == "No se puede editar un caso cerrado.")
                    return BadRequest(resultado);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el caso: {ex.Message}");
            }
        }
      
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _eliminarCasoService.EjecutarAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
        }
     }
}
