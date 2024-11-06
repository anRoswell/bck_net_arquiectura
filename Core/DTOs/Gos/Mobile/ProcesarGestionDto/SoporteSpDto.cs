namespace Core.DTOs.Gos.Mobile.ProcesarGestionDto
{
	public class SoporteSpDto
	{
        public SoporteSpDto(){}

        public string nombre { get; set; }
        public double peso { get; set; }
        public string formato { get; set; }
        public string url { get; set; }
        public string codigo_tipo_soporte { get; set; }

        public SoporteSpDto(string nombre, double peso, string formato, string url, string codigo_tipo_soporte)
        {
            this.nombre = nombre;
            this.peso = peso;
            this.formato = formato;
            this.url = url;
            this.codigo_tipo_soporte = codigo_tipo_soporte;
        }
    }
}