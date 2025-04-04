using Aplicacion.DTO;
using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace Aplicacion.Repositorio
    {
        public interface ICasoRepository
        {
            Task CrearAsync(Caso nuevoCaso);
            Task<Caso> ObtenerPorIdAsync(int casoId);
            Task<IEnumerable<Caso>> ObtenerTodosAsync();
            Task ActualizarAsync(Caso caso);
            Task<Caso?> ObtenerPorId(int id);
            Task EliminarAsync(Caso caso);
        Task<List<ConteoPorClienteDto>> ObtenerConteoCasosPorClienteAsync();
        IQueryable<Caso> ObtenerQueryable();



    }
}


