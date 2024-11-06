using Core.CustomEntities;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Tools;
using Infrastructure.Data;
using Infrastructure.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PlantillaRepository : BaseRepository<Plantilla>, IPlantillaRepository
    {
        public PlantillaRepository(DbModelContext context): base(context) { }

        public async Task<List<RequerimientoComparativoDto>> ConsultarJson(int idReq)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","2"),
                    new SqlParameter("@reqIdRequerimientos",idReq)
                };

                string sql = $"[dbo].[SpPlantilla] @Operacion = @Operacion, @reqIdRequerimientos = @reqIdRequerimientos";

                var response = await _context.ResponseJsonStrings.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return BaseResponseJson<RootJsonResponse<RequerimientoComparativoDto>>.ConvertJsonToEntity(response)?.JsonResponse;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<DataTableCollection> ConsultarMultiplesTablas()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","1")
                };

                string sql = $"[dbo].[SpPlantilla]";

                var ds = await SqlCommandExtension.GetDataTables(_context, sql, parameters);
                return ds.Tables;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
    }
}
