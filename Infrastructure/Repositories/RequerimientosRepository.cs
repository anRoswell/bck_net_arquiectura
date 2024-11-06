using Core.CustomEntities;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.ModelResponse;
using Core.QueryFilters;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Core.Tools;
using Core.Options;
using Core.CustomEntities.Parametros;
using Microsoft.AspNetCore.SignalR;
using Core.HubConfig;
using Core.Enumerations;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class RequerimientosRepository : BaseRepository<Requerimientos>, IRequerimientosRepository
    {
        public RequerimientosRepository(DbModelContext context) : base(context) { }

        public async Task<List<Requerimientos>> GetRequerimientos()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","1")
                };

                string sql = $"[req].[SpRequerimiento] @Operacion = @Operacion";

                var response = await _context.Requerimientos.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<Requerimientos>> GetFiltroRequerimiento(int Id, int Estado, int Empresa, DateTime FechaInicio, DateTime FechaFin, int Categoria)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","2"),
                    new SqlParameter("@reqIdRequerimientos",Id),
                    new SqlParameter("@reqEstado",Estado),
                    new SqlParameter("@reqCodEmpresa",Empresa),
                    new SqlParameter("@FechaInicio",FechaInicio.ToString("yyyyMMdd")),
                    new SqlParameter("@FechaFin",FechaFin.ToString("yyyyMMdd")),
                     new SqlParameter("@reqCodCategoria",Categoria),

                };

                string sql = $"[req].[SpRequerimiento] @Operacion = @Operacion, @reqIdRequerimientos=@reqIdRequerimientos, @reqEstado=@reqEstado, @reqCodEmpresa=@reqCodEmpresa, @FechaInicio=@FechaInicio, @FechaFin=@FechaFin, @reqCodCategoria=@reqCodCategoria ";

                var response = await _context.Requerimientos.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Obtenemos las empresas filtradas de acuerdo a los permisos asignados
        /// </summary>
        /// <param name="CodUsuario">id usuario logueado</param>
        /// <returns>List de empresas</returns>
        public async Task<List<Empresa>> GetEmpresas(string CodUsuario)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","12"),
                    new SqlParameter("@CodUsuario", CodUsuario)

                };

                string sql = $"[par].[SpParametrosIniciales] @Operacion = @Operacion, @CodUsuario = @CodUsuario";

                var response = await _context.Empresas.FromSqlRaw(sql, parameters: parameters).ToListAsync();

                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e}");
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

        public async Task<List<ResponseAction>> PostCrear(RequerimientoDto requerimientos, List<ReqArtSerRequeridosDoc> documentos)
        {
            try
            {
                StringBuilder ReqArtSerRequeridosTemp = new StringBuilder();
                StringBuilder ReqArtSerRequeridosDocTemp = new StringBuilder();
                StringBuilder ReqCriteriosEvaluaciontemp = new StringBuilder();
                StringBuilder ReqPolizasSeguroTemp = new StringBuilder();
                StringBuilder ReqListDocumentosTemp = new StringBuilder();
                StringBuilder ReqRtaCriteriosEvaluaciontemp = new StringBuilder();
                StringBuilder ReqCriterosEvaluacionRangoRtatemp = new StringBuilder();

                foreach (ReqArtSerRequeridosDto item in requerimientos.ReqArtSerRequeridos)
                {
                    ReqArtSerRequeridosTemp.Append("INSERT INTO #ReqArtSerRequeridosTemp (rasrCodProdServ,rasrItem,rasrEmpresaApot,rasrSolicitudApot,rasrLineaApot,rasrDepartamentoApot,rasrObservacionApot,rasrCodigoArticuloApot," +
                        "rasrDescripcionDteApot,rasrDescripcionDteAlternaApot,rasrUnidadMedidaApot,rasrObservacionDteApot,rasrCantidadApot,rasrFechaCreacionApot,rasrTipoFichaTecnica,rasrFichaTecnica,rasrAgrupado) ");
                    ReqArtSerRequeridosTemp.Append($"VALUES({item.RasrCodProdServ},{item.RasrItem},'{item.RasrEmpresaApot}',{item.RasrSolicitudApot},{item.RasrLineaApot},'{item.RasrDepartamentoApot}','{item.RasrObservacionApot}'," +
                        $"'{(item.RasrCodigoArticuloApot)}','{item.RasrDescripcionDteApot}','{item.RasrDescripcionDteAlternaApot}','{item.RasrUnidadMedidaApot}','{item.RasrObservacionDteApot}',{item.RasrCantidadApot}," +
                        $"'{item.RasrFechaCreacionApot:dd/MM/yyyy}',{item.RasrTipoFichaTecnica},'{item.RasrFichaTecnica}',{(item.RasrAgrupado ? 1 : 0)}) ");
                }

                foreach (ReqArtSerRequeridosDoc item in documentos)
                {
                    ReqArtSerRequeridosDocTemp.Append("INSERT INTO #ReqArtSerRequeridosDocTemp (itemArticulo, rasrEmpresaApot, rasrSolicitudApot, rasrCodigoArticuloApot, rasrLineaApot, rasrdCodTipoDocArtSerRequeridos, rasrdNameDocument, rasrdExtDocument, " +
                        "rasrdSizeDocument, rasrdUrlDocument, rasrdUrlRelDocument, rasrdOriginalNameDocument) ");
                    ReqArtSerRequeridosDocTemp.Append($"VALUES({item.ItemArticulo},'{item.RasrEmpresaApot}',{item.RasrSolicitudApot},'{item.RasrCodigoArticuloApot}',{item.RasrLineaApot},{item.RasrdCodTipoDocArtSerRequeridos},'{item.RasrdNameDocument}'," +
                        $"'{item.RasrdExtDocument}', {item.RasrdSizeDocument ?? 0},'{item.RasrdUrlDocument}','{item.RasrdUrlRelDocument}','{item.RasrdOriginalNameDocument}' ) ");
                }

                foreach (ReqCriterosEvaluacionDto item in requerimientos.ReqCriteriosEvaluacion)
                {
                    ReqCriteriosEvaluaciontemp.Append("INSERT INTO #ReqCriteriosEvaluaciontemp (rcriTipoCriterio, rcriTituloCriterio, rcriValorCriterio, rcriVlCriNumDesde, rcriVlCriNumHasta, rcriLlaveCriterio) ");
                    ReqCriteriosEvaluaciontemp.Append($"VALUES({item.RcriTipoCriterio},'{item.RcriTituloCriterio}',{item.RcriValorCriterio},{item.RcriVlCriNumDesde},{item.RcriVlCriNumHasta},{item.rcriLlaveCriterio}) ");

                    foreach (ReqRtaCriteriosEvaluacionDto resp in item.rcriRespuestasCriterio)
                    {
                        ReqRtaCriteriosEvaluaciontemp.Append("INSERT INTO #ReqRtaCriteriosEvaluaciontemp (rcriRtaCriterio, rcriRtaValorCriterio, rcriRtaLlaveCriterio) ");
                        ReqRtaCriteriosEvaluaciontemp.Append($"VALUES('{resp.rcriRtaCriterio}',{resp.rcriRtaValorCriterio},{resp.rcriRtaLlaveCriterio}) ");
                    }

                    foreach (ReqCriterosEvaluacionRangoRtaDto respRango in item.ReqCriterosEvaluacionRangoRta)
                    {
                        ReqCriterosEvaluacionRangoRtatemp.Append("INSERT INTO #ReqCriterosEvaluacionRangoRtatemp (rcRtaValorCriterioDesde, rcRtaValorCriterioHasta, rcRtaValorCriterio, rcRtaLlaveCriterio) ");
                        ReqCriterosEvaluacionRangoRtatemp.Append($"VALUES({respRango.RcRtaValorCriterioDesde},{respRango.RcRtaValorCriterioHasta},{respRango.RcRtaValorCriterio},{respRango.RcRtaLlaveCriterio}) ");
                    }
                }

                foreach (ReqPolizasSeguro item in requerimientos.ReqPolizasSeguro)
                {
                    ReqPolizasSeguroTemp.Append("INSERT INTO #ReqPolizasSeguroTemp (rpsRiesgoAsociado, rpsCuentia,rpsVigencia,rpsObservacion) ");
                    ReqPolizasSeguroTemp.Append($"VALUES('{item.rpsRiesgoAsociado}','{item.rpsCuentia}','{item.rpsVigencia}','{item.rpsObservacion}') ");
                }

                foreach (ReqListDocumentos item in requerimientos.ReqListDocumentos)
                {
                    if (!string.IsNullOrEmpty(item.RldocNombreDocumento))
                    {
                        ReqListDocumentosTemp.Append("INSERT INTO #ReqListDocumentosTemp (CodDocumento, rldocNombreDocumento, rldocVigencia,rldocObligatorio) ");
                        ReqListDocumentosTemp.Append($"VALUES({item.CodDocumento},'{item.RldocNombreDocumento}',{item.RldocVigencia},{(item.RldocObligatorio ? 1 : 0)}) ");
                    }
                    else
                    {
                        ReqListDocumentosTemp.Append("INSERT INTO #ReqListDocumentosTemp (CodDocumento, rldocVigencia,rldocObligatorio) ");
                        ReqListDocumentosTemp.Append($"VALUES({item.CodDocumento},{item.RldocVigencia},{(item.RldocObligatorio ? 1 : 0)}) ");
                    }
                }

                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","3"),
                    new SqlParameter("@reqNombreCentroCosto", requerimientos.ReqNombreCentroCosto),
                    new SqlParameter("@reqCodEmpresa", requerimientos.ReqCodEmpresa),
                    new SqlParameter("@reqCodAgencia", requerimientos.ReqCodAgencia),
                    new SqlParameter("@reqDescription", requerimientos.ReqDescription),
                    new SqlParameter("@reqLugarEntrega", requerimientos.ReqLugarEntrega),
                    new SqlParameter("@reqCierraOferta", requerimientos.ReqCierraOferta),
                    new SqlParameter("@reqCompraPrevista", requerimientos.ReqCompraPrevista),
                    new SqlParameter("@reqFechaEntrega", requerimientos.ReqFechaEntrega),
                    new SqlParameter("@reqGarantiasExigidas", requerimientos.ReqGarantiasExigidas),
                    new SqlParameter("@reqReqType", requerimientos.ReqReqType),
                    new SqlParameter("@reqCodGestorCompras", ""),
                    new SqlParameter("@reqRequiereContrato", requerimientos.ReqRequiereContrato),
                    new SqlParameter("@reqCodGestorContrato", requerimientos.CodUser),
                    new SqlParameter("@reqEstado", requerimientos.ReqEstado),
                    new SqlParameter("@CodUser", requerimientos.CodUser),
                    new SqlParameter("@ReqArtSerRequeridosTemp",ReqArtSerRequeridosTemp.ToString()),
                    new SqlParameter("@ReqArtSerRequeridosDocTemp",ReqArtSerRequeridosDocTemp.ToString()),
                    new SqlParameter("@ReqCriteriosEvaluaciontemp",ReqCriteriosEvaluaciontemp.ToString()),
                    new SqlParameter("@ReqRtaCriteriosEvaluaciontemp",ReqRtaCriteriosEvaluaciontemp.ToString()),
                    new SqlParameter("@ReqCriterosEvaluacionRangoRtatemp",ReqCriterosEvaluacionRangoRtatemp.ToString()),
                    new SqlParameter("@ReqPolizasSeguroTemp",ReqPolizasSeguroTemp.ToString()),
                    new SqlParameter("@ReqListDocumentosTemp",ReqListDocumentosTemp.ToString())
                };

                string sql = $"[req].[SpRequerimiento] @Operacion = @Operacion, @reqNombreCentroCosto = @reqNombreCentroCosto, @reqCodEmpresa = @reqCodEmpresa, @reqCodAgencia = @reqCodAgencia, @reqDescription = @reqDescription, " +
                    $"@reqLugarEntrega = @reqLugarEntrega, @reqCierraOferta = @reqCierraOferta, @reqCompraPrevista = @reqCompraPrevista, @reqFechaEntrega = @reqFechaEntrega, @reqGarantiasExigidas = @reqGarantiasExigidas, " +
                    $"@reqReqType = @reqReqType, @reqCodGestorCompras = @reqCodGestorCompras, @reqRequiereContrato = @reqRequiereContrato, @reqCodGestorContrato = @reqCodGestorContrato, " +
                    $"@reqEstado = @reqEstado, @CodUser = @CodUser, @ReqArtSerRequeridosTemp = @ReqArtSerRequeridosTemp, @ReqArtSerRequeridosDocTemp = @ReqArtSerRequeridosDocTemp, @ReqCriteriosEvaluaciontemp = @ReqCriteriosEvaluaciontemp, " +
                    $"@ReqRtaCriteriosEvaluaciontemp = @ReqRtaCriteriosEvaluaciontemp, @ReqCriterosEvaluacionRangoRtatemp = @ReqCriterosEvaluacionRangoRtatemp, @ReqPolizasSeguroTemp = @ReqPolizasSeguroTemp, " +
                    $"@ReqListDocumentosTemp = @ReqListDocumentosTemp";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;

            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PutActualizar(RequerimientoDto requerimientos, List<ReqArtSerRequeridosDoc> documentos)
        {
            try
            {
                StringBuilder ReqArtSerRequeridosTemp = new StringBuilder();
                StringBuilder ReqArtSerRequeridosDocTemp = new StringBuilder();
                StringBuilder ReqCriteriosEvaluaciontemp = new StringBuilder();
                StringBuilder ReqPolizasSeguroTemp = new StringBuilder();
                StringBuilder ReqListDocumentosTemp = new StringBuilder();
                StringBuilder ReqRtaCriteriosEvaluaciontemp = new StringBuilder();
                StringBuilder ReqCriterosEvaluacionRangoRtatemp = new StringBuilder();

                foreach (ReqArtSerRequeridosDto item in requerimientos.ReqArtSerRequeridos)
                {
                    ReqArtSerRequeridosTemp.Append("INSERT INTO #ReqArtSerRequeridosTemp (rasrCodProdServ,rasrItem,rasrEmpresaApot,rasrSolicitudApot,rasrLineaApot,rasrDepartamentoApot,rasrObservacionApot,rasrCodigoArticuloApot," +
                        "rasrDescripcionDteApot,rasrDescripcionDteAlternaApot,rasrUnidadMedidaApot,rasrObservacionDteApot,rasrCantidadApot,rasrFechaCreacionApot,rasrTipoFichaTecnica,rasrFichaTecnica,rasrAgrupado) ");
                    ReqArtSerRequeridosTemp.Append($"VALUES({item.RasrCodProdServ},{item.RasrItem},'{item.RasrEmpresaApot}',{item.RasrSolicitudApot},{item.RasrLineaApot},'{item.RasrDepartamentoApot}','{item.RasrObservacionApot}'," +
                        $"'{(item.RasrCodigoArticuloApot)}','{item.RasrDescripcionDteApot}','{item.RasrDescripcionDteAlternaApot}','{item.RasrUnidadMedidaApot}','{item.RasrObservacionDteApot}',{item.RasrCantidadApot}," +
                        $"'{item.RasrFechaCreacionApot:dd/MM/yyyy}',{item.RasrTipoFichaTecnica},'{item.RasrFichaTecnica}',{(item.RasrAgrupado ? 1 : 0)}) ");
                }

                foreach (ReqArtSerRequeridosDoc item in documentos)
                {
                    ReqArtSerRequeridosDocTemp.Append("INSERT INTO #ReqArtSerRequeridosDocTemp (itemArticulo, rasrEmpresaApot, rasrSolicitudApot, rasrCodigoArticuloApot, rasrLineaApot, rasrdCodTipoDocArtSerRequeridos, rasrdNameDocument, rasrdExtDocument, " +
                        "rasrdSizeDocument, rasrdUrlDocument, rasrdUrlRelDocument, rasrdOriginalNameDocument) ");
                    ReqArtSerRequeridosDocTemp.Append($"VALUES({item.ItemArticulo},'{item.RasrEmpresaApot}',{item.RasrSolicitudApot},'{item.RasrCodigoArticuloApot}',{item.RasrLineaApot},{item.RasrdCodTipoDocArtSerRequeridos},'{item.RasrdNameDocument}'," +
                        $"'{item.RasrdExtDocument}', {item.RasrdSizeDocument ?? 0},'{item.RasrdUrlDocument}','{item.RasrdUrlRelDocument}','{item.RasrdOriginalNameDocument}' ) ");
                }

                foreach (ReqCriterosEvaluacionDto item in requerimientos.ReqCriteriosEvaluacion)
                {
                    ReqCriteriosEvaluaciontemp.Append("INSERT INTO #ReqCriteriosEvaluaciontemp (rcriTipoCriterio, rcriTituloCriterio,rcriValorCriterio, rcriVlCriNumDesde, rcriVlCriNumHasta, rcriLlaveCriterio) ");
                    ReqCriteriosEvaluaciontemp.Append($"VALUES({item.RcriTipoCriterio},'{item.RcriTituloCriterio}',{item.RcriValorCriterio},{item.RcriVlCriNumDesde ?? 0},{item.RcriVlCriNumHasta ?? 0},{item.rcriLlaveCriterio}) ");

                    foreach (ReqRtaCriteriosEvaluacionDto resp in item.rcriRespuestasCriterio)
                    {
                        ReqRtaCriteriosEvaluaciontemp.Append("INSERT INTO #ReqRtaCriteriosEvaluaciontemp (rcriRtaCriterio, rcriRtaValorCriterio, rcriRtaLlaveCriterio) ");
                        ReqRtaCriteriosEvaluaciontemp.Append($"VALUES('{resp.rcriRtaCriterio}',{resp.rcriRtaValorCriterio},{resp.rcriRtaLlaveCriterio}) ");
                    }

                    foreach (ReqCriterosEvaluacionRangoRtaDto respRango in item.ReqCriterosEvaluacionRangoRta)
                    {
                        ReqCriterosEvaluacionRangoRtatemp.Append("INSERT INTO #ReqCriterosEvaluacionRangoRtatemp (rcRtaValorCriterioDesde, rcRtaValorCriterioHasta, rcRtaValorCriterio, rcRtaLlaveCriterio) ");
                        ReqCriterosEvaluacionRangoRtatemp.Append($"VALUES({respRango.RcRtaValorCriterioDesde},{respRango.RcRtaValorCriterioHasta},{respRango.RcRtaValorCriterio},{respRango.RcRtaLlaveCriterio}) ");
                    }
                }

                foreach (ReqPolizasSeguro item in requerimientos.ReqPolizasSeguro)
                {
                    ReqPolizasSeguroTemp.Append("INSERT INTO #ReqPolizasSeguroTemp (rpsRiesgoAsociado, rpsCuentia,rpsVigencia,rpsObservacion) ");
                    ReqPolizasSeguroTemp.Append($"VALUES('{item.rpsRiesgoAsociado}','{item.rpsCuentia}','{item.rpsVigencia}','{item.rpsObservacion}') ");
                }

                foreach (ReqListDocumentos item in requerimientos.ReqListDocumentos)
                {
                    if (!string.IsNullOrEmpty(item.RldocNombreDocumento))
                    {
                        ReqListDocumentosTemp.Append("INSERT INTO #ReqListDocumentosTemp (CodDocumento, rldocNombreDocumento, rldocVigencia,rldocObligatorio) ");
                        ReqListDocumentosTemp.Append($"VALUES({item.CodDocumento},'{item.RldocNombreDocumento}',{item.RldocVigencia},{(item.RldocObligatorio ? 1 : 0)}) ");
                    }
                    else
                    {
                        ReqListDocumentosTemp.Append("INSERT INTO #ReqListDocumentosTemp (CodDocumento, rldocVigencia,rldocObligatorio) ");
                        ReqListDocumentosTemp.Append($"VALUES({item.CodDocumento},{item.RldocVigencia},{(item.RldocObligatorio ? 1 : 0)}) ");
                    }
                }

                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","4"),
                    new SqlParameter("@reqIdRequerimientos", requerimientos.Id),
                    new SqlParameter("@reqNombreCentroCosto", requerimientos.ReqNombreCentroCosto),
                    new SqlParameter("@reqCodEmpresa", requerimientos.ReqCodEmpresa),
                    new SqlParameter("@reqCodAgencia", requerimientos.ReqCodAgencia),
                    new SqlParameter("@reqDescription", requerimientos.ReqDescription),
                    new SqlParameter("@reqLugarEntrega", requerimientos.ReqLugarEntrega),
                    new SqlParameter("@reqCierraOferta", requerimientos.ReqCierraOferta),
                    new SqlParameter("@reqCompraPrevista", requerimientos.ReqCompraPrevista),
                    new SqlParameter("@reqFechaEntrega", requerimientos.ReqFechaEntrega),
                    new SqlParameter("@reqGarantiasExigidas", requerimientos.ReqGarantiasExigidas),
                    new SqlParameter("@reqReqType", requerimientos.ReqReqType),
                    new SqlParameter("@reqCodGestorCompras", ""),
                    new SqlParameter("@reqRequiereContrato", requerimientos.ReqRequiereContrato),
                    new SqlParameter("@reqCodGestorContrato", requerimientos.ReqCodGestorContrato),
                    new SqlParameter("@reqEstado", requerimientos.ReqEstado),
                    new SqlParameter("@CodUser", requerimientos.CodUser),
                    new SqlParameter("@ReqArtSerRequeridosTemp",ReqArtSerRequeridosTemp.ToString()),
                    new SqlParameter("@ReqArtSerRequeridosDocTemp",ReqArtSerRequeridosDocTemp.ToString()),
                    new SqlParameter("@ReqCriteriosEvaluaciontemp",ReqCriteriosEvaluaciontemp.ToString()),
                    new SqlParameter("@ReqRtaCriteriosEvaluaciontemp",ReqRtaCriteriosEvaluaciontemp.ToString()),
                    new SqlParameter("@ReqCriterosEvaluacionRangoRtatemp",ReqCriterosEvaluacionRangoRtatemp.ToString()),
                    new SqlParameter("@ReqPolizasSeguroTemp",ReqPolizasSeguroTemp.ToString()),
                    new SqlParameter("@ReqListDocumentosTemp",ReqListDocumentosTemp.ToString())
                };

                string sql = $"[req].[SpRequerimiento] @Operacion = @Operacion, @reqIdRequerimientos = @reqIdRequerimientos, @reqNombreCentroCosto = @reqNombreCentroCosto, @reqCodEmpresa = @reqCodEmpresa, @reqCodAgencia = @reqCodAgencia, " +
                    $"@reqDescription = @reqDescription, @reqLugarEntrega = @reqLugarEntrega, @reqCierraOferta = @reqCierraOferta, @reqCompraPrevista = @reqCompraPrevista, @reqFechaEntrega = @reqFechaEntrega, " +
                    $"@reqGarantiasExigidas = @reqGarantiasExigidas, @reqReqType = @reqReqType, @reqCodGestorCompras = @reqCodGestorCompras, @reqRequiereContrato = @reqRequiereContrato, " +
                    $"@reqCodGestorContrato = @reqCodGestorContrato, @reqEstado = @reqEstado, @CodUser = @CodUser, @ReqArtSerRequeridosTemp = @ReqArtSerRequeridosTemp, @ReqArtSerRequeridosDocTemp = @ReqArtSerRequeridosDocTemp, " +
                    $"@ReqCriteriosEvaluaciontemp = @ReqCriteriosEvaluaciontemp, @ReqRtaCriteriosEvaluaciontemp = @ReqRtaCriteriosEvaluaciontemp, @ReqPolizasSeguroTemp = @ReqPolizasSeguroTemp, @ReqListDocumentosTemp = @ReqListDocumentosTemp, " +
                    $"@ReqCriterosEvaluacionRangoRtatemp = @ReqCriterosEvaluacionRangoRtatemp";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();


                return response;

            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PutActualizarEstado(QueryUpdateEstadoReq updateEstadoReq)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","5"),
                    new SqlParameter("@reqIdRequerimientos", updateEstadoReq.CodRequerimiento),
                    new SqlParameter("@reqEstado", updateEstadoReq.EstadoReq),
                    new SqlParameter("@CodUserUpdate", updateEstadoReq.CodUserUpdate)
                };

                string sql = $"[req].[SpRequerimiento] @Operacion = @Operacion, @reqIdRequerimientos = @reqIdRequerimientos, @reqEstado = @reqEstado, @CodUserUpdate = CodUserUpdate";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;

            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<Requerimientos>> GetRequerimientoPorID(int Id, int idProve)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","6"),
                    new SqlParameter("@reqIdRequerimientos", Id),
                     new SqlParameter("@CodProveedor", idProve),
                };

                string sql = $"[req].[SpRequerimiento] @Operacion = @Operacion, @reqIdRequerimientos=@reqIdRequerimientos,@CodProveedor=@CodProveedor";

                var response = await _context.Requerimientos.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ReqArtSerRequeridos>> GetArticulosPorIdReq(int Id, int idProve, bool IsNotIdUsuario)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","7"),
                    new SqlParameter("@reqIdRequerimientos", Id),
                    new SqlParameter("@CodProveedor", idProve),
                    new SqlParameter("@IsNotIdUsuario", IsNotIdUsuario),
                };

                string sql = $"[req].[SpRequerimiento] @Operacion = @Operacion, @reqIdRequerimientos = @reqIdRequerimientos, @CodProveedor = @CodProveedor, @IsNotIdUsuario = @IsNotIdUsuario";

                var response = await _context.ReqArtSerRequeridos.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ReqCriterosEvaluacion>> GetCriteriosPorIdReq(int Id, int idProve)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","8"),
                    new SqlParameter("@reqIdRequerimientos", Id),
                     new SqlParameter("@CodProveedor", idProve),
                };

                string sql = $"[req].[SpRequerimiento] @Operacion = @Operacion,@reqIdRequerimientos=@reqIdRequerimientos,@CodProveedor=@CodProveedor";

                var response = await _context.ReqCriteriosEvaluacions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ReqPolizasSeguro>> GetPolizasPorIdReq(int Id)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","9"),
                    new SqlParameter("@reqIdRequerimientos", Id),
                };

                string sql = $"[req].[SpRequerimiento] @Operacion = @Operacion, @reqIdRequerimientos=@reqIdRequerimientos";

                var response = await _context.ReqPolizasSeguros.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ReqListDocumentos>> GetDocumentosPorIdReq(QuerySearchRequerimientos parametros)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","10"),
                    new SqlParameter("@reqIdRequerimientos", parametros.Id),
                    new SqlParameter("@CodUser", parametros.CodUser),
                };

                string sql = $"[req].[SpRequerimiento] @Operacion = @Operacion, @reqIdRequerimientos = @reqIdRequerimientos, @CodUser = @CodUser";

                var response = await _context.ReqListDocumentos.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ReqRtaCriteriosEvaluacion>> GetRtaCriteriosPorIdReq(int Id)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","11"),
                    new SqlParameter("@reqIdRequerimientos", Id),
                };

                string sql = $"[req].[SpRequerimiento] @Operacion = @Operacion, @reqIdRequerimientos = @reqIdRequerimientos";

                var response = await _context.ReqRtaCriteriosEvaluacions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ReqCriterosEvaluacionRangoRta>> GetRtaRangoCriteriosPorIdReq(int Id)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","14"),
                    new SqlParameter("@reqIdRequerimientos", Id),
                };

                string sql = $"[req].[SpRequerimiento] @Operacion = @Operacion, @reqIdRequerimientos = @reqIdRequerimientos";

                var response = await _context.ReqCriterosEvaluacionRangoRta.FromSqlRaw(sql, parameters: parameters).ToListAsync();
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
                    new SqlParameter("@Operacion","4")
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

        public async Task<List<Estados>> GetEstadosRequerimiento()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","12")
                };

                string sql = $"[req].[SpRequerimiento] @Operacion = @Operacion";

                var response = await _context.EstadosRequerimiento.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<UnidadMedida>> GetUnidadMedida()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","10")
                };

                string sql = $"[par].[SpParametrosIniciales] @Operacion = @Operacion";

                var response = await _context.UnidadMedidas.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ReqTipoDocArtSerRequerido>> GetReqTipoDocArtSerRequerido()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","11")
                };

                string sql = $"[par].[SpParametrosIniciales] @Operacion = @Operacion";

                var response = await _context.ReqTipoDocArtSerRequerido.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ReqQuestionAnswer>> GetComentarios(int reqIdRequerimientos)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","16"),
                    new SqlParameter("@reqIdRequerimientos",reqIdRequerimientos),
                };

                string sql = $"[req].[SpRequerimiento] @Operacion = @Operacion, @reqIdRequerimientos = @reqIdRequerimientos";

                var response = await _context.ReqQuestionAnswers.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PostNewComentario(ReqQuestionAnswer reqQuestionAnswer)
        {
            try
            {
                SqlParameter[] parameters;
                string sql = string.Empty;

                if (reqQuestionAnswer.CodReqQuestionAnswer != null)
                {
                    parameters = new[] {
                        new SqlParameter("@Operacion","13"),
                        new SqlParameter("@codReqQuestionAnswer",reqQuestionAnswer.CodReqQuestionAnswer),
                        new SqlParameter("@reqIdRequerimientos",reqQuestionAnswer.RqaCodRequerimiento),
                        new SqlParameter("@CodUser",reqQuestionAnswer.CodUser),
                        new SqlParameter("@rqaIsGestor",reqQuestionAnswer.RqaIsGestor),
                        new SqlParameter("@rqaComentario",reqQuestionAnswer.RqaComentario),
                        new SqlParameter("@rqahasUploadFile",reqQuestionAnswer.RqahasUploadFile),
                        new SqlParameter("@rqaisPrivate",reqQuestionAnswer.RqaisPrivate),
                        new SqlParameter("@reqEstado",reqQuestionAnswer.RqaEstado),
                        new SqlParameter("@rqaFilePath",reqQuestionAnswer.RqaFilePath),
                        new SqlParameter("@rqaFileRelativo",reqQuestionAnswer.RqaFileRelativo),
                        new SqlParameter("@rqaFileSize",reqQuestionAnswer.RqaFileSize),
                        new SqlParameter("@rqaFileExt",reqQuestionAnswer.RqaFileExt),
                    };

                    sql = $"[req].[SpRequerimiento] @Operacion = @Operacion, @codReqQuestionAnswer = @codReqQuestionAnswer, @reqIdRequerimientos = @reqIdRequerimientos, @CodUser = @CodUser, " +
                    $"@rqaIsGestor = @rqaIsGestor, @rqaComentario = @rqaComentario, @rqahasUploadFile = @rqahasUploadFile, @rqaisPrivate = @rqaisPrivate, @reqEstado = @reqEstado," +
                    $"@rqaFilePath = @rqaFilePath, @rqaFileRelativo = @rqaFileRelativo, @rqaFileSize = @rqaFileSize, @rqaFileExt = @rqaFileExt";
                }
                else
                {
                    parameters = new[] {
                        new SqlParameter("@Operacion","13"),
                        new SqlParameter("@reqIdRequerimientos",reqQuestionAnswer.RqaCodRequerimiento),
                        new SqlParameter("@CodUser",reqQuestionAnswer.CodUser),
                        new SqlParameter("@rqaIsGestor",reqQuestionAnswer.RqaIsGestor),
                        new SqlParameter("@rqaComentario",reqQuestionAnswer.RqaComentario),
                        new SqlParameter("@rqahasUploadFile",reqQuestionAnswer.RqahasUploadFile),
                        new SqlParameter("@rqaisPrivate",reqQuestionAnswer.RqaisPrivate),
                        new SqlParameter("@reqEstado",reqQuestionAnswer.RqaEstado),
                        new SqlParameter("@rqaFilePath",reqQuestionAnswer.RqaFilePath),
                        new SqlParameter("@rqaFileRelativo",reqQuestionAnswer.RqaFileRelativo),
                        new SqlParameter("@rqaFileSize",reqQuestionAnswer.RqaFileSize),
                        new SqlParameter("@rqaFileExt",reqQuestionAnswer.RqaFileExt),
                    };

                    sql = $"[req].[SpRequerimiento] @Operacion = @Operacion, @reqIdRequerimientos = @reqIdRequerimientos, @CodUser = @CodUser, " +
                    $"@rqaIsGestor = @rqaIsGestor, @rqaComentario = @rqaComentario, @rqahasUploadFile = @rqahasUploadFile, @rqaisPrivate = @rqaisPrivate, @reqEstado = @reqEstado," +
                    $"@rqaFilePath = @rqaFilePath, @rqaFileRelativo = @rqaFileRelativo, @rqaFileSize = @rqaFileSize, @rqaFileExt = @rqaFileExt";
                }

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PostParticipar(ParamParticipacionReq parametersReq)
        {
            try
            {
                StringBuilder cadena = new StringBuilder();
                StringBuilder cadenaArt = new StringBuilder();
                StringBuilder insertDocsProv = new StringBuilder();
                StringBuilder insertDocsOtros = new StringBuilder();

                foreach (ReqRtaParticipante item in parametersReq.ReqParticipantesDto.Criterios)
                {
                    cadena.Append("INSERT INTO #ReqRtaParticipanteTemp (CodCriterio,Respuesta,CodReqRangoCriteriosEvaluacion,CodReqRtaCriteriosEvaluacion) ");
                    cadena.Append($"VALUES({item.CodCriterio},{item.Respuesta},{item.CodReqRangoCriteriosEvaluacion},{item.CodReqRtaCriteriosEvaluacion}) ");
                }

                foreach (ReqArtSerParticipante item in parametersReq.ReqParticipantesDto.ArtSers)
                {
                    cadenaArt.Append("INSERT INTO #ReqArtSerParticipanteTemp (CodArtSerRequerido,Observacion,Valor,Iva,Descuento,MarcaSolicitada) ");
                    cadenaArt.Append($"VALUES({item.CodArtSerRequerido},'{item.Observacion ?? ""}',{item.Valor},{item.Iva},{item.Descuento},{(item.MarcaSolicitada ? 1 : 0)}) ");
                }

                foreach (PrvDocumento item in parametersReq.PrvDocumentos)
                {
                    string queryInsert = "";
                    string queryInsertValues = "";

                    if (item.PrvdExpedicion is null)
                    {
                        queryInsert = "INSERT INTO #documentosProveedor (prvdCodDocumento, prvdNameDocument, prvdExtDocument, prvdSizeDocument, prvdUrlDocument, prvdUrlRelDocument, prvdOriginalNameDocument) ";
                        queryInsertValues = $"VALUES({item.PrvdCodDocumento},'{item.PrvdNameDocument}','{item.PrvdExtDocument}',{item.PrvdSizeDocument},'{item.PrvdUrlDocument}'," +
                        $"'{item.PrvdUrlRelDocument}','{item.PrvdOriginalNameDocument}') ";
                    }
                    else
                    {
                        queryInsert = "INSERT INTO #documentosProveedor (prvdCodDocumento, prvdNameDocument, prvdExtDocument, prvdSizeDocument, prvdUrlDocument, prvdUrlRelDocument, prvdOriginalNameDocument, prvdExpedicion) ";
                        queryInsertValues = $"VALUES({item.PrvdCodDocumento},'{item.PrvdNameDocument}','{item.PrvdExtDocument}',{item.PrvdSizeDocument},'{item.PrvdUrlDocument}','{item.PrvdUrlRelDocument}'," +
                        $"'{item.PrvdOriginalNameDocument}', '{item.PrvdExpedicion?.ToString("yyyyMMdd")}') ";
                    }

                    insertDocsProv.Append(queryInsert);
                    insertDocsProv.Append(queryInsertValues);
                }

                foreach (ReqDocumentosRequerido item in parametersReq.ReqDocumentosRequeridos)
                {
                    insertDocsOtros.Append("INSERT INTO #documentosOtros (rdrCodReqListDocumentos, rdrCodProveedor, rdrNameDocument, rdrExtDocument, rdrSizeDocument, rdrUrlDocument, rdrUrlRelDocument, rdrOriginalNameDocument) ");
                    insertDocsOtros.Append($"VALUES({item.RdrCodReqListDocumentos},{item.RdrCodProveedor},'{item.RdrNameDocument}','{item.RdrExtDocument}',{item.RdrSizeDocument},'{item.RdrUrlDocument}','{item.RdrUrlRelDocument}'," +
                        $"'{item.RdrOriginalNameDocument}') ");
                }

                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","1"),
                    new SqlParameter("@CodRequerimento",parametersReq.ReqParticipantesDto.CodRequerimento),
                    new SqlParameter("@CodProveedor",parametersReq.ReqParticipantesDto.CodProveedor),
                    new SqlParameter("@IsEnGuardadoTemporal",parametersReq.ReqParticipantesDto.IsEnGuardadoTemporal),
                    new SqlParameter("@FecOfePre",parametersReq.ReqParticipantesDto.FecOfePre.ToString("yyyyMMdd")),
                    new SqlParameter("@Observacion",parametersReq.ReqParticipantesDto.Observacion),
                    new SqlParameter("@CodUser",parametersReq.ReqParticipantesDto.CodUser),
                    new SqlParameter("@Cadena",cadena.ToString()),
                    new SqlParameter("@CadenaArt",cadenaArt.ToString()),
                    new SqlParameter("@insertDocsProv",insertDocsProv.ToString()),
                    new SqlParameter("@insertDocsOtros",insertDocsOtros.ToString())
                };

                string sql = $"req.SpReqParticipantes @Operacion = @Operacion, @CodRequerimento = @CodRequerimento, @IsEnGuardadoTemporal = @IsEnGuardadoTemporal, @CodProveedor = @CodProveedor, " +
                    $"@FecOfePre = @FecOfePre, @Observacion = @Observacion, @CodUser = @CodUser, @Cadena = @Cadena, @CadenaArt = @CadenaArt, @insertDocsProv = @insertDocsProv, @insertDocsOtros = @insertDocsOtros";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PutParticipar(ParamParticipacionReq parametersReq)
        {
            try
            {
                StringBuilder cadena = new StringBuilder();
                StringBuilder cadenaArt = new StringBuilder();
                StringBuilder insertDocsProv = new StringBuilder();
                StringBuilder insertDocsOtros = new StringBuilder();

                foreach (ReqRtaParticipante item in parametersReq.ReqParticipantesDto.Criterios)
                {
                    cadena.Append("INSERT INTO #ReqRtaParticipanteTemp (CodCriterio, Respuesta,CodReqRangoCriteriosEvaluacion,CodReqRtaCriteriosEvaluacion) ");
                    cadena.Append($"VALUES({item.CodCriterio},{item.Respuesta},{item.CodReqRangoCriteriosEvaluacion},{item.CodReqRtaCriteriosEvaluacion}) ");
                }

                foreach (ReqArtSerParticipante item in parametersReq.ReqParticipantesDto.ArtSers)
                {
                    cadenaArt.Append("INSERT INTO #ReqArtSerParticipanteTemp (CodArtSerRequerido,Observacion, Valor,Iva,Descuento,MarcaSolicitada) ");
                    cadenaArt.Append($"VALUES({item.CodArtSerRequerido},'{item.Observacion ?? ""}',{item.Valor},{item.Iva},{item.Descuento},{(item.MarcaSolicitada ? 1 : 0)}) ");
                }

                if (parametersReq.PrvDocumentos != null)
                {
                    foreach (PrvDocumento item in parametersReq.PrvDocumentos)
                    {
                        string queryInsert = "";
                        string queryInsertValues = "";

                        if (item.PrvdExpedicion is null)
                        {
                            queryInsert = "INSERT INTO #documentosProveedor (prvdCodDocumento, prvdNameDocument, prvdExtDocument, prvdSizeDocument, prvdUrlDocument, prvdUrlRelDocument, prvdOriginalNameDocument) ";
                            queryInsertValues = $"VALUES({item.PrvdCodDocumento},'{item.PrvdNameDocument}','{item.PrvdExtDocument}',{item.PrvdSizeDocument},'{item.PrvdUrlDocument}'," +
                            $"'{item.PrvdUrlRelDocument}','{item.PrvdOriginalNameDocument}') ";
                        }
                        else
                        {
                            queryInsert = "INSERT INTO #documentosProveedor (prvdCodDocumento, prvdNameDocument, prvdExtDocument, prvdSizeDocument, prvdUrlDocument, prvdUrlRelDocument, prvdOriginalNameDocument, prvdExpedicion) ";
                            queryInsertValues = $"VALUES({item.PrvdCodDocumento},'{item.PrvdNameDocument}','{item.PrvdExtDocument}',{item.PrvdSizeDocument},'{item.PrvdUrlDocument}','{item.PrvdUrlRelDocument}'," +
                            $"'{item.PrvdOriginalNameDocument}', '{item.PrvdExpedicion?.ToString("yyyyMMdd")}') ";
                        }

                        insertDocsProv.Append(queryInsert);
                        insertDocsProv.Append(queryInsertValues);
                    }
                }

                if (parametersReq.ReqDocumentosRequeridos != null)
                {
                    foreach (ReqDocumentosRequerido item in parametersReq.ReqDocumentosRequeridos)
                    {
                        insertDocsOtros.Append("INSERT INTO #documentosOtros (rdrCodReqListDocumentos, rdrCodProveedor, rdrNameDocument, rdrExtDocument, rdrSizeDocument, rdrUrlDocument, rdrUrlRelDocument, rdrOriginalNameDocument) ");
                        insertDocsOtros.Append($"VALUES({item.RdrCodReqListDocumentos},{item.RdrCodProveedor},'{item.RdrNameDocument}','{item.RdrExtDocument}',{item.RdrSizeDocument},'{item.RdrUrlDocument}','{item.RdrUrlRelDocument}'," +
                            $"'{item.RdrOriginalNameDocument}') ");
                    }
                }


                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","2"),
                    new SqlParameter("@IdReqParticipantes",parametersReq.ReqParticipantesDto.IdReqParticipantes),
                    new SqlParameter("@CodRequerimento",parametersReq.ReqParticipantesDto.CodRequerimento),
                    new SqlParameter("@CodProveedor",parametersReq.ReqParticipantesDto.CodProveedor),
                     new SqlParameter("@IsEnGuardadoTemporal",parametersReq.ReqParticipantesDto.IsEnGuardadoTemporal),
                    new SqlParameter("@FecOfePre",parametersReq.ReqParticipantesDto.FecOfePre.ToString("yyyyMMdd")),
                    new SqlParameter("@Observacion",parametersReq.ReqParticipantesDto.Observacion),
                    new SqlParameter("@CodUser",parametersReq.ReqParticipantesDto.CodUser),
                    new SqlParameter("@Cadena",cadena.ToString()),
                    new SqlParameter("@CadenaArt",cadenaArt.ToString()),
                    new SqlParameter("@insertDocsProv",insertDocsProv.ToString()),
                    new SqlParameter("@insertDocsOtros",insertDocsOtros.ToString())

                };

                string sql = $"req.SpReqParticipantes @Operacion = @Operacion, @IdReqParticipantes = @IdReqParticipantes,@IsEnGuardadoTemporal = @IsEnGuardadoTemporal, @CodRequerimento = @CodRequerimento, @CodProveedor = @CodProveedor, " +
                    $"@FecOfePre = @FecOfePre, @Observacion = @Observacion, @CodUser = @CodUser, @Cadena = @Cadena, @CadenaArt = @CadenaArt, @insertDocsProv = @insertDocsProv, @insertDocsOtros = @insertDocsOtros";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<Requerimientos>> GetRequerimientosAdjudicados(int user)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","15"),
                    new SqlParameter("@CodUser",user)
                };

                string sql = $"[req].[SpRequerimiento] @Operacion = @Operacion,@CodUser=@CodUser";

                var response = await _context.Requerimientos.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<int> GetCantidadParticipantesRequerimiento(int id)
        {
            try
            {
                SqlParameter[] @params =
                {
                    new SqlParameter("@Operacion","17"),
                    new SqlParameter("@reqIdRequerimientos",id),
                    new SqlParameter("@returnVal", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };

                await _context.Database.ExecuteSqlRawAsync($"EXEC [req].[SpRequerimiento] @Operacion = @Operacion, @reqIdRequerimientos = @reqIdRequerimientos, @contParticipantes = @returnVal OUTPUT", @params);
                var result = (int)@params[2].Value;
                return result;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<RequerimientoComparativoDto>> GetComparativoParticipantesRequerimiento(QuerySearchComparativaReq parametros, PathOptions pathOptions)
        {
            try
            {
                DataTable idsPrv = new DataTable();
                idsPrv.Columns.Add("Id", typeof(int));

                foreach (var item in parametros.IdsProveedores)
                {
                    idsPrv.Rows.Add(item);
                }

                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","18"),
                    new SqlParameter("@reqIdRequerimientos",parametros.IdRequerimiento),
                    new SqlParameter("@listadoIdsProveedores",idsPrv) { SqlDbType = SqlDbType.Structured, TypeName = "ListadoEnteros" },
                    new SqlParameter("@idPathFileServer",pathOptions.IdPathFileServer)
                };

                string sql = $"[req].[SpRequerimiento] @Operacion = @Operacion, @reqIdRequerimientos = @reqIdRequerimientos, @listadoIdsProveedores = @listadoIdsProveedores, @idPathFileServer = @idPathFileServer";

                var response = await _context.ResponseJsonStrings.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return BaseResponseJson<RootJsonResponse<RequerimientoComparativoDto>>.ConvertJsonToEntity(response).JsonResponse;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> UpdateItemValidadoComparativa(QueryUpdateItemComparativa comparativa)
        {
            try
            {
                DataTable idsPrv = new DataTable();
                idsPrv.Columns.Add("Id", typeof(int));

                foreach (var item in comparativa.ListIdReqArtSerParticipante)
                {
                    idsPrv.Rows.Add(item);
                }

                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","19"),
                    new SqlParameter("@listadoIdsProveedores",idsPrv) { SqlDbType = SqlDbType.Structured, TypeName = "ListadoEnteros" },
                    new SqlParameter("@ItemValido",comparativa.ItemValido),
                    new SqlParameter("@reqIdRequerimientos",comparativa.CodRequerimiento),
                    new SqlParameter("@CodUserUpdate",comparativa.CodUserUpdate)
                };

                string sql = $"[req].[SpRequerimiento] @Operacion = @Operacion, @listadoIdsProveedores = @listadoIdsProveedores, @ItemValido = @ItemValido, @reqIdRequerimientos = @reqIdRequerimientos, @CodUserUpdate = @CodUserUpdate";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<Requerimientos>> GetRequerimientosListosParaAdjudicar()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","20")
                };

                string sql = $"[req].[SpRequerimiento] @Operacion = @Operacion";

                var response = await _context.Requerimientos.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<RequerimientoAdjudicacionDto>> GetAdjudicacionParticipantesRequerimiento(QuerySearchComparativaReq parametros, PathOptions pathOptions)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","21"),
                    new SqlParameter("@reqIdRequerimientos",parametros.IdRequerimiento),
                    new SqlParameter("@idPathFileServer",pathOptions.IdPathFileServer)
                };

                string sql = $"[req].[SpRequerimiento] @Operacion = @Operacion, @reqIdRequerimientos = @reqIdRequerimientos, @idPathFileServer = @idPathFileServer";

                var response = await _context.ResponseJsonStrings.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return BaseResponseJson<RootJsonResponse<RequerimientoAdjudicacionDto>>.ConvertJsonToEntity(response).JsonResponse;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ParticipacionDataReporteDto>> GetDataParticipanteRequerimiento(QuerySearchDataParticipacionReq parametros)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","3"),
                    new SqlParameter("@CodRequerimento",parametros.IdRequerimiento),
                    new SqlParameter("@CodProveedor",parametros.IdProveedor)
                };

                string sql = $"[req].[SpReqParticipantes] @Operacion = @Operacion, @CodRequerimento = @CodRequerimento, @CodProveedor = @CodProveedor";

                var response = await _context.ResponseJsonStrings.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return BaseResponseJson<RootJsonResponse<ParticipacionDataReporteDto>>.ConvertJsonToEntity(response).JsonResponse;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> ActualizarUrlPdf(int? id, string url)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","4"),
                    new SqlParameter("@IdReqParticipantes", id),
                      new SqlParameter("@UrlPdfParticipacion", url)
                };

                string sql = $"[req].[SpReqParticipantes] @Operacion = @Operacion, @IdReqParticipantes = @IdReqParticipantes, @UrlPdfParticipacion = @UrlPdfParticipacion";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<PathArchivosArtSerReq>> GetDocumentosTipoArchivoReq(int IdReq)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","22"),
                    new SqlParameter("@reqIdRequerimientos", IdReq),
                };

                string sql = $"[req].[SpRequerimiento] @Operacion = @Operacion, @reqIdRequerimientos = @reqIdRequerimientos";

                var response = await _context.PathArchivosArtSerReqs.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ArticulosRequerimiento>> GetArticulosParaRequerimiento(QuerySearchSolicitudesApoteosys parametros)
        {
            try
            {
                SqlParameter[] parameters;
                string sql;

                if (parametros.Empresa.Count > 0)
                {
                    DataTable abrvEmpresas = new DataTable();
                    abrvEmpresas.Columns.Add("AbrevEmpresa", typeof(string));

                    foreach (var item in parametros.Empresa)
                    {
                        abrvEmpresas.Rows.Add(item);
                    }

                    parameters = new[] {
                        new SqlParameter("@Operacion",1),
                        new SqlParameter("@listadoAbrevEmpresas",abrvEmpresas) { SqlDbType = SqlDbType.Structured, TypeName = "ListadoAbrevEmpresas" },
                        new SqlParameter("@codigoArticulo",parametros.CodigoArticulo),
                        new SqlParameter("@tipoRequerimiento",parametros.TipoRequerimiento),
                        new SqlParameter("@estado",parametros.Estado),
                        new SqlParameter("@descripcion",parametros.Descripcion),
                        new SqlParameter("@fechaInicial",parametros.Fecha_Inicial.ToString("yyyy/MM/ddT00:00:00.000Z")),
                        new SqlParameter("@fechaFinal",parametros.Fecha_Final.ToString("yyyy/MM/ddT00:00:00.000Z")),
                    };

                    sql = $"[dbo].[SpConsultaArticulosApoteosys] @Operacion = @Operacion, @listadoAbrevEmpresas = @listadoAbrevEmpresas, @codigoArticulo = @codigoArticulo, @tipoRequerimiento = @tipoRequerimiento, @estado = @estado, " +
                        $"@descripcion = @descripcion, @fechaInicial = @fechaInicial, @fechaFinal = @fechaFinal";
                }
                else
                {
                    parameters = new[] {
                        new SqlParameter("@Operacion",1),
                        new SqlParameter("@codigoArticulo",parametros.CodigoArticulo),
                        new SqlParameter("@tipoRequerimiento",parametros.TipoRequerimiento),
                        new SqlParameter("@estado",parametros.Estado),
                        new SqlParameter("@descripcion",parametros.Descripcion),
                        new SqlParameter("@fechaInicial",parametros.Fecha_Inicial.ToString("yyyy/MM/ddT00:00:00.000Z")),
                        new SqlParameter("@fechaFinal",parametros.Fecha_Final.ToString("yyyy/MM/ddT00:00:00.000Z")),
                    };

                    sql = $"[dbo].[SpConsultaArticulosApoteosys] @Operacion = @Operacion, @codigoArticulo = @codigoArticulo, @tipoRequerimiento = @tipoRequerimiento, @estado = @estado, @descripcion = @descripcion, " +
                        $"@fechaInicial = @fechaInicial, @fechaFinal = @fechaFinal";
                }

                var response = await _context.ArticulosRequerimientos.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<AdjudicacionOrdenes>> GetAdjudicacionRequerimientoOrden(int idRequerimiento)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                        new SqlParameter("@idRequerimiento", idRequerimiento),
                    };

                string sql = $"[dbo].[SpConsultaOrdenesAdjudicacionApoteosys] @idRequerimiento = @idRequerimiento";

                var response = await _context.AdjudicacionOrdenes.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> CreateAdjudicarRequerimiento(QueryAdjudicarReq parametersAdj)
        {
            try
            {
                StringBuilder insertAdjDte = new StringBuilder();

                foreach (ReqAdjudicacionDetalleDto item in parametersAdj.ArticulosAdjudicar)
                {
                    insertAdjDte.Append("INSERT INTO #reqAdjudicacionDte (radteCodProveedor,radteCodArtSerRequerido,radteCantidadAdjudicada) ");
                    insertAdjDte.Append($"VALUES({item.RadteCodProveedor},{item.RadteCodArtSerRequerido},{item.RadteCantidadAdjudicada}) ");
                }

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Operacion", "1"),
                    new SqlParameter("@CodRequerimiento", parametersAdj.CodRequerimiento),
                    new SqlParameter("@TipoAdjudicacion", parametersAdj.TipoAdjudicacion),
                    new SqlParameter("@IsGuardadoTemporal",parametersAdj.RadjIsGuardadoTemporal),
                    new SqlParameter("@CodUser",parametersAdj.CodUser),
                    new SqlParameter("@InsertAdjDteTemp",insertAdjDte.ToString()),
                };

                if (!(parametersAdj.CodRequisitor is null) && parametersAdj.CodRequisitor != 0)
                {
                    parameters.Add(new SqlParameter("@CodRequisitorContrato", parametersAdj.CodRequisitor));
                }

                if (!(parametersAdj.CodProveedorSeleccionado is null) && parametersAdj.CodProveedorSeleccionado != 0)
                {
                    parameters.Add(new SqlParameter("@CodProveedorSelecTotal", parametersAdj.CodProveedorSeleccionado));
                }

                if (!(parametersAdj.CodGestor is null) && parametersAdj.CodGestor != 0)
                {
                    parameters.Add(new SqlParameter("@CodGestorContrato", parametersAdj.CodGestor));
                }

                string sql = $"[req].[SpAdjudicacion] @Operacion = @Operacion, @CodRequerimiento = @CodRequerimiento, @TipoAdjudicacion = @TipoAdjudicacion, " +
                    $"{ (parametersAdj.CodRequisitor != null ? " @CodRequisitorContrato = @CodRequisitorContrato, " : "") }" +
                    $"{ (parametersAdj.CodProveedorSeleccionado != null ? " @CodProveedorSelecTotal = @CodProveedorSelecTotal, " : "") }" +
                    $"{ (parametersAdj.CodGestor != null ? " @CodGestorContrato = @CodGestorContrato, " : "") }" +
                    $"@IsGuardadoTemporal = @IsGuardadoTemporal, @CodUser = @CodUser, @InsertAdjDteTemp = @InsertAdjDteTemp";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters.ToArray()).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> UpdateAdjudicarRequerimiento(QueryAdjudicarReq parametersAdj)
        {
            try
            {
                StringBuilder insertAdjDte = new StringBuilder();

                foreach (ReqAdjudicacionDetalleDto item in parametersAdj.ArticulosAdjudicar)
                {
                    insertAdjDte.Append("INSERT INTO #reqAdjudicacionDte (radteCodProveedor,radteCodArtSerRequerido,radteCantidadAdjudicada) ");
                    insertAdjDte.Append($"VALUES({item.RadteCodProveedor},{item.RadteCodArtSerRequerido},{item.RadteCantidadAdjudicada}) ");
                }

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Operacion","2"),
                    new SqlParameter("@IdReqAdjudicacion",parametersAdj.IdReqAdjudicacion),
                    new SqlParameter("@CodRequerimiento", parametersAdj.CodRequerimiento),
                    new SqlParameter("@TipoAdjudicacion", parametersAdj.TipoAdjudicacion),
                    new SqlParameter("@IsGuardadoTemporal",parametersAdj.RadjIsGuardadoTemporal),
                    new SqlParameter("@CodUserUpdate",parametersAdj.CodUserUpdate),
                    new SqlParameter("@InsertAdjDteTemp",insertAdjDte.ToString()),
                };

                if (!(parametersAdj.CodRequisitor is null) && parametersAdj.CodRequisitor != 0)
                {
                    parameters.Add(new SqlParameter("@CodRequisitorContrato", parametersAdj.CodRequisitor));
                }

                if (!(parametersAdj.CodProveedorSeleccionado is null) && parametersAdj.CodProveedorSeleccionado != 0)
                {
                    parameters.Add(new SqlParameter("@CodProveedorSelecTotal", parametersAdj.CodProveedorSeleccionado));
                }

                if (!(parametersAdj.CodGestor is null) && parametersAdj.CodGestor != 0)
                {
                    parameters.Add(new SqlParameter("@CodGestorContrato", parametersAdj.CodGestor));
                }

                string sql = $"[req].[SpAdjudicacion] @Operacion = @Operacion, @IdReqAdjudicacion = @IdReqAdjudicacion, @CodRequerimiento = @CodRequerimiento, @TipoAdjudicacion = @TipoAdjudicacion, " +
                    $"{ (parametersAdj.CodRequisitor != null ? " @CodRequisitorContrato = @CodRequisitorContrato, " : "") }" +
                    $"{ (parametersAdj.CodProveedorSeleccionado != null ? " @CodProveedorSelecTotal = @CodProveedorSelecTotal, " : "") }" +
                    $"{ (parametersAdj.CodGestor != null ? " @CodGestorContrato = @CodGestorContrato, " : "") }" +
                    $"@IsGuardadoTemporal = @IsGuardadoTemporal, @CodUserUpdate = @CodUserUpdate, @InsertAdjDteTemp = @InsertAdjDteTemp";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters.ToArray()).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> AsociarOrdenesRequerimiento(QueryAsociarOrdenesReq parametros)
        {
            try
            {
                DataTable asocOrd = new DataTable();
                asocOrd.Columns.Add("CodArtSerRequerido", typeof(int));
                asocOrd.Columns.Add("CodProveedor", typeof(int));
                asocOrd.Columns.Add("CodOrden", typeof(int));

                foreach (AsociacionOrdenes item in parametros.ListadoAsociacionOrdenes)
                {
                    DataRow row = asocOrd.NewRow();
                    row["CodArtSerRequerido"] = item.CodArtSerRequerido;
                    row["CodProveedor"] = item.CodProveedor;
                    row["CodOrden"] = item.CodOrden;
                    asocOrd.Rows.Add(row);
                }

                SqlParameter[] parameters = new[] {
                    new SqlParameter("@codRequerimiento", parametros.CodRequerimiento),
                    new SqlParameter("@listadoAsociacionOrdenesReq", asocOrd) { SqlDbType = SqlDbType.Structured, TypeName = "ListAsociacionOrdenesReq" },
                    new SqlParameter("@codUserUpdate", parametros.CodUserUpdate)
                };

                string sql = $"[req].[SpAsociarOrdenesRequerimiento] @codRequerimiento = @codRequerimiento, @listadoAsociacionOrdenesReq = @listadoAsociacionOrdenesReq, @codUserUpdate = @codUserUpdate";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
    }
}