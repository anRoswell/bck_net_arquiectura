create or replace PACKAGE                            "AIRE".PKG_G_CARLOS_VARGAS_TEST12 AS

    procedure prc_crear_tbl_temporal (		
        e_json  in  clob,
        s_json 	out	clob
	);
    --/--OJO  se migro el 16/04/2024 a pkg_g_ordenes
    --/--OJO OJO NO UTILIZAR
    procedure prc_consultar_ordenes_area_central (		
        e_json  in  clob,
        s_json 	out	clob
	);

    --/--OJO  se migro el 16/04/2024 a pkg_g_ordenes
    --/--OJO OJO NO UTILIZAR
    procedure prc_parametros_iniciales_areacentral (		        
        s_json 	out	clob
	);

    --/--OJO NO UTILIZAR ESTE CAMBIO, FUE EL INTENTO DE LA TABLA TEMPORAL
    procedure prc_cierre_ordenes_masivo(
        e_json  in 	clob,
        s_json  out	clob
    );

END PKG_G_CARLOS_VARGAS_TEST12;
/

create or replace PACKAGE BODY                        "AIRE".PKG_G_CARLOS_VARGAS_TEST12 AS

    procedure prc_crear_tbl_temporal (
        e_json  in  clob,
        s_json 	out	clob
	) is		
        v_json_objeto           json_object_t;
        v_id_log		        aire.gnl_logs.id_log%type;
        v_respuesta		        aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_ordenes.prc_crear_tbl_temporal');
        v_nombre_tabla_tmp      VARCHAR(1000);
        v_id_soporte            aire.gnl_soportes.id_soporte%type;
        
        v_dinamic_sql           clob;
        v_json_output           clob;        
    BEGIN
    
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
    apex_json.free_output;
    apex_json.initialize_clob_output( p_preserve => false );
    
    -- parseamos el json
    v_json_objeto := json_object_t(e_json);
    -- extraemos los datos de la orden
    v_nombre_tabla_tmp      := v_json_objeto.get_string('destinationTable');
    v_id_soporte            := v_json_objeto.get_string('id_soporte');

    DBMS_OUTPUT.PUT_LINE('v_nombre_tabla_tmp: ' || v_nombre_tabla_tmp);
    DBMS_OUTPUT.PUT_LINE('v_id_soporte: ' || v_id_soporte);

        if v_nombre_tabla_tmp = 'ORD_ORDENES_CARGUE_TEMPORAL_CIERRE' then
            v_dinamic_sql := '
                CREATE TABLE AIRE.ORD_ORDENES_CARGUE_TEMPORAL_CIERRE_' || v_id_soporte || ' (
                    ORDEN NUMBER,
                    NUMERO_ORDEN VARCHAR2(100),
                    LECTURA VARCHAR2(100),
                    OBSERVACION VARCHAR2(2000),
                    CODIGO_TIPO_ORDEN VARCHAR2(100),
                    CODIGO_CAUSAL_CIERRE VARCHAR2(100),
                    ID_SOPORTE NUMBER,
                    USUARIO_REGISTRA VARCHAR2(50)
                )
            ';
            DBMS_OUTPUT.PUT_LINE('Query: ' || v_dinamic_sql);
            EXECUTE IMMEDIATE v_dinamic_sql;
        end if;
      
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Se genera la tabla exitosamente');
        apex_json.free_output;
        apex_json.open_object();
        apex_json.write('codigo', 0);
        apex_json.write('mensaje', 'Proceso realizado con éxito para la tabla: ' || v_nombre_tabla_tmp);
        apex_json.close_object();
        
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
    exception
    when others then
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al generar la tabla: '|| sqlerrm);
        apex_json.free_output;
        apex_json.open_object();
        apex_json.write('codigo', 1);
        apex_json.write('mensaje', 'Error N '|| v_id_log ||' al generar la tabla temporal.' || sqlerrm);
        apex_json.close_object();
        
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
    END prc_crear_tbl_temporal;
    
    --/--OJO  se migro el 16/04/2024 a pkg_g_ordenes
    --/--OJO OJO NO UTILIZAR
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
    v_codigo_suspencion             := v_json_objeto.get_string('suspension');

    DBMS_OUTPUT.PUT_LINE('v_codigo_suspencion: ' || v_codigo_suspencion);

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
        case 
            when v_objeto.id_contratista = -2 and (x.id_contratista is null or x.id_contratista is not null) then 1
            when v_objeto.id_contratista = -1 and x.id_contratista is null then 1
            when v_objeto.id_contratista > 0 and x.id_contratista = v_objeto.id_contratista then 1
            else 0 end = 1;


    SELECT
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
                                                when v_estados_con_contratista = 0 then ' or a.id_contratista is null '
                                            else ' ' end;

    myDictionary('id_contratista').value := case
                                                when v_id_orden > 0 then ' '
                                                when v_objeto.id_contratista = -1 then ' and a.id_contratista is null '
                                                --/ si contratista es todos y zona se eligio una, filtrar contratistas de esa zona.
                                                -- when v_objeto.id_contratista = -2 then ' and (a.id_contratista in (' || v_list_contratistas || ') or a.id_contratista is null ) ' --es todo!
                                                when v_objeto.id_contratista = -2 then ' and (a.id_contratista in (' || v_list_contratistas || ') ' || myDictionary('id_contratistaComplemento').value || ' ) ' --es todo!
                                                when v_objeto.id_contratista > 0 then ' and a.id_contratista = ' || v_objeto.id_contratista || ' '
                                            else ' ' end;
    myDictionary('id_zona').value := case
                                                when v_id_orden > 0 then ' '
                                                when v_objeto.id_zona = -1 then ' and a.id_zona is null '                                                
                                                when v_objeto.id_zona = -2 then ' and (a.id_zona in (' || v_list_zonas || ') or a.id_zona is null) ' --es todo!
                                                when v_objeto.id_zona > 0 then ' and a.id_zona = ' || v_objeto.id_zona || ' '
                                            else ' ' end;
    myDictionary('codigo_suspencion').value := case
                                                when v_id_orden > 0 then ' '
                                                when v_codigo_suspencion = '-1' then ' and l.codigo is null '                                                
                                                when v_codigo_suspencion = '-2' then ' ' --es todo!
                                                when v_codigo_suspencion != '-2' AND v_codigo_suspencion != '-1' then ' and UPPER(l.codigo) = ''' || UPPER(v_codigo_suspencion) || ''' '
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

    --/--OJO  se migro el 16/04/2024 a pkg_g_ordenes
    --/--OJO OJO NO UTILIZAR
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

        /*
        BEGIN
            AIRE.SP_INSERTAR_TMP02BORREME();
        --rollback; 
        END;      
        */

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
                                    select x.id_zona
                                    from aire.ctn_contratos_zona x
                                    where id_contrato in (
                                                select id_contrato
                                                from aire.v_ctn_contratos
                                                where prefijo_actividad = 'G_SCR'
                                                and id_contratista = c_datos.id_contratista
                                                ) and
                                    x.ind_activo = 'S'
                                ) loop
                                    apex_json.write(c_datos2.id_zona);
                                end loop;
                            apex_json.close_array();
                            apex_json.open_array('Brigadas');
                                -- Brigadas
                                for x_datos in (
                                    select
                                          a.id_contratista_persona
                                        , a.identificacion_contratista_persona
                                        , a.nombre_contratista_persona
                                        , a.id_tipo_brigada
                                    from aire.v_ctn_contratistas_brigada		a
                                    where a.id_contratista in (
                                        select
                                            id_contratista
                                        from aire.v_ctn_contratos
                                        where lower(prefijo_actividad) = 'g_scr'
                                        and lower(ind_activo) = 's'
                                    )
                                    and lower(a.ind_activo) = 's' and a.id_contratista = c_datos.id_contratista and lower(a.codigo_rol) = 'tecnico'
                                    order by a.id_contratista_persona
                                ) loop
                                apex_json.open_object();
                                    apex_json.write('id_contratista_persona', x_datos.id_contratista_persona);
                                    apex_json.write('identificacion_contratista_persona', x_datos.identificacion_contratista_persona);
                                    apex_json.write('nombre_contratista_persona', x_datos.nombre_contratista_persona);
                                    apex_json.write('id_tipo_brigada', x_datos.id_tipo_brigada);
                                    
                                    apex_json.open_array('zonas_brigada');
                                        for c_datos3 in (
                                            select
                                                z.id_zona
                                            from aire.sgd_usuarios_zona z
                                            inner join aire.sgd_usuarios usr on usr.id_usuario = z.id_usuario
                                            where usr.usuario = x_datos.identificacion_contratista_persona and z.ind_activo = 'S'
                                        ) loop
                                            apex_json.write(c_datos3.id_zona);
                                        end loop;
                                    apex_json.close_array();

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
        
        
    exception
        when others then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al crear el json: '|| sqlerrm);
            apex_json.free_output;
            apex_json.open_object();
            apex_json.write('codigo', 1);
            apex_json.write('mensaje', 'Error N '||v_id_log ||' al crear los parametros iniciales, '|| sqlerrm);
            apex_json.close_object();
            --v_json := apex_json.get_clob_output;
            s_json := apex_json.get_clob_output;
            apex_json.free_output;
            --s_json := v_json;
   END prc_parametros_iniciales_areacentral;

   procedure prc_cierre_ordenes_masivo(
        e_json  in 	clob,
        s_json  out	clob
    ) is
        v_nombre_up                                     varchar2(100) := 'pkg_g_ordenes.prc_cierre_ordenes_masivo';
        v_respuesta                                     aire.tip_respuesta := aire.tip_respuesta(mensaje => 'Registro exitoso.', nombre_up => v_nombre_up);
        v_id_soporte                                    aire.gnl_soportes.id_soporte%type;
        v_id_usuario_registra                           aire.sgd_usuarios.id_usuario%type;
        v_nombre_archivo                                varchar2(250);
        v_id_archivo                                    number;
        v_id_actividad                                  number;
        v_id_archivo_instancia                          number;
        v_fecha_inicio                                  timestamp;
        v_id_estado_orden                               number;
        type t_temp is table of aire.ord_ordenes_cargue_temporal_cierre%rowtype;
        v_temp t_temp;
        v_orden                                         aire.ord_ordenes%rowtype;
        v_orden_gestion                                 aire.ord_ordenes_gestion%rowtype;
        v_cantidad_asignadas                            number := 0;
        v_id_estado_orden_cerrada                       aire.ord_ordenes.id_estado_orden%type;
        v_id_estado_orden_legalizacion_fallida          aire.ord_ordenes.id_estado_orden%type;
        v_json_array_notificacion                       json_array_t  := json_array_t();
        v_notificacion                                  json_object_t := json_object_t();
        v_id_dispositivo                                aire.sgd_usuarios.id_dispositivo%type;
        v_contador_exito                                number := 0;
        v_contador_error                                number := 0;
        v_id_log		                                aire.gnl_logs.id_log%type;
        v_clave_proceso varchar2(50) := to_char(systimestamp, 'DDMMYYHHMISSFF');
        v_dinamic_sql                                   CLOB;

        procedure sb_escribir_respuesta(codigo in varchar2, mensaje in varchar2, id_archivo_instancia in number default null)
        is begin
            apex_json.free_output; 
            apex_json.initialize_clob_output( p_preserve => false );
            apex_json.open_object();
            apex_json.write('codigo', codigo);
            apex_json.write('mensaje',mensaje);
            apex_json.close_object();
            s_json := apex_json.get_clob_output;
            apex_json.free_output;
        end;

        procedure sb_actualizar_temp (numero_orden_temp in varchar2, codigo in varchar2, mensaje in varchar2)
        is 
        begin
            if codigo = '0' then
                v_contador_exito := v_contador_exito + 1;
            else
                v_contador_error := v_contador_error +1;
            end if;

            update aire.ord_ordenes_cargue_temporal_cierre
              set con_errores       = codigo 
                , desc_validacion   = mensaje
             where id_soporte       = v_id_soporte 
               and usuario_registra = v_id_usuario_registra
               and numero_orden     = numero_orden_temp;

            --    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, v_tipo_mensaje_error, 'despues marcar como error');
        exception
            when others then
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al actualizar la orden: '|| numero_orden_temp, v_clave_proceso);
                rollback;
        end;
    begin
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'ENTRO AL SP: '|| e_json);
        
        -- validamos el json de entrada
        if e_json is not json then
            sb_escribir_respuesta(1, 'JSON invalido');
            return;
        end if;

        -- sacamos la informacion del JSON
        v_id_soporte            := json_value(e_json, '$.id_soporte');
        v_id_usuario_registra   := json_value(e_json, '$.usuario_registra');
        v_nombre_archivo        := json_value(e_json, '$.nombre_archivo');

        -- {16/04/2024} insertar registros desde tabla temporal dinamica a tabla temporal oficial.
            v_dinamic_sql := '
                INSERT INTO AIRE.ORD_ORDENES_CARGUE_TEMPORAL
                (ORDEN,NUMERO_ORDEN,LECTURA,OBSERVACION,CODIGO_TIPO_ORDEN,CODIGO_CAUSAL_CIERRE,ID_SOPORTE,USUARIO_REGISTRA)
                SELECT 
                    ORDEN,NUMERO_ORDEN,LECTURA,OBSERVACION,CODIGO_TIPO_ORDEN,CODIGO_CAUSAL_CIERRE,ID_SOPORTE,USUARIO_REGISTRA
                FROM AIRE.ORD_ORDENES_CARGUE_TEMPORAL_'|| v_id_soporte ||'
            ';
            DBMS_OUTPUT.PUT_LINE('Query: ' || v_dinamic_sql);
            EXECUTE IMMEDIATE v_dinamic_sql;
        
        -- {16/04/2024} insertar registros desde tabla temporal dinamica a tabla temporal oficial.
            v_dinamic_sql := 'DROP TABLE AIRE.ORD_ORDENES_CARGUE_TEMPORAL_'|| v_id_soporte ||'';
            DBMS_OUTPUT.PUT_LINE('Query: ' || v_dinamic_sql);
            EXECUTE IMMEDIATE v_dinamic_sql;

            commit;

        -- consultamos el estado cerrada
        begin
            select id_estado_orden 
              into v_id_estado_orden_cerrada
              from aire.ord_estados_orden 
             where codigo_estado = 'CERR';
        exception
            when others then
                sb_escribir_respuesta(1, 'Error al consultar el estado de la orden: Cerrada - ' || sqlerrm);
                return;
        end;
        
        -- consultamos el estado legalizacion fallida
        begin
            select id_estado_orden
              into v_id_estado_orden_legalizacion_fallida
              from aire.ord_estados_orden
             where codigo_estado = 'LFAL';
        exception
            when others then
                sb_escribir_respuesta(1, 'Error al consultar el estado de la orden: Legalizacion fallida - ' || sqlerrm);
                return;
        end;

        -- consultamos el archivo o el cargue
        begin
            select id_archivo
              into v_id_archivo
              from aire.gnl_archivos 
             where codigo = 'MCIO';
        exception 
            when others then
                sb_escribir_respuesta(1, 'Error al consultar el cargue: PRSO');
                return;
        end;

        -- consultamos la actividad
        begin
            select id_actividad
              into v_id_actividad
              from aire.gnl_actividades
             where prefijo    = 'G_SCR' 
               and ind_activo = 'S';
        exception 
            when others then
                sb_escribir_respuesta(1, 'Error al consultar la actividad: G_SCR');
                return;
        end;
        
        -- consultamos cuando inicio el cargue
        select min(fecha_registra)
          into v_fecha_inicio
          from aire.ord_ordenes_cargue_temporal_cierre
         where id_soporte       =  v_id_soporte
           and usuario_registra = v_id_usuario_registra;

        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Antes del bulk collect');
        select *
          bulk collect into v_temp
          from aire.ord_ordenes_cargue_temporal_cierre
         where id_soporte       = v_id_soporte 
           and usuario_registra = v_id_usuario_registra; 

        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Despues del bulk collect');
        -- recorremos los registros
        for i in 1..v_temp.count loop
           -- v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, v_tipo_mensaje_error, 'Procesando fila'||v_temp(i).orden||' - '||v_temp(i).numero_orden, v_clave_proceso);

            v_orden_gestion.lectura                := v_temp(i).lectura;
            v_orden_gestion.observacion            := v_temp(i).observacion;
            v_orden_gestion.fecha_cierre           := sysdate;
            v_orden_gestion.fecha_inicio_ejecucion := sysdate;
            v_orden_gestion.fecha_fin_ejecucion    := sysdate;

            -- consultamos la orden
            begin                
                select *
                 into v_orden
                 from aire.ord_ordenes
                where numero_orden    = to_number(v_temp(i).numero_orden)
                  and id_tipo_orden   = (select id_tipo_orden from aire.ord_tipos_orden where codigo_tipo_orden = v_temp(i).codigo_tipo_orden)
                  and id_estado_orden in (select id_estado_orden from aire.ord_estados_orden where codigo_estado in ('SEAS','SPEN','SBAN','SASI'));

                v_orden_gestion.id_orden               := v_orden.id_orden;
                v_orden_gestion.id_contratista_persona := v_orden.id_contratista_persona;
            exception
                when others then
                    -- actualizar registro con errores
                    sb_actualizar_temp(v_temp(i).numero_orden, '1', 'La orden '||v_temp(i).numero_orden||' no existe, o no esta asociada al tipo de orden '||v_temp(i).codigo_tipo_orden||', o no tiene un estado valido - '|| sqlerrm);
                    continue;
            end;
            
            --v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, v_tipo_mensaje_error, 'despues de consultar la orden', v_clave_proceso);

            -- consultamos la anomalia
            begin
                select a.id_anomalia
                     , a.id_resultado
                     , case when b.ind_ejecuta_actividad = 'N' then 'S' else 'N' end
                  into v_orden_gestion.id_anomalia
                     , v_orden_gestion.id_resultado
                     , v_orden_gestion.ind_anomalia
                  from aire.ord_anomalias  a
                  join aire.ord_resultados b on a.id_resultado = b.id_resultado
                 where a.codigo_anomalia = v_temp(i).codigo_causal_cierre
                   and a.id_tipo_orden   = v_orden.id_tipo_orden;
            exception
                when others then
                    -- actualizar registro con errores
                    sb_actualizar_temp(v_temp(i).numero_orden, '1', 'La anomalia '||v_temp(i).codigo_causal_cierre||' no existe o no esta asociada al tipo de la orden');
                    continue;
            end;

            --v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, v_tipo_mensaje_error, 'despues de consultar la anomalia', v_clave_proceso);

            -- consultamos la brigada
            begin
                select id_contratista_brigada 
                  into v_orden_gestion.id_contratista_brigada
                  from aire.ctn_contratistas_brigada
                 where id_contratista_persona = v_orden.id_contratista_persona
                   and ind_activo             = 'S';
            exception
                when others then
                    v_orden_gestion.id_contratista_brigada := null;
            end;

            --v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, v_tipo_mensaje_error, 'despues de consultar la brigada', v_clave_proceso);

            -- registramos la gestion de la orden para hacer el cierre
            aire.pkg_p_ordenes.prc_registrar_orden_gestion(
                e_orden_gestion	=> v_orden_gestion,
                s_respuesta	    => v_respuesta
            );

            --Validamos la respuesta del procedimiento
            if v_respuesta.codigo <> 0 then
                sb_actualizar_temp(v_temp(i).numero_orden, '1', 'Error al cerrar la orden: Error al registrar la gestion de la orden: ' || v_temp(i).numero_orden || ' - '|| v_respuesta.mensaje);
                continue;
            end if;

            --v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, v_tipo_mensaje_error, 'despues de registrar la gestion', v_clave_proceso);

            -- enviamos a cerrar la orden en OSF si el origen es OSF
            if v_orden.origen = 'OSF' then
                
                --v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, v_tipo_mensaje_error, 'orden de OSF se envia alegalizar', v_clave_proceso);

                aire.pkg_g_ordenes.prc_cerrar_orden(
                    e_id_orden_gestion	=> v_orden_gestion.id_orden_gestion,
                    s_respuesta			=> v_respuesta
                );

                --v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, v_tipo_mensaje_error, 'despues de enviar a legalizar', v_clave_proceso);

                --Validamos la respuesta del procedimiento, si hubo error marcamos la orden como legalizacion fallida
                if v_respuesta.codigo <> 0 then

                    --v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, v_tipo_mensaje_error, 'legalizacion fallida se manda a actualizar', v_clave_proceso);

                    v_orden.id_estado_orden := v_id_estado_orden_legalizacion_fallida;

                    -- actualizamos el estado de la orden
                    aire.pkg_p_ordenes.prc_actualizar_orden(
                        e_orden	    => v_orden,
                        s_respuesta	=> v_respuesta
                    );

                    --v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, v_tipo_mensaje_error, 'despues de enviar a actualizar', v_clave_proceso);

                    -- si ocurrio un error al actualizar la orden
                    if v_respuesta.codigo <> 0 then
                        sb_actualizar_temp(v_temp(i).numero_orden, '1', 'Error al cerrar la orden: Error al actualizar orden como legalizacion fallida: orden: ' || v_temp(i).numero_orden || ' - '||v_respuesta.mensaje);                    
                        continue;
                    end if;

                    if v_orden.id_contratista_persona is not null then
                        
                        begin
                            -- notificamos al usuario
                            select id_dispositivo
                              into v_id_dispositivo
                              from aire.sgd_usuarios
                             where id_persona = (
                                                    select id_persona
                                                      from aire.ctn_contratistas_persona
                                                     where id_contratista_persona = v_orden.id_contratista_persona
                                );

                            v_notificacion := json_object_t();
                            v_notificacion.put('id_dispositivo', v_id_dispositivo);
                            v_notificacion.put('ind_android', true);
                            v_notificacion.put('titulo', 'Desasignación ordenes SCR');
                            v_notificacion.put('cuerpo', 'Se desasignaron ordenes de su gestión');
                            v_notificacion.put('estado', 'desasignada');
                            v_notificacion.put('tipo', 'gestion_ordenes_scr');
                            v_notificacion.put('fecha_envio', to_char(systimestamp, 'DD/MM/YYYY HH:MI:SS:FF PM'));

                            v_json_array_notificacion.append(v_notificacion);

                        exception
                            when others then 
                                dbms_output.put_line('Error al armar la notificacion OSF: '|| sqlerrm);
                                null;
                        end;
                        
                    end if;

                    sb_actualizar_temp(v_temp(i).numero_orden, '1', 'Se marco orden ' || v_temp(i).numero_orden || ' como legalizacion fallida.');                
                    continue;
                end if;
            end if;

            if v_orden.id_contratista_persona is not null then
                begin
                    -- notificamos al usuario
                    select id_dispositivo
                      into v_id_dispositivo
                      from aire.sgd_usuarios
                     where id_persona = (
                                            select id_persona
                                              from aire.ctn_contratistas_persona
                                             where id_contratista_persona = v_orden.id_contratista_persona
                        );

                    v_notificacion := json_object_t();
                    v_notificacion.put('id_dispositivo', v_id_dispositivo);
                    v_notificacion.put('ind_android', true);
                    v_notificacion.put('titulo', 'Desasignación ordenes SCR');
                    v_notificacion.put('cuerpo', 'Se desasignaron ordenes de su gestión');
                    v_notificacion.put('estado', 'desasignada');
                    v_notificacion.put('tipo', 'gestion_ordenes_scr');
                    v_notificacion.put('fecha_envio', to_char(systimestamp, 'DD/MM/YYYY HH:MI:SS:FF PM'));

                    v_json_array_notificacion.append(v_notificacion);

                exception
                    when others then 
                        dbms_output.put_line('Error al armar la notificacion OP360: '|| sqlerrm);
                        null;
                end;
                
            end if;

            -- si el cierre fue exitoso capturamos informacion de cierre
            v_orden.fecha_cierre        := sysdate;
            v_orden.id_usuario_cierre   := v_id_usuario_registra;
            v_orden.id_estado_orden     := v_id_estado_orden_cerrada;

            -- cambiamos el estado de la orden a cerrada
            aire.pkg_p_ordenes.prc_actualizar_orden(
                e_orden	    => v_orden,
                s_respuesta	=> v_respuesta
            );
            

            -- Validamos la respuesta del procedimiento
            if v_respuesta.codigo <> 0 then
                sb_actualizar_temp(v_temp(i).numero_orden, '1', 'Error al cerrar la orden: Error al actualizar orden ' || v_temp(i).numero_orden || ' como cerrada: '||v_respuesta.mensaje);
                continue;
            end if;  

            --v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, v_tipo_mensaje_error, 'despues de enviar a actualizar cerrada', v_clave_proceso);

            sb_actualizar_temp(v_temp(i).numero_orden, '0', 'Ok');
        end loop;
        
        -- se mandan las notificaciones en segundo plano
        aire.pkg_g_generales.prc_enviar_notificaciones_push_segundo_plano(
            e_notificaciones => v_json_array_notificacion.to_clob, 
            s_respuesta	     => v_respuesta
        );

        --v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, v_tipo_mensaje_error, 'despues de enviar notificaciones', v_clave_proceso);

        insert into aire.gnl_archivos_instancia (
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
        ) values(
            v_id_archivo,
            v_nombre_archivo,
            v_contador_exito + v_contador_error,
            v_contador_exito,
            v_contador_error,
            v_fecha_inicio,
            localtimestamp,
            localtimestamp - v_fecha_inicio,
            v_id_usuario_registra,
            localtimestamp,
            163, --Finalizado 
            'Archivo procesado con exito',
            v_id_soporte
        ) returning id_archivo_instancia into v_id_archivo_instancia;

        --v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, v_tipo_mensaje_error, 'despues de registrar archivo intancia', v_clave_proceso);

        -- registramos la instancia detalle
        insert into aire.gnl_archivos_instancia_detalle(
              id_archivo_instancia
            , numero_fila
            , estado
            , observaciones
        ) select v_id_archivo_instancia id_archivo_instancia
               , rownum numero_fila
               , case when con_errores = 1 then 'ERROR' else 'OK' end estado
               , desc_validacion observaciones
            from aire.ord_ordenes_cargue_temporal_cierre  
           where id_soporte       = v_id_soporte 
             and usuario_registra = v_id_usuario_registra;

        --v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, v_tipo_mensaje_error, 'despues de registrar archivo intancia detalle', v_clave_proceso);

        delete aire.ord_ordenes_cargue_temporal_cierre
         where id_soporte       = v_id_soporte 
           and usuario_registra = v_id_usuario_registra;
        
        --v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, v_tipo_mensaje_error, 'despues de eliminar ord_ordenes_cargue_temporal_cierre', v_clave_proceso);
        
        commit;

        sb_escribir_respuesta(0, 'Archivo procesado');
    exception
        when others then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error interno: '|| sqlerrm);
            sb_escribir_respuesta(1, 'Error al procesar ordenes:'|| sqlerrm);
    end prc_cierre_ordenes_masivo;


END PKG_G_CARLOS_VARGAS_TEST12;