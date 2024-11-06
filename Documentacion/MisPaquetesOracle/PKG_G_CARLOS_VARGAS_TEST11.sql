create or replace PACKAGE      "AIRE".PKG_G_CARLOS_VARGAS_TEST11
AS

    v_tipo_mensaje_seguimiento		aire.gnl_logs.tipo_mensaje%type := aire.pkg_p_generales.v_tipo_mensaje_seguimiento;
	v_tipo_mensaje_error			aire.gnl_logs.tipo_mensaje%type := aire.pkg_p_generales.v_tipo_mensaje_error;

    --/--- OJO NO UTILIZAR
    procedure prc_consultar_reportes_traza (
        e_json  in  clob,
        s_json 	out	clob
	);

    --/--- OJO NO UTILIZAR
    procedure prc_consultar_reportes_traza_contratista (
        e_json  in  clob,
        s_json 	out	clob
	);

END PKG_G_CARLOS_VARGAS_TEST11;
/

create or replace PACKAGE BODY      "AIRE".PKG_G_CARLOS_VARGAS_TEST11 AS
    
    PROCEDURE prc_consultar_reportes_traza (
        e_json  IN  CLOB,
        s_json  OUT CLOB
    ) IS
        v_json_objeto       json_object_t;
        v_json              clob;
        v_id_log            aire.gnl_logs.id_log%type;
        v_respuesta         aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_carlos_vargas_test11.prc_consultar_reportes_traza');
        v_objeto      	    aire.ord_ordenes_historico%rowtype;

        pageNumber          NUMBER;
        pageSize            NUMBER;
        sortColumn          VARCHAR(100 BYTE);
        sortDirection       VARCHAR(100 BYTE);
        v_RegistrosTotales  NUMBER;
        v_f_inicio          varchar2(400);
        v_f_inicio2         varchar2(400);
        v_f_fin             varchar2(400);
        v_f_fin2            varchar2(400);
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
        --Nuevas variables
        v_id_zonas varchar2(4000);
        v_id_contratista varchar2(4000);
    begin
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, v_tipo_mensaje_seguimiento, 'Entro a realizar consulta del log');
        
        -- Deserializar JSON
        apex_json.free_output;
        apex_json.initialize_clob_output( p_preserve => false );
        v_json_objeto := json_object_t(e_json);

        --ServerSide
        v_ServerSide                    := v_json_objeto.get_Object('ServerSideJson');
        v_filtersString                 := v_ServerSide.get_string('filtersString');
        v_sortString                    := v_ServerSide.get_string('sortString');
        pageNumber                      := v_ServerSide.get_number('first');
        pageSize                        := v_ServerSide.get_number('rows');
        
        v_f_inicio                      := v_json_objeto.get_string('fechaInicial');
        v_f_fin                         := v_json_objeto.get_string('fechaFinal');
        
        --Obtengo los campos para filtrar
        v_id_contratista                := v_json_objeto.get_clob('id_contratista');
        v_id_zonas                      := v_json_objeto.get_clob('id_zona');
        --DBMS_OUTPUT.PUT_LINE('id_contratista: ' || v_id_contratista);
        --DBMS_OUTPUT.PUT_LINE('id_zona: ' || v_id_zonas);
        
        v_f_inicio2 := TRUNC(TO_TIMESTAMP(v_f_inicio, 'YYYY-MM-DD"T"HH24:MI:SS"Z"'));
        v_f_fin2    := TRUNC(TO_TIMESTAMP(v_f_fin, 'YYYY-MM-DD"T"HH24:MI:SS"Z"'));

        --DBMS_OUTPUT.PUT_LINE('fecha inicio: ' || v_f_inicio2);
        --DBMS_OUTPUT.PUT_LINE('fecha fin: ' || v_f_fin2);

        --- -//-->INICIO DINAMIC SQL 12/04/2024
        --- -//- Filtros Staticos                      
        myDictionary('fecha').value := case
                                when LENGTH(v_f_inicio) != 0 and nvl(LENGTH(v_f_fin),0) = 0 then ' trunc(a.FECHA_REGISTRO) = ''' || v_f_inicio2 || ''''
                                when LENGTH(v_f_inicio) = 0 and  nvl(LENGTH(v_f_fin),0) = 0 then ' '
                                when LENGTH(v_f_inicio) != 0 and nvl(LENGTH(v_f_fin),0) != 0 then ' trunc(a.FECHA_REGISTRO) between ' || '''' || v_f_inicio2 || '''' || ' and ' || '''' || v_f_fin2 || ''''
                                else ' ' end;

        /*myDictionary('id_contratista').value := case
                                                when v_objeto.id_contratista = -1 then ' and a.id_contratista is null '
                                                when v_objeto.id_contratista = -2 then ' ' --es todo!
                                                when v_objeto.id_contratista > 0 then ' and a.id_contratista = ' || v_objeto.id_contratista || ' '
                                            else ' ' end;*/
        myDictionary('add_null').value := case
                                        when LENGTH(NVL(v_id_contratista, '')) > 2 and INSTR(NVL(v_id_contratista, ''), '-1') > 0 then ' or a.id_contratista is null '
                                        else '' end;
        myDictionary('id_contratista').value := case
                                                when v_id_contratista = '-1' then ' and a.id_contratista is null '
                                                when v_id_contratista = '-2' then ' ' --es todo!
                                                when NVL(v_id_contratista, '')  NOT IN (' ', '-1','-2') then ' and ( a.id_contratista IN(
                                                                                                                SELECT REGEXP_SUBSTR('''|| v_id_contratista ||''', ''[^,]+'', 1, LEVEL) AS contratistas FROM DUAL
                                                                                                                CONNECT BY REGEXP_SUBSTR('''|| v_id_contratista ||''', ''[^,]+'', 1, LEVEL) IS NOT NULL
                                                                                                            ) ' || myDictionary('add_null').value || ' ) '
                                            else ' ' end;

        myDictionary('id_zona').value := case
                                                when v_id_zonas = '-1' then ' and a.id_zona is null '
                                                when v_id_zonas = '-2' then ' ' --es todo!
                                                when NVL(v_id_zonas, '')  NOT IN (' ', '-1','-2') then ' and a.id_zona IN (
                                                                                                                SELECT REGEXP_SUBSTR('''|| v_id_zonas ||''', ''[^,]+'', 1, LEVEL) AS zonas FROM DUAL
                                                                                                                CONNECT BY REGEXP_SUBSTR('''|| v_id_zonas ||''', ''[^,]+'', 1, LEVEL) IS NOT NULL
                                                                                                            )'
                                            else ' ' end;
        

        apex_json.open_object();
        apex_json.write('codigo', 0);
        apex_json.write('mensaje', 'Consulta exitosa');
        apex_json.open_object('datos');
        
        v_dinamic_sql := '
        DECLARE
        BEGIN
            apex_json.open_array(''reporte_traza'');
            for c_datos in (
                    WITH t as (
                        select 
                            a.id_orden_historico,
                            a.fecha_registro_historico,
                            a.estado_historico,
                            a.id_orden, --Este es el unico id que se necesita
                            b.codigo_tipo_orden as tipo, --a.id_tipo_orden,
                            a.numero_orden,
                            c.descripcion as estado, ---c.codigo_estado, a.id_estado_orden,
                            case when nvl(e.nombres,''-1'') = ''-1'' then null else e.nombres || '' '' || e.apellidos || ''_'' || a.id_contratista end as contratista, --a.id_contratista,
                            --f.nic, --a.id_cliente,
                            g.nombre as nombre_territorial, --a.id_territorial,
                            h.nombre as nombre_zona, --a.id_zona,
                            a.direcion,
                            a.fecha_creacion as fecha_generacion,
                            a.fecha_cierre,
                            i.usuario as usuario_cierre, --a.id_usuario_cierre,
                            a.descripcion as descripcion_de_tipo,
                            a.comentarios,
                            a.acta,
                            j.nombre as nombre_actividd, --a.id_actividad,
                            case when nvl(l.nombres,''-1'') = ''-1'' then null else l.nombres || '' '' || l.apellidos end as nombre_contratista_persona, --a.id_contratista_persona,
                            m.descripcion as tipo_trabajo, --a.id_tipo_trabajo,
                            n.descripcion as tipo_suspension, --a.id_tipo_suspencion,
                            a.actividad_orden,
                            a.fecha_estimada_respuesta,
                            a.numero_camp,
                            a.comentario_orden_servicio_num1,
                            a.comentario_orden_servicio_num2,
                            a.observacion_rechazo,
                            a.fecha_rechazo,
                            a.origen,
                            a.fecha_registro,
                            o.nombre as nombre_barrio, --a.id_barrio,
                            a.fecha_asigna_contratista,
                            a.fecha_asigna_tecnico,
                            a.nic,
                            a.deuda,
                            a.tecnico,
                            a.departamento,
                            a.municipio,
                            a.numero_factura,
                            a.tipo_brigada,
                            a.antiguedad
                        from aire.ord_ordenes_historico             a
                        left join aire.ord_tipos_orden              b on a.id_tipo_orden = b.id_tipo_orden
                        left join aire.ord_estados_orden            c on a.id_estado_orden = c.id_estado_orden
                        left join aire.ctn_contratistas             d on a.id_contratista = d.id_contratista
                        left join aire.gnl_personas                 e on d.id_persona = e.id_persona
                        left join aire.gnl_clientes                 f on f.id_cliente = a.id_cliente
                        left join aire.gnl_territoriales            g on g.id_territorial = a.id_territorial
                        left join aire.gnl_zonas                    h on h.id_zona = a.id_zona
                        left join aire.sgd_usuarios                 i on i.id_usuario = a.id_usuario_cierre
                        left join aire.gnl_actividades              j on j.id_actividad = a.id_actividad
                        left join aire.ctn_contratistas_persona     k on a.id_contratista_persona = k.id_contratista_persona
                        left join aire.v_gnl_personas               l on k.id_persona = l.id_persona
                        left join aire.ord_tipos_trabajo            m on a.id_tipo_trabajo = m.id_tipo_trabajo
                        left join aire.scr_tipos_suspencion         n on a.id_tipo_suspencion = n.id_tipo_suspencion
                        left join aire.gnl_barrios                  o on a.id_barrio = o.id_barrio
                        where ' || myDictionary('fecha').value || '
                        ' || myDictionary('id_contratista').value || ' 
                        ' || myDictionary('id_zona').value || '
                    ), t2 as (
                        select count(*) RegistrosTotales from t
                    )
                    select
                            t.*,
                            sysdate as fecha_consulta
                        ,t2.RegistrosTotales
                    from t, t2
                    OFFSET ' || pageNumber || ' ROWS FETCH NEXT ' || pageSize ||' ROWS ONLY
                ) loop
                    apex_json.open_object();
                    apex_json.write(''id_orden_historico'', c_datos.id_orden_historico, true);
                    apex_json.write(''fecha_registro_historico'', c_datos.fecha_registro_historico);
                    apex_json.write(''estado_historico'', c_datos.estado_historico, true);
                    apex_json.write(''id_orden'', c_datos.id_orden, true);
                    apex_json.write(''orden'', c_datos.numero_orden, true);
                    --apex_json.write(''id_cliente'', c_datos.id_cliente, true);
                    apex_json.write(''tipo'', c_datos.tipo, true);
                    apex_json.write(''nombre_contratista_persona'', c_datos.nombre_contratista_persona, true);
                    apex_json.write(''nombre_barrio'', c_datos.nombre_barrio, true);
                    apex_json.write(''estado'', c_datos.estado, true);
                    apex_json.write(''contratista'', c_datos.contratista, true);
                    apex_json.write(''nombre_territorial'', c_datos.nombre_territorial, true);
                    apex_json.write(''tipo_suspension'', c_datos.tipo_suspension, true);
                    apex_json.write(''nombre_zona'', c_datos.nombre_zona, true);
                    apex_json.write(''direcion'', c_datos.direcion, true);
                    apex_json.write(''fecha_generacion'', c_datos.fecha_generacion);
                    apex_json.write(''fecha_cierre'', c_datos.fecha_cierre);
                    apex_json.write(''usuario_cierre'', c_datos.usuario_cierre, true);
                    apex_json.write(''descripcion_de_tipo'', c_datos.descripcion_de_tipo, true);
                    apex_json.write(''comentarios'', c_datos.comentarios, true);
                    apex_json.write(''acta'', c_datos.acta, true);
                    apex_json.write(''nombre_actividd'', c_datos.nombre_actividd, true);
                    apex_json.write(''tipo_trabajo'', c_datos.tipo_trabajo, true);
                    apex_json.write(''actividad_orden'', c_datos.actividad_orden, true);
                    apex_json.write(''fecha_estimada_respuesta'', c_datos.fecha_estimada_respuesta);
                    apex_json.write(''numero_camp'', c_datos.numero_camp, true);
                    apex_json.write(''comentario_orden_servicio_num1'', c_datos.comentario_orden_servicio_num1, true);
                    apex_json.write(''comentario_orden_servicio_num2'', c_datos.comentario_orden_servicio_num2, true);
                    apex_json.write(''observacion_rechazo'', c_datos.observacion_rechazo, true);
                    apex_json.write(''fecha_rechazo'', c_datos.fecha_rechazo);
                    apex_json.write(''origen'', c_datos.origen, true);
                    apex_json.write(''fecha_ingreso_op360'', c_datos.fecha_registro);
                    apex_json.write(''fecha_asigna_contratista'', c_datos.fecha_asigna_contratista);
                    apex_json.write(''fecha_asigna_tecnico'', c_datos.fecha_asigna_tecnico);
                    apex_json.write(''nic'', c_datos.nic, true);
                    apex_json.write(''deuda'', c_datos.deuda, true);
                    apex_json.write(''tecnico'', c_datos.tecnico, true);
                    apex_json.write(''departamento'', c_datos.departamento, true);
                    apex_json.write(''municipio'', c_datos.municipio, true);
                    apex_json.write(''numero_factura'', c_datos.numero_factura, true);
                    apex_json.write(''tipo_brigada'', c_datos.tipo_brigada, true);
                    apex_json.write(''antiguedad'', c_datos.antiguedad, true);
                    apex_json.write(''fecha_consulta'', c_datos.fecha_consulta);
                    
                    BEGIN :v_RegistrosTotales := c_datos.RegistrosTotales; END;
                    apex_json.close_object();
                end loop;
            apex_json.close_array();
        END;
        ';
        DBMS_OUTPUT.PUT_LINE('Query: ' || v_dinamic_sql);

        /*insert into aire.tmpQueryBorreme
        (query)
        select v_dinamic_sql from dual;*/

        EXECUTE IMMEDIATE v_dinamic_sql
        USING OUT v_RegistrosTotales;
        
        apex_json.write('RegistrosTotales', v_RegistrosTotales);
        apex_json.close_object();
        apex_json.close_object();
        
        --Si todo sale bien devolver mensaje.
        s_json := apex_json.get_clob_output;
        apex_json.free_output;

        --LOG
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Consulta exitosa');

    exception
        when others then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, v_tipo_mensaje_error, 'Error al realizar la consulta: '|| sqlerrm);
            apex_json.free_output;
            apex_json.open_object();
            apex_json.write('codigo', 1);
            apex_json.write('mensaje', 'Error N '||v_id_log ||' al realizar la consulta');
            apex_json.close_object();
            v_json := apex_json.get_clob_output;
            apex_json.free_output;
            s_json := v_json;
    END prc_consultar_reportes_traza;

    PROCEDURE prc_consultar_reportes_traza_contratista (
        e_json  IN  CLOB,
        s_json  OUT CLOB
    ) IS
        v_json_objeto       json_object_t;
        v_json              clob;
        v_id_log            aire.gnl_logs.id_log%type;
        v_respuesta         aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_carlos_vargas_test11.prc_consultar_reportes_traza_contratista');
        v_objeto      	    aire.ord_ordenes_historico%rowtype;
        v_id_persona        VARCHAR2(50 BYTE);
        v_id_usuario        VARCHAR2(50 BYTE);
        v_Contador          NUMBER := 0;

        pageNumber          NUMBER;
        pageSize            NUMBER;
        sortColumn          VARCHAR(100 BYTE);
        sortDirection       VARCHAR(100 BYTE);
        v_RegistrosTotales  NUMBER;
        v_f_inicio          varchar2(400);
        v_f_inicio2         varchar2(400);
        v_f_fin             varchar2(400);
        v_f_fin2            varchar2(400);
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
        v_id_zonas_contratista      varchar2(4000);
        v_id_contratista_pesrona    varchar2(4000);
    begin
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, v_tipo_mensaje_seguimiento, 'Entro a realizar consulta del log');
        
        -- Deserializar JSON
        apex_json.free_output;
        apex_json.initialize_clob_output( p_preserve => false );
        v_json_objeto := json_object_t(e_json);

        --ServerSide
        v_ServerSide                    := v_json_objeto.get_Object('ServerSideJson');
        v_filtersString                 := v_ServerSide.get_string('filtersString');
        v_sortString                    := v_ServerSide.get_string('sortString');
        pageNumber                      := v_ServerSide.get_number('first');
        pageSize                        := v_ServerSide.get_number('rows');
        
        v_f_inicio                      := v_json_objeto.get_string('fechaInicial');
        v_f_fin                         := v_json_objeto.get_string('fechaFinal');
        
        v_f_inicio2 := TRUNC(TO_TIMESTAMP(v_f_inicio, 'YYYY-MM-DD"T"HH24:MI:SS"Z"'));
        v_f_fin2    := TRUNC(TO_TIMESTAMP(v_f_fin, 'YYYY-MM-DD"T"HH24:MI:SS"Z"'));

        --Obtengo los campos para filtrar
        v_id_contratista_pesrona        := v_json_objeto.get_clob('id_contratista_persona');
        v_id_zonas_contratista          := v_json_objeto.get_clob('id_zona');
        v_id_persona                    := v_json_objeto.get_string('id_persona');
        --v_id_usuario                    := v_json_objeto.get_string('id_usuario');

        --DBMS_OUTPUT.PUT_LINE('fecha inicio: ' || v_f_inicio2);
        --DBMS_OUTPUT.PUT_LINE('fecha fin: ' || v_f_fin2);

        select nvl(id_contratista,'-1') as id_contratista
        into v_objeto.id_contratista
        from aire.ctn_contratistas_persona
        where id_persona = v_id_persona and codigo_rol = 'ANALISTA';

        --- -//-->INICIO DINAMIC SQL 15/04/2024
        --- -//- Filtros Staticos                      
        myDictionary('fecha').value := case
                                when LENGTH(v_f_inicio) != 0 and nvl(LENGTH(v_f_fin),0) = 0 then ' trunc(x.FECHA_REGISTRO) = ''' || v_f_inicio2 || ''''
                                when LENGTH(v_f_inicio) = 0 and  nvl(LENGTH(v_f_fin),0) = 0 then ' '
                                when LENGTH(v_f_inicio) != 0 and nvl(LENGTH(v_f_fin),0) != 0 then ' trunc(x.FECHA_REGISTRO) between ' || '''' || v_f_inicio2 || '''' || ' and ' || '''' || v_f_fin2 || ''''
                                else ' ' end;

        /*myDictionary('id_contratista_persona').value := case
                                                when v_objeto.id_contratista_persona = -1 then ' and x.id_contratista_persona is null '
                                                when v_objeto.id_contratista_persona = -2 then ' ' --es todo!
                                                when v_objeto.id_contratista_persona > 0 then ' and x.id_contratista_persona = ' || v_objeto.id_contratista_persona || ' '
                                            else ' ' end;*/
        myDictionary('add_null').value := case
                                        when LENGTH(NVL(v_id_contratista_pesrona, '')) > 2 and INSTR(NVL(v_id_contratista_pesrona, ''), '-1') > 0 then ' or x.id_contratista_persona is null '
                                        else '' end;
        myDictionary('id_contratista_persona').value := case
                                                when v_id_contratista_pesrona = '-1' then ' and a.id_contratista_persona is null '
                                                when v_id_contratista_pesrona = '-2' then ' ' --es todo!
                                                when NVL(v_id_contratista_pesrona, '')  NOT IN (' ', '-1','-2') then ' and (x.id_contratista_persona IN (
                                                                                                            SELECT REGEXP_SUBSTR('''|| v_id_contratista_pesrona ||''', ''[^,]+'', 1, LEVEL) AS contratista_persona FROM DUAL
                                                                                                            CONNECT BY REGEXP_SUBSTR('''|| v_id_contratista_pesrona ||''', ''[^,]+'', 1, LEVEL) IS NOT NULL
                                                                                                        ) ' || myDictionary('add_null').value || ' ) '
                                            else ' ' end;

        /*myDictionary('id_zona').value := case
                                                when v_objeto.id_zona = -1 then ' and x.id_zona is null '
                                                when v_objeto.id_zona = -2 then ' ' --es todo!
                                                when v_objeto.id_zona > 0 then ' and x.id_zona = ' || v_objeto.id_zona || ' '
                                            else ' ' end;*/
        myDictionary('id_zona').value := case
                                                when v_id_zonas_contratista = '-1' then ' and x.id_zona is null '
                                                when v_id_zonas_contratista = '-2' then ' ' --es todo!
                                                when NVL(v_id_zonas_contratista, '')  NOT IN (' ', '-1','-2') then ' and x.id_zona IN (
                                                                                                                    SELECT REGEXP_SUBSTR('''|| v_id_zonas_contratista ||''', ''[^,]+'', 1, LEVEL) AS zonas_contratista FROM DUAL
                                                                                                                    CONNECT BY REGEXP_SUBSTR('''|| v_id_zonas_contratista ||''', ''[^,]+'', 1, LEVEL) IS NOT NULL
                                                                                                                )'
                                            else ' ' end;
        

        apex_json.open_object();
        apex_json.write('codigo', 0);
        apex_json.write('mensaje', 'Consulta exitosa');
        apex_json.open_object('datos');
        
        v_dinamic_sql := '
        DECLARE
        BEGIN
            apex_json.open_array(''reporte_traza'');
            for c_datos in (
                    WITH t as (
                        select 
                            a.id_orden_historico,
                            a.fecha_registro_historico,
                            a.estado_historico,
                            a.id_orden, --Este es el unico id que se necesita
                            b.codigo_tipo_orden as tipo, --a.id_tipo_orden,
                            a.numero_orden,
                            c.descripcion as estado, ---c.codigo_estado, a.id_estado_orden,
                            case when nvl(e.nombres,''-1'') = ''-1'' then null else e.nombres || '' '' || e.apellidos || ''_'' || a.id_contratista end as contratista, --a.id_contratista,
                            --f.nic, --a.id_cliente,
                            g.nombre as nombre_territorial, --a.id_territorial,
                            h.nombre as nombre_zona, --a.id_zona,
                            a.direcion,
                            a.fecha_creacion as fecha_generacion,
                            a.fecha_cierre,
                            i.usuario as usuario_cierre, --a.id_usuario_cierre,
                            a.descripcion as descripcion_de_tipo,
                            a.comentarios,
                            a.acta,
                            j.nombre as nombre_actividd, --a.id_actividad,
                            case when nvl(l.nombres,''-1'') = ''-1'' then null else l.nombres || '' '' || l.apellidos end as nombre_contratista_persona, --a.id_contratista_persona,
                            m.descripcion as tipo_trabajo, --a.id_tipo_trabajo,
                            n.descripcion as tipo_suspension, --a.id_tipo_suspencion,
                            a.actividad_orden,
                            a.fecha_estimada_respuesta,
                            a.numero_camp,
                            a.comentario_orden_servicio_num1,
                            a.comentario_orden_servicio_num2,
                            a.observacion_rechazo,
                            a.fecha_rechazo,
                            a.origen,
                            a.fecha_registro,
                            o.nombre as nombre_barrio, --a.id_barrio,
                            a.fecha_asigna_contratista,
                            a.fecha_asigna_tecnico,
                            a.nic,
                            a.deuda,
                            a.tecnico,
                            a.departamento,
                            a.municipio,
                            a.numero_factura,
                            a.tipo_brigada,
                            a.antiguedad
                        from aire.ord_ordenes_historico             a
                        left join aire.ord_tipos_orden              b on a.id_tipo_orden = b.id_tipo_orden
                        left join aire.ord_estados_orden            c on a.id_estado_orden = c.id_estado_orden
                        left join aire.ctn_contratistas             d on a.id_contratista = d.id_contratista
                        left join aire.gnl_personas                 e on d.id_persona = e.id_persona
                        left join aire.gnl_clientes                 f on f.id_cliente = a.id_cliente
                        left join aire.gnl_territoriales            g on g.id_territorial = a.id_territorial
                        left join aire.gnl_zonas                    h on h.id_zona = a.id_zona
                        left join aire.sgd_usuarios                 i on i.id_usuario = a.id_usuario_cierre
                        left join aire.gnl_actividades              j on j.id_actividad = a.id_actividad
                        left join aire.ctn_contratistas_persona     k on a.id_contratista_persona = k.id_contratista_persona
                        left join aire.v_gnl_personas               l on k.id_persona = l.id_persona
                        left join aire.ord_tipos_trabajo            m on a.id_tipo_trabajo = m.id_tipo_trabajo
                        left join aire.scr_tipos_suspencion         n on a.id_tipo_suspencion = n.id_tipo_suspencion
                        left join aire.gnl_barrios                  o on a.id_barrio = o.id_barrio
                        where a.id_orden in (
                            select x.id_orden
                            from aire.ord_ordenes_historico  x
                            where x.id_contratista = ' || v_objeto.id_contratista || ' and
                                ' || myDictionary('fecha').value || '
                                ' || myDictionary('id_contratista_persona').value || '
                                ' || myDictionary('id_zona').value || '
                                )
                    ), t2 as (
                        select count(*) RegistrosTotales from t
                    )
                    select
                            t.*,
                            sysdate as fecha_consulta
                        ,t2.RegistrosTotales
                    from t, t2
                    OFFSET ' || pageNumber || ' ROWS FETCH NEXT ' || pageSize ||' ROWS ONLY
                ) loop
                    apex_json.open_object();
                    apex_json.write(''id_orden_historico'', c_datos.id_orden_historico, true);
                    apex_json.write(''fecha_registro_historico'', c_datos.fecha_registro_historico);
                    apex_json.write(''estado_historico'', c_datos.estado_historico, true);
                    apex_json.write(''id_orden'', c_datos.id_orden, true);
                    apex_json.write(''orden'', c_datos.numero_orden, true);
                    --apex_json.write(''id_cliente'', c_datos.id_cliente, true);
                    apex_json.write(''tipo'', c_datos.tipo, true);
                    apex_json.write(''nombre_contratista_persona'', c_datos.nombre_contratista_persona, true);
                    apex_json.write(''nombre_barrio'', c_datos.nombre_barrio, true);
                    apex_json.write(''estado'', c_datos.estado, true);
                    apex_json.write(''contratista'', c_datos.contratista, true);
                    apex_json.write(''nombre_territorial'', c_datos.nombre_territorial, true);
                    apex_json.write(''tipo_suspension'', c_datos.tipo_suspension, true);
                    apex_json.write(''nombre_zona'', c_datos.nombre_zona, true);
                    apex_json.write(''direcion'', c_datos.direcion, true);
                    apex_json.write(''fecha_generacion'', c_datos.fecha_generacion);
                    apex_json.write(''fecha_cierre'', c_datos.fecha_cierre);
                    apex_json.write(''usuario_cierre'', c_datos.usuario_cierre, true);
                    apex_json.write(''descripcion_de_tipo'', c_datos.descripcion_de_tipo, true);
                    apex_json.write(''comentarios'', c_datos.comentarios, true);
                    apex_json.write(''acta'', c_datos.acta, true);
                    apex_json.write(''nombre_actividd'', c_datos.nombre_actividd, true);
                    apex_json.write(''tipo_trabajo'', c_datos.tipo_trabajo, true);
                    apex_json.write(''actividad_orden'', c_datos.actividad_orden, true);
                    apex_json.write(''fecha_estimada_respuesta'', c_datos.fecha_estimada_respuesta);
                    apex_json.write(''numero_camp'', c_datos.numero_camp, true);
                    apex_json.write(''comentario_orden_servicio_num1'', c_datos.comentario_orden_servicio_num1, true);
                    apex_json.write(''comentario_orden_servicio_num2'', c_datos.comentario_orden_servicio_num2, true);
                    apex_json.write(''observacion_rechazo'', c_datos.observacion_rechazo, true);
                    apex_json.write(''fecha_rechazo'', c_datos.fecha_rechazo);
                    apex_json.write(''origen'', c_datos.origen, true);
                    apex_json.write(''fecha_ingreso_op360'', c_datos.fecha_registro);
                    apex_json.write(''fecha_asigna_contratista'', c_datos.fecha_asigna_contratista);
                    apex_json.write(''fecha_asigna_tecnico'', c_datos.fecha_asigna_tecnico);
                    apex_json.write(''nic'', c_datos.nic, true);
                    apex_json.write(''deuda'', c_datos.deuda, true);
                    apex_json.write(''tecnico'', c_datos.tecnico, true);
                    apex_json.write(''departamento'', c_datos.departamento, true);
                    apex_json.write(''municipio'', c_datos.municipio, true);
                    apex_json.write(''numero_factura'', c_datos.numero_factura, true);
                    apex_json.write(''tipo_brigada'', c_datos.tipo_brigada, true);
                    apex_json.write(''antiguedad'', c_datos.antiguedad, true);
                    apex_json.write(''fecha_consulta'', c_datos.fecha_consulta);
                    
                    BEGIN :v_RegistrosTotales := c_datos.RegistrosTotales; END;
                    apex_json.close_object();
                end loop;
            apex_json.close_array();
        END;
        ';
        DBMS_OUTPUT.PUT_LINE('Query: ' || v_dinamic_sql);

        EXECUTE IMMEDIATE v_dinamic_sql
        USING OUT v_RegistrosTotales;
        
        apex_json.write('RegistrosTotales', v_RegistrosTotales);
        apex_json.close_object();
        apex_json.close_object();
        
        --Si todo sale bien devolver mensaje.
        s_json := apex_json.get_clob_output;
        apex_json.free_output;

        --LOG
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Consulta exitosa');

    exception
        when NO_DATA_FOUND then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, v_tipo_mensaje_error, 'No se encontraron tecnicos asociados');
            apex_json.free_output;
            apex_json.open_object();
            apex_json.write('codigo', 1);
            apex_json.write('mensaje', 'No se encontraron tecnicos asociados');
            apex_json.close_object();
            v_json := apex_json.get_clob_output;
            apex_json.free_output;
            s_json := v_json;

        when others then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, v_tipo_mensaje_error, 'Error al realizar la consulta: '|| sqlerrm);
            apex_json.free_output;
            apex_json.open_object();
            apex_json.write('codigo', 1);
            apex_json.write('mensaje', 'Error N '||v_id_log ||' al realizar la consulta');
            apex_json.close_object();
            v_json := apex_json.get_clob_output;
            apex_json.free_output;
            s_json := v_json;
    END prc_consultar_reportes_traza_contratista;

END PKG_G_CARLOS_VARGAS_TEST11;