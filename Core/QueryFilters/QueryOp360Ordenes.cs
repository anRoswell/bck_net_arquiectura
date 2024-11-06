using Newtonsoft.Json;
using System;

namespace Core.QueryFilters
{
    public class QueryOp360Ordenes
    {
        public string id_contratista { get; set; }
        //public int? id_contratista_persona { get; set; }
        public string id_zona { get; set; }
        public string id_estado_orden { get; set; }
        public string codigo_estado { get; set; }
        public string suspension { get; set; }
        public int? id_orden { get; set; }

        public string? ServerSide { get; set; }
        public QueryOp360ServerSide ServerSideJson { get; set; }
    }

    public class QueryOp360OrdenesContratistas
    {
        public int? id_contratista { get; set; }
        public string id_contratista_persona { get; set; }
        public string id_zona { get; set; }
        public int? id_estado_orden { get; set; }
        public string codigo_estado { get; set; }
        public string codigo_suspencion { get; set; }
        public int? id_orden { get; set; }

        public string id_persona { get; set; }
        public string id_usuario { get; set; }

        public string? ServerSide { get; set; }
        public QueryOp360ServerSide ServerSideJson { get; set; }
    }

    public class QueryOp360OrdenesDashBoard
    {
        public int? id_contratista { get; set; }
        public string fecha_creacion { get; set; }
        //public int? id_orden { get; set; }

        public int? pageSize { get; set; }
        public int? pageNumber { get; set; }
        public string sortColumn { get; set; }
        public string sortDirection { get; set; }
    }

    public class QueryOp360OrdenesDashBoardContratista
    {
        public int? id_contratista_persona { get; set; }
        public string fecha_creacion { get; set; }
        public int id_contratista { get; set; }

        public int? pageSize { get; set; }
        public int? pageNumber { get; set; }
        public string sortColumn { get; set; }
        public string sortDirection { get; set; }
    }

    public class QueryOp360OrdenesAgrupada
    {
        public int? id_contratista { get; set; }
    }

    public class QueryOp360Orden
    {
        public int id_orden { get; set; }
    }

    public class QueryOp360GetOrdenes
    {
        public string id_orden_string { get; set; }
    }

    public class QueryOp360GetFechaGeoreferenciacion
    {
        public string fecha { get; set; }
        public int id_contratista { get; set; }
        public int? id_contratista_persona { get; set; }
    }

    public class QueryOp360GetFechaGeoreferenciacionContratista
    {
        public string fecha { get; set; }
        public int id_contratista { get; set; }
        public int? id_contratista_persona { get; set; }
    }

    //cascarones
    public class QueryOp360OreportesScr
    {
        public string? ServerSide { get; set; }
        public QueryOp360ServerSide ServerSideJson { get; set; }
    }
}