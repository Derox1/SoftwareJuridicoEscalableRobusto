using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace Aplicacion.Usuarios.Commands
{
    public class AsignarRolAUsuarioCommand : IRequest<Unit>
    {
        public int UsuarioId { get; set; }
        public string NombreRol { get; set; } = string.Empty;
    }
}
