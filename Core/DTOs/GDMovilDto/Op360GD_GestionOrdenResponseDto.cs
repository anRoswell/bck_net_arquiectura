using Core.DTOs.OrdenesAsignadasTecnicoMovilDto;
using Core.DTOs.RegistrarGestionOrdenMovilDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.GDMovilDto
{
    public class Op360GD_GestionOrdenResponseDto
    {
        public Op360_PreguntaDto pregunta { get; set; }
        public Op360_CausaDto causa { get; set; }
        public IList<Op360_SoportesMovil2GDDto> reglas_de_oro { get; set; }
        public IList<Op360_Materiales_InstalarDto> materiales_instalar { get; set; }
        public IList<Op360_Materiales_RetirarDto> materiales_retirar { get; set; }
        public Op360_Cierre_OrdenDto cierre_orden { get; set; }
        public IList<int> acciones { get; set; }
        public IList<Op360_SoportesMovilGDDto> soportes { get; set; }

        public Op360GD_GestionOrdenResponseDto(/*int id_usuario, */Op360_PreguntaDto pregunta, Op360_CausaDto causa,
                       IList<Op360_SoportesMovil2GDDto> reglas_de_oro,
                       IList<Op360_Materiales_InstalarDto> materiales_instalar,
                       IList<Op360_Materiales_RetirarDto> materiales_retirar,
                       Op360_Cierre_OrdenDto cierre_orden,
                       IList<int> acciones,
                       IList<Op360_SoportesMovilGDDto> soportes)
        {
            //this.id_usuario = id_usuario;
            this.pregunta = pregunta;
            this.causa = causa;
            this.reglas_de_oro = reglas_de_oro;
            this.materiales_instalar = materiales_instalar;
            this.materiales_retirar = materiales_retirar;
            this.cierre_orden = cierre_orden;
            this.acciones = acciones;
            this.soportes = soportes;
        }
    }
}
