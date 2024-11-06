using Core.DTOs;
using Core.Messages;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Validators
{
    public class Aire_Scr_Ord_OrdenDtoValidator : AbstractValidator<Aire_Scr_Ord_OrdenDto>
    {
        public Aire_Scr_Ord_OrdenDtoValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleSet("UpdateOrdenValidation", () =>
            {
                RuleFor(entity => entity.id_orden)
                    .GreaterThan(0).WithMessage(ErrorMessage.ValueWithValueError);
            });
        }
    }
}
