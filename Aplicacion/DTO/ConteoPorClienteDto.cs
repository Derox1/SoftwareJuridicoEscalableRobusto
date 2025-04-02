using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTO
{
    public class ConteoPorClienteDto
    {
        public int ClienteId { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public int CantidadCasos { get; set; }
    }
}
