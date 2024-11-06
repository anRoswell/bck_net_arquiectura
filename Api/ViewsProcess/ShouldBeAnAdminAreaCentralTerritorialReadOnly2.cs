namespace Api.ViewsProcess
{
    using System;
    using Core.Options;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;

    public class ShouldBeAnAdminAreaCentralTerritorialReadOnly2 : IAuthorizationRequirement { }

    public class ShouldBeAnAdminAreaCentralTerritorialReadOnly2Handler : BaseAuthorizationHandler<ShouldBeAnAdminAreaCentralTerritorialReadOnly2>
    {
        public ShouldBeAnAdminAreaCentralTerritorialReadOnly2Handler(
                IHttpContextAccessor httpContextAccessor,
                IOptions<PoliticasOptions> pathOptionsOp360,
                IOptions<ParametrosOptions> parametrosOptions
            ) : base(httpContextAccessor, pathOptionsOp360, parametrosOptions) { }

        protected override bool HasRequiredRole(int[] rolesArray)
        {
            return _pathOptionsOp360.ShouldBeAnAdminAreaCentralTerritorialReadOnly2Array.Any(x => rolesArray.Contains(x));
        }
    }
}