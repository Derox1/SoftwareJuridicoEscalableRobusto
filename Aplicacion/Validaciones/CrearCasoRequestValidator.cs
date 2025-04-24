using Aplicacion.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Validaciones
{
    public  class CrearCasoRequestValidator : AbstractValidator<CrearCasoRequest>
    {
        public CrearCasoRequestValidator() 
        {
            RuleFor(x => x.Titulo)
            .NotEmpty().WithMessage("El título es obligatorio.");

            RuleFor(x => x.Descripcion)
                .MinimumLength(10).WithMessage("La descripción debe tener al menos 10 caracteres.");

            RuleFor(x => x.NombreCliente)
                .NotEmpty().WithMessage("El nombre del cliente es obligatorio.");

            RuleFor(x => x.TipoCaso)
                          .IsInEnum()
                          .WithMessage("Tipo de caso no válido.");
        }
    }
}
