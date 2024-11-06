using Core.DTOs;
using Core.Entities;
using Core.ModelResponse;
using Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IOp360Repository
    {
        /*
        * fecha: 19/12/2023
        * clave: 5e6ewrt546weasdf _07
        * clave: 5e6ewrtLOGINEXTERNO6weasdf _07
        * carlos vargas
        */
        #region Seguridad
        Task<Usuarios_Perfiles> ConsultarUsusariosxPerfiles(QueryOp360Seguridad parameters);
        Task<Usuarios_Perfiles> ValidarLoginExterno(string Id_Usuario, string Token_Apex);
        #endregion           

        #region Ordenes                
        Task<string> Prueba();
        ResponseDto Procesar_Crear_Tabla_Tmp_SCR(QueryOp360CargueMasivoData parameters);
        #endregion


        /*
        * fecha: 09/01/2023
        * clave: asvfasd56f4sadf6sad
        * carlos vargas
        */
        #region Secuencias
        Task<int> GetSecuenceOracle(string NameSequence);
        #endregion
    }
}

