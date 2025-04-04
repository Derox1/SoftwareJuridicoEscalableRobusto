using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs;

namespace Aplicacion.DTOs
{
    public class CasoDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
    }
}
