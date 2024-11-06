namespace Core.QueryFilters
{
    public class QuerySearchRequerimientos : BaseQuery
    {
        public int Id { get; set; }
        public int CodProveedor { get; set; }

        public bool IsNotIdUsuario { get; set; }
        //public DateTime FechaInicial { get; set; }
        //public DateTime FechaFinal { get; set; }
        //public int Estado { get; set; }
    }
}
