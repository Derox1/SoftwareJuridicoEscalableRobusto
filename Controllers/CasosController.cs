using Aplicacion.Casos;
using Aplicacion.DTO;
using Aplicacion.DTOs;
using Aplicacion.Servicios.Casos;
using Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;
using Aplicacion.Repositorio;
using Aplicacion.Servicios;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CasosController : ControllerBase
    {
        private readonly ListarCasosService _listarCasosService;
        private readonly CrearCasoService _crearCasoService;
        private readonly CerrarCasoService _cerrarCasosService;
        private readonly ActualizarCasoService _actualizarCasoService;
        private readonly EliminarCasoService _eliminarCasoService;
        private readonly ICasoRepository _casoRepository;

        public CasosController(
            ListarCasosService listarCasosService,
            CrearCasoService crearCasosService,
            CerrarCasoService cerrarCasoService,
            ActualizarCasoService actualizarCasoService,
            EliminarCasoService eliminarCasoService,
            ICasoRepository casoRepository)
        {
            _listarCasosService = listarCasosService;
            _crearCasoService = crearCasosService;
            _actualizarCasoService = actualizarCasoService;
            _cerrarCasosService = cerrarCasoService;
            _eliminarCasoService = eliminarCasoService;
            _casoRepository = casoRepository;
        }

#if DEBUG
        // Endpoint solo para probar el middleware en desarrollo
        [HttpGet("error-test")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult LanzarError()
        {
            throw new Exception("🔥 Esto es una excepción de prueba");
        }
#endif

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerCasos([FromQuery] FiltroCasosRequest filtro)
        {
            var resultado = await _listarCasosService.EjecutarAsync(filtro);
            return Ok(resultado);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CrearCaso([FromBody] CrearCasoRequest request)
        {
            var resultado = await _crearCasoService.EjecutarAsync(request);
            return StatusCode(201);
        }

        [HttpPut("{id}/cerrar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CerrarCaso(int id)
        {
            var resultado = await _cerrarCasosService.EjecutarAsync(id);

            if (resultado != "caso cerrado correctamente")
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ActualizarCaso(int id, [FromBody] ActualizarCasoRequest request)
        {
            var resultado = await _actualizarCasoService.EjecutarAsync(id, request);

            if (resultado == "El caso no existe.")
                return NotFound(resultado);

            if (resultado == "No se puede editar un caso cerrado.")
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _eliminarCasoService.EjecutarAsync(id);
            return NoContent();
        }

        [HttpGet("conteo-casos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ConteoPorClienteDto>>> ObtenerConteoCasosPorCliente()
        {
            var resultado = await _casoRepository.ObtenerConteoCasosPorClienteAsync();
            return Ok(resultado);
        }
    }
}