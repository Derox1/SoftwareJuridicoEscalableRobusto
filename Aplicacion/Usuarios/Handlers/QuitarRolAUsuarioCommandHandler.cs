using Aplicacion.Repositorio;
using Aplicacion.Usuarios.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Usuarios.Handlers
{
    public class QuitarRolAUsuarioCommandHandler : IRequestHandler<QuitarRolAUsuarioCommand, Unit>
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IRolRepositorio _rolRepositorio;
        public QuitarRolAUsuarioCommandHandler(IUsuarioRepositorio usuarioRepositorio, IRolRepositorio rolRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _rolRepositorio = rolRepositorio;
        }

        public async Task<Unit> Handle(QuitarRolAUsuarioCommand request, CancellationToken cancellationToken)
        {
            // Validación de seguridad para el usuario Admin principal
            if (request.UsuarioId == 1 && request.NombreRol.Trim().ToLower() == "admin")
            {
                throw new InvalidOperationException("No se puede quitar el rol Admin del usuario principal.");
            }

            // Buscar el rol por nombre (normalizado)
            var rol = await _rolRepositorio.ObtenerPorNombreAsync(request.NombreRol.Trim());
            if (rol == null)
            {
                throw new KeyNotFoundException("Rol no encontrado.");
            }

            // Verificar si el usuario tiene ese rol
            var usuario = await _usuarioRepositorio.ObtenerPorIdAsync(request.UsuarioId);
            if (usuario == null)
            {
                throw new KeyNotFoundException("Usuario no encontrado.");
            }

            var usuarioRol = usuario.UsuarioRoles.FirstOrDefault(ur => ur.RolId == rol.Id);
            if (usuarioRol == null)
            {
                throw new InvalidOperationException("El usuario no tiene ese rol asignado.");
            }

            // Eliminar el rol asignado
            usuario.UsuarioRoles.Remove(usuarioRol);

            // Guardar cambios
            await _usuarioRepositorio.GuardarCambiosAsync();

            return Unit.Value;
        }


    }

}
