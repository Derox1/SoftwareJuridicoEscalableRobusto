using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Usuario
    {
        public int Id { get; set; }


        [Required, StringLength(100)]

        public string Nombre { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;



        // Validaciones específicas (puedes usar esto más adelante en reglas de negocio)
        public ICollection<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();



        // ✔️ Tú usarás este para crear usuarios en código
        public Usuario(string nombre, string email, string passwordHash)
        {
            Nombre = nombre;
            Email = email;
            PasswordHash = passwordHash;
            UsuarioRoles = new List<UsuarioRol>();
        }
        public Usuario() { }


    }
}
