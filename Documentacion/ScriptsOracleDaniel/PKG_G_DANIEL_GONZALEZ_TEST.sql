create or replace PACKAGE                                                                                 PKG_G_DANIEL_GONZALEZ_TEST AS 

    v_tipo_mensaje_seguimiento		aire.gnl_logs.tipo_mensaje%type := aire.pkg_p_generales.v_tipo_mensaje_seguimiento;
	v_tipo_mensaje_error			aire.gnl_logs.tipo_mensaje%type := aire.pkg_p_generales.v_tipo_mensaje_error;
  /* TODO enter package declarations (types, exceptions, methods etc) here */ 
  /*	Procedimiento DUMMI que recibe json y retorna json con info dummi
    
        @autor: daniel gonzales
        @Fecha Creaci�n: 12/01/2024
        @Fecha Ultima Actualizaci�n: 13/01/2024
    
        Par�metros
        e_json   : corresponde al json de entrada
        s_json   : corresponde al json de salida
    */
    procedure prc_obtener_datos_dummi (		
        e_json  in  clob,
        s_json 	out	clob
	);
    
    /*	Procedimiento Registrar orden que recibe json y retorna json con mensaje con info de la insercion de datos
    
        @autor: daniel gonzales
        @Fecha Creaci�n: 14/01/2024
        @Fecha Ultima Actualizaci�n: 14/01/2024
    
        Par�metros
        e_json   : corresponde al json de entrada
        s_json   : corresponde al json de salida
    */
    procedure prc_registrar_orden_BORREMEE (		
        e_json      in clob,
        s_json      out clob
	);
    
    /*	Procedimiento Registrar orden que recibe json y retorna json con mensaje con info de la insercion de datos
    
        @autor: daniel gonzales
        @Fecha Creaci�n: 17/01/2024
        @Fecha Ultima Actualizaci�n: 14/01/2024
    
        Par�metros
        e_json   : corresponde al json de entrada
        s_json   : corresponde al json de salida
    */
    procedure prc_registro_ordenes_masivo_temporal (		
        e_json      in clob,
        s_json      out clob
	);
    
    /*	Procedimiento Registrar orden que recibe json y retorna json con mensaje con info de la insercion de datos
    
        @autor: daniel gonzales
        @Fecha Creaci�n: 17/01/2024
        @Fecha Ultima Actualizaci�n: 14/01/2024
    
        Par�metros
        e_json   : corresponde al json de entrada
        s_json   : corresponde al json de salida
    
    procedure prc_registro_ordenes_masivo_validaciones (		
        e_json      in clob,
        s_json      out clob
	);*/
    
    /*
    procedure prc_prueba (
        e_json in clob,
        s_json out clob
    );*/

    /*	Procedimiento para mostrar el ambiente
    
        @autor: daniel gonzalez
        @Fecha Creación: 31/03/2024
        @Fecha Ultima Actualización: 05/02/2024
    
        Parámetros
        s_json   : corresponde al json de salida
    */
    procedure prc_version_db (
        s_json      out clob
	);

END PKG_G_DANIEL_GONZALEZ_TEST;