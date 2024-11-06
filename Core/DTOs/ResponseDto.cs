using System.Collections.Generic;

namespace Core.DTOs
{
    public class ResponseDto
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; }
        public int TotalRecords { get; set; }
    }

    public class ResponseDto<T> : ResponseDto
    {
        public T Datos { get; set; }
    }

    public class ResponseArrayDto<T> : ResponseDto
    {
        public T[] Datos { get; set; }
    }

    public class ResponsePaginationDto<T> : ResponseDto
    {
        public T Datos { get; set; }
        public Paginacion? paginacion { get; set; }
    }

    public class Paginacion
    {
        public int? total_registros { get; set; }
    }

    public class FormdataResponseMasivo
    {
        public OrdenesArray[] ordenes { get; set; }
    }

    public class OrdenesArray
    {
        public int id_ordenes { get; set; }
    }

    //Generic Test
    public class ActividadEconomicaDto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
    }

    //List Gerenic Test
    public class DatosDto
    {
        public List<ActividadEconomicaDto> actividades_economicas { get; set; }
    }

    //Request Test
    public class RequestDto
    {
        public int pagina { get; set; }
    }

    public class Datos_Dummi
    {
        public Ordenes_Dummi[] ordenes_dummi { get; set; }
    }

    public class Ordenes_Dummi
    {
        public int id_orden { get; set; }
        public int id_tipo_orden { get; set; }
        public string tipo_orden { get; set; }
    }
}

