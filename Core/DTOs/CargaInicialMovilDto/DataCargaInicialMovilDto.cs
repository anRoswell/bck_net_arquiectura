namespace Core.DTOs.CargaInicialMovilDto
{
    using System.Collections.Generic;

    public class DataCargaInicialMovilDto
	{
        public IList<UsosEnergiaMovilDto> usos_energia { get; set; }
        public IList<ActividadEconomicaMovilDto> actividades_economicas { get; set; }
        public IList<TiposNoVisibilidadMovilDto> tipos_no_visibilidad { get; set; }
        public IList<EstadosPredioMovilDto> estados_predio { get; set; }
        public IList<RelacionTitularMovilDto> relacion_titular { get; set; }
        public IList<AccionesVsMovilDto> acciones_vs { get; set; }
        public IList<AccionesRcsMovilDto> acciones_rcs { get; set; }
        public IList<AccionesRiMovilDto> acciones_ri { get; set; }
        public IList<AccionesMdvMovilDto> acciones_mdv { get; set; }
        public IList<AccionesVmMovilDto> acciones_vm { get; set; }
        public IList<CuestionariosMovilDto> cuestionarios { get; set; }
        public IList<ResultadosMovilDto> resultados { get; set; }
        public IList<TiposOrdenMovilDto> tipos_orden { get; set; }
        public IList<AnomaliaMovilDto> anomalias { get; set; }
        public IList<SubanomaliasMovilDto> subanomalias { get; set; }
        public IList<ObservacionSubanomaliasMovilDto> observacion_subanomalias { get; set; }
        public IList<SubaccionesMovilDto> subacciones { get; set; }
        public IList<ArticulosMovilDto> articulos { get; set; }
    }
}

