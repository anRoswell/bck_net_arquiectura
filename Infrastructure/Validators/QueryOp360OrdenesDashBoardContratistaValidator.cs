﻿using Core.DTOs;
using Core.Messages;
using Core.QueryFilters;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Validators
{
    public class QueryOp360OrdenesDashBoardContratistaValidator : AbstractValidator<QueryOp360OrdenesDashBoardContratista>
    {
        public QueryOp360OrdenesDashBoardContratistaValidator()
        {
            RuleFor(entity => entity.id_contratista)
            .GreaterThan(0)
            .WithMessage("El usuario no tiene un perfil de contratista, no tiene permisos sobre este EndPoint.");
        }
    }
}