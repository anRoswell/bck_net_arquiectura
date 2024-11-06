using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IParametrosInicialesService
    {
        Task<List<Banco>> GetBancos();
        Task<List<Ciudade>> GetCiudades();
        Task<List<PrvCondicionesPago>> GetCondicionesPago();
        Task<List<Departamento>> GetDepartamentos();
        Task<List<Documento>> GetDocumentos();
        Task<List<Empresa>> GetEmpresas();
        Task<List<PrvProdServ>> GetProductosServicios();
        Task<List<TipoProveedor>> GetTipoProveedores();
        Task<ParametrosIniciales> GetParametrosIniciales();
        Task<ParametrosProveedor> GetParametrosProveedores();
        Task<List<PrvListaRestrictiva>> GetListaRestrictivas(int idProveedor);
    }
}
