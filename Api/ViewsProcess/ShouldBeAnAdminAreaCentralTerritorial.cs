namespace Api.ViewsProcess
{
    using System;
    using Core.Options;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;

    public class ShouldBeAnAdminAreaCentralTerritorial : IAuthorizationRequirement { }

    public class ShouldBeAnAdminAreaCentralTerritorialHandler : BaseAuthorizationHandler<ShouldBeAnAdminAreaCentralTerritorial>
    {
        public ShouldBeAnAdminAreaCentralTerritorialHandler(
                IHttpContextAccessor httpContextAccessor,
                IOptions<PoliticasOptions> pathOptionsOp360,
                IOptions<ParametrosOptions> parametrosOptions
            ) : base(httpContextAccessor, pathOptionsOp360, parametrosOptions) { }

        protected override bool HasRequiredRole(int[] rolesArray)
        {
            return _pathOptionsOp360.ShouldBeAnAdminAreaCentralTerritorialArray.Any(x => rolesArray.Contains(x));
        }
    }
}