using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Servicios
{
    public class FormateadorNombreService
    {
        public string Formatear(string nombre)
        {
            return nombre.Trim().ToUpper();
        }
    }
}
