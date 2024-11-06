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
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CertificadosEspecialesRepository : BaseRepository<CertificadosEspeciale>, ICertificadosEspecialesRepository
    {
        public CertificadosEspecialesRepository(DbModelContext context) : base(context) { }

        public async Task<List<CertificadosEspeciale>> GetCertificadosEspeciales()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","1")
                };

                string sql = $"[cer].[SpCertificadosEspeciales] @Operacion = @Operacion";

                var response = await _context.CertificadosEspeciales.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<CertificadosEspeciale>> GetCertificadosEspecialesProveedor(string idUser)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","5"),
                    new SqlParameter("@CodUser",idUser)
                };

                string sql = $"[cer].[SpCertificadosEspeciales] @Operacion = @Operacion, @CodUser = @CodUser";

                var response = await _context.CertificadosEspeciales.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<CertificadosEspeciale>> GetCertificadosEspecialesFiltro(QuerySearchFiltroCertificados query)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","8"),
                    new SqlParameter("@cerEstado",query.EstadoCertificado),
                    new SqlParameter("@Fecha_Inicio",query.Fecha_Inicio.ToString("yyyyMMdd")),
                    new SqlParameter("@Fecha_Fin",query.Fecha_Fin.ToString("yyyyMMdd")),
                    new SqlParameter("@CodUser",query.CodUser)
                };

                string sql = $"[cer].[SpCertificadosEspeciales] @Operacion = @Operacion, @cerEstado = @cerEstado, @Fecha_Inicio = @Fecha_Inicio, @Fecha_Fin = @Fecha_Fin, @CodUser = @CodUser";

                var response = await _context.CertificadosEspeciales.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<CertificadosEspeciale>> GetCertificadoPorID(int id)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","3"),
                    new SqlParameter("@cerIdCertificadosEspeciales",id)
                };

                string sql = $"[cer].[SpCertificadosEspeciales] @Operacion = @Operacion, @cerIdCertificadosEspeciales = @cerIdCertificadosEspeciales";

                var response = await _context.CertificadosEspeciales.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<RespuestasCertEspeciale>> GetRespuestasCertificadoPorID(int id)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","7"),
                    new SqlParameter("@cerIdCertificadosEspeciales",id)
                };

                string sql = $"[cer].[SpCertificadosEspeciales] @Operacion = @Operacion, @cerIdCertificadosEspeciales = @cerIdCertificadosEspeciales";

                var response = await _context.RespuestasCertEspeciales.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> CrearCertificadoEspecial(CertificadosEspecialesDto certificados)
        {
            try
            {
                SqlParameter[] parameters;
                string sql = string.Empty;

                if (certificados.CerCodTipoCertificado == 1) // Individual por Empresa
                {
                    StringBuilder InsertEmp = new StringBuilder();

                    // Construir Inserts Empresas
                    foreach (int item in certificados.ListadoEmpresas)
                    {
                        InsertEmp.Append("INSERT INTO #empresas (CodEmpresas) ");
                        InsertEmp.Append($"VALUES({item}) ");
                    }

                    parameters = new[] {
                        new SqlParameter("@Operacion","2"),
                        new SqlParameter("@cerDescripcion", certificados.CerDescripcion),
                        new SqlParameter("@cerPeriodo", certificados.CerPeriodo),
                        new SqlParameter("@cerDestinatario", certificados.CerDestinatario),
                        new SqlParameter("@cerIncluirGarantia", certificados.CerIncluirGarantia),
                        new SqlParameter("@cerCodTipoCertificado", certificados.CerCodTipoCertificado),
                        new SqlParameter("@CodUser", certificados.CodUser),
                        new SqlParameter("@InsertEmpresas", InsertEmp.ToString())
                    };

                    sql = $"[cer].[SpCertificadosEspeciales] @Operacion = @Operacion, @cerDescripcion = @cerDescripcion, @cerPeriodo = @cerPeriodo, @cerDestinatario = @cerDestinatario, @cerIncluirGarantia = @cerIncluirGarantia, " +
                        $"@cerCodTipoCertificado = @cerCodTipoCertificado, @CodUser = @CodUser, @InsertEmpresas = @InsertEmpresas";
                }
                else
                {
                    parameters = new[] {
                        new SqlParameter("@Operacion","2"),
                        new SqlParameter("@cerDescripcion", certificados.CerDescripcion),
                        new SqlParameter("@cerPeriodo", certificados.CerPeriodo),
                        new SqlParameter("@cerDestinatario", certificados.CerDestinatario),
                        new SqlParameter("@cerIncluirGarantia", certificados.CerIncluirGarantia),
                        new SqlParameter("@cerCodTipoCertificado", certificados.CerCodTipoCertificado),
                        new SqlParameter("@CodUser", certificados.CodUser)
                    };

                    sql = $"[cer].[SpCertificadosEspeciales] @Operacion = @Operacion, @cerDescripcion = @cerDescripcion, @cerPeriodo = @cerPeriodo, @cerDestinatario = @cerDestinatario, @cerIncluirGarantia = @cerIncluirGarantia, " +
                        $"@cerCodTipoCertificado = @cerCodTipoCertificado, @CodUser = @CodUser";
                }                

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> UpdateCertificadoEspecial(CertificadosEspecialesDto certificados)
        {
            try
            {
                StringBuilder InsertEmp = new StringBuilder();

                // Construir Inserts Empresas
                foreach (int item in certificados.ListadoEmpresas)
                {
                    InsertEmp.Append("INSERT INTO #empresas_update (CodEmpresas) ");
                    InsertEmp.Append($"VALUES({item}) ");
                }

                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","6"),
                    new SqlParameter("@cerIdCertificadosEspeciales", certificados.Id),
                    new SqlParameter("@cerDescripcion", certificados.CerDescripcion),
                    new SqlParameter("@cerPeriodo", certificados.CerPeriodo),
                    new SqlParameter("@cerDestinatario", certificados.CerDestinatario),
                    new SqlParameter("@cerIncluirGarantia", certificados.CerIncluirGarantia),
                    new SqlParameter("@cerCodTipoCertificado", certificados.CerCodTipoCertificado),
                    new SqlParameter("@CodUserUpdate", certificados.CodUserUpdate),
                    new SqlParameter("@InsertEmpresas", InsertEmp.ToString())
                };

                string sql = $"[cer].[SpCertificadosEspeciales] @Operacion = @Operacion, @cerIdCertificadosEspeciales = @cerIdCertificadosEspeciales, @cerDescripcion = @cerDescripcion, @cerPeriodo = @cerPeriodo, @cerDestinatario = @cerDestinatario, " +
                    $"@cerIncluirGarantia = @cerIncluirGarantia, @cerCodTipoCertificado = @cerCodTipoCertificado, @CodUserUpdate = @CodUserUpdate, @InsertEmpresas = @InsertEmpresas";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> UpdateEstadoCertificadoEspecial(QueryUpdateEstadoCertificado certificados)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","4"),
                    new SqlParameter("@cerIdCertificadosEspeciales", certificados.CodCertificado),
                    new SqlParameter("@cerEstado", certificados.Estado),
                    new SqlParameter("@ObservEstado", certificados.Observaciones ?? ""),
                    new SqlParameter("@cerHtmlPdf", certificados.CerHtmlPdf ?? ""),
                    new SqlParameter("@CodUserUpdate", certificados.CodUserUpdate)
                };

                string sql = $"[cer].[SpCertificadosEspeciales] @Operacion = @Operacion, @cerIdCertificadosEspeciales = @cerIdCertificadosEspeciales, @cerEstado = @cerEstado, @cerHtmlPdf = @cerHtmlPdf, @ObservEstado = @ObservEstado, @CodUserUpdate = @CodUserUpdate";

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
