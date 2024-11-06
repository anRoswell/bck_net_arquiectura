using Core.Enumerations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Core.QueryFilters
{
    /*
    * fecha: 12/01/2024
    * carlos vargas
    */

    public class QueryOp360RegistrarOrden
    {
        public FormOrdenRequest formdata { get; set; }
        public string fileBase64 { get; set; }
    }
        
    public class QueryOp360CargueMasivoValidacion
    {
        public int id_soporte { get; set; }
        public string usuario_registra { get; set; }
        public string nombre_archivo { get; set; }
    }

    public class QueryOp360CargueMasivoData
    {
        public int id_soporte { get; set; }
        public string destinationTable { get; set; }
    }

    public class QueryOp360CargueMasivoValidacionContratistas
    {
        public int id_soporte { get; set; }
        public string usuario_registra { get; set; }
        public string nombre_archivo { get; set; }
        public string ind_areacentral { get; set; }
    }

    public class FormOrdenRequest
    {
        public int? id_tipo_orden { get; set; }
        public int? nic { get; set; }
        public int? nis { get; set; }
        public int? id_estado_servicio { get; set; }
        public int? id_tipo_suspencion { get; set; }
        public int? contratista { get; set; }
        public string comentario1 { get; set; }
        public string comentario2 { get; set; }
        public string num_camp { get; set; }
    }

    public class FormOrdenRegistro
    {
        public string? id_tipo_orden { get; set; }
        public string? nic { get; set; }
        public string? nis { get; set; }
        public string? id_estado_servicio { get; set; }
        public string? id_tipo_suspencion { get; set; }

        public int? id_soporte { get; set; }
        public string? con_errores { get; set; }
        public string? usuario_registra { get; set; }
        public DateTime? fecha_registra { get; set; }
        public string? desc_validacion { get; set; }
        public int Autonumerico { get; set; }
    }

    public class FormdataResponse
    {
        public int id_orden { get; set; }
    }    
}
