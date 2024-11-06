namespace Core.DTOs.RegistrarGestionOrdenMovilDto
{
    using System.Collections.Generic;
    using Core.DTOs.OrdenesAsignadasTecnicoMovilDto;

    public class RegistrarGestionOrdenMovilRequestDto
	{
        public string fecha_inicio_ejecucion { get; set; }
        public string fecha_fin_ejecucion { get; set; }
        public int id_orden { get; set; }
        public int id_usuario { get; set; }
        public bool tiene_alguna_anomalia { get; set; }
        public InfoActaMovilDto info_acta { get; set; }
        public InfoServicioMovilDto info_servicio { get; set; }
        public PersonaAtiendeMovilDto persona_atiende { get; set; }
        public TestigoMovilDto testigo { get; set; }
        public AccionVsMovilRequestDto accion_vs { get; set; }
        public AccionVmMovilRequestDto accion_vm { get; set; }
        public ReconexionMovilDto reconexion { get; set; }
        public IrregularidadMovilDto irregularidad { get; set; }
        public MaterialRetiradoMovilDto material_retirado { get; set; }
        public MaterialesInstaladoMovilDto material_instalado { get; set; }
        public AnomaliasMovilRequestDto anomalias { get; set; }
        public IList<CuestionarioRequestDto> cuestionarios { get; set; }
        public GeorreferenciaMovilDto georreferencia { get; set; }
    }
}