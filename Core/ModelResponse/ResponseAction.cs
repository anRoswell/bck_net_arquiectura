namespace Core.ModelResponse
{
    public class ResponseAction
    {
        public bool estado { get; set; }
        public string mensaje { get; set; }
        public int? Id { get; set; }
        public string error { get; set; }
        public int codigo { get; set; }
        public decimal codigo_error { get; set; }
    }
    public class ResponseActionOp360
    {
        public int codigo { get; set; }
        public string mensaje { get; set; }
        public ResponseActionOp360()
        {
                
        }
        public ResponseActionOp360(int codigo, string mensaje)
        {
            this.codigo = codigo;
            this.mensaje = mensaje; 
        }
    }
}
