CREATE OR REPLACE PACKAGE "AIRE".PKG_G_CARLOS_VARGAS_TEST3 AS
    procedure prc_parametros_iniciales_areacentral (
        s_json 	out	clob
	);

    procedure prc_consultar_archivos_instancia (
        e_json  in  clob,
        s_json 	out	clob
	);

    procedure prc_consultar_ordenes_area_central (
        e_json  in  clob,
        s_json 	out	clob
	);
END PKG_G_CARLOS_VARGAS_TEST3;
/
CREATE OR REPLACE PACKAGE BODY "AIRE".PKG_G_CARLOS_VARGAS_TEST3 AS
  
  --/-- se migra a pkg_g_ordenes el 22/03/2024
    procedure prc_parametros_iniciales_areacentral (
        s_json 	out	clob
	) is
		--v_json              clob;
        v_id_actividad  NUMBER(3,0);
        v_id_log		aire.gnl_logs.id_log%type;
        v_respuesta		aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Carga realizada con exito.', nombre_up   => 'pkg_g_ordenes.prc_parametros_iniciales_areacentral');
    BEGIN
        -- consultamos el id de la actividad de SCR
        begin
            select id_actividad
              into v_id_actividad
              from aire.gnl_actividades
             where prefijo = 'G_SCR' and ind_activo = 'S';
        exception
            when others then
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Error al consultar la actividad de SCR: '||sqlerrm);
                --sb_escribir_respuesta(1, 'Error N '||v_id_log||' al registrar cuestionario: no se encuentra parametrizda la actividad SCR');
                apex_json.free_output;
                apex_json.open_object();
                apex_json.write('codigo', 1);
                apex_json.write('mensaje', 'Error N '||v_id_log ||' al registrar cuestionario: no se encuentra parametrizda la actividad SCR');
                apex_json.close_object();
                --v_json := apex_json.get_clob_output;
                s_json := apex_json.get_clob_output;
                apex_json.free_output;
        end;
        
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
        apex_json.free_output;
        apex_json.initialize_clob_output( p_preserve => false );
        
        apex_json.open_object();
            apex_json.write('codigo', 0);
            apex_json.write('mensaje', 'Carga exitosa');
            apex_json.open_object('datos');
            -- Tipos de orden
                apex_json.open_array('tipos_orden');
                    for c_datos in (
                        select id_tipo_orden
                             , codigo_tipo_orden
                             ,(codigo_tipo_orden || ' - ' || descripcion) as descripcion
                          from aire.ord_tipos_orden
                         where id_actividad = v_id_actividad
                           and ind_activo   = 'S'
                    ) loop
                        apex_json.open_object();
                        apex_json.write('id_tipo_orden', c_datos.id_tipo_orden);
                        apex_json.write('codigo_tipo_orden', c_datos.codigo_tipo_orden);
                        apex_json.write('descripcion',c_datos.descripcion);
                        apex_json.close_object();
                    end loop;
                apex_json.close_array();
                
            -- Estados orden
                apex_json.open_array('estados_orden');
                    for c_datos in (
                        select id_estado_orden
                             , codigo_estado
                             , descripcion
                          from aire.ord_estados_orden
                         where id_actividad     = v_id_actividad
                           and ind_activo = 'S' and UPPER(CODIGO_ESTADO) != 'CERR'
                    ) loop
                        apex_json.open_object();
                        apex_json.write('id_estado_orden', c_datos.id_estado_orden);
                        apex_json.write('codigo_estado',c_datos.codigo_estado);
                        apex_json.write('descripcion',c_datos.descripcion);
                        apex_json.close_object();
                    end loop;
                apex_json.close_array();
            
            -- v_ctn_contratistas
                apex_json.open_array('contratistas');
                    for c_datos in (
                        select x.id_contratista
                             , x.id_persona
                             , x.identificacion
                             , x.nombre_completo
                             , x.email
                             , x.ind_activo
                             , x.descripcion_ind_activo
                             , x.codigo
                          from aire.v_ctn_contratistas x
                          where x.id_contratista in (
                                 select id_contratista
                                        from aire.v_ctn_contratos
                                        where UPPER(prefijo_actividad) = 'G_SCR'
                                        and UPPER(ind_activo) = 'S'
                           ) and UPPER(x.ind_activo) = 'S'
                    ) loop
                        apex_json.open_object();
                            apex_json.write('id_contratista', c_datos.id_contratista);
                            apex_json.write('id_persona', c_datos.id_persona);
                            apex_json.write('identificacion', c_datos.identificacion);
                            apex_json.write('nombre_completo', c_datos.nombre_completo);
                            apex_json.write('email', c_datos.email);
                            apex_json.write('ind_activo', c_datos.ind_activo);
                            apex_json.write('descripcion_ind_activo', c_datos.descripcion_ind_activo);
                            apex_json.write('codigo', c_datos.codigo);
                            -- gnl_zonas_x_Contratista_arrays
                            apex_json.open_array('zonas');
                                for c_datos2 in (
                                    select id_zona
                                    from aire.ctn_contratos_zona
                                    where id_contrato in (
                                                select id_contrato
                                                from aire.v_ctn_contratos
                                                where prefijo_actividad = 'G_SCR'
                                                and id_contratista = c_datos.id_contratista
                                                )
                                ) loop
                                    apex_json.write(c_datos2.id_zona);
                                end loop;
                            apex_json.close_array();
                            apex_json.open_array('Brigadas');
                                -- Brigadas
                                for x_datos in (
                                    select
                                        a.id_contratista_persona
                                        ,a.identificacion_contratista_persona
                                        ,a.nombre_contratista_persona
                                    from aire.v_ctn_contratistas_persona		a
                                    where a.id_contratista in (
                                        select
                                            id_contratista
                                        from aire.v_ctn_contratos
                                        where LOWER(prefijo_actividad) = 'g_scr'
                                        and LOWER(ind_activo) = 's'
                                    )
                                    and LOWER(a.ind_activo) = 's' and a.id_contratista = c_datos.id_contratista and LOWER(a.codigo_rol) = 'tecnico'
                                    order by a.id_contratista_persona
                                ) loop
                            apex_json.open_object();
                                apex_json.write('id_contratista_persona', x_datos.id_contratista_persona);
                                apex_json.write('identificacion_contratista_persona', x_datos.identificacion_contratista_persona);
                                apex_json.write('nombre_contratista_persona', x_datos.nombre_contratista_persona);
                            apex_json.close_object();
                            end loop;
                            apex_json.close_array();
                        apex_json.close_object();
                    end loop;
                apex_json.close_array();
                
            -- gnl_territoriales
                apex_json.open_array('territoriales');
                    for c_datos in (
                        select id_territorial
                             , id_departamento
                             , codigo
                             , nombre
                          from aire.gnl_territoriales
                          where ind_activo = 'S'
                    ) loop
                        apex_json.open_object();
                        apex_json.write('id_territorial', c_datos.id_territorial);
                        apex_json.write('id_departamento', c_datos.id_departamento);
                        apex_json.write('codigo', c_datos.codigo);
                        apex_json.write('nombre', c_datos.nombre);
                        apex_json.close_object();
                    end loop;
                apex_json.close_array();
            
            -- gnl_zonas
                apex_json.open_array('zonas');
                    for c_datos in (
                        select id_zona
                             , id_territorial
                             , codigo
                             , nombre
                          from aire.gnl_zonas
                          where ind_activo = 'S'
                    ) loop
                        apex_json.open_object();
                        apex_json.write('id_zona', c_datos.id_zona);
                        apex_json.write('id_territorial', c_datos.id_territorial);
                        apex_json.write('codigo', c_datos.codigo);
                        apex_json.write('nombre', c_datos.nombre);
                        apex_json.close_object();
                    end loop;
                apex_json.close_array();
            
            -- scr_tipos_suspencion
                apex_json.open_array('tipos_suspencion');
                    for c_datos in (
                        select id_tipo_suspencion
                             , codigo
                             , descripcion
                             , id_actividad
                          from aire.scr_tipos_suspencion
                          where ind_activo = 'S'
                                AND id_actividad = v_id_actividad
                    ) loop
                        apex_json.open_object();
                            apex_json.write('id_tipo_suspencion', c_datos.id_tipo_suspencion);
                            apex_json.write('codigo', c_datos.codigo);
                            apex_json.write('descripcion', c_datos.descripcion);
                            apex_json.write('id_actividad', c_datos.id_actividad);
                                apex_json.open_array('tipos_brigada');
                                    for c_datos2 in (
                                        SELECT
                                            xy.id_tipo_brigada
                                            , y.codigo
                                            , y.descripcion
                                        from aire.scr_tipos_suspencion_tipo_brigada xy
                                        inner join aire.ctn_tipos_brigada y on y.id_tipo_brigada = xy.id_tipo_brigada
                                        WHERE       y.ind_activo = 'S'
                                                and xy.id_tipo_suspencion = c_datos.id_tipo_suspencion
                                    ) loop
                                    apex_json.open_object();
                                        apex_json.write('id_tipo_brigada', c_datos2.id_tipo_brigada);
                                        apex_json.write('codigo', c_datos2.codigo);
                                        apex_json.write('descripcion', c_datos2.descripcion);
                                    apex_json.close_object();
                                    end loop;
                                apex_json.close_array();
                        apex_json.close_object();
                    end loop;
                apex_json.close_array();

            -- anomalias y sub anomalias
                apex_json.open_array('anomalias');
                    for c_datos in (
                        select
                             a.id_anomalia
                            ,a.id_tipo_orden
                            ,a.descripcion
                        from aire.ord_anomalias a
                        where       a.ind_activo = 'S'
                                and a.id_resultado in (
                                    select x.id_resultado
                                    from aire.ord_resultados x
                                    where x.ind_ejecuta_actividad = 'N'
                                        and x.id_actividad in (select id_actividad from aire.gnl_actividades where prefijo = 'G_SCR' and ind_activo = 'S')
                                )
                    ) loop
                        apex_json.open_object();
                        apex_json.write('id_anomalia', c_datos.id_anomalia);
                        apex_json.write('id_tipo_orden', c_datos.id_tipo_orden);
                        apex_json.write('descripcion', c_datos.descripcion);
                            -- sub anomalias
                            apex_json.open_array('sub_anomalias');
                                for c_datos2 in (
                                    select
                                         b.id_subanomalia
                                        ,b.descripcion
                                    from aire.ord_subanomalias b
                                    where b.ind_activo = 'S' and b.id_anomalia = c_datos.id_anomalia
                                ) loop
                                    apex_json.open_object();
                                    apex_json.write('id_subanomalia', c_datos2.id_subanomalia);
                                    apex_json.write('descripcion', c_datos2.Descripcion);
                                    apex_json.close_object();
                                end loop;
                            apex_json.close_array();
                        apex_json.close_object();
                    end loop;
                apex_json.close_array();

            -- gnl_estados_servicio
                apex_json.open_array('estados_servicio');
                    for c_datos in (
                        select id_estado_servicio
                             , codigo
                             , descripcion
                          from aire.gnl_estados_servicio
                          where ind_activo = 'S'
                    ) loop
                        apex_json.open_object();
                        apex_json.write('id_estado_servicio', c_datos.id_estado_servicio);
                        apex_json.write('codigo', c_datos.codigo);
                        apex_json.write('descripcion', c_datos.descripcion);
                        apex_json.close_object();
                    end loop;
                apex_json.close_array();
            -- parametros generales dominios
                apex_json.open_array('parametros_generales');
                    for c_datos in (
                        select
                             dv.descripcion
                            ,dv.valor
                            ,dv.codigo
                        from aire.gnl_dominios_valor dv
                        inner join aire.gnl_dominios d on d.id_dominio = dv.id_dominio
                        where d.codigo = 'PARAM' and dv.codigo = 'PRM01'
                    ) loop
                        apex_json.open_object();
                        apex_json.write('codigo', c_datos.codigo);
                        apex_json.write('descripcion', c_datos.descripcion);
                        apex_json.write('valor', c_datos.valor);
                        apex_json.close_object();
                    end loop;
                apex_json.close_array();

            apex_json.close_object();
        apex_json.close_object();
        
        ---v_json := apex_json.get_clob_output;
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
        --s_json := v_json;
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Carga exitosa');
        dbms_output.put_line(s_json);
    exception
        when others then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al crear el json: '|| sqlerrm);
            apex_json.free_output;
            apex_json.open_object();
            apex_json.write('codigo', 1);
            apex_json.write('mensaje', 'Error N '||v_id_log ||' al crear los parametros iniciales');
            apex_json.close_object();
            --v_json := apex_json.get_clob_output;
            s_json := apex_json.get_clob_output;
            apex_json.free_output;
            --s_json := v_json;
  END prc_parametros_iniciales_areacentral;

  --/-- se migro a pkg_g_ordenes el 22/03/2024
    procedure prc_consultar_archivos_instancia (
        e_json  in  clob,
        s_json 	out	clob
    ) is
        --e_json  	clob :=  '{"codigo":"ASBO"}';
        v_json_objeto               json_object_t;
        v_id_log		            aire.gnl_logs.id_log%type;
        v_respuesta		            aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_ordenes.prc_consultar_archivos_instancia');
        v_codigo                    aire.gnl_archivos.codigo%type;
        v_id_archivo                aire.gnl_archivos.id_archivo%type;
        
        v_rutaweb                   VARCHAR2(255 BYTE);

        pageNumber                  NUMBER;
        pageSize                    NUMBER;
        sortColumn                  VARCHAR(100 BYTE);
        sortDirection               VARCHAR(100 BYTE);
        v_RegistrosTotales          NUMBER;
        v_id_ruta_archivo_servidor  NUMBER;

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
    BEGIN
    --LOG
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
    
    -- Deserializar JSON
    apex_json.free_output;
    apex_json.initialize_clob_output( p_preserve => false );
    v_json_objeto                       := json_object_t(e_json);
    v_codigo                            := v_json_objeto.get_string('codigo');
    -- pageNumber                          := v_json_objeto.get_number('pageNumber');
    -- pageSize                            := v_json_objeto.get_number('pageSize');
    v_id_ruta_archivo_servidor          := v_json_objeto.get_number('id_ruta_archivo_servidor');

    --ServerSide
    v_ServerSide                    := v_json_objeto.get_Object('ServerSideJson');
    v_filtersString                 := v_ServerSide.get_string('filtersString');
    v_sortString                    := v_ServerSide.get_string('sortString');
    pageNumber                      := v_ServerSide.get_number('first');
    pageSize                        := v_ServerSide.get_number('rows');

    

    --- -//-->INICIO DINAMIC SQL 26/02/2024
    --- -//- Filtros Staticos
    myDictionary('ServerSide_WhereAdd').value := case
                                                when LENGTH(v_filtersString) > 0 then ' and ' || v_filtersString
                                                else ' ' end;
    myDictionary('ServerSide_WhereAdd').value := replace(replace(myDictionary('ServerSide_WhereAdd').value,'#x#','''%'),'#y#','%''');
    myDictionary('ServerSide_SortAdd').value := case
                                                when LENGTH(v_sortString) > 0 then ' order by ' || v_sortString
                                                else ' order by a.id_archivo_instancia desc ' end;
    --- -//- ServerSide Angular Table

    SELECT
        ruta_web INTO v_rutaweb
    FROM aire.gnl_rutas_archivo_servidor
    WHERE id_ruta_archivo_servidor = v_id_ruta_archivo_servidor;

    select
        g.id_archivo
        into v_id_archivo
    from aire.gnl_archivos g
    where g.codigo = v_codigo;

    
    apex_json.open_object();
        apex_json.write('codigo', 0);
        apex_json.write('mensaje', 'Consulta exitosa');
        apex_json.open_object('datos');
         -- gnl_archivos_instancia
            v_dinamic_sql := '
            DECLARE
            BEGIN
            apex_json.open_array(''archivos_instancia'');
                for c_datos in (
                    WITH t as (
                        SELECT
                            a.id_archivo_instancia
                            , a.id_archivo
                            , a.nombre_archivo
                            , a.numero_registros_archivo
                            , a.numero_registros_procesados
                            , a.numero_errores
                            , a.fecha_inicio_cargue
                            , a.fecha_fin_cargue
                            , a.duracion
                            , a.id_usuario_registro
                            , a.fecha_registro
                            , a.id_estado_intancia
                            , a.observaciones
                            , a.id_soporte
                        FROM  aire.gnl_archivos_instancia a
                        WHERE a.id_archivo = :v_id_archivo ' || myDictionary('ServerSide_WhereAdd').value || '
                        ' || myDictionary('ServerSide_SortAdd').value || '
                    ), t2 as (
                        select count(*) RegistrosTotales from t
                    )
                    select
                         t.*
                        , :v_rutaweb || t.id_soporte as pathwebdescarga
                        ,t2.RegistrosTotales
                    from t, t2
                    OFFSET ' || pageNumber || ' ROWS FETCH NEXT ' || pageSize ||' ROWS ONLY
                ) loop
                apex_json.open_object();
                    apex_json.write(''id_archivo_instancia'', c_datos.id_archivo_instancia);
                    apex_json.write(''id_archivo'', c_datos.id_archivo);
                    apex_json.write(''nombre_archivo'', c_datos.nombre_archivo);
                    apex_json.write(''numero_registros_archivo'', c_datos.numero_registros_archivo);
                    apex_json.write(''numero_registros_procesados'', c_datos.numero_registros_procesados);
                    apex_json.write(''numero_errores'', c_datos.numero_errores);
                    apex_json.write(''fecha_inicio_cargue'', c_datos.fecha_inicio_cargue);
                    apex_json.write(''fecha_fin_cargue'', c_datos.fecha_fin_cargue);
                    apex_json.write(''duracion'', c_datos.duracion);
                    apex_json.write(''id_usuario_registro'', c_datos.id_usuario_registro);
                    apex_json.write(''fecha_registro'', c_datos.fecha_registro);
                    apex_json.write(''id_estado_intancia'', c_datos.id_estado_intancia);
                    apex_json.write(''observaciones'', c_datos.observaciones);
                    apex_json.write(''pathwebdescarga'', c_datos.pathwebdescarga);
                    
                    BEGIN :v_RegistrosTotales := c_datos.RegistrosTotales; END;
                apex_json.close_object();
                end loop;
            apex_json.close_array();
            END;
            ';
            DBMS_OUTPUT.PUT_LINE('Query: ' || v_dinamic_sql);
            EXECUTE IMMEDIATE v_dinamic_sql
            USING IN v_id_archivo, IN V_RUTAWEB, OUT v_RegistrosTotales;

            apex_json.write('RegistrosTotales', v_RegistrosTotales);
        apex_json.close_object();
    apex_json.close_object();
    
    --Si todo sale bien devolver mensaje.
    s_json := apex_json.get_clob_output;
    apex_json.free_output;
    
    --LOG
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Carga exitosa');
    --dbms_output.put_line(s_json);
    exception
    when others then
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al crear el json: '|| sqlerrm);
        apex_json.free_output;
        apex_json.open_object();
        apex_json.write('codigo', 400);
        apex_json.write('mensaje', 'Error N '||v_id_log ||' al consultar los registros.' || replace(replace(replace(sqlerrm,'"',''''),chr(13),''),chr(10),''));
        apex_json.close_object();
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
        --dbms_output.put_line('xxx: '|| sqlerrm);
    END prc_consultar_archivos_instancia;

  procedure prc_consultar_ordenes_area_central (
        e_json  in  clob,
        s_json 	out	clob
	) is
		--e_json  	clob :=  '{"id_contratista": "2076341","id_zona": ""}';
        --v_json      clob;
        v_json_objeto       json_object_t;
        v_id_log		    aire.gnl_logs.id_log%type;
        v_respuesta		    aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_ordenes.prc_consultar_ordenes_area_central');
        v_objeto      	    aire.ord_ordenes%rowtype;
        v_id_orden          NUMBER;
        pageNumber          NUMBER;
        pageSize            NUMBER;
        sortColumn          VARCHAR(100 BYTE);
        sortDirection       VARCHAR(100 BYTE);
        v_RegistrosTotales  NUMBER;
        v_id_estado_orden   aire.ord_estados_orden.id_estado_orden%type;
        v_codigo_estado     aire.ord_estados_orden.codigo_estado%type;

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
    BEGIN
    
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
    apex_json.free_output;
    apex_json.initialize_clob_output( p_preserve => false );
    
    -- parseamos el json
    v_json_objeto := json_object_t(e_json);
    -- extraemos los datos de la orden
    v_objeto.id_contratista         := v_json_objeto.get_string('id_contratista');
    v_objeto.id_zona                := v_json_objeto.get_string('id_zona');
    v_id_estado_orden               := v_json_objeto.get_string('id_estado_orden');
    v_codigo_estado                 := v_json_objeto.get_string('codigo_estado');
    v_id_orden                      := v_json_objeto.get_number('id_orden');
    -- pageNumber                      := v_json_objeto.get_number('pageNumber');
    -- pageSize                        := v_json_objeto.get_number('pageSize');
    v_ServerSide                    := v_json_objeto.get_Object('ServerSideJson');
    v_filtersString                 := v_ServerSide.get_string('filtersString');
    v_sortString                    := v_ServerSide.get_string('sortString');
    pageNumber                      := v_ServerSide.get_number('pageNumber');
    pageSize                        := v_ServerSide.get_number('pageSize');

    DBMS_OUTPUT.PUT_LINE('v_ServerSide: ' || v_ServerSide.to_string());
    

    --- -//-->INICIO DINAMIC SQL 26/02/2024
    --- -//- Filtros Staticos
    myDictionary('id_orden').value := case when v_id_orden > 0 then ' and a.id_orden = ' || v_id_orden else ' ' end;
    myDictionary('id_contratista').value := case
                                                when v_id_orden > 0 then ' '
                                                when v_objeto.id_contratista = -1 then ' and a.id_contratista is null '
                                                when v_objeto.id_contratista = -2 then ' ' --es todo!
                                                when v_objeto.id_contratista > 0 then ' and a.id_contratista = ' || v_objeto.id_contratista || ' '
                                            else ' ' end;
    myDictionary('id_zona').value := case
                                                when v_id_orden > 0 then ' '
                                                when v_objeto.id_zona = -1 then ' and a.id_zona is null '
                                                when v_objeto.id_zona = -2 then ' ' --es todo!
                                                when v_objeto.id_zona > 0 then ' and a.id_zona = ' || v_objeto.id_zona || ' '
                                            else ' ' end;
    myDictionary('codigo_estado').value := case
                                                when v_id_orden > 0 then ' '
                                                when v_codigo_estado = '-1' then ' and b.codigo_estado is null '
                                                when v_codigo_estado = '-2' then ' ' --es todo!
                                                when TRIM(NVL(v_codigo_estado,'-')) NOT IN (' ', '-1','-2') then ' and b.codigo_estado IN (SELECT
                                                                                                                        REGEXP_SUBSTR(''' || v_codigo_estado || ''', ''[^,]+'', 1, LEVEL) AS valor
                                                                                                                    FROM dual
                                                                                                                    CONNECT BY REGEXP_SUBSTR(''' || v_codigo_estado || ''', ''[^,]+'', 1, LEVEL) IS NOT NULL
                                                                                                                ) '
                                            else ' ' end;
    myDictionary('ServerSide_WhereAdd').value := case
                                                when LENGTH(v_filtersString) > 0 then ' where ' || v_filtersString
                                                else ' ' end;
    myDictionary('ServerSide_WhereAdd').value := replace(replace(myDictionary('ServerSide_WhereAdd').value,'#x#','''%'),'#y#','%''');
    myDictionary('ServerSide_SortAdd').value := case
                                                when LENGTH(v_sortString) > 0 then ' order by ' || v_sortString
                                                else ' ' end;
    --- -//- ServerSide Angular Table


    --DBMS_OUTPUT.PUT_LINE('e_json: ' || e_json);
    -- DBMS_OUTPUT.PUT_LINE('v_ServerSide: ' || v_ServerSide.to_string());
    -- DBMS_OUTPUT.PUT_LINE('------------------------------------------------------------');
    DBMS_OUTPUT.PUT_LINE('ServerSide_WhereAdd: ' || myDictionary('ServerSide_WhereAdd').value);
    DBMS_OUTPUT.PUT_LINE('ServerSide_SortAdd: ' || myDictionary('ServerSide_SortAdd').value);
    -- DBMS_OUTPUT.PUT_LINE('------------------------------------------------------------');
    --- -//-->FIN DINAMIC SQL
    
    apex_json.open_object();
        apex_json.write('codigo', 0);
        apex_json.write('mensaje', 'Consulta exitosa: '); -- || v_ServerSide.to_string()
        apex_json.open_object('datos');
         -- ordenes
            v_dinamic_sql := '
            DECLARE
            BEGIN
            apex_json.open_array(''ordenes'');
                for c_datos in (
                    with tx as (
                        select a.id_orden
                            , a.id_tipo_orden
                            , h.descripcion as tipo_orden
                            , a.numero_orden
                            , a.id_estado_orden
                            , b.descripcion  as estado_orden
                            , NVL(a.id_contratista,-1) id_contratista
                            , NVL(c.nombre_completo,''-'') as contratista
                            , a.id_cliente
                            , d.nombre_cliente as cliente
                            , a.id_territorial
                            , e.nombre as territorial
                            , NVL(a.id_zona,-1) id_zona
                            , NVL(f.nombre,''-'') as zona
                            , a.direcion
                            , a.fecha_creacion
                            , NVL(a.fecha_cierre,TO_TIMESTAMP(''1900/01/01 12:01:01,000000000 AM'', ''YYYY/MM/DD HH:MI:SS,FF9 AM'')) as fecha_cierre
                            , NVL(a.id_usuario_cierre,-1) as id_usuario_cierre
                            , NVL(g.nombres,''-'') as usuario_cierre
                            , a.id_actividad
                            , i.nombre as actividad
                            , a.id_contratista_persona
                            , j.nombre_contratista_persona
                            , NVL(a.id_tipo_trabajo,-1) as id_tipo_trabajo
                            , NVL(k.descripcion,''-'') as tipo_trabajo
                            , NVL(a.id_tipo_suspencion,-1) as id_tipo_suspencion
                            , NVL(l.descripcion,''-'') as tipo_suspencion
                            , a.origen
                            , mensaje_error_wsgr as mensaje_error_ws
                        from aire.ord_ordenes              				a
                        inner join aire.ord_estados_orden   			b on a.id_estado_orden   		= b.id_estado_orden
                        left join aire.v_ctn_contratistas  				c on a.id_contratista    		= c.id_contratista
                        left join aire.gnl_clientes        				d on a.id_cliente        		= d.id_cliente
                        left join aire.gnl_territoriales   				e on a.id_territorial    		= e.id_territorial
                        left join aire.gnl_zonas           				f on a.id_zona           		= f.id_zona
                        left join aire.v_sgd_usuarios      				g on a.id_usuario_cierre 		= g.id_usuario
                        left join aire.ord_tipos_orden     				h on a.id_tipo_orden     		= h.id_tipo_orden
                        left join aire.gnl_actividades     				i on a.id_actividad      		= i.id_actividad
                        left join aire.v_ctn_contratistas_persona  		j on a.id_contratista_persona 	= j.id_contratista_persona
                        left join aire.ord_tipos_trabajo 				k on a.id_tipo_trabajo 		 	= k.id_tipo_trabajo
                        left join aire.scr_tipos_suspencion 			l on a.id_tipo_suspencion 	 	= l.id_tipo_suspencion
                        left join (
                            select
                                x.id_orden, listagg(distinct nvl(x.mensaje_error_ws,''x''),''| '') WITHIN GROUP (ORDER BY x.id_orden) as mensaje_error_wsgr
                            from aire.ord_ordenes_gestion              x
                            group by x.id_orden
                        )                                               m on a.id_orden                 = m.id_orden
                        where
                            NVL(a.id_contratista,-1) in (
                                    select id_contratista
                                    from aire.v_ctn_contratos
                                    where UPPER(prefijo_actividad) = ''G_SCR''
                                    and UPPER(ind_activo) = ''S''
                                    union all
                                    select -1 from dual
                            )
                            ' || myDictionary('id_orden').value || '
                            ' || myDictionary('id_contratista').value || '
                            ' || myDictionary('id_zona').value || '
                            ' || myDictionary('codigo_estado').value || '
                    )
                    select
                            a.*
                          , SUM(1) OVER() RegistrosTotales
                    from tx a
                    ' || myDictionary('ServerSide_WhereAdd').value || '
                    ' || myDictionary('ServerSide_SortAdd').value || '
                    OFFSET ' || pageNumber * pageSize || ' ROWS FETCH NEXT ' || pageSize ||' ROWS ONLY
                ) loop
                apex_json.open_object();
                    apex_json.write(''id_orden'', c_datos.id_orden);
                    apex_json.write(''id_tipo_orden'', c_datos.id_tipo_orden);
                    apex_json.write(''tipo_orden'', c_datos.tipo_orden);
                    apex_json.write(''numero_orden'', c_datos.numero_orden);
                    apex_json.write(''id_estado_orden'', c_datos.id_estado_orden);
                    apex_json.write(''estado_orden'', c_datos.estado_orden);
                    apex_json.write(''id_contratista'', c_datos.id_contratista);
                    apex_json.write(''contratista'', c_datos.contratista);
                    apex_json.write(''id_cliente'', c_datos.id_cliente);
                    apex_json.write(''cliente'', c_datos.cliente);
                    apex_json.write(''id_territorial'', c_datos.id_territorial);
                    apex_json.write(''territorial'', c_datos.territorial);
                    apex_json.write(''id_zona'', c_datos.id_zona);
                    apex_json.write(''zona'', c_datos.zona);
                    apex_json.write(''direcion'', c_datos.direcion);
                    apex_json.write(''fecha_creacion'', c_datos.fecha_creacion);
                    apex_json.write(''fecha_cierre'', c_datos.fecha_cierre);
                    apex_json.write(''id_usuario_cierre'', c_datos.id_usuario_cierre);
                    apex_json.write(''usuario_cierre'', c_datos.usuario_cierre);
                    apex_json.write(''id_actividad'', c_datos.id_actividad);
                    apex_json.write(''actividad'', c_datos.actividad);
                    apex_json.write(''id_contratista_persona'', c_datos.id_contratista_persona);
                    apex_json.write(''nombre_contratista_persona'', c_datos.nombre_contratista_persona);
                    apex_json.write(''id_tipo_trabajo'', c_datos.id_tipo_trabajo);
                    apex_json.write(''tipo_trabajo'', c_datos.tipo_trabajo);
                    apex_json.write(''id_tipo_suspencion'', c_datos.id_tipo_suspencion);
                    apex_json.write(''tipo_suspencion'', c_datos.tipo_suspencion);
                    apex_json.write(''origen'', c_datos.origen);
                    apex_json.write(''mensaje_error_ws'', c_datos.mensaje_error_ws);
                    --v_RegistrosTotales := c_datos.RegistrosTotales;
                    BEGIN :v_RegistrosTotales := c_datos.RegistrosTotales; END;
                apex_json.close_object();
            end loop;
            apex_json.close_array();
            --BEGIN :v_json_output := apex_json.get_clob_output; END;
            END;
            ';
            EXECUTE IMMEDIATE v_dinamic_sql
            USING OUT v_RegistrosTotales;

            -- DBMS_OUTPUT.PUT_LINE('------------------------------------------------------------');
            -- DBMS_OUTPUT.PUT_LINE('Query: ' || v_dinamic_sql);
            

         -- agrupamiento asignadas y no asignadas
            apex_json.open_array('grafica_asignacion');
                for c_datos in (
                    select
                        case
                            when a.id_contratista is null then 'no asignadas'
                            else 'asignadas'
                        end as asignacion,
                        count(*) as noregistros
                    from aire.ord_ordenes a
                    where NVL(a.id_contratista,-1) in (
                                 select id_contratista
                                        from aire.v_ctn_contratos
                                        where UPPER(prefijo_actividad) = 'G_SCR'
                                        and UPPER(ind_activo) = 'S'
                                union all
                                select -1 from dual
                            )
                    group by
                        case
                            when a.id_contratista is null then 'no asignadas'
                            else 'asignadas'
                        end
                ) loop
                apex_json.open_object();
                    apex_json.write('asignacion', c_datos.asignacion);
                    apex_json.write('noregistros', c_datos.noregistros);
                apex_json.close_object();
                end loop;
            apex_json.close_array();
            apex_json.write('RegistrosTotales', v_RegistrosTotales);
        apex_json.close_object();
    apex_json.close_object();
    
    
    
    s_json := apex_json.get_clob_output;
    apex_json.free_output;
    
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Carga exitosa');
    
    exception
    when others then
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al crear el json: '|| sqlerrm);
        apex_json.free_output;
        apex_json.open_object();
        apex_json.write('codigo', 1);
        apex_json.write('mensaje', 'Error N '||v_id_log ||' al consultar el listado de ordenes.' || sqlerrm);
        apex_json.close_object();
        
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
    END prc_consultar_ordenes_area_central;
  
END PKG_G_CARLOS_VARGAS_TEST3;