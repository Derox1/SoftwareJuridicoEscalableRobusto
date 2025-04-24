using Aplicacion.DTO;
using Aplicacion.DTOs;
using Aplicacion.Repositorio;
using Aplicacion.Servicios;
using Dominio.Entidades;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Aplicacion.Casos
{

    /*This method follows a clear business workflow: validate input, format data,}
     check for duplicates, create missing dependencies, persist the main entity, 
    and log everything. It’s simple, maintainable, and respects business rules.
    */


    /*Este método sigue un flujo claro: validar, formatear, evitar duplicados, 
     * crear cliente si no existe, guardar el caso y registrar logs. Es limpio, 
     * mantenible y respeta las reglas del negocio.*/

    public class CrearCasoService  
        {
        private readonly ICasoRepository _casoRepository;
        private readonly FormateadorNombreService _formateador;
        private readonly IClienteRepository _clienteRepository;
        private readonly ILogger<CrearCasoService> _logger;

        public CrearCasoService(ICasoRepository casoRepository,IClienteRepository clienteRepository, FormateadorNombreService formateador, ILogger<CrearCasoService> logger)
            {
                _casoRepository = casoRepository;
                _formateador = formateador;
            _clienteRepository = clienteRepository; // 🔥 ESTA LÍNEA ES LA CLAVE
            _logger = logger;
        }

        //APLICA PATRON RESTFUL ESTA LINEA YA QUE devuelve
        //datos reales en pantalla
        public async Task<CasoDto> EjecutarAsync(CrearCasoRequest request)
        {
            try
            {
                // 1. Validaciones
                if (string.IsNullOrWhiteSpace(request.Titulo))
                    throw new ArgumentException("El título del caso es obligatorio.");

                if (string.IsNullOrWhiteSpace(request.NombreCliente))
                    throw new ArgumentException("El nombre del cliente es obligatorio.");

                _logger.LogInformation("🟢 Iniciando creación de caso para cliente: {Cliente}", request.NombreCliente);

                // 2. Formateo del nomnbre

                var nombreFormateado = _formateador.Formatear(request.NombreCliente);
                _logger.LogDebug(" Nombre cliente formateado: {Nombre}", nombreFormateado);

                // 3. Validar casos activos
                 var existentes = await _casoRepository.ObtenerTodosAsync();
                  if (existentes.Any(c => c.NombreCliente == request.NombreCliente && c.Estado != EstadoCaso.Cerrado
))
                throw new InvalidOperationException("Ya existe un caso activo para ese cliente.");
             
                // 5. Buscar o crear cliente
                var cliente = await _clienteRepository.ObtenerPorNombreAsync(nombreFormateado);

                //validamos o creamos un ciente
                if (cliente is null)
                {
                    _logger.LogInformation("Cliente no encontrado. Se crea nuevo cliente: {Cliente}", nombreFormateado);

                    cliente = new Cliente { Nombre = nombreFormateado };
                    await _clienteRepository.CrearAsync(cliente);
                }


                //6. Crear caso
                var nuevoCaso = new Caso
                {
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    NombreCliente = nombreFormateado,
                    TipoCaso = request.TipoCaso,
                    FechaCreacion = DateTime.UtcNow,
                    Estado = EstadoCaso.Pendiente,
                    ClienteId = cliente.Id
                };
                await _casoRepository.CrearAsync(nuevoCaso);
                _logger.LogInformation(" Caso creado exitosamente con ID: {CasoId}", nuevoCaso.Id);
               
                // 6. Retornar DTO
                return new CasoDto
                {
                    Id = nuevoCaso.Id,
                    Titulo = nuevoCaso.Titulo,
                    Estado = nuevoCaso.Estado,
                    FechaCreacion = nuevoCaso.FechaCreacion,
                    NombreCliente = cliente.Nombre,
                    TipoCaso = nuevoCaso.TipoCaso
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error al crear caso.");
                throw;
            }

           

        }

    }

}

