using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ASEGREDWebDto
{
    public class Op360ASEGRED_DataCargaInicialWebDto
    {
        public Centros_Costo[] centros_costo { get; set; }
        public Proyectos_Tipo[] proyectos_tipo { get; set; }
        public Centros_Responsable[] centros_responsable { get; set; }
        public Obras_Tipo[] obras_tipo { get; set; }
        public Plane[] planes { get; set; }
        public Pep[] pep { get; set; }
        public Orden_Materiales[] orden_materiales { get; set; }
        public Orden_Mano_Obra[] orden_mano_obra { get; set; }
        public Contratista[] contratista { get; set; }
        public Interventore[] interventores { get; set; }
        public Estado[] estados { get; set; }
    }

    public class Centros_Costo
    {
        public int id_centro_costo { get; set; }
        public string nombre { get; set; }
    }

    public class Proyectos_Tipo
    {
        public int id_proyecto_tipo { get; set; }
        public string nombre { get; set; }
    }

    public class Centros_Responsable
    {
        public int id_centro_responsable { get; set; }
        public string nombre { get; set; }
    }

    public class Obras_Tipo
    {
        public int id_obra_tipo { get; set; }
        public string nombre { get; set; }
    }

    public class Plane
    {
        public int id_plan { get; set; }
        public string nombre { get; set; }
    }

    public class Pep
    {
        public int id_pep { get; set; }
        public string nombre { get; set; }
    }

    public class Orden_Materiales
    {
        public int id_orden { get; set; }
        public string numero_orden { get; set; }
    }

    public class Orden_Mano_Obra
    {
        public int id_orden { get; set; }
        public string numero_orden { get; set; }
    }

    public class Contratista
    {
        public int id_contratista { get; set; }
        public int codigo { get; set; }
        public string nombre_completo { get; set; }
        public string identificacion { get; set; }
    }

    public class Interventore
    {
        public int id_interventor { get; set; }
        public int id_persona { get; set; }
        public string identificacion { get; set; }
        public string nombre_completo { get; set; }
    }

    public class Estado
    {
        public int id_estado { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
    }
}
