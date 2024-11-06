namespace Core.DTOs.ProcesarGestionDto
{
    using System.Collections.Generic;

    public class InicialDto
	{
        public int accion_a_realizar { get; set; }
        public int motivo { get; set; }
        public IList<SoporteDTO> fotos { get; set; }
    }
}

