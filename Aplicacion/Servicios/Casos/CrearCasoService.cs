using Aplicacion.DTO;
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

        public async Task<string> EjecutarAsync(CrearCasoRequest request)
        {
            _logger.LogInformation("🟢 Iniciando creación de caso para cliente: {Cliente}", request.NombreCliente);

            var nombreFormateado = _formateador.Formatear(request.NombreCliente);
            _logger.LogDebug(" Nombre cliente formateado: {Nombre}", nombreFormateado);

            var existentes = await _casoRepository.ObtenerTodosAsync();

                if (existentes.Any(c => c.NombreCliente == request.NombreCliente && c.Estado != "Cerrado"))
                    return "Ya existe un caso activo para ese cliente.";
            var cliente = await _clienteRepository.ObtenerPorNombreAsync(nombreFormateado);

            //validamos o creamos un ciente
            if (cliente is null)
            {
                _logger.LogInformation("Cliente no encontrado. Se crea nuevo cliente: {Cliente}", nombreFormateado);

                cliente = new Cliente { Nombre = nombreFormateado };
                await _clienteRepository.CrearAsync(cliente);
            }

            var nuevoCaso = new Caso
            {
                Titulo = request.Titulo,
                Descripcion = request.Descripcion,
                NombreCliente = nombreFormateado,
                FechaCreacion = DateTime.UtcNow,
                Estado = "Pendiente",
                ClienteId = cliente.Id
            };

                await _casoRepository.CrearAsync(nuevoCaso);
                _logger.LogInformation("✅ Caso creado exitosamente con ID: {CasoId}", nuevoCaso.Id);

                return "Caso creado exitosamente.";
            }
        }
    }

