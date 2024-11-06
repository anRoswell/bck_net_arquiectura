create or replace PACKAGE                            "AIRE".PKG_G_CARLOS_VARGAS_TEST10 AS

    v_tipo_mensaje_seguimiento		aire.gnl_logs.tipo_mensaje%type := aire.pkg_p_generales.v_tipo_mensaje_seguimiento;
	v_tipo_mensaje_error			aire.gnl_logs.tipo_mensaje%type := aire.pkg_p_generales.v_tipo_mensaje_error;

  /* 	Procedimiento que retorna un json con informacion del log de legalizacion
    
    @autor: Daniel Gonzalez
    @Fecha Creación: 02/04/2024
    @Fecha Ultima Actualización: 05/04/2024

    Parámetros
    e_json  : Corresponde al json de entrada
    s_json  : Corresponde al json de salida

    */
    procedure prc_consultar_reportes_log_legalizacion (
        e_json  in  clob,
        s_json 	out	clob
	);
    
    /* 	Procedimiento que retorna un json con informacion del log de legalizacion
    
    @autor: Daniel Gonzalez
    @Fecha Creación: 05/04/2024
    @Fecha Ultima Actualización: 05/04/2024

    Parámetros
    e_json  : Corresponde al json de entrada
    s_json  : Corresponde al json de salida

    */
    procedure prc_consultar_reportes_log_legalizacion_contratista (
        e_json  in  clob,
        s_json 	out	clob
	);

END PKG_G_CARLOS_VARGAS_TEST10;
/

