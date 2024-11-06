namespace Core.DTOs.RegistrarGestionOrdenMovilDto
{
    using Core.DTOs.CuestionarioInstanciasMovilDto;
    using System.Collections.Generic;

    public class AnomaliasMovilRequestDto
	{
        public int? id_anomalia { get; set; }
        public int? id_subanomalia { get; set; }
        public string observaciones_subanomalia { get; set; }
        public string observaciones { get; set; }
        public IList<SoporteRequestMovilDto> fotos { get; set; }
    }
}