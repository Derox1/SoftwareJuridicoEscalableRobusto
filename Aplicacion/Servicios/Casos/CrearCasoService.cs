using Aplicacion.DTO;
using Aplicacion.Repositorio;
using Aplicacion.Servicios;
using Dominio.Entidades;
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

        public CrearCasoService(ICasoRepository casoRepository, FormateadorNombreService formateador)
            {
                _casoRepository = casoRepository;
                _formateador = formateador;
            }

        public async Task<string> EjecutarAsync(CrearCasoRequest request)
        {
            var nombreFormateado = _formateador.Formatear(request.NombreCliente);

                var existentes = await _casoRepository.ObtenerTodosAsync();

                if (existentes.Any(c => c.NombreCliente == request.NombreCliente && c.Estado != "Cerrado"))
                    return "Ya existe un caso activo para ese cliente.";


            var nuevoCaso = new Caso
            {
                Titulo = request.Titulo,
                Descripcion = request.Descripcion,
                NombreCliente = nombreFormateado,
                FechaCreacion = DateTime.UtcNow,
                Estado = "Pendiente"
            };

                await _casoRepository.CrearAsync(nuevoCaso);

                return "Caso creado exitosamente.";
            }
        }
    }

