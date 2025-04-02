using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio.Entidades;

namespace Aplicacion.Repositorio
{
    public interface IClienteRepository
    {
        Task<Cliente?> ObtenerPorNombreAsync(string nombre);
        Task CrearAsync(Cliente cliente);
    }
}
