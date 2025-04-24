using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTOs
{
    //solo lo que el cliente pueda modificar
    public class ActualizarCasoRequest
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public TipoCaso TipoCaso { get; set; }
    }
}
