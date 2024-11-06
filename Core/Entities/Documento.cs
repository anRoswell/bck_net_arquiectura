using System;

#nullable disable

namespace Core.Entities
{
    public  class Documento : BaseEntity
    {
        public int CocCodModuloDocumentos { get; set; }
        public string CocNombreDocumento { get; set; }
        public string CocDescripcion { get; set; }
        public bool? CocEstado { get; set; }
        public bool Cocrequiered { get; set; }
        public int CoclimitLoad { get; set; }
        public bool CocVigencia { get; set; }
        public int CocVigenciaMaxima { get; set; }
        public int Selected { get; set; }
        public DateTime? VigenciaDate { get; set; }
        public int VigenciaSelected { get; set; }
        public string UrlDoc { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
