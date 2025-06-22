using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.Usuarios.Queries;
using FluentValidation;

public class ObtenerRolesPorUsuarioQueryValidator : AbstractValidator<ObtenerRolesPorUsuarioQuery>
{
    public ObtenerRolesPorUsuarioQueryValidator()
    {
        RuleFor(x => x.UsuarioId).GreaterThan(0).WithMessage("El ID de usuario debe ser mayor que cero.");
    }
}
