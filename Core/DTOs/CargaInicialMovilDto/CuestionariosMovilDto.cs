namespace Core.DTOs.CargaInicialMovilDto
{
    using System.Collections.Generic;

    public class CuestionariosMovilDto
    {
		public int id_cuestionario { get; set; }
        public string nombre { get; set; }
        public string codigo_tipo_formulario { get; set; }
        public string descripcion_tipo_formulario { get; set; }
        public IList<PreguntasMovilDto> preguntas { get; set; }
    }

    public class PreguntasMovilDto
    {
        public int id_pregunta { get; set; }
        public string pregunta { get; set; }
        public ConfiguracionMovilDto configuracion { get; set; }
    }

    public class ConfiguracionMovilDto
    {
        public string ind_requerida { get; set; }
        public IList<ValoresMovilDto> valores { get; set; }
        public string codigo_tipo_pregunta { get; set; }
        public string descripcion_tipo_pregunta { get; set; }
    }

    public class ValoresMovilDto
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}

