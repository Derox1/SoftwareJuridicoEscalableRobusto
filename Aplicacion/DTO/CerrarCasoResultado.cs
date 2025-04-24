using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTO
{
    public class CerrarCasoResultado
    {
        public bool Exito { get; set; }
        public string? Mensaje { get; set; }
        public bool EsErrorNegocio { get; set; } // true si es un error esperable tipo "ya estaba cerrado"
        public bool NoEncontrado { get; set; }   // true si no existe el caso
    }
}
