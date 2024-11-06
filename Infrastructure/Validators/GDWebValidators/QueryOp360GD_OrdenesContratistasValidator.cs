using Core.QueryFilters.QueryFiltersGDWeb;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Validators.GDWebValidators
{
    public class QueryOp360GD_OrdenesContratistasValidator : AbstractValidator<QueryOp360GD_OrdenesContratistas>
    {
        public QueryOp360GD_OrdenesContratistasValidator()
        {
            // Regla de validación para la propiedad id_contratista.
            RuleFor(entity => entity.id_contratista)
                // Define una regla que indica que el valor de id_contratista debe ser mayor que 0.
                .GreaterThan(0)
                // Asocia un mensaje de error personalizado si la regla no se cumple.
                .WithMessage("El usuario no tiene un perfil de contratista, no tiene permisos sobre este EndPoint.");
        }
    }
}
