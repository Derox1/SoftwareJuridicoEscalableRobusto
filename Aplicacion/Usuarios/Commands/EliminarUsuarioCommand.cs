using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace Aplicacion.Usuarios.Commands
{
    public class EliminarUsuarioCommand : IRequest<Unit> // 👈 ESTA es la línea clave
    {
        public int UsuarioId { get; }

        public EliminarUsuarioCommand(int usuarioId)
        {
            UsuarioId = usuarioId;
        }
    }
}