using Core.Entities;
using Core.Interfaces;

namespace Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ParametrosInicialesService : IParametrosInicialesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ParametrosInicialesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Banco>> GetBancos()
        {
            return await _unitOfWork.ParametrosInicialesRepository.GetBancos();
        }

        public async Task<List<Ciudade>> GetCiudades()
        {
            return await _unitOfWork.ParametrosInicialesRepository.GetCiudades();
        }

        public async Task<List<PrvCondicionesPago>> GetCondicionesPago()
        {
            return await _unitOfWork.ParametrosInicialesRepository.GetCondicionesPago();
        }

        public async Task<List<Departamento>> GetDepartamentos()
        {
            return await _unitOfWork.ParametrosInicialesRepository.GetDepartamentos();
        }

        public async Task<List<Documento>> GetDocumentos()
        {
            return await _unitOfWork.ParametrosInicialesRepository.GetDocumentos();
        }

        public async Task<List<Empresa>> GetEmpresas()
        {
            return await _unitOfWork.ParametrosInicialesRepository.GetEmpresas();
        }

        public async Task<List<PrvProdServ>> GetProductosServicios()
        {
            return await _unitOfWork.ParametrosInicialesRepository.GetProductosServicios();
        }

        public async Task<List<TipoProveedor>> GetTipoProveedores()
        {
            return await _unitOfWork.ParametrosInicialesRepository.GetTipoProveedores();
        }

        public async Task<ParametrosIniciales> GetParametrosIniciales()
        {
            ParametrosIniciales paramsIniciales = new ParametrosIniciales
            {
                Ciudades = await GetCiudades(),
                Departamentos = await GetDepartamentos(),
                Empresas = await GetEmpresas()
            };

            return paramsIniciales;
        }

        public async Task<ParametrosProveedor> GetParametrosProveedores()
        {
            ParametrosProveedor paramsPrv = new ParametrosProveedor
            {
                Ciudades = await GetCiudades(),
                Departamentos = await GetDepartamentos(),
                Empresas = await GetEmpresas(),
                CondicionesPagos = await GetCondicionesPago(),
                ProductoServicios = await GetProductosServicios(),
                TipoProveedores = await GetTipoProveedores(),
                Documentos = await GetDocumentos(),
                Bancos = await GetBancos()
            };

            return paramsPrv;
        }

        public async Task<List<PrvListaRestrictiva>> GetListaRestrictivas(int idProveedor)
        {
            return await _unitOfWork.ParametrosInicialesRepository.GetListaRestrictivas(idProveedor);
        }
    }
}
