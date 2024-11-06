namespace Core.DTOs.Gos.Mobile.ProcesarGestionDto
{
    using Core.DTOs.ProcesarGestionDto;
    using System.Collections.Generic;

    public class ProcesarGestionSpDto
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
        public IList<SoporteSpDto> soportes { get; set; }


        public ProcesarGestionSpDto(
            int? idOrden,
            string numeroOrden,
            string indAmbienteRegistra,
            int? idContratistaPersona,
            int? idUsuarioRegistra,
            AnomaliaDto anomalias,
            CausalDTO causal,
            ProgramacionDto programacion,
            ActividadDto actividades,
            IList<SoporteSpDto> soportes)
        {
            id_orden = idOrden;
            numero_orden = numeroOrden;
            ind_ambiente_registra = indAmbienteRegistra;
            id_contratista_persona = idContratistaPersona;
            id_usuario_registra = idUsuarioRegistra;
            this.anomalias = anomalias;
            this.causal = causal;
            this.programacion = programacion;
            this.actividades = actividades;
            this.soportes = soportes;
        }
    }
}