create or replace PACKAGE BODY                        "AIRE".PKG_G_CARLOS_VARGAS_TEST10 AS

    procedure prc_consultar_reportes_log_legalizacion(
        e_json  in  clob,
        s_json  out clob
    ) is
        v_json_objeto       json_object_t;
        v_json              clob;
        v_id_log            aire.gnl_logs.id_log%type;
        v_respuesta         aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_carlos_vargas_test10.prc_consultar_reportes_log_legalizacion');
        v_codigo                    aire.gnl_archivos.codigo%type;
        v_id_archivo                aire.gnl_archivos.id_archivo%type;
        
        v_rutaweb                   VARCHAR2(255 BYTE);
        
        pageNumber                  NUMBER;
        pageSize                    NUMBER;
        sortColumn                  VARCHAR(100 BYTE);
        sortDirection               VARCHAR(100 BYTE);
        v_RegistrosTotales          NUMBER;
        v_id_ruta_archivo_servidor  NUMBER;
        
        v_f_inicio          varchar2(400);
        v_f_inicio2         varchar2(400);
        v_f_fin             varchar2(400);
        v_f_fin2             varchar2(400);
        
        --Parametros ServerSide
        v_dinamic_sql       clob;
        v_json_output       clob;
        v_ServerSide        json_object_t;
        v_filtersString     varchar2(4000);
        v_sortString        varchar2(4000);
        TYPE KeyValueRecord IS RECORD (
            value VARCHAR2(4000) -- Tipo de valor
        );
        TYPE Dictionary IS TABLE OF KeyValueRecord INDEX BY VARCHAR2(100); -- Índice por clave
        myDictionary Dictionary;
    begin
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, v_tipo_mensaje_seguimiento, 'Entro a realizar consulta del log');
        
        -- Deserializar JSON
        apex_json.free_output; 
        apex_json.initialize_clob_output( p_preserve => false );
        v_json_objeto := json_object_t(e_json);
        v_codigo                            := v_json_objeto.get_string('codigo');
        v_id_ruta_archivo_servidor          := v_json_objeto.get_number('id_ruta_archivo_servidor');

        --ServerSide
        v_ServerSide                    := v_json_objeto.get_Object('ServerSideJson');
        v_filtersString                 := v_ServerSide.get_string('filtersString');
        v_sortString                    := v_ServerSide.get_string('sortString');
        pageNumber                      := v_ServerSide.get_number('first');
        pageSize                        := v_ServerSide.get_number('rows');
        
        v_f_inicio                      := v_json_objeto.get_string('fechaInicial');
        v_f_fin                         := v_json_objeto.get_string('fechaFinal');
        
        --DBMS_OUTPUT.PUT_LINE('fecha inicio: ' || v_f_inicio);
        --DBMS_OUTPUT.PUT_LINE('fecha fin: ' || v_f_fin);
        
        v_f_inicio2 := TRUNC(TO_TIMESTAMP(v_f_inicio, 'YYYY-MM-DD"T"HH24:MI:SS"Z"'));
        v_f_fin2    := TRUNC(TO_TIMESTAMP(v_f_fin, 'YYYY-MM-DD"T"HH24:MI:SS"Z"'));
        DBMS_OUTPUT.PUT_LINE('fecha inicio: ' || v_f_inicio2);
        DBMS_OUTPUT.PUT_LINE('fecha fin: ' || v_f_fin2);

        --- -//-->INICIO DINAMIC SQL 26/02/2024
        --- -//- Filtros Staticos
        myDictionary('ServerSide_WhereAdd').value := case
                                                    when LENGTH(v_filtersString) > 0 then ' where ' || v_filtersString
                                                    else ' ' end;
                                                    
        myDictionary('ServerSide_WhereAdd').value := replace(replace(myDictionary('ServerSide_WhereAdd').value,'#x#','''%'),'#y#','%''');
        
        myDictionary('ServerSide_SortAdd').value := case
                                                    when LENGTH(v_sortString) > 0 then ' order by ' || v_sortString
                                                    else ' order by numero_orden desc ' end;
                                                    
        myDictionary('fecha').value := case
                                when LENGTH(v_f_inicio) != 0 and nvl(LENGTH(v_f_fin),0) = 0 then ' and trunc(d.fecha_cierre) = ''' || v_f_inicio2 || ''''
                                when LENGTH(v_f_inicio) = 0 and  nvl(LENGTH(v_f_fin),0) = 0 then ' '
                                when LENGTH(v_f_inicio) != 0 and nvl(LENGTH(v_f_fin),0) != 0 then ' and trunc(d.fecha_cierre) between ' || '''' || v_f_inicio2 || '''' || ' and ' || '''' || v_f_fin2 || ''''
                                else ' ' end;
        
        apex_json.open_object();
        apex_json.write('codigo', 0);
        apex_json.write('mensaje', 'Consulta exitosa');
        apex_json.open_object('datos');
         -- gnl_archivos_instancia
        v_dinamic_sql := '
        DECLARE
        BEGIN
            apex_json.open_array(''reportes_legalizacion'');
            for c_datos in (
                WITH t as (
                select a.numero_orden
                    , b.descripcion as estado
                    , SUBSTR(d.mensaje_error_ws,INSTR(d.mensaje_error_ws, '' '') + 1,1000) as error
                    , d.fecha_cierre
                from aire.ord_ordenes          a
                join aire.ord_estados_orden    b on a.id_estado_orden = b.id_estado_orden
                join aire.ord_tipos_orden      c on a.id_tipo_orden   = c.id_tipo_orden
                join aire.ord_ordenes_gestion  d on a.id_orden        = d.id_orden
                join aire.gnl_clientes         e on a.id_cliente      = e.id_cliente
                where d.ind_procesado_ws = ''N'' ' || myDictionary('fecha').value || '
                ), t2 as (
                    select count(*) RegistrosTotales 
                    from t
                    ' || myDictionary('ServerSide_WhereAdd').value || '
                    ' || myDictionary('ServerSide_SortAdd').value || '
                )
                select
                         t.*
                        , :v_rutaweb || t.numero_orden as pathwebdescarga
                        ,t2.RegistrosTotales
                    from t, t2
                    ' || myDictionary('ServerSide_WhereAdd').value || '
                    ' || myDictionary('ServerSide_SortAdd').value || '
                    OFFSET ' || pageNumber || ' ROWS FETCH NEXT ' || pageSize ||' ROWS ONLY
                ) loop    
                    apex_json.open_object();
                    apex_json.write(''numero_orden'', c_datos.numero_orden);
                    apex_json.write(''estado'', c_datos.estado);
                    apex_json.write(''error'',c_datos.error);
                    apex_json.write(''fecha_movimiento'',c_datos.fecha_cierre);
                    
                    BEGIN :v_RegistrosTotales := c_datos.RegistrosTotales; END;
                    apex_json.close_object();
                end loop;
            apex_json.close_array();
        END;
        ';
        DBMS_OUTPUT.PUT_LINE('Query: ' || v_dinamic_sql);
        EXECUTE IMMEDIATE v_dinamic_sql
        USING IN v_rutaweb, OUT v_RegistrosTotales;
        
        --DBMS_OUTPUT.PUT_LINE('llego');
        
        apex_json.write('RegistrosTotales', v_RegistrosTotales);
        apex_json.close_object();
        apex_json.close_object();
        
        --Si todo sale bien devolver mensaje.
        s_json := apex_json.get_clob_output;
        apex_json.free_output;

        --LOG
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Consulta exitosa');
        --dbms_output.put_line(s_json);
    exception
        when others then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, v_tipo_mensaje_error, 'Error al realizar la consulta: '|| sqlerrm);
            apex_json.free_output; 
            apex_json.open_object();
            apex_json.write('codigo', 1);
            apex_json.write('mensaje', 'Error N '||v_id_log ||' al realizar la consulta'|| replace(replace(replace(sqlerrm,'"',''''),chr(13),''),chr(10),''));
            apex_json.close_object();
            v_json := apex_json.get_clob_output;
            apex_json.free_output; 
            s_json := v_json;
    end prc_consultar_reportes_log_legalizacion;
    
    procedure prc_consultar_reportes_log_legalizacion_contratista(
        e_json  in  clob,
        s_json  out clob
    ) is
        v_json_objeto       json_object_t;
        v_json              clob;
        v_id_log            aire.gnl_logs.id_log%type;
        v_respuesta         aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_carlos_vargas_test10.prc_consultar_reportes_log_legalizacion');
        v_codigo                    aire.gnl_archivos.codigo%type;
        v_id_archivo                aire.gnl_archivos.id_archivo%type;
        v_objeto      	    aire.ord_ordenes%rowtype;
        v_id_persona        VARCHAR2(50 BYTE);
        v_id_usuario        VARCHAR2(50 BYTE);
        
        v_rutaweb                   VARCHAR2(255 BYTE);
        
        pageNumber                  NUMBER;
        pageSize                    NUMBER;
        sortColumn                  VARCHAR(100 BYTE);
        sortDirection               VARCHAR(100 BYTE);
        v_RegistrosTotales          NUMBER;
        v_id_ruta_archivo_servidor  NUMBER;
        
        v_f_inicio          varchar2(400);
        v_f_inicio2         varchar2(400);
        v_f_fin             varchar2(400);
        v_f_fin2             varchar2(400);
        
        --Parametros ServerSide
        v_dinamic_sql       clob;
        v_json_output       clob;
        v_ServerSide        json_object_t;
        v_filtersString     varchar2(4000);
        v_sortString        varchar2(4000);
        TYPE KeyValueRecord IS RECORD (
            value VARCHAR2(4000) -- Tipo de valor
        );
        TYPE Dictionary IS TABLE OF KeyValueRecord INDEX BY VARCHAR2(100); -- Índice por clave
        myDictionary Dictionary;
    begin
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, v_tipo_mensaje_seguimiento, 'Entro a realizar consulta del log');
        
        -- Deserializar JSON
        apex_json.free_output; 
        apex_json.initialize_clob_output( p_preserve => false );
        v_json_objeto := json_object_t(e_json);
        
        v_id_ruta_archivo_servidor          := v_json_objeto.get_number('id_ruta_archivo_servidor');
        v_id_persona                    := v_json_objeto.get_string('id_persona');
        v_id_usuario                    := v_json_objeto.get_string('id_usuario');

        --ServerSide
        v_ServerSide                    := v_json_objeto.get_Object('ServerSideJson');
        v_filtersString                 := v_ServerSide.get_string('filtersString');
        v_sortString                    := v_ServerSide.get_string('sortString');
        pageNumber                      := v_ServerSide.get_number('first');
        pageSize                        := v_ServerSide.get_number('rows');
        
        v_f_inicio                      := v_json_objeto.get_string('fechaInicial');
        v_f_fin                         := v_json_objeto.get_string('fechaFinal');

        --/-- no se valida si el contratista tiene contrato ya que son datos historicos.
        select
            nvl(a.id_contratista,'-1') 
                into v_objeto.id_contratista
        from aire.ctn_contratistas_persona		a
        where 
        -- a.id_contratista in (
        --     select
        --         id_contratista
        --     from aire.v_ctn_contratos
        --     where UPPER(prefijo_actividad) = 'G_SCR'
        --     and UPPER(ind_activo) = 'S'
        -- ) and 
        a.id_persona = v_id_persona and a.codigo_rol = 'ANALISTA';

        v_objeto.id_contratista_persona := v_json_objeto.get_string('id_contratista_persona');
        v_objeto.id_zona                := v_json_objeto.get_string('id_zona');
        
        --DBMS_OUTPUT.PUT_LINE('fecha inicio: ' || v_f_inicio);
        --DBMS_OUTPUT.PUT_LINE('fecha fin: ' || v_f_fin);
        
        v_f_inicio2 := TRUNC(TO_TIMESTAMP(v_f_inicio, 'YYYY-MM-DD"T"HH24:MI:SS"Z"'));
        v_f_fin2    := TRUNC(TO_TIMESTAMP(v_f_fin, 'YYYY-MM-DD"T"HH24:MI:SS"Z"'));
        DBMS_OUTPUT.PUT_LINE('fecha inicio: ' || v_f_inicio2);
        DBMS_OUTPUT.PUT_LINE('fecha fin: ' || v_f_fin2);

        --- -//-->INICIO DINAMIC SQL 26/02/2024
        --- -//- Filtros Staticos
        myDictionary('ServerSide_WhereAdd').value := case
                                                    when LENGTH(v_filtersString) > 0 then ' where ' || v_filtersString
                                                    else ' ' end;
                                                    
        myDictionary('ServerSide_WhereAdd').value := replace(replace(myDictionary('ServerSide_WhereAdd').value,'#x#','''%'),'#y#','%''');
        
        myDictionary('ServerSide_SortAdd').value := case
                                                    when LENGTH(v_sortString) > 0 then ' order by ' || v_sortString
                                                    else ' order by numero_orden desc ' end;
                                                    
        myDictionary('fecha').value := case
                                when LENGTH(v_f_inicio) != 0 and nvl(LENGTH(v_f_fin),0) = 0 then ' and trunc(d.fecha_cierre) = ''' || v_f_inicio2 || ''''
                                when LENGTH(v_f_inicio) = 0 and  nvl(LENGTH(v_f_fin),0) = 0 then ' '
                                when LENGTH(v_f_inicio) != 0 and nvl(LENGTH(v_f_fin),0) != 0 then ' and trunc(d.fecha_cierre) between ' || '''' || v_f_inicio2 || '''' || ' and ' || '''' || v_f_fin2 || ''''
                                else ' ' end;
        
        /*myDictionary('id_contratista_persona').value := case
                                                    when v_objeto.id_contratista_persona = -1 then ' and a.id_contratista_persona is null '
                                                    when v_objeto.id_contratista_persona = -2 then ' ' --es todo!
                                                    when v_objeto.id_contratista_persona > 0 then ' and a.id_contratista_persona = ' || v_objeto.id_contratista_persona || ' '
                                                else ' ' end;
        myDictionary('id_zona').value := case
                                                    when v_objeto.id_zona = -1 then ' and a.id_zona is null '
                                                    when v_objeto.id_zona = -2 then ' ' --es todo!
                                                    when v_objeto.id_zona > 0 then ' and a.id_zona = ' || v_objeto.id_zona || ' '
                                                else ' ' end;*/
        --- -//- ServerSide Angular Table
        SELECT
            ruta_web INTO v_rutaweb
        FROM aire.gnl_rutas_archivo_servidor
        WHERE id_ruta_archivo_servidor = v_id_ruta_archivo_servidor;
        
        /*select
            g.id_archivo
            into v_id_archivo
        from aire.gnl_archivos g
        where g.codigo = v_codigo;*/
        
        /*apex_json.open_object();
        apex_json.open_object('datos');
        apex_json.open_array('reportes_legalizacion');*/
        
        apex_json.open_object();
        apex_json.write('codigo', 0);
        apex_json.write('mensaje', 'Consulta exitosa');
        apex_json.open_object('datos');
         -- gnl_archivos_instancia
        v_dinamic_sql := '
        DECLARE
        BEGIN
            apex_json.open_array(''reportes_legalizacion'');
            for c_datos in (
                WITH t as (
                    select a.numero_orden
                        , b.descripcion as estado
                        , SUBSTR(d.mensaje_error_ws,INSTR(d.mensaje_error_ws, '' '') + 1,1000) as error
                        , d.fecha_cierre
                    from aire.ord_ordenes          a
                    join aire.ord_estados_orden    b on a.id_estado_orden = b.id_estado_orden
                    join aire.ord_tipos_orden      c on a.id_tipo_orden   = c.id_tipo_orden
                    join aire.ord_ordenes_gestion  d on a.id_orden        = d.id_orden
                    join aire.gnl_clientes         e on a.id_cliente      = e.id_cliente
                    where d.ind_procesado_ws = ''N''  and a.id_contratista = '||v_objeto.id_contratista||'                    
                    ' || myDictionary('fecha').value || '
                ), t2 as (
                    select count(*) RegistrosTotales 
                    from t
                    ' || myDictionary('ServerSide_WhereAdd').value || '
                    ' || myDictionary('ServerSide_SortAdd').value || '
                )
                select
                         t.*
                        , :v_rutaweb || t.numero_orden as pathwebdescarga
                        ,t2.RegistrosTotales
                    from t, t2
                    ' || myDictionary('ServerSide_WhereAdd').value || '
                    ' || myDictionary('ServerSide_SortAdd').value || '
                    OFFSET ' || pageNumber || ' ROWS FETCH NEXT ' || pageSize ||' ROWS ONLY
                ) loop    
                    apex_json.open_object();
                    apex_json.write(''numero_orden'', c_datos.numero_orden);
                    apex_json.write(''estado'', c_datos.estado);
                    apex_json.write(''error'',c_datos.error);
                    apex_json.write(''fecha_movimiento'',c_datos.fecha_cierre);
                    
                    BEGIN :v_RegistrosTotales := c_datos.RegistrosTotales; END;
                    apex_json.close_object();
                end loop;
            apex_json.close_array();
        END;
        ';
        DBMS_OUTPUT.PUT_LINE('Query: ' || v_dinamic_sql);
        EXECUTE IMMEDIATE v_dinamic_sql
        USING IN v_rutaweb, OUT v_RegistrosTotales;
        
        --DBMS_OUTPUT.PUT_LINE('llego');
        
        apex_json.write('RegistrosTotales', v_RegistrosTotales);
        apex_json.close_object();
        apex_json.close_object();
        
        --Si todo sale bien devolver mensaje.
        s_json := apex_json.get_clob_output;
        apex_json.free_output;

        --LOG
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Consulta exitosa');
        --dbms_output.put_line(s_json);
    exception
        when others then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, v_tipo_mensaje_error, 'Error al realizar la consulta: '|| sqlerrm);
            apex_json.free_output; 
            apex_json.open_object();
            apex_json.write('codigo', 1);
            apex_json.write('mensaje', 'Error N '||v_id_log ||' al realizar la consulta' || replace(replace(replace(sqlerrm,'"',''''),chr(13),''),chr(10),''));
            apex_json.close_object();
            v_json := apex_json.get_clob_output;
            apex_json.free_output; 
            s_json := v_json;
    end prc_consultar_reportes_log_legalizacion_contratista;

END PKG_G_CARLOS_VARGAS_TEST10;