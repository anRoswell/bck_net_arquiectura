using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using ImageMagick;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    public class RepresentantesLegalEmpresaService : IRepresentantesLegalEmpresaService
    {
        public IUnitOfWork _unitOfWork;
        public IMapper _mapper;

        public RepresentantesLegalEmpresaService(IUnitOfWork unitOfWork, IMapper mapper) 
            => (_unitOfWork, _mapper) = (unitOfWork, mapper);        

        public async Task<RepresentantesLegalEmpresaDto> Add(RepresentantesLegalEmpresaDto dto)
        {
            var entity = _mapper.Map<RepresentantesLegalEmpresa>(dto) ?? throw new BusinessException($"No se pudo mapear la entidad");

            await _unitOfWork.RepresentantesLegalEmpresaReporitory.Add(entity);

            return _mapper.Map<RepresentantesLegalEmpresaDto>(entity);
        }

        public async Task Delete(int id)
        {
            var entityDb = await _unitOfWork.RepresentantesLegalEmpresaReporitory.GetById(id) ?? throw new BusinessException($"No se encontró el registro con Id: {id}");
            entityDb.RleEstado = false;
            await _unitOfWork.RepresentantesLegalEmpresaReporitory.Update(entityDb);
        }

        public async Task<RepresentantesLegalEmpresaDto> Edit(RepresentantesLegalEmpresaDto dto)
        {
            var entity = _mapper.Map<RepresentantesLegalEmpresa>(dto) ?? throw new BusinessException($"No se pudo mapear la entidad");

            var entityDb = await _unitOfWork.RepresentantesLegalEmpresaReporitory.GetById(entity.Id) ?? throw new BusinessException($"No se encontró el registro con Id: {entity.Id}");

            entityDb.RleIdentificacionRteLegal = entity.RleIdentificacionRteLegal;
            entityDb.RleNombreRteLegal = entity.RleNombreRteLegal;
            entityDb.RleEmailRteLegal = entity.RleEmailRteLegal;

            await _unitOfWork.RepresentantesLegalEmpresaReporitory.Update(entityDb);

            return _mapper.Map<RepresentantesLegalEmpresaDto>(entityDb);
        }

        public async Task<List<RepresentantesLegalEmpresaDto>> Get()
        {
            var entities = await _unitOfWork.RepresentantesLegalEmpresaReporitory.GetAsync(rle => rle.RleEstado == true);

            return _mapper.Map<List<RepresentantesLegalEmpresaDto>>(entities);
        }

        public async Task<RepresentantesLegalEmpresaDto> Get(int id)
        {
            var entity = await _unitOfWork.RepresentantesLegalEmpresaReporitory.GetById(id);

            return _mapper.Map<RepresentantesLegalEmpresaDto>(entity);
        }

        public async Task<List<RepresentantesLegalEmpresaDto>> GetByEmpresa(int idEmpresa)
        {
            var entity = await _unitOfWork.RepresentantesLegalEmpresaReporitory.GetAsync(rle => rle.RleEstado == true && rle.RleCodEmpresa == idEmpresa);

            return _mapper.Map<List<RepresentantesLegalEmpresaDto>>(entity);
        }
    }
}
