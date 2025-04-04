using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs
{
    public class FiltroCasosRequest
    {
        public int Pagina { get; set; } = 1;
        public int Tamanio { get; set; } = 10;
        public string? Estado { get; set; }       // "Abierto", "Cerrado", etc.
        public string? Buscar { get; set; }       // Título, cliente, etc.
        public string? Orden { get; set; }        // "fecha_desc", "titulo_asc", etc.
    }
}
