using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Contraseña { get; set; } = string.Empty;
        public string Rol { get; set; } = "Cliente";

        // Validaciones específicas (puedes usar esto más adelante en reglas de negocio)
        public bool EsAdmin() => Rol.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        public bool EsAbogado() => Rol.Equals("Abogado", StringComparison.OrdinalIgnoreCase);
        public bool EsCliente() => Rol.Equals("Cliente", StringComparison.OrdinalIgnoreCase);

    }
}
