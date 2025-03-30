using Aplicacion.Repositorio;
using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Servicios.Casos
{
    public class ListarCasosService
    {
        private readonly ICasoRepository _casoRepository;
        public ListarCasosService(ICasoRepository casoRepository)
        {
            _casoRepository = casoRepository;
        }
        public async Task<IEnumerable<Caso>> EjecutarAsync()
        {
            return await _casoRepository.ObtenerTodosAsync();
        }

    }
}
