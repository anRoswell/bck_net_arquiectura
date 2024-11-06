using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.QueryFilters;
using Core.Tools;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SispoRepository : ISispoRepository
    {
        protected readonly DbSispoContext _context;

        public SispoRepository(DbSispoContext context)
        {
            _context = context;
        }

        #region Facturas Por Pagar
        public async Task<List<EstadoCuentasXPorPagar>> EstadoCuentasXPorPagarApoteosys(QuerySearchEstadoCuentasXPagar parameters, List<Empresa> empresasActivas)
        {
            try
            {
                string empresaQuery = string.Empty;
                string numFacturaQuery = string.Empty;
                string fechaQuery = string.Empty;

                if (!string.IsNullOrEmpty(parameters.Empresa))
                {
                    empresaQuery = $"AND t.Empresa = '{parameters.Empresa}' ";
                }
                else
                {
                    var abrevEmpresasActivas = empresasActivas.Select(x => string.Concat("'",x.EmpAbreviatura,"'")).Distinct().ToList();
                    empresaQuery = $"AND t.Empresa IN ({string.Join(",", abrevEmpresasActivas)}) ";
                }

                if (!string.IsNullOrEmpty(parameters.Numero_Documento))
                {
                    numFacturaQuery = $"AND t.NUMERO_DOC = '{parameters.Numero_Documento}' ";
                }

                if (!string.IsNullOrEmpty(parameters.Fecha_Inicial) && !string.IsNullOrEmpty(parameters.Fecha_Final))
                {
                    fechaQuery = $"AND t.fecha_emision BETWEEN TO_DATE('{parameters.Fecha_Inicial}','dd/mm/yyyy')  " +
                            $"AND TO_DATE('{parameters.Fecha_Final}','dd/mm/yyyy')   ";
                }

                string sql = $"SELECT " +
                            $"TO_CHAR(t.FECHA_B,'MM') Periodo, " +
                            $"NVL(t.EMPRESA,'0') Empresa, " +
                            Funciones.GetQueryNombreEmpresa_Sispo() +
                            $"TO_CHAR(t.FECHA_B,'YYYY') Anio, " +
                            $"REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(TO_CHAR(MAX(NVL(TRUNC(t.fecha_emision), TO_DATE('01/12/1900','DD/MM/YYYY'))), 'DD/MM/YYYY'), '0202', '2020'), '0218', '2018'), '1899', '2020'), '0217', '2017'), '0220', '2020'), '0020', '2020'),'0201','2021'),'0021','2021') Fecha_Emision, " +
                            $"TO_CHAR(NVL(TRUNC(t.FECHA_GENERACION),TO_DATE('01/12/1900','DD/MM/YYYY')),'DD/MM/YYYY') Fecha_Generacion, " +
                            $"TO_CHAR(MAX(NVL(TRUNC(t.FECHA_VENCIMIENTO),TO_DATE('01/12/1900','DD/MM/YYYY'))),'DD/MM/YYYY') Fecha_Vencimiento, " +
                            $"TO_CHAR(MAX(NVL(TRUNC(t.FECHA_ESPERADA_PAGO),TO_DATE('01/12/1900','DD/MM/YYYY'))),'DD/MM/YYYY') Fecha_Estimado_Pago, " +
                            $"NVL(t.TERCERO,0) Codigo_Tercero, " +
                            $"NVL(t.NAME_TERCERO,'0') Nombre_Tercero, " +
                            $"NVL(t.DOC_DS,'0') Tipo_Documento, " +
                            $"NVL(t.CODIGO_TD,'0') Codigo_Td, " +
                            $"NVL(t.NUMERO_B,0) Numero_B, " +
                            $@"REPLACE(REGEXP_REPLACE(LISTAGG(REPLACE(REPLACE(NVL(t.NUMERO_DOC,'0'),'|',' '),',',' '), ',') WITHIN GROUP (ORDER BY NUMERO_DOC),'([^,]+)(,\1)+', '\1'),',','|') Numero_Documento, " +
                            $"LTRIM(TO_CHAR(SUM(SALDO), '$999,999,999,999,999.00')) Valor_A_Pagar " +
                            $"FROM sgfinanc.tmpcpx t " +
                            $"WHERE " +
                            $"nvl(t.tipo_mvto,0) NOT IN (0,5) " +
                            $"AND UPPER(t.USUARIO_CREA) LIKE 'JOB' " +
                            $"AND NVL(t.TERCERO,0) = {parameters.NitProveedor} " +
                            $"AND (t.DOC_DS LIKE 'FE%' OR t.DOC_DS LIKE 'FC%') " +
                            $"{empresaQuery}" +
                            $"{numFacturaQuery}" +
                            $"{fechaQuery}" +
                            $"GROUP BY " +
                            $"t.TIPO_MVTO, " +
                            $"t.NAME_MVTO, " +
                            $"t.EMPRESA, " +
                            $"NVL(t.EMPRESA,'0'), " +
                            $"NVL(t.TERCERO,0), " +
                            $"t.NAME_TERCERO, " +
                            $"t.DOC_DS, " +
                            $"NVL(t.NRO_TRANSACCION,0), " +
                            $"t.USUARIO_CREA, " +
                            $"TO_CHAR(NVL(TRUNC(t.FECHA_GENERACION),TO_DATE('01/12/1900','DD/MM/YYYY')),'DD/MM/YYYY'), " +
                            $"TO_CHAR(NVL(TRUNC(t.FECHA_EMISION),TO_DATE('01/12/1900','DD/MM/YYYY')),'DD/MM/YYYY'), " +
                            $"TO_CHAR(NVL(TRUNC(t.FECHA_VENCIMIENTO),TO_DATE('01/12/1900','DD/MM/YYYY')),'DD/MM/YYYY'), " +
                            $"TO_CHAR(NVL(TRUNC(t.FECHA_ESPERADA_PAGO),TO_DATE('01/12/1900','DD/MM/YYYY')),'DD/MM/YYYY'), " +
                            $"TO_CHAR(NVL(TRUNC(t.FECHA_ULT_ABONO),TO_DATE('01/12/1900','DD/MM/YYYY')),'DD/MM/YYYY'), " +
                            $"NVL(t.CODIGO_TD,'0'), " +
                            $"NVL(t.NUMERO_B,0), " +
                            $"TO_CHAR(NVL(TRUNC(t.FECHA_B),TO_DATE('01/12/1900','DD/MM/YYYY')),'DD/MM/YYYY'), " +
                            $"TO_CHAR(t.FECHA_B,'MM'), " +
                            $"TO_CHAR(t.FECHA_B,'YYYY') " +
                            $"UNION " +
                            $"SELECT " +
                            $"TO_CHAR(t.FECHA_B,'MM') Periodo, " +
                            $"NVL(t.EMPRESA,'0') Empresa, " +
                            Funciones.GetQueryNombreEmpresa_Sispo() +
                            $"TO_CHAR(t.FECHA_B,'YYYY') Anio, " +
                            $"REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(TO_CHAR(MAX(NVL(TRUNC(t.fecha_emision), TO_DATE('01/12/1900','DD/MM/YYYY'))), 'DD/MM/YYYY'), '0202', '2020'), '0218', '2018'), '1899', '2020'), '0217', '2017'), '0220', '2020'), '0020', '2020') Fecha_Emision, " +
                            $"TO_CHAR(NVL(TRUNC(t.FECHA_GENERACION),TO_DATE('01/12/1900','DD/MM/YYYY')),'DD/MM/YYYY') Fecha_Generacion, " +
                            $"TO_CHAR(MAX(NVL(TRUNC(t.FECHA_VENCIMIENTO),TO_DATE('01/12/1900','DD/MM/YYYY'))),'DD/MM/YYYY') Fecha_Vencimiento, " +
                            $"TO_CHAR(MAX(NVL(TRUNC(t.FECHA_ESPERADA_PAGO),TO_DATE('01/12/1900','DD/MM/YYYY'))),'DD/MM/YYYY') Fecha_Estimado_Pago, " +
                            $"NVL(t.TERCERO,0) Codigo_Tercero, " +
                            $"NVL(t.NAME_TERCERO,'0') Nombre_Tercero, " +
                            $"NVL(t.DOC_DS,'0') Tipo_Documento, " +
                            $"NVL(t.CODIGO_TD,'0') Codigo_Td, " +
                            $"NVL(t.NUMERO_B,0) Numero_B, " +
                            $"t.NUMERO_DOC Numero_Documento, " +
                            $"LTRIM(TO_CHAR(SUM(SALDO), '$999,999,999,999,999.00')) Valor_A_Pagar " +
                            $"FROM sgfinanc.tmpcpx t " +
                            $"WHERE " +
                            $"NVL(t.tipo_mvto,0) IN (5) " +
                            $"AND UPPER(t.USUARIO_CREA) LIKE 'JOB' " +
                            $"AND NVL(t.TERCERO,0) = {parameters.NitProveedor} " +
                            $"AND (t.DOC_DS LIKE 'FE%' OR t.DOC_DS LIKE 'FC%') " +
                            $"{empresaQuery}" +
                            $"{numFacturaQuery}" +
                            $"{fechaQuery}" +
                            $"GROUP BY " +
                            $"t.TIPO_MVTO, " +
                            $"t.NAME_MVTO, " +
                            $"t.EMPRESA, " +
                            $"NVL(t.EMPRESA,'0'), " +
                            $"NVL(t.TERCERO,0), " +
                            $"t.NAME_TERCERO, " +
                            $"t.DOC_DS, " +
                            $"NVL(t.NRO_TRANSACCION,0), " +
                            $"t.USUARIO_CREA, " +
                            $"TO_CHAR(NVL(TRUNC(t.FECHA_GENERACION),TO_DATE('01/12/1900','DD/MM/YYYY')),'DD/MM/YYYY'), " +
                            $"TO_CHAR(NVL(TRUNC(t.FECHA_EMISION),TO_DATE('01/12/1900','DD/MM/YYYY')),'DD/MM/YYYY'), " +
                            $"TO_CHAR(NVL(TRUNC(t.FECHA_VENCIMIENTO),TO_DATE('01/12/1900','DD/MM/YYYY')),'DD/MM/YYYY'), " +
                            $"TO_CHAR(NVL(TRUNC(t.FECHA_ESPERADA_PAGO),TO_DATE('01/12/1900','DD/MM/YYYY')),'DD/MM/YYYY'), " +
                            $"TO_CHAR(NVL(TRUNC(t.FECHA_ULT_ABONO),TO_DATE('01/12/1900','DD/MM/YYYY')),'DD/MM/YYYY'), " +
                            $"NVL(t.CODIGO_TD,'0'), " +
                            $"NVL(t.NUMERO_B,0), " +
                            $"TO_CHAR(NVL(TRUNC(t.FECHA_B),TO_DATE('01/12/1900','DD/MM/YYYY')),'DD/MM/YYYY'), " +
                            $"TO_CHAR(t.FECHA_B,'MM'), " +
                            $"TO_CHAR(t.FECHA_B,'YYYY'), " +
                            $"t.NUMERO_DOC ";

                var response = await _context.EstadoCuentasXPorPagar.FromSqlRaw(sql).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<EstadoCuentasXPorPagar>> EstadoCuentasXPorPagarApoteosysDte(QuerySearchEstadoCuentasXPagarDetalle parameters)
        {
            try
            {
                string sql = $"SELECT " +
                            $"TO_CHAR(t.FECHA_B,'MM') Periodo, " +
                            $"NVL(t.EMPRESA,'0') Empresa, " +
                            Funciones.GetQueryNombreEmpresa_Sispo() +
                            $"TO_CHAR(t.FECHA_B,'YYYY') Anio, " +
                            $"REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(TO_CHAR(MAX(NVL(TRUNC(t.fecha_emision), TO_DATE('01/12/1900','DD/MM/YYYY'))), 'DD/MM/YYYY'), '0202', '2020'), '0218', '2018'), '1899', '2020'), '0217', '2017'), '0220', '2020'), '0020', '2020'),'0201','2021'),'0021','2021') Fecha_Emision, " +
                            $"TO_CHAR(NVL(TRUNC(t.FECHA_GENERACION),TO_DATE('01/12/1900','DD/MM/YYYY')),'DD/MM/YYYY') Fecha_Generacion, " +
                            $"TO_CHAR(MAX(NVL(TRUNC(t.FECHA_VENCIMIENTO),TO_DATE('01/12/1900','DD/MM/YYYY'))),'DD/MM/YYYY') Fecha_Vencimiento, " +
                            $"TO_CHAR(MAX(NVL(TRUNC(t.FECHA_ESPERADA_PAGO),TO_DATE('01/12/1900','DD/MM/YYYY'))),'DD/MM/YYYY') Fecha_Estimado_Pago, " +
                            $"NVL(t.TERCERO,0) Codigo_Tercero, " +
                            $"NVL(t.NAME_TERCERO,'0') Nombre_Tercero, " +
                            $"NVL(t.DOC_DS,'0') Tipo_Documento, " +
                            $"NVL(t.CODIGO_TD,'0') Codigo_Td, " +
                            $"NVL(t.NUMERO_B,0) Numero_B, " +
                            $@"REPLACE(REGEXP_REPLACE(LISTAGG(REPLACE(REPLACE(NVL(t.NUMERO_DOC,'0'),'|',' '),',',' '), ',') WITHIN GROUP (ORDER BY NUMERO_DOC),'([^,]+)(,\1)+', '\1'),',','|') Numero_Documento, " +
                            $"LTRIM(TO_CHAR(SUM(SALDO), '$999,999,999,999,999.00')) Valor_A_Pagar " +
                            $"FROM sgfinanc.tmpcpx t " +
                            $"WHERE " +
                            $"nvl(t.tipo_mvto,0) NOT IN (0,5) " +
                            $"AND UPPER(t.USUARIO_CREA) LIKE 'JOB' " +
                            $"AND NVL(t.TERCERO,0) = {parameters.NitProveedor} " +
                            $"AND t.EMPRESA = '{parameters.Empresa}' " +
                            $"AND t.DOC_DS = '{parameters.Tipo_Documento}' " +
                            $"AND t.NUMERO_B = '{parameters.Numero_B}' " +
                            $"AND ROWNUM = 1 " +
                            $"GROUP BY " +
                            $"t.TIPO_MVTO, " +
                            $"t.NAME_MVTO, " +
                            $"t.EMPRESA, " +
                            $"NVL(t.EMPRESA,'0'), " +
                            $"NVL(t.TERCERO,0), " +
                            $"t.NAME_TERCERO, " +
                            $"t.DOC_DS, " +
                            $"NVL(t.NRO_TRANSACCION,0), " +
                            $"t.USUARIO_CREA, " +
                            $"TO_CHAR(NVL(TRUNC(t.FECHA_GENERACION),TO_DATE('01/12/1900','DD/MM/YYYY')),'DD/MM/YYYY'), " +
                            $"TO_CHAR(NVL(TRUNC(t.FECHA_EMISION),TO_DATE('01/12/1900','DD/MM/YYYY')),'DD/MM/YYYY'), " +
                            $"TO_CHAR(NVL(TRUNC(t.FECHA_VENCIMIENTO),TO_DATE('01/12/1900','DD/MM/YYYY')),'DD/MM/YYYY'), " +
                            $"TO_CHAR(NVL(TRUNC(t.FECHA_ESPERADA_PAGO),TO_DATE('01/12/1900','DD/MM/YYYY')),'DD/MM/YYYY'), " +
                            $"TO_CHAR(NVL(TRUNC(t.FECHA_ULT_ABONO),TO_DATE('01/12/1900','DD/MM/YYYY')),'DD/MM/YYYY'), " +
                            $"NVL(t.CODIGO_TD,'0'), " +
                            $"NVL(t.NUMERO_B,0), " +
                            $"TO_CHAR(NVL(TRUNC(t.FECHA_B),TO_DATE('01/12/1900','DD/MM/YYYY')),'DD/MM/YYYY'), " +
                            $"TO_CHAR(t.FECHA_B,'MM'), " +
                            $"TO_CHAR(t.FECHA_B,'YYYY') " +
                            $"UNION " +
                            $"SELECT " +
                            $"TO_CHAR(t.FECHA_B,'MM') Periodo, " +
                            $"NVL(t.EMPRESA,'0') Empresa, " +
                            Funciones.GetQueryNombreEmpresa_Sispo() +
                            $"TO_CHAR(t.FECHA_B,'YYYY') Anio, " +
                            $"REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(TO_CHAR(MAX(NVL(TRUNC(t.fecha_emision), TO_DATE('01/12/1900','DD/MM/YYYY'))), 'DD/MM/YYYY'), '0202', '2020'), '0218', '2018'), '1899', '2020'), '0217', '2017'), '0220', '2020'), '0020', '2020') Fecha_Emision, " +
                            $"TO_CHAR(NVL(TRUNC(t.FECHA_GENERACION),TO_DATE('01/12/1900','DD/MM/YYYY')),'DD/MM/YYYY') Fecha_Generacion, " +
                            $"TO_CHAR(MAX(NVL(TRUNC(t.FECHA_VENCIMIENTO),TO_DATE('01/12/1900','DD/MM/YYYY'))),'DD/MM/YYYY') Fecha_Vencimiento, " +
                            $"TO_CHAR(MAX(NVL(TRUNC(t.FECHA_ESPERADA_PAGO),TO_DATE('01/12/1900','DD/MM/YYYY'))),'DD/MM/YYYY') Fecha_Estimado_Pago, " +
                            $"NVL(t.TERCERO,0) Codigo_Tercero, " +
                            $"NVL(t.NAME_TERCERO,'0') Nombre_Tercero, " +
                            $"NVL(t.DOC_DS,'0') Tipo_Documento, " +
                            $"NVL(t.CODIGO_TD,'0') Codigo_Td, " +
                            $"NVL(t.NUMERO_B,0) Numero_B, " +
                            $"t.NUMERO_DOC Numero_Documento, " +
                            $"LTRIM(TO_CHAR(SUM(SALDO), '$999,999,999,999,999.00')) Valor_A_Pagar " +
                            $"FROM sgfinanc.tmpcpx t " +
                            $"WHERE " +
                            $"NVL(t.tipo_mvto,0) IN (5) " +
                            $"AND UPPER(t.USUARIO_CREA) LIKE 'JOB' " +
                            $"AND NVL(t.TERCERO,0) = {parameters.NitProveedor} " +
                            $"AND t.EMPRESA = '{parameters.Empresa}' " +
                            $"AND t.DOC_DS = '{parameters.Tipo_Documento}' " +
                            $"AND t.NUMERO_B = '{parameters.Numero_B}' " +
                            $"AND ROWNUM = 1 " +
                            $"GROUP BY " +
                            $"t.TIPO_MVTO, " +
                            $"t.NAME_MVTO, " +
                            $"t.EMPRESA, " +
                            $"NVL(t.EMPRESA,'0'), " +
                            $"NVL(t.TERCERO,0), " +
                            $"t.NAME_TERCERO, " +
                            $"t.DOC_DS, " +
                            $"NVL(t.NRO_TRANSACCION,0), " +
                            $"t.USUARIO_CREA, " +
                            $"TO_CHAR(NVL(TRUNC(t.FECHA_GENERACION),TO_DATE('01/12/1900','DD/MM/YYYY')),'DD/MM/YYYY'), " +
                            $"TO_CHAR(NVL(TRUNC(t.FECHA_EMISION),TO_DATE('01/12/1900','DD/MM/YYYY')),'DD/MM/YYYY'), " +
                            $"TO_CHAR(NVL(TRUNC(t.FECHA_VENCIMIENTO),TO_DATE('01/12/1900','DD/MM/YYYY')),'DD/MM/YYYY'), " +
                            $"TO_CHAR(NVL(TRUNC(t.FECHA_ESPERADA_PAGO),TO_DATE('01/12/1900','DD/MM/YYYY')),'DD/MM/YYYY'), " +
                            $"TO_CHAR(NVL(TRUNC(t.FECHA_ULT_ABONO),TO_DATE('01/12/1900','DD/MM/YYYY')),'DD/MM/YYYY'), " +
                            $"NVL(t.CODIGO_TD,'0'), " +
                            $"NVL(t.NUMERO_B,0), " +
                            $"TO_CHAR(NVL(TRUNC(t.FECHA_B),TO_DATE('01/12/1900','DD/MM/YYYY')),'DD/MM/YYYY'), " +
                            $"TO_CHAR(t.FECHA_B,'MM'), " +
                            $"TO_CHAR(t.FECHA_B,'YYYY'), " +
                            $"t.NUMERO_DOC ";

                var response = await _context.EstadoCuentasXPorPagar.FromSqlRaw(sql).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
        #endregion
    }
}
