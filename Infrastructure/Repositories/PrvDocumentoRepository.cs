using Core.CustomEntities.Parametros;
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

namespace Infrastructure.Repositories
{
    public class PrvDocumentoRepository : BaseRepository<PrvDocumento>, IPrvDocumentoRepository
    {
        public PrvDocumentoRepository(DbModelContext context) : base(context) { }

        public async Task<List<PrvDocumento>> GetPrvDocumento(int id)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","1"),
                    new SqlParameter("@prvdCodProveedor",id)
                };

                string sql = $"[prv].[SpPrvDocumento] @Operacion = @Operacion, @prvdCodProveedor = @prvdCodProveedor";

                var response = await _context.PrvDocumentos.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseActionUrl>> DeleteDocumentoOther(QueryDeleteDocOther documento)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion", "5"),
                    new SqlParameter("@IdDocumento", documento.IdDocumento),
                    new SqlParameter("@CodUserUpdate", documento.CodUserUpdate)
                };

                string sql = $"[prv].[SpPrvDocumento] @Operacion = @Operacion, @IdDocumento = @IdDocumento, @CodUserUpdate = @CodUserUpdate";

                var response = await _context.ResponseActionUrls.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<PrvDocumento>> GetDocumentosProveedorReq(ParamDocumentosPrvReq parametros)
        {
            try
            {
                DataTable ids = new DataTable();
                ids.Columns.Add("Id", typeof(int));

                foreach (var item in parametros.IdsDocumentos)
                {
                    ids.Rows.Add(item);
                }

                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion", "5"),
                    new SqlParameter("@CodProveedor",parametros.CodProveedor),
                    new SqlParameter("@listadoIdsDocumentos",ids) { SqlDbType = SqlDbType.Structured, TypeName = "ListadoEnteros" }
                };

                string sql = $"[prv].[SpGetDocuments] @Operacion = @Operacion, @CodProveedor = @CodProveedor, @listadoIdsDocumentos = @listadoIdsDocumentos";

                var response = await _context.PrvDocumentos.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
    }
}
