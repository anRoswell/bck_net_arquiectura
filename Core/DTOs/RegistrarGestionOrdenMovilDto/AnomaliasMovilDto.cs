namespace Core.DTOs.RegistrarGestionOrdenMovilDto
{
    using Core.DTOs.CuestionarioInstanciasMovilDto;
    using System.Collections.Generic;

    public class AnomaliasMovilDto
	{
        public int? id_anomalia { get; set; }
        public int? id_subanomalia { get; set; }
        public string observaciones_subanomalia { get; set; }
        public string observaciones { get; set; }
        public IList<SoporteMovilDto> fotos { get; set; }
    }
}