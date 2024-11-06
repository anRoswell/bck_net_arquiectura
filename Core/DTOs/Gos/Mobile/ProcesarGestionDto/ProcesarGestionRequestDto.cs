namespace Core.DTOs.ProcesarGestionDto
{
    using System.Collections.Generic;

    public class ProcesarGestionRequestDto
	{
        public int? id_orden { get; set; }
        public string numero_orden { get; set; }
        public string ind_ambiente_registra { get; set; }
        public int? id_contratista_persona { get; set; }
        public int? id_usuario_registra { get; set; }
        public AnomaliaDto anomalias { get; set; }
        public CausalDTO causal { get; set; }
        public ProgramacionDto programacion { get; set; }
        public ActividadDto actividades { get; set; }
        public IList<SoporteDTO> soportes { get; set; }
    }
}