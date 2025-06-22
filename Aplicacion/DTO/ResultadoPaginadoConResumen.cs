using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTO
{
    public class ResultadoPaginadoConResumen<T>
    {
        public List<T> Items { get; set; }
        public int TotalRegistros { get; set; }
        public int Pagina { get; set; }
        public int Tamanio { get; set; }
        public int TotalPaginas { get; set; } //paginado en pantalla 

        public ResumenCasosDto Resumen { get; set; } // <- NUEVO
    }
}
