using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;

namespace Aplicacion.Usuarios.Commands
{
    public class AsignarRolAUsuarioValidator : AbstractValidator<AsignarRolAUsuarioCommand>
    {
        public AsignarRolAUsuarioValidator()
        {
            RuleFor(x => x.UsuarioId).GreaterThan(0).WithMessage("El ID del usuario debe ser mayor que cero.");
            RuleFor(x => x.NombreRol).NotEmpty().WithMessage("El nombre del rol es obligatorio.");
        }
    }
}
