namespace Core.Services
{
    using Core.Entities;
    using Core.Enumerations;
    using System.Threading.Tasks;
    using Core.Interfaces;
    using Core.DTOs.FilesDto;
    using System;

    public class FileAccessService : IFileAccessService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FileAccessService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<gnl_rutas_archivo_servidor> GetRoot(Enum_RutasArchivos modulo)
        {
            int idModulo = (int)modulo;

            return await _unitOfWork.RutasArchivoServidorOracleRepository.GetById(idModulo);
        }

        public async Task<gnl_soportes> SaveFileSoportes(FileResponse fileResponse, int userId)
        {
            gnl_soportes tmpgnl_soportes = new()
            {
                ID_ACTIVIDAD = 101, // select * from aire.gnl_actividades 101 = SCR, pendiente parametrizar
                ID_TIPO_SOPORTE = 81, // SELECT * FROM GNL_TIPOS_SOPORTE, pendiente parametrizar
                NOMBRE = fileResponse.NombreInterno,
                PESO = fileResponse.Size.ToString(),
                ID_USUARIO_REGISTRO = userId,
                FECHA_REGISTRO = DateTime.Now,
                FORMATO = fileResponse.Extension,
                IND_ARCHIVO_EXTERNO = "S",
                URL_EXTERNA = fileResponse.PathWebRelative
            };
            await _unitOfWork.Gnl_SoportesOracleRepository.Add(tmpgnl_soportes);

            return tmpgnl_soportes;
        }

        public async Task<gnl_soportes> SaveFileScrSoportes(FileResponse fileResponse, int userId, TipoSoporteEnum tipoSoporte)
        {
            var objTipoSoporte = await _unitOfWork.TiposSoporteOracleRepository.GetByCodigox(tipoSoporte.ToString());
            var idTipoSoporte = objTipoSoporte.Id;

            gnl_soportes tmpgnl_soportes = new()
            {
                ID_ACTIVIDAD = 101, // select * from aire.gnl_actividades 101 = SCR, pendiente parametrizar
                ID_TIPO_SOPORTE = idTipoSoporte,
                NOMBRE = fileResponse.NombreInterno,
                PESO = fileResponse.Size.ToString(),
                ID_USUARIO_REGISTRO = userId,
                FECHA_REGISTRO = DateTime.Now,
                FORMATO = fileResponse.Extension,
                IND_ARCHIVO_EXTERNO = "S",
                URL_EXTERNA = fileResponse.PathWebRelative
            };
            await _unitOfWork.Gnl_SoportesOracleRepository.Add(tmpgnl_soportes);

            return tmpgnl_soportes;
        }

        public async Task<gos_soporte> SaveFileGosSoportes(FileResponse fileResponse, int userId, TipoSoporteEnum tipoSoporte)
        {
            var objTipoSoporte = await _unitOfWork.TiposSoporteOracleRepository.GetByCodigox(tipoSoporte.ToString());
            var idTipoSoporte = objTipoSoporte.Id;
            gos_soporte gnlSoporte = new()
            {
                id_tipo_soporte = idTipoSoporte, //SELECT * FROM GNL_TIPOS_SOPORTE
                nombre = fileResponse.NombreInterno,
                peso = fileResponse.Size.ToString(),
                id_usuario_registra = userId,
                fecha_registra = DateTime.Now,
                formato = fileResponse.Extension,
                ind_url_externo = "S",
                url = fileResponse.PathWebRelative
            };
            await _unitOfWork.GosSoporteRepository.Add(gnlSoporte);

            return gnlSoporte;
        }
    }
}