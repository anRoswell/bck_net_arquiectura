create or replace PACKAGE                     PKG_G_DANIEL_GONZALEZ_TEST3 AS 
    
    /*	Procedimiento que valida los registros del archivo de excel, genera los logs y carga los registros correctos.
    
        @autor: daniel gonzales
        @Fecha Creaci�n: 17/01/2024
        @Fecha Ultima Actualizaci�n: 17/01/2024
    
        Par�metros
        e_json   : corresponde al json de entrada
        s_json   : corresponde al json de salida
    */
    procedure prc_registro_ordenes_masivo_final (		
        e_json      in clob,
        s_json      out clob
	);  
    
    /*	Procedimiento que retorna el cargue inicial para el modulo de filtros
    
        @autor: antonio molina y carlos vargas
        @Fecha Creaci�n: 26/01/2024
        @Fecha Ultima Actualizaci�n: 26/01/2024
    
        Par�metros
        e_json   : corresponde al json de entrada
        s_json   : corresponde al json de salida
    */
    procedure prc_filtros_parametros_Iniciales (		
        s_json      out clob
	);

END PKG_G_DANIEL_GONZALEZ_TEST3;