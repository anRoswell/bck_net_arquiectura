using Core.CustomEntities;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.ModelResponse;
using Infrastructure.Data;
using Infrastructure.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AdobeSignRepository : BaseRepository<PrvAdobeSign>, IAdobeSignRepository
    {
        public AdobeSignRepository(DbModelContext context) : base(context) { }

        public async Task<List<ResponseAction>> GuardarTrazaAdobeSign_Prv(int idProveedor, int codAdobeSignEstado, string returnedAdobeSignId, string returnedAdobeSignJson)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion", "1"),
                    new SqlParameter("@CodProveedor", idProveedor),
                    new SqlParameter("@CodAdobeSignEstado", codAdobeSignEstado),
                    new SqlParameter("@adsgnIdAdobeSign", returnedAdobeSignId ?? ""),
                    new SqlParameter("@adsgnJson", returnedAdobeSignJson ?? ""),
                };

                string sql = CallStoredProcedures.AdobeSign.GuardarTraza_Prv;
                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> GuardarTrazaAdobeSign_Contrato(int idContrato, int codAdobeSignEstado, string returnedAdobeSignId, string returnedAdobeSignJson)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion", "2"),
                    new SqlParameter("@CodContrato", idContrato),
                    new SqlParameter("@CodAdobeSignEstado", codAdobeSignEstado),
                    new SqlParameter("@adsgnIdAdobeSign", returnedAdobeSignId ?? ""),
                    new SqlParameter("@adsgnJson", returnedAdobeSignJson ?? ""),
                };

                string sql = CallStoredProcedures.AdobeSign.GuardarTraza_Contrato;
                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> GuardarLogErrorAdobeSign(adobeErrorResponse adobeError, string proceso, int idLlave)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion", "3"),
                    new SqlParameter("@adsgeErrorCode", adobeError.Code ?? ""),
                    new SqlParameter("@adsgeErrorMessage", adobeError.Message ?? ""),
                    new SqlParameter("@adsgeCodLlave", idLlave),
                    new SqlParameter("@adsgeErrorProcess", proceso ?? ""),
                };

                string sql = CallStoredProcedures.AdobeSign.GuardarLogError_AdobeSign;
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
