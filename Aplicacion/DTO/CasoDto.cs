using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs;
using Dominio.Entidades;

namespace Aplicacion.DTOs
{
    public class CasoDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public EstadoCaso Estado { get; set; } = EstadoCaso.Pendiente;
        public DateTime FechaCreacion { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public TipoCaso TipoCaso { get; set; }  // ¡Agrega este campo!

    }
}
