using AutoMapper;
using Core.DTOs;
using Core.Interfaces;
using Core.ModelResponse;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    public class UnidadesNegocioService : IUnidadesNegocioService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public UnidadesNegocioService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UnidadesNegocioDto> BuscarPorId(int id)
        {
            var response = await _unitOfWork.UnidadesNegocio.BuscarPorId(id);

            return _mapper.Map<UnidadesNegocioDto>(response);
        }

        public async Task<List<UnidadesNegocioDto>> BuscarTodos()
        {
            var response = await _unitOfWork.UnidadesNegocio.BuscarTodos();
            return _mapper.Map<List<UnidadesNegocioDto>>(response);
        }

        public async Task<List<ResponseAction>> Crear(UnidadesNegocioDto unidadesNegocio)
        {
            var response = await _unitOfWork.UnidadesNegocio.Crear(unidadesNegocio);

            if (response is null)
            {
                return new List<ResponseAction>() 
                {
                    new ResponseAction()
                    {
                        error = string.Empty,
                        estado = false,
                        Id = 0,
                        mensaje = "No se guardo"
                    }
                };
            }

            return new List<ResponseAction>() 
            {
                new ResponseAction(){
                    error = string.Empty,
                    Id = response.Id,
                    estado = true,
                    mensaje = "Creación Exitosa"
                }
            };
        }

        public async Task<List<ResponseAction>> Editar(UnidadesNegocioDto unidadesNegocio)
        {

            var entity = await _unitOfWork.UnidadesNegocio.BuscarPorId(unidadesNegocio.Id);

            if (entity is null)
            {
                return new List<ResponseAction>()
                {
                    new ResponseAction()
                    {
                        error = $"No se encontro Unidad de negocio con id: ${unidadesNegocio.Id}",
                        estado = false,
                        Id = unidadesNegocio.Id,
                        mensaje = string.Empty
                    }
                };
            }

            entity.UnDescripcion = unidadesNegocio.UnDescripcion;
            entity.UnEstado = unidadesNegocio.UnEstado;
            entity.CodUserUpdate = unidadesNegocio.CodUserUpdate;

            var response = await _unitOfWork.UnidadesNegocio.Editar(entity);

            if (response is null)
            {
                return new List<ResponseAction>()
                {
                    new ResponseAction()
                    {
                        error = string.Empty,
                        estado = false,
                        Id = 0,
                        mensaje = "No se actualizó"
                    }
                };
            }

            return new List<ResponseAction>()
            {
                new ResponseAction(){
                    error = string.Empty,
                    Id = response.Id,
                    estado = true,
                    mensaje = "Actualización Exitosa"
                }
            };
        }

        public async Task<List<ResponseAction>> Eliminar(int id)
        {
            var response = await _unitOfWork.UnidadesNegocio.Eliminar(id);

            if (response is null)
            {
                return new List<ResponseAction>()
                {
                    new ResponseAction()
                    {
                        error = string.Empty,
                        estado = false,
                        Id = 0,
                        mensaje = "No se elimino"
                    }
                };
            }

            return new List<ResponseAction>()
            {
                new ResponseAction(){
                    error = string.Empty,
                    Id = response.Id,
                    estado = true,
                    mensaje = "Eliminación Exitosa"
                }
            };
        }
    }
}