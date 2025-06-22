using Aplicacion.Repositorio;
using Aplicacion.Servicios.Auth;
using Aplicacion.Usuarios.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Usuarios.Handlers
{
    public class ActualizarUsuarioCommandHandler : IRequestHandler<ActualizarUsuarioCommand, Unit>
    {

        private readonly IUsuarioRepositorio _repositorio;
        private readonly IHashService _hashService;

        public ActualizarUsuarioCommandHandler(IUsuarioRepositorio repositorio, IHashService hashService)
        {
            _repositorio = repositorio;
            _hashService = hashService;

        }
        public async Task<Unit> Handle(ActualizarUsuarioCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _repositorio.ObtenerPorIdAsync(request.Id);
            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            usuario.Nombre = request.Nombre;
            usuario.Email = request.Email;

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                usuario.PasswordHash = _hashService.Hash(request.Password);
            }


            await _repositorio.GuardarCambiosAsync();

            return Unit.Value;
        }

    }
}
