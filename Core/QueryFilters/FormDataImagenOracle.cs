using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Enumerations;
using Microsoft.AspNetCore.Http;

namespace Core.QueryFilters
{
    /// <summary>
    /// Almacenamiento Imagen
    /// </summary>
    public class FormDataImagenOracle
    {
        public List<IFormFile> Files { get; set; }
        public Enum_RutasArchivos? Id_Ruta { get; set; }
        public string NombreModulo { get; set; }
        public int MyUser { get; set; }
    }
    public class FormDataImagenOracleBase64xxBorremeee
    {
        public string FileBase64 { get; set; }
        public int IdModulo { get; set; }
        public string NombreModulo { get; set; }
        public int MyUser { get; set; }
        public string FileExtension { get; set; }
    }

    public class Op360FileBase64
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
