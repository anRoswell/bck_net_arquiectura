using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.ModelResponse;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EmpresasRepository : BaseRepository<Empresa>, IEmpresasRepository
    {
        public EmpresasRepository(DbModelContext context) : base(context) { }

        public async Task<List<Empresa>> GetEmpresas()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","3")
                };

                string sql = $"[par].[SpParametrosIniciales] @Operacion = @Operacion";

                var response = await _context.Empresas.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<Empresa>> GetListEmpresas()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","1")
                };

                string sql = $"[par].[Empresas] @Operacion = @Operacion";

                var response = await _context.Empresas.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PutEmpresa(Empresa empresa)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","3"),
                    new SqlParameter("@Emp_NombreEmpresa", empresa.EmpNombreEmpresa),
                    new SqlParameter("@Emp_Direccion", empresa.EmpDireccion),
                    new SqlParameter("@Emp_Telefono", empresa.EmpTelefono),
                    new SqlParameter("@Emp_Abreviatura", empresa.EmpAbreviatura),
                    new SqlParameter("@Emp_Trb_CodTrabajadorRepresentanteLegal", empresa.EmpTrbCodTrabajadorRepresentanteLegal),
                    new SqlParameter("@Emp_Nit", empresa.EmpNit),
                    new SqlParameter("@Emp_Estado", empresa.EmpEstado),     
             };

                string sql = $"[par].[Empresas] @Operacion = @Operacion, @Emp_NombreEmpresa = @Emp_NombreEmpresa, @Emp_Direccion = @Emp_Direccion, @Emp_Telefono = @Emp_Telefono, @Emp_Abreviatura = @Emp_Abreviatura, @Emp_Trb_CodTrabajadorRepresentanteLegal = @Emp_Trb_CodTrabajadorRepresentanteLegal, @Emp_Nit = @Emp_Nit, @Emp_Estado = @Emp_Estado " ;

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
        
        public async Task<List<ResponseAction>> DeleteEmpresa(Empresa empresa)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","3"),
                    new SqlParameter("@Emp_IdEmpresa", empresa.Id),
                };

                string sql = $"[par].[Empresas] @Operacion = @Operacion, @Emp_IdEmpresa = @Emp_IdEmpresa";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PostCrear(Empresa empresa)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","3"),
                    new SqlParameter("@Emp_NombreEmpresa", empresa.EmpNombreEmpresa),
                    new SqlParameter("@Emp_Direccion", empresa.EmpDireccion),
                    new SqlParameter("@Emp_Telefono", empresa.EmpTelefono),
                    new SqlParameter("@Emp_Abreviatura", empresa.EmpAbreviatura),
                    new SqlParameter("@Emp_Trb_CodTrabajadorRepresentanteLegal", empresa.EmpTrbCodTrabajadorRepresentanteLegal),
                    new SqlParameter("@Emp_Nit", empresa.EmpNit),
                    new SqlParameter("@Emp_Estado", empresa.EmpEstado),
             };

                string sql = $"[par].[Empresas] @Operacion = @Operacion, @Emp_NombreEmpresa = @Emp_NombreEmpresa, @Emp_Direccion = @Emp_Direccion, @Emp_Telefono = @Emp_Telefono, @Emp_Abreviatura = @Emp_Abreviatura, @Emp_Trb_CodTrabajadorRepresentanteLegal = @Emp_Trb_CodTrabajadorRepresentanteLegal, @Emp_Nit = @Emp_Nit, @Emp_Estado = @Emp_Estado ";

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
