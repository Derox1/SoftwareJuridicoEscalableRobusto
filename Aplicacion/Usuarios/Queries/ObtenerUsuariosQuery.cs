using Aplicacion.Usuarios.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.Usuarios.Queries
{
  public class ObtenerUsuariosQuery : IRequest<List<UsuarioDto>>
    {

    }
}
    