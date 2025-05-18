using Aplicacion.DTOs;
using Aplicacion.Repositorio;
using Dominio.Entidades;

namespace Aplicacion.Servicios.Casos
{
    public class ActualizarCasoService
    {

        private readonly ICasoRepository _casoRepository;
        private readonly FormateadorNombreService _formateador;

        public ActualizarCasoService(ICasoRepository casoRepository, FormateadorNombreService formateador)
        {
            _casoRepository = casoRepository;
            _formateador = formateador;
        }

        public async Task<string> EjecutarAsync(int id, ActualizarCasoRequest request)
        {
            var caso = await _casoRepository.ObtenerPorIdAsync(id);
            if (caso is null)
                return "El caso no existe.";

            if (caso.Estado == EstadoCaso.Cerrado)
                return "No se puede editar un caso cerrado.";

            // Validar transición de estado (si el estado viene desde el request)
            //if (!TransicionValida(caso.Estado, request.Estado))
            //    return "La transición de estado no está permitida.";

            caso.Titulo = request.Titulo;
            caso.Descripcion = request.Descripcion;
            if (caso.Estado == EstadoCaso.Pendiente)
            {
                caso.Estado = EstadoCaso.EnProceso;
                caso.FechaCambioEstado = DateTime.UtcNow;
            }
            // Solo si cambia el estado, guarda la transición y la fecha
            //if (caso.Estado != request.Estado)
            //{
            //    caso.Estado = request.Estado;
            //    caso.FechaCambioEstado = DateTime.UtcNow;
            //}

            await _casoRepository.ActualizarAsync(caso);
            return "Caso actualizado correctamente.";
        }

        //private bool TransicionValida(EstadoCaso actual, EstadoCaso nuevo)
        //{
        //    return
        //        actual == nuevo
        //        || (actual == EstadoCaso.Pendiente && nuevo == EstadoCaso.EnProceso)
        //        || (actual == EstadoCaso.EnProceso && nuevo == EstadoCaso.Cerrado)
        //        || (actual == EstadoCaso.Pendiente && nuevo == EstadoCaso.Cerrado); // si decides permitirlo desde acá también
        //}
    }
}
