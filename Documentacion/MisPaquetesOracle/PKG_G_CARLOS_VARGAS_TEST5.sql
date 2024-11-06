CREATE OR REPLACE PACKAGE "AIRE".PKG_G_CARLOS_VARGAS_TEST5 AS
    /*
    este sp sobreescribe pkg_g_carlos_vargas_test4.sql y pkg_g_carlos_vargas_test3.sql y pgk_g_ordenes
    */
    procedure prc_consultar_ordenes_area_central (
        e_json  in  clob,
        s_json 	out	clob
	);

    /*
    este sp sobreescribe pgk_g_ordenes
    */
    procedure prc_registrar_orden2 (
        e_json  in 	clob,
        s_json 	out	clob
	);

    /*
    este NO sobreescribe sp en pgk_g_ordenes
    debido a que se crea uno nuevo, la versión V2
    */
    procedure prc_registro_ordenes_masivo_final_V2 (
        e_json      in clob,
        s_json      out clob
	);

    /*
    este sp sobreescribe pgk_g_ordenes
    */
    procedure prc_consulta_agrupada_ordenes_area_central (
        e_json  in  clob,
        s_json 	out	clob
	);

    /*
    este sp sobreescribe pgk_g_ordenes
    */
    procedure prc_desasignar_ordenes_masivo_tecnico (
        e_json      in clob,
        s_json      out clob
	);

