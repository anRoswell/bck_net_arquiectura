CREATE OR REPLACE PACKAGE "AIRE".PKG_G_CARLOS_VARGAS_TEST2 AS  
    
    /*	Procedimiento que retorna json con el listado de ordenes para procesar por parte de area central
    
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
    
        @autor: Carlos Vargas
        @Fecha Creación: 29/01/2024
        @Fecha Ultima Actualización: 29/01/2024
    
        Parámetros        
        s_json   : corresponde al json de respuesta
    */
    procedure prc_resumen_global_ordenes (		
        s_json 	out	clob
	); 
    
    /*	Procedimiento que retorna json con la trazabilidad de una o varias ordenes de trabajo
    
        @autor: Carlos Vargas
        @Fecha Creación: 02/02/2024
        @Fecha Ultima Actualización: 02/02/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
    */
    procedure prc_consultar_trazabilidad_ordenes (		
        e_json  in  clob,
        s_json 	out	clob
	);  
    
    /*	Procedimiento que retorna json con la información de georeferenciacion para area central
    
        @autor: Carlos Vargas
        @Fecha Creación: 03/02/2024
        @Fecha Ultima Actualización: 03/02/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
    */
    procedure prc_consultar_georreferencia_areacentral (		
        e_json  in  clob,
        s_json 	out	clob
	);  
    
    /*	Procedimiento que retorna json con la información de georeferenciacion para area central
    
        @autor: Carlos Vargas
        @Fecha Creación: 03/02/2024
        @Fecha Ultima Actualización: 03/02/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
    */
    procedure prc_consultar_georreferencia_contratista_persona (		
        e_json  in  clob,
        s_json 	out	clob
	);  
    
END PKG_G_CARLOS_VARGAS_TEST2;