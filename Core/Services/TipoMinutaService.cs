using AutoMapper;
using Core.DTOs;
using Core.DTOs.FilesDto;
using Core.Exceptions;
using Core.Interfaces;
using Core.ModelProcess;
using Core.ModelResponse;
using Core.Options;
using Core.QueryFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Services
{
    public class TipoMinutaService : ITipoMinutaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFilesProcess _filesProcess;
        private readonly PathOptions _pathOptions;

        public TipoMinutaService(IUnitOfWork unitOfWork, IMapper mapper, IFilesProcess filesProcess, IOptions<PathOptions> pathOptions)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _filesProcess = filesProcess;
            _pathOptions = pathOptions.Value;
        }

        public async Task<TipoMinutaDto> GuardarTipoDeMinuta(TipoMinutaCreateCommand command)
        {
            if (command is null)
            {
                throw new BusinessException(nameof(command));
            }

            FormDataImagen dataFile = new FormDataImagen()
            {
                Carpeta = _pathOptions.Folder_Archivos_TipoMinuta,
                Files = new List<IFormFile> { command.File },
                IdPathFileServer = _pathOptions.IdPathFileServer
            };

            //List<FileResponse> filesReq = await _filesProcess.GetFilesCreated(dataFile);
            List<FileResponse> filesReq = new List<FileResponse>() { };

            if (!filesReq.Any())
            {
                throw new BusinessException(nameof(filesReq));
            }

            var file = filesReq.First();

            string nombreOriginal = file.NombreOriginal;
            string nombreDocumento = string.Concat(nombreOriginal, file.Extension);
            int tamano = int.Parse(file.Size.ToString());
            
            var tipoMinuta = new Entities.TipoMinuta
            {
                Nombre = command.Nombre,
                UrlDocument = file.PathWebAbsolute,
                UrlRelDocument = file.PathWebRelative,
                ExtDocument = file.Extension,
                SizeDocument = tamano,
                NameDocument = $"{file.NombreInterno}{file.Extension}",
                OriginalNameDocument = nombreDocumento
            };

            await _unitOfWork.TipoMinutaRepository.Add(tipoMinuta);

            return _mapper.Map<TipoMinutaDto>(tipoMinuta);
        }

        public async Task<ResponseAction> EliminarTipoMinuta(int idTipoMinuta)
        {
            _ = await _unitOfWork.TipoMinutaRepository.GetById(idTipoMinuta) ?? throw new BusinessException($"No se encontró tipo de minuta con id: {idTipoMinuta}");

            await _unitOfWork.TipoMinutaRepository.Delete(idTipoMinuta);

            return new ResponseAction()
            {
                estado = true,
                Id = idTipoMinuta,
                mensaje = "Eliminación exitosa"
            };
        }

        public async Task<List<TipoMinutaDto>> ObtenerTipoMinuta() 
        {
            var tiposMinuta = await _unitOfWork.TipoMinutaRepository.GetAll();

            return _mapper.Map<List<TipoMinutaDto>>(tiposMinuta);
        }
    }
}