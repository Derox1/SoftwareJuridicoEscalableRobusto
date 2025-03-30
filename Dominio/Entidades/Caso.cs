using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Caso
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public string TipoCaso { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public string Estado { get; set; } = "Pendiente"; // "Pendiente", "En Proceso", "Cerrado"

        // Validación de estados
        public bool PuedeSerCerrado() => Estado == "En Proceso";
        public bool EstaActivo() => Estado != "Cerrado";
        public bool EstaCerrado() => Estado == "En Proceso";
    }
}
