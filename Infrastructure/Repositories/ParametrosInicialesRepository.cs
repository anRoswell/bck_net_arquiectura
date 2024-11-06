using Core.CustomEntities;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.QueryFilters;
using Core.Tools;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ParametrosInicialesRepository : IParametrosInicialesRepository
    {
        protected readonly DbModelContext _context;

        public ParametrosInicialesRepository(DbModelContext context)
        {
            _context = context;
        }

        public async Task<List<Ciudade>> GetCiudades()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","1")

                };

                string sql = $"[par].[SpParametrosIniciales] @Operacion = @Operacion";

                var response = await _context.Ciudades.FromSqlRaw(sql, parameters: parameters).ToListAsync();

                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<Departamento>> GetDepartamentos()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","2")

                };

                string sql = $"[par].[SpParametrosIniciales] @Operacion = @Operacion";

                var response = await _context.Departamentos.FromSqlRaw(sql, parameters: parameters).ToListAsync();

                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<Empresa>> GetEmpresas()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","3")

                };

                string sql = $"[par].[SpParametrosIniciales] @Operacion = @Operacion";

                var response = await _context.Empresas.FromSqlRaw(sql, parameters: parameters).ToListAsync();

                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<PrvCondicionesPago>> GetCondicionesPago()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","4")
                };

                string sql = $"[par].[SpParametrosIniciales] @Operacion = @Operacion";

                var response = await _context.CondicionesPagos.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<PrvProdServ>> GetProductosServicios()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","5")
                };

                string sql = $"[par].[SpParametrosIniciales] @Operacion = @Operacion";

                var response = await _context.ProductoServicios.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<TipoProveedor>> GetTipoProveedores()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","6")
                };

                string sql = $"[par].[SpParametrosIniciales] @Operacion = @Operacion";

                var response = await _context.TipoProveedors.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<Banco>> GetBancos()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","7")
                };

                string sql = $"[par].[SpParametrosIniciales] @Operacion = @Operacion";

                var response = await _context.Bancos.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<Documento>> GetDocumentos()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","1")
                };

                string sql = $"[prv].[SpGetDocuments] @Operacion = @Operacion";

                var response = await _context.Documento.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<PrvListaRestrictiva>> GetListaRestrictivas(int idProveedor)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","8"),
                    new SqlParameter("@CodProveedor",idProveedor),
                };

                string sql = $"[par].[SpParametrosIniciales] @Operacion = @Operacion, @CodProveedor = @CodProveedor";

                var response = await _context.ListaRestrictivas.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<Estados>> GetEstadoProveedores()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","9")
                };

                string sql = $"[par].[SpParametrosIniciales] @Operacion = @Operacion";

                var response = await _context.EstadosProveedor.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<TiposCertificado>> GetTiposCertificados()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","13")
                };

                string sql = $"[par].[SpParametrosIniciales] @Operacion = @Operacion";

                var response = await _context.TiposCertificados.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<TiposCertificadoEspeciale>> GetTiposCertificadosEspeciales()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","14")
                };

                string sql = $"[par].[SpParametrosIniciales] @Operacion = @Operacion";

                var response = await _context.TiposCertificadoEspeciales.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<Empresa>> GetEmpresasByUser(int idUser, string tipoUsuario)
        {
            try
            {
                SqlParameter[] parameters;

                if (tipoUsuario == "Proveedor")
                {
                    parameters = new[] {
                        new SqlParameter("@Operacion","15"),
                        new SqlParameter("@CodUsuario",idUser)
                    };
                }
                else
                {
                    parameters = new[] {
                        new SqlParameter("@Operacion","12"),
                        new SqlParameter("@CodUsuario",idUser)
                    };
                }

                string sql = $"[par].[SpParametrosIniciales] @Operacion = @Operacion, @CodUsuario = @CodUsuario";

                var response = await _context.Empresas.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<Empresa>> GetEmpresasByUserApoteosys(int idUser)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                        new SqlParameter("@Operacion","17"),
                        new SqlParameter("@CodUsuario",idUser)
                    };

                string sql = $"[par].[SpParametrosIniciales] @Operacion = @Operacion, @CodUsuario = @CodUsuario";

                var response = await _context.Empresas.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<Sino>> GetSiNo()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","16")
                };

                string sql = $"[par].[SpParametrosIniciales] @Operacion = @Operacion";

                var response = await _context.Sinos.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ParamsGenerale>> GetCantidadProveedores_A_ValidarReq()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","4"),
                    new SqlParameter("@Id",4) // CantidadProveedoresValidateRequerimiento
                };

                string sql = $"[par].[SpParamsGenerales] @Operacion = @Operacion, @Id = @Id";

                var response = await _context.ParamsGenerales.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<PathsPortal>> GetPathsImagenesCertificados()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","18")
                };

                string sql = $"[par].[SpParametrosIniciales] @Operacion = @Operacion";

                var response = await _context.PathsPortals.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<IdentidadesLocale>> GetSelectoresIva()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","19")
                };

                string sql = $"[par].[SpParametrosIniciales] @Operacion = @Operacion";

                var response = await _context.IdentidadesLocales.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ParamsInicialesReqPrv>> GetParametrosInicialesReqPrv()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","20")
                };

                string sql = $"[par].[SpParametrosIniciales] @Operacion = @Operacion";

                var response = await _context.ResponseJsonStrings.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return BaseResponseJson<RootJsonResponse<ParamsInicialesReqPrv>>.ConvertJsonToEntity(response).JsonResponse;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<TipoCriterio>> GetSelectorTipoCriterio()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","21")
                };

                string sql = $"[par].[SpParametrosIniciales] @Operacion = @Operacion";

                var response = await _context.TipoCriterios.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<FacturasPagas>> GetFacturasPagas(QueryGenerateFacturaPagada query)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","2"),
                    new SqlParameter("@FechaInicial",query.Fecha_Inicial),
                    new SqlParameter("@FechaFinal",query.Fecha_Final),
                    new SqlParameter("@Empresa",query.Empresa),
                    new SqlParameter("@NitProveedor",query.NitProveedor),
                    new SqlParameter("@TipoDocCausacion",query.Tipo_Documento_Causacion),
                    new SqlParameter("@NumDocCausacion",query.Numero_Documento_Causacion),
                };

                string sql = $"[dbo].[SpConsultaHistoricoPagosApoteosys] @Operacion = @Operacion, @FechaInicial = @FechaInicial, @FechaFinal = @FechaFinal, @Empresa = @Empresa, @NitProveedor = @NitProveedor, " +
                    $"@TipoDocCausacion = @TipoDocCausacion, @NumDocCausacion = @NumDocCausacion";

                var response = await _context.FacturasPagas.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<Empresa>> GetEmpresasByUserDashboardApoteosys(int idUser)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                        new SqlParameter("@Operacion","22"),
                        new SqlParameter("@CodUsuario",idUser)
                    };

                string sql = $"[par].[SpParametrosIniciales] @Operacion = @Operacion, @CodUsuario = @CodUsuario";

                var response = await _context.Empresas.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<TipoRequerimiento>> GetTipoRequerimientos()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","23")
                };

                string sql = $"[par].[SpParametrosIniciales] @Operacion = @Operacion";

                var response = await _context.TipoRequerimientos.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
    }
}
