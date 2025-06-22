using Aplicacion.DTO;
using Aplicacion.Usuarios.DTO;
using Dominio.Entidades;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Repositorio
{
    public interface IUsuarioRepositorio
    {
        Task<bool> ExistePorEmailAsync(string email);
        Task CrearUsuarioConRolesAsync(Usuario usuario);
        Task<List<UsuarioDto>> ObtenerUsuariosConRolesAsync(CancellationToken ct);
        Task<List<RolAsignadoDto>> ObtenerRolesAsignadosAsync(int usuarioId);
        Task EliminarAsync(Usuario usuario);



        //“¿Existe un usuario con este Id en la base de datos?”
        Task<bool> ExistePorIdAsync(int id);


        // Obtiene un usuario con sus roles, o null si no existe
        Task<Usuario?> ObtenerPorIdAsync(int id);

        Task GuardarCambiosAsync(); // ✅ NUEVO




    }
}
