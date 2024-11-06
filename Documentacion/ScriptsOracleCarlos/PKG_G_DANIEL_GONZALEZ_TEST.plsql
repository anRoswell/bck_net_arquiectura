CREATE OR REPLACE PACKAGE "AIRE".PKG_G_DANIEL_GONZALEZ_TEST AS 

    --v_tipo_mensaje_seguimiento		aire.gnl_logs.tipo_mensaje%type := aire.pkg_p_generales.v_tipo_mensaje_seguimiento;
	--v_tipo_mensaje_error			aire.gnl_logs.tipo_mensaje%type := aire.pkg_p_generales.v_tipo_mensaje_error;
  /* TODO enter package declarations (types, exceptions, methods etc) here */ 
    /*	Procedimiento DUMMI que recibe json y retorna json con info dummi
    
        @autor: daniel gonzalez
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
    
        @autor: daniel gonzalez
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
    
    --nuevo
    /*	Procedimiento Registrar orden que recibe json y retorna json con mensaje con info de la insercion de datos
    
        @autor: daniel gonzalez
        @Fecha Creación: 05/02/2024
        @Fecha Ultima Actualización: 05/02/2024
    
        Parámetros
        e_json   : corresponde al json de entrada
        s_json   : corresponde al json de salida
    */
    procedure prc_resgistrar_ordenes_ws_osf (		
        e_json      in clob,
        s_json      out clob
	);

END PKG_G_DANIEL_GONZALEZ_TEST;