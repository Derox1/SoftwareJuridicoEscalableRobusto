using Aplicacion.Repositorio;
using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Servicios
{
    public class EliminarCasoService
    {
        private readonly ICasoRepository _repositorio;

        public EliminarCasoService(ICasoRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task EjecutarAsync(int id)
        {
            var caso = await _repositorio.ObtenerPorIdAsync(id); // ← buscas el caso primero
            if (caso == null)
                throw new Exception("El caso no existe.");

            // Validación crítica: no permitir eliminar si está cerrado
            if (caso.Estado == EstadoCaso.Cerrado)
                throw new InvalidOperationException("No se puede eliminar un caso cerrado.");

            await _repositorio.EliminarAsync(caso); // ← pasas el objeto completo
        }
    }
}
