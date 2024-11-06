using System;
using System.Collections.Generic;

namespace Core.CustomEntities
{
    public class ContratoListado
    {
        public int IdContrato { get; set; }
        public string NitContratista { get; set; }
        public string NombreContratista { get; set; }
        public string Tipo { get; set; }
        public string Numero { get; set; }
        public string Objeto { get; set; }
        public int NumeroMeses { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public DateTime FechaInicioActa { get; set; }
        public int NumeroDiasFinalizacion { get; set; }
        public double Valor { get; set; }
        public string Administrador { get; set; }
        public string Estado { get; set; }
        public string OtrosSies { get; set; }
        public string Polizas { get; set; }
    }
}