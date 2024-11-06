using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTOs
{
    public class SeguimientosContratoDto
    {
        public int Id { get; set; }
        public int ScoIdSeguimiento { get; set; }
        public int? ScoCodContrato { get; set; }
        public DateTime? ScoFecha { get; set; }
        public string ScoObservacion { get; set; }
        public decimal? ScoPagosEfectuados { get; set; }
        public int? CodArchivo { get; set; }
        public int? KeyFile { get; set; }
        public string UrlArchivo { get; set; }
        public string NombreArchivo { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
        public string ScoTipo { get; set; }

        public int? CodGestor { get; set; }
        public int? CodGestorRiesgo { get; set; }
        public DocReqUploadDto DocumentoAGuardar { get; set; }
    }
}