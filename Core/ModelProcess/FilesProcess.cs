namespace Core.ModelProcess
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using Core.DTOs;
    using Core.DTOs.FilesDto;
    using Core.Entities;
    using Core.Entities.SCRWebEntities;
    using Core.Exceptions;
    using Core.Interfaces;
    using Core.QueryFilters;

    public class FilesProcess : IFilesProcess
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly List<List<FileResponse>> FileListCreated = new List<List<FileResponse>>();
        private readonly IOp360Service _op360Service;        

        public FilesProcess(
            IUnitOfWork unitOfWork,
            IOp360Service op360Service
            )
        {
            _unitOfWork = unitOfWork;
            _op360Service = op360Service;
        }              

        public async Task<ResponseDto<List<FileResponseOracle>>> SaveFilesOracle(QueryOp360Files parameters)
        {
            try
            {
                ///---FormDataImagenOracle data
                // Obtenemos Path de File Server (Red)
                gnl_rutas_archivo_servidor rootFileServer = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById((int)parameters.Id_Ruta);
                gnl_actividades rootActividades = await _unitOfWork.ActividadesOracleRepository.GetByCodigox(parameters.Actividad);
                gnl_tipos_soporte rootTiposSoporte = await _unitOfWork.TiposSoporteOracleRepository.GetByCodigox(parameters.Tipos_Soporte);
                
                string PathRedBase = Path.Combine(rootFileServer.RUTA_RED);
                string NombreArchivo = string.Empty;
                string PathRedCompleto = string.Empty;
                FileResponseOracle fileData = new();
                List<FileResponseOracle> fileList = new List<FileResponseOracle>() { };
                ResponseDto<List<FileResponseOracle>> fileResponse = new();                
                FileInfo fileInfo = null;

                // Si no existe la ruta del 'PathRedBase' se crea.
                if (!Directory.Exists(PathRedBase))
                {
                    Directory.CreateDirectory(PathRedBase); //Creamos Carpetas
                }
                if(parameters.Files.Count > 0)
                {
                    foreach (var file in parameters.Files)
                    {
                        // Obtenemos información más relevante del adjunto.
                        string fileExt = Path.GetExtension(file.FileName);
                        string NombreUnico = Tools.Funciones.GetCodigoUnico(string.Concat($"{parameters.Actividad}_", $"{parameters.Tipos_Soporte}_", parameters.Id_Ruta.ToString()));
                        NombreArchivo = string.Concat(NombreUnico, fileExt);
                        PathRedCompleto = Path.Combine(PathRedBase, NombreArchivo);
                        using (var stream = System.IO.File.Create(PathRedCompleto))
                        {
                            await file.CopyToAsync(stream);
                        }
                        //////Grabar tabla aire.gnl_soportes
                        gnl_soportes tmpgnl_soportes = new gnl_soportes()
                        {
                            //Id = GetSecuenciaValor,
                            ID_ACTIVIDAD = rootActividades.Id,//--select * from aire.gnl_actividades 101 = SCR
                            ID_TIPO_SOPORTE = rootTiposSoporte.Id,
                            //BFILE = null,
                            NOMBRE = NombreArchivo,
                            PESO = file.Length.ToString(),
                            ID_USUARIO_REGISTRO = parameters.id_usuario,//---data.MyUser,
                            FECHA_REGISTRO = DateTime.Now,
                            FORMATO = file.ContentType,
                            IND_ARCHIVO_EXTERNO = "S",
                            URL_EXTERNA = PathRedCompleto
                        };
                        await _unitOfWork.Gnl_SoportesOracleRepository.Add(tmpgnl_soportes);

                        if (!System.IO.File.Exists(PathRedCompleto))
                        {
                            throw new BusinessException("Se presentó un error al crear archivo");
                        }

                        fileInfo = new FileInfo(PathRedCompleto);

                        // Consultamos el Path de las imágenes (Web)                        
                        fileData = new FileResponseOracle
                        {
                            Extension = fileInfo.Extension,
                            NombreInterno = NombreArchivo,
                            NombreOriginal = file.FileName,
                            PathWebDescarga = new Uri($"{rootFileServer.RUTA_WEB}{tmpgnl_soportes.Id}"),
                            Id_Soporte = tmpgnl_soportes.Id,
                            Size = fileInfo.Length
                        };
                        fileList.Add(fileData);
                    }
                    fileResponse = new ResponseDto<List<FileResponseOracle>>()
                    {
                        Codigo = 200,
                        Mensaje = "Registros Creados con Éxito",
                        Datos = fileList,
                        TotalRecords = parameters.Files.Count
                    };
                }
                else
                {
                    fileResponse = new ResponseDto<List<FileResponseOracle>>()
                    {
                        Codigo = 200,
                        Mensaje = "No se encontro un archivo para guardar",
                        Datos = null,
                        TotalRecords = 0
                    };
                }
                return fileResponse;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error inesperado: {e.Message} - {e.StackTrace}");
            }
        }

        public async Task<ResponseDto<FileResponseOracle>> SaveFileOracleBase64_Scr(QueryOp360FileBase64 parameters)
        {
            try
            {
                // Obtenemos Path de File Server (Red)
                gnl_rutas_archivo_servidor rootFileServer = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById((int)parameters.Id_Ruta);
                gnl_actividades rootActividades = await _unitOfWork.ActividadesOracleRepository.GetByCodigox(parameters.Actividad);
                gnl_tipos_soporte rootTiposSoporte = await _unitOfWork.TiposSoporteOracleRepository.GetByCodigox(parameters.Tipos_Soporte);
                string PathRedBase = Path.Combine(rootFileServer.RUTA_RED);
                string NombreArchivo = string.Empty;
                string PathRedCompleto = string.Empty;
                ResponseDto<FileResponseOracle> fileData = null;
                FileInfo fileInfo = null;                
                if (!Directory.Exists(PathRedBase)) Directory.CreateDirectory(PathRedBase);
                // Obtenemos información más relevante del adjunto.                
                string NombreUnico = Tools.Funciones.GetCodigoUnico(string.Concat(string.Concat($"{parameters.Actividad}_", $"{parameters.Tipos_Soporte}_", parameters.Id_Ruta.ToString())));                
                NombreArchivo = string.Concat(NombreUnico, parameters.Extension);
                PathRedCompleto = Path.Combine(PathRedBase, NombreArchivo);
                if (!string.IsNullOrWhiteSpace(parameters.FileBase64))
                {
                    byte[] fileBytes = Convert.FromBase64String(parameters.FileBase64);
                    System.IO.File.WriteAllBytes(PathRedCompleto, fileBytes);
                    var tmpfile = new FileInfo(PathRedCompleto);
                    var tipemime = GetContentType(PathRedCompleto);
                    gnl_soportes tmpgnl_soportes = new()
                    {
                        ID_ACTIVIDAD = rootActividades.Id,
                        ID_TIPO_SOPORTE = rootTiposSoporte.Id,
                        NOMBRE = NombreArchivo,
                        PESO = tmpfile.Length.ToString(),
                        ID_USUARIO_REGISTRO = parameters.id_usuario,
                        FECHA_REGISTRO = DateTime.Now,
                        FORMATO = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        IND_ARCHIVO_EXTERNO = "S",
                        URL_EXTERNA = PathRedCompleto
                    };
                    await _unitOfWork.Gnl_SoportesOracleRepository.Add(tmpgnl_soportes);
                    if (!System.IO.File.Exists(PathRedCompleto)) throw new BusinessException("Se presentó un error al crear archivo");
                    fileInfo = new FileInfo(PathRedCompleto);
                    fileData = new ResponseDto<FileResponseOracle>()
                    {
                        Codigo = 200,
                        Mensaje = "Registro Creado con Éxito",
                        Datos = new FileResponseOracle
                        {
                            Extension = fileInfo.Extension,
                            NombreInterno = NombreArchivo,
                            NombreOriginal = "NA",
                            PathWebDescarga = new Uri($"{rootFileServer.RUTA_WEB}{tmpgnl_soportes.Id}"),
                                Id_Soporte = tmpgnl_soportes.Id,
                            Size = fileInfo.Length
                        }
                    };
                }
                else
                {
                    fileData = new ResponseDto<FileResponseOracle>()
                    {
                        Codigo = 200,
                        TotalRecords = 0,
                        Mensaje = "No se encontro un archivo para guardar.",
                        Datos = null
                    };
                }

                return fileData;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error inesperado: {e.Message} - {e.StackTrace}");
            }
        }

        public async Task<FileResponseOracle> SaveFileOracleBase64_Gos(Op360FileBase64 parameters)
        {
            try
            {
                gnl_rutas_archivo_servidor rootFileServer = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById((int)parameters.Id_Ruta);
                gnl_actividades rootActividades = await _unitOfWork.ActividadesOracleRepository.GetByCodigox(parameters.Actividad);
                gnl_tipos_soporte rootTiposSoporte = await _unitOfWork.TiposSoporteOracleRepository.GetByCodigox(parameters.Tipos_Soporte);
                string PathRedBase = Path.Combine(rootFileServer.RUTA_RED);
                string NombreArchivo = string.Empty;
                string PathRedCompleto = string.Empty;
                FileResponseOracle fileData = null;
                FileInfo fileInfo = null;
                if (!Directory.Exists(PathRedBase)) Directory.CreateDirectory(PathRedBase);               
                string NombreUnico = Tools.Funciones.GetCodigoUnico(string.Concat(string.Concat($"{parameters.Actividad}_", $"{parameters.Tipos_Soporte}_", parameters.Id_Ruta.ToString())));
                NombreArchivo = string.Concat(NombreUnico, parameters.Extension);
                PathRedCompleto = Path.Combine(PathRedBase, NombreArchivo);
                if (!string.IsNullOrWhiteSpace(parameters.FileBase64))
                {
                    byte[] fileBytes = Convert.FromBase64String(parameters.FileBase64);
                    System.IO.File.WriteAllBytes(PathRedCompleto, fileBytes);
                    var tmpfile = new FileInfo(PathRedCompleto);
                    var tipemime = GetContentType(PathRedCompleto);
                    gos_soporte tmpgos_soportes = new()
                    {
                        id_tipo_soporte = rootTiposSoporte.Id,
                        nombre = NombreArchivo,
                        peso = tmpfile.Length.ToString(),
                        id_usuario_registra = rootActividades.Id,
                        fecha_registra = DateTime.Now,
                        formato = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        ind_url_externo = "S",
                        url = PathRedCompleto
                    };
                    await _unitOfWork.GosSoporteRepository.Add(tmpgos_soportes);
                    if (!System.IO.File.Exists(PathRedCompleto)) throw new BusinessException("Se presentó un error al crear archivo");
                    fileInfo = new FileInfo(PathRedCompleto);
                    fileData = new FileResponseOracle
                    {
                        Extension = fileInfo.Extension,
                        NombreInterno = NombreArchivo,
                        NombreOriginal = "NA",
                        PathWebDescarga = new Uri($"{rootFileServer.RUTA_WEB}{tmpgos_soportes.Id}"),
                        Id_Soporte = tmpgos_soportes.Id,
                        Size = fileInfo.Length
                    };
                }
                else
                {
                    throw new BusinessException("El archivo no contiene un tamaño específico.");
                }

                return fileData;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error inesperado: {e.Message} - {e.StackTrace}");
            }
        }

        public async Task<ResponseDto<FileResponseOracleBase64>> GetFileBase64(QueryOp360GetFiles parameters)
        {
            try
            {
                gnl_soportes rootSoportes = await _unitOfWork.Gnl_SoportesOracleRepository.GetById(parameters.Id_Soporte);
                gnl_rutas_archivo_servidor rootFileServer = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById((int)parameters.Id_Ruta);
                string PathRedBase = Path.Combine(rootFileServer.RUTA_RED, rootSoportes.NOMBRE);

                if (System.IO.File.Exists(PathRedBase))
                {
                    // Lee el contenido del archivo en un arreglo de bytes
                    byte[] fileBytes = System.IO.File.ReadAllBytes(PathRedBase);

                    // Convierte el arreglo de bytes a una cadena Base64
                    string base64String = Convert.ToBase64String(fileBytes);

                    // Determine the content type based on the file extension
                    FileInfo fileInfo = new FileInfo(PathRedBase);
                    var fileData = new ResponseDto<FileResponseOracleBase64>()
                    {
                        Codigo = 200,
                        Mensaje = "Registro Encontrado con Éxito",
                        Datos = new FileResponseOracleBase64
                        {
                            ArchivoBase64 = base64String,
                            NombreArchivo = rootSoportes.NOMBRE,
                            Extension = fileInfo.Extension,
                            TypeMime = rootSoportes.FORMATO,
                            Size = fileBytes.Length,
                            Id_Soporte = parameters.Id_Soporte
                        }
                    };

                    // Return the file
                    return fileData;
                }
                else
                {
                    return new ResponseDto<FileResponseOracleBase64>()
                    {
                        Codigo = 200,
                        TotalRecords = 0,
                        Mensaje = "No se encontro el archivo.",
                        Datos = null
                    };
                }                
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error inesperado: {e.Message} - {e.StackTrace}");
            }
        }

        public async Task<ResponseDto<FileResponsePlantillasOracleBase64Dto>> GetFilePlantillasBase64(QueryOp360ObtenerGnlPlantilla parameters)
        {
            try
            {
                var listTest = await _op360Service.DescargarArhivoGnlPlantillaToBase64(parameters);
                //listTest.Datos.archivobase64 = listTest.Datos.archivobase64.Replace(@"\r\", "\r");
                return listTest;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error inesperado: {e.Message} - {e.StackTrace}");
            }
        }

        public async Task<ResponseDto<FileResponseOracleBase64>> GetFilePlantillasCMBase64(QueryOp360ObtenerGnlPlantilla parameters)
        {
            try
            {
                var listTest = await _op360Service.DescargarArhivoGnlPlantillaToBase64(parameters);
                var response = new ResponseDto<FileResponseOracleBase64>()
                {
                    Codigo = listTest.Codigo,
                    Mensaje = listTest.Mensaje,
                    TotalRecords = 1,
                    Datos = new FileResponseOracleBase64
                    {
                        Id_Soporte = listTest.Datos.id_plantilla,
                        NombreArchivo = listTest.Datos.nombre_archivo,
                        Extension = listTest.Datos.extension,
                        TypeMime = listTest.Datos.typemime,
                        Size = listTest.Datos.archivobase64.Length,
                        ArchivoBase64 = listTest.Datos.archivobase64
                    }
                };                
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error inesperado: {e.Message}, {e.InnerException?.Message ?? ""} - {e.StackTrace}");
            }
        }

        public async Task<ResponseDto<FileResponseOracleBase64>> GetFileActaOrdenSCRBase64(FileResponsePdfDto ms)
        {
            try
            {
                string NombreUnico = Tools.Funciones.GetCodigoUnico("JasperReport");
                ResponseDto<FileResponseOracleBase64> listTest = new ResponseDto<FileResponseOracleBase64>()
                {
                    Datos = new FileResponseOracleBase64()
                    {
                        Id_Soporte = 0,
                        NombreArchivo = $"{NombreUnico}.pdf",
                        Extension = ".pdf",
                        TypeMime = "application/pdf",
                        Size = ms.File.Length,
                        ArchivoBase64 = Tools.Funciones.ConvertStreamToBase64(ms.File)
                    }
                };
                listTest.Codigo = (int)ms.StatusCode;
                listTest.Mensaje = ms.Mensaje;
                return listTest;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error inesperado: {e.Message} - {e.StackTrace}");
            }
        }

        public async Task<ResponseDto<FileResponseReportesJasper>> GetFilePlantillas(QueryOp360ObtenerGnlPlantilla parameters)
        {
            try
            {
                var listTest = await _op360Service.DescargarArhivoGnlPlantilla(parameters);                
                return listTest;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error inesperado: {e.Message} - {e.StackTrace}");
            }
        }

        public async Task<ResponseDto<FileResponseReportesJasper>> GetFileGnlSoporte(QueryOp360ObtenerGnlPlantilla parameters)
        {
            try
            {
                var listTest = await _op360Service.DescargarArhivoGnlSoporte(parameters);
                return listTest;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error inesperado: {e.Message} - {e.StackTrace}");
            }
        }

        public async Task<FileResponseBytes> ObtenerLogo()
        {
            try
            {
                gnl_rutas_archivo_servidor rootFileServer = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById(200);

                string PathRedBase = Path.Combine(rootFileServer.RUTA_RED, "logo2.png");

                if (System.IO.File.Exists(PathRedBase))
                {
                    // Lee el contenido del archivo en un arreglo de bytes
                    byte[] fileBytes = System.IO.File.ReadAllBytes(PathRedBase);

                    // Determine the content type based on the file extension
                    FileInfo fileInfo = new FileInfo(PathRedBase);

                    FileResponseBytes file = new()
                    {
                        _file = fileBytes,
                        TypeMime = "image/png",
                        Nombre = "logo2.png"
                    };

                    // Return the file
                    return file;
                }
                else
                {
                    return new FileResponseBytes();
                }
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error inesperado: {e.Message} - {e.StackTrace}");
            }
        }

        public async Task<FileResponseBytes> GetFileBytes(QueryOp360GetFiles parameters)
        {
            try
            {
                gnl_soportes rootSoportes = await _unitOfWork.Gnl_SoportesOracleRepository.GetById(parameters.Id_Soporte);
                gnl_rutas_archivo_servidor rootFileServer = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById((int)parameters.Id_Ruta);

                string PathRedBase = Path.Combine(rootFileServer.RUTA_RED, rootSoportes.NOMBRE);

                if (System.IO.File.Exists(PathRedBase))
                {
                    // Lee el contenido del archivo en un arreglo de bytes
                    byte[] fileBytes = System.IO.File.ReadAllBytes(PathRedBase);
                    
                    // Determine the content type based on the file extension
                    FileInfo fileInfo = new FileInfo(PathRedBase);

                    FileResponseBytes file = new()
                    {
                        _file = fileBytes,
                        TypeMime = rootSoportes.FORMATO,
                        Nombre = rootSoportes.NOMBRE
                    };

                    // Return the file
                    return file;
                }
                else
                {
                    return new FileResponseBytes();
                }
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error inesperado: {e.Message} - {e.StackTrace}");
            }
        }

        static string GetContentType(string filePath)
        {
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "file",
                    Arguments = $"--mime-type -b \"{filePath}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processStartInfo))
                {
                    if (process != null)
                    {
                        process.WaitForExit();
                        string output = process.StandardOutput.ReadToEnd().Trim();
                        return output;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el tipo MIME: {ex.Message}");
            }

            return null;
        }
    }
}