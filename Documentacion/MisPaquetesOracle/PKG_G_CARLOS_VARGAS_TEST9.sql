CREATE OR REPLACE package "AIRE".pkg_g_carlos_vargas_test9 as
    --/-- !NO USAR YA QUEDO MIGRADO
    procedure prc_consultar_reportes_ejecutados_contratista (		
        e_json  in  clob,
        s_json 	out	clob
	);

end pkg_g_carlos_vargas_test9;
/

CREATE OR REPLACE package body "AIRE".pkg_g_carlos_vargas_test9 as
    --/-- !NO USAR YA QUEDO MIGRADO
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

    DBMS_OUTPUT.PUT_LINE('v_id_persona: ' || v_id_persona);

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

    DBMS_OUTPUT.PUT_LINE('v_objeto.id_contratista: ' || v_objeto.id_contratista);

    v_objeto.id_contratista_persona := v_json_objeto.get_string('id_contratista_persona');
    DBMS_OUTPUT.PUT_LINE('v_objeto.id_contratista_persona: ' || v_objeto.id_contratista_persona);

    v_objeto.id_zona                := v_json_objeto.get_string('id_zona');
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

    DBMS_OUTPUT.PUT_LINE('v_json_objeto2: ' || v_json_objeto.to_string());
    DBMS_OUTPUT.PUT_LINE('v_f_inicio: ' || v_f_inicio);
    DBMS_OUTPUT.PUT_LINE('v_f_inicio2: ' || v_f_inicio2);
    DBMS_OUTPUT.PUT_LINE('v_f_fin: ' || v_f_fin);
    DBMS_OUTPUT.PUT_LINE('v_f_fin2: ' || v_f_fin2);
    DBMS_OUTPUT.PUT_LINE('LENGTH(v_f_inicio): ' || LENGTH(v_f_inicio));
    DBMS_OUTPUT.PUT_LINE('LENGTH(v_f_fin): ' || nvl(LENGTH(v_f_fin),0));

    myDictionary('fecha').value := case 
                                when LENGTH(v_f_inicio) != 0 and nvl(LENGTH(v_f_fin),0) = 0 then ' and trunc(a.fecha_registro) = ''' || v_f_inicio2 || ''''
                                when LENGTH(v_f_inicio) = 0 and  nvl(LENGTH(v_f_fin),0) = 0 then ' '
                                when LENGTH(v_f_inicio) != 0 and nvl(LENGTH(v_f_fin),0) != 0 then ' and trunc(a.fecha_registro) between ' || '''' || v_f_inicio2 || '''' || ' and ' || '''' || v_f_fin2 || ''''
                                else ' ' end;
    DBMS_OUTPUT.PUT_LINE('fecha: ' || myDictionary('fecha').value);                                
    
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
    DBMS_OUTPUT.PUT_LINE('v_objeto.id_contratista_persona: ' || v_objeto.id_contratista_persona);
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
    DBMS_OUTPUT.PUT_LINE('myDictionary(''id_contratista_persona''): ' || myDictionary('id_contratista_persona').value);                                           
    DBMS_OUTPUT.PUT_LINE('myDictionary(''id_zona''): ' || myDictionary('id_zona').value);                                           
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
    DBMS_OUTPUT.PUT_LINE('v_list_zonas: ' || 'ok');
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
                            , min(nvl(q.acta,''0'')) over() as acta
                            , min(q.fecha_ejecucion) over() as fechaejecucion
                            , min(q.fecha_inicio_ejecucion) over() as fechainicial
                            , min(q.fecha_cierre) over() as fechafinal
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

                            , ''Accion: '' || acc.descripcion || '', Anomalia: '' || qa.descripcion as Accion
                            , ''SubAccion: '' || ascx.descripcion || '', SubAnomalia: '' || qsa.descripcion as SubAccion

                            , k.descripcion as tipo_trabajo
                            , n.expired_periodos as num_factura
                            --, n.expired_balance as deuda_act
                            , n.expired_balance as deuda_ejec
                            , d.tarifa
                            , tc.descripcion as  tipo_actividad                            
                            , l.descripcion as tipo_suspencion                            
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
                        left join aire.ord_ordenes_gestion                      q       on q.id_orden = a.id_orden
                        left join aire.ord_anomalias                            qa      on q.id_anomalia = qa.id_anomalia
                        left join aire.ord_subanomalias                         qsa     on q.id_anomalia = qsa.id_anomalia and q.id_subanomalia = qsa.id_subanomalia

                        left join aire.gnl_dominios_valor                       gdv2    on q.id_estado_predio = gdv2.id_dominio_valor
                        left join aire.ord_ordenes_gestion_acciones             oga on oga.id_orden_gestion = q.id_orden_gestion
                        left join aire.scr_acciones                             acc on acc.id_accion = oga.id_accion
                        left join aire.scr_subacciones                          ascx on ascx.id_accion = oga.id_accion and ascx.id_subaccion = oga.id_subaccion

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
                        
                        left join aire.ctn_contratistas_brigada                 j2 on a.id_contratista_persona  = j2.id_contratista_persona and j2.ind_activo = ''S''
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

                        , q.num_factura
                        -- , q.deuda_act
                        , q.deuda_ejec
                        , q.tarifa
                        , q.tipo_actividad
                        , q.tipo_suspencion                        
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

                    apex_json.write(''num_factura'', c_datos.num_factura);
                    apex_json.write(''deuda_ejec'', c_datos.deuda_ejec);
                    apex_json.write(''tarifa'', c_datos.tarifa);
                    apex_json.write(''tipo_actividad'', c_datos.tipo_actividad);
                    apex_json.write(''tipo_suspencion'', c_datos.tipo_suspencion);                    
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
            DBMS_OUTPUT.PUT_LINE('Query: ' || v_dinamic_sql);

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
        apex_json.write('mensaje', 'Error N '||v_id_log ||' al consultar el listado de ordenes.: ' || replace(sqlerrm,'"',''));
        apex_json.close_object();
        
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
    END prc_consultar_reportes_ejecutados_contratista;

end pkg_g_carlos_vargas_test9;