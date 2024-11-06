using Api.Responses;
using Api.ViewsProcess;
using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Core.HubConfig;
using Core.Interfaces;
using Core.ModelResponse;
using Core.QueryFilters;
using FluentValidation;
using Infrastructure.Interfaces;
using Infrastructure.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Authorize(Policy = "ShouldBeAnAdmin")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUsuarioService _usuarioService;
        private readonly IParametrosInicialesService _paramsInicialesService;
        private readonly IPermisosUsuarioxMenuService _permisosUsuarioxMenuService;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;
        private readonly IHubContext<UsuarioHub> _hubContext;

        public UsuarioController(
            IConfiguration configuration,
            IUsuarioService usuarioService,
            IMapper mapper,
            IPasswordService passwordService,
            IParametrosInicialesService paramsInicialesService,
            IPermisosUsuarioxMenuService permisosUsuarioxMenuService,
            IHubContext<UsuarioHub> hubContext)
        {
            _configuration = configuration;
            _usuarioService = usuarioService;
            _mapper = mapper;
            _passwordService = passwordService;
            _paramsInicialesService = paramsInicialesService;
            _permisosUsuarioxMenuService = permisosUsuarioxMenuService;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Consultar usuario especifico por cédula
        /// </summary>
        /// <param name="cedula"></param>
        /// <returns></returns>
        [HttpGet("SearchByCedula", Name = "SearchUserByCedula")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchUserByCedula(string cedula)
        {
            try
            {
                var listuser = await _usuarioService.GetUsuarioXCedula(cedula);
                var user = listuser.Count > 0 ? listuser[0] : null;

                if (user is null)
                {
                    return Ok(ErrorResponse.GetError(false, "Usuario Inválido", 400));
                }

                var usuarioDto = _mapper.Map<UsuarioDto>(user);
                var response = new ApiResponse<UsuarioDto>(usuarioDto, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Consultar usuario por Id
        /// </summary>
        /// <param name="idUser"></param>
        /// <returns></returns>
        [HttpGet("SearchById", Name = "SearchUserById")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchUserById(int idUser)
        {
            try
            {
                var listuser = await _usuarioService.GetUsuarioPorId(idUser);
                var user = listuser.Count > 0 ? listuser[0] : null;

                if (user is null)
                {
                    return Ok(ErrorResponse.GetError(false, "Usuario Inválido", 400));
                }

                var usuarioDto = _mapper.Map<UsuarioDto>(user);
                var response = new ApiResponse<UsuarioDto>(usuarioDto, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Consultar todos los usuarios
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchAll", Name = "SearchUsers")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchUsers()
        {
            try
            {
                var users = await _usuarioService.GetListarUsuarios();

                var usersDto = _mapper.Map<List<UsuarioDto>>(users);

                var response = new ApiResponse<List<UsuarioDto>>(usersDto, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        ///// <summary>
        ///// Autenticar usuario en el Portal
        ///// seguimiento ejm token:
        ///// claves_eguimiento_token: seguimientotokenasdf644654sdaf6
        ///// </summary>
        ///// <param name="login">UserLogin JSON</param>
        ///// <returns></returns>
        //[HttpPost("Authentication", Name = "UserAuthentication")]
        //[Consumes("application/json")]
        //[AllowAnonymous]
        //public async Task<IActionResult> UserAuthentication([FromBody] UserLogin login)
        //{
        //    try
        //    {
        //        UserLoginValidator val = new();
        //        var validationResult = val.Validate(login);

        //        if (!validationResult.IsValid)
        //        {
        //            return Ok(ErrorResponse.GetError(false, validationResult.ToString("|"), 400));
        //        }

        //        //if it is a valid user
        //        var validation = await IsValidUser(login);
        //        if (validation.Item1)
        //        {
        //            await _hubContext.Clients.All.SendAsync("AskServerResponse", $"{validation.Item2.UsrNombreCompleto} ha iniciado sesión");
        //            TokenProcess tokenProcess = new TokenProcess(_configuration);
        //            var token = tokenProcess.GenerateToken(validation.Item2);
        //            HttpContext.Response.Headers.Add("Authorization", token);

        //            UsuarioDto usuarioDto = _mapper.Map<UsuarioDto>(validation.Item2);

        //            // Capturamos el Tipo Usuario con el fin de realizar la consulta de parametros Iniciales
        //            var tipoUsuario = usuarioDto.UsrTusrCodTipoUsuario;
        //            var parametros = new ParametrosIniciales();
        //            List<PermisosXUsuarioDto> permisosXUsuario = null;

        //            if (tipoUsuario == 1)
        //            {
        //                parametros = await _paramsInicialesService.GetParametrosIniciales();
        //                List<PermisosXUsuario> listPermisosXUsuario = await _permisosUsuarioxMenuService.GetPermisosXUsuario(usuarioDto.Id);
        //                permisosXUsuario = _mapper.Map<List<PermisosXUsuarioDto>>(listPermisosXUsuario);
        //            }

        //            var objResponse = new
        //            {
        //                parametros,
        //                usuario = usuarioDto,
        //                permisosXUsuario
        //            };

        //            var response = new ApiResponse<object>(objResponse, 200);
        //            return Ok(response);
        //        }

        //        return Ok(ErrorResponse.GetError(false, "Usuario Inválido", 400));
        //    }
        //    catch (Exception e)
        //    {
        //        throw new BusinessException($"Error en la autenticación. Detalle: {e.Message}");
        //    }
        //}

        /// <summary>
        /// Agregar usuario en el Portal
        /// </summary>
        /// <param name="usuarioDto">UsuarioDto JSON</param>
        /// <returns></returns>
        [HttpPost("Create", Name = "CreateUser")]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateUser([FromBody] UsuarioDto usuarioDto)
        {
            try
            {
                usuarioDto.CodUser = HttpContext.Items["UserID"]?.ToString();

                UsuarioValidator val = new();
                var validationResult = val.Validate(usuarioDto, e => e.IncludeRuleSets("CreateValidation"));

                if (!validationResult.IsValid)
                {
                    return Ok(ErrorResponse.GetError(false, validationResult.ToString("|"), 400));
                }

                usuarioDto.UsrPassword = usuarioDto.UsrCedula;
                var usuario = _mapper.Map<Usuario>(usuarioDto);

                usuario.UsrPassword = _passwordService.Hash(usuario?.UsrPassword);
                var responseAction = await _usuarioService.PostCrearUsuario(usuario);

                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error al intentar insertar registro. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Actualizar Usuario
        /// </summary>
        /// <param name="usuarioDto"></param>
        /// <returns></returns>
        [HttpPut("Update", Name = "UpdateUser")]
        [Consumes("application/json")]
        public async Task<IActionResult> UpdateUser([FromBody] UsuarioDto usuarioDto)
        {
            try
            {
                usuarioDto.CodUserUpdate = HttpContext.Items["UserID"]?.ToString();

                UsuarioValidator val = new();
                var validationResult = val.Validate(usuarioDto, e => e.IncludeRuleSets("UpdateValidation"));

                if (!validationResult.IsValid)
                {
                    return Ok(ErrorResponse.GetError(false, validationResult.ToString("|"), 400));
                }

                //usuarioDto.UsrPassword = usuarioDto.UsrPasswordSetter;
                var usuario = _mapper.Map<Usuario>(usuarioDto);
                //usuario.UsrPassword = _passwordService.Hash(usuario?.UsrPassword);

                var responseAction = await _usuarioService.PutActualizarUsuario(usuario);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la actualización del usuario. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para confirmar cambio de correo y habilitar el usuario suspendido
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("UpdateTmpSuspendido", Name = "UpdateTmpSuspendidoUser")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateTmpSuspendidoUser([FromBody] QueryToken param)
        {
            try
            {
                QueryUpdateTmpSuspendidoValidator val = new();
                var validationResult = val.Validate(param);

                if (!validationResult.IsValid)
                {
                    return Ok(ErrorResponse.GetError(false, validationResult.ToString("|"), 400));
                }

                var responseAction = await _usuarioService.PutTmpSuspendidoUsuario(param);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la confirmación de datos. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Eliminar Usuario
        /// </summary>
        /// <param name="usuarioDto"></param>
        /// <returns></returns>
        [HttpDelete("Delete", Name = "DeleteUser")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteUser(UsuarioDto usuarioDto)
        {
            try
            {
                usuarioDto.CodUserUpdate = HttpContext.Items["UserID"]?.ToString();

                UsuarioValidator val = new();
                var validationResult = val.Validate(usuarioDto, e => e.IncludeRuleSets("DeleteValidation"));

                if (!validationResult.IsValid)
                {
                    return Ok(ErrorResponse.GetError(false, validationResult.ToString("|"), 400));
                }

                var usuario = _mapper.Map<Usuario>(usuarioDto);

                var responseAction = await _usuarioService.DeleteUsuario(usuario);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la eliminación del usuario. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Actualiza la contraseña del usuario
        /// </summary>
        /// <param name="usuarioDto"></param>
        /// <returns></returns>
        [HttpPost("UpdatePassword", Name = "UserUpdatePassword")]
        [Consumes("application/json")]
        public async Task<IActionResult> UserUpdatePassword([FromBody] UsuarioDto usuarioDto)
        {
            usuarioDto.CodUserUpdate = HttpContext.Items["UserID"]?.ToString();

            UsuarioValidator val = new();
            var validationResult = val.Validate(usuarioDto, e => e.IncludeRuleSets("UpdatePasswordValidation"));

            if (!validationResult.IsValid)
            {
                return Ok(ErrorResponse.GetError(false, validationResult.ToString("|"), 400));
            }

            int idUser = (int)usuarioDto?.Id;
            var listuser = await _usuarioService.GetUsuarioPorId(idUser);
            var user = listuser.Count > 0 ? listuser[0] : null;

            if (user is null)
            {
                return Ok(ErrorResponse.GetError(false, "Usuario Inválido", 400));
            }

            user.UsrPassword = _passwordService.Hash(usuarioDto?.UsrPasswordSetter); // asignamos la nueva clave
            user.CodUserUpdate = usuarioDto?.CodUserUpdate;

            var responseAction = await _usuarioService.CambiarClaveUsuario(user);
            var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
            if (!responseAction[0].estado)
            {
                response.Status = 400;
            }
            return Ok(response);
        }

        /// <summary>
        /// Cambiar la contraseña del Usuario, conociendo la contraseña anterior
        /// </summary>
        /// <param name="usuarioDto"></param>
        /// <returns></returns>
        [HttpPost("ChangePassword", Name = "UserChangePassword")]
        [Consumes("application/json")]
        public async Task<IActionResult> UserChangePassword([FromBody] UsuarioDto usuarioDto)
        {
            try
            {
                usuarioDto.CodUserUpdate = HttpContext.Items["UserID"]?.ToString();

                UsuarioValidator val = new();
                var validationResult = val.Validate(usuarioDto, e => e.IncludeRuleSets("ChangePasswordValidation"));

                if (!validationResult.IsValid)
                {
                    return Ok(ErrorResponse.GetError(false, validationResult.ToString("|"), 400));
                }

                int idUser = usuarioDto.Id;
                var listuser = await _usuarioService.GetUsuarioPorId(idUser);
                var user = listuser.Count > 0 ? listuser[0] : null;

                if (user is null)
                {
                    return Ok(ErrorResponse.GetError(false, "Usuario Inválido", 400));
                }

                // Validamos que la contraseña nueva y la antigua no sean iguales
                //if (usuarioDto.OldPassword.Equals(usuarioDto.UsrPasswordSetter))
                //{
                //    return Ok(ErrorResponse.GetError(false, "La nueva contraseña no puede ser igual a la anterior!", 400));
                //}
                //else if (usuarioDto.UsrPasswordSetter.Equals(usuarioDto.UsrCedula)) // Validamos que la contraseña no sea igual a la cedula
                //{
                //    return Ok(ErrorResponse.GetError(false, "La nueva contraseña no puede ser igual a la identificación del usuario!", 400));
                //}
                //else
                //{
                // Validamos la contraseña actual
                bool validUser = _passwordService.Check(user.UsrPassword, usuarioDto.OldPassword);
                if (validUser)
                {
                    user.UsrPassword = _passwordService.Hash(usuarioDto.UsrPasswordSetter); // asignamos la nueva clave
                    user.CodUserUpdate = usuarioDto.CodUserUpdate;

                    var responseAction = await _usuarioService.CambiarClaveUsuario(user);
                    var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                    if (!responseAction[0].estado)
                    {
                        response.Status = 400;
                    }
                    return Ok(response);
                }
                else
                {
                    return Ok(ErrorResponse.GetError(false, "La contraseña anterior no coincide con la diligenciada", 400));
                }
                //}
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en el cambio de contraseña. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Acción para notificar por correo y realizar el Reseteo de contraseña del Usuario
        /// </summary>
        /// <param name="usuarioDto"></param>
        /// <returns></returns>
        [HttpPost("ResetPassword", Name = "UserResetPassword")]
        [Consumes("application/json")]
        public async Task<IActionResult> UserResetPassword([FromBody] UsuarioDto usuarioDto)
        {
            try
            {
                usuarioDto.CodUserUpdate = HttpContext.Items["UserID"]?.ToString();

                UsuarioValidator val = new();
                var validationResult = val.Validate(usuarioDto, e => e.IncludeRuleSets("ResetPassValidator"));

                if (!validationResult.IsValid)
                {
                    return Ok(ErrorResponse.GetError(false, validationResult.ToString("|"), 400));
                }

                var usuario = _mapper.Map<Usuario>(usuarioDto);

                // Le asignamos como contraseña la cedula del usuario
                usuario.UsrPassword = _passwordService.Hash(usuarioDto.UsrCedula);
                usuario.CodUserUpdate = usuarioDto.CodUserUpdate;

                var responseAction = await _usuarioService.ResetearClaveUsuario(usuario);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en el reinicio de contraseña. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Acción para notificar por correo y realizar la recuperación de contraseña del Usuario
        /// </summary>
        /// <param name="usuarioDto"></param>
        /// <returns></returns>
        [HttpPost("ForgottenPassword", Name = "UserForgottenPassword")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> UserForgottenPassword([FromBody] UsuarioDto usuarioDto)
        {
            try
            {
                UsuarioValidator val = new();
                var validationResult = val.Validate(usuarioDto, e => e.IncludeRuleSets("ForgottenPasswordValidator"));

                if (!validationResult.IsValid)
                {
                    return Ok(ErrorResponse.GetError(false, validationResult.ToString("|"), 400));
                }

                var usuario = _mapper.Map<Usuario>(usuarioDto);

                var responseAction = await _usuarioService.ForgottenPassword(usuario);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la recuperación de contraseña. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Valida usuario por el token de contraseña, realiza la actualizacion de la misma y modifica el estado del token
        /// </summary>
        /// <param name="recoveryParams"></param>
        /// <returns></returns>
        [HttpPost("RecoveryPassword", Name = "UserUpdatePasswordByToken")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> UserUpdatePasswordByToken([FromBody] RecoveryParams recoveryParams)
        {
            try
            {
                RecoveryParamsValidator val = new();
                var validationResult = val.Validate(recoveryParams);

                if (!validationResult.IsValid)
                {
                    return Ok(ErrorResponse.GetError(false, validationResult.ToString("|"), 400));
                }

                //if (string.IsNullOrEmpty(recoveryParams.Token) || string.IsNullOrEmpty(recoveryParams.NewPassword))
                //{
                //    return Ok(ErrorResponse.GetError(false, "Error inesperado, comuníquese con el Administrador del sistema", 400));
                //}

                recoveryParams.NewPassword = _passwordService.Hash(recoveryParams.NewPassword);
                var responseAction = await _usuarioService.RecoveryPassword(recoveryParams);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la recuperación de contraseña. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Cambiar la EmpresaProceso del Usuario
        /// </summary>
        /// <param name="usuarioDto"></param>
        /// <returns></returns>
        [HttpPost("ChangeEmpresa", Name = "UserChangeEmpresa")]
        [Consumes("application/json")]
        public async Task<IActionResult> UserChangeEmpresa([FromBody] UsuarioDto usuarioDto)
        {
            try
            {
                usuarioDto.CodUserUpdate = HttpContext.Items["UserID"]?.ToString();

                UsuarioValidator val = new();
                var validationResult = val.Validate(usuarioDto, e => e.IncludeRuleSets("ChangeEmpresaValidator"));

                if (!validationResult.IsValid)
                {
                    return Ok(ErrorResponse.GetError(false, validationResult.ToString("|"), 400));
                }

                var usuario = _mapper.Map<Usuario>(usuarioDto);
                var responseAction = await _usuarioService.PutActualizarEmpresaUsuario(usuario);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en el cambio de empresa. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// claves_eguimiento_token: seguimientotokenasdf644654sdaf6
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        private async Task<(bool, Usuario)> IsValidUser([FromBody] UserLogin login)
        {
            var listuser = await _usuarioService.GetLoginByCredentials(login);
            var user = listuser.Count > 0 ? listuser[0] : null;
            bool isValid = false;
            if (user != null)
            {
                //isValid = _passwordService.Check(user.UsrPassword, login.Password);
                //carlos.vargas con fines de prueba se deshabilita la linea anterior
                isValid = true;
            }
            return (isValid, user);
        }
    }
}