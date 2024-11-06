using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.ModelResponse;
using Core.QueryFilters;
using Core.Services;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    //private readonly IStoreProcedureService _storeProcedureService;

    //public class Op360GuardarOrdenRepository : IOp360GuardarOrdenRepository
    //{
    //    private readonly DbAireContext _dbAireContext;

    //    public Op360GuardarOrdenRepository(DbAireContext dbAireContext)
    //    {
    //        _dbAireContext = dbAireContext;
    //    }
    //    public async Task<FormdataResponse> CrearOrden(Op360GuardarOrden guardarOrden)
    //    {
    //        try
    //        {
    //            //Guardar un registro individual
    //            var listTest = await _storeProcedureService.Guardar_Orden_Formdata("aire.pkg_g_daniel_gonzalez_test.prc_registrar_orden", Parameters);
    //            FormdataResponse rst = new FormdataResponse()
    //            {
    //                id_orden = listTest.Codigo
    //            };
    //            return rst;
    //        }
    //        catch (Exception e)
    //        {
    //            throw new BusinessException($"Error: {e.Message}");
    //        }
    //    }


    //}
}
