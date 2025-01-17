﻿using Core.Entities;
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
    public class ApoteosysRepository : IApoteosysRepository
    {
        protected readonly DbOracleContext _context;

        public ApoteosysRepository(DbOracleContext context)
        {
            _context = context;
        }

        #region Facturación
        #region Facturas Por Pagar
        public async Task<List<EstadoCuentasXPagarDetalle>> EstadoCuentasXPorPagarDetalleApoteosys(QuerySearchEstadoCuentasXPagarDetalle parameters)
        {
            try
            {
                string sql = $"SELECT " +
                    $"MC_____CODIGO____CONTAB_B Empresa, " +
                    $"MC_____CODIGO____TD_____B Tipo_Documento, " +
                    $"MC_____FECHA_____B Fecha, " +
                    $"MC_____NUMERO____B Numero, " +
                    $"MC_____SECUINTE__B Consecutivo, " +
                    $"MC_____CODIGO____PF_____B Anio, " +
                    $"MC_____NUMERO____PERIOD_B Periodo, " +
                    $"MC_____CODIGO____PC_____B Codigo_Plan_Contable, " +
                    $"MC_____CODIGO____CPC____B Cuenta_Contable, " +
                    $"p.CPC____NOMBRE____B Nombre_Cuenta_Contable, " +
                    $"MC_____CODIGO____CU_____B Centro_Costo, " +
                    $"c.CU_____NOMBRE____B Nombre_Centro_Costo, " +
                    $"MC_____IDENTIFIC_TERCER_B Codigo_Tercero, " +
                    $"t.TERCER_NOMBEXTE__B Nombre_Tercero, " +
                    $"MC_____CODIGO____DS_____B Documento_Alterno, " +
                    $"MC_____NUMDOCSOP_B Numero_Documento_Alterno, " +
                    $"MC_____FECHEMIS__B Fecha_Emision, " +
                    $"MC_____FECHVENC__B Fecha_Vencimiento, " +
                    $"LTRIM(TO_CHAR(MC_____VALOIVA___B, '$999,999,999,999,999.00')) Iva_Mayor_Valor, " +
                    $"LTRIM(TO_CHAR(MC_____BASE______B, '$999,999,999,999,999.00')) Base_Retencion, " +
                    $"LTRIM(TO_CHAR(MC_____DEBMONLOC_B, '$999,999,999,999,999.00')) Debito, " +
                    $"LTRIM(TO_CHAR(MC_____CREMONLOC_B, '$999,999,999,999,999.00')) Credito, " +
                    $"MC_____OBSERVACI_B Observaciones " +
                    $"FROM APOTEOSYS.MC____ a " +
                    $"INNER JOIN APOTEOSYS.CPC___ p ON a.MC_____CODIGO____CPC____B = p.CPC____CODIGO____B AND p.CPC____CODIGO____PC_____B = 'NIIF' " +
                    $"INNER JOIN APOTEOSYS.CU____ c ON a.MC_____CODIGO____CU_____B = c.CU_____CODIGO____B " +
                    $"INNER JOIN APOTEOSYS.TERCER t ON a.MC_____IDENTIFIC_TERCER_B = t.TERCER_IDENTIFIC_B  " +
                    $"WHERE a.MC_____CODIGO____CONTAB_B = '{parameters.Empresa}' " +
                    $"AND a.MC_____CODIGO____CU_____B IS NOT NULL " +
                    $"AND a.MC_____IDENTIFIC_TERCER_B = '{parameters.NitProveedor}' " +
                    $"AND a.MC_____CODIGO____TD_____B = '{parameters.Codigo_Td}' " +
                    $"AND a.MC_____NUMERO____B = '{parameters.Numero_B}' " +
                    $"AND a.MC_____CODIGO____PF_____B = {parameters.Anio} " +
                    $"AND a.MC_____BASE______B <> 0;";

                var response = await _context.EstadoCuentasXPagarDetalle.FromSqlRaw(sql).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<EstadoCuentasXPagarDetalle>> EstadoCuentasXPorPagarDetalleApoteosys_Reporte(QuerySearchEstadoCuentasXPagarDetalle parameters)
        {
            try
            {
                string sql = $"SELECT " +
                            $"'' Empresa, " +
                            $"'' Tipo_Documento, " +
                            $"'' Fecha, " +
                            $"'' Numero, " +
                            $"'' Consecutivo, " +
                            $"'' Anio, " +
                            $"'' Periodo, " +
                            $"'' Codigo_Plan_Contable, " +
                            $"MC_____CODIGO____CPC____B Cuenta_Contable, " +
                            $"p.CPC____NOMBRE____B Nombre_Cuenta_Contable," +
                            $"'' Centro_Costo, " +
                            $"'' Nombre_Centro_Costo, " +
                            $"'' Codigo_Tercero, " +
                            $"'' Nombre_Tercero, " +
                            $"'' Documento_Alterno, " +
                            $"'' Numero_Documento_Alterno, " +
                            $"'' Fecha_Emision, " +
                            $"'' Fecha_Vencimiento, " +
                            $"'' Iva_Mayor_Valor, " +
                            $"LTRIM(TO_CHAR(SUM(MC_____BASE______B), '$999,999,999,999,999.00')) Base_Retencion, " +
                            $"LTRIM(TO_CHAR(SUM(MC_____DEBMONLOC_B), '$999,999,999,999,999.00')) Debito, " +
                            $"LTRIM(TO_CHAR(SUM(MC_____CREMONLOC_B), '$999,999,999,999,999.00')) Credito, " +
                            $"'' Observaciones " +
                            $"FROM APOTEOSYS.MC____ a " +
                            $"INNER JOIN APOTEOSYS.CPC___ p ON a.MC_____CODIGO____CPC____B = p.CPC____CODIGO____B AND p.CPC____CODIGO____PC_____B = 'NIIF' AND p.CPC____CODIGO____B NOT LIKE '2445%' " +
                            $"INNER JOIN APOTEOSYS.CU____ c ON a.MC_____CODIGO____CU_____B = c.CU_____CODIGO____B " +
                            $"INNER JOIN APOTEOSYS.TERCER t ON a.MC_____IDENTIFIC_TERCER_B = t.TERCER_IDENTIFIC_B  " +
                            $"WHERE a.MC_____CODIGO____CONTAB_B = '{parameters.Empresa}' " +
                            $"AND a.MC_____CODIGO____CU_____B IS NOT NULL " +
                            $"AND a.MC_____IDENTIFIC_TERCER_B = '{parameters.NitProveedor}' " +
                            $"AND a.MC_____CODIGO____TD_____B = '{parameters.Codigo_Td}' " +
                            $"AND a.MC_____NUMERO____B = '{parameters.Numero_B}' " +
                            $"AND a.MC_____NUMERO____PERIOD_B = {parameters.Periodo} " +
                            $"AND a.MC_____CODIGO____PF_____B = {parameters.Anio} " +
                            $"AND a.MC_____BASE______B <> 0 " +
                            $"GROUP BY MC_____CODIGO____CPC____B, p.CPC____NOMBRE____B";

                var response = await _context.EstadoCuentasXPagarDetalle.FromSqlRaw(sql).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
        #endregion

        #region Facturas Pagadas
        public async Task<List<EstadoCuentasPagadas>> EstadoCuentasPagadasApoteosys(QuerySearchEstadoCuentasPagadas parameters, List<Empresa> empresasActivas)
        {
            try
            {
                string empresaQuery = string.Empty;
                string numFacturaQuery = string.Empty;

                if (!string.IsNullOrEmpty(parameters.Empresa))
                {
                    empresaQuery = $"AND a.MC_____CODIGO____CONTAB_B = '{parameters.Empresa}' ";
                }
                else
                {
                    // OJOO:::: Falta que sean las empresas a las que el proveedor tiene acceso
                    var abrevEmpresasActivas = empresasActivas.Select(x => string.Concat("'", x.EmpAbreviatura, "'")).Distinct().ToList();
                    empresaQuery = $"AND a.MC_____CODIGO____CONTAB_B IN ({string.Join(",", abrevEmpresasActivas)}) ";
                }

                if (!string.IsNullOrEmpty(parameters.Numero_Documento))
                {
                    numFacturaQuery = $"AND a.MC_____NUMDOCSOP_B = '{parameters.Numero_Documento}' ";
                }

                string sql = $"SELECT " +
                    $"MC_____CODIGO____CONTAB_B Empresa, " +
                    Funciones.GetQueryNombreEmpresa_Apoteosys() +
                    $"MC_____CODIGO____PF_____B Anio, " +
                    $"MC_____NUMERO____PERIOD_B Periodo, " +
                    $"TO_CHAR(MC_____FECHA_____B, 'DD/MM/YYYY') Fecha_Pago, " +
                    $"MC_____IDENTIFIC_TERCER_B Codigo_Tercero, " +
                    $"t.TERCER_NOMBEXTE__B Nombre_Tercero, " +
                    $"MC_____CODIGO____TD_____B Tipo_Documento_Causacion, " +
                    $"MC_____NUMERO____B Numero_Documento_Causacion, " +
                    $"MC_____CODIGO____DS_____B Tipo_Documento_Alterno, " +
                    $"MC_____NUMDOCSOP_B Numero_Documento_Alterno, " +
                    $"MC_____OBSERVACI_B Observaciones, " +
                    $"LTRIM(TO_CHAR(SUM(MC_____DEBMONLOC_B), '$999,999,999,999,999.00')) Debito, " +
                    $"LTRIM(TO_CHAR(SUM(MC_____CREMONLOC_B), '$999,999,999,999,999.00')) Credito " +
                    //$"LTRIM(TO_CHAR(SUM(MC_____DEBMONLOC_B) OVER(PARTITION BY MC_____CODIGO____TD_____B, MC_____NUMERO____B ORDER BY MC_____CODIGO____TD_____B, MC_____NUMERO____B DESC), '$999,999,999,999,999.00')) AS Total_Movimiento " +
                    $"FROM APOTEOSYS.MC____ a " +
                    $"INNER JOIN APOTEOSYS.TERCER t ON a.MC_____IDENTIFIC_TERCER_B = t.TERCER_IDENTIFIC_B " +
                    $"INNER JOIN APOTEOSYS.TCBM__ c ON c.TCBM___IDENTIFIC_TERCER_B = t.TERCER_IDENTIFIC_B AND c.TCBM___INDCUEPRI_B = 'S' " +
                    $"INNER JOIN APOTEOSYS.EF____ b ON b.EF_____CODIGO____B = c.TCBM___CODIGO____EF_____B " +
                    $"INNER JOIN APOTEOSYS.CPC___ p ON a.MC_____CODIGO____CPC____B = p.CPC____CODIGO____B AND p.CPC____CODIGO____PC_____B = 'NIIF' " +
                    $"WHERE MC_____CODIGO____TD_____B LIKE 'ET%' " +
                    empresaQuery +
                    numFacturaQuery +
                    $"AND a.MC_____IDENTIFIC_TERCER_B = {parameters.NitProveedor} " +

                    $"AND a.MC_____CODIGO____CONTAB_B != 'AQSA' " +
                    $"AND a.MC_____CODIGO____CU_____B IS NOT NULL " +
                    $"AND a.MC_____CODIGO____TD_____B != 'CA' " +

                    $"AND a.MC_____FECHA_____B BETWEEN TO_DATE('{parameters.Fecha_Inicial}','dd/mm/yyyy')  " +
                    $"AND TO_DATE('{parameters.Fecha_Final}','dd/mm/yyyy')   " +
                    $"AND a.MC_____DEBMONLOC_B > 0 " +

                    $"GROUP BY " +
                    $"MC_____CODIGO____CONTAB_B, " +
                    $"MC_____CODIGO____PF_____B, " +
                    $"MC_____NUMERO____PERIOD_B, " +
                    $"MC_____FECHA_____B, " +
                    $"MC_____IDENTIFIC_TERCER_B, " +
                    $"t.TERCER_NOMBEXTE__B, " +
                    $"MC_____CODIGO____TD_____B, " +
                    $"MC_____NUMERO____B, " +
                    $"MC_____CODIGO____DS_____B, " +
                    $"MC_____NUMDOCSOP_B, " +
                    $"MC_____OBSERVACI_B " +

                    $"ORDER BY MC_____FECHA_____B DESC, MC_____CODIGO____TD_____B ASC, MC_____NUMDOCSOP_B DESC ";

                var response = await _context.EstadoCuentasPagadas.FromSqlRaw(sql).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<EstadoCuentasPagadasDetalle>> EstadoCuentasPagadasDetalleApoteosys(QuerySearchEstadoCuentasPagadasDetalle parameters)
        {
            try
            {
                string sql = $"SELECT " +
                            $"MC_____CODIGO____CONTAB_B Empresa, " +
                            Funciones.GetQueryNombreEmpresa_Apoteosys() +
                            $"MC_____CODIGO____PF_____B Anio, " +
                            $"MC_____NUMERO____PERIOD_B Periodo, " +
                            $"TO_CHAR(MC_____FECHA_____B, 'DD/MM/YYYY') Fecha, " +
                            $"MC_____IDENTIFIC_TERCER_B Codigo_Tercero, " +
                            $"t.TERCER_NOMBEXTE__B Nombre_Tercero, " +
                            $"c.TCBM___NUMECUEN__B Cuenta_Consignacion, " +
                            $"b.EF_____NOMBRE____B Nombre_Banco, " +
                            $"CASE c.TCBM___TIPOCUEN__B " +
                            $"WHEN 1 THEN 'Corriente' " +
                            $"WHEN 2 THEN 'Ahorros' " +
                            $"END AS Tipo_Cuenta_Banco, " +
                            $"MC_____CODIGO____CPC____B Cuenta, " +
                            $"MC_____CODIGO____DS_____B||' '||MC_____NUMDOCSOP_B Nombre_Cuenta, " +
                            $"MC_____CODIGO____TD_____B Tipo_Documento_Causacion, " +
                            $"MC_____NUMERO____B Numero_Documento_Causacion, " +
                            $"MC_____CODIGO____DS_____B Tipo_Documento_Alterno, " +
                            $"MC_____NUMDOCSOP_B Numero_Documento_Alterno, " +
                            $"MC_____OBSERVACI_B Observaciones, " +
                            $"LTRIM(TO_CHAR(MC_____DEBMONLOC_B, '$999,999,999,999,999.00')) Debito, " +
                            $"LTRIM(TO_CHAR(MC_____CREMONLOC_B, '$999,999,999,999,999.00')) Credito, " +
                            $"LTRIM(TO_CHAR(sum(MC_____DEBMONLOC_B), '$999,999,999,999,999.00')) TotalCredito, " +
                            $"GROUPING(MC_____CREMONLOC_B) IsTotal " +
                            $"FROM APOTEOSYS.MC____ a " +
                            $"INNER JOIN APOTEOSYS.TERCER t ON t.TERCER_IDENTIFIC_B = a.MC_____IDENTIFIC_TERCER_B " +
                            $"INNER JOIN APOTEOSYS.TCBM__ c ON c.TCBM___IDENTIFIC_TERCER_B = t.TERCER_IDENTIFIC_B AND c.TCBM___INDCUEPRI_B = 'S' " +
                            $"INNER JOIN APOTEOSYS.EF____ b ON b.EF_____CODIGO____B = c.TCBM___CODIGO____EF_____B " +
                            $"INNER JOIN APOTEOSYS.CPC___ p ON p.CPC____CODIGO____B = a.MC_____CODIGO____CPC____B AND p.CPC____CODIGO____PC_____B = 'NIIF' " +
                            $"WHERE a.MC_____CODIGO____CONTAB_B = '{parameters.Empresa}' " +
                            $"AND MC_____CODIGO____TD_____B = '{parameters.Tipo_Documento_Causacion}' " +
                            $"AND MC_____NUMERO____B = '{parameters.Numero_Documento_Causacion}' " +
                            $"AND a.MC_____IDENTIFIC_TERCER_B = '{parameters.NitProveedor}' " +
                            $"AND a.MC_____NUMDOCSOP_B = '{parameters.Numero_Documento_Alterno}' " +
                            $"AND a.MC_____CODIGO____CONTAB_B != 'AQSA' " +
                            $"AND a.MC_____CODIGO____CU_____B IS NOT NULL " +
                            $"AND a.MC_____CODIGO____TD_____B != 'CA' " +
                            //$"AND a.MC_____CODIGO____CPC____B LIKE '1%' " +
                            $"GROUP BY " +
                            $"GROUPING SETS ( " +
                            $"(MC_____CODIGO____CONTAB_B), " +
                            $"(MC_____CODIGO____PF_____B, MC_____NUMERO____PERIOD_B, MC_____FECHA_____B, MC_____IDENTIFIC_TERCER_B, t.TERCER_NOMBEXTE__B, c.TCBM___NUMECUEN__B, b.EF_____NOMBRE____B, c.TCBM___TIPOCUEN__B, " +
                            $"MC_____CODIGO____CPC____B,TERCER_NUMECUEN__B, MC_____CODIGO____DS_____B,MC_____NUMDOCSOP_B, MC_____CODIGO____TD_____B, MC_____NUMERO____B, MC_____DEBMONLOC_B, MC_____CREMONLOC_B, MC_____OBSERVACI_B) " +
                            $") " +
                            $"ORDER BY MC_____FECHA_____B, " +
                            $"MC_____CODIGO____TD_____B, " +
                            $"MC_____NUMDOCSOP_B ";

                var response = await _context.EstadoCuentasPagadasDetalle.FromSqlRaw(sql).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<EstadoCuentasPagadasDetalle_Reporte>> EstadoCuentasPagadasDetalleApoteosys_Reporte(QuerySearchEstadoCuentasPagadasDetalle parameters)
        {
            try
            {
                string sql = $"SELECT " +
                            $"MC_____CODIGO____CONTAB_B Empresa, " +
                            Funciones.GetQueryNombreEmpresa_Apoteosys() +
                            $"MC_____CODIGO____PF_____B Anio, " +
                            $"MC_____NUMERO____PERIOD_B Periodo, " +
                            $"TO_CHAR(MC_____FECHA_____B, 'DD/MM/YYYY') Fecha, " +
                            $"MC_____IDENTIFIC_TERCER_B Codigo_Tercero, " +
                            $"t.TERCER_NOMBEXTE__B Nombre_Tercero, " +
                            $"MC_____CODIGO____CPC____B Cuenta, " +
                            $"c.TCBM___NUMECUEN__B Cuenta_Consignacion, " +
                            $"b.EF_____NOMBRE____B Nombre_Banco, " +
                            $"CASE c.TCBM___TIPOCUEN__B " +
                            $"WHEN 1 THEN 'Corriente' " +
                            $"WHEN 2 THEN 'Ahorros' " +
                            $"END AS Tipo_Cuenta_Banco, " +
                            $"MC_____CODIGO____DS_____B||' '||MC_____NUMDOCSOP_B Nombre_Cuenta, " +
                            $"MC_____CODIGO____TD_____B Tipo_Documento_Causacion, " +
                            $"MC_____NUMERO____B Numero_Documento_Causacion, " +
                            $"MC_____CODIGO____DS_____B Tipo_Documento_Alterno, " +
                            $"MC_____NUMDOCSOP_B Numero_Documento_Alterno, " +
                            $"MC_____OBSERVACI_B Observaciones, " +
                            $"LTRIM(TO_CHAR(MC_____DEBMONLOC_B, '$999,999,999,999,999.00')) Debito, " +
                            $"LTRIM(TO_CHAR(MC_____CREMONLOC_B, '$999,999,999,999,999.00')) Credito, " +
                            $"LTRIM(TO_CHAR(sum(MC_____DEBMONLOC_B), '$999,999,999,999,999.00')) TotalDebito, " +
                            $"GROUPING(MC_____DEBMONLOC_B) IsTotal " +
                            $"FROM APOTEOSYS.MC____ a " +
                            $"INNER JOIN APOTEOSYS.TERCER t ON a.MC_____IDENTIFIC_TERCER_B = t.TERCER_IDENTIFIC_B " +
                            $"INNER JOIN APOTEOSYS.TCBM__ c ON c.TCBM___IDENTIFIC_TERCER_B = t.TERCER_IDENTIFIC_B AND c.TCBM___INDCUEPRI_B = 'S' " +
                            $"INNER JOIN APOTEOSYS.EF____ b ON b.EF_____CODIGO____B = c.TCBM___CODIGO____EF_____B " +
                            $"INNER JOIN APOTEOSYS.CPC___ p ON a.MC_____CODIGO____CPC____B = p.CPC____CODIGO____B AND p.CPC____CODIGO____PC_____B = 'NIIF' " +
                            $"WHERE a.MC_____CODIGO____CONTAB_B = '{parameters.Empresa}' " +
                            $"AND MC_____CODIGO____TD_____B = '{parameters.Tipo_Documento_Causacion}' " +
                            $"AND MC_____NUMERO____B = '{parameters.Numero_Documento_Causacion}' " +
                            $"AND a.MC_____IDENTIFIC_TERCER_B = '{parameters.NitProveedor}' " +
                            $"AND a.MC_____NUMDOCSOP_B = '{parameters.Numero_Documento_Alterno}' " +
                            $"AND a.MC_____CODIGO____CONTAB_B != 'AQSA' " +
                            $"AND a.MC_____CODIGO____CU_____B IS NOT NULL " +
                            $"AND a.MC_____CODIGO____TD_____B != 'CA' " +
                            $"and a.MC_____DEBMONLOC_B > 0 " +
                            $"GROUP BY " +
                            $"GROUPING SETS ( " +
                            $"(MC_____CODIGO____CONTAB_B), " +
                            $"(MC_____CODIGO____PF_____B,MC_____NUMERO____PERIOD_B,MC_____FECHA_____B,MC_____IDENTIFIC_TERCER_B,t.TERCER_NOMBEXTE__B, MC_____CODIGO____CPC____B,c.TCBM___NUMECUEN__B,b.EF_____NOMBRE____B, " +
                            $"c.TCBM___TIPOCUEN__B,MC_____CODIGO____DS_____B,MC_____NUMDOCSOP_B,MC_____CODIGO____TD_____B,MC_____NUMERO____B,MC_____DEBMONLOC_B,MC_____CREMONLOC_B,MC_____OBSERVACI_B) " +
                            $") " +
                            $"ORDER BY MC_____FECHA_____B, " +
                            $"MC_____CODIGO____TD_____B, " +
                            $"MC_____NUMDOCSOP_B ";

                var response = await _context.EstadoCuentasPagadasDetalle_Reportes.FromSqlRaw(sql).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        #region Descuentos Aplicados en Facturas Pagadas
        public async Task<List<EstadoCuentasXPagar_FactPagas_Maestro>> EstadoCuentasXPorPagar_FactPagas_Maestro(QuerySearchEstadoCuentasXPagar_FactPagas parameters)
        {
            try
            {
                string sql = $"SELECT " +
                            $"MC_____CODIGO____TD_____B Tipo_Documento_Causacion, " +
                            $"MC_____NUMERO____B Numero_Documento_Causacion " +
                            $"FROM APOTEOSYS.MC____ a " +
                            $"INNER JOIN APOTEOSYS.TERCER t ON a.MC_____IDENTIFIC_TERCER_B = t.TERCER_IDENTIFIC_B " +
                            $"INNER JOIN APOTEOSYS.CPC___ p ON a.MC_____CODIGO____CPC____B = p.CPC____CODIGO____B AND p.CPC____CODIGO____PC_____B = 'NIIF' " +
                            $"WHERE a.MC_____CODIGO____CONTAB_B = '{parameters.Empresa}' " +
                            $"AND MC_____CODIGO____TD_____B LIKE 'F%' " +
                            $"AND a.MC_____IDENTIFIC_TERCER_B = {parameters.NitProveedor} " +
                            $"AND a.MC_____CODIGO____CONTAB_B != 'AQSA' " +
                            $"AND a.MC_____CODIGO____CU_____B IS NOT NULL " +
                            $"AND a.MC_____CODIGO____TD_____B != 'CA' " +
                            $"AND a.MC_____NUMDOCSOP_B = '{parameters.Numero_Documento_Alterno}' " +
                            $"AND ROWNUM <= 1";

                var response = await _context.EstadoCuentasXPagar_FactPagas_Maestro.FromSqlRaw(sql).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<EstadoCuentasXPagar_FactPagas_Detalle>> EstadoCuentasXPorPagar_FactPagas_Detalle(QuerySearchEstadoCuentasXPagar_FactPagas parameters)
        {
            try
            {
                string sql = $"SELECT " +
                            $"MC_____CODIGO____CONTAB_B Empresa, " +
                            Funciones.GetQueryNombreEmpresa_Apoteosys() +
                            $"MC_____CODIGO____PF_____B Anio, " +
                            $"MC_____NUMERO____PERIOD_B Periodo, " +
                            $"TO_CHAR(MC_____FECHA_____B, 'DD/MM/YYYY') Fecha, " +
                            $"TO_CHAR(MC_____FECHVENC__B, 'DD/MM/YYYY') Fecha_Vence, " +
                            $"MC_____IDENTIFIC_TERCER_B Codigo_Tercero, " +
                            $"t.TERCER_NOMBEXTE__B Nombre_Tercero, " +
                            $"MC_____CODIGO____CPC____B Cuenta, " +
                            $"p.CPC____NOMBRE____B Nombre_Cuenta_Con, " +
                            $"c.TCBM___NUMECUEN__B Cuenta_Consignacion, " +
                            $"b.EF_____NOMBRE____B Nombre_Banco,             " +
                            $"CASE c.TCBM___TIPOCUEN__B " +
                            $"WHEN 1 THEN 'Corriente' " +
                            $"WHEN 2 THEN 'Ahorros' " +
                            $"END AS Tipo_Cuenta_Banco,	 " +
                            $"MC_____CODIGO____DS_____B||' '||MC_____NUMDOCSOP_B Nombre_Cuenta, " +
                            $"MC_____CODIGO____TD_____B Tipo_Documento_Causacion, " +
                            $"MC_____NUMERO____B Numero_Documento_Causacion, " +
                            $"MC_____CODIGO____DS_____B Tipo_Documento_Alterno, " +
                            $"MC_____NUMDOCSOP_B Numero_Documento_Alterno, " +
                            $"MC_____OBSERVACI_B Observaciones, " +
                            Funciones.GetQueryBaseRetencion() +
                            $"LTRIM(TO_CHAR (sum(a.MC_____DEBMONLOC_B),'$999,999,999,999,999.00')) Debito, " +
                            $"LTRIM(TO_CHAR (sum(MC_____CREMONLOC_B)-sum(MC_____DEBMONLOC_B),'$999,999,999,999,999.00')) Credito " +
                            $"FROM APOTEOSYS.MC____ a " +
                            $"INNER JOIN APOTEOSYS.TERCER t ON a.MC_____IDENTIFIC_TERCER_B = t.TERCER_IDENTIFIC_B " +
                            $"INNER JOIN APOTEOSYS.TCBM__ c ON c.TCBM___IDENTIFIC_TERCER_B = t.TERCER_IDENTIFIC_B AND c.TCBM___INDCUEPRI_B = 'S' " +
                            $"INNER JOIN APOTEOSYS.EF____ b ON b.EF_____CODIGO____B = c.TCBM___CODIGO____EF_____B " +
                            $"INNER JOIN APOTEOSYS.CPC___ p ON a.MC_____CODIGO____CPC____B = p.CPC____CODIGO____B AND p.CPC____CODIGO____PC_____B = 'NIIF' " +
                            $"WHERE a.MC_____CODIGO____CONTAB_B = '{parameters.Empresa}' " +
                            $"AND MC_____CODIGO____TD_____B LIKE '{parameters.Tipo_Documento_Causacion}' " +
                            $"AND a.MC_____IDENTIFIC_TERCER_B = {parameters.NitProveedor} " +
                            $"AND a.MC_____CODIGO____CONTAB_B != 'AQSA' " +
                            $"AND a.MC_____CODIGO____CU_____B IS NOT NULL " +
                            $"AND a.MC_____CODIGO____TD_____B != 'CA' " +
                            $"AND a.MC_____NUMERO____B  = {parameters.Numero_Documento_Causacion} " +
                            $"AND a.MC_____BASE______B > 0 " +

                            $"GROUP BY MC_____CODIGO____CONTAB_B, MC_____CODIGO____PF_____B, MC_____NUMERO____PERIOD_B, MC_____FECHA_____B, MC_____FECHVENC__B, MC_____IDENTIFIC_TERCER_B, t.TERCER_NOMBEXTE__B, MC_____CODIGO____CPC____B, " +
                            $"p.CPC____NOMBRE____B, c.TCBM___NUMECUEN__B, b.EF_____NOMBRE____B, c.TCBM___TIPOCUEN__B, MC_____CODIGO____DS_____B, MC_____NUMDOCSOP_B, MC_____CODIGO____TD_____B, MC_____NUMERO____B, MC_____OBSERVACI_B " +

                            $"ORDER BY MC_____FECHA_____B, " +
                            $"MC_____CODIGO____TD_____B, " +
                            $"MC_____NUMDOCSOP_B";

                var response = await _context.EstadoCuentasXPagar_FactPagas_Detalle.FromSqlRaw(sql).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
        #endregion

        #region Consultas desde SP SQL Server
        public async Task<List<FacturasPagas_SQL>> GetFacturasPagadas_SpSQL(QuerySearchFactPagas_SQL parameters)
        {
            try
            {
                string sql = $"SELECT " +
                            $"'P' TIPO_MOVIMIENTO,  " +
                            $"MC_____CODIGO____CONTAB_B Empresa, " +
                            Funciones.GetQueryNombreEmpresa_Apoteosys() +
                            $"MC_____CODIGO____PF_____B Anio, " +
                            $"MC_____NUMERO____PERIOD_B Periodo, " +
                            $"TO_CHAR(MC_____FECHA_____B, 'DD/MM/YYYY') Fecha, " +
                            $"MC_____IDENTIFIC_TERCER_B Codigo_Tercero, " +
                            $"t.TERCER_NOMBEXTE__B Nombre_Tercero, " +
                            $"MC_____CODIGO____CPC____B Cuenta, " +
                            $"p.CPC____NOMBRE____B Nombre_cuenta_con, " +
                            $"c.TCBM___NUMECUEN__B Cuenta_Consignacion, " +
                            $"b.EF_____NOMBRE____B Nombre_Banco, " +
                            $"CASE c.TCBM___TIPOCUEN__B " +
                            $"WHEN 1 THEN 'Corriente' " +
                            $"WHEN 2 THEN 'Ahorros' " +
                            $"END AS Tipo_Cuenta_Banco, " +
                            $"MC_____CODIGO____DS_____B||' '||MC_____NUMDOCSOP_B Nombre_Cuenta, " +
                            $"MC_____CODIGO____TD_____B Tipo_Documento_Causacion, " +
                            $"MC_____NUMERO____B Numero_Documento_Causacion, " +
                            $"MC_____CODIGO____DS_____B Tipo_Documento_Alterno, " +
                            $"MC_____NUMDOCSOP_B Numero_Documento_Alterno, " +
                            $"MC_____OBSERVACI_B Observaciones, " +
                            $"MC_____BASE______B Base_Retencion, " +
                            $"MC_____DEBMONLOC_B Debito, " +
                            $"MC_____CREMONLOC_B Credito " +
                            $"FROM APOTEOSYS.MC____ a " +
                            $"INNER JOIN APOTEOSYS.TERCER t ON a.MC_____IDENTIFIC_TERCER_B = t.TERCER_IDENTIFIC_B " +
                            $"INNER JOIN APOTEOSYS.TCBM__ c ON c.TCBM___IDENTIFIC_TERCER_B = t.TERCER_IDENTIFIC_B AND c.TCBM___INDCUEPRI_B = 'S' " +
                            $"INNER JOIN APOTEOSYS.EF____ b ON b.EF_____CODIGO____B = c.TCBM___CODIGO____EF_____B " +
                            $"INNER JOIN APOTEOSYS.CPC___ p ON a.MC_____CODIGO____CPC____B = p.CPC____CODIGO____B AND p.CPC____CODIGO____PC_____B = 'NIIF' " +
                            $"WHERE a.MC_____CODIGO____CONTAB_B = '{parameters.Empresa}' " +
                            $"AND MC_____CODIGO____TD_____B LIKE 'ET%' " +
                            $"AND a.MC_____IDENTIFIC_TERCER_B = {parameters.NitProveedor} " +

                            $"AND a.MC_____CODIGO____CONTAB_B != 'AQSA' " +
                            $"AND a.MC_____CODIGO____CU_____B IS NOT NULL " +
                            $"AND a.MC_____CODIGO____TD_____B != 'CA' " +

                            $"AND a.MC_____FECHA_____B BETWEEN TO_DATE('{parameters.Fecha_Inicial}','dd/mm/yyyy')  " +
                            $"AND TO_DATE('{parameters.Fecha_Final}','dd/mm/yyyy')   " +
                            $"AND a.MC_____DEBMONLOC_B > 0 " +
                            $"ORDER BY MC_____FECHA_____B, MC_____CODIGO____TD_____B, MC_____NUMDOCSOP_B ";

                var response = await _context.FacturasPagas_SQL.FromSqlRaw(sql).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<FacturasPagas_SQL>> GetRetencionesFacturasPagadas_SpSQL(QuerySearchFactPagas_SQL parameters)
        {
            try
            {
                string sql = $"SELECT " +
                            $"'C' TIPO_MOVIMIENTO,  " +
                            $"MC_____CODIGO____CONTAB_B Empresa, " +
                            Funciones.GetQueryNombreEmpresa_Apoteosys() +
                            $"MC_____CODIGO____PF_____B Anio, " +
                            $"MC_____NUMERO____PERIOD_B Periodo, " +
                            $"TO_CHAR(MC_____FECHA_____B, 'DD/MM/YYYY') Fecha, " +
                            $"MC_____IDENTIFIC_TERCER_B Codigo_Tercero, " +
                            $"t.TERCER_NOMBEXTE__B Nombre_Tercero, " +
                            $"MC_____CODIGO____CPC____B Cuenta, " +
                            $"p.CPC____NOMBRE____B Nombre_cuenta_con, " +
                            $"c.TCBM___NUMECUEN__B Cuenta_Consignacion, " +
                            $"b.EF_____NOMBRE____B Nombre_Banco, " +
                            $"CASE c.TCBM___TIPOCUEN__B " +
                            $"WHEN 1 THEN 'Corriente' " +
                            $"WHEN 2 THEN 'Ahorros' " +
                            $"END AS Tipo_Cuenta_Banco,	 " +
                            $"MC_____CODIGO____DS_____B||' '||MC_____NUMDOCSOP_B Nombre_Cuenta, " +
                            $"MC_____CODIGO____TD_____B Tipo_Documento_Causacion, " +
                            $"MC_____NUMERO____B Numero_Documento_Causacion, " +
                            $"MC_____CODIGO____DS_____B Tipo_Documento_Alterno, " +
                            $"MC_____NUMDOCSOP_B Numero_Documento_Alterno, " +
                            $"MC_____OBSERVACI_B Observaciones, " +
                            $"MC_____BASE______B Base_Retencion, " +
                            $"MC_____DEBMONLOC_B Debito, " +
                            $"MC_____CREMONLOC_B Credito " +
                            $"FROM APOTEOSYS.MC____ a " +
                            $"INNER JOIN APOTEOSYS.TERCER t ON a.MC_____IDENTIFIC_TERCER_B = t.TERCER_IDENTIFIC_B " +
                            $"INNER JOIN APOTEOSYS.TCBM__ c ON c.TCBM___IDENTIFIC_TERCER_B = t.TERCER_IDENTIFIC_B AND c.TCBM___INDCUEPRI_B = 'S' " +
                            $"INNER JOIN APOTEOSYS.EF____ b ON b.EF_____CODIGO____B = c.TCBM___CODIGO____EF_____B " +
                            $"INNER JOIN APOTEOSYS.CPC___ p ON a.MC_____CODIGO____CPC____B = p.CPC____CODIGO____B AND p.CPC____CODIGO____PC_____B = 'NIIF' " +
                            $"WHERE a.MC_____CODIGO____CONTAB_B = '{parameters.Empresa}' " +
                            $"AND MC_____CODIGO____TD_____B LIKE 'F%' " +
                            $"AND a.MC_____IDENTIFIC_TERCER_B = {parameters.NitProveedor} " +

                            $"AND a.MC_____CODIGO____CONTAB_B != 'AQSA' " +
                            $"AND a.MC_____CODIGO____CU_____B IS NOT NULL " +
                            $"AND a.MC_____CODIGO____TD_____B != 'CA' " +
                            //$"AND a.MC_____BASE______B > 0 " +
                            $"ORDER BY MC_____FECHA_____B, MC_____CODIGO____TD_____B, MC_____NUMDOCSOP_B ";

                var response = await _context.FacturasPagas_SQL.FromSqlRaw(sql).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
        #endregion
        #endregion
        #endregion

        #region Certificados
        public async Task<List<CertificadoRetencionMaestro>> CertificadosRetencionApoteosys(QuerySearchCertificados parameters)
        {
            try
            {
                string sql = $"SELECT " +
                            $"TERCER_IDENTIFIC_B Nit, " +
                            $"TERCER_NOMBEXTE__B Razon_Social, " +
                            $"TERCER_CODIGO____TIT____B Clase " +
                            $"FROM APOTEOSYS.TERCER t " +
                            $"WHERE t.tercer_identific_b='{parameters.NitProveedor}' ";

                var response = await _context.CertificadoRetencionMaestros.FromSqlRaw(sql).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<CertificadoExperiencia>> CertificadosExperienciaApoteosys(QuerySearchCertificados parameters)
        {
            try
            {
                string sql = $"SELECT " +
                            $"MC_____CODIGO____PF_____B Anio, " +
                            $"MC_____IDENTIFIC_TERCER_B Nit_Tercero, " +
                            $"TERCER_NOMBEXTE__B Nombre_Tercero " +
                            $"FROM ( " +
                            $"SELECT ROWNUM, c.* FROM( " +
                            $"SELECT * " +
                            $"FROM ( " +
                            $"SELECT  " +
                            $"a.*, " +
                            $"b.TERCER_NOMBEXTE__B " +
                            $"FROM APOTEOSYS.MC____ a " +
                            $"INNER JOIN APOTEOSYS.TERCER b ON b.TERCER_IDENTIFIC_B = a.MC_____IDENTIFIC_TERCER_B " +
                            $"WHERE a.MC_____IDENTIFIC_TERCER_B = '{parameters.NitProveedor}' " +
                            $"ORDER BY MC_____FECHA_____B ASC " +
                            $") " +
                            $") c " +
                            $") " +
                            $"WHERE ROWNUM < 2;";

                var response = await _context.CertificadoExperiencias.FromSqlRaw(sql).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<CertificadoRetencionFuenteDte>> CertificadosRetencionFuenteApoteosys(QuerySearchCertificados parameters)
        {
            try
            {
                string sql = string.Empty;

                if (parameters.Month == 0)
                {
                    sql = $"SELECT " +
                        $"a.MC_____CODIGO____CONTAB_B Empresa, " +
                        $"a.MC_____CODIGO____CPC____B Cuenta, " +
                        $"p.CPC____NOMBRE____B Nombre_Cuenta, " +
                        //$"LTRIM(TO_CHAR (ROUND((sum(MC_____CREMONLOC_B) / sum(a.MC_____BASE______B) * 100),2),'999.99')) Porcentaje_Practicado, " +
                        Funciones.GetQueryPorcentajePracticado() +
                        Funciones.GetQueryBaseRetencion() +
                        $"LTRIM(TO_CHAR (sum(a.MC_____DEBMONLOC_B),'$999,999,999,999,999.00')) Debito, " +
                        $"LTRIM(TO_CHAR (sum(MC_____CREMONLOC_B)-sum(MC_____DEBMONLOC_B),'$999,999,999,999,999.00')) Credito, " +
                        $"(sum(MC_____CREMONLOC_B)-sum(MC_____DEBMONLOC_B)) CreditoSinFormato, " +
                        $"GROUPING (a.MC_____CODIGO____CPC____B) Total " +
                        $"FROM APOTEOSYS.MC____ a " +
                        $"INNER JOIN APOTEOSYS.CPC___ p ON a.MC_____CODIGO____CPC____B = p.CPC____CODIGO____B AND p.CPC____CODIGO____PC_____B = 'NIIF' " +
                        $"INNER JOIN APOTEOSYS.TERCER t ON a.MC_____IDENTIFIC_TERCER_B = t.TERCER_IDENTIFIC_B " +

                        $"WHERE a.MC_____CODIGO____CONTAB_B != 'AQSA' " +
                        $"AND a.MC_____CODIGO____CU_____B IS NOT NULL " +
                        $"AND a.MC_____CODIGO____TD_____B != 'CA' " +
                        $"AND a.MC_____CODIGO____TD_____B <> 'CIAN'" +
                        $"AND a.MC_____CODIGO____CPC____B LIKE '2436%' " +
                        $"AND a.MC_____CODIGO____CPC____B NOT LIKE '243625%' " +

                        $"AND a.MC_____CODIGO____CONTAB_B = '{parameters.Empresa}' " +
                        $"AND a.MC_____IDENTIFIC_TERCER_B = '{parameters.NitProveedor}' " +

                        $"AND EXTRACT (YEAR FROM a.MC_____FECHA_____B) >= {parameters.Year} " +
                        $"AND EXTRACT (YEAR FROM a.MC_____FECHA_____B) <= {parameters.Year} " +
                        $"AND EXTRACT (MONTH FROM a.MC_____FECHA_____B) >= 1 " +
                        $"AND EXTRACT (MONTH FROM a.MC_____FECHA_____B) <= 12 " +

                        $"GROUP BY " +
                        $"GROUPING SETS( " +
                        $"(a.MC_____CODIGO____CONTAB_B), " +
                        $"(a.MC_____IDENTIFIC_TERCER_B, " +
                        $"EXTRACT (YEAR FROM a.MC_____FECHA_____B), t.TERCER_NOMBEXTE__B, " +
                        $"a.MC_____CODIGO____CPC____B, p.CPC____NOMBRE____B)) ";
                }
                else
                {
                    sql = $"SELECT " +
                        $"a.MC_____CODIGO____CONTAB_B Empresa, " +
                        $"a.MC_____CODIGO____CPC____B Cuenta, " +
                        $"p.CPC____NOMBRE____B Nombre_Cuenta, " +
                        //$"LTRIM(TO_CHAR (ROUND((sum(MC_____CREMONLOC_B) / sum(a.MC_____BASE______B) * 100),2),'999.99')) Porcentaje_Practicado, " +
                        Funciones.GetQueryPorcentajePracticado() +
                        Funciones.GetQueryBaseRetencion() +
                        $"LTRIM(TO_CHAR (sum(a.MC_____DEBMONLOC_B),'$999,999,999,999,999.00')) Debito, " +
                        $"LTRIM(TO_CHAR (sum(MC_____CREMONLOC_B)-sum(MC_____DEBMONLOC_B),'$999,999,999,999,999.00')) Credito, " +
                        $"(sum(MC_____CREMONLOC_B)-sum(MC_____DEBMONLOC_B)) CreditoSinFormato, " +
                        $"GROUPING (a.MC_____CODIGO____CPC____B) Total " +
                        $"FROM APOTEOSYS.MC____ a " +
                        $"INNER JOIN APOTEOSYS.CPC___ p ON a.MC_____CODIGO____CPC____B = p.CPC____CODIGO____B AND p.CPC____CODIGO____PC_____B = 'NIIF' " +
                        $"INNER JOIN APOTEOSYS.TERCER t ON a.MC_____IDENTIFIC_TERCER_B = t.TERCER_IDENTIFIC_B " +

                        $"WHERE a.MC_____CODIGO____CONTAB_B != 'AQSA' " +
                        $"AND a.MC_____CODIGO____CU_____B IS NOT NULL " +
                        $"AND a.MC_____CODIGO____TD_____B != 'CA' " +
                        $"AND a.MC_____CODIGO____TD_____B <> 'CIAN'" +
                        $"AND a.MC_____CODIGO____CPC____B LIKE '2436%' " +
                        $"AND a.MC_____CODIGO____CPC____B NOT LIKE '243625%' " +

                        $"AND a.MC_____CODIGO____CONTAB_B = '{parameters.Empresa}' " +
                        $"AND a.MC_____IDENTIFIC_TERCER_B = '{parameters.NitProveedor}' " +

                        $"AND EXTRACT (YEAR FROM a.MC_____FECHA_____B) >= {parameters.Year} " +
                        $"AND EXTRACT (YEAR FROM a.MC_____FECHA_____B) <= {parameters.Year} " +
                        $"AND EXTRACT (MONTH FROM a.MC_____FECHA_____B) >= {parameters.Month} " +
                        $"AND EXTRACT (MONTH FROM a.MC_____FECHA_____B) <= {parameters.Month} " +

                        $"GROUP BY " +
                        $"GROUPING SETS( " +
                        $"(a.MC_____CODIGO____CONTAB_B), " +
                        $"(a.MC_____IDENTIFIC_TERCER_B, " +
                        $"EXTRACT (YEAR FROM a.MC_____FECHA_____B), t.TERCER_NOMBEXTE__B, " +
                        $"a.MC_____CODIGO____CPC____B, p.CPC____NOMBRE____B)) ";
                }

                var response = await _context.CertificadoRetencionFuenteDtes.FromSqlRaw(sql).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<CertificadoRetencionIvaDte>> CertificadosRetencionIvaApoteosys(QuerySearchCertificados parameters)
        {
            try
            {
                string sql = $"SELECT " +
                            $"a.MC_____CODIGO____CONTAB_B Empresa, " +
                            $"a.MC_____CODIGO____CPC____B Cuenta, " +
                            $"p.CPC____NOMBRE____B Nombre_Cuenta, " +
                            //$"LTRIM(TO_CHAR (ROUND((SUM(MC_____CREMONLOC_B) / SUM(a.MC_____BASE______B) * 100),2),'999.99')) Porcentaje_Practicado, " +
                            Funciones.GetQueryPorcentajePracticado() +
                            //$"LTRIM(TO_CHAR (sum(a.MC_____BASEORIG__B),'$999,999,999,999,999.00')) Base_Origen, " +
                            Funciones.GetQueryBaseOrigen() +
                            Funciones.GetQueryBaseRetencionIva() +
                            $"LTRIM(TO_CHAR (sum(a.MC_____DEBMONLOC_B),'$999,999,999,999,999')) Debito, " +
                            $"LTRIM(TO_CHAR (sum(MC_____CREMONLOC_B)-sum(MC_____DEBMONLOC_B),'$999,999,999,999,999')) Credito, " +
                            $"(sum(MC_____CREMONLOC_B)-sum(MC_____DEBMONLOC_B)) CreditoSinFormato, " +
                            $"GROUPING (a.MC_____CODIGO____CPC____B) Total " +
                            $"FROM APOTEOSYS.MC____ a " +
                            $"INNER JOIN APOTEOSYS.CPC___ p ON a.MC_____CODIGO____CPC____B = p.CPC____CODIGO____B AND p.CPC____CODIGO____PC_____B = 'NIIF' " +
                            $"INNER JOIN APOTEOSYS.TERCER t ON a.MC_____IDENTIFIC_TERCER_B = t.TERCER_IDENTIFIC_B " +

                            $"WHERE a.MC_____CODIGO____CONTAB_B != 'AQSA' " +
                            $"AND a.MC_____CODIGO____CU_____B IS NOT NULL " +
                            $"AND a.MC_____CODIGO____TD_____B != 'CA' " +
                            $"AND a.MC_____CODIGO____TD_____B <> 'CIAN'" +
                            $"AND a.MC_____CODIGO____CPC____B LIKE '243625%' " +

                            $"AND a.MC_____CODIGO____CONTAB_B = '{parameters.Empresa}' " +
                            $"AND a.MC_____IDENTIFIC_TERCER_B = '{parameters.NitProveedor}' " +
                            $"AND EXTRACT (YEAR FROM a.MC_____FECHA_____B) >= {parameters.Year} " +
                            $"AND EXTRACT (YEAR FROM a.MC_____FECHA_____B) <= {parameters.Year} " +
                            $"AND EXTRACT (MONTH FROM a.MC_____FECHA_____B) >= {parameters.Month} " +
                            $"AND EXTRACT (MONTH FROM a.MC_____FECHA_____B) <= {parameters.Month + 1} " +

                            $"GROUP BY " +
                            $"GROUPING SETS( " +
                            $"(a.MC_____CODIGO____CONTAB_B), " +
                            $"(a.MC_____IDENTIFIC_TERCER_B, " +
                            $"EXTRACT (YEAR FROM a.MC_____FECHA_____B), t.TERCER_NOMBEXTE__B, " +
                            $"a.MC_____CODIGO____CPC____B, p.CPC____NOMBRE____B)) ";

                var response = await _context.CertificadoRetencionIvaDtes.FromSqlRaw(sql).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<CertificadoRetencionIcaDte>> CertificadosRetencionIcaApoteosys(QuerySearchCertificadosICA parameters)
        {
            try
            {
                string whereQuery = string.Empty;

                if (parameters.Periodicidad == 1) // Anual
                {
                }
                else if (parameters.Periodicidad == 2) // Mensual
                {
                    whereQuery = $"AND EXTRACT (MONTH FROM a.MC_____FECHA_____B) >= {parameters.Month} " +
                        $"AND EXTRACT (MONTH FROM a.MC_____FECHA_____B) <= {parameters.Month} ";
                }
                else if (parameters.Periodicidad == 3) // Bimestral
                {
                    whereQuery = $"AND EXTRACT (MONTH FROM a.MC_____FECHA_____B) >= {parameters.Month} " +
                        $"AND EXTRACT (MONTH FROM a.MC_____FECHA_____B) <= {parameters.Month + 1} ";
                }

                string sql = $"SELECT " +
                            $"a.MC_____CODIGO____CONTAB_B Empresa, " +
                            $"a.MC_____CODIGO____CPC____B Cuenta, " +
                            $"p.CPC____NOMBRE____B Nombre_Cuenta, " +
                            $"LTRIM(TO_CHAR (ROUND((SUM(MC_____CREMONLOC_B) / SUM(a.MC_____BASE______B) * 1000),1),'999.99')) Porcentaje_Practicado, " +
                            $"LTRIM(TO_CHAR (SUM(a.MC_____BASEORIG__B),'$999,999,999,999,999.00')) Base_Origen, " +
                            Funciones.GetQueryBaseRetencion() +
                            $"LTRIM(TO_CHAR (SUM(a.MC_____DEBMONLOC_B),'$999,999,999,999,999.00')) Debito, " +
                            $"LTRIM(TO_CHAR (sum(MC_____CREMONLOC_B)-sum(MC_____DEBMONLOC_B),'$999,999,999,999,999.00')) Credito, " +
                            $"(sum(MC_____CREMONLOC_B)-sum(MC_____DEBMONLOC_B)) CreditoSinFormato, " +
                            $"GROUPING (a.MC_____CODIGO____CPC____B) Total " +
                            $"FROM APOTEOSYS.MC____ a " +
                            $"INNER JOIN APOTEOSYS.CPC___ p ON a.MC_____CODIGO____CPC____B = p.CPC____CODIGO____B AND p.CPC____CODIGO____PC_____B = 'NIIF' " +
                            $"INNER JOIN APOTEOSYS.TERCER t ON a.MC_____IDENTIFIC_TERCER_B = t.TERCER_IDENTIFIC_B " +
                            $"WHERE a.MC_____CODIGO____CONTAB_B = '{parameters.Empresa}' " +
                            $"AND a.MC_____CODIGO____CU_____B IS NOT NULL " +
                            $"AND a.MC_____CODIGO____TD_____B != 'CA' " +
                            $"AND a.MC_____CODIGO____TD_____B <> 'CIAN'" +

                            $"AND a.MC_____CODIGO____CPC____B LIKE '{parameters.Cuenta}%' " +
                            $"AND a.MC_____IDENTIFIC_TERCER_B = '{parameters.NitProveedor}' " +
                            $"AND EXTRACT (YEAR FROM a.MC_____FECHA_____B) >= {parameters.Year} " +
                            $"AND EXTRACT (YEAR FROM a.MC_____FECHA_____B) <= {parameters.Year} " +
                            whereQuery +
                            $"GROUP BY " +
                            $"GROUPING SETS( " +
                            $"(a.MC_____CODIGO____CONTAB_B), " +
                            $"(a.MC_____IDENTIFIC_TERCER_B, " +
                            $"EXTRACT (YEAR FROM a.MC_____FECHA_____B), t.TERCER_NOMBEXTE__B, " +
                            $"a.MC_____CODIGO____CPC____B, p.CPC____NOMBRE____B)) ";

                var response = await _context.CertificadoRetencionIcaDtes.FromSqlRaw(sql).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<CertificadoRetencionEstampillaBoyacaDte>> CertificadosRetencionEstampillaBoyacaApoteosys(QuerySearchCertificados parameters)
        {
            try
            {
                string sql = string.Empty;

                if (parameters.Month == 0)
                {
                    sql = $"SELECT " +
                        $"a.MC_____CODIGO____CONTAB_B Empresa, " +
                        $"a.MC_____CODIGO____CPC____B Cuenta, " +
                        $"p.CPC____NOMBRE____B Nombre_Cuenta, " +
                        //$"LTRIM(TO_CHAR (ROUND((sum(MC_____CREMONLOC_B) / sum(a.MC_____BASE______B) * 100),2),'999.99')) Porcentaje_Practicado, " +
                        Funciones.GetQueryPorcentajePracticado() +
                        $"LTRIM(TO_CHAR (sum(a.MC_____BASEORIG__B),'$999,999,999,999,999.00')) Base_Origen, " +
                        Funciones.GetQueryBaseRetencion() +
                        $"LTRIM(TO_CHAR (sum(a.MC_____DEBMONLOC_B),'$999,999,999,999,999.00')) Debito, " +
                        $"LTRIM(TO_CHAR (sum(MC_____CREMONLOC_B)-sum(MC_____DEBMONLOC_B),'$999,999,999,999,999.00')) Credito, " +
                        $"(sum(MC_____CREMONLOC_B)-sum(MC_____DEBMONLOC_B)) CreditoSinFormato, " +
                        $"GROUPING (a.MC_____CODIGO____CPC____B) Total " +
                        $"FROM APOTEOSYS.MC____ a " +
                        $"INNER JOIN APOTEOSYS.CPC___ p ON a.MC_____CODIGO____CPC____B = p.CPC____CODIGO____B AND p.CPC____CODIGO____PC_____B = 'NIIF' " +
                        $"INNER JOIN APOTEOSYS.TERCER t ON a.MC_____IDENTIFIC_TERCER_B = t.TERCER_IDENTIFIC_B " +

                        $"WHERE a.MC_____CODIGO____CONTAB_B != 'AQSA' " +
                        $"AND a.MC_____CODIGO____CU_____B IS NOT NULL " +
                        $"AND a.MC_____CODIGO____TD_____B != 'CA' " +
                        $"AND a.MC_____CODIGO____TD_____B <> 'CIAN'" +
                        $"AND a.MC_____CODIGO____CPC____B LIKE '244080%' " +

                        $"AND a.MC_____CODIGO____CONTAB_B = '{parameters.Empresa}' " +
                        $"AND a.MC_____IDENTIFIC_TERCER_B = '{parameters.NitProveedor}' " +
                        $"AND EXTRACT (YEAR FROM a.MC_____FECHA_____B) >= {parameters.Year} " +
                        $"AND EXTRACT (YEAR FROM a.MC_____FECHA_____B) <= {parameters.Year} " +
                        $"AND EXTRACT (MONTH FROM a.MC_____FECHA_____B) >= 1 " +
                        $"AND EXTRACT (MONTH FROM a.MC_____FECHA_____B) <= 12 " +

                        //$"AND EXTRACT (YEAR FROM a.MC_____FECHA_____B) = {parameters.Year} " +
                        //$"AND EXTRACT (MONTH FROM a.MC_____FECHA_____B) = {parameters.Month} " +

                        $"GROUP BY " +
                        $"GROUPING SETS( " +
                        $"(a.MC_____CODIGO____CONTAB_B), " +
                        $"(a.MC_____IDENTIFIC_TERCER_B, " +
                        $"EXTRACT (YEAR FROM a.MC_____FECHA_____B), t.TERCER_NOMBEXTE__B, " +
                        $"a.MC_____CODIGO____CPC____B, p.CPC____NOMBRE____B)) ";
                }
                else
                {
                    sql = $"SELECT " +
                        $"a.MC_____CODIGO____CONTAB_B Empresa, " +
                        $"a.MC_____CODIGO____CPC____B Cuenta, " +
                        $"p.CPC____NOMBRE____B Nombre_Cuenta, " +
                        //$"LTRIM(TO_CHAR (ROUND((sum(MC_____CREMONLOC_B) / sum(a.MC_____BASE______B) * 100),2),'999.99')) Porcentaje_Practicado, " +
                        Funciones.GetQueryPorcentajePracticado() +
                        $"LTRIM(TO_CHAR (sum(a.MC_____BASEORIG__B),'$999,999,999,999,999.00')) Base_Origen, " +
                        Funciones.GetQueryBaseRetencion() +
                        $"LTRIM(TO_CHAR (sum(a.MC_____DEBMONLOC_B),'$999,999,999,999,999.00')) Debito, " +
                        $"LTRIM(TO_CHAR (sum(MC_____CREMONLOC_B)-sum(MC_____DEBMONLOC_B),'$999,999,999,999,999.00')) Credito, " +
                        $"(sum(MC_____CREMONLOC_B)-sum(MC_____DEBMONLOC_B)) CreditoSinFormato, " +
                        $"GROUPING (a.MC_____CODIGO____CPC____B) Total " +
                        $"FROM APOTEOSYS.MC____ a " +
                        $"INNER JOIN APOTEOSYS.CPC___ p ON a.MC_____CODIGO____CPC____B = p.CPC____CODIGO____B AND p.CPC____CODIGO____PC_____B = 'NIIF' " +
                        $"INNER JOIN APOTEOSYS.TERCER t ON a.MC_____IDENTIFIC_TERCER_B = t.TERCER_IDENTIFIC_B " +

                        $"WHERE a.MC_____CODIGO____CONTAB_B != 'AQSA' " +
                        $"AND a.MC_____CODIGO____CU_____B IS NOT NULL " +
                        $"AND a.MC_____CODIGO____TD_____B != 'CA' " +
                        $"AND a.MC_____CODIGO____TD_____B <> 'CIAN'" +
                        $"AND a.MC_____CODIGO____CPC____B LIKE '244080%' " +

                        $"AND a.MC_____CODIGO____CONTAB_B = '{parameters.Empresa}' " +
                        $"AND a.MC_____IDENTIFIC_TERCER_B = '{parameters.NitProveedor}' " +
                        $"AND EXTRACT (YEAR FROM a.MC_____FECHA_____B) >= {parameters.Year} " +
                        $"AND EXTRACT (YEAR FROM a.MC_____FECHA_____B) <= {parameters.Year} " +
                        $"AND EXTRACT (MONTH FROM a.MC_____FECHA_____B) >= {parameters.Month} " +
                        $"AND EXTRACT (MONTH FROM a.MC_____FECHA_____B) <= {parameters.Month} " +

                        $"GROUP BY " +
                        $"GROUPING SETS( " +
                        $"(a.MC_____CODIGO____CONTAB_B), " +
                        $"(a.MC_____IDENTIFIC_TERCER_B, " +
                        $"EXTRACT (YEAR FROM a.MC_____FECHA_____B), t.TERCER_NOMBEXTE__B, " +
                        $"a.MC_____CODIGO____CPC____B, p.CPC____NOMBRE____B)) ";
                }

                var response = await _context.CertificadoRetencionEstampillaBoyacaDtes.FromSqlRaw(sql).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
        #endregion

        #region Requerimientos
        public async Task<List<SolicitudesApoteosys>> GetSolicitudes(QuerySearchSolicitudesApoteosys parameters)
        {
            try
            {
                string filterQuery = string.Empty;

                if (!string.IsNullOrEmpty(parameters.TipoRequerimiento))
                {
                    filterQuery += $"AND a.SCM_TIPO = '{parameters.TipoRequerimiento}' ";
                }

                if (!string.IsNullOrEmpty(parameters.CodigoArticulo))
                {
                    filterQuery += $"AND b.DSC_ATO_ARTICULO = '{parameters.CodigoArticulo}' ";
                }

                if (!string.IsNullOrEmpty(parameters.Descripcion))
                {
                    filterQuery += $"AND LOWER(b.DSC_DESCRIPCION) LIKE LOWER('%{parameters.Descripcion}%') ";
                }

                if (parameters.Empresa != null && parameters.Empresa.Count > 0)
                {
                    var empresasSearch = parameters.Empresa.Select(x => string.Concat("'", x, "'")).ToList();
                    filterQuery += $"AND a.SCM_EMP_EMPRESA IN ({string.Join(",", empresasSearch)}) ";
                }

                if (parameters.Fecha_Inicial.ToString("dd/MM/yyyy") != "01/01/0001" && parameters.Fecha_Final.ToString("dd/MM/yyyy") != "01/01/0001")
                {
                    filterQuery += $"AND (TRUNC(a.SCM_FECHA) BETWEEN TO_DATE('{parameters.Fecha_Inicial:dd/MM/yyyy}', 'DD/MM/YYYY') AND TO_DATE('{parameters.Fecha_Final:dd/MM/yyyy}', 'DD/MM/YYYY')) ";
                }

                string sql = $"SELECT " +
                            $"ROWNUM AS Id, " +
                            $"a.SCM_EMP_EMPRESA AS Scm_Emp_Empresa, " +
                            $"a.SCM_SOLICITUD_COMPRA AS Scm_Solicitud_Compra, " +
                            $"a.SCM_TIPO AS Tipo_Solicitud, " +
                            $"a.SCM_DEP_DEPARTAMENTO AS Scm_Dep_Departamento, " +
                            $"TO_CHAR(a.SCM_FECHA,'DD/MM/YYYY') AS Scm_Fecha_Creacion, " +
                            $"a.SCM_STATUS AS Scm_Status, " +
                            $"a.SCM_OBSERVACIONES AS Scm_Observaciones, " +
                            $"a.SCM_VALOSOLI AS Valor_Solicitud, " +
                            $"b.DSC_LINEA AS Dsc_Linea, " +
                            $"b.DSC_ATO_ARTICULO AS Dsc_Ato_Articulo, " +
                            $"b.DSC_DESCRIPCION AS Dsc_Descripcion, " +
                            $"b.DSC_DESCRIPCION_ALTERNA AS Dsc_Descripcion_Alterna, " +
                            $"b.DSC_CANTIDAD AS Dsc_Cantidad, " +
                            $"b.DSC_CANTIDAD_APROBADA AS Cantidad_Aprobada, " +
                            $"b.DSC_APROBADA AS Dsc_Aprobada, " +
                            $"b.DSC_OBSERVACIONES AS Dsc_Observaciones, " +
                            $"c.ATO_MED_MEDIDA AS Art_Unidad_Medida " +
                            $"FROM APOTEOSYS.COM_SOLICITUD_COMPRA_TB_NX a " +
                            $"INNER JOIN APOTEOSYS.COM_DETALLE_SOL_COMPRA_TB_NX b ON a.SCM_EMP_EMPRESA = b.DSC_SCM_EMP_EMPRESA " +
                            $"AND a.SCM_SOLICITUD_COMPRA = b.DSC_SCM_SOLICITUD_COMPRA  " +
                            $"INNER JOIN APOTEOSYS.INV_ARTICULO_TB_NX c ON a.SCM_EMP_EMPRESA = c.ATO_EMP_EMPRESA " +
                            $"AND b.DSC_ATO_ARTICULO = c.ATO_ARTICULO " +
                            $"LEFT JOIN APOTEOSYS.COM_DET_SOLIC_ORDEN_TB_NX d ON a.SCM_EMP_EMPRESA = d.DSO_DOR_ORD_EMP_EMPRESA " +
                            $"AND a.SCM_SOLICITUD_COMPRA = d.DSO_DSC_SCM_SOLICITUD_COMPRA  " +
                            $"AND b.DSC_LINEA = d.DSO_DSC_LINEA " +
                            $"LEFT JOIN APOTEOSYS.COM_ORDEN_TB_NX e ON a.SCM_EMP_EMPRESA = e.ORD_EMP_EMPRESA " +
                            $"AND d.DSO_DOR_ORD_ORDEN = e.ORD_ORDEN " +
                            $"LEFT JOIN APOTEOSYS.COM_DETALLE_ORDEN_TB_NX f ON a.SCM_EMP_EMPRESA = f.DOR_ORD_EMP_EMPRESA " +
                            $"AND e.ORD_ORDEN = f.DOR_ORD_ORDEN " +
                            $"AND d.DSO_DOR_LINEA = f.DOR_LINEA " +
                            $"WHERE " +
                            $"a.SCM_STATUS = 'AP' " + // -- Solicitud Aprobada
                            $"AND b.DSC_APROBADA = 'S' " + //  -- Articulo Aprobado
                            $"AND b.DSC_AUTORIZADO = 'S' " + // -- Articulo Autorizado
                            $"AND d.DSO_DOR_ORD_ORDEN IS NULL " + // -- Obtenemos los articulos que aún no se les ha generado orden de compra
                            $"{filterQuery}" +
                            $"ORDER BY a.SCM_EMP_EMPRESA ASC, a.SCM_SOLICITUD_COMPRA ASC, b.DSC_LINEA ASC";

                var response = await _context.SolicitudesApoteosys.FromSqlRaw(sql).ToListAsync();
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