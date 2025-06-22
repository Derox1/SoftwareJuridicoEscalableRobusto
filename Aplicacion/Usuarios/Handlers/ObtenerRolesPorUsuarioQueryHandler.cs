using Aplicacion.DTO;
using Aplicacion.Excepciones;
using Aplicacion.Repositorio;
using Aplicacion.Usuarios.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Usuarios.Handlers
{

    public class ObtenerRolesPorUsuarioQueryHandler : IRequestHandler <ObtenerRolesPorUsuarioQuery, List<RolAsignadoDto>>
    {
        //esto es  porque necesitamos conectar con la interfaz que implementa 
        private readonly IUsuarioRepositorio _repositorio;

        public ObtenerRolesPorUsuarioQueryHandler (IUsuarioRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<List<RolAsignadoDto>> Handle(ObtenerRolesPorUsuarioQuery request , CancellationToken cancellationToken)
        {
            // 🧠 Validación: ¿existe el usuario?
            var existeUsuario = await _repositorio.ExistePorIdAsync(request.UsuarioId);
            if (!existeUsuario)
                throw new NotFoundException("El usuario solicitado no existe en la base de datos.");

            // 📦 Obtener roles asignados
            var roles = await _repositorio.ObtenerRolesAsignadosAsync(request.UsuarioId);

            // 🧠 Validación extra (opcional): ¿tiene roles?
            if (roles == null || roles.Count == 0)
                throw new NotFoundException("El usuario no tiene roles asignados.");

            return roles;
        }
    }
}
