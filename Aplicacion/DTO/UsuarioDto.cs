using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Usuarios.DTO
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<string> Roles { get; set; } = new();
    }
}
