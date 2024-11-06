using Core.CustomEntities;
using Core.Entities;
using Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IParametrosInicialesRepository
    {
        Task<List<Banco>> GetBancos();
        Task<List<Ciudade>> GetCiudades();
        Task<List<PrvCondicionesPago>> GetCondicionesPago();
        Task<List<Departamento>> GetDepartamentos();
        Task<List<Documento>> GetDocumentos();
        Task<List<Empresa>> GetEmpresas();
        Task<List<PrvProdServ>> GetProductosServicios();
        Task<List<TipoProveedor>> GetTipoProveedores();
        Task<List<PrvListaRestrictiva>> GetListaRestrictivas(int idProveedor);
        Task<List<Estados>> GetEstadoProveedores();
        Task<List<TiposCertificado>> GetTiposCertificados();
        Task<List<TiposCertificadoEspeciale>> GetTiposCertificadosEspeciales();
        Task<List<Empresa>> GetEmpresasByUser(int idUser, string tipoUsuario);
        Task<List<Empresa>> GetEmpresasByUserApoteosys(int idUser);
        Task<List<Sino>> GetSiNo();
        Task<List<ParamsGenerale>> GetCantidadProveedores_A_ValidarReq();
        Task<List<PathsPortal>> GetPathsImagenesCertificados();
        Task<List<IdentidadesLocale>> GetSelectoresIva();
        Task<List<ParamsInicialesReqPrv>> GetParametrosInicialesReqPrv();
        Task<List<TipoCriterio>> GetSelectorTipoCriterio();
        Task<List<FacturasPagas>> GetFacturasPagas(QueryGenerateFacturaPagada query);
        Task<List<Empresa>> GetEmpresasByUserDashboardApoteosys(int idUser);
        Task<List<TipoRequerimiento>> GetTipoRequerimientos();
    }
}
