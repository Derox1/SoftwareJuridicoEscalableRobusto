using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTO
{


    //solo ingresamos propiedades del caso que se ingresen desde el cliente
    //resto de informacion se ingresa automaticamente 
    public class CrearCasoRequest
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion {  get; set; } = string.Empty;
        public string NombreCliente { get; set; } = string.Empty;
    }
}
