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
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string NombreCliente { get; set; } = string.Empty;
        public TipoCaso TipoCaso { get; set; } 
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public EstadoCaso Estado { get; set; } = EstadoCaso.Pendiente;
        public Cliente Cliente { get; set; } = null!;  // navegación
        public int ClienteId { get; set; }

        // Validación de estados
        public bool PuedeSerCerrado() => Estado == EstadoCaso.EnProceso;
        public bool EstaActivo() => Estado != EstadoCaso.Cerrado;
        public bool EstaCerrado() => Estado == EstadoCaso.Cerrado;
    }
}