END PKG_G_CARLOS_VARGAS_TEST5;
/
CREATE OR REPLACE PACKAGE BODY "AIRE".PKG_G_CARLOS_VARGAS_TEST5 AS

  --/-- se migra a pkg_g_ordenes el 22/03/2024
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

        v_list_contratistas varchar2(4000);
        v_list_zonas varchar2(4000);
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
    
    --- Hallar los contratistas y zonas definidas por contrato en scr.
    select 
         LISTAGG(distinct x.id_contratista, ', ') WITHIN GROUP (ORDER BY x.id_contratista) as id_contratista
        ,LISTAGG(distinct c.id_zona, ', ') WITHIN GROUP (ORDER BY c.id_zona) as id_zona
            into v_list_contratistas, v_list_zonas
    from aire.v_ctn_contratistas x
    inner join (
        select id_contratista,id_contrato
        from aire.v_ctn_contratos
        where UPPER(prefijo_actividad) = 'G_SCR'
        and UPPER(ind_activo) = 'S'
    ) b on b.id_contratista = x.id_contratista
    inner join aire.ctn_contratos_zona c on c.id_contrato = b.id_contrato
    where UPPER(x.ind_activo) = 'S' and
        case 
            when v_objeto.id_contratista = -2 and (x.id_contratista is null or x.id_contratista is not null) then 1
            when v_objeto.id_contratista = -1 and x.id_contratista is null then 1
            when v_objeto.id_contratista > 0 and x.id_contratista = v_objeto.id_contratista then 1
            else 0 end = 1;


    -- DBMS_OUTPUT.PUT_LINE('v_objeto.id_contratista: ' || v_objeto.id_contratista);
    -- DBMS_OUTPUT.PUT_LINE('v_objeto.id_zona: ' || v_objeto.id_zona);
    -- DBMS_OUTPUT.PUT_LINE('v_list_contratistas: ' || v_list_contratistas);
    -- DBMS_OUTPUT.PUT_LINE('v_list_zonas: ' || v_list_zonas);


    --- -//-->INICIO DINAMIC SQL 26/02/2024
    --- -//- Filtros Staticos
    myDictionary('id_orden').value := case when v_id_orden > 0 then ' and a.id_orden = ' || v_id_orden else ' ' end;
    -- myDictionary('id_contratista').value := case
    --                                             when v_id_orden > 0 then ' '
    --                                             when v_objeto.id_contratista = -1 then ' and a.id_contratista is null '
    --                                             when v_objeto.id_contratista = -2 then ' ' --es todo!
    --                                             when v_objeto.id_contratista > 0 then ' and a.id_contratista = ' || v_objeto.id_contratista || ' '
    --                                         else ' ' end;

    -- myDictionary('id_zona').value := case
    --                                             when v_id_orden > 0 then ' '
    --                                             when v_objeto.id_zona = -1 then ' and a.id_zona is null '
    --                                             when v_objeto.id_zona = -2 then ' ' --es todo!
    --                                             when v_objeto.id_zona > 0 then ' and a.id_zona = ' || v_objeto.id_zona || ' '
    --                                         else ' ' end;

    myDictionary('id_contratista').value := case
                                                when v_id_orden > 0 then ' '
                                                when v_objeto.id_contratista = -1 then ' and a.id_contratista is null '
                                                --/ si contratista es todos y zona se eligio una, filtrar contratistas de esa zona.
                                                when v_objeto.id_contratista = -2 then ' and (a.id_contratista in (' || v_list_contratistas || ') or a.id_contratista is null) ' --es todo!
                                                when v_objeto.id_contratista > 0 then ' and a.id_contratista = ' || v_objeto.id_contratista || ' '
                                            else ' ' end;
    myDictionary('id_zona').value := case
                                                when v_id_orden > 0 then ' '
                                                when v_objeto.id_zona = -1 then ' and a.id_zona is null '                                                
                                                when v_objeto.id_zona = -2 then ' and (a.id_zona in (' || v_list_zonas || ') or a.id_zona is null) ' --es todo!
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

    

    -- DBMS_OUTPUT.PUT_LINE('ok');
    -- DBMS_OUTPUT.PUT_LINE('myDictionary(''id_contratista'').value: ' || myDictionary('id_contratista').value);
    -- DBMS_OUTPUT.PUT_LINE('myDictionary(''id_zona'').value: ' || myDictionary('id_zona').value);
    
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
                            , a.id_tipo_orden
                            , h.descripcion as tipo_orden
                            , a.numero_orden
                            --, a.id_estado_orden
                            , b.descripcion  as estado_orden
                            --, a.id_contratista id_contratista
                            , case when nvl(m1.nombres,''-1'') = ''-1'' then null else m1.nombres || '' '' || m1.apellidos || ''_'' || a.id_contratista end as contratista
                            --, a.id_cliente
                            , d.nombre_cliente as cliente
                            --, a.id_territorial
                            , e.nombre as territorial
                            --, a.id_zona id_zona
                            , f.nombre || ''_'' || a.id_zona as zona
                            , a.direcion
                            , a.fecha_registro as fecha_creacion
                            , a.fecha_cierre as fecha_cierre
                            --, a.id_usuario_cierre as id_usuario_cierre
                            , case when nvl(m2.nombres,''-1'') = ''-1'' then null else m2.nombres || '' '' || m2.apellidos end as usuario_cierre
                            --, a.id_actividad
                            , i.nombre as actividad
                            --, a.id_contratista_persona
                            , case when nvl(m3.nombres,''-1'') = ''-1'' then null else m3.nombres || '' '' || m3.apellidos end as nombre_contratista_persona
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
                            b.codigo_estado != ''CERR'' and
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
                            , q.id_tipo_orden
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
                    apex_json.write(''id_tipo_orden'', c_datos.id_tipo_orden);
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
            -- DBMS_OUTPUT.PUT_LINE('Query: ' || v_dinamic_sql);

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

    procedure prc_registrar_orden2 (
        e_json  		in 	clob,
        s_json 	out	clob
    ) is
        v_json_orden                json_object_t;
        v_id_log		            aire.gnl_logs.id_log%type;
        v_respuesta                 aire.tip_respuesta := aire.tip_respuesta(mensaje => 'Registro exitoso.', nombre_up => 'pkg_g_ordenes.prc_registrar_orden2');
        v_orden      	            aire.ord_ordenes%rowtype;
        v_id_contratista            aire.ord_ordenes.id_contratista%type;
        v_estado_orden              aire.ord_estados_orden.descripcion%type;
        c_orden                     aire.gnl_clientes%rowtype;
        valor_nic                   number;
        valor_nis                   number;
        valor_id_estado_servicio    number;
        v_id_actividad              aire.gnl_actividades.id_actividad%type;
        v_mje                       varchar2(4000) := 'Orden creada con éxito.';
        v_exist_contratista           number;
    begin
        -- validamos el Json de la orden y armamos el rowtype
        begin
            -- parseamos el json
            v_json_orden := json_object_t(e_json);
    
            -- extraemos los datos de la orden
            c_orden.nic                     := v_json_orden.get_number('nic');
            c_orden.nis                     := v_json_orden.get_number('nis');
            valor_nic                       := c_orden.nic;
            valor_nis                       := c_orden.nis;
            
            v_orden.id_tipo_orden           := v_json_orden.get_number('id_tipo_orden');
            valor_id_estado_servicio        := v_json_orden.get_number('id_estado_servicio');
            v_orden.id_tipo_suspencion      := v_json_orden.get_number('id_tipo_suspencion');
            v_id_contratista                := v_json_orden.get_number('id_contratista');
            
            --se valida que exista el contratista y que sea de SCR.
            if v_id_contratista > 0 then
                select
                     count(*)
                        into v_exist_contratista
                from aire.v_ctn_contratistas
                where id_contratista in (
                        select id_contratista
                            from aire.v_ctn_contratos
                            where UPPER(prefijo_actividad) = 'G_SCR'
                            and UPPER(ind_activo) = 'S'
                ) and id_contratista = v_id_contratista;
                
                -- Retornar error
                if v_exist_contratista = 0 then
                    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error el contratista no existe: ');
                    apex_json.initialize_clob_output( p_preserve => false );
                    apex_json.open_object();
                    apex_json.write('codigo', 1);
                    apex_json.write('mensaje', 'Error #'||v_id_log||' el contratista no existe por cuanto no se crea ninguna orden');
                    apex_json.close_object();
                    s_json := apex_json.get_clob_output;
                    apex_json.free_output;
                    return;
                end if;
            end if;
            
            select
                aire.sec__ord_ordenes_cargue_temporal__numero_orden.NEXTVAL * -1
                    into v_orden.numero_orden
            from dual;
            
            select id_actividad
                    into v_id_actividad
            from aire.gnl_actividades where prefijo = 'G_SCR' and ind_activo = 'S';
            
            /*
            24-01-2024
            Se obtiene el valor del estado asignar
            12-03-2024
            ahora se añade el id contratista, si viene en el json entonces
            se utiliza en estado: Asignada a Contratista
            */
            SELECT
                id_estado_orden,descripcion
                    into v_orden.id_estado_orden, v_estado_orden
            FROM aire.ord_estados_orden
            where   codigo_estado = case when v_id_contratista <= 0 then 'SPEN' else 'SASI' end
            and     id_actividad  = v_id_actividad;
            
            
            select
                 id_cliente
                ,id_territorial
                ,id_zona
                ,direccion
                into
                     v_orden.id_cliente
                    ,v_orden.id_territorial
                    ,v_orden.id_zona
                    ,v_orden.direcion
            from aire.gnl_clientes
            where nic = valor_nic;
            
            
            --Se realiza insert a tabla aire.ord_ordenes
            insert into aire.ord_ordenes(
                id_tipo_orden
                ,id_cliente
                ,id_estado_orden
                ,numero_orden
                ,id_actividad
                ,id_tipo_suspencion
                ,id_territorial
                ,id_zona
                ,direcion
                ,origen
                ,id_contratista
            )
            values(
                 v_orden.id_tipo_orden
                ,v_orden.id_cliente
                ,v_orden.id_estado_orden
                ,v_orden.numero_orden
                ,v_id_actividad
                ,v_orden.id_tipo_suspencion
                ,v_orden.id_territorial
                ,v_orden.id_zona
                ,v_orden.direcion
                ,'OP360'
                ,case when v_id_contratista > 0 then v_id_contratista else null end
            );
            commit;
            
            select
                id_orden
                into v_orden.id_orden
            from aire.ord_ordenes
            where numero_orden = v_orden.numero_orden;
        exception
            when others then

                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al procesar el json de la orden: '|| sqlerrm);
                apex_json.initialize_clob_output( p_preserve => false );
                apex_json.open_object();
                apex_json.write('codigo', 1);
                apex_json.write('mensaje', 'Error #'||v_id_log||' al registrar orden'|| sqlerrm);
                apex_json.close_object();
                s_json := apex_json.get_clob_output;
                apex_json.free_output;
                commit;
                return;
        end;
        
        if v_id_contratista > 0 then
                v_mje := 'Registro creado correctamente con el id_orden: ' || v_orden.id_orden || ', Se asigno al contratista: ' || v_id_contratista || ', y la orden quedo en estado: ' || v_estado_orden;
            else
                v_mje := 'Registro creado correctamente con el id_orden: ' || v_orden.id_orden || ', no se asigno a ningun contratista! , y la orden quedo en estado: ' || v_estado_orden;
        end if;

        apex_json.initialize_clob_output( p_preserve => false );
        apex_json.open_object();
        apex_json.write('codigo', v_orden.id_orden);
        apex_json.write('mensaje', v_mje);
        apex_json.open_object('datos');
        apex_json.write('id_orden',v_orden.id_orden);
        apex_json.close_object();
        apex_json.close_object();
        s_json := apex_json.get_clob_output;
        apex_json.free_output;

    exception
        when others then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al registrar orden: '|| sqlerrm);
            apex_json.initialize_clob_output( p_preserve => false );
            apex_json.open_object();
            apex_json.write('codigo', 1);
            apex_json.write('mensaje', 'Error #'||v_id_log||' al registrar orden' || sqlerrm);
            apex_json.close_object();
            s_json := apex_json.get_clob_output;
            apex_json.free_output;
            commit;
    end prc_registrar_orden2;

    procedure prc_registro_ordenes_masivo_final_V2 (
        e_json  		in 	clob,
        s_json 	        out	clob
    ) is
        v_json_orden                json_object_t;
        v_id_log		            aire.gnl_logs.id_log%type;
        v_respuesta                 aire.tip_respuesta := aire.tip_respuesta(mensaje => 'Registro exitoso.', nombre_up => 'pkg_g_ordenes.prc_registro_ordenes_masivo_final');
        
        v_id_soporte                NUMBER;
        v_usuario_registra          VARCHAR2(50 BYTE);
        v_nombre_archivo            VARCHAR2(100 BYTE);
        
        --v_numero_orden              NUMBER;
        v_id_archivo                NUMBER;
        v_id_actividad              NUMBER;
        v_id_archivo_instancia      NUMBER;
        v_fechainicio               timestamp;
        v_id_estado_orden           NUMBER;
    begin
        ----e_json := '{"id_soporte":1720,"usuario_registra",362,"nombre_archivo":"minombre.xlsx"}';
        -- validamos el Json de la orden y armamos el rowtype
        begin
            -- parseamos el json
            v_json_orden := json_object_t(e_json);
    
            -- extraemos los datos de la orden
            v_id_soporte               := v_json_orden.get_number('id_soporte');
            v_usuario_registra         := v_json_orden.get_string('usuario_registra');
            v_nombre_archivo           := v_json_orden.get_string('nombre_archivo');
            
            --actualizar tabla temporal  aire.ord_ordenes_cargue_temporal
            UPDATE aire.ord_ordenes_cargue_temporal
            SET Numero_Orden = aire.sec__ord_ordenes_cargue_temporal__numero_orden.NEXTVAL * -1
            where Numero_Orden is null and id_soporte = v_id_soporte and usuario_registra = v_usuario_registra;


            SELECT
                 id_archivo into v_id_archivo
            FROM aire.gnl_archivos x
            WHERE x.codigo = 'OTAC';
           
            SELECT
                id_actividad into v_id_actividad
            FROM aire.gnl_actividades x
            WHERE x.prefijo = 'G_SCR' and x.ind_activo = 'S';
            
            SELECT
                min(fecha_registra) into v_fechainicio
            FROM aire.ord_ordenes_cargue_temporal
            where id_soporte = v_id_soporte;
        exception
            when others then
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al procesar el json de la orden: '|| sqlerrm);
                apex_json.initialize_clob_output( p_preserve => false );
                apex_json.open_object();
                apex_json.write('codigo', 400);
                apex_json.write('mensaje', 'Error #'||v_id_log||' al registrar las ordenes masivas'|| sqlerrm);
                apex_json.close_object();
                s_json := apex_json.get_clob_output;
                apex_json.free_output;
                return;
        end;
        
        --ACA EL CODIGO PRINCIPAL
            MERGE
            INTO    aire.ord_ordenes_cargue_temporal trg
            USING   (
                    SELECT
                         x.Numero_Orden
                         
                        ,x.nic
                        --//--validar si el nic es numerico
                        ,case when REGEXP_LIKE(x.nic, '^[[:digit:]]+$') then 0 else 1 end                                       vnic
                        ,(select nvl(sum(0),1) from aire.gnl_clientes where nic = x.nic)                                        enic
                        
                        ,x.codigo_tipo_orden
                        --//--validar si el tipo orden es texto
                        ,case when REGEXP_LIKE(x.codigo_tipo_orden, '^[[:digit:]]+$') then 1 else 0 end                         vtor
                        ,(select nvl(sum(0),1) from aire.ord_tipos_orden where codigo_tipo_orden = x.codigo_tipo_orden)         etor
                        
                        ,x.codigo_tipo_suspencion
                        --//--validar si el tipo suspensión es texto
                        ,case when REGEXP_LIKE(x.codigo_tipo_suspencion, '^[[:digit:]]+$') then 1 else 0 end                    vtsu
                        ,(select nvl(sum(0),1) from aire.scr_tipos_suspencion where codigo = x.codigo_tipo_suspencion)          etsu
                        
                        ,x.codigo_estado_servicio
                        --//--validar si el tipo suspensión es texto
                        ,case when REGEXP_LIKE(x.codigo_estado_servicio, '^[[:digit:]]+$') then 1 else 0 end                    vtes
                        ,(select nvl(sum(0),1) from aire.gnl_estados_servicio where codigo = x.codigo_estado_servicio)          etes
                    FROM aire.ord_ordenes_cargue_temporal x
                    WHERE x.id_soporte = v_id_soporte and x.usuario_registra = v_usuario_registra
                    --WHERE x.id_soporte = 1720 and x.usuario_registra = '362'
                    order by 1
                    ) src
            ON      (trg.Numero_Orden = src.Numero_Orden)
            WHEN MATCHED THEN UPDATE
                SET
                    trg.con_errores = case when ((vnic + vtor) + (enic + etor) + (vtsu + etsu) + (vtes + etes)) = 0 then 0 else 1 end,
                    trg.desc_validacion =
                                case when ((vnic + vtor) + (enic + etor) + (vtsu + etsu) + (vtes + etes)) <= 0 then 'Validación Exitosa' end ||
                                case when (vnic + vtor + vtsu + vtes) > 0 then '-Tipo de dato incorrecto para: ' end    || case when src.vnic = 1 then '*-nic' end
                                                                                                                        || case when src.vtor = 1 then ' *-tipo_orden' end
                                                                                                                        || case when src.vtsu = 1 then ' *-tipo_suspencion' end
                                                                                                                        || case when src.vtes = 1 then ' *-estado_servicio' end
                                || case when (enic + etor + etsu + etes) > 0 then ' -No se encontraron los registros para: ' end    || case when src.enic = 1 then '*-nic' end
                                                                                                                                    || case when src.etor = 1 then ' *-tipo_orden' end
                                                                                                                                    || case when src.etsu = 1 then ' *-tipo_suspencion' end
                                                                                                                                    || case when src.etes = 1 then ' *-estado_servicio' end;
        
        /*
        24-01-2024
        Se obtiene el valor del estado asignar
        */
        SELECT
            id_estado_orden into v_id_estado_orden
        FROM aire.ord_estados_orden
        where   codigo_estado = 'SPEN'
        and     id_actividad IN (select id_actividad from aire.gnl_actividades where prefijo = 'G_SCR' and ind_activo = 'S');
        
        --Crear registros de ordenes masivamente.
            insert into aire.ord_ordenes
            (
                id_tipo_orden
                ,id_cliente
                ,id_estado_orden
                ,numero_orden
                ,id_actividad
                ,id_tipo_suspencion
                ,origen
                ,id_territorial
                ,id_zona
                ,direcion
            )
            select
                 --x.id_tipo_orden
                 t.id_tipo_orden
                ,c.id_cliente
                ,v_id_estado_orden id_estado_orden
                ,x.numero_orden numero_orden
                ,v_id_actividad
                --,x.id_tipo_suspencion
                ,s.id_tipo_suspencion
                ,'OP360'
                ,c.id_territorial
                ,c.id_zona
                ,c.direccion
            from aire.ord_ordenes_cargue_temporal x
            left join aire.gnl_clientes c on c.nic = x.nic
            left join aire.ord_tipos_orden t on t.codigo_tipo_orden = x.codigo_tipo_orden
            left join aire.scr_tipos_suspencion s on s.codigo = x.codigo_tipo_suspencion
            WHERE x.id_soporte = v_id_soporte and x.usuario_registra = v_usuario_registra and x.con_errores = 0;
            
            insert into aire.ord_ordenes_soporte
            (id_orden,id_soporte)
            select
                 x.id_orden
                , v_id_soporte id_soporte
            from aire.ord_ordenes x
            inner join aire.ord_ordenes_cargue_temporal tmp
                                on  tmp.numero_orden = x.numero_orden
                                and tmp.id_soporte = v_id_soporte
                                and tmp.usuario_registra = v_usuario_registra
                                and tmp.con_errores = 0;
            
        --Alimentar el log.
        --tabla principal
            INSERT INTO aire.gnl_archivos_instancia
            (
                --id_archivo_instancia,
                id_archivo,
                nombre_archivo,
                numero_registros_archivo,
                numero_registros_procesados,
                numero_errores,
                fecha_inicio_cargue,
                fecha_fin_cargue,
                duracion,
                id_usuario_registro,
                fecha_registro,
                id_estado_intancia,
                observaciones,
                id_soporte
            )
            SELECT
                 --x.id_archivo_instancia
                 v_id_archivo id_archivo
                ,v_nombre_archivo nombre_archivo
                ,SUM(1) numero_registros_archivo
                ,SUM(case when con_errores = 0 then 1 else 0 end) numero_registros_procesados
                ,SUM(con_errores) numero_errores
                ,v_fechainicio fecha_inicio_cargue
                ,localtimestamp fecha_fin_cargue
                ,'0' duracion
                ,v_usuario_registra id_usuario_registro
                ,localtimestamp fecha_registro
                ,163 id_estado_intancia --Finalizado
                ,'se cargaron ' || SUM(case when con_errores = 0 then 1 else 0 end) || ' ordenes.' observaciones
                ,v_id_soporte id_soporte
            FROM aire.ord_ordenes_cargue_temporal x
            where id_soporte = v_id_soporte and usuario_registra = v_usuario_registra
            group by id_soporte,usuario_registra;
        
        --se obtiene el id de la tabla principal
            select id_archivo_instancia into v_id_archivo_instancia
            from aire.gnl_archivos_instancia
            where id_soporte = v_id_soporte;
        
        --se alimenta la tabla detalle
            INSERT INTO aire.gnl_archivos_instancia_detalle
            (
            --id_archivo_instancia_detalle
             id_archivo_instancia
            ,numero_fila
            ,estado
            ,observaciones
            )
            SELECT
                 --id_archivo_instancia_detalle
                v_id_archivo_instancia id_archivo_instancia
                ,rownum numero_fila
                ,case when x.con_errores = 1 then 'ERROR' else 'OK' end estado
                ,x.desc_validacion observaciones
            FROM aire.ord_ordenes_cargue_temporal x
            where id_soporte = v_id_soporte and usuario_registra = v_usuario_registra;
            
        --se actualiza la fecha final
        
        UPDATE aire.gnl_archivos_instancia
        SET fecha_fin_cargue = localtimestamp, duracion = localtimestamp - fecha_inicio_cargue
        WHERE id_soporte = v_id_soporte and id_usuario_registro = v_usuario_registra and id_archivo_instancia = v_id_archivo_instancia;
        
        --se eliminan los registros de la tabla temporal aire.ord_ordenes_cargue_temporal
            DELETE aire.ord_ordenes_cargue_temporal
            WHERE id_soporte = v_id_soporte and usuario_registra = v_usuario_registra;
        
        commit;
        
        --------------------------------------------------------------------------------------
        apex_json.initialize_clob_output( p_preserve => false );
        apex_json.open_object();
        apex_json.write('codigo', 200);
        apex_json.write('mensaje', 'Ordenes masivas creadas con éxito.');
        apex_json.open_object('datos');
            apex_json.open_array('ordenes');
                for c_datos in (
                    select id_orden
                    from aire.ord_ordenes_soporte
                    where id_soporte = v_id_soporte
                ) loop
                apex_json.open_object();
                    apex_json.write('id_ordenes',c_datos.id_orden);
                apex_json.close_object();
                end loop;
            apex_json.close_array();
        apex_json.close_object();
        apex_json.close_object();
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
        
    exception
        when others then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al registrar orden: '|| sqlerrm);
            apex_json.initialize_clob_output( p_preserve => false );
            apex_json.open_object();
            apex_json.write('codigo', 400);
            apex_json.write('mensaje', 'Error #'||v_id_log||' al registrar las ordenas masivas' || sqlerrm);
            apex_json.close_object();
            s_json := apex_json.get_clob_output;
            apex_json.free_output;
    end prc_registro_ordenes_masivo_final_V2;

    --/--se migra a pkg_g_ordenes el 22/03/2024
    procedure prc_consulta_agrupada_ordenes_area_central (
        e_json  in  clob,
        s_json 	out	clob
    ) is
        --e_json  	clob :=  '{"id_contratista": "2076341"}';
        v_json_objeto    json_object_t;
        v_id_log		aire.gnl_logs.id_log%type;
        v_respuesta		aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_ordenes.prc_consulta_agrupada_ordenes_area_central');
        v_objeto      	aire.ord_ordenes%rowtype;
    BEGIN
    
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
    apex_json.free_output;
    apex_json.initialize_clob_output( p_preserve => false );
    -- dbms_output.put_line('A: ' || e_json);
    -- validamos el Json de la orden y armamos el rowtype
    begin
        -- parseamos el json
        v_json_objeto := json_object_t(e_json);
        
        -- extraemos los datos de la orden
        v_objeto.id_contratista    := v_json_objeto.get_string('id_contratista');
        --v_objeto.id_zona           := v_json_objeto.get_string('id_zona');
    exception
        when others then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al procesar el json de la orden: '|| sqlerrm);
            apex_json.initialize_clob_output( p_preserve => false );
            apex_json.open_object();
            apex_json.write('codigo', 1);
            apex_json.write('mensaje', 'Error #'||v_id_log||' en los parametros de entrada');
            apex_json.close_object();
            s_json := apex_json.get_clob_output;
            apex_json.free_output;
            return;
    end;
    
    apex_json.open_object();
        apex_json.write('codigo', 0);
        apex_json.write('mensaje', 'Consulta exitosa');
        apex_json.open_object('datos');
        -- ordenes agrupadas
            apex_json.open_array('ordenes_agrupadas');
                for c_datos in (
                    select
                          NVL(a.id_contratista,-1) as id_contratista
                        , NVL(c.nombre_completo,'-') as contratista
                        , NVL(c.identificacion,'-') as identificacion
                        , Count(*) NoRegistros
                    from aire.ord_ordenes               a
                    left join aire.v_ctn_contratistas   c on a.id_contratista    		= c.id_contratista
                    inner join aire.ord_estados_orden   b on a.id_estado_orden   		= b.id_estado_orden
                    where b.codigo_estado != 'CERR' and UPPER(c.ind_activo) = 'S' and
                                NVL(a.id_contratista,-1) in (
                                 select id_contratista
                                        from aire.v_ctn_contratos
                                        where UPPER(prefijo_actividad) = 'G_SCR'
                                        and UPPER(ind_activo) = 'S'
                                union all
                                select -1 from dual
                            )
                    group by
                          NVL(a.id_contratista,-1)
                        , NVL(c.nombre_completo,'-')
                        , NVL(c.identificacion,'-')
                    order by 1,2
                ) loop
                apex_json.open_object();
                    apex_json.write('id_contratista', c_datos.id_contratista);
                    apex_json.write('contratista', c_datos.contratista);
                    apex_json.write('identificacion', c_datos.identificacion);
                    -- zonas
                    apex_json.open_array('zonas');
                        for z_datos in (
                            SELECT
                                 c.id_contratista
                                ,cz.id_zona
                                ,z.Nombre
                            FROM aire.ctn_contratos_zona cz
                            join aire.ctn_contratos c on c.id_contrato = cz.id_contrato
                            join aire.gnl_zonas z on z.id_zona = cz.id_zona
                            where   c.id_contratista = c_datos.id_contratista
                                and cz.ind_activo = 'S'
                                and c.ind_activo = 'S'
                                and z.ind_activo = 'S'
                        ) loop
                        apex_json.open_object();
                            apex_json.write('id_contratista', z_datos.id_contratista);
                            apex_json.write('id_zona', z_datos.id_zona);
                            apex_json.write('Nombre', z_datos.Nombre);
                        apex_json.close_object();
                        end loop;
                    apex_json.close_array();
                    apex_json.write('NoRegistros', c_datos.NoRegistros);
                apex_json.close_object();
            end loop;
            apex_json.close_array();
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
        apex_json.close_object();
    apex_json.close_object();
    
    --v_json := apex_json.get_clob_output;
    s_json := apex_json.get_clob_output;
    apex_json.free_output;
    --s_json := v_json;
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Carga exitosa');
    --dbms_output.put_line(s_json);
    exception
    when others then
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al crear el json: '|| sqlerrm);
        apex_json.free_output;
        apex_json.open_object();
        apex_json.write('codigo', 1);
        apex_json.write('mensaje', 'Error N '||v_id_log ||' al consultar el listado de ordenes.' || sqlerrm);
        apex_json.close_object();
        --v_json := apex_json.get_clob_output;
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
        --s_json := v_json;
        --dbms_output.put_line('xxx: '|| sqlerrm);
    END prc_consulta_agrupada_ordenes_area_central;

    procedure prc_desasignar_ordenes_masivo_tecnico (
        e_json  		in 	clob,
        s_json 	        out	clob
    ) is
        v_json_orden                json_object_t;
        v_id_log		            aire.gnl_logs.id_log%type;
        v_respuesta                 aire.tip_respuesta := aire.tip_respuesta(mensaje => 'Registro exitoso.', nombre_up => 'pkg_g_ordenes.prc_desasignar_ordenes_masivo_tecnico');
        
        v_id_soporte                NUMBER;
        v_usuario_registra          VARCHAR2(50 BYTE);
        v_nombre_archivo            VARCHAR2(100 BYTE);
        
        --v_numero_orden              NUMBER;
        v_id_archivo                NUMBER;
        v_id_actividad              NUMBER;
        v_id_archivo_instancia      NUMBER;
        v_fechainicio               timestamp;
        v_id_estado_orden           NUMBER;
    begin
        ----e_json := '{"id_soporte":1720,"usuario_registra",362,"nombre_archivo":"minombre.xlsx"}';
        -- validamos el Json de la orden y armamos el rowtype
        begin
            -- parseamos el json
            v_json_orden := json_object_t(e_json);
    
            -- extraemos los datos de la orden
            v_id_soporte               := v_json_orden.get_number('id_soporte');
            v_usuario_registra         := v_json_orden.get_string('usuario_registra');
            v_nombre_archivo           := v_json_orden.get_string('nombre_archivo');
            
            --actualizar tabla temporal  aire.ord_ordenes_cargue_temporal
            UPDATE aire.ord_ordenes_cargue_temporal
            SET Numero_Orden = aire.sec__ord_ordenes_cargue_temporal__numero_orden.NEXTVAL * -1
            where Numero_Orden is null and id_soporte = v_id_soporte and usuario_registra = v_usuario_registra;


            SELECT
                 id_archivo into v_id_archivo
            FROM aire.gnl_archivos x
            WHERE x.codigo = 'OTAC';
           
            SELECT
                id_actividad into v_id_actividad
            FROM aire.gnl_actividades x
            WHERE x.prefijo = 'G_SCR' and x.ind_activo = 'S';
            
            SELECT
                min(fecha_registra) into v_fechainicio
            FROM aire.ord_ordenes_cargue_temporal
            where id_soporte = v_id_soporte;
        exception
            when others then
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al procesar el json de la orden: '|| sqlerrm);
                apex_json.initialize_clob_output( p_preserve => false );
                apex_json.open_object();
                apex_json.write('codigo', 400);
                apex_json.write('mensaje', 'Error #'||v_id_log||' al registrar las ordenes masivas'|| sqlerrm);
                apex_json.close_object();
                s_json := apex_json.get_clob_output;
                apex_json.free_output;
                return;
        end;
        
        --ACA EL CODIGO PRINCIPAL
            MERGE
            INTO    aire.ord_ordenes_cargue_temporal trg
            USING   (
                    SELECT
                         x.Numero_Orden
                         
                        ,x.nic
                        --//--validar si el nic es numerico
                        ,case when REGEXP_LIKE(x.nic, '^[[:digit:]]+$') then 0 else 1 end                                       vnic
                        ,(select nvl(sum(0),1) from aire.gnl_clientes where nic = x.nic)                                        enic
                        
                        ,x.codigo_tipo_orden
                        --//--validar si el tipo orden es texto
                        ,case when REGEXP_LIKE(x.codigo_tipo_orden, '^[[:digit:]]+$') then 1 else 0 end                         vtor
                        ,(select nvl(sum(0),1) from aire.ord_tipos_orden where codigo_tipo_orden = x.codigo_tipo_orden)         etor
                        
                        ,x.codigo_tipo_suspencion
                        --//--validar si el tipo suspensión es texto
                        ,case when REGEXP_LIKE(x.codigo_tipo_suspencion, '^[[:digit:]]+$') then 1 else 0 end                    vtsu
                        ,(select nvl(sum(0),1) from aire.scr_tipos_suspencion where codigo = x.codigo_tipo_suspencion)          etsu
                        
                        ,x.codigo_estado_servicio
                        --//--validar si el tipo suspensión es texto
                        ,case when REGEXP_LIKE(x.codigo_estado_servicio, '^[[:digit:]]+$') then 1 else 0 end                    vtes
                        ,(select nvl(sum(0),1) from aire.gnl_estados_servicio where codigo = x.codigo_estado_servicio)          etes
                    FROM aire.ord_ordenes_cargue_temporal x
                    WHERE x.id_soporte = v_id_soporte and x.usuario_registra = v_usuario_registra
                    --WHERE x.id_soporte = 1720 and x.usuario_registra = '362'
                    order by 1
                    ) src
            ON      (trg.Numero_Orden = src.Numero_Orden)
            WHEN MATCHED THEN UPDATE
                SET
                    trg.con_errores = case when ((vnic + vtor) + (enic + etor) + (vtsu + etsu) + (vtes + etes)) = 0 then 0 else 1 end,
                    trg.desc_validacion =
                                case when ((vnic + vtor) + (enic + etor) + (vtsu + etsu) + (vtes + etes)) <= 0 then 'Validación Exitosa' end ||
                                case when (vnic + vtor + vtsu + vtes) > 0 then '-Tipo de dato incorrecto para: ' end    || case when src.vnic = 1 then '*-nic' end
                                                                                                                        || case when src.vtor = 1 then ' *-tipo_orden' end
                                                                                                                        || case when src.vtsu = 1 then ' *-tipo_suspencion' end
                                                                                                                        || case when src.vtes = 1 then ' *-estado_servicio' end
                                || case when (enic + etor + etsu + etes) > 0 then ' -No se encontraron los registros para: ' end    || case when src.enic = 1 then '*-nic' end
                                                                                                                                    || case when src.etor = 1 then ' *-tipo_orden' end
                                                                                                                                    || case when src.etsu = 1 then ' *-tipo_suspencion' end
                                                                                                                                    || case when src.etes = 1 then ' *-estado_servicio' end;
        
        /*
        24-01-2024
        Se obtiene el valor del estado asignar
        */
        SELECT
            id_estado_orden into v_id_estado_orden
        FROM aire.ord_estados_orden
        where   codigo_estado = 'SPEN'
        and     id_actividad IN (select id_actividad from aire.gnl_actividades where prefijo = 'G_SCR' and ind_activo = 'S');
        
        --Crear registros de ordenes masivamente.
            insert into aire.ord_ordenes
            (
                id_tipo_orden
                ,id_cliente
                ,id_estado_orden
                ,numero_orden
                ,id_actividad
                ,id_tipo_suspencion
                ,origen
                ,id_territorial
                ,id_zona
                ,direcion
            )
            select
                 --x.id_tipo_orden
                 t.id_tipo_orden
                ,c.id_cliente
                ,v_id_estado_orden id_estado_orden
                ,x.numero_orden numero_orden
                ,v_id_actividad
                --,x.id_tipo_suspencion
                ,s.id_tipo_suspencion
                ,'OP360'
                ,c.id_territorial
                ,c.id_zona
                ,c.direccion
            from aire.ord_ordenes_cargue_temporal x
            left join aire.gnl_clientes c on c.nic = x.nic
            left join aire.ord_tipos_orden t on t.codigo_tipo_orden = x.codigo_tipo_orden
            left join aire.scr_tipos_suspencion s on s.codigo = x.codigo_tipo_suspencion
            WHERE x.id_soporte = v_id_soporte and x.usuario_registra = v_usuario_registra and x.con_errores = 0;
            
            insert into aire.ord_ordenes_soporte
            (id_orden,id_soporte)
            select
                 x.id_orden
                , v_id_soporte id_soporte
            from aire.ord_ordenes x
            inner join aire.ord_ordenes_cargue_temporal tmp
                                on  tmp.numero_orden = x.numero_orden
                                and tmp.id_soporte = v_id_soporte
                                and tmp.usuario_registra = v_usuario_registra
                                and tmp.con_errores = 0;
            
        --Alimentar el log.
        --tabla principal
            INSERT INTO aire.gnl_archivos_instancia
            (
                --id_archivo_instancia,
                id_archivo,
                nombre_archivo,
                numero_registros_archivo,
                numero_registros_procesados,
                numero_errores,
                fecha_inicio_cargue,
                fecha_fin_cargue,
                duracion,
                id_usuario_registro,
                fecha_registro,
                id_estado_intancia,
                observaciones,
                id_soporte
            )
            SELECT
                 --x.id_archivo_instancia
                 v_id_archivo id_archivo
                ,v_nombre_archivo nombre_archivo
                ,SUM(1) numero_registros_archivo
                ,SUM(case when con_errores = 0 then 1 else 0 end) numero_registros_procesados
                ,SUM(con_errores) numero_errores
                ,v_fechainicio fecha_inicio_cargue
                ,localtimestamp fecha_fin_cargue
                ,'0' duracion
                ,v_usuario_registra id_usuario_registro
                ,localtimestamp fecha_registro
                ,163 id_estado_intancia --Finalizado
                ,'se cargaron ' || SUM(case when con_errores = 0 then 1 else 0 end) || ' ordenes.' observaciones
                ,v_id_soporte id_soporte
            FROM aire.ord_ordenes_cargue_temporal x
            where id_soporte = v_id_soporte and usuario_registra = v_usuario_registra
            group by id_soporte,usuario_registra;
        
        --se obtiene el id de la tabla principal
            select id_archivo_instancia into v_id_archivo_instancia
            from aire.gnl_archivos_instancia
            where id_soporte = v_id_soporte;
        
        --se alimenta la tabla detalle
            INSERT INTO aire.gnl_archivos_instancia_detalle
            (
            --id_archivo_instancia_detalle
             id_archivo_instancia
            ,numero_fila
            ,estado
            ,observaciones
            )
            SELECT
                 --id_archivo_instancia_detalle
                v_id_archivo_instancia id_archivo_instancia
                ,rownum numero_fila
                ,case when x.con_errores = 1 then 'ERROR' else 'OK' end estado
                ,x.desc_validacion observaciones
            FROM aire.ord_ordenes_cargue_temporal x
            where id_soporte = v_id_soporte and usuario_registra = v_usuario_registra;
            
        --se actualiza la fecha final
        
        UPDATE aire.gnl_archivos_instancia
        SET fecha_fin_cargue = localtimestamp, duracion = localtimestamp - fecha_inicio_cargue
        WHERE id_soporte = v_id_soporte and id_usuario_registro = v_usuario_registra and id_archivo_instancia = v_id_archivo_instancia;
        
        --se eliminan los registros de la tabla temporal aire.ord_ordenes_cargue_temporal
            DELETE aire.ord_ordenes_cargue_temporal
            WHERE id_soporte = v_id_soporte and usuario_registra = v_usuario_registra;
        
        commit;
        
        --------------------------------------------------------------------------------------
        apex_json.initialize_clob_output( p_preserve => false );
        apex_json.open_object();
        apex_json.write('codigo', 200);
        apex_json.write('mensaje', 'Ordenes masivas creadas con éxito.');
        apex_json.open_object('datos');
            apex_json.open_array('ordenes');
                for c_datos in (
                    select id_orden
                    from aire.ord_ordenes_soporte
                    where id_soporte = v_id_soporte
                ) loop
                apex_json.open_object();
                    apex_json.write('id_ordenes',c_datos.id_orden);
                apex_json.close_object();
                end loop;
            apex_json.close_array();
        apex_json.close_object();
        apex_json.close_object();
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
        
    exception
        when others then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al registrar orden: '|| sqlerrm);
            apex_json.initialize_clob_output( p_preserve => false );
            apex_json.open_object();
            apex_json.write('codigo', 400);
            apex_json.write('mensaje', 'Error #'||v_id_log||' al registrar las ordenas masivas' || sqlerrm);
            apex_json.close_object();
            s_json := apex_json.get_clob_output;
            apex_json.free_output;
    end prc_desasignar_ordenes_masivo_tecnico;

END PKG_G_CARLOS_VARGAS_TEST5;