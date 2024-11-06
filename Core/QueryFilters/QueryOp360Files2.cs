using Core.Enumerations;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.QueryFilters
{
    /*
    * fecha: 19/12/2023
    * clave: 5e6ewrt546weasdf _02
    * carlos vargas
    */
    public class QueryOp360Files2
    {
        public List<IFormFile>? Files { get; set; }
        public string? FileBase64 { get; set; }
        public string nombreFichero { get; set; }
        public Enum_RutasArchivos? Id_Ruta { get; set; }
    }

    public class QueryOp360Files
    {
        public List<IFormFile>? Files { get; set; }
        public string nombreFichero { get; set; }
        [Required]
        public Enum_RutasArchivos? Id_Ruta { get; set; }
        [Required]
        public string Actividad { get; set; }
        [Required]
        public string Tipos_Soporte { get; set; }
        public int id_usuario { get; set; }
    }

    public class QueryOp360GetFiles
    {
        [Required]
        public int Id_Soporte { get; set; }
        [Required]
        public Enum_RutasArchivos? Id_Ruta { get; set; }        
    }

    public class QueryOp360FileBase64
    {
        public string? FileBase64 { get; set; }
        public string Extension { get; set; }
        public Enum_RutasArchivos? Id_Ruta { get; set; }
        [Required]
        public string Actividad { get; set; }
        [Required]
        public string Tipos_Soporte { get; set; }
        public int id_usuario { get; set; }
    }
}
