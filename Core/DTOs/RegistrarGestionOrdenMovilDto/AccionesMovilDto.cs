namespace Core.DTOs.RegistrarGestionOrdenMovilDto
{
    using System.Collections.Generic;
    using Core.DTOs.CuestionarioInstanciasMovilDto;

    public class AccionesMovilDto
	{
        public string accion { get; set; }
        public string observacion { get; set; }
        public IList<SoporteMovilDto> fotos { get; set; }
        public int? id_accion { get; set; }
        public int? subactividad { get; set; }
        public IList<MaterialesMovilDto> materiales { get; set; }

        /// <summary>
        /// Mapeo del dto.
        /// </summary>
        /// <param name="accion">La accion.</param>
        /// <param name="observacion">La observacion.</param>
        /// <param name="fotos">Las fotos.</param>
        /// <param name="id_accion">El identificador.</param>
        /// <param name="subactividad">La subactividad.</param>
        /// <param name="materiales">Los materiales.</param>
        public AccionesMovilDto(string accion, string observacion, IList<SoporteMovilDto> fotos, int? id_accion, int? subactividad, IList<MaterialesMovilDto> materiales)
        {
            this.accion = accion;
            this.observacion = observacion;
            this.fotos = fotos;
            this.id_accion = id_accion;
            this.subactividad = subactividad;
            this.materiales = materiales;
        }
    }
}

