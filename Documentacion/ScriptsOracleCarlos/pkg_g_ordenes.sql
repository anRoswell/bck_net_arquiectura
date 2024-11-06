create or replace package                pkg_g_ordenes as

    v_tipo_mensaje_seguimiento		aire.gnl_logs.tipo_mensaje%type := aire.pkg_p_generales.v_tipo_mensaje_seguimiento;
	v_tipo_mensaje_error			aire.gnl_logs.tipo_mensaje%type := aire.pkg_p_generales.v_tipo_mensaje_error;

    /*	Procedimiento que registra la orden

    @autor: Pedro Sotter
    @Fecha Creación: 27/01/2024
    @Fecha Ultima Actualización: 04/01/2024

    Parámetros
    e_json_orden	    : Corresponde al json que contiene la estructura con la informacion de la orden
    s_json_respuesta	: Corresponde al json que contiene la estructura con la respuesta del procedimiento

    */
    procedure prc_registrar_orden (
		e_json_orden  		in 	clob,
        s_json_respuesta 	out	clob
	);

    /*	Procedimiento que actualiza la orden

    @autor: Pedro Sotter
    @Fecha Creación: 27/01/2024
    @Fecha Ultima Actualización: 04/01/2024

    Parámetros
    e_json_orden	    : Corresponde al json que contiene la estructura con la informacion de la orden
    s_json_respuesta	: Corresponde al json que contiene la estructura con la respuesta del procedimiento

    */
    procedure prc_actualizar_orden (
        e_json_orden        in  clob,
        s_json_respuesta	out clob
    );


    /*	Procedimiento que actualiza la orden

    @autor: Antonio Molina
    @Fecha Creación: 05/01/2024
    @Fecha Ultima Actualización: 05/01/2024

    Parámetros
    e_parametros	    : Corresponde al json que contiene los parametros de consulta
    s_json_respuesta	: Corresponde al json que contiene la estructura con la respuesta del procedimiento

    */
	procedure prc_consultar_ordenes_asignadas_tecnico(
		e_parametros 	 in 	clob,
		s_json_respuesta out 	clob
	);

    procedure prc_registrar_filtro(
        e_json    in  clob,
        s_json    out clob
    );

    procedure prc_actualizar_filtro(
        e_json    in  clob,
        s_json    out clob
    );

    procedure prc_eliminar_filtro(
        e_json    in  clob,
        s_json    out clob
    );

	procedure prc_consultar_filtros(
        s_json    out clob
	);

    /*	Procedimiento que gestiona la orden

    @autor: Pedro Sotter
    @Fecha Creación: 18/01/2024
    @Fecha Ultima Actualización: 18/01/2024

    Parámetros
    e_json_orden	    : Corresponde al json que contiene la estructura con la informacion de la orden
    s_json_respuesta	: Corresponde al json que contiene la estructura con la respuesta del procedimiento

    */
    procedure prc_registrar_orden_gestion (
        e_json_orden        in  clob,
        s_json_respuesta	out clob
    );


    procedure prc_cerrar_orden_osf(
        e_xml_orden         in  clob,
        s_respuesta         out aire.tip_respuesta
    );

    procedure prc_cerrar_orden(
        e_id_orden_gestion	in 	aire.ord_ordenes_gestion.id_orden_gestion%type,
        s_respuesta			out	aire.tip_respuesta
    );
    
    --//-->--------------------------- INICIO PKG_G_CARLOS_VARGAS_TEST Procedimientos Carlos Vargas
    
    /*	Procedimiento que retorna json con los parametros iniciales para ORDENES PERFIL AREA CENTRAL PERSONAL DE AIRE
        Migrado a pkg_g_ordenes 31/01/2024
    
        @autor: Carlos Vargas
        @Fecha Creación: 10/01/2024
        @Fecha Ultima Actualización: 11/01/2024
    
        Parámetros
        s_json   : corresponde al json de respuesta
    */
    procedure prc_parametros_iniciales_areacentral (		        
        s_json 	out	clob
	);
    
    /*	Procedimiento que retorna json con la info del cliente
        Migrado a pkg_g_ordenes 31/01/2024
    
        @autor: Carlos Vargas
        @Fecha Creación: 10/01/2024
        @Fecha Ultima Actualización: 11/01/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
    */
    procedure prc_consultar_cliente_por_nic_nis (		
        e_json  in  clob,
        s_json 	out	clob
	);    
    
    /*	Procedimiento que retorna json con el listado AGRUPADO de ordenes para procesar por parte de area central
        Migrado a pkg_g_ordenes 31/01/2024
    
        @autor: Carlos Vargas
        @Fecha Creación: 12/01/2024
        @Fecha Ultima Actualización: 12/01/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
    */
    procedure prc_consulta_agrupada_ordenes_area_central (		
        e_json  in  clob,
        s_json 	out	clob
	);
    
    /*	Procedimiento de asignacion o desasignación invidual o masiva de contratistas o aliados
        Migrado a pkg_g_ordenes 31/01/2024
    
        @autor: Carlos Vargas
        @Fecha Creación: 12/01/2024
        @Fecha Ultima Actualización: 12/01/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
    */
    procedure prc_gestionar_orden_asignar_contratista (		
        e_json  in  clob,
        s_json 	out	clob
	);
    
    /*	Procedimiento que retorna json con los parametros iniciales para CONTRATISTAS
        Migrado a pkg_g_ordenes 31/01/2024
    
        @autor: Carlos Vargas
        @Fecha Creación: 19/01/2024
        @Fecha Ultima Actualización: 19/01/2024
    
        Parámetros
        s_json   : corresponde al json de respuesta
    */
    procedure prc_parametros_iniciales_contratistas (		        
        e_json 	in	clob,
        s_json 	out	clob
	);
    
     /*	Procedimiento de asignacion o desasignación invidual o masiva de brigadas o tecnicos
        Migrado a pkg_g_ordenes 31/01/2024

    
        @autor: Carlos Vargas
        @Fecha Creación: 21/01/2024
        @Fecha Ultima Actualización: 21/01/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
    */
    procedure prc_gestionar_orden_asignar_brigada (		
        e_json  in  clob,
        s_json 	out	clob
	);
    
    --//-->--------------------------- FIN PKG_G_CARLOS_VARGAS_TEST Procedimientos Carlos Vargas
    
    --//-->--------------------------- INICIO PKG_G_CARLOS_VARGAS_TEST2 Procedimientos Carlos Vargas
    /*	Procedimiento que retorna json con el listado de ordenes para procesar por parte de area central
        ---Migrado a pkg_g_ordenes 31/01/2024
    
        @autor: Carlos Vargas
        @Fecha Creación: 11/01/2024
        @Fecha Ultima Actualización: 11/01/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
    */
    procedure prc_consultar_ordenes_area_central (		
        e_json  in  clob,
        s_json 	out	clob
	);    
        
    /*	Procedimiento que retorna json con el listado de la tabla aire.gnl_archivos_instancia
        ---Migrado a pkg_g_ordenes 31/01/2024
    
        @autor: Carlos Vargas
        @Fecha Creación: 16/01/2024
        @Fecha Ultima Actualización: 16/01/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
    */
    procedure prc_consultar_archivos_instancia (		
        e_json  in  clob,
        s_json 	out	clob
	);
    
    /*	Procedimiento que retorna json con el listado de la tabla aire.gnl_archivos_instancia_detalle
        ---Migrado a pkg_g_ordenes 31/01/2024
    
        @autor: Carlos Vargas
        @Fecha Creación: 16/01/2024
        @Fecha Ultima Actualización: 16/01/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
    */
    procedure prc_consultar_archivos_instancia_detalle (		
        e_json  in  clob,
        s_json 	out	clob
	);    
    
    /*	Procedimiento de desasignación invidual o masiva de contratistas o aliados
        ---Migrado a pkg_g_ordenes 31/01/2024
    
        @autor: Carlos Vargas
        @Fecha Creación: 16/01/2024
        @Fecha Ultima Actualización: 16/01/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
    */
    procedure prc_gestionar_orden_des_asignar_contratista (		
        e_json  in  clob,
        s_json 	out	clob
	);
    
    /*	Procedimiento que retorna json con la orden consultada por el id
        ---Migrado a pkg_g_ordenes 31/01/2024
    
        @autor: Carlos Vargas
        @Fecha Creación: 17/01/2024
        @Fecha Ultima Actualización: 17/01/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
    */
    procedure prc_consultar_orden_por_id (		
        e_json  in  clob,
        s_json 	out	clob
	); 
    
    /*	Procedimiento que retorna json con el listado de ordenes para el dashboard de area central
        ---Migrado a pkg_g_ordenes 31/01/2024
    
        @autor: Carlos Vargas
        @Fecha Creación: 17/01/2024
        @Fecha Ultima Actualización: 17/01/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
    */
    procedure prc_consultar_ordenes_dashboard_area_central (		
        e_json  in  clob,
        s_json 	out	clob
	);
    
    /*	Procedimiento de desasignación invidual o masiva de brigadas
        ---Migrado a pkg_g_ordenes 31/01/2024
    
        @autor: Carlos Vargas
        @Fecha Creación: 21/01/2024
        @Fecha Ultima Actualización: 21/01/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
    */
    procedure prc_gestionar_orden_des_asignar_brigada (		
        e_json  in  clob,
        s_json 	out	clob
	);
    
    /*	Procedimiento que retorna json con el listado de ordenes para procesar por parte de los contratistas
        ---Migrado a pkg_g_ordenes 31/01/2024
    
        @autor: Carlos Vargas
        @Fecha Creación: 21/01/2024
        @Fecha Ultima Actualización: 21/01/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
    */
    procedure prc_consultar_ordenes_contratistas (		
        e_json  in  clob,
        s_json 	out	clob
	);
    
    /*	Procedimiento que retorna json con el listado de ordenes para el dashboard de contratistas
        ---Migrado a pkg_g_ordenes 31/01/2024
    
        @autor: Carlos Vargas
        @Fecha Creación: 21/01/2024
        @Fecha Ultima Actualización: 21/01/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
    */
    procedure prc_consultar_ordenes_dashboard_contratistas (		
        e_json  in  clob,
        s_json 	out	clob
	);
    
    /*	Procedimiento que retorna json con la info del contratista consultando por identificacion
        ---Migrado a pkg_g_ordenes 31/01/2024
    
        @autor: Carlos Vargas
        @Fecha Creación: 22/01/2024
        @Fecha Ultima Actualización: 22/01/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
    */
    procedure prc_consultar_contratista_por_identificacion (		
        e_json  in  clob,
        s_json 	out	clob
	);
    
    /*	Procedimiento que retorna json con un resumen general de la tabla de ordenes para control de los fronted
        ---Migrado a pkg_g_ordenes 31/01/2024
    
        @autor: Carlos Vargas
        @Fecha Creación: 29/01/2024
        @Fecha Ultima Actualización: 29/01/2024
    
        Parámetros        
        s_json   : corresponde al json de respuesta
    */
    procedure prc_resumen_global_ordenes (		
        s_json 	out	clob
	); 
    --//-->--------------------------- FIN PKG_G_CARLOS_VARGAS_TEST2 Procedimientos Carlos Vargas
    
    --//-->--------------------------- INICIO PKG_G_DANIEL_GONZALEZ_TEST Procedimientos Daniel
    /*	Procedimiento DUMMI que recibe json y retorna json con info dummi
        ---Migrado a pkg_g_ordenes 31/01/2024
    
        @autor: daniel gonzales
        @Fecha Creación: 12/01/2024
        @Fecha Ultima Actualización: 13/01/2024
    
        Parámetros
        e_json   : corresponde al json de entrada
        s_json   : corresponde al json de salida
    */
    procedure prc_obtener_datos_dummi (		
        e_json  in  clob,
        s_json 	out	clob
	);
    
    /*	Procedimiento Registrar orden que recibe json y retorna json con mensaje con info de la insercion de datos
        ---Migrado a pkg_g_ordenes 31/01/2024
    
        @autor: daniel gonzales
        @Fecha Creación: 17/01/2024
        @Fecha Ultima Actualización: 14/01/2024
    
        Parámetros
        e_json   : corresponde al json de entrada
        s_json   : corresponde al json de salida
    */
    procedure prc_registro_ordenes_masivo_temporal (		
        e_json      in clob,
        s_json      out clob
	);  
    --//-->--------------------------- FIN    PKG_G_DANIEL_GONZALEZ_TEST Procedimientos Daniel
    
    --//-->--------------------------- INICIO PKG_G_DANIEL_GONZALEZ_TEST2 Procedimientos Daniel
    /*	Procedimiento Registrar orden que recibe json y retorna json con mensaje con info de la insercion de datos
        ---Migrado a pkg_g_ordenes 31/01/2024
        
        @autor: daniel gonzales
        @Fecha Creación: 14/01/2024
        @Fecha Ultima Actualización: 14/01/2024
    
        Parámetros
        e_json_orden   : corresponde al json de entrada
        s_json_respuesta   : corresponde al json de salida
    */
    procedure prc_registrar_orden (		
        e_json  		in 	clob,
        s_json 	out	clob
	);
    --//-->--------------------------- FIN    PKG_G_DANIEL_GONZALEZ_TEST2 Procedimientos Daniel
    
    --//-->--------------------------- INICIO PKG_G_DANIEL_GONZALEZ_TEST3 Procedimientos Daniel
    /*	Procedimiento que valida los registros del archivo de excel, genera los logs y carga los registros correctos.
        ---Migrado a pkg_g_ordenes 31/01/2024
        
        @autor: daniel gonzales
        @Fecha Creación: 17/01/2024
        @Fecha Ultima Actualización: 17/01/2024
    
        Parámetros
        e_json   : corresponde al json de entrada
        s_json   : corresponde al json de salida
    */
    procedure prc_registro_ordenes_masivo_final (		
        e_json      in clob,
        s_json      out clob
	);  
    
    /*	Procedimiento que retorna el cargue inicial para el modulo de filtros
        ---Migrado a pkg_g_ordenes 31/01/2024
        
        @autor: antonio molina y carlos vargas
        @Fecha Creación: 26/01/2024
        @Fecha Ultima Actualización: 26/01/2024
    
        Parámetros
        e_json   : corresponde al json de entrada
        s_json   : corresponde al json de salida
    */
    procedure prc_filtros_parametros_Iniciales (		
        s_json      out clob
	);
    --//-->--------------------------- FIN    PKG_G_DANIEL_GONZALEZ_TEST3 Procedimientos Daniel
    
    
end pkg_g_ordenes;