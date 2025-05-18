using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTO
{
    public class ResumenCasosDto
    {
        public int Total { get; set; }
        public int Pendientes { get; set; }
        public int Resueltos { get; set; }
    }
}
