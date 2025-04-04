using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs
{
    public class ResultadoPaginado<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalRegistros { get; set; }
        public int Pagina { get; set; }
        public int Tamanio { get; set; }
    }
}
