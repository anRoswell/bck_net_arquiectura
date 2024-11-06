namespace Api.ViewsProcess
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Api.Responses;
    using Core.Options;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;

    public abstract class BaseAuthorizationHandler<TRequirement> : AuthorizationHandler<TRequirement> where TRequirement : IAuthorizationRequirement
    {
        #region Properties
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly PoliticasOptions _pathOptionsOp360;
        protected readonly ParametrosOptions _parametrosOptions;
        protected virtual bool isExpirationToken { get; } = false;
        #endregion

        protected BaseAuthorizationHandler(
            IHttpContextAccessor httpContextAccessor,
            IOptions<PoliticasOptions> pathOptionsOp360,
            IOptions<ParametrosOptions> parametrosOptions
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _pathOptionsOp360 = pathOptionsOp360.Value;
            _parametrosOptions = parametrosOptions.Value;
        }

        protected int[] GetRolesArray(ClaimsPrincipal user)
        {
            return user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Role || x.Type == "Role")?.Value?.Split(',').Select(int.Parse).ToArray() ?? Array.Empty<int>();
        }

        protected abstract bool HasRequiredRole(int[] rolesArray);

        protected bool ShouldSkipValidation()
        {
            bool HabilitarJWT = _parametrosOptions.HabilitarJWT ?? false;
            return !HabilitarJWT;
        }

        protected void AddUserClaims(AuthorizationHandlerContext context, HttpContext httpContext)
        {
            if (context.User is not null)
            {
                var claims = context.User.Claims;
                foreach (var claim in claims)
                {
                    httpContext.Items.Add(claim.Type, claim.Value);
                }
                string roles = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role || x.Type == "Role")?.Value ?? "0";
                var rolesArray = roles.Split(',').Select(int.Parse).ToArray();
                httpContext.Items.Add("UserRole", rolesArray);
            }
        }

        protected async Task HandleForbiddenAccess(string error, string errorType, int codigoResponse, HttpContext httpContext)
        {
            httpContext.Items.Add("ErrorAuth", error);

            if (!httpContext.Items.ContainsKey("TipoError"))
            {
                httpContext.Response.StatusCode = codigoResponse;
                httpContext.Response.Headers.Append("WWW-Authenticate", $"Bearer error='{error}'");

                var response = ErrorResponse.GetErrorDescripcion(false, error, errorType, codigoResponse);
                await httpContext.Response.WriteAsJsonAsync(response);
                await httpContext.Response.CompleteAsync();
            }
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement)
        {
            if (ShouldSkipValidation())
            {
                context.Succeed(requirement);
                return;
            }
            HttpContext httpContext = _httpContextAccessor.HttpContext;
            var authHeader = httpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader))
            {
                await Task.CompletedTask;
                return;
            }
            int[] rolesArray = GetRolesArray(context.User);
            int codigoResponse;
            string error;
            if (rolesArray.Length == 0)
            {
                error = "'Usuario sin permisos de acceso.'";
                codigoResponse = StatusCodes.Status403Forbidden;
                await HandleForbiddenAccess(error, "Usuario sin Rol", codigoResponse, httpContext);
            }
            else if (!HasRequiredRole(rolesArray))
            {
                error = "'El usuario no tiene autorizacion para realizar este proceso'";
                codigoResponse = StatusCodes.Status403Forbidden;
                await HandleForbiddenAccess(error, "Usuario no tiene autorizacion", codigoResponse, httpContext);
            }
            else if (ExpirationToken())
            {
                error = "'El token se ha vencido'";
                codigoResponse = StatusCodes.Status403Forbidden;
                await HandleForbiddenAccess(error, "Token vencido", codigoResponse, httpContext);
            }
            else
            {
                context.Succeed(requirement);
            }
            AddUserClaims(context, httpContext);
            await Task.CompletedTask;
        }

        protected bool ExpirationToken()
        {
            if (isExpirationToken)
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                var authHeader = httpContext.Request.Headers["Authorization"].ToString();
                string token = authHeader.Substring("Bearer ".Length).Trim();
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
                var expirationDate = securityToken.ValidTo;
                var datenow = DateTime.UtcNow;
                var IsExpiration = expirationDate <= datenow;
                return IsExpiration;
            }
            return isExpirationToken;
        }
    }
}