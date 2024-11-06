namespace Core.DTOs.RegistrarGestionOrdenMovilDto
{
    using System.Collections.Generic;
    using Core.DTOs.OrdenesAsignadasTecnicoMovilDto;

    public class RegistrarGestionOrdenMovilSpDto
    {
        public string fecha_inicio_ejecucion { get; set; }
        public string fecha_fin_ejecucion { get; set; }
        public int id_orden { get; set; }
        public bool tiene_alguna_anomalia { get; set; }
        public InfoActaMovilDto info_acta { get; set; }
        public InfoServicioMovilDto info_servicio { get; set; }
        public PersonaAtiendeMovilDto persona_atiende { get; set; }
        public TestigoMovilDto testigo { get; set; }
        public IList<AccionesMovilDto> acciones { get; set; }
        public MaterialesInstaladoMovilDto material_instalado { get; set; }
        public AnomaliasMovilDto anomalias { get; set; }
        public IList<CuestionarioMovilDto> cuestionarios { get; set; }
        public GeorreferenciaMovilDto georreferencia { get; set; }


        /// <summary>
        /// Mapeo de dto.
        /// </summary>
        /// <param name="fechaInicioEjecucion">Fecha de inicio de ejecución.</param>
        /// <param name="fechaFinEjecucion">Fecha de fin de ejecución.</param>
        /// <param name="idOrden">Identificador de la orden.</param>
        /// <param name="infoActa">Información del acta.</param>
        /// <param name="infoServicio">Información del servicio.</param>
        /// <param name="personaAtiende">Persona que atiende.</param>
        /// <param name="testigo">Testigo.</param>
        /// <param name="acciones">Acciones.</param>
        /// <param name="materialInstalado">Materiales instalados.</param>
        /// <param name="anomalias">Anomalías.</param>
        /// <param name="cuestionarios">Cuestionarios.</param>
        public RegistrarGestionOrdenMovilSpDto(
            string fechaInicioEjecucion,
            string fechaFinEjecucion,
            int idOrden,
            bool isAnomalia,
            InfoActaMovilDto infoActa,
            InfoServicioMovilDto infoServicio,
            PersonaAtiendeMovilDto personaAtiende,
            TestigoMovilDto testigo,
            IList<AccionesMovilDto> acciones,
            MaterialesInstaladoMovilDto materialInstalado,
            AnomaliasMovilDto anomalias,
            IList<CuestionarioMovilDto> cuestionarios,
            GeorreferenciaMovilDto georreferencia)
        {
            fecha_inicio_ejecucion = fechaInicioEjecucion;
            fecha_fin_ejecucion = fechaFinEjecucion;
            id_orden = idOrden;
            tiene_alguna_anomalia = isAnomalia;
            info_acta = infoActa;
            info_servicio = infoServicio;
            persona_atiende = personaAtiende;
            this.testigo = testigo;
            this.acciones = acciones;
            material_instalado = materialInstalado;
            this.anomalias = anomalias;
            this.cuestionarios = cuestionarios;
            this.georreferencia = georreferencia;
        }
    }
}