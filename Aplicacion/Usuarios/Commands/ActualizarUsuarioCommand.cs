using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace Aplicacion.Usuarios.Commands
{
    public class ActualizarUsuarioCommand : IRequest<Unit> // ✅ esta línea es CLAVE
    {
        // Pero lo inyectamos desde el controlador antes de enviar el comando

        [System.Text.Json.Serialization.JsonIgnore]

        public int Id { get; set; } // viene desde la URL (ruta)
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string? Password { get; set; } // << opcional

    }
}
