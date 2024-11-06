CREATE OR REPLACE PACKAGE "AIRE".PKG_G_CARLOS_VARGAS_TEST AS 

  /* TODO enter package declarations (types, exceptions, methods etc) here */ 
    /*	Procedimiento que retorna json con los parametros iniciales para ORDENES PERFIL AREA CENTRAL PERSONAL DE AIRE
    
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
    
    /*Borremeee*/
    procedure prc_registro_ordenes_masivo_temporal_Borreme (		
        e_json      in clob,
        s_json      out clob
	);

END PKG_G_CARLOS_VARGAS_TEST;