CREATE OR REPLACE PACKAGE BODY "AIRE".PKG_G_DANIEL_GONZALEZ_TEST3 AS
    
    procedure prc_registro_ordenes_masivo_final (
		e_json  		in 	clob,
        s_json 	        out	clob
	) is
        v_json_orden                json_object_t;  
		v_id_log		            aire.gnl_logs.id_log%type;
		v_respuesta                 aire.tip_respuesta := aire.tip_respuesta(mensaje => 'Registro exitoso.', nombre_up => 'pkg_g_daniel_gonzalez_test3.prc_registro_ordenes_masivo_final');
        
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
                         x.id_orden_cargue_temporal
                         
                        ,x.nic
                        --//--validar si el nic es numerico
                        ,case when REGEXP_LIKE(x.nic, '^[[:digit:]]+$') then 0 else 1 end       vnic
                        ,(select nvl(sum(0),1) from aire.gnl_clientes where nic = x.nic)        enic
                        
                        ,x.codigo_tipo_orden
                        --//--validar si el tipo orden es texto
                        ,case when REGEXP_LIKE(x.codigo_tipo_orden, '^[[:digit:]]+$') then 1 else 0 end                             vtor
                        ,(select nvl(sum(0),1) from aire.ord_tipos_orden where codigo_tipo_orden = x.codigo_tipo_orden)             etor
                        
                        ,x.codigo_tipo_suspencion
                        --//--validar si el tipo suspensión es texto
                        ,case when REGEXP_LIKE(x.codigo_tipo_suspencion, '^[[:digit:]]+$') then 1 else 0 end                                       vtsu
                        ,(select nvl(sum(0),1) from aire.scr_tipos_suspencion where codigo = x.codigo_tipo_suspencion)        etsu
                        
                        ,x.codigo_estado_servicio
                        --//--validar si el tipo suspensión es texto
                        ,case when REGEXP_LIKE(x.codigo_estado_servicio, '^[[:digit:]]+$') then 1 else 0 end                                       vtes
                        ,(select nvl(sum(0),1) from aire.gnl_estados_servicio where codigo = x.codigo_estado_servicio)        etes
                    FROM aire.ord_ordenes_cargue_temporal x
                    WHERE x.id_soporte = v_id_soporte and x.usuario_registra = v_usuario_registra
                    --WHERE x.id_soporte = 1720 and x.usuario_registra = '362'
                    order by 1
                    ) src
            ON      (trg.id_orden_cargue_temporal = src.id_orden_cargue_temporal)
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
        and     id_actividad IN (select id_actividad from aire.gnl_actividades where nombre = 'SCR' and ind_activo = 'S');
        
        --Crear registros de ordenes masivamente.
            insert into aire.ord_ordenes
            (
            id_tipo_orden
            ,id_cliente
            ,id_estado_orden
            ,numero_orden
            ,id_actividad
            ,id_tipo_suspencion
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
                ,sysdate fecha_fin_cargue
                ,'0' duracion
                ,v_usuario_registra id_usuario_registro
                ,localtimestamp fecha_registro
                ,163 id_estado_intancia --Finalizado
                ,'se cargaron ' || SUM(case when con_errores = 0 then 1 else 0 end) || ' archivos' observaciones
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
	end prc_registro_ordenes_masivo_final;
    
    /*	Procedimiento que retorna el cargue inicial para el modulo de filtros
    
        @autor: antonio molina y carlos vargas
        @Fecha Creación: 26/01/2024
        @Fecha Ultima Actualización: 26/01/2024
    
        Parámetros
        e_json   : corresponde al json de entrada
        s_json   : corresponde al json de salida
    */
    procedure prc_filtros_parametros_Iniciales (
		s_json  		out 	clob
	) is
        v_id_log		            aire.gnl_logs.id_log%type;
		v_respuesta                 aire.tip_respuesta := aire.tip_respuesta(mensaje => 'Registro exitoso.', nombre_up => 'pkg_g_daniel_gonzalez_test3.prc_filtros_parametros_Iniciales');
        v_json          clob;  
        v_cursor        sys_refcursor;
        v_id            number;
        v_descripcion   varchar2(1000);
	begin
        apex_json.free_output; 
        apex_json.initialize_clob_output( p_preserve => false );
        apex_json.open_object;
        apex_json.open_object('datos');
        apex_json.open_array('columnas');
        
        -- consultamos las columnas
        for c_columnas in (
            select id_columna_filtro
                 , descripcion_columna
                 , tipo_dato
                 , tipo_elemento
                 , ind_requerido
                 , operadores
                 , origen
              from aire.ord_columnas_filtro
             where ind_activo = 'S' 
        ) loop
            apex_json.open_object;
            apex_json.write('id_columna', c_columnas.id_columna_filtro);
            apex_json.write('descripcion',c_columnas.descripcion_columna);
            apex_json.write('tipo_dato',c_columnas.tipo_dato);
            apex_json.write('tipo_elemento',c_columnas.tipo_elemento);
            apex_json.write('ind_requerido',c_columnas.ind_requerido);
            
            -- recorremos los operadores
            apex_json.open_array('operadores');
            for c_operadores in (
                select id_dominio_valor
                     , descripcion
                     , valor
                  from aire.gnl_dominios_valor
                 where id_dominio_valor in ( 
                            select regexp_substr(c_columnas.operadores, '[^:]+', 1, level)
                              from dual
                            connect by regexp_substr(c_columnas.operadores, '[^:]+', 1, level) is not null
                       )        
            ) loop
                apex_json.open_object;
                apex_json.write('id_operador',c_operadores.id_dominio_valor);
                apex_json.write('descripcion',c_operadores.descripcion);
                apex_json.write('operador',c_operadores.valor);
                apex_json.close_object;
            end loop;
            apex_json.close_array;        
            
            
            if c_columnas.origen is not null then
                begin
                    execute immediate 'begin :cursor := ' || c_columnas.origen || '; end;' using out v_cursor;
                exception
                    when others then
                        dbms_output.put_line('Error al consultar el origen de la columna: '|| c_columnas.descripcion_columna);
                        return;
                end;
                
                -- Recorrer el cursor y agregar cada fila al array JSON
                apex_json.open_array('lista');
                loop
                    fetch v_cursor into v_id, v_descripcion;
                    exit when v_cursor%notfound;
            
                    -- Agregar cada fila como un objeto JSON al array
                    apex_json.open_object;
                    apex_json.write('id', v_id);
                    apex_json.write('descripcion', v_descripcion);
                    apex_json.close_object;
                end loop;            
                apex_json.close_array;
    
            end if;
            apex_json.close_object;
        end loop;
        apex_json.close_array;
        apex_json.close_object;
    
        apex_json.write('codigo', 0);
        apex_json.write('mensaje', 'Carga exitosa');
        apex_json.close_object;
        
        v_json := apex_json.get_clob_output;
        apex_json.free_output;
        s_json := v_json;
        dbms_output.put_line(v_json);
        
	exception
        when others then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al consultar los parametros iniciales de filtros: '|| sqlerrm);
            apex_json.initialize_clob_output( p_preserve => false );
            apex_json.open_object();
            apex_json.write('codigo', 400);
            apex_json.write('mensaje', 'Error #'||v_id_log||' al consultar los filtros' || sqlerrm);
            apex_json.close_object();
            s_json := apex_json.get_clob_output;
            apex_json.free_output;
	end prc_filtros_parametros_Iniciales;

END PKG_G_DANIEL_GONZALEZ_TEST3;