namespace Api.ViewsProcess
{
    using Core.DTOs.Integration.Authentication;
    using Core.Entities;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;

    public class TokenProcess
    {
        private readonly IConfiguration _configuration;        

        public TokenProcess(IConfiguration configuration)
        {
            _configuration = configuration;            
        }

        //claves_eguimiento_token: seguimientotokenasdf644654sdaf6
        public string GenerateToken(Datos_Usuario usuario, HttpContext httpContext)
        {
            //Header
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            //Claims
            var claims = new[]
            {
                //new Claim(ClaimTypes.Email, usuario.email ?? ""),
                new Claim("id_usuario", usuario.id_usuario.ToString()),
                new Claim("id_persona", usuario.id_persona.ToString()),
                new Claim("nombres_apellidos", usuario.nombres_apellidos.ToString()),
                new Claim("identificacion", usuario.identificacion.ToString()),
                //new Claim("id_contratista", usuario.id_contratista.ToString()),
                new Claim("tipo_identificacion", usuario.tipo_identificacion.ToString()),
                new Claim(ClaimTypes.Role, string.Join(",",usuario.Perfiles.Select(x => x.id_perfil).ToArray())),
            };

            //Payload
            var payload = new JwtPayload
            (
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claims,
                DateTime.Now,
                DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Authentication:ExpireToken"]))
            );

            var token = new JwtSecurityToken(header, payload);            

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateTokenOsf(AuthenticationResponseDto authenticationResponseDto)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);
            DateTime fecha_vence = DateTime.Parse(authenticationResponseDto.fecha_vence);
            var claims = new[]
            {
                new Claim("Role", authenticationResponseDto.role),
            };
            var payload = new JwtPayload
            (
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claims,
                DateTime.UtcNow,
                fecha_vence
            );
            var token = new JwtSecurityToken(header, payload);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            //var count = tokenString.Length;
            return tokenString;
        }

    }
}