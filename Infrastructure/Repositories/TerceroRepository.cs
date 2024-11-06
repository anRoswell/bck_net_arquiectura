using Core.CustomEntities;
using Core.DTOs;
using Core.Entities;
using Core.Enumerations;
using Core.Exceptions;
using Core.HubConfig;
using Core.Interfaces;
using Core.ModelResponse;
using Core.Options;
using Core.QueryFilters;
using Core.Tools;
using Dapper;
using Infrastructure.Data;
using Infrastructure.Utils;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TerceroRepository : BaseRepository<Tercero>, ITerceroRepository
    {
        private readonly IOptions<PerfilesOptions> _perfilesOptions;
        private readonly IDbConnection _dapperSource;

        public TerceroRepository(DbModelContext context, IDbConnection dapperSource, IOptions<PerfilesOptions> perfilesOptions) : base(context)
        {
            _perfilesOptions = perfilesOptions;
            _dapperSource = dapperSource;
        }

        public async Task<List<TerceroDto>> GetTerceros(int idPathFileServer)
        {
            try
            {
                var result = (await _dapperSource.QueryAsync<TerceroDto>("[prv].[SpTerceros]", new
                {
                    Operacion = "1",
                    idPathFileServer
                }, commandType: CommandType.StoredProcedure)).ToList();

                return result;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<TerceroDto>> GetTerceroPorNit(string nit, int idPathFileServer)
        {
            try
            {
                var result = (await _dapperSource.QueryAsync<TerceroDto>("[prv].[SpTerceros]", new
                {
                    Operacion = "2",
                    prvNit = nit,
                    idPathFileServer
                }, commandType: CommandType.StoredProcedure)).ToList();

                return result;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<TerceroDto>> GetTerceroPorID(int id, int idPathFileServer)
        {
            try
            {
                var result = (await _dapperSource.QueryAsync<TerceroDto>("[prv].[SpTerceros]", new
                {
                    Operacion = "6",
                    prvIdProveedores = id,
                    idPathFileServer
                }, commandType: CommandType.StoredProcedure)).ToList();
               
                return result;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> SearchByCodValidation(QuerySearchByCodValidation data)
        {
            try
            {
                var result = (await _dapperSource.QueryAsync<ResponseAction>("[prv].[SpPrvDocumento]", new
                {
                    Operacion = "4",
                    prvValidationNumber = data.CodValidation,
                    prvNit = data.Nit
                }, commandType: CommandType.StoredProcedure)).ToList();

                return result;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<TerceroDto>> GetFiltroTercero(string idLocalidad, int idCategoriaServicio, int idEstado, string razonSocial, string prvNit, int idPathFileServer)
        {
            try
            {
                var result = (await _dapperSource.QueryAsync<TerceroDto>("[prv].[SpTerceros]", new
                {
                    Operacion = "9",
                    idLocalidad,
                    idCategoriaServicio,
                    idEstado,
                    razonSocial,
                    prvNit,
                    idPathFileServer
                }, commandType: CommandType.StoredProcedure)).ToList();

                return result;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<Requerimientos>> GetRequerimientosTercero(int idProveedor)
        {
            try
            {
                SqlParameter[] parameters = new[] 
                {
                    new SqlParameter("@Operacion","11"),
                    new SqlParameter("@prvIdProveedores", idProveedor)
                };

                string sql = $"[prv].[SpTerceros] @Operacion = @Operacion, @prvIdProveedores = @prvIdProveedores";

                var response = await _context.Requerimientos.FromSqlRaw(sql, parameters: parameters).AsNoTracking().ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseActionUrl>> PostCrear(TerceroCrearDto proveedor, List<PrvDocumento> documentos, string Generatedpassword, SignalParams<ProveedorHub, DataProveedores> signalParam)
        {
            try
            {
                string token = Funciones.GetSHA256(Guid.NewGuid().ToString());

                StringBuilder InsertDocs = new();
                StringBuilder InsertSocios = new();
                StringBuilder InsertRef = new();
                StringBuilder InsertEmp = new();
                StringBuilder InsertProdServ = new();

                // Construir Inserts Documentos
                foreach (PrvDocumento item in documentos)
                {
                    string queryInsert = "";
                    string queryInsertValues = "";

                    if (item.PrvdExpedicion is null)
                    {
                        queryInsert = "INSERT INTO #documentos (prvdCodDocumento, prvdNameDocument, prvdExtDocument, prvdSizeDocument, prvdUrlDocument, prvdUrlRelDocument, prvdOriginalNameDocument) ";
                        queryInsertValues = $"VALUES({item.PrvdCodDocumento},'{item.PrvdNameDocument}','{item.PrvdExtDocument}',{item.PrvdSizeDocument},'{item.PrvdUrlDocument}'," +
                        $"'{item.PrvdUrlRelDocument}','{item.PrvdOriginalNameDocument}') ";
                    }
                    else
                    {
                        queryInsert = "INSERT INTO #documentos (prvdCodDocumento, prvdNameDocument, prvdExtDocument, prvdSizeDocument, prvdUrlDocument, prvdUrlRelDocument, prvdOriginalNameDocument, prvdExpedicion) ";
                        queryInsertValues = $"VALUES({item.PrvdCodDocumento},'{item.PrvdNameDocument}','{item.PrvdExtDocument}',{item.PrvdSizeDocument},'{item.PrvdUrlDocument}','{item.PrvdUrlRelDocument}'," +
                        $"'{item.PrvdOriginalNameDocument}', '{item.PrvdExpedicion?.ToString("yyyyMMdd")}') ";
                    }

                    InsertDocs.Append(queryInsert);
                    InsertDocs.Append(queryInsertValues);
                }
                signalParam.DataModel.Data.Progress = 50;
                await signalParam.HubContext.Clients.All.SendAsync(HubConectionsMethods.CreateProveedor, signalParam.DataModel);

                // Construir Inserts Socios
                foreach (PrvSocio item in proveedor.PrvSocios)
                {
                    InsertSocios.Append("INSERT INTO #socios (socNombre, socCodCiudad, socIdentificacion, socDigVerificacion, socDireccion) ");
                    InsertSocios.Append($"VALUES('{item.SocNombre}','{item.SocCodCiudad}','{item.SocIdentificacion}','{item.SocDigVerificacion}','{item.SocDireccion}') ");
                }
                signalParam.DataModel.Data.Progress = 60;
                await signalParam.HubContext.Clients.All.SendAsync(HubConectionsMethods.CreateProveedor, signalParam.DataModel);

                // Construir Inserts Referencias
                foreach (PrvReferencia item in proveedor.PrvReferencias)
                {
                    InsertRef.Append("INSERT INTO #referencias (refEmpresa, refTelefono, refContacto) ");
                    InsertRef.Append($"VALUES('{item.RefEmpresa}','{item.RefTelefono}','{item.RefContacto}') ");
                }
                signalParam.DataModel.Data.Progress = 70;
                await signalParam.HubContext.Clients.All.SendAsync(HubConectionsMethods.CreateProveedor, signalParam.DataModel);

                // Construir Inserts Empresas
                foreach (int item in proveedor.ListEmpresas)
                {
                    InsertEmp.Append("INSERT INTO #empresas (CodEmpresas) ");
                    InsertEmp.Append($"VALUES({item}) ");
                }
                signalParam.DataModel.Data.Progress = 80;
                await signalParam.HubContext.Clients.All.SendAsync(HubConectionsMethods.CreateProveedor, signalParam.DataModel);

                // Construir Inserts ProductosServicios
                foreach (int item in proveedor.ListProdServicios)
                {
                    InsertProdServ.Append("INSERT INTO #Produtos_Servicios (CodPrvProdServ) ");
                    InsertProdServ.Append($"VALUES({item}) ");
                }

                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion", "3"),
                    new SqlParameter("@prvNit", proveedor.PrvNit),
                    new SqlParameter("@prvDigitoVerificacion", proveedor.PrvDigitoVerificacion??""),
                    new SqlParameter("@prvNombreProveedor", proveedor.PrvNombreProveedor),
                    new SqlParameter("@prvDireccion", proveedor.PrvDireccion),
                    new SqlParameter("@prvCodCiudad", proveedor.PrvCodCiudad),
                    new SqlParameter("@prvTelefono", proveedor.PrvTelefono),
                    new SqlParameter("@prvContacto", proveedor.PrvContacto),
                    new SqlParameter("@prvMail", proveedor.PrvMail),
                    new SqlParameter("@prvMailAlterno", proveedor.PrvMailAlterno),
                    new SqlParameter("@prvRteLegalNombre", proveedor.PrvRteLegalNombre??""),
                    new SqlParameter("@prvRteLegalIdentificacion", proveedor.PrvRteLegalIdentificacion??""),
                    new SqlParameter("@prvRteLegalDigVerificacion", proveedor.PrvRteLegalDigVerificacion??""),
                    new SqlParameter("@prvRteLegal_CodCiudad", proveedor.PrvRteLegalCodCiudad??""),
                    new SqlParameter("@prvRteLegalTelefonoMovil", proveedor.PrvRteLegalTelefonoMovil??""),
                    new SqlParameter("@prvRteLegalEmail", proveedor.PrvRteLegalEmail??""),
                    new SqlParameter("@prvRteLegalSuplenteNombre", proveedor.PrvRteLegalSuplenteNombre??""),
                    new SqlParameter("@prvRteLegalSuplenteIdentificacion", proveedor.PrvRteLegalSuplenteIdentificacion??""),
                    new SqlParameter("@prvRteLegalSuplenteDigVerificacion", proveedor.PrvRteLegalSuplenteDigVerificacion??""),
                    new SqlParameter("@prvRteLegalSuplente_CodCiudad", proveedor.PrvRteLegalSuplenteCodCiudad??""),
                    new SqlParameter("@prvRteLegalSuplenteTelefonoMovil", proveedor.PrvRteLegalSuplenteTelefonoMovil??""),
                    new SqlParameter("@prvRteLegalSuplenteEmail", proveedor.PrvRteLegalSuplenteEmail??""),
                    new SqlParameter("@prvRevFiscalNombre", proveedor.PrvRevFiscalNombre??""),
                    new SqlParameter("@prvRevFiscalIdentificacion", proveedor.PrvRevFiscalIdentificacion??""),
                    new SqlParameter("@prvRevFiscalDigVerificacion", proveedor.PrvRevFiscalDigVerificacion??""),
                    new SqlParameter("@prvRevFiscal_CodCiudad", proveedor.PrvRevFiscalCodCiudad??""),
                    new SqlParameter("@prvRevFiscalTelefonoMovil", proveedor.PrvRevFiscalTelefonoMovil??""),
                    new SqlParameter("@prvRevFiscalEmail", proveedor.PrvRevFiscalEmail??""),
                    new SqlParameter("@prvCodBanco", proveedor.PrvCodBanco),
                    new SqlParameter("@prvDtllesBanNroCuenta", proveedor.PrvDtllesBanNroCuenta),
                    new SqlParameter("@prvCodTipoCuenta", proveedor.PrvCodTipoCuenta),
                    new SqlParameter("@prvProveeedor", proveedor.PrvProveeedor),
                    new SqlParameter("@prvCodTipoProveeedor", proveedor.PrvCodTipoProveeedor),
                    new SqlParameter("@prvTipoProveedorCual", proveedor.PrvTipoProveedorCual),
                    new SqlParameter("@prvCpaCodCondicionesPago", proveedor.PrvCpaCodCondicionesPago),
                    new SqlParameter("@prvCpaCual", proveedor.PrvCpaCual??""),
                    new SqlParameter("@prvCpaContadoCual", proveedor.PrvCpaContadoCual),
                    new SqlParameter("@prvExperienciaSector", proveedor.PrvExperienciaSector),
                    new SqlParameter("@prvPoliticaTratamientoDatosPersonales", proveedor.PrvPoliticaTratamientoDatosPersonales),
                    new SqlParameter("@prvDeclaramientoInhabilidadesInteres", proveedor.PrvDeclaramientoInhabilidadesInteres),
                    new SqlParameter("@InsertDocs", InsertDocs.ToString()),
                    new SqlParameter("@InsertSocios", InsertSocios.ToString()),
                    new SqlParameter("@InsertReferencias", InsertRef.ToString()),
                    new SqlParameter("@InsertEmpresas", InsertEmp.ToString()),
                    new SqlParameter("@InsertProdServ", InsertProdServ.ToString()),
                    new SqlParameter("@Password", Generatedpassword),
                    new SqlParameter("@Token", token),
                    new SqlParameter("@codOficialCumplimiento", _perfilesOptions.Value.Codigo_Oficial_Cumplimiento),
                };

                string sql = $"[prv].[SpTerceros] @Operacion = @Operacion, @prvNit = @prvNit, @prvDigitoVerificacion = @prvDigitoVerificacion, " +
                    $"@prvNombreProveedor = @prvNombreProveedor, @prvDireccion = @prvDireccion, @prvCodCiudad = @prvCodCiudad, @prvTelefono = @prvTelefono, " +
                    $"@prvContacto = @prvContacto, @prvMail = @prvMail, @prvMailAlterno = @prvMailAlterno, " +
                    $"@prvRteLegalNombre = @prvRteLegalNombre, @prvRteLegalIdentificacion = @prvRteLegalIdentificacion, " +
                    $"@prvRteLegalDigVerificacion = @prvRteLegalDigVerificacion, @prvRteLegal_CodCiudad = @prvRteLegal_CodCiudad, @prvRteLegalTelefonoMovil = @prvRteLegalTelefonoMovil, " +
                    $"@prvRteLegalEmail = @prvRteLegalEmail, " +
                    $"@prvRteLegalSuplenteNombre = @prvRteLegalSuplenteNombre, @prvRteLegalSuplenteIdentificacion = @prvRteLegalSuplenteIdentificacion, " +
                    $"@prvRteLegalSuplenteDigVerificacion = @prvRteLegalSuplenteDigVerificacion, @prvRteLegalSuplente_CodCiudad = @prvRteLegalSuplente_CodCiudad, @prvRteLegalSuplenteTelefonoMovil = @prvRteLegalSuplenteTelefonoMovil, " +
                    $"@prvRteLegalSuplenteEmail = @prvRteLegalSuplenteEmail, " +
                    $"@prvRevFiscalNombre = @prvRevFiscalNombre, @prvRevFiscalIdentificacion = @prvRevFiscalIdentificacion, " +
                    $"@prvRevFiscalDigVerificacion = @prvRevFiscalDigVerificacion, @prvRevFiscal_CodCiudad = @prvRevFiscal_CodCiudad, @prvRevFiscalTelefonoMovil = @prvRevFiscalTelefonoMovil, " +
                    $"@prvRevFiscalEmail = @prvRevFiscalEmail, @prvCodBanco = @prvCodBanco, @prvDtllesBanNroCuenta = @prvDtllesBanNroCuenta, @prvCodTipoCuenta = @prvCodTipoCuenta, " +
                    $"@prvProveeedor = @prvProveeedor, @prvCodTipoProveeedor = @prvCodTipoProveeedor, @prvTipoProveedorCual = @prvTipoProveedorCual, @prvCpaCodCondicionesPago = @prvCpaCodCondicionesPago, " +
                    $"@prvCpaCual = @prvCpaCual, @prvCpaContadoCual = @prvCpaContadoCual, @prvExperienciaSector = @prvExperienciaSector, " +
                    $"@prvPoliticaTratamientoDatosPersonales = @prvPoliticaTratamientoDatosPersonales, @prvDeclaramientoInhabilidadesInteres = @prvDeclaramientoInhabilidadesInteres, " +
                    $"@InsertDocs = @InsertDocs, @InsertSocios = @InsertSocios, @InsertReferencias = @InsertReferencias, @InsertEmpresas = @InsertEmpresas, @InsertProdServ = @InsertProdServ, " +
                    $"@Password = @Password, @Token = @Token, @codOficialCumplimiento = @codOficialCumplimiento";

                var response = await _context.ResponseActionUrls.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                signalParam.DataModel.Data.Progress = 90;
                await signalParam.HubContext.Clients.All.SendAsync(HubConectionsMethods.CreateProveedor, signalParam.DataModel);
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseActionUrl>> PutActualizar(TerceroActualizarDto proveedor, List<PrvDocumento> documentos, List<PrvDocumento> listDocumentsOthers, SignalParams<ProveedorHub, DataProveedores> signalParam, int idPathFileServer)
        {
            try
            {
                StringBuilder InsertDocs = new();
                StringBuilder InsertDocsOthers = new();
                StringBuilder InsertSocios = new();
                StringBuilder InsertRef = new();
                StringBuilder InsertEmp = new();
                StringBuilder InsertProdServ = new();

                // Construir Inserts Documentos
                foreach (PrvDocumento item in documentos)
                {
                    string queryInsert = "";
                    string queryInsertValues = "";

                    if (item.PrvdExpedicion is null)
                    {
                        queryInsert = "INSERT INTO #documentos_update (prvdCodDocumento, prvdCodProveedor, prvdNameDocument, prvdExtDocument, prvdSizeDocument, prvdUrlDocument, prvdUrlRelDocument, prvdOriginalNameDocument) ";
                        queryInsertValues = $"VALUES({item.PrvdCodDocumento}, {proveedor.Id},'{item.PrvdNameDocument}','{item.PrvdExtDocument}',{item.PrvdSizeDocument},'{item.PrvdUrlDocument}'," +
                        $"'{item.PrvdUrlRelDocument}','{item.PrvdOriginalNameDocument}') ";
                    }
                    else
                    {
                        queryInsert = "INSERT INTO #documentos_update (prvdCodDocumento, prvdCodProveedor, prvdNameDocument, prvdExtDocument, prvdSizeDocument, prvdUrlDocument, prvdUrlRelDocument, prvdOriginalNameDocument, prvdExpedicion) ";
                        queryInsertValues = $"VALUES({item.PrvdCodDocumento}, {proveedor.Id},'{item.PrvdNameDocument}','{item.PrvdExtDocument}',{item.PrvdSizeDocument},'{item.PrvdUrlDocument}','{item.PrvdUrlRelDocument}'," +
                        $"'{item.PrvdOriginalNameDocument}', '{item.PrvdExpedicion?.ToString("yyyyMMdd")}') ";
                    }

                    InsertDocs.Append(queryInsert);
                    InsertDocs.Append(queryInsertValues);
                }

                // Construir Inserts Documentos_Others
                foreach (PrvDocumento item in listDocumentsOthers)
                {
                    string queryInsert = "";
                    string queryInsertValues = "";

                    if (item.PrvdExpedicion is null)
                    {
                        queryInsert = "INSERT INTO #documentos_others (prvdCodDocumento, prvdCodProveedor, prvdNameDocument, prvdExtDocument, prvdSizeDocument, prvdUrlDocument, prvdUrlRelDocument, prvdOriginalNameDocument) ";
                        queryInsertValues = $"VALUES({item.PrvdCodDocumento}, {proveedor.Id},'{item.PrvdNameDocument}','{item.PrvdExtDocument}',{item.PrvdSizeDocument},'{item.PrvdUrlDocument}'," +
                        $"'{item.PrvdUrlRelDocument}','{item.PrvdOriginalNameDocument}') ";
                    }
                    else
                    {
                        queryInsert = "INSERT INTO #documentos_others (prvdCodDocumento, prvdCodProveedor, prvdNameDocument, prvdExtDocument, prvdSizeDocument, prvdUrlDocument, prvdUrlRelDocument, prvdOriginalNameDocument, prvdExpedicion) ";
                        queryInsertValues = $"VALUES({item.PrvdCodDocumento}, {proveedor.Id},'{item.PrvdNameDocument}','{item.PrvdExtDocument}',{item.PrvdSizeDocument},'{item.PrvdUrlDocument}','{item.PrvdUrlRelDocument}'," +
                        $"'{item.PrvdOriginalNameDocument}', '{item.PrvdExpedicion?.ToString("yyyyMMdd")}') ";
                    }

                    InsertDocsOthers.Append(queryInsert);
                    InsertDocsOthers.Append(queryInsertValues);
                }

                signalParam.DataModel.Data.Progress = 50;
                await signalParam.HubContext.Clients.All.SendAsync(HubConectionsMethods.UpdateProveedor, signalParam.DataModel);

                // Construir Inserts Socios
                foreach (PrvSocio item in proveedor.PrvSocios)
                {
                    InsertSocios.Append("INSERT INTO #socios_update (socNombre, socCodCiudad, socIdentificacion, socDigVerificacion, socDireccion) ");
                    InsertSocios.Append($"VALUES('{item.SocNombre}','{item.SocCodCiudad}','{item.SocIdentificacion}','{item.SocDigVerificacion}','{item.SocDireccion}') ");
                }

                signalParam.DataModel.Data.Progress = 60;
                await signalParam.HubContext.Clients.All.SendAsync(HubConectionsMethods.UpdateProveedor, signalParam.DataModel);

                // Construir Inserts Referencias
                foreach (PrvReferencia item in proveedor.PrvReferencias)
                {
                    InsertRef.Append("INSERT INTO #referencias_update (refEmpresa, refTelefono, refContacto) ");
                    InsertRef.Append($"VALUES('{item.RefEmpresa}','{item.RefTelefono}','{item.RefContacto}') ");
                }
                signalParam.DataModel.Data.Progress = 70;
                await signalParam.HubContext.Clients.All.SendAsync(HubConectionsMethods.UpdateProveedor, signalParam.DataModel);

                // Construir Inserts Empresas
                foreach (int item in proveedor.ListEmpresas)
                {
                    InsertEmp.Append("INSERT INTO #empresas_update (CodEmpresas) ");
                    InsertEmp.Append($"VALUES({item}) ");
                }
                signalParam.DataModel.Data.Progress = 80;
                await signalParam.HubContext.Clients.All.SendAsync(HubConectionsMethods.UpdateProveedor, signalParam.DataModel);

                // Construir Inserts ProductosServicios
                foreach (int item in proveedor.ListProdServicios)
                {
                    InsertProdServ.Append("INSERT INTO #Produtos_Servicios_update (CodPrvProdServ) ");
                    InsertProdServ.Append($"VALUES({item}) ");
                }

                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion", "4"),
                    new SqlParameter("@prvIdProveedores", proveedor.Id),
                    new SqlParameter("@prvNit", proveedor.PrvNit),
                    new SqlParameter("@prvDigitoVerificacion", proveedor.PrvDigitoVerificacion??""),
                    new SqlParameter("@prvNombreProveedor", proveedor.PrvNombreProveedor),
                    new SqlParameter("@prvDireccion", proveedor.PrvDireccion),
                    new SqlParameter("@prvCodCiudad", proveedor.PrvCodCiudad),
                    new SqlParameter("@prvTelefono", proveedor.PrvTelefono),
                    new SqlParameter("@prvContacto", proveedor.PrvContacto),
                    new SqlParameter("@prvMail", proveedor.PrvMail),
                    new SqlParameter("@prvMailAlterno", proveedor.PrvMailAlterno),
                    new SqlParameter("@prvRteLegalNombre", proveedor.PrvRteLegalNombre??""),
                    new SqlParameter("@prvRteLegalIdentificacion", proveedor.PrvRteLegalIdentificacion??""),
                    new SqlParameter("@prvRteLegalDigVerificacion", proveedor.PrvRteLegalDigVerificacion??""),
                    new SqlParameter("@prvRteLegal_CodCiudad", proveedor.PrvRteLegalCodCiudad??""),
                    new SqlParameter("@prvRteLegalTelefonoMovil", proveedor.PrvRteLegalTelefonoMovil??""),
                    new SqlParameter("@prvRteLegalEmail", proveedor.PrvRteLegalEmail??""),
                    new SqlParameter("@prvRteLegalSuplenteNombre", proveedor.PrvRteLegalSuplenteNombre??""),
                    new SqlParameter("@prvRteLegalSuplenteIdentificacion", proveedor.PrvRteLegalSuplenteIdentificacion??""),
                    new SqlParameter("@prvRteLegalSuplenteDigVerificacion", proveedor.PrvRteLegalSuplenteDigVerificacion??""),
                    new SqlParameter("@prvRteLegalSuplente_CodCiudad", proveedor.PrvRteLegalSuplenteCodCiudad??""),
                    new SqlParameter("@prvRteLegalSuplenteTelefonoMovil", proveedor.PrvRteLegalSuplenteTelefonoMovil??""),
                    new SqlParameter("@prvRteLegalSuplenteEmail", proveedor.PrvRteLegalSuplenteEmail??""),
                    new SqlParameter("@prvRevFiscalNombre", proveedor.PrvRevFiscalNombre??""),
                    new SqlParameter("@prvRevFiscalIdentificacion", proveedor.PrvRevFiscalIdentificacion??""),
                    new SqlParameter("@prvRevFiscalDigVerificacion", proveedor.PrvRevFiscalDigVerificacion??""),
                    new SqlParameter("@prvRevFiscal_CodCiudad", proveedor.PrvRevFiscalCodCiudad??""),
                    new SqlParameter("@prvRevFiscalTelefonoMovil", proveedor.PrvRevFiscalTelefonoMovil??""),
                    new SqlParameter("@prvRevFiscalEmail", proveedor.PrvRevFiscalEmail??""),
                    new SqlParameter("@prvCodBanco", proveedor.PrvCodBanco),
                    new SqlParameter("@prvDtllesBanNroCuenta", proveedor.PrvDtllesBanNroCuenta),
                    new SqlParameter("@prvCodTipoCuenta", proveedor.PrvCodTipoCuenta),
                    new SqlParameter("@prvProveeedor", proveedor.PrvProveeedor),
                    new SqlParameter("@prvCodTipoProveeedor", proveedor.PrvCodTipoProveeedor),
                    new SqlParameter("@prvTipoProveedorCual", proveedor.PrvTipoProveedorCual),
                    new SqlParameter("@prvCpaCodCondicionesPago", proveedor.PrvCpaCodCondicionesPago),
                    new SqlParameter("@prvCpaCual", proveedor.PrvCpaCual??""),
                    new SqlParameter("@prvCpaContadoCual", proveedor.PrvCpaContadoCual),
                    new SqlParameter("@prvExperienciaSector", proveedor.PrvExperienciaSector),
                    new SqlParameter("@prvPoliticaTratamientoDatosPersonales", proveedor.PrvPoliticaTratamientoDatosPersonales),
                    new SqlParameter("@prvDeclaramientoInhabilidadesInteres", proveedor.PrvDeclaramientoInhabilidadesInteres),
                    new SqlParameter("@InsertDocs", InsertDocs.ToString()),
                    new SqlParameter("@InsertDocsOthers", InsertDocsOthers.ToString()),
                    new SqlParameter("@InsertSocios", InsertSocios.ToString()),
                    new SqlParameter("@InsertReferencias", InsertRef.ToString()),
                    new SqlParameter("@InsertEmpresas", InsertEmp.ToString()),
                    new SqlParameter("@InsertProdServ", InsertProdServ.ToString()),
                    new SqlParameter("@idPathFileServer", idPathFileServer),
                    new SqlParameter("@codOficialCumplimiento", _perfilesOptions.Value.Codigo_Oficial_Cumplimiento),
                };

                string sql = $"[prv].[SpTerceros] @Operacion = @Operacion, @prvIdProveedores = @prvIdProveedores, @prvNit = @prvNit, @prvDigitoVerificacion = @prvDigitoVerificacion, " +
                    $"@prvNombreProveedor = @prvNombreProveedor, @prvDireccion = @prvDireccion, @prvCodCiudad = @prvCodCiudad, @prvTelefono = @prvTelefono, " +
                    $"@prvContacto = @prvContacto, @prvMail = @prvMail, @prvMailAlterno = @prvMailAlterno, " +
                    $"@prvRteLegalNombre = @prvRteLegalNombre, @prvRteLegalIdentificacion = @prvRteLegalIdentificacion, " +
                    $"@prvRteLegalDigVerificacion = @prvRteLegalDigVerificacion, @prvRteLegal_CodCiudad = @prvRteLegal_CodCiudad, @prvRteLegalTelefonoMovil = @prvRteLegalTelefonoMovil, " +
                    $"@prvRteLegalEmail = @prvRteLegalEmail, " +
                    $"@prvRteLegalSuplenteNombre = @prvRteLegalSuplenteNombre, @prvRteLegalSuplenteIdentificacion = @prvRteLegalSuplenteIdentificacion, " +
                    $"@prvRteLegalSuplenteDigVerificacion = @prvRteLegalSuplenteDigVerificacion, @prvRteLegalSuplente_CodCiudad = @prvRteLegalSuplente_CodCiudad, @prvRteLegalSuplenteTelefonoMovil = @prvRteLegalSuplenteTelefonoMovil, " +
                    $"@prvRteLegalSuplenteEmail = @prvRteLegalSuplenteEmail, " +
                    $"@prvRevFiscalNombre = @prvRevFiscalNombre, @prvRevFiscalIdentificacion = @prvRevFiscalIdentificacion, " +
                    $"@prvRevFiscalDigVerificacion = @prvRevFiscalDigVerificacion, @prvRevFiscal_CodCiudad = @prvRevFiscal_CodCiudad, @prvRevFiscalTelefonoMovil = @prvRevFiscalTelefonoMovil, " +
                    $"@prvRevFiscalEmail = @prvRevFiscalEmail, @prvCodBanco = @prvCodBanco, @prvDtllesBanNroCuenta = @prvDtllesBanNroCuenta, @prvCodTipoCuenta = @prvCodTipoCuenta, " +
                    $"@prvProveeedor = @prvProveeedor, @prvCodTipoProveeedor = @prvCodTipoProveeedor, @prvTipoProveedorCual = @prvTipoProveedorCual, @prvCpaCodCondicionesPago = @prvCpaCodCondicionesPago, " +
                    $"@prvCpaCual = @prvCpaCual, @prvCpaContadoCual = @prvCpaContadoCual, @prvExperienciaSector = @prvExperienciaSector, @prvPoliticaTratamientoDatosPersonales = @prvPoliticaTratamientoDatosPersonales, " +
                    $"@prvDeclaramientoInhabilidadesInteres = @prvDeclaramientoInhabilidadesInteres, @InsertDocs = @InsertDocs, @InsertDocsOthers = @InsertDocsOthers, @InsertSocios = @InsertSocios, @InsertReferencias = @InsertReferencias, " +
                    $"@InsertEmpresas = @InsertEmpresas, @InsertProdServ = @InsertProdServ, @idPathFileServer = @idPathFileServer, @codOficialCumplimiento = @codOficialCumplimiento";

                var response = await _context.ResponseActionUrls.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                signalParam.DataModel.Data.Progress = 90;
                await signalParam.HubContext.Clients.All.SendAsync(HubConectionsMethods.UpdateProveedor, signalParam.DataModel);
                return response;
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
                    new SqlParameter("@Operacion","7"),
                    new SqlParameter("@prvIdProveedores", id),
                      new SqlParameter("@prvUrlPdf", url)
                };

                string sql = $"[prv].[SpTerceros] @Operacion = @Operacion, @prvIdProveedores = @prvIdProveedores, @prvUrlPdf=@prvUrlPdf";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PutUpdateEstadoDocument(QueryUpdateEstadoDocument data)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion", "2"),
                    new SqlParameter("@IdDocumento", data.IdDocument),
                     new SqlParameter("@CodUserUpdate", data.CodUserUpdate ?? "")
                };

                string sql = $"[prv].[SpPrvDocumento] @Operacion = @Operacion, @IdDocumento = @IdDocumento, @CodUserUpdate = @CodUserUpdate";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();

                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PutTerceroAprobar(QueryUpdateEstadoPrv updEstPrv)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion", "8"),
                    new SqlParameter("@prvNit", updEstPrv.PrvNit),
                    new SqlParameter("@prvCodEstado", updEstPrv.Estado),
                    new SqlParameter("@ObservEstado", updEstPrv.Observaciones ?? ""),
                    new SqlParameter("@CodUserUpdate", updEstPrv.CodUserUpdate ?? ""),
                    new SqlParameter("@codOficialCumplimiento", _perfilesOptions.Value.Codigo_Oficial_Cumplimiento),
                };

                string sql = $"[prv].[SpTerceros] @Operacion = @Operacion, @prvNit = @prvNit, @prvCodEstado = @prvCodEstado, @ObservEstado = @ObservEstado, @CodUserUpdate = @CodUserUpdate, @codOficialCumplimiento = @codOficialCumplimiento";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();

                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PutUpdateTrazaInspect(QueryUpdateTrazaInspect data)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion", "3"),
                    new SqlParameter("@IdTrazaGestionProveedores", data.IdTrazaGestionProveedores),
                    new SqlParameter("@CodUserUpdate", data.CodUserUpdate ?? "")
                };

                string sql = $"[prv].[SpPrvDocumento] @Operacion = @Operacion, @IdTrazaGestionProveedores = @IdTrazaGestionProveedores, @CodUserUpdate = @CodUserUpdate";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();

                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ProveedorInspektor>> ValidateBlackList(string numIdenti, string nombre)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion", "10"),
                    new SqlParameter("@NumeIdent", numIdenti),
                    new SqlParameter("@NombrePrv", nombre),
                    new SqlParameter("@codOficialCumplimiento", _perfilesOptions.Value.Codigo_Oficial_Cumplimiento),
                };

                string sql = $"[prv].[SpTerceros] @Operacion = @Operacion, @NumeIdent = @NumeIdent, @NombrePrv = @NombrePrv, @codOficialCumplimiento = @codOficialCumplimiento";

                var response = await _context.ProveedorInspektors.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<Tercero>> GetParticipantesRequerimiento(int id)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","13"),
                    new SqlParameter("@reqIdRequerimientos",id)
                };

                string sql = $"[prv].[SpTerceros] @Operacion = @Operacion, @reqIdRequerimientos = @reqIdRequerimientos";

                var response = await _context.Terceros.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> AprobarDesaprobarInspektor(QueryAprobarDesaprobarInspektor data)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion", "14"),
                    new SqlParameter("@prvIdProveedores", data.CodProveedor),
                    new SqlParameter("@aprobarInspektor", data.AprobarInspektor),
                    new SqlParameter("@CodUserUpdate", data.CodUserUpdate ?? "")
                };

                string sql = CallStoredProcedures.Tercero.AprobarDesaprobarInspektor;
                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> ReenviarNotificacionCodigoValidacion(QueryReenvioNotificacionCodigoValidacion data)
        {
            try
            {
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Operacion", "15"),
                    new SqlParameter("@prvNit", data.Nit),
                };

                string sql = CallStoredProcedures.Tercero.ReenviarNotificacionCodigoValidacion;
                return await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> CambiarCorreoTercero(QueryCambiarCorreoProveedor data)
        {
            try
            {
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Operacion", "16"),
                    new SqlParameter("@prvNit", data.Nit),
                    new SqlParameter("@prvMail", data.Correo),
                    new SqlParameter("@prvMailAlterno", data.CorreoAlterno),
                    new SqlParameter("@CodUserUpdate", data.CodUserUpdate ?? "")
                };

                return await _context.ResponseActions.FromSqlRaw(CallStoredProcedures.Tercero.CambiarCorreoProveedor, parameters: parameters).ToListAsync();
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
    }
}