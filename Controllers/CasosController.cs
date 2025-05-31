using Aplicacion.Casos;
using Aplicacion.DTO;
using Aplicacion.DTOs;
using Aplicacion.Servicios.Casos;
using Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;
using Aplicacion.Repositorio;
using Aplicacion.Servicios;
using Microsoft.AspNetCore.Http;
using API.Helpers;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
        // Endpoint solo para probar el middleware en desarrollo
        [HttpGet("error-test")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult LanzarError()
        {
            throw new Exception("🔥 Esto es una excepción de prueba");
        }
#endif


        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerCasos([FromQuery] FiltroCasosRequest filtro)
        {
            var resultado = await _listarCasosService.EjecutarAsync(filtro);

            if (resultado == null || resultado.Items == null || !resultado.Items.Any())
            {
                return NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = "Casos no encontrados",
                    Detail = "No se encontraron casos con los filtros aplicados.",
                    Instance = HttpContext.Request.Path
                });
            }

            return Ok(resultado);
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerCasoPorId(int id)
        {
            var caso = await _casoRepository.ObtenerPorIdAsync(id);

            if (caso == null)
            {
                return NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = "Caso no encontrado",
                    Detail = $"No se encontró un caso con ID {id}.",
                    Instance = HttpContext.Request.Path
                });
            }

            var dto = new CasoDto
            {
                Id = caso.Id,
                Titulo = caso.Titulo,
                Estado = caso.Estado,
                FechaCreacion = caso.FechaCreacion,
                NombreCliente = caso.NombreCliente,
                TipoCaso = caso.TipoCaso,
                Descripcion = caso.Descripcion
            };

            return Ok(dto);
        }


        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CrearCaso([FromBody] CrearCasoRequest request)
        {
            var nuevoCaso = await _crearCasoService.EjecutarAsync(request);
            return CreatedAtAction(nameof(ObtenerCasoPorId), new { id = nuevoCaso.Id }, nuevoCaso.Id);
        }







        [Authorize]
        [HttpPut("{id}/cerrar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CerrarCaso(int id, [FromBody] CerrarCasoRequest request)
        {
            var resultado = await _cerrarCasosService.EjecutarAsync(id, request);

            if (resultado.NoEncontrado)
                return NotFound(ApiError.NotFound(resultado.Mensaje!, HttpContext));

            if (!resultado.Exito && resultado.EsErrorNegocio)
                return BadRequest(ApiError.BadRequest(resultado.Mensaje!, HttpContext));

            return Ok(resultado);
        }






        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CasoDto>> ActualizarCaso(int id, [FromBody] ActualizarCasoRequest request)
        {
            try
            {
                var resultado = await _actualizarCasoService.EjecutarAsync(id, request);

                //AQUI USAMOS PROBLEMDETAILS para compatibilidad con swagger 
                //compatibilidad con postman 
                //resultados reales esperados 
                if (resultado == null)
                    return NotFound(ApiError.NotFound($"No existe un caso con ID {id}.", HttpContext));
                // ✅ Caso actualizado correctamente

                return Ok(resultado);



                //AQUI USAMOS PROBLEMDETAILS para compatibilidad con swagger 
                //compatibilidad con postman 
                //resultados reales esperados 



                //return NotFound(new ProblemDetails

                //{

                //    Title = "Caso no encontrado",


                //    Detail = $"No existe un caso con ID {id}.",

                //    Status = 404,

                //    Instance = HttpContext.Request.Path

                //});

                //return Ok(resultado);
            }
            catch (InvalidOperationException ex)
            {
                // Devolvemos error 400 RESTful si no se puede actualizar (regla de negocio)

                return BadRequest(ApiError.BadRequest(ex.Message, HttpContext));

            }
        }


        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _eliminarCasoService.EjecutarAsync(id);
            return NoContent();
        }



        [Authorize]
        [HttpGet("conteo-casos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ConteoPorClienteDto>>> ObtenerConteoCasosPorCliente()
        {
            var resultado = await _casoRepository.ObtenerConteoCasosPorClienteAsync();
            return Ok(resultado);
        }


        [Authorize]
        [HttpGet("estado/{estado}")]
        public async Task<IActionResult> GetPorEstado(string estado)
        {
            if(!Enum.TryParse<EstadoCaso>(estado, true, out var estadoEnum))
            {
                return BadRequest("Estado invalido.");
            }

            var lista = await _casoRepository.ObtenerPorEstadoAsync(estadoEnum);

            return Ok(lista.Select(c => new CasoDto
            {
                Id = c.Id,
                Titulo = c.Titulo,
                Estado = c.Estado,
                TipoCaso =c.TipoCaso,
                FechaCreacion = c.FechaCreacion,
                NombreCliente = c.NombreCliente
            }));
               
            }
        }
    }
