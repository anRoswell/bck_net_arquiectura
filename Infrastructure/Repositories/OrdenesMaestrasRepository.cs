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
    public class OrdenesMaestrasRepository : BaseRepository<OrdenesMaestras>, IOrdenesMaestrasRepository
    {
        public OrdenesMaestrasRepository(DbModelContext context) : base(context) { }

        public async Task<List<OrdenesMaestras>> GetListado()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","1")
                };

                string sql = $"ord.SPOrdenesMaestras @Operacion = @Operacion";

                var response = await _context.OrdenesMaestras.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PostOrdenReq(OrdenReq ordenReq)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","2"),
                    new SqlParameter("@CodOrden",ordenReq.Id),
                    new SqlParameter("@CodRequerimiento",ordenReq.CodRequerimiento),
                    new SqlParameter("@CodProveedor",ordenReq.CodProveedor),
                    new SqlParameter("@CodUser",ordenReq.CodUser),
                };

                string sql = $"ord.SPOrdenesMaestras @Operacion = @Operacion, @CodOrden=@CodOrden, @CodRequerimiento=@CodRequerimiento, @CodProveedor=@CodProveedor,@CodUser=@CodUser ";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<OrdenesMaestras>> SearchByProveedor( QuerySearchOrdenes parametros)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","3"),
                    new SqlParameter("@CodProveedor",parametros.CodProveedor),
                };

                string sql = $"ord.SPOrdenesMaestras @Operacion = @Operacion, @CodProveedor=@CodProveedor";

                var response = await _context.OrdenesMaestras.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
    }
}
