using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Servicios.Auth
{

    public interface IHashService
    {
        string Hash(string textoPlano);
        bool Verificar(string textoPlano, string hashAlmacenado);

    }
}
