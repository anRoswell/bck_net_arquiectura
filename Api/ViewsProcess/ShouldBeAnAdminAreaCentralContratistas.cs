﻿namespace Api.ViewsProcess
{
    using System;
    using Core.Options;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;

    public class ShouldBeAnAdminAreaCentralContratistas : IAuthorizationRequirement { }

    public class ShouldBeAnAdminAreaCentralContratistasHandler : BaseAuthorizationHandler<ShouldBeAnAdminAreaCentralContratistas>
    {
        public ShouldBeAnAdminAreaCentralContratistasHandler(
                IHttpContextAccessor httpContextAccessor,
                IOptions<PoliticasOptions> pathOptionsOp360,
                IOptions<ParametrosOptions> parametrosOptions
            ) : base(httpContextAccessor, pathOptionsOp360, parametrosOptions) { }

        protected override bool HasRequiredRole(int[] rolesArray)
        {
            return _pathOptionsOp360.ShouldBeAnAdminAreaCentralContratistasArray.Any(x => rolesArray.Contains(x));
        }
    }
}