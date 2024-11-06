namespace Core.QueryFilters
{
    /*
    * fecha: 19/12/2023
    * clave: 5e6ewrt546weasdf _02
    * carlos vargas
    */
    public class QueryOp360Seguridad
    {
        public int? e_id_usuario { get; set; }
    }

    /*
    * fecha: 19/12/2023
    * clave: aaaaaa1234567 _02
    * carlos vargas
    */
    public class JsonRequestPost
    {
        public string Op { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }        
        public int e_id_usuario { get; set; }
    }
}
