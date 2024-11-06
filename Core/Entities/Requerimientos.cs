using System;

namespace Core.Entities
{
    public partial class Requerimientos : BaseEntity
    {
        public string ReqNombreCentroCosto { get; set; }
        public int ReqCodEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
        public int ReqCodAgencia { get; set; }
        public string ReqDescription { get; set; }
        public string ReqLugarEntrega { get; set; }
        public DateTime ReqCierraOferta { get; set; }
        public DateTime ReqCompraPrevista { get; set; }
        public DateTime ReqFechaEntrega { get; set; }
        public string ReqGarantiasExigidas { get; set; }
        public DateTime? ReqfinOfertaPresentada { get; set; }
        public int ReqReqType { get; set; }
        public int ReqCodGestorCompras { get; set; }
        public bool ReqRequiereContrato { get; set; }
        public int? ReqCodGestorContrato { get; set; }
        public int ReqEstado { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }

        public int CodPrvServ { get; set; }
        public string NombreProdServ { get; set; }
        public int CodProveedor { get; set; }
        public string NombreGestorCompra { get; set; }
        public string NombreGestorContrato { get; set; }
        public int Participo { get; set; } // si es 3, está en construccion
        //public DateTime? FecOfePre { get; set; }
        //public string? Observacion { get; set; }
        //public int? IdReqParticipantes { get; set; }
    }

    public partial class Requerimientos
    {
        public int CantidadParticipantes { get; set; }

        public string? Observacion  { get; set; }

        public DateTime? FecOfePre { get; set; }

        public int? IdReqParticipantes { get; set; }
        public int? ProveedorAdjudicado { get; set; }
        
    }
}
