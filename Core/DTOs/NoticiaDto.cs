using Core.Entities;
using System;

namespace Core.DTOs
{
    public class NoticiaDto : BaseEntity
    {
        public int NotIdNoticias { get; set; }
        public int CodEmpresa { get; set; }
        public string NotTitle { get; set; }
        public string NotDescripcion { get; set; }
        public bool? NotEstado { get; set; }
        public int CodTipoNoticia { get; set; }
        public int CodAlcance { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }

        public string Emp_NombreEmpresa { get; set; }
        public string TnDescripcion { get; set; }
        public string AlDescripcion { get; set; }
        public string UrlPortada { get; set; }
    }
}
