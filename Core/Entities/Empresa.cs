using System;

#nullable disable

namespace Core.Entities
{
    public partial class Empresa : BaseEntity
    {
        public string EmpNit { get; set; }
        public string EmpNombreEmpresa { get; set; }
        public string Emp_NombreEmpresaGnr { get; set; }
        public string EmpDireccion { get; set; }
        public string Emp_Ciudad { get; set; }
        public string EmpTelefono { get; set; }
        public string EmpAbreviatura { get; set; }
        public string EmpTrbCodTrabajadorRepresentanteLegal { get; set; }
        public bool Emp_IsBimestralApot { get; set; }
        public string Emp_CuentaApot { get; set; }
        public string Emp_TipoDocAlternoApot { get; set; }
        public string CodArchivo { get; set; }
        public bool? EmpEstado { get; set; }
        public string EmpPrefijoContrato { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }

    public partial class Empresa
    {
        public string EmpNombreEmpresaAbrev { get; set; }
    }
}
