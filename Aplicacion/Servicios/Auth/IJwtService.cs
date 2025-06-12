using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Servicios.Auth
{
    // Esta interfaz define el contrato
    // para cualquier clase que quiera
    // generar tokens JWT.
    // Permite que el controlador dependa
    // de la abstracción y no de la implementación
    // concreta (principio SOLID).
    public interface IJwtService
    {
        // Método que debe implementar cualquier clase que quiera generar JWTs.
        string GenerarToken(string email, int usuarioId, List<string> roles);
    }
}
