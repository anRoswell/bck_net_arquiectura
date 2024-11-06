namespace Core.DTOs.CargaInicialGosDto
{
    using System.Collections.Generic;

    public class ParametrosInicialesResponseDto
	{
        public IList<IdDescripcionDto> Actividades { get; set; }
        public IList<GestionDTO> Gestiones { get; set; }
        public IList<AccionDTO> Acciones { get; set; }
        public IList<IdDescripcionAcccionDto> Temas { get; set; }
        public IList<IdSubTemaDTO> Subtemas { get; set; }
        public IList<PublicoTipoDTO> publicos_tipo { get; set; }
        public IList<PublicoTipoDTO> actores_tipo { get; set; }
        public IList<PublicoTipoDTO> mercados_tipo { get; set; }
        public IList<IdDescripcionDto> Anomalias { get; set; }
        public IList<IdDescripcionDto> Causales { get; set; }
        public IList<EstadosOrdenDto> estados_orden { get; set; }
        public IList<ContratistasPersonaDto> contratistas_persona { get; set; }
        public IList<DepartamentoDto> Departamentos { get; set; }
        public IList<MunicipioDto> Municipios { get; set; }
        public IList<CorregimientosDto> Corregimientos { get; set; }
        public IList<BarrioDto> Barrios { get; set; }
    }
}