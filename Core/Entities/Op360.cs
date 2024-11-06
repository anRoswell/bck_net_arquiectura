using Core.CustomEntities;
using Core.DTOs;
using Core.ModelResponse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Core.Entities
{
    public class Op360
    {

    }

    #region MyRegion

    #endregion
    #region Op360Seguridad
    /*
    * fecha: 19/12/2023
    * clave: 5e6ewrt546weasdf _09
    * carlos vargas
    */
    public class Usuarios_Perfiles
    {
        public int Estado { get; set; }
        public Uri Path { get; set; }
        public Uri Url { get; set; }
        public Uri url_origen { get; set; }
        
        public string Mensaje { get; set; }
        public Datos_Usuario Datos { get; set; }
    }

    public class Datos_Usuario
    {
        [Key]
        public int id_usuario { get; set; }
        public int id_persona { get; set; }
        public string nombres_apellidos { get; set; }
        public string identificacion { get; set; }
        //public int? id_contratista { get; set; }
        //public int? id_contratista_persona { get; set; }
        public string tipo_identificacion { get; set; }
        //public string email { get; set; }
        public Zona2[] Zonas { get; set; }
        public Perfil2[] Perfiles { get; set; }
        public Menu2[] Menus { get; set; }
    }

    public class Zona2
    {
        [Key]
        public int id_zona { get; set; }
        public string nombre_zona { get; set; }
    }

    public class Perfil2
    {
        [Key]
        public long id_perfil { get; set; }
        public string nombre_perfil { get; set; }
    }

    public class Menu2
    {
        [Key]
        public int id { get; set; }
        public string label { get; set; }
        public string routerLink { get; set; }
        public string icon { get; set; }
        public int order { get; set; }
        public bool disabled { get; set; }
        public bool clicked { get; set; }
        public bool expanded { get; set; }
        public Itemx[] items { get; set; }
        [NotMapped]
        public string[] permisos { get; set; }
        [NotMapped]
        public int[] perfiles { get; set; }
    }

    public class Itemx
    {
        [Key]
        public int id { get; set; }
        public string label { get; set; }
        public string icon { get; set; }
        public string routerLink { get; set; }
        public bool disabled { get; set; }
        public bool clicked { get; set; }
        [NotMapped]
        public string[] permisos { get; set; }
        [NotMapped]
        public int[] perfiles { get; set; }
        public bool expanded { get; set; }
        public Itemx1[] items { get; set; }
    }

    public class Itemx1
    {
        [Key]
        public int id { get; set; }
        public string label { get; set; }
        public string icon { get; set; }
        public string routerLink { get; set; }
        public bool disabled { get; set; }
        public bool clicked { get; set; }
        [NotMapped]
        public string[] permisos { get; set; }
        [NotMapped]
        public int[] perfiles { get; set; }
    }
    #endregion

    /*
    * fecha: 19/12/2023
    * clave: aaaaORDENESa65sd4f65sdf _07
    * carlos vargas
    */
    #region Op360Ordenes
    public class Aire_Scr_OrdenAreaCentral_ResponseDto
    {
        public Aire_Scr_OrdenAreaCentral[] ordenes { get; set; }
        public Grafica_Asignacion[] grafica_asignacion { get; set; }
        public int[] ArrayIdOrdenesFiltradas { get; set; }        
    }

    public class Aire_Scr_OrdenAreaCentral_Response
    {
        public Aire_Scr_OrdenAreaCentral[] ordenes { get; set; }
        public Grafica_Asignacion[] grafica_asignacion { get; set; }
        public int[] ArrayIdOrdenesFiltradas { get; set; }
        public int? RegistrosTotales { get; set; }
    }

    public class Aire_Scr_Orden_ResponseDto
    {
        public Aire_Scr_Orden[] ordenes { get; set; }
        public Grafica_Asignacion[] grafica_asignacion { get; set; }
        public int[] ArrayIdOrdenesFiltradas { get; set; }
    }

    public class Aire_Scr_OrdenById_Response
    {
        public Aire_Scr_Orden[] orden { get; set; }
    }

    public class Aire_Scr_Orden_DashBoard_AreaCentral_Response
    {
        public Aire_Scr_OrdenConGeorreferencia[] ordenes { get; set; }
        public int? RegistrosTotales { get; set; }
    }

    public class Aire_Scr_Orden_Response
    {
        public Aire_Scr_Orden[] ordenes { get; set; }
        public Grafica_Asignacion[] grafica_asignacion { get; set; }
        public int[] ArrayIdOrdenesFiltradas
        {
            get
            {
                return ordenes.Select(x => x.id_orden).ToArray();
            }
        }        
        public int? RegistrosTotales { get; set; }
    }

    public class Aire_Scr_OrdenAreaCentral
    {
        [Key]
        public string antiguedad { get; set; }
        public DateTime fecha_registro { get; set; }
        public string origen { get; set; }
        public int id_orden { get; set; }
        public string numero_orden { get; set; }
        public string nic { get; set; }
        public string tarifa { get; set; }
        public string codigo_tipo_orden { get; set; }
        public string tipo_orden { get; set; }
        public string deuda { get; set; }
        public string ultima_factura { get; set; }
        public string id_contratista_persona { get; set; }
        public string nombre_contratista_persona { get; set; }
        public string contratista { get; set; }
        public string territorial { get; set; }
        public string zona { get; set; }
        public string Departamento { get; set; }
        public string Municipio { get; set; }
        public string Barrio { get; set; }
        public string direcion { get; set; }
        public string estado_orden { get; set; }
        public DateTime? fecha_asigna_tecnico { get; set; }
        public string tipo_brigada { get; set; }
        public string tipo_suspencion { get; set; }
        public string Comanterio_OS { get; set; }
        public string Fecha_Consulta { get; set; }

        public Uri UrlDescargaActa { get; set; }
    }

    public class Aire_Scr_Orden
    {
        [Key]
        public string antiguedad { get; set; }
        public DateTime fecha_registro { get; set; }
        public string origen { get; set; }
        public int id_orden { get; set; }
        public string numero_orden { get; set; }
        public string nic { get; set; }
        public string tarifa { get; set; }
        public string codigo_tipo_orden { get; set; }
        public string tipo_orden { get; set; }
        public string deuda { get; set; }
        public string ultima_factura { get; set; }
        public string id_contratista_persona { get; set; }
        public string nombre_contratista_persona { get; set; }
        public string contratista { get; set; }
        public string territorial { get; set; }
        public string zona { get; set; }
        public string Departamento { get; set; }
        public string Municipio { get; set; }
        public string Barrio { get; set; }
        public string direcion { get; set; }
        public string estado_orden { get; set; }
        public DateTime? fecha_asigna_tecnico { get; set; }
        public string tipo_brigada { get; set; }
        public int? id_tipo_suspencion { get; set; }
        public string Comanterio_OS { get; set; }
        public string Fecha_Consulta { get; set; }
        public string tipo_suspencion { get; set; }

        public Uri UrlDescargaActa { get; set; }


        //public int id_tipo_orden { get; set; }
        //public int id_estado_orden { get; set; }
        //public int? id_contratista { get; set; }
        //public int id_cliente { get; set; }
        //public string cliente { get; set; }
        //public int? id_territorial { get; set; }
        //public int? id_zona { get; set; }
        //public DateTime fecha_creacion { get; set; }
        //public DateTime? fecha_cierre { get; set; }
        //public string id_usuario_cierre { get; set; }
        //public string usuario_cierre { get; set; }
        //public int id_actividad { get; set; }
        //public string actividad { get; set; }
        //public int? id_contratista_persona { get; set; }
        //public int? id_tipo_trabajo { get; set; }
        //public string tipo_trabajo { get; set; }

    }

    public class Aire_Scr_OrdenConGeorreferencia
    {
        [Key]
        public int id_orden { get; set; }
        public int id_tipo_orden { get; set; }
        public string tipo_orden { get; set; }

        public string numero_orden { get; set; }
        public int id_estado_orden { get; set; }
        public string estado_orden { get; set; }

        public int? id_contratista { get; set; }
        public string contratista { get; set; }

        public int id_cliente { get; set; }
        public string cliente { get; set; }

        public int? id_territorial { get; set; }
        public string territorial { get; set; }

        public int? id_zona { get; set; }
        public string zona { get; set; }

        public string direcion { get; set; }

        public DateTime fecha_creacion { get; set; }
        public DateTime? fecha_cierre { get; set; }

        public string id_usuario_cierre { get; set; }
        public string usuario_cierre { get; set; }

        public int id_actividad { get; set; }
        public string actividad { get; set; }

        public int? id_contratista_persona { get; set; }
        public string nombre_contratista_persona { get; set; }

        public int? id_tipo_trabajo { get; set; }
        public string tipo_trabajo { get; set; }

        public int? id_tipo_suspencion { get; set; }
        public string tipo_suspencion { get; set; }

        public Georreferencia2 georeferencia_cliente { get; set; }
        public DateTime? fecha_ejecucion { get; set; }
        public Georreferencia2 georeferencia_ejecucion { get; set; }
        public int? id_anomalia { get; set; }
        public string descripcion_anomalia { get; set; }
        public string origen { get; set; }
    }

    public class Georreferencia2
    {
        [Key]
        public float? longitud { get; set; }
        public float? latitud { get; set; }
    }

    public class Grafica_Asignacion
    {
        public string asignacion { get; set; }
        public int noregistros { get; set; }
    }

    public class Aire_Scr_Orden_Agrupada_Response
    {
        public Aire_Scr_Orden_Agrupada[] ordenes_agrupadas { get; set; }
        public Grafica_Asignacion[] grafica_asignacion { get; set; }
    }

    public class Aire_Scr_Orden_Agrupada
    {
        public int id_contratista { get; set; }
        public string contratista { get; set; }
        public string identificacion { get; set; }
        public Zona_Response[] zonas { get; set; }
        public int[] zonas_array { 
            get
            {
                return zonas.Select(x => x.id_zona).ToArray();
            }
        }
        public int NoRegistros { get; set; }
    }

    public class Zona_Response
    {
        public int id_contratista { get; set; }
        public int id_zona { get; set; }
        public string Nombre { get; set; }
    }

    

    public class Archivos_Instancia_Response
    {
        public Archivos_Instancia[] archivos_instancia { get; set; }
        public int? RegistrosTotales { get; set; }
    }

    public class Archivos_Instancia
    {
        public int id_archivo_instancia { get; set; }
        public int id_archivo { get; set; }
        public int numero_registros_archivo { get; set; }
        public int numero_registros_procesados { get; set; }
        public int numero_errores { get; set; }
        public string fecha_inicio_cargue { get; set; }
        public string fecha_fin_cargue { get; set; }
        public string duracion { get; set; }
        public int id_usuario_registro { get; set; }
        public string fecha_registro { get; set; }
        public int id_estado_intancia { get; set; }
        public string observaciones { get; set; }
        public string nombre_archivo { get; set; }
        public string pathwebdescarga { get; set; }
    }

    public class Archivos_Instancia_Detalle_Response
    {
        public Archivos_Instancia_Detalle[] archivos_instancia_detalle { get; set; }
        public int? RegistrosTotales { get; set; }
    }

    public class Archivos_Instancia_Detalle
    {
        public int id_archivo_instancia_detalle { get; set; }
        public int id_archivo_instancia { get; set; }
        public int numero_fila { get; set; }
        public string estado { get; set; }
        public string observaciones { get; set; }
        //public int? RegistrosTotales { get; set; }
    }

    public class Aire_Scr_ReporteEjectuados_Response
    {
        public Aire_Scr_OrdenEjecutados[] ordenes { get; set; }
        public int? RegistrosTotales { get; set; }        
    }

    public class Aire_Scr_OrdenEjecutados
    {
        [Key]
        public string id_orden { get; set; }
        public string contratista { get; set; }
        public string territorial { get; set; }
        public string zona { get; set; }
        public string acta { get; set; }
        public DateTime fechaejecucion { get; set; }
        public DateTime fechainicial { get; set; }
        public DateTime fechafinal { get; set; }
        public string numero_orden { get; set; }
        public string? Fecha_Sincronizacion { get; set; }
        public string? Hora_Sincronizacion { get; set; }       


        public string nic { get; set; }
        public string? ciudad { get; set; }
        public string? barrio { get; set; }
        public string? direccion { get; set; }
        public string? tipo_orden { get; set; }
        public string? tipo_proceso { get; set; }
        
        public string? accion { get; set; }
        public string? subaccion { get; set; }
        public string? caracterizacion { get; set; }


        public string? num_factura { get; set; }
        //public string? deuda_act { get; set; }
        public string? deuda_ejec { get; set; }
        public string? tarifa { get; set; }
        public string? tipo_actividad { get; set; }
        public string? actividad { get; set; }
        public string? tipo_suspension { get; set; }
        public string? GPS { get; set; }
        //public string? georreferencia { get; set; }
        public string? vehiculo { get; set; }
        public string? tipo_operativa { get; set; }
        public string? id_contratista_persona { get; set; }
        public string? nombre_contratista_persona { get; set; }
        public string? observacion { get; set; }
        public string? origen { get; set; }


        public Uri UrlDescargaActa { get; set; }
    }

    //cascarones
    public class Aire_Scr_reporte_ejecutados_Response
    {
        public Aire_Scr_OrdenAreaCentral[] ordenes { get; set; }
        public int[] ArrayIdOrdenesFiltradas { get; set; }
        public int? RegistrosTotales { get; set; }
    }

    public class Georreferencia
    {
        public float longitud { get; set; }
        public float latitud { get; set; }
    }
    #endregion
}
