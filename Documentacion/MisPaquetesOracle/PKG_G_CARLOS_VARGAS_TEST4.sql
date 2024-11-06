CREATE OR REPLACE PACKAGE "AIRE".PKG_G_CARLOS_VARGAS_TEST4 AS
    /*
    este sp sobreescribe pkg_g_carlos_vargas_test3.sql y pgk_g_ordenes
    */
    procedure prc_consultar_ordenes_area_central (
        e_json  in  clob,
        s_json 	out	clob
	);

    procedure prc_consultar_archivos_instancia_detalle (
        e_json  in  clob,
        s_json 	out	clob
	);

    procedure prc_consultar_ordenes_contratistas (
        e_json  in  clob,
        s_json 	out	clob
	);

END PKG_G_CARLOS_VARGAS_TEST4;
/
CREATE OR REPLACE PACKAGE BODY "AIRE".PKG_G_CARLOS_VARGAS_TEST4 AS

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
    pageNumber                      := v_ServerSide.get_number('first');
    pageSize                        := v_ServerSide.get_number('rows');

    -- DBMS_OUTPUT.PUT_LINE('v_ServerSide: ' || v_ServerSide.to_string());
    

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
    -- myDictionary('ServerSide_WhereAdd').value := case
    --                                             when LENGTH(v_filtersString) > 0 then ' and ' || v_filtersString
    --                                             else ' ' end;
    myDictionary('ServerSide_WhereAdd').value := replace(replace(myDictionary('ServerSide_WhereAdd').value,'#x#','''%'),'#y#','%''');
    myDictionary('ServerSide_SortAdd').value := case
                                                when LENGTH(v_sortString) > 0 then ' order by ' || v_sortString
                                                else ' order by id_orden ' end;
    --- -//- ServerSide Angular Table


    --DBMS_OUTPUT.PUT_LINE('e_json: ' || e_json);
    -- DBMS_OUTPUT.PUT_LINE('v_ServerSide: ' || v_ServerSide.to_string());
    -- DBMS_OUTPUT.PUT_LINE('------------------------------------------------------------');
    --DBMS_OUTPUT.PUT_LINE('ServerSide_WhereAdd: ' || myDictionary('ServerSide_WhereAdd').value);
    --DBMS_OUTPUT.PUT_LINE('ServerSide_SortAdd: ' || myDictionary('ServerSide_SortAdd').value);
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
                        select
                             a.id_orden
                            --, a.id_tipo_orden
                            , h.descripcion as tipo_orden
                            , a.numero_orden
                            --, a.id_estado_orden
                            , b.descripcion  as estado_orden
                            --, a.id_contratista id_contratista
                            , m1.nombres || '' '' || m1.apellidos as contratista
                            --, a.id_cliente
                            , d.nombre_cliente as cliente
                            --, a.id_territorial
                            , e.nombre as territorial
                            --, a.id_zona id_zona
                            , f.nombre as zona
                            , a.direcion
                            , a.fecha_creacion
                            , a.fecha_cierre as fecha_cierre
                            --, a.id_usuario_cierre as id_usuario_cierre
                            , m2.nombres || '' '' || m2.apellidos as usuario_cierre
                            --, a.id_actividad
                            , i.nombre as actividad
                            --, a.id_contratista_persona
                            , m3.nombres || '' '' || m3.apellidos as nombre_contratista_persona
                            --, a.id_tipo_trabajo as id_tipo_trabajo
                            , k.descripcion as tipo_trabajo
                            --, a.id_tipo_suspencion as id_tipo_suspencion
                            , l.descripcion as tipo_suspencion
                            , a.origen
                            --, mensaje_error_wsgr as mensaje_error_ws
                            --,''''  as mensaje_error_ws
                        from aire.ord_ordenes              				a
                        inner join aire.ord_estados_orden   			b on a.id_estado_orden   		= b.id_estado_orden
                        
                        left join aire.ctn_contratistas  				c on a.id_contratista    		= c.id_contratista
                        left join aire.gnl_personas                     m1 on c.id_persona = m1.id_persona


                        left join aire.gnl_clientes        				d on a.id_cliente        		= d.id_cliente
                        left join aire.gnl_territoriales   				e on a.id_territorial    		= e.id_territorial
                        left join aire.gnl_zonas           				f on a.id_zona           		= f.id_zona

                        left join aire.sgd_usuarios      				g on a.id_usuario_cierre 		= g.id_usuario
                        left join aire.gnl_personas             		m2 on g.id_persona = m2.id_persona


                        left join aire.ord_tipos_orden     				h on a.id_tipo_orden     		= h.id_tipo_orden
                        left join aire.gnl_actividades     				i on a.id_actividad      		= i.id_actividad

                        left join aire.ctn_contratistas_persona  		j on a.id_contratista_persona 	= j.id_contratista_persona
                        left join aire.v_gnl_personas			 	    m3 on j.id_persona     = m3.id_persona

                        left join aire.ord_tipos_trabajo 				k on a.id_tipo_trabajo 		 	= k.id_tipo_trabajo
                        left join aire.scr_tipos_suspencion 			l on a.id_tipo_suspencion 	 	= l.id_tipo_suspencion
                        /*left join (
                            select
                                x.id_orden, listagg(distinct nvl(x.mensaje_error_ws,''x''),''| '') WITHIN GROUP (ORDER BY x.id_orden) as mensaje_error_wsgr
                            from aire.ord_ordenes_gestion              x
                            group by x.id_orden
                        ) m on a.id_orden = m.id_orden */
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
                    , txcnt as (
                        SELECT sum(1) AS MaxRows
                        FROM tx
                        ' || myDictionary('ServerSide_WhereAdd').value || '
                        ' || myDictionary('ServerSide_SortAdd').value || '
                    )
                    select
                              q.id_orden
                            , q.tipo_orden
                            , q.numero_orden
                            , q.estado_orden
                            , q.contratista
                            , q.cliente
                            , q.territorial
                            , q.zona
                            , q.direcion
                            , q.fecha_creacion
                            , q.fecha_cierre
                            , q.usuario_cierre
                            , q.actividad
                            , q.nombre_contratista_persona
                            , q.tipo_trabajo
                            , q.tipo_suspencion
                            , q.origen
                            , acnt.MaxRows RegistrosTotales
                    from tx q, txcnt acnt
                    ' || myDictionary('ServerSide_WhereAdd').value || '
                    ' || myDictionary('ServerSide_SortAdd').value || '
                    OFFSET ' || pageNumber || ' ROWS FETCH NEXT ' || pageSize ||' ROWS ONLY
                ) loop
                apex_json.open_object();
                    apex_json.write(''id_orden'', c_datos.id_orden);
                    --apex_json.write(''id_tipo_orden'', c_datos.id_tipo_orden);
                    apex_json.write(''tipo_orden'', c_datos.tipo_orden);
                    apex_json.write(''numero_orden'', c_datos.numero_orden);
                    -- apex_json.write(''id_estado_orden'', c_datos.id_estado_orden);
                    apex_json.write(''estado_orden'', c_datos.estado_orden);
                    -- apex_json.write(''id_contratista'', c_datos.id_contratista);
                    apex_json.write(''contratista'', c_datos.contratista);
                    -- apex_json.write(''id_cliente'', c_datos.id_cliente);
                    apex_json.write(''cliente'', c_datos.cliente);
                    -- apex_json.write(''id_territorial'', c_datos.id_territorial);
                    apex_json.write(''territorial'', c_datos.territorial);
                    -- apex_json.write(''id_zona'', c_datos.id_zona);
                    apex_json.write(''zona'', c_datos.zona);
                    apex_json.write(''direcion'', c_datos.direcion);
                    apex_json.write(''fecha_creacion'', c_datos.fecha_creacion);
                    apex_json.write(''fecha_cierre'', c_datos.fecha_cierre);
                    -- apex_json.write(''id_usuario_cierre'', c_datos.id_usuario_cierre);
                    apex_json.write(''usuario_cierre'', c_datos.usuario_cierre);
                    -- apex_json.write(''id_actividad'', c_datos.id_actividad);
                    apex_json.write(''actividad'', c_datos.actividad);
                    -- -- apex_json.write(''id_contratista_persona'', c_datos.id_contratista_persona);
                    apex_json.write(''nombre_contratista_persona'', c_datos.nombre_contratista_persona);
                    -- apex_json.write(''id_tipo_trabajo'', c_datos.id_tipo_trabajo);
                    apex_json.write(''tipo_trabajo'', c_datos.tipo_trabajo);
                    -- apex_json.write(''id_tipo_suspencion'', c_datos.id_tipo_suspencion);
                    apex_json.write(''tipo_suspencion'', c_datos.tipo_suspencion);
                    apex_json.write(''origen'', c_datos.origen);
                    --apex_json.write(''mensaje_error_ws'', c_datos.mensaje_error_ws);
                    --v_RegistrosTotales := c_datos.RegistrosTotales;
                    BEGIN :v0 := c_datos.RegistrosTotales; END;
                apex_json.close_object();
            end loop;
            apex_json.close_array();
            END;
            ';

            -- DBMS_OUTPUT.PUT_LINE('------------------------------------------------------------');
            DBMS_OUTPUT.PUT_LINE('Query: ' || v_dinamic_sql);

            EXECUTE IMMEDIATE v_dinamic_sql
            USING OUT v_RegistrosTotales;

         -- agrupamiento asignadas y no asignadas
            apex_json.open_array('grafica_asignacion');
                /*for c_datos in (
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
                ) loop*/
                apex_json.open_object();
                    apex_json.write('asignacion', 0);
                    apex_json.write('noregistros', 0);

                    -- apex_json.write('asignacion', c_datos.asignacion);
                    -- apex_json.write('noregistros', c_datos.noregistros);
                apex_json.close_object();
                -- end loop;
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
  
    --/-- se migra a pkg_g_ordenes el 22/03/2024
    procedure prc_consultar_archivos_instancia_detalle (
        e_json  in  clob,
        s_json 	out	clob
    ) is
        --e_json  	clob :=  '{"id_archivo_instancia":0}';
        v_json_objeto           json_object_t;
        v_id_log		        aire.gnl_logs.id_log%type;
        v_respuesta		        aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_ordenes.prc_consultar_archivos_instancia_detalle');
        v_id_archivo_instancia    number;

        pageNumber          NUMBER;
        pageSize            NUMBER;
        sortColumn          VARCHAR(100 BYTE);
        sortDirection       VARCHAR(100 BYTE);
        v_RegistrosTotales  NUMBER;

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
    v_json_objeto := json_object_t(e_json);
    v_id_archivo_instancia          := v_json_objeto.get_number('id_archivo_instancia');
    -- pageNumber                      := v_json_objeto.get_number('pageNumber');
    -- pageSize                        := v_json_objeto.get_number('pageSize');
    --sortColumn              := v_json_objeto.get_string('sortColumn');
    --sortDirection              := v_json_objeto.get_string('sortDirection');

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
                                                else ' order by id_archivo_instancia_detalle asc ' end;
    
    apex_json.open_object();
        apex_json.write('codigo', 0);
        apex_json.write('mensaje', 'Consulta exitosa');
        apex_json.open_object('datos');
         -- gnl_archivos_instancia
            v_dinamic_sql := '
            DECLARE
            BEGIN
            apex_json.open_array(''archivos_instancia_detalle'');
                for c_datos in (
                    WITH t as (
                        SELECT
                              a.id_archivo_instancia_detalle
                            , a.id_archivo_instancia
                            , a.numero_fila
                            , a.estado
                            , a.observaciones
                            , a.datos
                            , a.referencia
                        FROM aire.gnl_archivos_instancia_detalle a
                        where a.id_archivo_instancia = :v_id_archivo_instancia2 '  || myDictionary('ServerSide_WhereAdd').value || '
                        ' || myDictionary('ServerSide_SortAdd').value || '
                    ),
                    t2 as (
                        select count(*) as RegistrosTotales from t
                    )
                    select
                          a.id_archivo_instancia_detalle
                        , a.id_archivo_instancia
                        , a.numero_fila
                        , a.estado
                        , replace(a.observaciones,''"'','''') as observaciones
                        , a.datos
                        , a.referencia
                        , b.RegistrosTotales
                    from t a, t2 b
                    OFFSET ' || pageNumber || ' ROWS FETCH NEXT ' || pageSize ||' ROWS ONLY
                ) loop
                apex_json.open_object();
                    apex_json.write(''id_archivo_instancia_detalle'', c_datos.id_archivo_instancia_detalle);
                    apex_json.write(''id_archivo_instancia'', c_datos.id_archivo_instancia);
                    apex_json.write(''numero_fila'', c_datos.numero_fila);
                    apex_json.write(''estado'', c_datos.estado);
                    apex_json.write(''observaciones'', c_datos.observaciones);
                    apex_json.write(''datos'', c_datos.datos);
                    apex_json.write(''referencia'', c_datos.referencia);
                    
                    BEGIN :vx_RegistrosTotales := c_datos.RegistrosTotales; END;
                apex_json.close_object();
                end loop;
            apex_json.close_array();
            END;
            ';

            EXECUTE IMMEDIATE v_dinamic_sql
            USING IN v_id_archivo_instancia, OUT v_RegistrosTotales;

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
        apex_json.write('mensaje', 'Error N '||v_id_log ||' al consultar los registros.' || sqlerrm);
        apex_json.close_object();
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
        --dbms_output.put_line('xxx: '|| sqlerrm);
    END prc_consultar_archivos_instancia_detalle;

    --/--se migra a pkg_g_ordenes el 22/03/2024
    procedure prc_consultar_ordenes_contratistas (
        e_json  in  clob,
        s_json 	out	clob
    ) is
        --e_json  	clob :=  '{"id_contratista": "2076341","id_zona": ""}';
        --v_json      clob;
        v_json_objeto           json_object_t;
        v_id_log		        aire.gnl_logs.id_log%type;
        v_respuesta		        aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_ordenes.prc_consultar_ordenes_contratistas');
        v_objeto      	        aire.ord_ordenes%rowtype;
        v_id_persona            VARCHAR2(50 BYTE);
        v_id_usuario            VARCHAR2(50 BYTE);
        v_id_orden              NUMBER;
        pageNumber              NUMBER;
        pageSize                NUMBER;
        sortColumn              VARCHAR(100 BYTE);
        sortDirection           VARCHAR(100 BYTE);
        v_RegistrosTotales      NUMBER;
        v_id_estado_orden       aire.ord_estados_orden.id_estado_orden%type;
        v_codigo_estado         aire.ord_estados_orden.codigo_estado%type;
        v_codigo_suspencion     varchar2(10);
        v_id_tipo_suspencion    aire.scr_tipos_suspencion.id_tipo_suspencion%type;

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
    -- v_objeto.id_contratista         := v_json_objeto.get_string('id_contratista');
    v_id_persona                    := v_json_objeto.get_string('id_persona');
    v_id_usuario                    := v_json_objeto.get_string('id_usuario');
    select
        nvl(a.id_contratista,'-1') into v_objeto.id_contratista
    from aire.ctn_contratistas_persona		a
    where a.id_contratista in (
        select
            id_contratista
        from aire.v_ctn_contratos
        where UPPER(prefijo_actividad) = 'G_SCR'
        and UPPER(ind_activo) = 'S'
    )
    and a.id_persona = v_id_persona and a.codigo_rol = 'ANALISTA';


    v_objeto.id_contratista_persona := v_json_objeto.get_string('id_contratista_persona');

    v_objeto.id_estado_orden        := v_json_objeto.get_string('id_estado_orden');
    v_codigo_estado                 := v_json_objeto.get_string('codigo_estado');
    v_codigo_suspencion             := v_json_objeto.get_string('codigo_suspencion');

    v_objeto.id_zona                := v_json_objeto.get_string('id_zona');
    v_id_orden                      := v_json_objeto.get_number('id_orden');
    
    -- pageNumber                      := v_json_objeto.get_number('pageNumber');
    -- pageSize                        := v_json_objeto.get_number('pageSize');
    --sortColumn              := v_json_objeto.get_string('sortColumn');
    --sortDirection              := v_json_objeto.get_string('sortDirection');

    v_ServerSide                    := v_json_objeto.get_Object('ServerSideJson');
    v_filtersString                 := v_ServerSide.get_string('filtersString');
    v_sortString                    := v_ServerSide.get_string('sortString');
    pageNumber                      := v_ServerSide.get_number('first');
    pageSize                        := v_ServerSide.get_number('rows');

    --DBMS_OUTPUT.PUT_LINE('v_ServerSide: ' || v_ServerSide.to_string());
    DECLARE
    BEGIN
        --obtener el id tipo suspencion
        --obtener el id tipo suspencion
        SELECT
            NVL(e.id_tipo_suspencion,0)
                into v_id_tipo_suspencion
        FROM aire.scr_tipos_suspencion e
        where e.id_actividad in (select id_actividad from aire.gnl_actividades where prefijo = 'G_SCR' and ind_activo = 'S')
                and e.codigo = v_codigo_suspencion;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            v_id_tipo_suspencion := 0; -- Asignar un valor por defecto cuando no se encuentra ningún resultado
    END;

    if v_codigo_suspencion = '-1' or v_codigo_suspencion = '-2' then
        v_id_tipo_suspencion := to_number(v_codigo_suspencion);
    end if;

    --- -//-->INICIO DINAMIC SQL 26/02/2024
    --- -//- Filtros Staticos
    myDictionary('id_orden').value := case when v_id_orden > 0 then ' and a.id_orden = ' || v_id_orden else ' ' end;
    myDictionary('id_contratista_persona').value := case
                                                when v_id_orden > 0 then ' '
                                                when v_objeto.id_contratista_persona = -1 then ' and a.id_contratista_persona is null '
                                                when v_objeto.id_contratista_persona = -2 then ' ' --es todo!
                                                when v_objeto.id_contratista_persona > 0 then ' and a.id_contratista_persona = ' || v_objeto.id_contratista_persona || ' '
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
    myDictionary('id_tipo_suspencion').value := case
                                                when v_id_orden > 0 then ' '
                                                when v_id_tipo_suspencion = -1 then ' and a.id_tipo_suspencion is null '
                                                when v_id_tipo_suspencion = -2 then ' ' --es todo!
                                                when v_id_tipo_suspencion > 0 then ' and a.id_tipo_suspencion = ' || v_id_tipo_suspencion || ' '
                                            else ' ' end;

    myDictionary('ServerSide_WhereAdd').value := case
                                                when LENGTH(v_filtersString) > 0 then ' where ' || v_filtersString
                                                else ' ' end;
    -- myDictionary('ServerSide_WhereAdd').value := case
    --                                             when LENGTH(v_filtersString) > 0 then ' and ' || v_filtersString
    --                                             else ' ' end;
    myDictionary('ServerSide_WhereAdd').value := replace(replace(myDictionary('ServerSide_WhereAdd').value,'#x#','''%'),'#y#','%''');
    myDictionary('ServerSide_SortAdd').value := case
                                                when LENGTH(v_sortString) > 0 then ' order by ' || v_sortString
                                                else ' order by id_orden ' end;
    --- -//- ServerSide Angular Table

        
    apex_json.open_object();
        apex_json.write('codigo', 0);
        apex_json.write('mensaje', 'Consulta exitosa');
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
                             , a.id_contratista id_contratista
                             , c.nombre_completo as contratista
                             , a.id_cliente
                             , d.nombre_cliente as cliente
                             , a.id_territorial
                             , e.nombre as territorial
                             , a.id_zona id_zona
                             , f.nombre as zona
                             , a.direcion
                             , a.fecha_registro as fecha_creacion
                             , NVL(a.fecha_cierre,TO_TIMESTAMP(''1900/01/01 12:01:01,000000000 AM'', ''YYYY/MM/DD HH:MI:SS,FF9 AM'')) as fecha_cierre
                             , a.id_usuario_cierre as id_usuario_cierre
                             , g.nombres as usuario_cierre
                             , a.id_actividad
                             , i.nombre as actividad
                             , a.id_contratista_persona
                             , j.nombre_contratista_persona
                             , a.id_tipo_trabajo as id_tipo_trabajo
                             , k.descripcion as tipo_trabajo
                             , a.id_tipo_suspencion as id_tipo_suspencion
                             , l.descripcion as tipo_suspencion
                             , a.Origen
                        from aire.ord_ordenes              				a
                        inner join aire.ord_estados_orden   				b on a.id_estado_orden   		= b.id_estado_orden
                        left join aire.v_ctn_contratistas  				c on a.id_contratista    		= c.id_contratista
                        left join aire.gnl_clientes        				d on a.id_cliente        		= d.id_cliente
                        left join aire.gnl_territoriales   				e on a.id_territorial    		= e.id_territorial
                        left join aire.gnl_zonas           				f on a.id_zona           		= f.id_zona
                        inner join aire.sgd_usuarios_zona uz on uz.id_zona = f.id_zona and uz.id_usuario = '||v_id_usuario||'
                        left join aire.v_sgd_usuarios      				g on a.id_usuario_cierre 		= g.id_usuario
                        left join aire.ord_tipos_orden     				h on a.id_tipo_orden     		= h.id_tipo_orden
                        left join aire.gnl_actividades     				i on a.id_actividad      		= i.id_actividad
                        left join aire.v_ctn_contratistas_persona  		j on a.id_contratista_persona 	= j.id_contratista_persona
                        left join aire.ord_tipos_trabajo 				k on a.id_tipo_trabajo 		 	= k.id_tipo_trabajo
                        left join aire.scr_tipos_suspencion 			l on a.id_tipo_suspencion 	 	= l.id_tipo_suspencion
                        where
                            --b.codigo_estado NOT IN (''CERR'',''SCOM'',''SEJE'') and
                            NVL(a.id_contratista,-1) in (
                                 select id_contratista
                                        from aire.v_ctn_contratos
                                        where UPPER(prefijo_actividad) = ''G_SCR''
                                        and UPPER(ind_activo) = ''S''
                                union all
                                select -1 from dual
                            ) and a.id_contratista = '||v_objeto.id_contratista||'
                            ' || myDictionary('id_orden').value || '
                            ' || myDictionary('id_contratista_persona').value || '
                            ' || myDictionary('id_zona').value || '
                            ' || myDictionary('id_tipo_suspencion').value || '
                            ' || myDictionary('codigo_estado').value || '
                    )
                    select
                          a.*
                        , SUM(1) OVER() RegistrosTotales
                    from tx a
                    ' || myDictionary('ServerSide_WhereAdd').value || '
                    ' || myDictionary('ServerSide_SortAdd').value || '
                    OFFSET ' || pageNumber || ' ROWS FETCH NEXT ' || pageSize ||' ROWS ONLY
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
                    BEGIN :v1 := c_datos.RegistrosTotales; END;
                apex_json.close_object();
            end loop;
            apex_json.close_array();
            END;
            ';

            -- DBMS_OUTPUT.PUT_LINE('------------------------------------------------------------');
            DBMS_OUTPUT.PUT_LINE('Query: ' || v_dinamic_sql);

            EXECUTE IMMEDIATE v_dinamic_sql
            USING OUT v_RegistrosTotales;

         -- agrupamiento asignadas y no asignadas
            apex_json.open_array('grafica_asignacion');
                for c_datos in (
                    select
                        case
                            when a.id_contratista_persona is null then 'no asignadas'
                            else 'asignadas'
                        end as asignacion,
                        count(*) as noregistros
                    from aire.ord_ordenes a
                    where a.id_contratista = v_objeto.id_contratista
                    group by
                        case
                            when a.id_contratista_persona is null then 'no asignadas'
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
        apex_json.write('mensaje', 'Error N '||v_id_log ||' al consultar el listado de ordenes.' || replace(replace(replace(sqlerrm,'"',''''),chr(13),''),chr(10),''));
        apex_json.close_object();
        
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
    END prc_consultar_ordenes_contratistas;

    --Ultima Modificacion: 21/03/2024 14:55 pm carlos vargas

END PKG_G_CARLOS_VARGAS_TEST4;