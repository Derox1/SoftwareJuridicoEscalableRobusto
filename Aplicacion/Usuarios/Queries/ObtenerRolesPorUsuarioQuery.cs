using Aplicacion.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Usuarios.Queries
{
        public record ObtenerRolesPorUsuarioQuery(int UsuarioId) : IRequest<List<RolAsignadoDto>>;
}
