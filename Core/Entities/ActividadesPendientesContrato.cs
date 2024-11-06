namespace Core.Entities
{
    public class ActividadesPendientesContrato : BaseEntity
    {
        public int IdEstado { get; set; }
        public int IdContrato { get; set; }
        public int CodUsuario { get; set; }
        public string EmailUsuario { get; set; }
        public string Pendiente { get; set; }
    }
}
