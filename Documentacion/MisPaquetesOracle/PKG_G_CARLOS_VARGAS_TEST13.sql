create or replace PACKAGE                            "AIRE".PKG_G_CARLOS_VARGAS_TEST13 AS
    --/--Implementacion campos multiselect.19/04/2024
    --/--- OJO NO UTILIZAR
    procedure prc_consultar_ordenes_area_central (		
        e_json  in  clob,
        s_json 	out	clob
	);  

    --/--Implementacion campos multiselect.19/04/2024
    --/--- OJO NO UTILIZAR
    procedure prc_consultar_ordenes_contratistas (		
        e_json  in  clob,
        s_json 	out	clob
	);

    --/--- OJO NO UTILIZAR
    procedure prc_consultar_reportes_ejecutados (		
        e_json  in  clob,
        s_json 	out	clob
	);

    --/--- OJO NO UTILIZAR
    procedure prc_consultar_reportes_ejecutados_contratista (		
        e_json  in  clob,
        s_json 	out	clob
	);

END PKG_G_CARLOS_VARGAS_TEST13;
/

create or replace PACKAGE BODY                        "AIRE".PKG_G_CARLOS_VARGAS_TEST13 AS

    procedure prc_consultar_ordenes_area_central (
        e_json  in  clob,
        s_json 	out	clob
	) is
		--e_json  	clob :=  '{"id_contratista": "2076341","id_zona": ""}';
        --v_json      clob;
        v_json_objeto           json_object_t;
        v_id_log		        aire.gnl_logs.id_log%type;
        v_respuesta		        aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_ordenes.prc_consultar_ordenes_area_central');
        v_objeto      	        aire.ord_ordenes%rowtype;
        v_id_orden              NUMBER;
        pageNumber              NUMBER;
        pageSize                NUMBER;
        sortColumn              VARCHAR(100 BYTE);
        sortDirection           VARCHAR(100 BYTE);
        v_RegistrosTotales      NUMBER;
        v_id_estado_orden       aire.ord_estados_orden.id_estado_orden%type;
        v_codigo_estado         aire.ord_estados_orden.codigo_estado%type;
        v_codigo_suspencion     varchar2(10);

        v_dinamic_sql           clob;
        v_json_output           clob;
        v_ServerSide            json_object_t;
        v_filtersString         varchar2(4000);
        v_sortString            varchar2(4000);
        TYPE KeyValueRecord IS RECORD (
            value VARCHAR2(4000) -- Tipo de valor
        );
        TYPE Dictionary IS TABLE OF KeyValueRecord INDEX BY VARCHAR2(100); -- Índice por clave
        myDictionary Dictionary;

        v_list_contratistas varchar2(4000);
        v_list_zonas varchar2(4000);
        v_estados_con_contratista number;
        v_id_zonas varchar2(4000);
        v_id_contratista varchar2(4000);
    BEGIN
    
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
    apex_json.free_output;
    apex_json.initialize_clob_output( p_preserve => false );
    
    -- parseamos el json
    v_json_objeto := json_object_t(e_json);
    -- extraemos los datos de la orden
    v_id_contratista                := v_json_objeto.get_clob('id_contratista');
    v_id_zonas                      := v_json_objeto.get_clob('id_zona');
    v_id_estado_orden               := v_json_objeto.get_string('id_estado_orden');
    v_codigo_estado                 := v_json_objeto.get_string('codigo_estado');
    v_codigo_suspencion             := v_json_objeto.get_string('suspension');

    DBMS_OUTPUT.PUT_LINE('id_zona: ' || v_id_zonas);
    DBMS_OUTPUT.PUT_LINE('id_contratista: ' || v_id_contratista);
    --DBMS_OUTPUT.PUT_LINE('v_codigo_suspencion: ' || v_codigo_suspencion);

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
    --- xxxxxxxxxxxxx
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
        (
        case 
            when v_id_contratista = '-2' and (x.id_contratista is null or x.id_contratista is not null) then 1
            when v_id_contratista = '-1' and x.id_contratista is null then 1
            else 0 end = 1 or
        x.id_contratista in (
                    SELECT
                        REGEXP_SUBSTR(v_id_contratista, '[^,]+', 1, LEVEL) AS valor
                    FROM dual
                    CONNECT BY REGEXP_SUBSTR(v_id_contratista, '[^,]+', 1, LEVEL) IS NOT NULL
                )
        );

    --DBMS_OUTPUT.PUT_LINE('v_list_zonas: ' || v_list_zonas);
    --DBMS_OUTPUT.PUT_LINE('v_list_contratistas: ' || v_list_contratistas);

    /*SELECT
        count(*)
            into v_estados_con_contratista
    FROM aire.ord_estados_orden x
    where 
        x.id_actividad = 101 AND
        x.ind_activo = 'S' AND
        x.codigo_estado in ('LFAL','SCOM','SEAS','SASI','CERR') AND
        x.codigo_estado in (
            SELECT
                REGEXP_SUBSTR(v_codigo_estado, '[^,]+', 1, LEVEL) AS valor
            FROM dual
            CONNECT BY REGEXP_SUBSTR(v_codigo_estado, '[^,]+', 1, LEVEL) IS NOT NULL
        );
    DBMS_OUTPUT.PUT_LINE('v_estados_con_contratista: ' || v_estados_con_contratista);*/

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
    myDictionary('id_contratistaComplemento').value := case
                                                --when v_estados_con_contratista = 0 then ' or a.id_contratista is null '
                                                when v_id_contratista = '-2' then ' or a.id_contratista is null '
                                            else ' ' end;   
    myDictionary('add_null').value := case
                                        when LENGTH(NVL(v_id_contratista, '')) > 2 and INSTR(NVL(v_id_contratista, ''), '-1') > 0 then ' or a.id_contratista is null '
                                        else '' end;
    myDictionary('id_contratista').value := case
                                                --when v_id_orden > 0 then ' '
                                                when v_id_contratista = '-1' then ' and a.id_contratista is null '
                                                --/ si contratista es todos y zona se eligio una, filtrar contratistas de esa zona.
                                                -- when v_objeto.id_contratista = -2 then ' and (a.id_contratista in (' || v_list_contratistas || ') or a.id_contratista is null ) ' --es todo!
                                                when v_id_contratista = '-2' then ' and (a.id_contratista in (' || v_list_contratistas || ') ' || myDictionary('id_contratistaComplemento').value || ' ) ' --es todo!
                                                when NVL(v_id_contratista, '')  NOT IN (' ', '-1','-2') then ' and ( a.id_contratista IN (
                                                                                                                SELECT REGEXP_SUBSTR('''|| v_id_contratista ||''', ''[^,]+'', 1, LEVEL) AS contratistas FROM DUAL
                                                                                                                CONNECT BY REGEXP_SUBSTR('''|| v_id_contratista ||''', ''[^,]+'', 1, LEVEL) IS NOT NULL
                                                                                                            ) ' || myDictionary('add_null').value || ' ) '
                                            else ' ' end;
    /*myDictionary('id_zona').value := case
                                                when v_id_orden > 0 then ' '
                                                when v_objeto.id_zona = -1 then ' and a.id_zona is null '                                                
                                                when v_objeto.id_zona = -2 then ' and (a.id_zona in (' || v_list_zonas || ') or a.id_zona is null) ' --es todo!
                                                when v_objeto.id_zona > 0 then ' and a.id_zona = ' || v_objeto.id_zona || ' '
                                            else ' ' end;*/
                                            --DBMS_OUTPUT.PUT_LINE('llego 1');
    myDictionary('id_zona').value := case
                                                --when v_id_orden > 0 then ' '
                                                when v_id_zonas = '-1' then ' and a.id_zona is null '                                                
                                                when v_id_zonas = '-2' then ' and (a.id_zona in (' || v_list_zonas || ') or a.id_zona is null) ' --es todo!
                                                when NVL(v_id_zonas, '')  NOT IN (' ', '-1','-2') then ' and a.id_zona IN (
                                                                                                                SELECT REGEXP_SUBSTR('''|| v_id_zonas ||''', ''[^,]+'', 1, LEVEL) AS zonas FROM DUAL
                                                                                                                CONNECT BY REGEXP_SUBSTR('''|| v_id_zonas ||''', ''[^,]+'', 1, LEVEL) IS NOT NULL
                                                                                                            )'
                                            else ' ' end;
                                            --DBMS_OUTPUT.PUT_LINE('llego 2');
   /* myDictionary('codigo_suspencion').value := case
                                                when v_id_orden > 0 then ' '
                                                when v_codigo_suspencion = '-1' then ' and l.codigo is null '                                                
                                                when v_codigo_suspencion = '-2' then ' ' --es todo!
                                                when v_codigo_suspencion != '-2' AND v_codigo_suspencion != '-1' then ' and UPPER(l.codigo) = ''' || UPPER(v_codigo_suspencion) || ''' '
                                            else ' ' end;*/
    myDictionary('codigo_suspencion').value := case
                                            --when v_id_orden > 0 then ' '
                                            when v_codigo_suspencion = '-1' then ' and l.codigo is null '                                                
                                            when v_codigo_suspencion = '-2' then ' ' --es todo!
                                            when TRIM(NVL(v_codigo_suspencion, '')) NOT IN (' ', '-1','-2') then ' and UPPER(l.codigo) IN (
                                                                                                                SELECT REGEXP_SUBSTR('''|| v_codigo_suspencion ||''', ''[^,]+'', 1, LEVEL) AS Codigo_Suspencion FROM DUAL
                                                                                                                CONNECT BY REGEXP_SUBSTR('''|| v_codigo_suspencion ||''', ''[^,]+'', 1, LEVEL) IS NOT NULL
                                                                                                            )'
                                            else ' ' end;
    DBMS_OUTPUT.PUT_LINE('myDictionary(''codigo_suspencion'').value: ' || myDictionary('codigo_suspencion').value);

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
                            , b.descripcion  as estado_orden
                            , case when nvl(m1.nombres,''-1'') = ''-1'' then null else m1.nombres || '' '' || m1.apellidos || ''_'' || a.id_contratista end as contratista
                            , REPLACE(REGEXP_REPLACE(d.nombre_cliente, ''[[:cntrl:]]'', '' ''),''"'','' '') as cliente
                            , e.nombre as territorial
                            , f.nombre || ''_'' || a.id_zona as zona
                            , a.direcion
                            , a.fecha_creacion                            
                            , a.fecha_cierre as fecha_cierre
                            , case when nvl(m2.nombres,''-1'') = ''-1'' then null else m2.nombres || '' '' || m2.apellidos end as usuario_cierre
                            , i.nombre as actividad
                            , case when nvl(m3.nombres,''-1'') = ''-1'' then null else m3.nombres || '' '' || m3.apellidos end as nombre_contratista_persona
                            , k.descripcion as tipo_trabajo
                            , l.descripcion as tipo_suspencion
                            , a.origen                           
                            
                            , TRUNC(sysdate - TRUNC(a.fecha_registro)) as antiguedad
                            , a.fecha_registro
                            , d.nic
                            , tc.descripcion || '' - '' || ts.descripcion  tarifa
                            , h.codigo_tipo_orden
                            --, d.deuda
                            , trim(to_char(n.expired_balance, ''999G999G999G999G999G999G990'')) deuda
                            --, d.ultima_factura
                            , n.expired_periodos as ultima_factura

                            , a.id_contratista_persona                            
                            , dpt.nombre as Departamento
                            , mnc.nombre as Municipio
                            , brr.nombre as Barrio
                            , tb.descripcion as tipo_brigada
                            , a.comentario_orden_servicio_num1 Comanterio_OS
                            , localtimestamp Fecha_Consulta
                            , a.fecha_asigna_tecnico
                        from aire.ord_ordenes              				a
                        left join aire.ord_ordenes_dato_suministro      n on n.id_orden = a.id_orden
                        left join aire.gnl_tarifas_subcategoria         ts on ts.codigo_osf = n.codigo_tarifa
                        left join aire.gnl_tarifas_categoria            tc on tc.codigo_tarifa_categoria = ts.codigo_tarifa_categoria


                        left join aire.ord_estados_orden   			b on a.id_estado_orden   		= b.id_estado_orden                        
                        left join aire.ctn_contratistas  				c on a.id_contratista    		= c.id_contratista
                        left join aire.gnl_personas                     m1 on c.id_persona              = m1.id_persona
                        left join aire.gnl_clientes        				d on a.id_cliente        		= d.id_cliente
                        left join aire.gnl_departamentos                dpt on dpt.id_departamento      = d.id_departamento
                        left join aire.gnl_municipios                   mnc on mnc.id_municipio         = d.id_municipio
                        left join aire.gnl_barrios                      brr on brr.id_barrio            = a.id_barrio
                        left join aire.gnl_territoriales   				e on a.id_territorial    		= e.id_territorial
                        left join aire.gnl_zonas           				f on a.id_zona           		= f.id_zona
                        left join aire.sgd_usuarios      				g on a.id_usuario_cierre 		= g.id_usuario
                        left join aire.gnl_personas             		m2 on g.id_persona              = m2.id_persona
                        left join aire.ord_tipos_orden     				h on a.id_tipo_orden     		= h.id_tipo_orden
                        left join aire.gnl_actividades     				i on a.id_actividad      		= i.id_actividad
                        left join aire.ctn_contratistas_persona  		j on  a.id_contratista_persona 	= j.id_contratista_persona
                        left join aire.ctn_contratistas_brigada         j2 on a.id_contratista_persona = j2.id_contratista_persona and j2.ind_activo = ''S''
                        left join aire.ctn_tipos_brigada                tb on tb.id_tipo_brigada        = j2.id_tipo_brigada
                        left join aire.v_gnl_personas			 	    m3 on j.id_persona              = m3.id_persona
                        left join aire.ord_tipos_trabajo 				k on a.id_tipo_trabajo 		 	= k.id_tipo_trabajo
                        left join aire.scr_tipos_suspencion 			l on a.id_tipo_suspencion 	 	= l.id_tipo_suspencion
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
                            ' || myDictionary('codigo_suspencion').value || '
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
                            , q.antiguedad
                            , q.fecha_registro
                            , q.nic
                            , q.tarifa
                            , q.codigo_tipo_orden
                            , q.deuda
                            , q.ultima_factura
                            , q.id_contratista_persona                            
                            , q.Departamento
                            , q.Municipio
                            , q.Barrio
                            , q.tipo_brigada
                            , q.Comanterio_OS
                            , q.Fecha_Consulta
                            , q.fecha_asigna_tecnico

                            , acnt.MaxRows RegistrosTotales
                    from tx q, txcnt acnt
                    ' || myDictionary('ServerSide_WhereAdd').value || '
                    ' || myDictionary('ServerSide_SortAdd').value || '
                    OFFSET ' || pageNumber || ' ROWS FETCH NEXT ' || pageSize ||' ROWS ONLY
                ) loop
                apex_json.open_object();
                    apex_json.write(''antiguedad'', c_datos.antiguedad);
                    apex_json.write(''fecha_registro'', c_datos.fecha_registro);
                    apex_json.write(''origen'', c_datos.origen);
                    apex_json.write(''id_orden'', c_datos.id_orden);
                    apex_json.write(''numero_orden'', c_datos.numero_orden);
                    apex_json.write(''nic'', c_datos.nic);
                    apex_json.write(''tarifa'', c_datos.tarifa);
                    apex_json.write(''codigo_tipo_orden'', c_datos.codigo_tipo_orden);
                    apex_json.write(''tipo_orden'', c_datos.tipo_orden);
                    apex_json.write(''deuda'', c_datos.deuda);
                    apex_json.write(''ultima_factura'', c_datos.ultima_factura);
                    apex_json.write(''id_contratista_persona'', c_datos.id_contratista_persona);
                    apex_json.write(''nombre_contratista_persona'', c_datos.nombre_contratista_persona);
                    apex_json.write(''contratista'', c_datos.contratista);
                    apex_json.write(''territorial'', c_datos.territorial);
                    apex_json.write(''zona'', c_datos.zona);
                    apex_json.write(''Departamento'', c_datos.Departamento);
                    apex_json.write(''Municipio'', c_datos.Municipio);
                    apex_json.write(''Barrio'', c_datos.Barrio);
                    apex_json.write(''direcion'', c_datos.direcion);
                    apex_json.write(''estado_orden'', c_datos.estado_orden);
                    apex_json.write(''fecha_asigna_tecnico'', c_datos.fecha_asigna_tecnico);
                    apex_json.write(''tipo_brigada'', c_datos.tipo_brigada);
                    apex_json.write(''tipo_suspencion'', c_datos.tipo_suspencion);
                    apex_json.write(''Comanterio_OS'', c_datos.Comanterio_OS);
                    apex_json.write(''Fecha_Consulta'', c_datos.Fecha_Consulta);                    
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

        v_dinamic_sql               clob;
        v_json_output               clob;
        v_ServerSide                json_object_t;
        v_filtersString             varchar2(4000);
        v_sortString                varchar2(4000);
        v_id_zonas_contratista      varchar2(4000);
        v_id_contratista_pesrona    varchar2(4000);
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

    v_id_orden                      := v_json_objeto.get_number('id_orden');

    v_objeto.id_estado_orden        := v_json_objeto.get_string('id_estado_orden');
    v_codigo_estado                 := v_json_objeto.get_string('codigo_estado');
    v_codigo_suspencion             := v_json_objeto.get_string('codigo_suspencion');

    v_id_zonas_contratista          := v_json_objeto.get_clob('id_zona');
    v_id_contratista_pesrona        := v_json_objeto.get_clob('id_contratista_persona');
    
    -- pageNumber                      := v_json_objeto.get_number('pageNumber');
    -- pageSize                        := v_json_objeto.get_number('pageSize');
    --sortColumn              := v_json_objeto.get_string('sortColumn');
    --sortDirection              := v_json_objeto.get_string('sortDirection');

    v_ServerSide                    := v_json_objeto.get_Object('ServerSideJson');
    v_filtersString                 := v_ServerSide.get_string('filtersString');
    v_sortString                    := v_ServerSide.get_string('sortString');
    pageNumber                      := v_ServerSide.get_number('first');
    pageSize                        := v_ServerSide.get_number('rows');

    --obtener el id tipo suspencion
    --obtener el id tipo suspencion
    /*SELECT
        NVL(e.id_tipo_suspencion,0)
            into v_id_tipo_suspencion
    FROM aire.scr_tipos_suspencion e
    where e.id_actividad in (select id_actividad from aire.gnl_actividades where prefijo = 'G_SCR' and ind_activo = 'S')
            and e.codigo = v_codigo_suspencion;

    if v_codigo_suspencion = '-1' or v_codigo_suspencion = '-2' then
        v_id_tipo_suspencion := to_number(v_codigo_suspencion);
    end if;*/

    --- -//-->INICIO DINAMIC SQL 26/02/2024
    --- -//- Filtros Staticos
    myDictionary('id_orden').value := case when v_id_orden > 0 then ' and a.id_orden = ' || v_id_orden else ' ' end;
    /*myDictionary('id_contratista_persona').value := case
                                                --when v_id_orden > 0 then ' '
                                                when v_objeto.id_contratista_persona = -1 then ' and a.id_contratista_persona is null '
                                                when v_objeto.id_contratista_persona = -2 then ' ' --es todo!
                                                when v_objeto.id_contratista_persona > 0 then ' and a.id_contratista_persona = ' || v_objeto.id_contratista_persona || ' '
                                            else ' ' end;*/
    myDictionary('add_null').value := case
                                        when LENGTH(NVL(v_id_contratista_pesrona, '')) > 2 and INSTR(NVL(v_id_contratista_pesrona, ''), '-1') > 0 then ' or a.id_contratista_persona is null '
                                        else '' end;
    myDictionary('id_contratista_persona').value := case
                                                --when v_id_orden > 0 then ' '
                                                when v_id_contratista_pesrona = '-1' then ' and a.id_contratista_persona is null '
                                                when v_id_contratista_pesrona = '-2' then ' ' --es todo!
                                                when NVL(v_id_contratista_pesrona, '')  NOT IN (' ', '-1','-2') then ' and ( a.id_contratista_persona IN (
                                                                                                            SELECT REGEXP_SUBSTR('''|| v_id_contratista_pesrona ||''', ''[^,]+'', 1, LEVEL) AS contratista_persona FROM DUAL
                                                                                                            CONNECT BY REGEXP_SUBSTR('''|| v_id_contratista_pesrona ||''', ''[^,]+'', 1, LEVEL) IS NOT NULL
                                                                                                        ) ' || myDictionary('add_null').value || ' ) '
                                            else ' ' end;

    /*myDictionary('id_zona').value := case
                                                when v_id_orden > 0 then ' '
                                                when v_objeto.id_zona = -1 then ' and a.id_zona is null '
                                                when v_objeto.id_zona = -2 then ' ' --es todo!
                                                when v_objeto.id_zona > 0 then ' and a.id_zona = ' || v_objeto.id_zona || ' '
                                            else ' ' end;*/
    myDictionary('id_zona').value := case
                                                --when v_id_orden > 0 then ' '
                                                when v_id_zonas_contratista = '-1' then ' and a.id_zona is null '
                                                when v_id_zonas_contratista = '-2' then ' ' --es todo!
                                                when NVL(v_id_zonas_contratista, '')  NOT IN (' ', '-1','-2') then ' and a.id_zona IN (
                                                                                                            SELECT REGEXP_SUBSTR('''|| v_id_zonas_contratista ||''', ''[^,]+'', 1, LEVEL) AS zonas_contratista FROM DUAL
                                                                                                            CONNECT BY REGEXP_SUBSTR('''|| v_id_zonas_contratista ||''', ''[^,]+'', 1, LEVEL) IS NOT NULL
                                                                                                        )'
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
    /*myDictionary('id_tipo_suspencion').value := case
                                                when v_id_orden > 0 then ' '
                                                when v_id_tipo_suspencion = -1 then ' and a.id_tipo_suspencion is null '
                                                when v_id_tipo_suspencion = -2 then ' ' --es todo!
                                                when v_id_tipo_suspencion > 0 then ' and a.id_tipo_suspencion = ' || v_id_tipo_suspencion || ' '
                                            else ' ' end;*/
    myDictionary('codigo_suspencion').value := case
                                            --when v_id_orden > 0 then ' '
                                            when v_codigo_suspencion = '-1' then ' and l.codigo is null '                                                
                                            when v_codigo_suspencion = '-2' then ' ' --es todo!
                                            when TRIM(NVL(v_codigo_suspencion, '')) NOT IN (' ', '-1','-2') then ' and UPPER(l.codigo) IN (
                                                                                                                SELECT REGEXP_SUBSTR('''|| v_codigo_suspencion ||''', ''[^,]+'', 1, LEVEL) AS Codigo_Suspencion FROM DUAL
                                                                                                                CONNECT BY REGEXP_SUBSTR('''|| v_codigo_suspencion ||''', ''[^,]+'', 1, LEVEL) IS NOT NULL
                                                                                                            )'
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
                             --, a.id_contratista_persona
                             , j.nombre_contratista_persona
                             , a.id_tipo_trabajo as id_tipo_trabajo
                             , k.descripcion as tipo_trabajo
                             , a.id_tipo_suspencion as id_tipo_suspencion
                             , l.descripcion as tipo_suspencion
                             , a.Origen

                            , TRUNC(sysdate - TRUNC(a.fecha_registro)) as antiguedad
                            , a.fecha_registro
                            , d.nic
                            , d.tarifa
                            , h.codigo_tipo_orden
                            , trim(to_char(m.expired_balance, ''999G999G999G999G999G999G990'')) deuda
                            , m.expired_periodos		ultima_factura
                            , a.id_contratista_persona                            
                            , dpt.nombre as Departamento
                            , mnc.nombre as Municipio
                            , brr.nombre as Barrio
                            , tb.descripcion as tipo_brigada
                            , a.comentario_orden_servicio_num1 Comanterio_OS
                            , localtimestamp Fecha_Consulta
                            , a.fecha_asigna_tecnico
                        from aire.ord_ordenes              				a
                        left join aire.ord_estados_orden   				b on a.id_estado_orden   		= b.id_estado_orden
                        left join aire.v_ctn_contratistas  				c on a.id_contratista    		= c.id_contratista
                        left join aire.gnl_clientes        				d on a.id_cliente        		= d.id_cliente
                        left join aire.ord_ordenes_dato_suministro			m on a.id_orden					= m.id_orden
                        
                        left join aire.gnl_departamentos                dpt on dpt.id_departamento      = d.id_departamento
                        left join aire.gnl_municipios                   mnc on mnc.id_municipio         = d.id_municipio
                        left join aire.gnl_barrios                      brr on brr.id_barrio            = d.id_barrio

                        left join aire.gnl_territoriales   				e on a.id_territorial    		= e.id_territorial
                        left join aire.gnl_zonas           				f on a.id_zona           		= f.id_zona
                        left join aire.sgd_usuarios_zona uz on uz.id_zona = f.id_zona and uz.id_usuario = '||v_id_usuario||'
                        left join aire.v_sgd_usuarios      				g on a.id_usuario_cierre 		= g.id_usuario
                        left join aire.ord_tipos_orden     				h on a.id_tipo_orden     		= h.id_tipo_orden
                        left join aire.gnl_actividades     				i on a.id_actividad      		= i.id_actividad
                        left join aire.v_ctn_contratistas_persona  		j on a.id_contratista_persona 	= j.id_contratista_persona

                        left join aire.ctn_contratistas_brigada         j2 on a.id_contratista_persona = j2.id_contratista_persona and j2.ind_activo = ''S''
                        left join aire.ctn_tipos_brigada                tb on tb.id_tipo_brigada        = j2.id_tipo_brigada

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
                            ' || myDictionary('codigo_suspencion').value || '
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
                    apex_json.write(''antiguedad'', c_datos.antiguedad);
                    apex_json.write(''fecha_registro'', c_datos.fecha_registro);
                    apex_json.write(''origen'', c_datos.origen);
                    apex_json.write(''id_orden'', c_datos.id_orden);
                    apex_json.write(''numero_orden'', c_datos.numero_orden);
                    apex_json.write(''nic'', c_datos.nic);
                    apex_json.write(''tarifa'', c_datos.tarifa);
                    apex_json.write(''codigo_tipo_orden'', c_datos.codigo_tipo_orden);
                    apex_json.write(''tipo_orden'', c_datos.tipo_orden);
                    apex_json.write(''deuda'', c_datos.deuda);
                    apex_json.write(''ultima_factura'', c_datos.ultima_factura);
                    apex_json.write(''id_contratista_persona'', c_datos.id_contratista_persona);
                    apex_json.write(''nombre_contratista_persona'', c_datos.nombre_contratista_persona);
                    apex_json.write(''contratista'', c_datos.contratista);
                    apex_json.write(''territorial'', c_datos.territorial);
                    apex_json.write(''zona'', c_datos.zona);
                    apex_json.write(''Departamento'', c_datos.Departamento);
                    apex_json.write(''Municipio'', c_datos.Municipio);
                    apex_json.write(''Barrio'', c_datos.Barrio);
                    apex_json.write(''direcion'', c_datos.direcion);
                    apex_json.write(''estado_orden'', c_datos.estado_orden);
                    apex_json.write(''fecha_asigna_tecnico'', c_datos.fecha_asigna_tecnico);
                    apex_json.write(''tipo_brigada'', c_datos.tipo_brigada);
                    apex_json.write(''tipo_suspencion'', c_datos.tipo_suspencion);
                    apex_json.write(''Comanterio_OS'', c_datos.Comanterio_OS);
                    apex_json.write(''Fecha_Consulta'', c_datos.Fecha_Consulta);
                    apex_json.write(''id_tipo_suspencion'', c_datos.id_tipo_suspencion);
                    


                    --apex_json.write(''id_tipo_orden'', c_datos.id_tipo_orden);
                    --apex_json.write(''id_estado_orden'', c_datos.id_estado_orden);
                    --apex_json.write(''id_contratista'', c_datos.id_contratista);
                    --apex_json.write(''id_cliente'', c_datos.id_cliente);
                    --apex_json.write(''cliente'', c_datos.cliente);
                    --apex_json.write(''id_territorial'', c_datos.id_territorial);
                    --apex_json.write(''id_zona'', c_datos.id_zona);
                    --apex_json.write(''fecha_creacion'', c_datos.fecha_creacion);
                    --apex_json.write(''fecha_cierre'', c_datos.fecha_cierre);
                    --apex_json.write(''id_usuario_cierre'', c_datos.id_usuario_cierre);
                    --apex_json.write(''usuario_cierre'', c_datos.usuario_cierre);
                    --apex_json.write(''id_actividad'', c_datos.id_actividad);
                    --apex_json.write(''actividad'', c_datos.actividad);
                    --apex_json.write(''id_contratista_persona'', c_datos.id_contratista_persona);
                    --apex_json.write(''id_tipo_trabajo'', c_datos.id_tipo_trabajo);
                    --apex_json.write(''tipo_trabajo'', c_datos.tipo_trabajo);
                    
                    
                    BEGIN :v1 := c_datos.RegistrosTotales; END;
                apex_json.close_object();
            end loop;
            apex_json.close_array();
            END;
            ';

            -- DBMS_OUTPUT.PUT_LINE('------------------------------------------------------------');
            dbms_output.PUT_LINE('Query: ' || v_dinamic_sql);

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
    when NO_DATA_FOUND then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'No se encontraron tecnicos asociados');
            apex_json.free_output;
            apex_json.open_object();
            apex_json.write('codigo', 1);
            apex_json.write('mensaje', 'No se encontraron tecnicos asociados');
            apex_json.close_object();
            
            s_json := apex_json.get_clob_output;
            apex_json.free_output;

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

    procedure prc_consultar_reportes_ejecutados (
        e_json  in  clob,
        s_json 	out	clob
    ) is
        --e_json  	clob :=  '{"id_contratista": "2076341","id_zona": ""}';
        --v_json      clob;
        v_json_objeto       json_object_t;
        v_id_log		    aire.gnl_logs.id_log%type;
        v_respuesta		    aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_ordenes.prc_consultar_reportes_ejecutados');
        v_objeto      	    aire.ord_ordenes%rowtype;
        v_id_orden          NUMBER;
        pageNumber          NUMBER;
        pageSize            NUMBER;
        sortColumn          VARCHAR(100 BYTE);
        sortDirection       VARCHAR(100 BYTE);
        v_RegistrosTotales  NUMBER;
        
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
        v_list_zonas        varchar2(4000);
        v_ruta_web          varchar2(4000);
        v_f_inicio          varchar2(400);
        v_f_inicio2         varchar2(400);
        v_f_fin             varchar2(400);
        v_f_fin2             varchar2(400);
        --Nuevas variables
        v_id_zonas          varchar2(4000);
        v_id_contratista    varchar2(4000);
    BEGIN
    
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
    apex_json.free_output;
    apex_json.initialize_clob_output( p_preserve => false );
    
    -- parseamos el json
    v_json_objeto := json_object_t(e_json);
    -- extraemos los datos de la orden
    v_id_contratista                := v_json_objeto.get_string('id_contratista');
    v_id_zonas                      := v_json_objeto.get_string('id_zona');
    -- v_id_estado_orden               := v_json_objeto.get_string('id_estado_orden');
    -- v_codigo_estado                 := v_json_objeto.get_string('codigo_estado');
    v_id_orden                      := v_json_objeto.get_number('id_orden');
    -- pageNumber                      := v_json_objeto.get_number('pageNumber');
    -- pageSize                        := v_json_objeto.get_number('pageSize');
    v_ServerSide                    := v_json_objeto.get_Object('ServerSideJson');
    v_filtersString                 := v_ServerSide.get_string('filtersString');
    v_sortString                    := v_ServerSide.get_string('sortString');
    pageNumber                      := v_ServerSide.get_number('first');
    pageSize                        := v_ServerSide.get_number('rows');
    v_f_inicio                      := v_json_objeto.get_string('fechaInicial');
    v_f_fin                         := v_json_objeto.get_string('fechaFinal');

    -- DBMS_OUTPUT.PUT_LINE('v_json_objeto: ' || v_json_objeto);
    v_f_inicio2 := TRUNC(TO_TIMESTAMP(v_f_inicio, 'YYYY-MM-DD"T"HH24:MI:SS"Z"'));
    v_f_fin2    := TRUNC(TO_TIMESTAMP(v_f_fin, 'YYYY-MM-DD"T"HH24:MI:SS"Z"'));

    --dbms_output.PUT_LINE('v_json_objeto2: ' || v_json_objeto.to_string());
    --dbms_output.PUT_LINE('v_f_inicio: ' || v_f_inicio);
    --dbms_output.PUT_LINE('v_f_inicio2: ' || v_f_inicio2);
    --dbms_output.PUT_LINE('v_f_fin: ' || v_f_fin);
    --dbms_output.PUT_LINE('v_f_fin2: ' || v_f_fin2);
    --dbms_output.PUT_LINE('LENGTH(v_f_inicio): ' || LENGTH(v_f_inicio));
    --dbms_output.PUT_LINE('LENGTH(v_f_fin): ' || nvl(LENGTH(v_f_fin),0));

    myDictionary('fecha').value := case 
                                when LENGTH(v_f_inicio) != 0 and nvl(LENGTH(v_f_fin),0) = 0 then ' and trunc(q.fecha_cierre) = ''' || v_f_inicio2 || ''''
                                when LENGTH(v_f_inicio) = 0 and  nvl(LENGTH(v_f_fin),0) = 0 then ' '
                                when LENGTH(v_f_inicio) != 0 and nvl(LENGTH(v_f_fin),0) != 0 then ' and trunc(q.fecha_cierre) between ' || '''' || v_f_inicio2 || '''' || ' and ' || '''' || v_f_fin2 || ''''
                                else ' ' end;
    --dbms_output.PUT_LINE('fecha: ' || myDictionary('fecha').value);                                
    
    -- DBMS_OUTPUT.PUT_LINE('v_list_zonas: ' || v_list_zonas);


    --- -//-->INICIO DINAMIC SQL 26/02/2024
    --- -//- Filtros Staticos
    myDictionary('id_orden').value := case when v_id_orden > 0 then ' and a.id_orden = ' || v_id_orden else ' ' end;
    
    myDictionary('add_null').value := case
                                        when LENGTH(NVL(v_id_contratista, '')) > 2 and INSTR(NVL(v_id_contratista, ''), '-1') > 0 then ' or a.id_contratista is null '
                                        else '' end;                            
    myDictionary('id_contratista').value := case
                                                --when v_id_orden > 0 then ' '
                                                when v_id_contratista = '-1' then ' and a.id_contratista is null '
                                                when v_id_contratista = '-2' then ' ' --es todo!
                                                when NVL(v_id_contratista, '')  NOT IN (' ', '-1','-2') then ' and (a.id_contratista IN (
                                                                                                                SELECT REGEXP_SUBSTR('''|| v_id_contratista ||''', ''[^,]+'', 1, LEVEL) AS contratistas FROM DUAL
                                                                                                                CONNECT BY REGEXP_SUBSTR('''|| v_id_contratista ||''', ''[^,]+'', 1, LEVEL) IS NOT NULL
                                                                                                            ) ' || myDictionary('add_null').value || ' ) '
                                            else ' ' end;

    myDictionary('id_zona').value := case
                                                --when v_id_orden > 0 then ' '
                                                when v_id_zonas = '-1' then ' and a.id_zona is null '
                                                when v_id_zonas = '-2' then ' ' --es todo!
                                                when NVL(v_id_zonas, '')  NOT IN (' ', '-1','-2') then ' and a.id_zona IN (
                                                                                                                SELECT REGEXP_SUBSTR('''|| v_id_zonas ||''', ''[^,]+'', 1, LEVEL) AS zonas FROM DUAL
                                                                                                                CONNECT BY REGEXP_SUBSTR('''|| v_id_zonas ||''', ''[^,]+'', 1, LEVEL) IS NOT NULL
                                                                                                            )'
                                            else ' ' end;

    -- myDictionary('id_contratista').value := case
    --                                             when v_id_orden > 0 then ' '
    --                                             when v_objeto.id_contratista = -1 then ' and a.id_contratista is null '
    --                                             --/ si contratista es todos y zona se eligio una, filtrar contratistas de esa zona.
    --                                             when v_objeto.id_contratista = -2 then ' and (a.id_contratista in (' || v_list_contratistas || ') or a.id_contratista is null) ' --es todo!
    --                                             when v_objeto.id_contratista > 0 then ' and a.id_contratista = ' || v_objeto.id_contratista || ' '
    --                                         else ' ' end;
    -- myDictionary('id_zona').value := case
    --                                             when v_id_orden > 0 then ' '
    --                                             when v_objeto.id_zona = -1 then ' and a.id_zona is null '                                                
    --                                             when v_objeto.id_zona = -2 then ' and (a.id_zona in (' || v_list_zonas || ') or a.id_zona is null) ' --es todo!
    --                                             when v_objeto.id_zona > 0 then ' and a.id_zona = ' || v_objeto.id_zona || ' '
    --                                         else ' ' end;
    -- myDictionary('codigo_estado').value := case
    --                                             when v_id_orden > 0 then ' '
    --                                             when v_codigo_estado = '-1' then ' and b.codigo_estado is null '
    --                                             when v_codigo_estado = '-2' then ' ' --es todo!
    --                                             when TRIM(NVL(v_codigo_estado,'-')) NOT IN (' ', '-1','-2') then ' and b.codigo_estado IN (SELECT
    --                                                                                                                     REGEXP_SUBSTR(''' || v_codigo_estado || ''', ''[^,]+'', 1, LEVEL) AS valor
    --                                                                                                                 FROM dual
    --                                                                                                                 CONNECT BY REGEXP_SUBSTR(''' || v_codigo_estado || ''', ''[^,]+'', 1, LEVEL) IS NOT NULL
    --                                                                                                             ) '
    --                                         else ' ' end;
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
    select
        ruta_web
        into v_ruta_web
    from aire.gnl_rutas_archivo_servidor
    where id_ruta_archivo_servidor = 105;
    
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
                              case when nvl(m1.nombres,''-1'') = ''-1'' then null else m1.nombres || '' '' || m1.apellidos || ''_'' || a.id_contratista end as contratista
                            , e.nombre as territorial
                            , f.nombre || ''_'' || a.id_zona as zona
                            , q.acta

                            , q.fecha_ejecucion as fechaejecucion
                            , q.fecha_inicio_ejecucion as fechainicial
                            , q.fecha_fin_ejecucion as fechafinal

                            , trunc(q.fecha_cierre) as fecha_Sincronizacion
                            , to_char(q.fecha_cierre, ''HH24:MI:SS'') as hora_Sincronizacion

                            , a.id_orden
                            , a.numero_orden
                            , d.nic
                            , mnc.nombre as Ciudad
                            , brr.nombre as barrio
                            , a.direcion as direccion
                            , h.codigo_tipo_orden as tipo_orden
                            , i.nombre as tipo_proceso

                            --, ''Accion: '' || oga22.ListagAcciones || '', Anomalia: '' || qa.descripcion as Accion
                            , qa.descripcion as Accion
                            
                            -- , ''SubAccion: '' || oga22.ListagSubAcciones || '', SubAnomalia: '' || qsa.descripcion as SubAccion
                            , case when qa.codigo_anomalia IN (3481,3476) then qa.descripcion || '' - '' || qsa.descripcion else qsa.descripcion end as SubAccion

                            , q3.ListagCaracterizacion caracterizacion

                            , k.descripcion as tipo_trabajo
                            , n.expired_periodos as num_factura
                            -- , n.expired_balance as deuda_act

                            -- , n.expired_balance as deuda_ejec
                            -- , n.expired_balance as deuda_ejec
                            , TRIM(TO_CHAR(n.expired_balance,''L999G999G999G999D99MI'',''NLS_NUMERIC_CHARACTERS = '''',.'''' NLS_CURRENCY = ''''$ ''''''))  as deuda_ejec

                            , d.tarifa
                            
                            --, tc.descripcion tipo_actividad     
                            
                            , ae.nombre tipo_actividad     
                            , gdvq.descripcion as actividad

                            , l.descripcion as tipo_suspension                            
                            , json_object(
                                    key ''latitud''  VALUE nvl(to_number(json_value(replace(replace(replace(q.georreferencia,'',"'',''#x#''),'','',''.''),''#x#'','',"''), ''$.latitud''), ''9999.9999999999999999''), 0),
                                    key ''longitud'' VALUE nvl(to_number(json_value(replace(replace(replace(q.georreferencia,'',"'',''#x#''),'','',''.''),''#x#'','',"''), ''$.longitud''), ''9999.9999999999999999''), 0) 
                            ) as georreferencia                            
                            , gdv.descripcion vehiculo
                            , tb.descripcion tipo_operativa
                            , m3.identificacion as id_contratista_persona
                            , case when nvl(m3.nombres,''-1'') = ''-1'' then null else m3.nombres || '' '' || m3.apellidos end as nombre_contratista_persona
                            , ''Fecha: '' || q.fecha_ejecucion || 
                              '', ACTA: '' || q.acta || 
                              '', TECNICO: '' || (case when nvl(m3.nombres,''-1'') = ''-1'' then null else m3.nombres || '' '' || m3.apellidos end) || 
                              '', PREDIO: '' || gdv2.descripcion || 
                              '', ATENDIO: '' || nvl(q.nombres_persona_atiende,''no'') || 
                              '', MEDIDOR: '' || q.Numero_Medidor || 
                              '', LECTURA: '' || q.lectura || 
                              '', CT: '' || q.ct || 
                              '', MT: '' || q.mt || 
                              '', Otros: '' || oga2.ListagFaltante || 
                              '' '' as observacion
                              , a.origen
                            --, REPLACE(:v0,''xnOrdnxn'',a.id_orden) as UrlDescargaActa
                              , :v0 || a.id_orden as UrlDescargaActa
                        from aire.ord_ordenes              				        a
                        left join (
                            select
                                og.ID_ORDEN
                                ,LISTAGG(ac.CODIGO || '': '' || oga.OBSERVACION, ''; '') as ListagFaltante
                            from aire.ord_ordenes_gestion_acciones oga
                            inner join aire.ORD_ORDENES_GESTION og on og.ID_ORDEN_GESTION = oga.ID_ORDEN_GESTION
                            inner join aire.scr_acciones ac on ac.ID_ACCION = oga.ID_ACCION
                            where ac.IND_ACTIVO = ''S''
                            group by og.ID_ORDEN
                        )                                                       oga2    on oga2.id_orden = a.id_orden
                        left join (
                            select 
                                  subq.id_orden_gestion, subq.id_orden
                                , subq.acta
                                , subq.id_anomalia
                                , subq.id_uso_energia
                                , subq.id_actividad_economica
                                , subq.id_estado_predio
                                , subq.id_subanomalia
                                , subq.georreferencia
                                , subq.nombres_persona_atiende
                                , subq.Numero_Medidor
                                , subq.lectura
                                , subq.ct
                                , subq.mt                                    
                                , subq.fecha_ejecucion
                                , subq.fecha_inicio_ejecucion
                                , subq.fecha_fin_ejecucion
                                , subq.fecha_cierre
                                , subq.id_contratista_brigada
                            from (
                                select qs.id_orden_gestion, qs.id_orden
                                    , qs.acta
                                    , qs.id_anomalia
                                    , qs.id_uso_energia
                                    , qs.id_actividad_economica
                                    , qs.id_estado_predio
                                    , qs.id_subanomalia
                                    , qs.georreferencia
                                    , qs.nombres_persona_atiende
                                    , qs.Numero_Medidor
                                    , qs.lectura
                                    , qs.ct
                                    , qs.mt                                    
                                    , qs.fecha_ejecucion
                                    , qs.fecha_inicio_ejecucion
                                    , qs.fecha_fin_ejecucion
                                    , qs.fecha_cierre
                                    , qs.id_contratista_brigada
                                    ,row_number() over (partition by qs.id_orden order by qs.id_orden_gestion) as rn
                                from aire.ord_ordenes_gestion qs
                            ) subq
                            where subq.rn = 1
                        )                                                       q       on q.id_orden = a.id_orden
                        left join (
                            select 
                                z.id_orden
                                ,LISTAGG(y.Descripcion,'', '') as ListagCaracterizacion
                            from aire.ord_comentarios_subanomalia y
                            join (
                                select 
                                    t.id_orden
                                    ,regexp_substr(t.id_observacion_anomalia,''[^,]+'',1,rn) as id_comentario_subanomalia
                                from aire.ord_ordenes_gestion t      
                                cross join lateral (
                                select level rn from dual
                                connect by level <=   
                                    length (t.id_observacion_anomalia) - length (replace(t.id_observacion_anomalia, '','' )) + 1
                                )
                            -- where t.id_observacion_anomalia is not null --and t.id_orden_gestion = 683
                            ) z on z.id_comentario_subanomalia = y.id_comentario_subanomalia
                            group by z.ID_ORDEN
                        )                                                       q3      on q3.id_orden = a.id_orden
                        left join aire.ord_anomalias                            qa      on q.id_anomalia = qa.id_anomalia
                        left join aire.ord_subanomalias                         qsa     on q.id_anomalia = qsa.id_anomalia and q.id_subanomalia = qsa.id_subanomalia
                        left join aire.gnl_dominios_valor                       gdvq    on gdvq.id_dominio_valor = q.id_uso_energia
                        left join aire.gnl_actividades_economica                ae      on ae.id_actividad_economica = q.id_actividad_economica

                        left join aire.gnl_dominios_valor                       gdv2    on q.id_estado_predio = gdv2.id_dominio_valor
                        left join (
                            select 
                                oga.id_orden_gestion
                                ,LISTAGG(acc.descripcion || '', '') as ListagAcciones
                                ,LISTAGG(distinct ascx.descripcion || '', '') as ListagSubAcciones
                            from aire.ord_ordenes_gestion_acciones oga
                            left join aire.scr_acciones                             acc on acc.id_accion = oga.id_accion
                            left join aire.scr_subacciones                          ascx on ascx.id_accion = oga.id_accion and ascx.id_subaccion = oga.id_subaccion                            
                            group by oga.id_orden_gestion
                        )                                                           oga22 on oga22.id_orden_gestion = q.id_orden_gestion

                        --left join aire.ord_ordenes_gestion_acciones             oga on oga.id_orden_gestion = q.id_orden_gestion
                        --left join aire.scr_acciones                             acc on acc.id_accion = oga.id_accion
                        --left join aire.scr_subacciones                          ascx on ascx.id_accion = oga.id_accion and ascx.id_subaccion = oga.id_subaccion

                        -- left join aire.gnl_actividades_economica                q2 on q2.id_actividad_economica = q.id_actividad_economica

                        left join aire.ord_ordenes_dato_suministro              n on n.id_orden = a.id_orden
                        left join aire.gnl_tarifas_subcategoria                 ts on ts.codigo_osf = n.codigo_tarifa
                        left join aire.gnl_tarifas_categoria                    tc on tc.codigo_tarifa_categoria = ts.codigo_tarifa_categoria

                        inner join aire.ord_estados_orden   			        b on a.id_estado_orden   		= b.id_estado_orden
                        left join aire.ctn_contratistas  				        c on a.id_contratista    		= c.id_contratista
                        left join aire.gnl_personas                             m1 on c.id_persona              = m1.id_persona
                        left join aire.gnl_clientes        				        d on a.id_cliente        		= d.id_cliente
                        -- left join aire.gnl_departamentos                        dpt on dpt.id_departamento      = d.id_departamento
                        left join aire.gnl_municipios                           mnc on mnc.id_municipio         = d.id_municipio
                        left join aire.gnl_barrios                              brr on brr.id_barrio            = a.id_barrio
                        left join aire.gnl_territoriales   				        e on a.id_territorial    		= e.id_territorial
                        left join aire.gnl_zonas           				        f on a.id_zona           		= f.id_zona
                        -- left join aire.sgd_usuarios      				        g on a.id_usuario_cierre 		= g.id_usuario
                        -- left join aire.gnl_personas             		        m2 on g.id_persona              = m2.id_persona
                        left join aire.ord_tipos_orden     				        h on a.id_tipo_orden     		= h.id_tipo_orden
                        left join aire.gnl_actividades     				        i on a.id_actividad      		= i.id_actividad
                        left join aire.ctn_contratistas_persona  		        j on  a.id_contratista_persona 	= j.id_contratista_persona
                        left join aire.v_gnl_personas			 	            m3 on j.id_persona              = m3.id_persona
                        
                        left join aire.ctn_contratistas_brigada                 j2 on q.id_contratista_brigada  = j2.id_contratista_brigada
                        left join aire.CTN_CONTRATISTAS_VEHICULO                cv on cv.id_contratista_vehiculo = j2.id_contratista_vehiculo and cv.ind_activo = ''S''
                        left join aire.gnl_dominios_valor                       gdv on gdv.id_dominio_valor = cv.id_tipo_vehiculo


                        left join aire.ctn_tipos_brigada                        tb on tb.id_tipo_brigada        = j2.id_tipo_brigada
                        
                        left join aire.ord_tipos_trabajo 				        k on a.id_tipo_trabajo 		 	= k.id_tipo_trabajo
                        left join aire.scr_tipos_suspencion 			        l on a.id_tipo_suspencion 	 	= l.id_tipo_suspencion
                        
                        where
                            b.codigo_estado = ''CERR'' and
                            NVL(a.id_contratista,-1) in (
                                    select id_contratista
                                    from aire.v_ctn_contratos
                                    where UPPER(prefijo_actividad) = ''G_SCR''
                                    and UPPER(ind_activo) = ''S''
                                    union all
                                    select -1 from dual
                            )
                            ' || myDictionary('id_orden').value || '
                            ' || myDictionary('fecha').value || '                        
                            ' || myDictionary('id_contratista').value || '
                            ' || myDictionary('id_zona').value || '                             
                    )
                    , txcnt as (
                        SELECT sum(1) AS MaxRows
                        FROM tx
                        ' || myDictionary('ServerSide_WhereAdd').value || '                        
                        ' || myDictionary('ServerSide_SortAdd').value || '                        
                    )
                    select
                          q.contratista
                        , q.territorial
                        , q.zona
                        , q.acta
                        , q.fechaejecucion
                        , q.fechainicial
                        , q.fechafinal
                        , q.fecha_Sincronizacion
                        , q.hora_Sincronizacion
                        , q.id_orden
                        , q.numero_orden

                        , q.nic
                        , q.ciudad
                        , q.barrio

                        , q.direccion
                        , q.tipo_orden
                        , q.tipo_trabajo
                        , q.tipo_proceso
                        , q.Accion
                        , q.SubAccion

                        , q.caracterizacion

                        , q.num_factura
                        -- , q.deuda_act
                        , q.deuda_ejec
                        , q.tarifa
                        
                        , q.tipo_actividad
                        , q.actividad


                        , q.tipo_suspension                        
                        , q.georreferencia
                        , q.vehiculo
                        , q.tipo_operativa
                        , q.id_contratista_persona
                        , q.nombre_contratista_persona
                        , q.observacion
                        , q.origen

                        , q.UrlDescargaActa

                        , acnt.MaxRows RegistrosTotales
                    from tx q, txcnt acnt
                    ' || myDictionary('ServerSide_WhereAdd').value || '
                    ' || myDictionary('ServerSide_SortAdd').value || '
                    OFFSET ' || pageNumber || ' ROWS FETCH NEXT ' || pageSize ||' ROWS ONLY
                ) loop
                apex_json.open_object();
                    apex_json.write(''nombre_contratista_persona'', c_datos.nombre_contratista_persona);
                    apex_json.write(''contratista'', c_datos.contratista);
                    apex_json.write(''territorial'', c_datos.territorial);
                    apex_json.write(''zona'', c_datos.zona);
                    apex_json.write(''acta'', c_datos.acta);
                    apex_json.write(''fechaejecucion'', c_datos.fechaejecucion);
                    apex_json.write(''fechainicial'', c_datos.fechainicial);
                    apex_json.write(''fechafinal'', c_datos.fechafinal);
                    apex_json.write(''fecha_Sincronizacion'', c_datos.fecha_Sincronizacion);
                    apex_json.write(''hora_Sincronizacion'', c_datos.hora_Sincronizacion);

                    apex_json.write(''id_orden'', c_datos.id_orden);
                    apex_json.write(''numero_orden'', c_datos.numero_orden);
                    apex_json.write(''nic'', c_datos.nic);
                    apex_json.write(''Ciudad'', c_datos.Ciudad);
                    apex_json.write(''barrio'', c_datos.barrio);
                    apex_json.write(''direccion'', c_datos.direccion);
                    apex_json.write(''tipo_orden'', c_datos.tipo_orden);
                    apex_json.write(''tipo_trabajo'', c_datos.tipo_trabajo);
                    apex_json.write(''tipo_proceso'', c_datos.tipo_proceso);
                    apex_json.write(''Accion'', c_datos.Accion);
                    apex_json.write(''SubAccion'', c_datos.SubAccion);

                    apex_json.write(''caracterizacion'', c_datos.caracterizacion);

                    apex_json.write(''num_factura'', c_datos.num_factura);
                    apex_json.write(''deuda_ejec'', c_datos.deuda_ejec);
                    apex_json.write(''tarifa'', c_datos.tarifa);
                    
                    apex_json.write(''actividad'', c_datos.actividad);
                    apex_json.write(''tipo_actividad'', c_datos.tipo_actividad);

                    apex_json.write(''tipo_suspension'', c_datos.tipo_suspension);                    
                    apex_json.write(''GPS'', json_object_t(c_datos.georreferencia).get_string(''latitud'') || '','' || json_object_t(c_datos.georreferencia).get_string(''longitud''));
                    apex_json.write(''vehiculo'', c_datos.vehiculo);                    
                    apex_json.write(''tipo_operativa'', c_datos.tipo_operativa);
                    apex_json.write(''id_contratista_persona'', c_datos.id_contratista_persona);
                    apex_json.write(''nombre_contratista_persona'', c_datos.nombre_contratista_persona);
                    apex_json.write(''observacion'', c_datos.observacion);
                    apex_json.write(''origen'', c_datos.origen);


                    apex_json.write(''UrlDescargaActa'', c_datos.UrlDescargaActa);
                    
                    
                    BEGIN :v1 := c_datos.RegistrosTotales; END;
                apex_json.close_object();
            end loop;
            apex_json.close_array();
            END;
            ';

            -- DBMS_OUTPUT.PUT_LINE('------------------------------------------------------------');
            --dbms_output.PUT_LINE('Query: ' || v_dinamic_sql);

            EXECUTE IMMEDIATE replace(v_dinamic_sql,'xnx','&')
            USING IN v_ruta_web, OUT v_RegistrosTotales;

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
        apex_json.write('mensaje', 'Error N '||v_id_log ||' al consultar el listado de ordenes.: ' || replace(replace(replace(sqlerrm,'"',''''),chr(13),''),chr(10),''));
        apex_json.close_object();
        
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
    END prc_consultar_reportes_ejecutados;

    procedure prc_consultar_reportes_ejecutados_contratista (
        e_json  in  clob,
        s_json 	out	clob
    ) is
        --e_json  	clob :=  '{"id_contratista": "2076341","id_zona": ""}';
        --v_json      clob;
        v_json_objeto       json_object_t;
        v_id_log		    aire.gnl_logs.id_log%type;
        v_respuesta		    aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_ordenes.prc_consultar_reportes_ejecutados_contratista');
        v_objeto      	    aire.ord_ordenes%rowtype;
        v_id_persona        VARCHAR2(50 BYTE);
        v_id_usuario        VARCHAR2(50 BYTE);
        v_id_orden          NUMBER;
        pageNumber          NUMBER;
        pageSize            NUMBER;
        sortColumn          VARCHAR(100 BYTE);
        sortDirection       VARCHAR(100 BYTE);
        v_RegistrosTotales  NUMBER;
        
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
        v_list_zonas        varchar2(4000);
        v_ruta_web          varchar2(4000);
        v_f_inicio          varchar2(400);
        v_f_inicio2         varchar2(400);
        v_f_fin             varchar2(400);
        v_f_fin2             varchar2(400);

        v_id_zonas_contratista      varchar2(4000);
        v_id_contratista_pesrona    varchar2(4000);
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

    --dbms_output.PUT_LINE('v_id_persona: ' || v_id_persona);

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

    --dbms_output.PUT_LINE('v_objeto.id_contratista: ' || v_objeto.id_contratista);

    v_id_contratista_pesrona        := v_json_objeto.get_string('id_contratista_persona');
    --dbms_output.PUT_LINE('v_objeto.id_contratista_persona: ' || v_objeto.id_contratista_persona);

    v_id_zonas_contratista          := v_json_objeto.get_string('id_zona');
    -- v_id_estado_orden               := v_json_objeto.get_string('id_estado_orden');
    -- v_codigo_estado                 := v_json_objeto.get_string('codigo_estado');
    v_id_orden                      := v_json_objeto.get_number('id_orden');
    -- pageNumber                      := v_json_objeto.get_number('pageNumber');
    -- pageSize                        := v_json_objeto.get_number('pageSize');
    v_ServerSide                    := v_json_objeto.get_Object('ServerSideJson');
    v_filtersString                 := v_ServerSide.get_string('filtersString');
    v_sortString                    := v_ServerSide.get_string('sortString');
    pageNumber                      := v_ServerSide.get_number('first');
    pageSize                        := v_ServerSide.get_number('rows');
    v_f_inicio                      := v_json_objeto.get_string('fechaInicial');
    v_f_fin                         := v_json_objeto.get_string('fechaFinal');

    -- DBMS_OUTPUT.PUT_LINE('v_json_objeto: ' || v_json_objeto);
    v_f_inicio2 := TRUNC(TO_TIMESTAMP(v_f_inicio, 'YYYY-MM-DD"T"HH24:MI:SS"Z"'));
    v_f_fin2    := TRUNC(TO_TIMESTAMP(v_f_fin, 'YYYY-MM-DD"T"HH24:MI:SS"Z"'));

    --dbms_output.PUT_LINE('v_json_objeto2: ' || v_json_objeto.to_string());
    --dbms_output.PUT_LINE('v_f_inicio: ' || v_f_inicio);
    --dbms_output.PUT_LINE('v_f_inicio2: ' || v_f_inicio2);
    --dbms_output.PUT_LINE('v_f_fin: ' || v_f_fin);
    --dbms_output.PUT_LINE('v_f_fin2: ' || v_f_fin2);
    --dbms_output.PUT_LINE('LENGTH(v_f_inicio): ' || LENGTH(v_f_inicio));
    --dbms_output.PUT_LINE('LENGTH(v_f_fin): ' || nvl(LENGTH(v_f_fin),0));

    myDictionary('fecha').value := case 
                                when LENGTH(v_f_inicio) != 0 and nvl(LENGTH(v_f_fin),0) = 0 then ' and trunc(q.fecha_cierre) = ''' || v_f_inicio2 || ''''
                                when LENGTH(v_f_inicio) = 0 and  nvl(LENGTH(v_f_fin),0) = 0 then ' '
                                when LENGTH(v_f_inicio) != 0 and nvl(LENGTH(v_f_fin),0) != 0 then ' and trunc(q.fecha_cierre) between ' || '''' || v_f_inicio2 || '''' || ' and ' || '''' || v_f_fin2 || ''''
                                else ' ' end;
    --dbms_output.PUT_LINE('fecha: ' || myDictionary('fecha').value);                                
    
    -- DBMS_OUTPUT.PUT_LINE('v_list_zonas: ' || v_list_zonas);


    --- -//-->INICIO DINAMIC SQL 26/02/2024
    --- -//- Filtros Staticos
    myDictionary('id_orden').value := case when v_id_orden > 0 then ' and a.id_orden = ' || v_id_orden else ' ' end;
                                    
    
    -- myDictionary('id_zona').value := case
    --                                             when v_id_orden > 0 then ' '
    --                                             when v_objeto.id_zona = -1 then ' and a.id_zona is null '
    --                                             when v_objeto.id_zona = -2 then ' ' --es todo!
    --                                             when v_objeto.id_zona > 0 then ' and a.id_zona = ' || v_objeto.id_zona || ' '
    --                                         else ' ' end;
    --dbms_output.PUT_LINE('v_objeto.id_contratista_persona: ' || v_objeto.id_contratista_persona);
    myDictionary('add_null').value := case
                                        when LENGTH(NVL(v_id_contratista_pesrona, '')) > 2 and INSTR(NVL(v_id_contratista_pesrona, ''), '-1') > 0 then ' or a.id_contratista_persona is null '
                                        else '' end;
    myDictionary('id_contratista_persona').value := case
                                                --when v_id_orden > 0 then ' '
                                                when v_id_contratista_pesrona = '-1' then ' and a.id_contratista_persona is null '
                                                when v_id_contratista_pesrona = '-2' then ' ' --es todo!
                                                when NVL(v_id_contratista_pesrona, '')  NOT IN (' ', '-1','-2') then ' and ( a.id_contratista_persona IN (
                                                                                                            SELECT REGEXP_SUBSTR('''|| v_id_contratista_pesrona ||''', ''[^,]+'', 1, LEVEL) AS contratista_persona FROM DUAL
                                                                                                            CONNECT BY REGEXP_SUBSTR('''|| v_id_contratista_pesrona ||''', ''[^,]+'', 1, LEVEL) IS NOT NULL
                                                                                                        ) ' || myDictionary('add_null').value || ' ) '
                                            else ' ' end;
    myDictionary('id_zona').value := case
                                                --when v_id_orden > 0 then ' '
                                                when v_id_zonas_contratista = '-1' then ' and a.id_zona is null '
                                                when v_id_zonas_contratista = '-2' then ' ' --es todo!
                                                when NVL(v_id_zonas_contratista, '')  NOT IN (' ', '-1','-2') then ' and a.id_zona IN (
                                                                                                                    SELECT REGEXP_SUBSTR('''|| v_id_zonas_contratista ||''', ''[^,]+'', 1, LEVEL) AS zonas_contratista FROM DUAL
                                                                                                                    CONNECT BY REGEXP_SUBSTR('''|| v_id_zonas_contratista ||''', ''[^,]+'', 1, LEVEL) IS NOT NULL
                                                                                                                )'
                                            else ' ' end;
    --dbms_output.PUT_LINE('myDictionary(''id_contratista_persona''): ' || myDictionary('id_contratista_persona').value);                                           
    --dbms_output.PUT_LINE('myDictionary(''id_zona''): ' || myDictionary('id_zona').value);                                           
    -- myDictionary('codigo_estado').value := case
    --                                             when v_id_orden > 0 then ' '
    --                                             when v_codigo_estado = '-1' then ' and b.codigo_estado is null '
    --                                             when v_codigo_estado = '-2' then ' ' --es todo!
    --                                             when TRIM(NVL(v_codigo_estado,'-')) NOT IN (' ', '-1','-2') then ' and b.codigo_estado IN (SELECT
    --                                                                                                                     REGEXP_SUBSTR(''' || v_codigo_estado || ''', ''[^,]+'', 1, LEVEL) AS valor
    --                                                                                                                 FROM dual
    --                                                                                                                 CONNECT BY REGEXP_SUBSTR(''' || v_codigo_estado || ''', ''[^,]+'', 1, LEVEL) IS NOT NULL
    --                                                                                                             ) '
    --                                         else ' ' end;
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
    --dbms_output.PUT_LINE('v_list_zonas: ' || 'ok');
    --- -//-->FIN DINAMIC SQL
    select
        ruta_web
        into v_ruta_web
    from aire.gnl_rutas_archivo_servidor
    where id_ruta_archivo_servidor = 105;
    
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
                              case when nvl(m1.nombres,''-1'') = ''-1'' then null else m1.nombres || '' '' || m1.apellidos || ''_'' || a.id_contratista end as contratista
                            , e.nombre as territorial
                            , f.nombre || ''_'' || a.id_zona as zona

                            , q.acta
                            , q.fecha_ejecucion as fechaejecucion
                            , q.fecha_inicio_ejecucion as fechainicial
                            , q.fecha_fin_ejecucion as fechafinal

                            , trunc(q.fecha_cierre) as fecha_Sincronizacion
                            , to_char(q.fecha_cierre, ''HH24:MI:SS'') as hora_Sincronizacion

                            , a.id_orden
                            , a.numero_orden
                            , d.nic
                            , mnc.nombre as Ciudad
                            , brr.nombre as barrio
                            , a.direcion as direccion
                            , h.codigo_tipo_orden as tipo_orden
                            , i.nombre as tipo_proceso

                            --, ''Accion: '' || oga22.ListagAcciones || '', Anomalia: '' || qa.descripcion as Accion
                            , qa.descripcion as Accion
                            
                            -- , ''SubAccion: '' || oga22.ListagSubAcciones || '', SubAnomalia: '' || qsa.descripcion as SubAccion
                            , case when qa.codigo_anomalia IN (3481,3476) then qa.descripcion || '' - '' || qsa.descripcion else qsa.descripcion end as SubAccion

                            , q3.ListagCaracterizacion caracterizacion
                            , k.descripcion as tipo_trabajo
                            , n.expired_periodos as num_factura
                            --, n.expired_balance as deuda_act
                            
                            -- , n.expired_balance as deuda_ejec
                            , TRIM(TO_CHAR(n.expired_balance,''L999G999G999G999D99MI'',''NLS_NUMERIC_CHARACTERS = '''',.'''' NLS_CURRENCY = ''''$ ''''''))  as deuda_ejec
                            , d.tarifa
                            
                            --, tc.descripcion as  tipo_actividad   
                            , ae.nombre tipo_actividad
                            , gdvq.descripcion as actividad

                            , l.descripcion as tipo_suspension                            
                            , json_object(
                                    key ''latitud''  VALUE nvl(to_number(json_value(replace(replace(replace(q.georreferencia,'',"'',''#x#''),'','',''.''),''#x#'','',"''), ''$.latitud''), ''9999.9999999999999999''), 0),
                                    key ''longitud'' VALUE nvl(to_number(json_value(replace(replace(replace(q.georreferencia,'',"'',''#x#''),'','',''.''),''#x#'','',"''), ''$.longitud''), ''9999.9999999999999999''), 0) 
                            ) as georreferencia                            
                            , gdv.descripcion vehiculo
                            , tb.descripcion tipo_operativa
                            , m3.identificacion as id_contratista_persona
                            , case when nvl(m3.nombres,''-1'') = ''-1'' then null else m3.nombres || '' '' || m3.apellidos end as nombre_contratista_persona
                            , ''Fecha: '' || q.fecha_ejecucion || 
                              '', ACTA: '' || q.acta || 
                              '', TECNICO: '' || (case when nvl(m3.nombres,''-1'') = ''-1'' then null else m3.nombres || '' '' || m3.apellidos end) || 
                              '', PREDIO: '' || gdv2.descripcion || 
                              '', ATENDIO: '' || nvl(q.nombres_persona_atiende,''no'') || 
                              '', MEDIDOR: '' || q.Numero_Medidor || 
                              '', LECTURA: '' || q.lectura || 
                              '', CT: '' || q.ct || 
                              '', MT: '' || q.mt || 
                              '', Otros: '' || oga2.ListagFaltante || 
                              '' '' as observacion
                              , a.origen
                            --, REPLACE(:v0,''xnOrdnxn'',a.id_orden) as UrlDescargaActa
                              , :v0 || a.id_orden as UrlDescargaActa
                        from aire.ord_ordenes              				        a
                        left join (
                            select
                                og.ID_ORDEN
                                ,LISTAGG(ac.CODIGO || '': '' || oga.OBSERVACION, ''; '') as ListagFaltante
                            from aire.ord_ordenes_gestion_acciones oga
                            inner join aire.ORD_ORDENES_GESTION og on og.ID_ORDEN_GESTION = oga.ID_ORDEN_GESTION
                            inner join aire.scr_acciones ac on ac.ID_ACCION = oga.ID_ACCION
                            where ac.IND_ACTIVO = ''S''
                            group by og.ID_ORDEN
                        )                                                       oga2    on oga2.id_orden = a.id_orden
                        left join (
                            select 
                                  subq.id_orden_gestion, subq.id_orden
                                , subq.acta
                                , subq.id_anomalia
                                , subq.id_uso_energia
                                , subq.id_actividad_economica
                                , subq.id_estado_predio
                                , subq.id_subanomalia
                                , subq.georreferencia
                                , subq.nombres_persona_atiende
                                , subq.Numero_Medidor
                                , subq.lectura
                                , subq.ct
                                , subq.mt                                    
                                , subq.fecha_ejecucion
                                , subq.fecha_inicio_ejecucion
                                , subq.fecha_fin_ejecucion
                                , subq.fecha_cierre
                                , subq.id_contratista_brigada
                            from (
                                select qs.id_orden_gestion, qs.id_orden
                                    , qs.acta
                                    , qs.id_anomalia
                                    , qs.id_uso_energia
                                    , qs.id_actividad_economica
                                    , qs.id_estado_predio
                                    , qs.id_subanomalia
                                    , qs.georreferencia
                                    , qs.nombres_persona_atiende
                                    , qs.Numero_Medidor
                                    , qs.lectura
                                    , qs.ct
                                    , qs.mt                                    
                                    , qs.fecha_ejecucion
                                    , qs.fecha_inicio_ejecucion
                                    , qs.fecha_fin_ejecucion
                                    , qs.fecha_cierre
                                    , qs.id_contratista_brigada
                                    ,row_number() over (partition by qs.id_orden order by qs.id_orden_gestion) as rn
                                from aire.ord_ordenes_gestion qs
                            ) subq
                            where subq.rn = 1
                        )                                                       q       on q.id_orden = a.id_orden
                        left join (
                            select 
                                z.id_orden
                                ,LISTAGG(y.Descripcion,'', '') as ListagCaracterizacion
                            from aire.ord_comentarios_subanomalia y
                            join (
                                select 
                                    t.id_orden
                                    ,regexp_substr(t.id_observacion_anomalia,''[^,]+'',1,rn) as id_comentario_subanomalia
                                from aire.ord_ordenes_gestion t      
                                cross join lateral (
                                select level rn from dual
                                connect by level <=   
                                    length (t.id_observacion_anomalia) - length (replace(t.id_observacion_anomalia, '','' )) + 1
                                )
                            -- where t.id_observacion_anomalia is not null --and t.id_orden_gestion = 683
                            ) z on z.id_comentario_subanomalia = y.id_comentario_subanomalia
                            group by z.ID_ORDEN
                        )                                                       q3      on q3.id_orden = a.id_orden
                        left join aire.ord_anomalias                            qa      on q.id_anomalia = qa.id_anomalia
                        left join aire.ord_subanomalias                         qsa     on q.id_anomalia = qsa.id_anomalia and q.id_subanomalia = qsa.id_subanomalia
                        left join aire.gnl_dominios_valor                       gdvq    on gdvq.id_dominio_valor = q.id_uso_energia
                        left join aire.gnl_actividades_economica                ae      on ae.id_actividad_economica = q.id_actividad_economica


                        left join aire.gnl_dominios_valor                       gdv2    on q.id_estado_predio = gdv2.id_dominio_valor
                        left join (
                            select 
                                oga.id_orden_gestion
                                ,LISTAGG(acc.descripcion || '', '') as ListagAcciones
                                ,LISTAGG(distinct ascx.descripcion || '', '') as ListagSubAcciones
                            from aire.ord_ordenes_gestion_acciones oga
                            left join aire.scr_acciones                             acc on acc.id_accion = oga.id_accion
                            left join aire.scr_subacciones                          ascx on ascx.id_accion = oga.id_accion and ascx.id_subaccion = oga.id_subaccion                            
                            group by oga.id_orden_gestion
                        )                                                           oga22 on oga22.id_orden_gestion = q.id_orden_gestion

                        --left join aire.ord_ordenes_gestion_acciones             oga on oga.id_orden_gestion = q.id_orden_gestion
                        --left join aire.scr_acciones                             acc on acc.id_accion = oga.id_accion
                        --left join aire.scr_subacciones                          ascx on ascx.id_accion = oga.id_accion and ascx.id_subaccion = oga.id_subaccion

                        -- left join aire.gnl_actividades_economica                q2 on q2.id_actividad_economica = q.id_actividad_economica

                        left join aire.ord_ordenes_dato_suministro              n on n.id_orden = a.id_orden
                        left join aire.gnl_tarifas_subcategoria                 ts on ts.codigo_osf = n.codigo_tarifa
                        left join aire.gnl_tarifas_categoria                    tc on tc.codigo_tarifa_categoria = ts.codigo_tarifa_categoria

                        inner join aire.ord_estados_orden   			        b on a.id_estado_orden   		= b.id_estado_orden
                        left join aire.ctn_contratistas  				        c on a.id_contratista    		= c.id_contratista
                        left join aire.gnl_personas                             m1 on c.id_persona              = m1.id_persona
                        left join aire.gnl_clientes        				        d on a.id_cliente        		= d.id_cliente
                        -- left join aire.gnl_departamentos                        dpt on dpt.id_departamento      = d.id_departamento
                        left join aire.gnl_municipios                           mnc on mnc.id_municipio         = d.id_municipio
                        left join aire.gnl_barrios                              brr on brr.id_barrio            = a.id_barrio
                        left join aire.gnl_territoriales   				        e on a.id_territorial    		= e.id_territorial
                        left join aire.gnl_zonas           				        f on a.id_zona           		= f.id_zona
                        -- left join aire.sgd_usuarios      				        g on a.id_usuario_cierre 		= g.id_usuario
                        -- left join aire.gnl_personas             		        m2 on g.id_persona              = m2.id_persona
                        left join aire.ord_tipos_orden     				        h on a.id_tipo_orden     		= h.id_tipo_orden
                        left join aire.gnl_actividades     				        i on a.id_actividad      		= i.id_actividad
                        left join aire.ctn_contratistas_persona  		        j on  a.id_contratista_persona 	= j.id_contratista_persona
                        left join aire.v_gnl_personas			 	            m3 on j.id_persona              = m3.id_persona
                        
                        left join aire.ctn_contratistas_brigada                 j2 on q.id_contratista_brigada  = j2.id_contratista_brigada
                        left join aire.CTN_CONTRATISTAS_VEHICULO                cv on cv.id_contratista_vehiculo = j2.id_contratista_vehiculo and cv.ind_activo = ''S''
                        left join aire.gnl_dominios_valor                       gdv on gdv.id_dominio_valor = cv.id_tipo_vehiculo


                        left join aire.ctn_tipos_brigada                        tb on tb.id_tipo_brigada        = j2.id_tipo_brigada
                        
                        left join aire.ord_tipos_trabajo 				        k on a.id_tipo_trabajo 		 	= k.id_tipo_trabajo
                        left join aire.scr_tipos_suspencion 			        l on a.id_tipo_suspencion 	 	= l.id_tipo_suspencion
                        
                        where
                            b.codigo_estado = ''CERR'' and 
                            NVL(a.id_contratista,-1) in (
                                    select id_contratista
                                    from aire.v_ctn_contratos
                                    where UPPER(prefijo_actividad) = ''G_SCR''
                                    and UPPER(ind_activo) = ''S''
                                    union all
                                    select -1 from dual
                            )  and a.id_contratista = '||v_objeto.id_contratista||'
                            ' || myDictionary('fecha').value || ' 
                            ' || myDictionary('id_contratista_persona').value || ' 
                            ' || myDictionary('id_zona').value || '                             
                    )
                    , txcnt as (
                        SELECT sum(1) AS MaxRows
                        FROM tx
                        ' || myDictionary('ServerSide_WhereAdd').value || '                        
                        ' || myDictionary('ServerSide_SortAdd').value || '                        
                    )
                    select
                          q.contratista
                        , q.territorial
                        , q.zona
                        , q.acta
                        , q.fechaejecucion
                        , q.fechainicial
                        , q.fechafinal
                        , q.fecha_Sincronizacion
                        , q.hora_Sincronizacion
                        , q.id_orden
                        , q.numero_orden

                        , q.nic
                        , q.ciudad
                        , q.barrio

                        , q.direccion
                        , q.tipo_orden
                        , q.tipo_trabajo
                        , q.tipo_proceso
                        , q.Accion
                        , q.SubAccion

                        , q.caracterizacion

                        , q.num_factura
                        -- , q.deuda_act
                        , q.deuda_ejec
                        , q.tarifa
                        
                        , q.tipo_actividad
                        , q.actividad

                        , q.tipo_suspension                        
                        , q.georreferencia
                        , q.vehiculo
                        , q.tipo_operativa
                        , q.id_contratista_persona
                        , q.nombre_contratista_persona
                        , q.observacion
                        , q.origen

                        , q.UrlDescargaActa

                        , acnt.MaxRows RegistrosTotales
                    from tx q, txcnt acnt
                    ' || myDictionary('ServerSide_WhereAdd').value || '
                    ' || myDictionary('ServerSide_SortAdd').value || '
                    OFFSET ' || pageNumber || ' ROWS FETCH NEXT ' || pageSize ||' ROWS ONLY
                ) loop
                apex_json.open_object();
                    apex_json.write(''nombre_contratista_persona'', c_datos.nombre_contratista_persona);
                    apex_json.write(''contratista'', c_datos.contratista);
                    apex_json.write(''territorial'', c_datos.territorial);
                    apex_json.write(''zona'', c_datos.zona);
                    apex_json.write(''acta'', c_datos.acta);
                    apex_json.write(''fechaejecucion'', c_datos.fechaejecucion);
                    apex_json.write(''fechainicial'', c_datos.fechainicial);
                    apex_json.write(''fechafinal'', c_datos.fechafinal);
                    apex_json.write(''fecha_Sincronizacion'', c_datos.fecha_Sincronizacion);
                    apex_json.write(''hora_Sincronizacion'', c_datos.hora_Sincronizacion);

                    apex_json.write(''id_orden'', c_datos.id_orden);
                    apex_json.write(''numero_orden'', c_datos.numero_orden);
                    apex_json.write(''nic'', c_datos.nic);
                    apex_json.write(''Ciudad'', c_datos.Ciudad);
                    apex_json.write(''barrio'', c_datos.barrio);
                    apex_json.write(''direccion'', c_datos.direccion);
                    apex_json.write(''tipo_orden'', c_datos.tipo_orden);
                    apex_json.write(''tipo_trabajo'', c_datos.tipo_trabajo);
                    apex_json.write(''tipo_proceso'', c_datos.tipo_proceso);
                    apex_json.write(''Accion'', c_datos.Accion);
                    apex_json.write(''SubAccion'', c_datos.SubAccion);

                    apex_json.write(''caracterizacion'', c_datos.caracterizacion);

                    apex_json.write(''num_factura'', c_datos.num_factura);
                    apex_json.write(''deuda_ejec'', c_datos.deuda_ejec);
                    apex_json.write(''tarifa'', c_datos.tarifa);
                    
                    apex_json.write(''tipo_actividad'', c_datos.tipo_actividad);
                    apex_json.write(''actividad'', c_datos.actividad);

                    apex_json.write(''tipo_suspension'', c_datos.tipo_suspension);                    
                    apex_json.write(''GPS'', json_object_t(c_datos.georreferencia).get_string(''latitud'') || '','' || json_object_t(c_datos.georreferencia).get_string(''longitud''));
                    apex_json.write(''vehiculo'', c_datos.vehiculo);                    
                    apex_json.write(''tipo_operativa'', c_datos.tipo_operativa);
                    apex_json.write(''id_contratista_persona'', c_datos.id_contratista_persona);
                    apex_json.write(''nombre_contratista_persona'', c_datos.nombre_contratista_persona);
                    apex_json.write(''observacion'', c_datos.observacion);
                    apex_json.write(''origen'', c_datos.origen);


                    apex_json.write(''UrlDescargaActa'', c_datos.UrlDescargaActa);
                    
                    
                    BEGIN :v1 := c_datos.RegistrosTotales; END;
                apex_json.close_object();
            end loop;
            apex_json.close_array();
            END;
            ';

            -- DBMS_OUTPUT.PUT_LINE('------------------------------------------------------------');
            dbms_output.PUT_LINE('Query: ' || v_dinamic_sql);

            EXECUTE IMMEDIATE replace(v_dinamic_sql,'xnx','&')
            USING IN v_ruta_web, OUT v_RegistrosTotales;

            apex_json.write('RegistrosTotales', v_RegistrosTotales);
        apex_json.close_object();
    apex_json.close_object();
    
    s_json := apex_json.get_clob_output;
    apex_json.free_output;
    
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Carga exitosa');
    
    exception
    when NO_DATA_FOUND then
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'No se encontraron tecnicos asociados');
        apex_json.free_output;
        apex_json.open_object();
        apex_json.write('codigo', 1);
        apex_json.write('mensaje', 'No se encontraron tecnicos asociados');
        apex_json.close_object();
        
        s_json := apex_json.get_clob_output;
        apex_json.free_output;

    when others then
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al crear el json: '|| sqlerrm);
        apex_json.free_output;
        apex_json.open_object();
        apex_json.write('codigo', 1);
        apex_json.write('mensaje', 'Error N '||v_id_log ||' al consultar el listado de ordenes.: ' || replace(replace(replace(sqlerrm,'"',''''),chr(13),''),chr(10),''));
        apex_json.close_object();
        
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
    END prc_consultar_reportes_ejecutados_contratista; 

END PKG_G_CARLOS_VARGAS_TEST13;