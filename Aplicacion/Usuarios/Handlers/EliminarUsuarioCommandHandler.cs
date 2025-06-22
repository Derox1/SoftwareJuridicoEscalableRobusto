using Aplicacion.Excepciones;
using Aplicacion.Repositorio;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Usuarios.Commands
{

        public class EliminarUsuarioCommandHandler : IRequestHandler<EliminarUsuarioCommand, Unit>
        {
            private readonly IUsuarioRepositorio _repositorio;

            public EliminarUsuarioCommandHandler(IUsuarioRepositorio repositorio)
            {
                _repositorio = repositorio;
            }

            public async Task<Unit> Handle(EliminarUsuarioCommand request, CancellationToken cancellationToken)
            {
                var usuario = await _repositorio.ObtenerPorIdAsync(request.UsuarioId);
                if (usuario == null)
                    throw new Exception("Usuario no encontrado");

                await _repositorio.EliminarAsync(usuario);
                await _repositorio.GuardarCambiosAsync();

                return Unit.Value;
            }
        }

}
