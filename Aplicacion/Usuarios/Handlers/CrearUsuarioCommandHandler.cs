using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTOs;
using Aplicacion.Servicios;
using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Aplicacion.Repositorio;
using Aplicacion.Servicios.Auth;
using Aplicacion.Usuarios.Commands;


namespace Aplicacion.Usuarios.Handlers
{
    // Este handler procesa el comando CrearUsuarioCommand y devuelve un int (el Id del nuevo usuario)
    public class CrearUsuarioCommandHandler : IRequestHandler<CrearUsuarioCommand, int>
    {
        // Inyectamos el repositorio que nos permite acceder a la base de datos
        private readonly IUsuarioRepositorio _repositorio;

        // Inyectamos el servicio que nos permite hashear la contraseña
        private readonly IHashService _hashService;



        // Constructor: el framework inyectará las dependencias configuradas en Program.cs
        public CrearUsuarioCommandHandler(IUsuarioRepositorio repositorio, IHashService hashService)
        {
            _repositorio = repositorio;
            _hashService = hashService;
        }


        // Este método se ejecuta cuando se llama a Mediator.Send(new CrearUsuarioCommand(...))
        public async Task<int> Handle(CrearUsuarioCommand request, CancellationToken cancellationToken)
        {
            // Paso 1: Validar si el email ya existe en la base de datos
            var existe = await _repositorio.ExistePorEmailAsync(request.Email);
            if (existe)
                throw new Exception("El correo ya está registrado."); // Idealmente lanzarías una excepción personalizada

            // Paso 2: Hashear la contraseña antes de guardarla (nunca guardar texto plano)
            var passwordHash = _hashService.Hash(request.Password);

            // Paso 3: Crear la entidad de dominio Usuario con los datos recibidos
            var nuevoUsuario = new Usuario(
                nombre: request.Nombre,
                email: request.Email,
                passwordHash: passwordHash
            );
            // Asignar roles si vienen en la lista
            //if (request.RolesId != null && request.RolesId.Any())
            //{
            //    foreach (var rolId in request.RolesId)
            //    {
            //        nuevoUsuario.UsuarioRoles.Add(new UsuarioRol
            //        {
            //            Usuario = nuevoUsuario,
            //            RolId = rolId
            //        });
            //    }
            //}

            await _repositorio.CrearUsuarioConRolesAsync(nuevoUsuario);



            // Paso 5: Devolver el ID generado del nuevo usuario
            return nuevoUsuario.Id;
        }

    }
}
