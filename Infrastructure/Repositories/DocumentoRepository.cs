namespace Infrastructure.Repositories
{
    using Core.DTOs;
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

    public class DocumentoRepository : BaseRepository<Documento>, IDocumentoRepository
    {
        public DocumentoRepository(DbModelContext context) : base(context) { }

        public async Task<List<Documento>> GetDocumentos()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","1")
                };

                string sql = CallStoredProcedures.Documento.BuscarDocumentos;
                var response = await _context.Documento.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<Documento>> GetDocumento(int id)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","2"),
                    new SqlParameter("@cocIdDocumentos", id)
                };

                string sql = CallStoredProcedures.Documento.BuscarDocumentoById;
                var response = await _context.Documento.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PostCrear(DocumentoDto doc)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","3"),
                    new SqlParameter("@cocCodModuloDocumentos", doc.CocCodModuloDocumentos),
                    new SqlParameter("@cocNombreDocumento", doc.CocNombreDocumento),
                    new SqlParameter("@cocDescripcion", doc.CocDescripcion),
                    new SqlParameter("@cocrequiered", doc.Cocrequiered),
                    new SqlParameter("@coclimitLoad", doc.CoclimitLoad),
                    new SqlParameter("@cocVigencia", doc.CocVigencia),
                    new SqlParameter("@cocVigenciaMaxima", doc.CocVigenciaMaxima),
                    new SqlParameter("@CodUser", doc.CodUser)
                };

                string sql = CallStoredProcedures.Documento.CrearDocumento;
                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PutActualizar(DocumentoDto doc)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","4"),
                    new SqlParameter("@cocIdDocumentos", doc.Id),
                    new SqlParameter("@cocCodModuloDocumentos", doc.CocCodModuloDocumentos),
                    new SqlParameter("@cocNombreDocumento", doc.CocNombreDocumento),
                    new SqlParameter("@cocDescripcion", doc.CocDescripcion),
                    new SqlParameter("@cocEstado", doc.CocEstado),
                    new SqlParameter("@cocrequiered", doc.Cocrequiered),
                    new SqlParameter("@coclimitLoad", doc.CoclimitLoad),
                    new SqlParameter("@cocVigencia", doc.CocVigencia),
                    new SqlParameter("@cocVigenciaMaxima", doc.CocVigenciaMaxima),
                    new SqlParameter("@CodUserUpdate", doc.CodUserUpdate ?? "")
                };

                string sql = CallStoredProcedures.Documento.ActualizarDocumento;
                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> Delete(DocumentoDto doc)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","5"),
                    new SqlParameter("@cocIdDocumentos", doc.Id),
                    new SqlParameter("@CodUserUpdate", doc.CodUserUpdate ?? "")
                };

                string sql = CallStoredProcedures.Documento.EliminarDocumento;
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
