using Aplicacion.Repositorio;
using Aplicacion.Usuarios.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Aplicacion.Usuarios.Queries
{
    public class ObtenerUsuariosQueryHandler : IRequestHandler<ObtenerUsuariosQuery, List<UsuarioDto>>
    {
        private readonly IUsuarioRepositorio _repositorio;

        public ObtenerUsuariosQueryHandler(IUsuarioRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<List<UsuarioDto>> Handle(ObtenerUsuariosQuery request, CancellationToken cancellationToken)
        {
            return await _repositorio.ObtenerUsuariosConRolesAsync(cancellationToken);
        }
    }
}
