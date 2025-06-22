using API.Aplicacion.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
namespace Aplicacion.Usuarios.Commands 
{
    // Devuelve el ID creado

    public class CrearUsuarioCommand : IRequest<int>
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}





