create or replace PACKAGE                                                   PKG_G_DANIEL_GONZALEZ_TEST2 AS 
-- para trabajo de carmen
    v_tipo_mensaje_seguimiento		aire.gnl_logs.tipo_mensaje%type := aire.pkg_p_generales.v_tipo_mensaje_seguimiento;
	v_tipo_mensaje_error			aire.gnl_logs.tipo_mensaje%type := aire.pkg_p_generales.v_tipo_mensaje_error;
  /* TODO enter package declarations (types, exceptions, methods etc) here */ 
  /*	Procedimiento DUMMI que recibe json y retorna json con info dummi
    
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

END PKG_G_DANIEL_GONZALEZ_TEST2;