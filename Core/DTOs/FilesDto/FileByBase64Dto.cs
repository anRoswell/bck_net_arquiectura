namespace Core.DTOs.FilesDto
{
    using Core.Enumerations;

    public class FileByBase64Dto
	{
		public string Base64 { get; set; }
        public string PathBase { get; set; }
        public Enum_RutasArchivos IdRoute { get; set; }

        /// <summary>
        /// Mapeo del dto.
        /// </summary>
        /// <param name="base64">Imagen en base64.</param>
        /// <param name="idRoute">Modulo de la ruta.</param>
        /// <param name="pathBase">Ruta base.</param>
        public FileByBase64Dto(string base64, Enum_RutasArchivos idRoute, string pathBase)
        {
            Base64 = base64;
            IdRoute = idRoute;
            PathBase = pathBase;
        }
    }

    public class NewFileByBase64Dto
    {
        public string Base64 { get; set; }
        public string PathBase { get; set; }
        public TipoRutaEnum IdRoute { get; set; }

        /// <summary>
        /// Mapeo del dto.
        /// </summary>
        /// <param name="base64">Imagen en base64.</param>
        /// <param name="idRoute">Modulo de la ruta.</param>
        /// <param name="pathBase">Ruta base.</param>
        public NewFileByBase64Dto(string base64, TipoRutaEnum idRoute, string pathBase)
        {
            Base64 = base64;
            IdRoute = idRoute;
            PathBase = pathBase;
        }
    }
}

