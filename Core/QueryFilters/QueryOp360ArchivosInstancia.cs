using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Core.QueryFilters
{
    /*
    * fecha: 19/12/2023
    * clave: aaaaORDENESa65sd4f65sdf _02
    * carlos vargas
    */
    public class QueryOp360ArchivosInstancia
    {
        public string codigo { get; set; }

        public int? pageSize { get; set; }
        public int? pageNumber { get; set; }
        public string sortColumn { get; set; }
        public string sortDirection { get; set; }
        public int? id_ruta_archivo_servidor { get; set; }

        public string? ServerSide { get; set; }
        public QueryOp360ServerSide ServerSideJson { get; set; }
    }

    public class QueryOp360ArchivosInstanciaDetalle
    {
        [Required]
        public int id_archivo_instancia { get; set; }

        public int? pageSize { get; set; }
        public int? pageNumber { get; set; }
        public string sortColumn { get; set; }
        public string sortDirection { get; set; }

        public string? ServerSide { get; set; }
        public QueryOp360ServerSide ServerSideJson { get; set; }
    }
}
