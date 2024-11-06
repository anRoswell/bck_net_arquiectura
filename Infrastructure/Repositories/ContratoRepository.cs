namespace Infrastructure.Repositories
{
    using Core.CustomEntities;
    using Core.DTOs;
    using Core.Entities;
    using Core.Enumerations;
    using Core.Exceptions;
    using Core.Interfaces;
    using Core.ModelResponse;
    using Core.QueryFilters;
    using Core.Tools;
    using Dapper;
    using Infrastructure.Data;
    using Infrastructure.Utils;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ContratoRepository : BaseRepository<Contrato>, IContratoRepository
    {
        private readonly IOptions<Core.Options.PasosContratosOptions> _pasosContratosOptions;
        private readonly IDbConnection _dapperSource;

        public ContratoRepository(
            DbModelContext context,
            IOptions<Core.Options.PasosContratosOptions> pasosContratosOptions,
            IDbConnection dapperSource) : base(context)
        {
            _pasosContratosOptions = pasosContratosOptions;
            _dapperSource = dapperSource;
        }

        public async Task<List<Contrato>> SearchAll()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","1")
                };

                string sql = $"[cont].[SpContrato] @Operacion = @Operacion";

                var response = await _context.Contratos.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<Contrato>> SearchById(int id)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","2"),
                    new SqlParameter("@contIdContrato", id),
                };

                string sql = CallStoredProcedures.Contrato.BuscarContratoPorId;

                var response = await _context.Contratos.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> Post(ContratoDto contrato)
        {
            try
            {
                StringBuilder Aprobadores = new();
                StringBuilder DocumentosContrato = new();
                StringBuilder DocumentosReqProv = new();
                StringBuilder DocumentosReqProvOtros = new();
                StringBuilder DocumentosPolizas = new();
                StringBuilder DocumentosAGuardar = new();

                foreach (AprobadoresContratoDto item in contrato.Aprobadores)
                {
                    Aprobadores.Append("INSERT INTO #AprobadoresContrato (apcIdAprobadoresContrato, apcCodContrato, apcCodTipoAprobadoresContrato, apcCodRequisitor, apcAprobacion, apcJustificacion) ");
                    Aprobadores.Append($"VALUES ({item.Id},{item.ApcCodContrato},{item.ApcCodTipoAprobadoresContrato},{item.ApcCodRequisitor}, {(item.ApcAprobacion ? 1 : 0)}, '{item.ApcJustificacion}' ) ");
                }

                foreach (DocumentoContratoDto item in contrato.DocumentacionContrato) // Documentacion Contrato
                {
                    DocumentosContrato.Append(@"INSERT INTO #DocumentacionContratoTemp (Id, CodContrato, CodTipoDocumento, CodDocumento, DcPermisos, CodArchivo, KeyFile) ");
                    DocumentosContrato.Append(@$"VALUES({item.Id ?? 0}, {item.CodContrato}, {item.CodTipoDocumento}, {item.CodDocumento}, '{item.DcPermisos}', {item.CodArchivo}, {item.KeyFile}) ");
                }

                foreach (DocReqProveedorDto item in contrato.DocumentosPrv) // Docs proveedor
                {
                    DocumentosReqProv.Append("INSERT INTO #DocReqProveedorTemp (Id, CodContrato, CodDocumento, CodPrvDocumento, CodArchivo, TipoVersion, Obligatorio, KeyFile) ");
                    DocumentosReqProv.Append($"VALUES({item.Id ?? 0}, {item.DrpCodContrato}, {item.DrpCodDocumento}, {item.DrpCodPrvDocumento}, {item.CodArchivo}, {item.DrpTipoVersion ?? 1}, {Convert.ToInt32(item.DrpObligatorio)}, {item.KeyFile}) ");
                }

                foreach (DocReqProveedorOtroDto item in contrato.DocumentosPrvOtros) // Docs proveedor otros
                {
                    DocumentosReqProvOtros.Append("INSERT INTO #DocReqProveedorOtrosTemp (Id, CodContrato, CodDocumento, DrpoObligatorio, DrpoVigencia, DrpoNombreDocumento, CodArchivo) ");
                    DocumentosReqProvOtros.Append($"VALUES({item.Id}, {item.DrpoCodContrato}, {item.DrpoCodDocumento}, {Convert.ToInt32(item.DrpoObligatorio)}, {item.DrpoVigencia}, '{item.DrpoNombreDocumento}', {item.CodArchivo}) ");
                }

                foreach (DocReqPolizaDto item in contrato.DocumentosReqPoliza) // Docs Polizas
                {
                    DocumentosPolizas.Append("INSERT INTO #DocReqPolizaTemp (Id, DrpoCodContrato, DrpoCodTipoDocumento, DrpoTipoPoliza,  DrpoCobertura, DrpoVigencia, DrpoExpedida, DrpoAprobada, DrpoEstado, CodArchivo, KeyFile, DrpoFechaVencimiento, DrpoFechaEmision) ");
                    DocumentosPolizas.Append($"VALUES({item.Id ?? 0}, {item.DrpoCodContrato}, {item.DrpoCodTipoDocumento}, '{item.DrpoTipoPoliza}', '{item.DrpoCobertura}', '{item.DrpoVigencia}', {item.DrpoExpedida}, {item.DrpoAprobada}, {item.DrpoEstado}, {item.CodArchivo}, {item.KeyFile}, '{item.DrpoFechaVencimiento?.ToString("yyyyMMdd") ?? "19000101"}', '{item.DrpoFechaEmision?.ToString("yyyyMMdd") ?? "19000101"}')");
                }

                foreach (DocReqUploadDto item in contrato.DocumentosAguardar)
                {
                    DocumentosAGuardar.Append("INSERT INTO #DocReqUploadTemp (Id, DrpuUrl, DrcoNameDocument, DrcoExtDocument, DrcoSizeDocument, DrcoUrlDocument, DrcoUrlRelDocument, DrcoOriginalNameDocument, KeyFile)");
                    DocumentosAGuardar.Append($"VALUES({item.Id ?? 0}, '{item.DrpuUrl}', '{item.DrcoNameDocument}', '{item.DrcoExtDocument}', {item.DrcoSizeDocument ?? 0}, '{item.DrcoUrlDocument}', '{item.DrcoUrlRelDocument}', '{item.DrcoOriginalNameDocument}', {item.KeyFile})");
                }

                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","3"),
                    new SqlParameter("@contIdContrato",contrato.Id),
                    new SqlParameter("@ContCodEmpresa", contrato.ContCodEmpresa),
                    new SqlParameter("@ContCodProveedor", contrato.ContCodProveedor),
                    new SqlParameter("@ContCodRequerimiento", contrato.ContCodRequerimiento),
                    new SqlParameter("@ContNombreRteLegalCtante", contrato.ContNombreRteLegalCtante ?? ""),
                    new SqlParameter("@contEmailRteLegalCtante", contrato.ContEmailRteLegalCtante ?? ""),
                    new SqlParameter("@ContCcrepresentanteCtante", contrato.ContCcrepresentanteCtante ?? ""),
                    new SqlParameter("@ContDireccionNotificacionCtante", contrato.ContDireccionNotificacionCtante ?? ""),
                    new SqlParameter("@ContTelefonoCtante", contrato.ContTelefonoCtante ?? ""),
                    new SqlParameter("@ContCodGestorContrato", contrato.ContCodGestorContrato ?? 0),
                    new SqlParameter("@ContCodCoordinadorContrato", contrato.ContCodCoordinadorContrato ?? 0),
                    new SqlParameter("@ContCodGestorRiesgo", contrato.ContCodGestorRiesgo ?? 0),
                    new SqlParameter("@ContCodRequisitor", contrato.ContCodRequisitor ?? 0),
                    new SqlParameter("@ContCodUnidadNegocio", contrato.ContCodUnidadNegocio ?? 0),
                    new SqlParameter("@ContCodClaseContrato", contrato.ContCodClaseContrato ?? 0),
                    new SqlParameter("@ContAprobFinanciera", contrato.ContAprobFinanciera ?? 0),
                    new SqlParameter("@ContAprobCompras", contrato.ContAprobCompras ?? 0),
                    new SqlParameter("@ContAprobArea", contrato.ContAprobArea ?? 0),
                    new SqlParameter("@ContObjetoContrato", contrato.ContObjetoContrato ?? ""),
                    new SqlParameter("@ContCarateristicasEspecificas", contrato.ContCarateristicasEspecificas ?? ""),
                    new SqlParameter("@ContDuracionContrato", contrato.ContDuracionContrato ?? ""),
                    new SqlParameter("@ContVigenciaDesde", contrato.ContVigenciaDesde),
                    new SqlParameter("@ContVigenciaHasta", contrato.ContVigenciaHasta),
                    new SqlParameter("@ContValorContrato", contrato.ContValorContrato ?? 0),
                    new SqlParameter("@ContCodFormaPago", contrato.ContCodFormaPago ?? ""),
                    new SqlParameter("@ContRequierenAnticipos", contrato.ContRequierenAnticipos),
                    new SqlParameter("@ContValorAnticipo", contrato.ContValorAnticipo ?? 0),
                    new SqlParameter("@ContRequiereIngresoPersonal", contrato.ContRequiereIngresoPersonal),
                    new SqlParameter("@ContPresupuestado", contrato.ContPresupuestado),
                    new SqlParameter("@ContCodTipoProrroga", contrato.ContCodTipoProrroga ?? 0),
                    new SqlParameter("@ContDuracionProrroga", contrato.ContDuracionProrroga ?? 0),
                    new SqlParameter("@ContPreavisoProrrogaDias", contrato.ContPreavisoProrrogaDias ?? 0),
                    new SqlParameter("@ContRequiereActaInicio", contrato.ContRequiereActaInicio),
                    new SqlParameter("@ContRequiereActaLiquidacion", contrato.ContRequiereActaLiquidacion),
                    new SqlParameter("@ContFechaLiquidacionEsperada", contrato.ContFechaLiquidacionEsperada),
                    new SqlParameter("@ContTipoDocumento", contrato.ContTipoDocumento),
                    new SqlParameter("@ContCodEstado", contrato.ContCodEstado),
                    new SqlParameter("@ContObservacion",contrato.ContObservacion ?? ""),
                    new SqlParameter("@ContContactoContratista",contrato.ContContactoContratista ?? ""),
                    new SqlParameter("@ContEmailContactoContratista",contrato.ContEmailContactoContratista ?? ""),
                    new SqlParameter("@ContNitContratista",contrato.ContNitContratista ?? ""),
                    new SqlParameter("@CodUser", contrato.CodUser ?? ""),
                    new SqlParameter("@Info", contrato.Info ?? ""),
                    new SqlParameter("@CodUserUpdate", contrato.CodUserUpdate ?? ""),
                    new SqlParameter("@InfoUpdate", contrato.InfoUpdate ?? ""),
                    new SqlParameter("@cadenaAprobadores",Aprobadores.ToString()),
                    new SqlParameter("@cadenaDocumentosContrato",DocumentosContrato.ToString()),
                    new SqlParameter("@cadenaDocumentosProveedor",DocumentosReqProv.ToString()),
                    new SqlParameter("@cadenaDocumentosProveedorOtros",DocumentosReqProvOtros.ToString()),
                    new SqlParameter("@cadenaDocumentosPoliza",DocumentosPolizas.ToString()),
                    new SqlParameter("@cadenaArchivos",DocumentosAGuardar.ToString()),
                    new SqlParameter("@cadenaDocumentosContratoEliminados", contrato.DocumentacionContratoEliminados),
                    new SqlParameter("@cadenaDocumentosProveedorElimminados", contrato.DocumentosPrvEliminados),
                    new SqlParameter("@cadenaDocumentosProveedorOtrosElimminados", contrato.DocumentosPrvOtrosEliminados),
                    new SqlParameter("@cadenaDocumentosPolizaEliminados", contrato.DocumentosReqPolizaEliminados),
                    new SqlParameter("@ContArchivoCompraNoPresupuestada",contrato.ContArchivoCompraNoPresupuestada ?? 0),
                    new SqlParameter("@KeyFileCompraNoProsupuestada",contrato.KeyFileCompraNoProsupuestada ?? 0),
                    new SqlParameter("@ContEmailContratante",contrato.ContEmailContratante ?? ""),
                    new SqlParameter("@ContArchivoActaInicio",contrato.ContArchivoActaInicio ?? 0),
                    new SqlParameter("@KeyFileActaInicio",contrato.KeyFileActaInicio ?? 0),
                    new SqlParameter("@ContConsecutivoAlterno", contrato.ContConsecutivoAlterno ?? ""),
                    new SqlParameter("@ContFechaActaInicio", contrato.ContFechaActaInicio ?? contrato.ContVigenciaDesde),
                    new SqlParameter("@ContCodContratoHistoricoActual", contrato.ContCodContratoHistoricoActual ?? 0),
                    new SqlParameter("@ContCodTipoContrato", contrato.ContCodTipoContrato),
                    new SqlParameter("@ContCodRepresentanteLegal", contrato.ContCodRepresentanteLegal)
                };

                string sql = CallStoredProcedures.Contrato.CrearContrato;
                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> Update(ContratoDto contrato)
        {
            try
            {
                StringBuilder Aprobadores = new();
                StringBuilder DocumentosContrato = new();
                StringBuilder DocumentosReqProv = new();
                StringBuilder DocumentosReqProvOtros = new();
                StringBuilder DocumentosPolizas = new();
                StringBuilder DocumentosAGuardar = new();

                foreach (AprobadoresContratoDto item in contrato.Aprobadores)
                {
                    Aprobadores.Append("INSERT INTO #AprobadoresContrato (apcIdAprobadoresContrato, apcCodContrato, apcCodTipoAprobadoresContrato, apcCodRequisitor, apcAprobacion, apcJustificacion) ");
                    Aprobadores.Append($"VALUES ({item.Id},{item.ApcCodContrato},{item.ApcCodTipoAprobadoresContrato},{item.ApcCodRequisitor}, {(item.ApcAprobacion ? 1 : 0)}, '{item.ApcJustificacion}' ) ");
                }

                foreach (DocumentoContratoDto item in contrato.DocumentacionContrato) // Documentacion Contrato
                {
                    DocumentosContrato.Append(@"INSERT INTO #DocumentacionContratoTemp (Id, CodContrato, CodTipoDocumento, CodDocumento, DcPermisos, CodArchivo, KeyFile) ");
                    DocumentosContrato.Append(@$"VALUES({item.Id ?? 0}, {item.CodContrato}, {item.CodTipoDocumento}, {item.CodDocumento}, '{item.DcPermisos}', {item.CodArchivo}, {item.KeyFile}) ");
                }

                foreach (DocReqProveedorDto item in contrato.DocumentosPrv) // Docs proveedor
                {
                    DocumentosReqProv.Append("INSERT INTO #DocReqProveedorTemp (Id, CodContrato, CodDocumento, CodPrvDocumento, CodArchivo, TipoVersion, Obligatorio, KeyFile) ");
                    DocumentosReqProv.Append($"VALUES({item.Id ?? 0}, {item.DrpCodContrato}, {item.DrpCodDocumento}, {item.DrpCodPrvDocumento}, {item.CodArchivo}, {item.DrpTipoVersion ?? 1}, {Convert.ToInt32(item.DrpObligatorio)}, {item.KeyFile}) ");
                }

                foreach (DocReqProveedorOtroDto item in contrato.DocumentosPrvOtros) // Docs proveedor otros
                {
                    DocumentosReqProvOtros.Append("INSERT INTO #DocReqProveedorOtrosTemp (Id, CodContrato, CodDocumento, DrpoObligatorio, DrpoVigencia, DrpoNombreDocumento, CodArchivo) ");
                    DocumentosReqProvOtros.Append($"VALUES({item.Id}, {item.DrpoCodContrato}, {item.DrpoCodDocumento}, {Convert.ToInt32(item.DrpoObligatorio)}, {item.DrpoVigencia}, '{item.DrpoNombreDocumento}', {item.CodArchivo}) ");
                }

                foreach (DocReqPolizaDto item in contrato.DocumentosReqPoliza) // Docs Polizas
                {
                    DocumentosPolizas.Append("INSERT INTO #DocReqPolizaTemp (Id, DrpoCodContrato, DrpoCodTipoDocumento, DrpoTipoPoliza,  DrpoCobertura, DrpoVigencia, DrpoExpedida, DrpoAprobada, DrpoEstado, CodArchivo, KeyFile, DrpoFechaVencimiento, DrpoFechaEmision) ");
                    DocumentosPolizas.Append($"VALUES({item.Id ?? 0}, {item.DrpoCodContrato}, {item.DrpoCodTipoDocumento}, '{item.DrpoTipoPoliza}', '{item.DrpoCobertura}', '{item.DrpoVigencia}', {item.DrpoExpedida}, {item.DrpoAprobada}, {item.DrpoEstado}, {item.CodArchivo}, {item.KeyFile}, '{item.DrpoFechaVencimiento?.ToString("yyyyMMdd") ?? "19000101"}', '{item.DrpoFechaEmision?.ToString("yyyyMMdd") ?? "19000101"}')");
                }

                foreach (DocReqUploadDto item in contrato.DocumentosAguardar)
                {
                    DocumentosAGuardar.Append("INSERT INTO #DocReqUploadTemp (Id, DrpuUrl, DrcoNameDocument, DrcoExtDocument, DrcoSizeDocument, DrcoUrlDocument, DrcoUrlRelDocument, DrcoOriginalNameDocument, KeyFile)");
                    DocumentosAGuardar.Append($"VALUES({item.Id ?? 0}, '{item.DrpuUrl}', '{item.DrcoNameDocument}', '{item.DrcoExtDocument}', {item.DrcoSizeDocument ?? 0}, '{item.DrcoUrlDocument}', '{item.DrcoUrlRelDocument}', '{item.DrcoOriginalNameDocument}', {item.KeyFile})");
                }

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Operacion","4"),
                    new SqlParameter("@contIdContrato",contrato.Id),
                    new SqlParameter("@ContCodEmpresa", contrato.ContCodEmpresa),
                    new SqlParameter("@ContCodProveedor", contrato.ContCodProveedor),
                    new SqlParameter("@ContCodRequerimiento", contrato.ContCodRequerimiento ?? 0),
                    new SqlParameter("@ContNombreRteLegalCtante", contrato.ContNombreRteLegalCtante ?? ""),
                    new SqlParameter("@contEmailRteLegalCtante", contrato.ContEmailRteLegalCtante ?? ""),
                    new SqlParameter("@ContCcrepresentanteCtante", contrato.ContCcrepresentanteCtante ?? ""),
                    new SqlParameter("@ContDireccionNotificacionCtante", contrato.ContDireccionNotificacionCtante ?? ""),
                    new SqlParameter("@ContTelefonoCtante", contrato.ContTelefonoCtante ?? ""),
                    new SqlParameter("@ContCodGestorContrato", contrato.ContCodGestorContrato ?? 0),
                    new SqlParameter("@ContCodCoordinadorContrato", contrato.ContCodCoordinadorContrato ?? 0),
                    new SqlParameter("@ContCodGestorRiesgo", contrato.ContCodGestorRiesgo ?? 0),
                    new SqlParameter("@ContCodUnidadNegocio", contrato.ContCodUnidadNegocio ?? 0),
                    new SqlParameter("@ContCodClaseContrato", contrato.ContCodClaseContrato ?? 0),
                    new SqlParameter("@ContAprobFinanciera", contrato.ContAprobFinanciera ?? 0),
                    new SqlParameter("@ContAprobCompras", contrato.ContAprobCompras ?? 0),
                    new SqlParameter("@ContAprobArea", contrato.ContAprobArea ?? 0),
                    new SqlParameter("@ContObjetoContrato", contrato.ContObjetoContrato ?? ""),
                    new SqlParameter("@ContCarateristicasEspecificas", contrato.ContCarateristicasEspecificas ?? ""),
                    new SqlParameter("@ContDuracionContrato", contrato.ContDuracionContrato ?? ""),
                    new SqlParameter("@ContVigenciaDesde", contrato.ContVigenciaDesde),
                    new SqlParameter("@ContVigenciaHasta", contrato.ContVigenciaHasta),
                    new SqlParameter("@ContValorContrato", contrato.ContValorContrato ?? 0),
                    new SqlParameter("@ContCodFormaPago", contrato.ContCodFormaPago ?? ""),
                    new SqlParameter("@ContRequierenAnticipos", contrato.ContRequierenAnticipos),
                    new SqlParameter("@ContValorAnticipo", contrato.ContValorAnticipo ?? 0),
                    new SqlParameter("@ContRequiereIngresoPersonal", contrato.ContRequiereIngresoPersonal),
                    new SqlParameter("@ContPresupuestado", contrato.ContPresupuestado),
                    new SqlParameter("@ContCodTipoProrroga", contrato.ContCodTipoProrroga ?? 0),
                    new SqlParameter("@ContDuracionProrroga", contrato.ContDuracionProrroga ?? 0),
                    new SqlParameter("@ContPreavisoProrrogaDias", contrato.ContPreavisoProrrogaDias ?? 0),
                    new SqlParameter("@ContRequiereActaInicio", contrato.ContRequiereActaInicio),
                    new SqlParameter("@ContRequiereActaLiquidacion", contrato.ContRequiereActaLiquidacion),
                    new SqlParameter("@ContFechaLiquidacionEsperada", contrato.ContFechaLiquidacionEsperada),
                    new SqlParameter("@ContTipoDocumento", contrato.ContTipoDocumento),
                    new SqlParameter("@ContCodEstado", contrato.ContCodEstado),
                    new SqlParameter("@ContObservacion",contrato.ContObservacion ?? ""),
                    new SqlParameter("@ContContactoContratista",contrato.ContContactoContratista ?? ""),
                    new SqlParameter("@ContEmailContactoContratista",contrato.ContEmailContactoContratista ?? ""),
                    new SqlParameter("@ContNitContratista",contrato.ContNitContratista ?? ""),
                    new SqlParameter("@CodUserUpdate", contrato.CodUserUpdate ?? ""),
                    new SqlParameter("@InfoUpdate", contrato.InfoUpdate ?? ""),
                    new SqlParameter("@cadenaAprobadores",Aprobadores.ToString()),
                    new SqlParameter("@cadenaDocumentosContrato",DocumentosContrato.ToString()),
                    new SqlParameter("@cadenaDocumentosProveedor",DocumentosReqProv.ToString()),
                    new SqlParameter("@cadenaDocumentosProveedorOtros",DocumentosReqProvOtros.ToString()),
                    new SqlParameter("@cadenaDocumentosPoliza",DocumentosPolizas.ToString()),
                    new SqlParameter("@cadenaArchivos",DocumentosAGuardar.ToString()),
                    new SqlParameter("@cadenaDocumentosContratoEliminados", contrato.DocumentacionContratoEliminados),
                    new SqlParameter("@cadenaDocumentosProveedorElimminados", contrato.DocumentosPrvEliminados),
                    new SqlParameter("@cadenaDocumentosProveedorOtrosElimminados", contrato.DocumentosPrvOtrosEliminados),
                    new SqlParameter("@cadenaDocumentosPolizaEliminados", contrato.DocumentosReqPolizaEliminados),
                    new SqlParameter("@ContArchivoCompraNoPresupuestada",contrato.ContArchivoCompraNoPresupuestada ?? 0),
                    new SqlParameter("@KeyFileCompraNoProsupuestada",contrato.KeyFileCompraNoProsupuestada ?? 0),
                    new SqlParameter("@ContEmailContratante",contrato.ContEmailContratante ?? ""),
                    new SqlParameter("@ContArchivoActaInicio",contrato.ContArchivoActaInicio ?? 0),
                    new SqlParameter("@KeyFileActaInicio",contrato.KeyFileActaInicio ?? 0),
                    new SqlParameter("@ContConsecutivoAlterno", contrato.ContConsecutivoAlterno ?? ""),
                    new SqlParameter("@ContFechaActaInicio", contrato.ContFechaActaInicio ?? contrato.ContVigenciaDesde),
                    new SqlParameter("@ContCodContratoHistoricoActual", contrato.ContCodContratoHistoricoActual ?? 0),
                    new SqlParameter("@ContCodTipoContrato", contrato.ContCodTipoContrato),
                    new SqlParameter("@ContCodRepresentanteLegal", contrato.ContCodRepresentanteLegal)
                };

                string sql = CallStoredProcedures.Contrato.EditarContrato;

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<DocumentoContrato>> GetDocumentoContratoById(int id)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","5"),
                    new SqlParameter("@contIdContrato",id)
                };

                string sql = CallStoredProcedures.Contrato.BuscarDocumentosContratoPorId;

                var response = await _context.DocumentosContrato.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<PrvDocumentoDocReqProveedor>> GetDocReqProveedorById(int id)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","6"),
                    new SqlParameter("@contIdContrato",id)
                };

                string sql = CallStoredProcedures.Contrato.BuscarDocumentosReqContById;

                var response = await _context.PrvDocumentoDocReqProveedors.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<DocReqPoliza>> GetDocReqPolizaById(int id)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","7"),
                    new SqlParameter("@contIdContrato",id)
                };

                string sql = CallStoredProcedures.Contrato.BuscarDocumentosReqContOtroById;

                var response = await _context.DocReqPolizas.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> EliminarDocContrato(int id)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","8"),
                    new SqlParameter("@contIdContrato",id)
                };

                string sql = CallStoredProcedures.Contrato.EliminarContrato;

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<ParametrosContrato> GetParametrosContrato()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","9")
                };

                string sql = CallStoredProcedures.Contrato.ParametrosContrato;

                var response = await _context.ResponseJsonStrings.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return BaseResponseJson<RootJsonResponse<ParametrosContrato>>.ConvertJsonToEntity(response).JsonResponse.FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> UpdateEstado(QueryContrato par)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","10"),
                    new SqlParameter("@contIdContrato", par.IdContrato),
                    new SqlParameter("@contCodEstado", par.ContEstado),
                    new SqlParameter("@CodUserUpdate", par.CodUserUpdate),
                    new SqlParameter("@ContConsecutivoAlterno", par.ContConsecutivoAlterno ?? ""),
                };

                string sql = CallStoredProcedures.Contrato.CambiarEstadoContrato;

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> AprobacionContrato(QueryAprobacionContrato par)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","11"),
                    new SqlParameter("@apcIdAprobadoresContrato", par.IdAprobadoresContrato),
                    new SqlParameter("@apcAprobacion", par.Aprobacion),
                    new SqlParameter("@apcJustificacion", par.Justificacion ?? ""),
                    new SqlParameter("@CodUserUpdate", par.CodUserUpdate),
                };

                string sql = CallStoredProcedures.Contrato.AprobacionActoresContrato;

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<AprobadoresContrato>> GetAbrobadoresContratoById(int id)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","12"),
                    new SqlParameter("@contIdContrato",id)
                };

                string sql = CallStoredProcedures.Contrato.BuscarAprobadoresContratoById;

                var response = await _context.AprobadoresContratos.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<TimelineDto>> GetTimelineContratoById(int id)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","1"),
                    new SqlParameter("@pcnCodContrato", id)
                };

                string sql = CallStoredProcedures.PasosContrato.BuscarTimelineContratoById;

                var response = await _context.ResponseJsonStrings.FromSqlRaw(sql, parameters: parameters).ToListAsync();

                if (response?.FirstOrDefault()?.JsonResponse is null)
                    return new List<TimelineDto>();

                return BaseResponseJson<RootJsonResponse<TimelineDto>>.ConvertJsonToEntity(response).JsonResponse;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<ContratoDetalle> GetContratoDetallePorID(int id)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","13"),
                    new SqlParameter("@contIdContrato", id)
                };

                string sql = CallStoredProcedures.Contrato.BuscarDetalleDelContratoPorId;

                var response = await _context.ResponseJsonStrings.FromSqlRaw(sql, parameters: parameters).ToListAsync();

                var responseEntity = BaseResponseJson<RootJsonResponse<ContratoDetalle>>.ConvertJsonToEntity(response).JsonResponse.FirstOrDefault();

                responseEntity.HistoricosContrato = _context.ContratoHistoricos?
                                                            .Where((ch) => ch.ContCodContrato == id)
                                                            .Include(ach => ach.AprobadoresContratoHistoricos)
                                                            .Include(dch => dch.DocumentoContratoHistoricos)
                                                            .Include(dph => dph.DocReqProveedorHistoricos)
                                                            .Include(dpoh => dpoh.DocReqProveedorOtrosHistoricos)
                                                            .Include(ph => ph.DocReqPolizaHistoricos)
                                                            .ToList();

                return responseEntity;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> AsociarDocumentosProveedor(DocumentosProveedorDto documentos)
        {
            try
            {
                StringBuilder DocumentosReqProv = new StringBuilder();
                StringBuilder DocumentosReqProvOtros = new StringBuilder();
                StringBuilder DocumentosAGuardar = new StringBuilder();

                foreach (DocReqProveedorDto item in documentos.DocumentosPrv) // Docs proveedor
                {
                    DocumentosReqProv.Append("INSERT INTO #DocReqProveedorTemp (Id, CodContrato, CodDocumento, CodPrvDocumento, CodArchivo, KeyFile, TipoVersion, Obligatorio) ");
                    DocumentosReqProv.Append($"VALUES({item.Id ?? 0}, {item.DrpCodContrato}, {item.DrpCodDocumento}, {item.DrpCodPrvDocumento}, {item.CodArchivo}, {item.KeyFile}, {item.DrpTipoVersion}, {Convert.ToInt32(item.DrpObligatorio)}) ");
                }

                foreach (DocReqProveedorOtroDto item in documentos.DocumentosPrvOtros) // Docs proveedor otros
                {
                    DocumentosReqProvOtros.Append("INSERT INTO #DocReqProveedorOtrosTemp (Id, CodContrato, CodDocumento, DrpoObligatorio, DrpoVigencia, DrpoNombreDocumento, CodArchivo, KeyFile) ");
                    DocumentosReqProvOtros.Append($"VALUES({item.Id}, {item.DrpoCodContrato}, {item.DrpoCodDocumento}, {Convert.ToInt32(item.DrpoObligatorio)}, {item.DrpoVigencia}, '{item.DrpoNombreDocumento}', {item.CodArchivo}, {item.KeyFile}) ");
                }

                foreach (DocReqUploadDto item in documentos.DocumentosAguardar)
                {
                    DocumentosAGuardar.Append("INSERT INTO #DocReqUploadTemp (Id, DrpuUrl, DrcoNameDocument, DrcoExtDocument, DrcoSizeDocument, DrcoUrlDocument, DrcoUrlRelDocument, DrcoOriginalNameDocument, KeyFile)");
                    DocumentosAGuardar.Append($"VALUES({item.Id ?? 0}, '{item.DrpuUrl}', '{item.DrcoNameDocument}', '{item.DrcoExtDocument}', {item.DrcoSizeDocument ?? 0}, '{item.DrcoUrlDocument}', '{item.DrcoUrlRelDocument}', '{item.DrcoOriginalNameDocument}', {item.KeyFile})");
                }

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Operacion","14"),
                    new SqlParameter("@contIdContrato",documentos.IdContrato),
                    new SqlParameter("@cadenaDocumentosProveedor",DocumentosReqProv.ToString() ?? ""),
                    new SqlParameter("@cadenaDocumentosProveedorOtros",DocumentosReqProvOtros.ToString() ?? ""),
                    new SqlParameter("@cadenaArchivos",DocumentosAGuardar.ToString() ?? ""),
                    new SqlParameter("@cadenaDocumentosProveedorElimminados", documentos.DocumentosPrvEliminados ?? ""),
                    new SqlParameter("@cadenaDocumentosProveedorOtrosElimminados", documentos.DocumentosPrvOtrosEliminados ?? ""),
                    new SqlParameter("@CodUserUpdate", documentos.CodUserUpdate ?? ""),
                };

                string sql = CallStoredProcedures.DocumentosProveedor.AsociarDocumentos;

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> UpdateUrlContrato(QueryUpdateUrlContrato par)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","15"),
                    new SqlParameter("@contIdContrato", par.CodContrato),
                    new SqlParameter("@contUrlPdf", par.Url),
                    new SqlParameter("@contUrlAbsolutePdf", par.UrlAbsolute),
                    new SqlParameter("@CodUserUpdate", par.CodUserUpdate),
                };

                string sql = CallStoredProcedures.Contrato.UpdateUrlContrato;
                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> UpdateUrlContrato_CargadoManual(QueryUpdateUrlContrato par)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","16"),
                    new SqlParameter("@contIdContrato", par.CodContrato),
                    new SqlParameter("@contUrlPdf", par.Url),
                    new SqlParameter("@contUrlAbsolutePdf", par.UrlAbsolute),
                    new SqlParameter("@CodUserUpdate", par.CodUserUpdate),
                };

                string sql = CallStoredProcedures.Contrato.UpdateUrlContrato;
                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> SolicitarDocumentosProveedor(QuerySolicitarDocumentosProveedor par)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","17"),
                    new SqlParameter("@contIdContrato", par.IdContrato),
                    new SqlParameter("@CodUserUpdate", par.CodUserUpdate),
                };
                string sql = CallStoredProcedures.Contrato.SolicitarDocumentosProveedor;
                return await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<ParametrosContrato> SearchByProveedor(int idUsuario)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","18"),
                    new SqlParameter("@CodUser", idUsuario),
                };
                string sql = CallStoredProcedures.Contrato.BuscarContratosDeProveedor;

                var response = await _context.ResponseJsonStrings.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return BaseResponseJson<RootJsonResponse<ParametrosContrato>>.ConvertJsonToEntity(response).JsonResponse.FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> RechazarDocumentosProveedor(QueryRechazarDocumentosProveedor par)
        {
            try
            {
                StringBuilder DocumentosReqProv = new StringBuilder();
                StringBuilder DocumentosAGuardar = new StringBuilder();

                foreach (DocReqProveedorDto item in par.DocumentosPrv) // Docs proveedor
                {
                    DocumentosReqProv
                        .Append("INSERT INTO #DocReqProveedorTemp (Id, CodContrato, CodDocumento, CodPrvDocumento, CodArchivo, KeyFile, Aprobado, TipoVersion, Obligatorio) ")
                        .Append($"VALUES({item.Id ?? 0}, {item.DrpCodContrato}, {item.DrpCodDocumento}, {item.DrpCodPrvDocumento}, {item.CodArchivo}, {item.KeyFile}, {Convert.ToInt32(item.DrpAprobado)}, {item.DrpTipoVersion}, {Convert.ToInt32(item.DrpObligatorio)}) ");
                }

                foreach (DocReqUploadDto item in par.DocumentosAguardar)
                {
                    DocumentosAGuardar.Append("INSERT INTO #DocReqUploadTemp (Id, DrpuUrl, DrcoNameDocument, DrcoExtDocument, DrcoSizeDocument, DrcoUrlDocument, DrcoUrlRelDocument, DrcoOriginalNameDocument, KeyFile)");
                    DocumentosAGuardar.Append($"VALUES({item.Id ?? 0}, '{item.DrpuUrl}', '{item.DrcoNameDocument}', '{item.DrcoExtDocument}', {item.DrcoSizeDocument ?? 0}, '{item.DrcoUrlDocument}', '{item.DrcoUrlRelDocument}', '{item.DrcoOriginalNameDocument}', {item.KeyFile})");
                }

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Operacion","19"),
                    new SqlParameter("@contIdContrato",par.ContIdContrato),
                    new SqlParameter("@contJustificacionRechazo",par.ContJustificacionRechazo),
                    new SqlParameter("@cadenaArchivos",DocumentosAGuardar.ToString()),
                    new SqlParameter("@cadenaDocumentosProveedor",DocumentosReqProv.ToString() ?? ""),
                    new SqlParameter("@cadenaDocumentosProveedorElimminados", par.DocumentosPrvEliminados),
                    new SqlParameter("@CodUserUpdate", par.CodUserUpdate ?? ""),
                };

                string sql = CallStoredProcedures.DocumentosProveedor.RechazarDocumentos;

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> GuardarSeguimiento(SeguimientosContratoDto par)
        {
            try
            {
                StringBuilder DocumentosAGuardar = new StringBuilder();

                if (par.DocumentoAGuardar != null)
                {
                    DocumentosAGuardar.Append("INSERT INTO #DocReqUploadTemp (Id, DrpuUrl, DrcoNameDocument, DrcoExtDocument, DrcoSizeDocument, DrcoUrlDocument, DrcoUrlRelDocument, DrcoOriginalNameDocument, KeyFile)");
                    DocumentosAGuardar.Append($"VALUES({par.DocumentoAGuardar.Id ?? 0}, '{par.DocumentoAGuardar.DrpuUrl}', '{par.DocumentoAGuardar.DrcoNameDocument}', '{par.DocumentoAGuardar.DrcoExtDocument}', {par.DocumentoAGuardar.DrcoSizeDocument ?? 0}, '{par.DocumentoAGuardar.DrcoUrlDocument}', '{par.DocumentoAGuardar.DrcoUrlRelDocument}', '{par.DocumentoAGuardar.DrcoOriginalNameDocument}', {par.DocumentoAGuardar.KeyFile})");
                }

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Operacion","20"),
                    new SqlParameter("@contIdContrato",par.ScoCodContrato),
                    new SqlParameter("@scoIdSeguimiento",par.Id),
                    new SqlParameter("@scoFecha",par.ScoFecha),
                    new SqlParameter("@scoObservacion",par.ScoObservacion),
                    new SqlParameter("@scoPagosEfectuados", par.ScoPagosEfectuados),
                    new SqlParameter("@codArchivoSeguimiento", par.CodArchivo),
                    new SqlParameter("@keyFileSeguimiento", par.KeyFile),
                    new SqlParameter("@cadenaArchivos",DocumentosAGuardar.ToString()),
                    new SqlParameter("@CodUserUpdate", par.CodUserUpdate ?? ""),
                    new SqlParameter("@scoTipo", par.ScoTipo ?? ""),
                    new SqlParameter("@contCodGestorContrato", par.CodGestor ?? 0),
                    new SqlParameter("@contCodGestorRiesgo", par.CodGestorRiesgo ?? 0),
                };

                string sql = CallStoredProcedures.Seguimientos.CrearSeguimiento;

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> GuardarNotificacionNoProrroga(ContratoNotificacionNoProrroga par)
        {
            try
            {
                StringBuilder DocumentosAGuardar = new StringBuilder();

                if (par.DocumentoAGuardar != null)
                {
                    DocumentosAGuardar.Append("INSERT INTO #DocReqUploadTemp (Id, DrpuUrl, DrcoNameDocument, DrcoExtDocument, DrcoSizeDocument, DrcoUrlDocument, DrcoUrlRelDocument, DrcoOriginalNameDocument, KeyFile)");
                    DocumentosAGuardar.Append($"VALUES({par.DocumentoAGuardar.Id ?? 0}, '{par.DocumentoAGuardar.DrpuUrl}', '{par.DocumentoAGuardar.DrcoNameDocument}', '{par.DocumentoAGuardar.DrcoExtDocument}', {par.DocumentoAGuardar.DrcoSizeDocument ?? 0}, '{par.DocumentoAGuardar.DrcoUrlDocument}', '{par.DocumentoAGuardar.DrcoUrlRelDocument}', '{par.DocumentoAGuardar.DrcoOriginalNameDocument}', {par.DocumentoAGuardar.KeyFile})");
                }

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Operacion","21"),
                    new SqlParameter("@contIdContrato",par.IdContrato),
                    new SqlParameter("@contFechaNotificacionNoProrroga", par.FechaNotificacion.ToString("yyyyMMdd")),
                    new SqlParameter("@CodArchivoNotificacionNoProrroga", par.CodArchivoNotificacionNoProrroga),
                    new SqlParameter("@contObservacionNoProrroga", par.Observacion),
                    new SqlParameter("@KeyFileArchivoNotificacionNoProrroga", par.KeyFileArchivoNotificacionNoProrroga),
                    new SqlParameter("@cadenaArchivos",DocumentosAGuardar.ToString()),
                    new SqlParameter("@CodUserUpdate", par.CodUserUpdate ?? ""),
                };

                string sql = CallStoredProcedures.Contrato.GuardarNotificacionNoProrroga;

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> GuardarNotificacionTerminacionAnticipada(ContratoNotificacionTerminacion par)
        {
            try
            {
                StringBuilder DocumentosAGuardar = new StringBuilder();

                if (par.DocumentoAGuardar != null)
                {
                    DocumentosAGuardar.Append("INSERT INTO #DocReqUploadTemp (Id, DrpuUrl, DrcoNameDocument, DrcoExtDocument, DrcoSizeDocument, DrcoUrlDocument, DrcoUrlRelDocument, DrcoOriginalNameDocument, KeyFile)");
                    DocumentosAGuardar.Append($"VALUES({par.DocumentoAGuardar.Id ?? 0}, '{par.DocumentoAGuardar.DrpuUrl}', '{par.DocumentoAGuardar.DrcoNameDocument}', '{par.DocumentoAGuardar.DrcoExtDocument}', {par.DocumentoAGuardar.DrcoSizeDocument ?? 0}, '{par.DocumentoAGuardar.DrcoUrlDocument}', '{par.DocumentoAGuardar.DrcoUrlRelDocument}', '{par.DocumentoAGuardar.DrcoOriginalNameDocument}', {par.DocumentoAGuardar.KeyFile})");
                }

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Operacion","22"),
                    new SqlParameter("@contIdContrato",par.IdContrato),
                    new SqlParameter("@contFechaNotifTermAnticipada", par.FechaNotificacion.ToString("yyyyMMdd")),
                    new SqlParameter("@CodArchivoNotificacionTerminacion", par.CodArchivoNotificacionTerminacion),
                    new SqlParameter("@contObservacionTermAnticipada", par.Observacion),
                    new SqlParameter("@KeyFileArchivoNotificacionTerminacion", par.KeyFileArchivoNotificacionTerminacion),
                    new SqlParameter("@cadenaArchivos",DocumentosAGuardar.ToString()),
                    new SqlParameter("@CodUserUpdate", par.CodUserUpdate ?? ""),
                };

                string sql = CallStoredProcedures.Contrato.GuardarNotificacionTerminacionAnticipada;

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> AprobarProrroga(QueryAprobarProrroga par)
        {
            try
            {
                var contrato = await GetById(par.IdContrato);

                if (contrato is null)
                {
                    throw new BusinessException($"Error: No se encontro el contrato con ID: {par.IdContrato}.");
                }

                if (par.Aprobar)
                {
                    var historicoProrroga = new HistoricosProrroga()
                    {
                        CodContrato = contrato.Id,
                        VigenciaAnterior = contrato.ContVigenciaHasta,
                        VigenciaHasta = par.contReajusteVigencia,
                        CodUser = par.CodUser
                    };

                    _context.HistoricosProrroga.Add(historicoProrroga);
                }

                var pasoProrroga = new PasosContrato()
                {
                    PcnCodContrato = contrato.Id,
                    PcnCodEstadoContrato = contrato.ContCodEstado,
                    PcnCodTipoPaso = par.Aprobar ? _pasosContratosOptions.Value.ProrrogaAprobada : _pasosContratosOptions.Value.ProrrogaRechazada,
                    PcnConsecutivoFlujo = contrato.ContConsecutivoFlujo,
                    CodUser = par.CodUser
                };

                _context.PasosContrato.Add(pasoProrroga);

                contrato.ContAprobacionProrroga = par.Aprobar;
                contrato.CodUserUpdate = par.CodUserUpdate;
                contrato.ContVigenciaHasta = par.contReajusteVigencia;

                await Update(contrato);

                return new List<ResponseAction> { new ResponseAction() { estado = true, Id = par.IdContrato, mensaje = "Actualización Exitosa" } };
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> AsociarPolizasRenovadas(QueryContratoPolizasRenovadas par)
        {
            try
            {
                StringBuilder DocumentosPolizas = new StringBuilder();
                StringBuilder DocumentosAGuardar = new StringBuilder();

                foreach (DocReqPolizaDto item in par.DocumentosReqPoliza) // Docs Polizas
                {
                    DocumentosPolizas.Append("INSERT INTO #DocReqPolizaTemp (Id, DrpoCodContrato, DrpoCodTipoDocumento, DrpoTipoPoliza,  DrpoCobertura, DrpoVigencia, DrpoExpedida, DrpoAprobada, DrpoEstado, CodArchivo, KeyFile, DrpoFechaVencimiento, DrpoFechaEmision) ");
                    DocumentosPolizas.Append($"VALUES({item.Id ?? 0}, {item.DrpoCodContrato}, {item.DrpoCodTipoDocumento}, '{item.DrpoTipoPoliza}', '{item.DrpoCobertura}', '{item.DrpoVigencia}', {item.DrpoExpedida}, {item.DrpoAprobada}, {item.DrpoEstado}, {item.CodArchivo}, {item.KeyFile} ,'{item.DrpoFechaVencimiento?.ToString("yyyyMMdd") ?? "19000101"}', '{item.DrpoFechaEmision?.ToString("yyyyMMdd") ?? "19000101"}')");
                }

                foreach (DocReqUploadDto item in par.DocumentosAguardar)
                {
                    DocumentosAGuardar.Append("INSERT INTO #DocReqUploadTemp (Id, DrpuUrl, DrcoNameDocument, DrcoExtDocument, DrcoSizeDocument, DrcoUrlDocument, DrcoUrlRelDocument, DrcoOriginalNameDocument, KeyFile)");
                    DocumentosAGuardar.Append($"VALUES({item.Id ?? 0}, '{item.DrpuUrl}', '{item.DrcoNameDocument}', '{item.DrcoExtDocument}', {item.DrcoSizeDocument ?? 0}, '{item.DrcoUrlDocument}', '{item.DrcoUrlRelDocument}', '{item.DrcoOriginalNameDocument}', {item.KeyFile})");
                }

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Operacion","23"),
                    new SqlParameter("@contIdContrato",par.IdContrato),
                    new SqlParameter("@cadenaDocumentosPoliza",DocumentosPolizas.ToString()),
                    new SqlParameter("@cadenaArchivos",DocumentosAGuardar.ToString()),
                    new SqlParameter("@CodUserUpdate", par.CodUserUpdate ?? ""),
                };

                string sql = CallStoredProcedures.Contrato.AsociarPolizasRenovadas;

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> GuardarActaLiquidacion(ContratoActaLiquidacion par)
        {
            try
            {
                StringBuilder DocumentosAGuardar = new StringBuilder();

                if (par.DocumentoAGuardar != null)
                {
                    DocumentosAGuardar.Append("INSERT INTO #DocReqUploadTemp (Id, DrpuUrl, DrcoNameDocument, DrcoExtDocument, DrcoSizeDocument, DrcoUrlDocument, DrcoUrlRelDocument, DrcoOriginalNameDocument, KeyFile)");
                    DocumentosAGuardar.Append($"VALUES({par.DocumentoAGuardar.Id ?? 0}, '{par.DocumentoAGuardar.DrpuUrl}', '{par.DocumentoAGuardar.DrcoNameDocument}', '{par.DocumentoAGuardar.DrcoExtDocument}', {par.DocumentoAGuardar.DrcoSizeDocument ?? 0}, '{par.DocumentoAGuardar.DrcoUrlDocument}', '{par.DocumentoAGuardar.DrcoUrlRelDocument}', '{par.DocumentoAGuardar.DrcoOriginalNameDocument}', {par.DocumentoAGuardar.KeyFile})");
                }

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Operacion","24"),
                    new SqlParameter("@contIdContrato",par.IdContrato),
                    new SqlParameter("@CodArchivoActaLiquidacion", par.CodArchivoActaLiquidacion),
                    new SqlParameter("@KeyFileArchivoActaLiquidacion", par.KeyFileArchivoActaLiquidacion),
                    new SqlParameter("@cadenaArchivos",DocumentosAGuardar.ToString()),
                    new SqlParameter("@CodUserUpdate", par.CodUserUpdate ?? ""),
                };

                string sql = CallStoredProcedures.Contrato.GuardarActaLiquidacion;

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> SolicitarModificacionContrato(QuerySolicitudModificacion par)
        {
            try
            {
                StringBuilder aprobadoresString = new StringBuilder();

                foreach (AprobadoresContratoDto item in par.Aprobadores)
                {
                    aprobadoresString
                        .Append("INSERT INTO #AprobadoresContrato (apcIdAprobadoresContrato, apcCodContrato, apcCodTipoAprobadoresContrato, apcCodRequisitor, apcAprobacion, apcJustificacion) ")
                        .Append($"VALUES ({item.Id},{item.ApcCodContrato},{item.ApcCodTipoAprobadoresContrato},{item.ApcCodRequisitor}, {(item.ApcAprobacion ? 1 : 0)}, '{item.ApcJustificacion}' ) ");
                }

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Operacion","25"),
                    new SqlParameter("@contIdContrato",par.IdContrato),
                    new SqlParameter("@ContObjetoContrato", par.ContObjetoContrato),
                    new SqlParameter("@ContCarateristicasEspecificas", par.ContCarateristicasEspecificas),
                    new SqlParameter("@ContDuracionContrato", par.ContDuracionContrato),
                    new SqlParameter("@ContVigenciaDesde", par.ContVigenciaDesde?.ToString("yyyyMMdd")),
                    new SqlParameter("@ContVigenciaHasta", par.ContVigenciaHasta?.ToString("yyyyMMdd")),
                    new SqlParameter("@ContValorContrato", par.ContValorContrato),
                    new SqlParameter("@ContCodFormaPago", par.ContCodFormaPago),
                    new SqlParameter("@contObservacion", par.Observaciones),
                    new SqlParameter("@oscConsecutivoAlterno", par.ConsecutivoAlternoOtroSi),
                    new SqlParameter("@ContCodRepresentanteLegal", par.ContCodRepresentanteLegal),
                    new SqlParameter("@cadenaAprobadores",aprobadoresString.ToString()),
                    new SqlParameter("@CodUserUpdate", par.CodUserUpdate ?? ""),
                };

                string sql = CallStoredProcedures.SolicitudModificacion.SolicitarModificacion;

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<ParametrosHistoricos> GetParametrosHistoricos()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","9")
                };

                string sql = CallStoredProcedures.Contrato.ParametrosContrato;

                var response = await _context.ResponseJsonStrings.FromSqlRaw(sql, parameters: parameters).ToListAsync();

                var parametrosContrato = BaseResponseJson<RootJsonResponse<ParametrosContrato>>.ConvertJsonToEntity(response).JsonResponse.FirstOrDefault();

                var constratosList = await _context.Contratos
                                            .Include(c => c.ContratoHistoricos)
                                                .ThenInclude(ch => ch.AprobadoresContratoHistoricos)
                                            .Include(c => c.ContratoHistoricos)
                                                .ThenInclude(ch => ch.DocumentoContratoHistoricos)
                                                    .ThenInclude(a => a.Archivo)
                                            .Include(c => c.ContratoHistoricos)
                                                .ThenInclude(ch => ch.DocReqProveedorHistoricos)
                                                    .ThenInclude(a => a.Archivo)
                                            .Include(c => c.ContratoHistoricos)
                                                .ThenInclude(ch => ch.DocReqProveedorOtrosHistoricos)
                                                    .ThenInclude(a => a.Archivo)
                                            .Include(c => c.ContratoHistoricos)
                                                .ThenInclude(ch => ch.DocReqPolizaHistoricos)
                                                    .ThenInclude(a => a.Archivo)
                                            .Include(c => c.ContratoHistoricos)
                                                .ThenInclude(ch => ch.ArchivoActaInicio)
                                            .Include(c => c.ContratoHistoricos)
                                                .ThenInclude(ch => ch.ArchivoActaLiquidacion)
                                            .Include(c => c.ContratoHistoricos)
                                                .ThenInclude(ch => ch.ArchivoCompraNoPresupuestada)
                                            .Include(c => c.ContratoHistoricos)
                                                .ThenInclude(ch => ch.ArchivoNotificacionNoProrroga)
                                            .Include(c => c.ContratoHistoricos)
                                                .ThenInclude(ch => ch.ArchivoNotificacionTerminacion)
                                            .Where(c => c.ContCodEstado != (int)EEstadosContrato.Anulados)
                                            .ToListAsync();

                if (parametrosContrato != null)
                {
                    foreach (Contrato contrato in constratosList)
                    {
                        Contrato contratoToMap = parametrosContrato.Contratos.Find(e => e.Id == contrato.Id);

                        if (contratoToMap is not null)
                        {
                            contrato.EmpresaContrato = contratoToMap.EmpresaContrato;
                            contrato.ClaseContrato = contratoToMap.ClaseContrato;
                            contrato.ProveedorContrato = contratoToMap.ProveedorContrato;
                            contrato.EmpresaContrato = contratoToMap.EmpresaContrato;
                            contrato.AprobadoPorCoordinador = contratoToMap.AprobadoPorCoordinador;
                            contrato.AprobadoPorFinanciero = contratoToMap.AprobadoPorFinanciero;
                            contrato.AprobadoPorCompras = contratoToMap.AprobadoPorCompras;
                            contrato.AprobadoPorArea = contratoToMap.AprobadoPorArea;
                            contrato.UrlArchivoCompraNoPresupuestada = contratoToMap.UrlArchivoCompraNoPresupuestada;
                            contrato.NombreArchivoCompraNoPresupuestada = contratoToMap.NombreArchivoCompraNoPresupuestada;
                            contrato.UrlArchivoActaInicio = contratoToMap.UrlArchivoActaInicio;
                            contrato.ContFechaActaInicio = contratoToMap.ContFechaActaInicio;
                            contrato.NombreArchivoActaInicio = contratoToMap.NombreArchivoActaInicio;
                            contrato.UrlArchivoNotificacionNoProrroga = contratoToMap.UrlArchivoNotificacionNoProrroga;
                            contrato.NombreArchivoNotificacionNoProrroga = contratoToMap.NombreArchivoNotificacionNoProrroga;
                            contrato.UrlArchivoNotificacionTerminacion = contratoToMap.UrlArchivoNotificacionTerminacion;
                            contrato.NombreArchivoNotificacionTerminacion = contratoToMap.NombreArchivoNotificacionTerminacion;
                            contrato.UrlArchivoActaLiquidacion = contratoToMap.UrlArchivoActaLiquidacion;
                            contrato.NombreArchivoActaLiquidacion = contratoToMap.NombreArchivoActaLiquidacion;
                            contrato.ContUrlAbsolutePdf = contratoToMap.ContUrlAbsolutePdf;
                        }
                    }
                }

                return new ParametrosHistoricos()
                {
                    Contratos = constratosList,
                    ClasesContrato = parametrosContrato?.ClasesContrato,
                    FormasPago = parametrosContrato?.FormasPago,
                    Si_No = parametrosContrato?.Si_No,
                    UnidadesNegocio = parametrosContrato?.UnidadesNegocio,
                    DocsProveedor = parametrosContrato?.DocsProveedor,
                    UsuariosSelect = parametrosContrato?.UsuariosSelect,
                    TiposContratos = parametrosContrato?.TiposContratos,
                };
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> GuardarComentario(ContQuestionAnswer entity)
        {
            try
            {
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Operacion",1),
                    new SqlParameter("@IdQuestionAnswer",entity.IdQuestionAnswer ?? 0),
                    new SqlParameter("@IdContrato", entity.IdContrato),
                    new SqlParameter("@IdUsuario", entity.IdUsuario),
                    new SqlParameter("@EsGestor",entity.EsGestor),
                    new SqlParameter("@Comentario", entity.Comentario),
                    new SqlParameter("@ContieneAdjunto", entity.ContieneAdjunto),
                    new SqlParameter("@EsPrivado", entity.EsPrivado),
                    new SqlParameter("@Estado", entity.Estado),
                    new SqlParameter("@FilePath", entity.FilePath),
                    new SqlParameter("@FileRelativo", entity.FileRelativo),
                    new SqlParameter("@FileSize", entity.FileSize),
                    new SqlParameter("@FileExt", entity.FileExt),
                    new SqlParameter("@CodUser", entity.CodUser),
                    new SqlParameter("@Info", entity.Info ?? ""),
                };

                string sql = CallStoredProcedures.Contrato.Comentarios.GuardarComentario;

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ContratoComentarios>> ConsultarComentariosPorContratoId(int IdContrato)
        {
            try
            {
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Operacion", 2),
                    new SqlParameter("@IdContrato",IdContrato)
                };

                string sql = CallStoredProcedures.Contrato.Comentarios.BuscarComentariosContrato;

                var response = await _context.ResponseJsonStrings.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return BaseResponseJson<RootJsonResponse<ContratoComentarios>>.ConvertJsonToEntity(response)?.JsonResponse;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> CambiarEstadoContrato(CommandUpdateEstadoContrato command)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@contIdContrato", command.IdContrato),
                    new SqlParameter("@contCodEstado", command.ContEstado),
                    new SqlParameter("@CodUserUpdate", command.CodUserUpdate),
                };

                string sql = CallStoredProcedures.Contrato.CambiarEstadoContratoAnterior;

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ContratoListado>> ListadoContratosReporte(QueryListadoContratosReporte query)
        {
            try
            {
                var parameters = new
                {
                    Operacion = 1,
                    query.NumeroContrato,
                    query.CedulaProveedor,
                    query.TipoContrato,
                    query.VigenciaDesde,
                    query.VigenciaHasta,
                    query.Estado,
                };

                var result = (await _dapperSource.QueryAsync<ContratoListado>("[cont].[SpListadoContratos]", parameters, commandType: CommandType.StoredProcedure)).ToList();

                return result;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ContratoListado>> ListadoContratosHistoricosReporte(QueryListadoContratosReporte query)
        {
            try
            {
                var parameters = new
                {
                    Operacion = 2,
                    query.NumeroContrato,
                    query.CedulaProveedor,
                    query.TipoContrato,
                    query.VigenciaDesde,
                    query.VigenciaHasta,
                    query.Estado,
                };

                var result = (await _dapperSource.QueryAsync<ContratoListado>("[cont].[SpListadoContratos]", parameters, commandType: CommandType.StoredProcedure)).ToList();

                return result;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> AprobacionAdministrador(CommandUpdateEstadoContrato command)
        {
            try
            {
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Operacion","26"),
                    new SqlParameter("@contIdContrato",command.IdContrato),
                };

                string sql = CallStoredProcedures.Contrato.AprobacionAdministrador;

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