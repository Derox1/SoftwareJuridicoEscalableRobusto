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

            if (caso.Estado == "Cerrado")
                return "No se puede editar un caso cerrado.";

            caso.Titulo = request.Titulo;
            caso.Descripcion = request.Descripcion;
            caso.NombreCliente = _formateador.Formatear(request.NombreCliente);

            await _casoRepository.ActualizarAsync(caso);

            return "Caso actualizado correctamente.";
        }
    }
}
