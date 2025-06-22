using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Entidades;
using System.Threading.Tasks;

namespace Aplicacion.Repositorio
{
    public interface IRolRepositorio
    {
        Task<Rol?> ObtenerPorNombreAsync(string nombre);
    }
}
