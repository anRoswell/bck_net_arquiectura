using Azure.Core;
using Core.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{
    public class SwaggerBasicAuthMiddleware
    {
        private readonly RequestDelegate next;        
        private SwaggerOptions _swaggerOptions2;

        public SwaggerBasicAuthMiddleware(
            RequestDelegate next, IOptions<SwaggerOptions> swaggerOptions2)
        {
            this.next = next;            
            _swaggerOptions2 = swaggerOptions2.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //Make sure we are hitting the swagger path, and not doing it locally as it just gets annoying :-)
            //&& !this.IsLocalRequest(context)
            if (context.Request.Path.StartsWithSegments("/swagger") || context.Request.Path.StartsWithSegments("/index.html"))
            {
                string authHeader = context.Request.Headers["Authorization"];
                if (authHeader != null && authHeader.StartsWith("Basic "))
                {
                    // Get the encoded username and password
                    var encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();

                    // Decode from Base64 to string
                    var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));

                    // Split username and password
                    var username = decodedUsernamePassword.Split(':', 2)[0];
                    var password = decodedUsernamePassword.Split(':', 2)[1];

                    // Check if login is correct
                    if (IsAuthorized(username, password))
                    {
                        //_swaggerOptions.Autorizar = true;
                        _swaggerOptions2.Autorizar = true;
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        await next.Invoke(context);
                        return;
                    }
                    else
                    {
                        //_swaggerOptions.Autorizar = false;
                        _swaggerOptions2.Autorizar = false;
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        await next.Invoke(context);
                        return;
                    }
                }

                // Return authentication type (causes browser to show login dialog)
                context.Response.Headers["WWW-Authenticate"] = "Basic";

                _swaggerOptions2.Autorizar = false;
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                await next.Invoke(context);
            }
        }

        public bool IsAuthorized(string username, string password)
        {
            string _username2 = _swaggerOptions2.Usuario;
            string _password2 = _swaggerOptions2.Contrasena;

            // Check that username and password are correct
            return username.Equals(_username2, StringComparison.InvariantCultureIgnoreCase) && password.Equals(_password2);
        }

        //public bool IsLocalRequest(HttpContext context)
        //{
        //    //Handle running using the Microsoft.AspNetCore.TestHost and the site being run entirely locally in memory without an actual TCP/IP connection
        //    if (context.Connection.RemoteIpAddress == null && context.Connection.LocalIpAddress == null)
        //    {
        //        return true;
        //    }
        //    if (context.Connection.RemoteIpAddress.Equals(context.Connection.LocalIpAddress))
        //    {
        //        return true;
        //    }
        //    if (IPAddress.IsLoopback(context.Connection.RemoteIpAddress))
        //    {
        //        return true;
        //    }
        //    return false;
        //}

    }

    public static class SwaggerAuthorizeExtensions
    {
        public static IApplicationBuilder UseSwaggerAuthorized(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SwaggerBasicAuthMiddleware>();
        }
    }
}
