namespace Core.DTOs.FilesDto
{
    using System;

    public class FileResponsePlantillasOracleBase64
    {
        public int id_plantilla { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public string nombre_archivo { get; set; }
        public string typemime { get; set; }
        public string archivobase64 { get; set; }
        public string mensaje { get; set; }
        public string extension
        {
            get
            {
                if (nombre_archivo != null)
                {
                    return string.Concat(".",nombre_archivo.Substring(nombre_archivo.LastIndexOf('.') + 1));
                }
                return null;
            }
        }
    }
}

