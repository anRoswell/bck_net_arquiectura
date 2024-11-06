CREATE OR REPLACE package "AIRE".pkg_g_carlos_vargas_test7 as 

    -- este sp sobreescribe pgk_g_ordenes
    procedure prc_desasignar_ordenes_masivo_tecnico(
        e_json  in 	clob,
        s_json  out	clob
    );

    -- 27/03/2024
    -- este sp sobreescribe pgk_g_ordenes y pkg_g_carlos_vargas_test8
    procedure prc_gestionar_orden_asignar_brigada (		
        e_json  in  clob,
        s_json 	out	clob
	);

    -- 27/03/2024
    -- este sp sobreescribe pgk_g_ordenes y pkg_g_carlos_vargas_test8
    procedure prc_asignar_ordenes_masivo_tecnico(
        e_json  in 	clob,
        s_json  out	clob
    );

end pkg_g_carlos_vargas_test7;
/
CREATE OR REPLACE package body "AIRE".pkg_g_carlos_vargas_test7 as

    --/--se migro a pkg_g_ordenes el 22/03/2024 11:16am
    procedure prc_desasignar_ordenes_masivo_tecnico(
        e_json  in 	clob,
        s_json  out	clob
    ) is
        v_respuesta                 				aire.tip_respuesta := aire.tip_respuesta(mensaje => 'Registro exitoso.', nombre_up => 'pkg_g_ordenes.prc_desasignar_ordenes_masivo_tecnico');
        v_id_soporte                				aire.gnl_soportes.id_soporte%type;
        v_id_usuario_registra       				aire.sgd_usuarios.id_usuario%type;
        v_nombre_archivo            				varchar2(250);
        v_id_archivo                				number;
        v_id_archivo_instancia      				number;
        v_fecha_inicio              				timestamp;
        v_id_orden          						aire.ord_ordenes.id_orden%type;
        v_id_estado_orden_asignada_tecnico          aire.ord_ordenes.id_estado_orden%type;
        v_id_estado_orden_asignada_contratista      aire.ord_ordenes.id_estado_orden%type;
		v_id_estado_orden							aire.ord_ordenes.id_estado_orden%type;
        v_id_contratista_persona					aire.ctn_contratistas_persona.id_contratista_persona%type;
		v_id_contratista_tecnico                    aire.ctn_contratistas.id_contratista%type;
        v_id_contratista_analista                   aire.ctn_contratistas.id_contratista%type;
		v_nic										aire.gnl_clientes.nic%type;
		v_ind_activo								varchar2(1);
        v_json_array_notificacion                   json_array_t  := json_array_t();
        v_notificacion                              json_object_t := json_object_t();
        v_id_dispositivo                            aire.sgd_usuarios.id_dispositivo%type;
        v_contador_exito                            number := 0;
        v_contador_error                            number := 0;
        v_ind_area_central                          varchar2(1);
        type 										t_temp is table of aire.ord_ordenes_temporal_desasignacion_tecnico%rowtype;
        v_temp 										t_temp;
        v_id_log		                            aire.gnl_logs.id_log%type;
        
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
        is begin
            if codigo = '0' then
                v_contador_exito := v_contador_exito + 1;
            else
                v_contador_error := v_contador_error + 1;
            end if;

            update aire.ord_ordenes_temporal_desasignacion_tecnico
              set con_errores       = codigo 
                , desc_validacion   = mensaje
             where id_soporte       = v_id_soporte 
               and usuario_registra = v_id_usuario_registra
               and numero_orden     = numero_orden_temp;
        end;
    begin
        -- validamos el json de entrada
        if e_json is not json then
            sb_escribir_respuesta(1, 'JSON invalido');
            return;
        end if;
        
        v_id_log := aire.pkg_p_generales.fnc_registrar_log ('prc_desasignar_ordenes_masivo_tecnico', 'ERROR', 'Entrando  e_json = '||e_json);
        
        -- sacamos la informacion del JSON
        v_id_soporte            := json_value(e_json, '$.id_soporte');
        v_id_usuario_registra   := json_value(e_json, '$.usuario_registra');
        v_nombre_archivo        := json_value(e_json, '$.nombre_archivo');
        v_ind_area_central      := json_value(e_json, '$.ind_areacentral');

        if v_ind_area_central is null then
            sb_escribir_respuesta(1, 'Se debe derinir el rol de la persona que realiza el cargue');
            return;
        end if;
        -- consultamos el estado cerrada
        begin
            select id_estado_orden 
              into v_id_estado_orden_asignada_tecnico
              from aire.ord_estados_orden 
             where codigo_estado = 'SEAS';
        exception
            when others then
                sb_escribir_respuesta(1, 'Error al consultar el estado de la orden: Asignada tecnico - ' || sqlerrm);
                return;
        end;

        -- consultamos el estado legalizacion fallida
        begin
            select id_estado_orden
              into v_id_estado_orden_asignada_contratista
              from aire.ord_estados_orden
             where codigo_estado = 'SASI';
        exception
            when others then
                sb_escribir_respuesta(1, 'Error al consultar el estado de la orden: Asignada contratista - ' || sqlerrm);
                return;
        end;

        -- consultamos el archivo o el cargue
        begin
            select id_archivo
              into v_id_archivo
              from aire.gnl_archivos 
             where codigo = 'MDSG';
        exception 
            when others then
                sb_escribir_respuesta(1, 'Error al consultar el cargue: MDSG');
                return;
        end;

        -- consultamos cuando inicio el cargue
        select min(fecha_registra)
          into v_fecha_inicio
          from aire.ord_ordenes_temporal_desasignacion_tecnico
         where id_soporte       =  v_id_soporte
           and usuario_registra = v_id_usuario_registra;

        -- consultamos el contratista del analista que hace el cargue
        if v_ind_area_central = 'N' then
            begin
                select id_contratista
                  into v_id_contratista_analista
                  from aire.ctn_contratistas_persona a
                 where a.id_persona = (select id_persona from aire.sgd_usuarios where id_usuario = v_id_usuario_registra)
                   and a.codigo_rol = 'ANALISTA';
            exception
                when no_data_found then
                    sb_escribir_respuesta(1, 'No se pudo consultar el contratista asociado al analista');
                    return;
                when others then
                    sb_escribir_respuesta(1, 'Error al  consultar el contratista asociado al analista: '|| sqlerrm);
                    return;
            end;
        end if;

        select *
          bulk collect into v_temp
          from aire.ord_ordenes_temporal_desasignacion_tecnico
         where id_soporte       = v_id_soporte 
           and usuario_registra = v_id_usuario_registra; 

        if sql%rowcount = 0 then
            sb_escribir_respuesta(1, 'No se encontraron registros para procesar');
            return;
        end if;

        -- recorremos los registros
        for i in 1..v_temp.count loop
            
            if trim(v_temp(i).numero_orden) is null then
                -- actualizar registro con errores
                sb_actualizar_temp(v_temp(i).numero_orden, '1', 'El numero de la orden es requerido');
                continue;
            elsif trim(v_temp(i).nic) is null then
                -- actualizar registro con errores
                sb_actualizar_temp(v_temp(i).numero_orden, '1', 'El nic es requerido');
                continue;
            end if;
            
            
            -- consultamos la orden
            begin
                select a.id_orden
				     , a.id_estado_orden
					 , b.nic
					 , a.id_contratista_persona
                     , a.id_contratista
                  into v_id_orden
				     , v_id_estado_orden
					 , v_nic
					 , v_id_contratista_persona
                     , v_id_contratista_tecnico
                  from aire.ord_ordenes  a
				  join aire.gnl_clientes b on a.id_cliente = b.id_cliente
                 where a.numero_orden    = to_number(v_temp(i).numero_orden);
dbms_output.put_line('v_id_estado_orden: '||v_id_estado_orden||' v_id_estado_orden_asignada_tecnico: '|| v_id_estado_orden_asignada_tecnico);
				-- validamos el estado de la orden
				if v_id_estado_orden <> v_id_estado_orden_asignada_tecnico then
                    -- actualizar registro con errores
                    sb_actualizar_temp(v_temp(i).numero_orden, '1', 'La orden '||v_temp(i).numero_orden||' no tiene un estado valido');
                    continue;				
				end if;

				-- validamos el nic
				if v_nic <> trim(v_temp(i).nic) then
                    -- actualizar registro con errores
                    sb_actualizar_temp(v_temp(i).numero_orden, '1', 'La orden '||v_temp(i).numero_orden||' no se encuentra asociada al nic '||v_temp(i).nic);
                    continue;				
				end if;

                -- si es contratista validamos que quien hace el cargue tenga asociado el mismo tecnico que el contratista
                if v_id_contratista_analista <> v_id_contratista_tecnico and v_ind_area_central = 'N' then
                    -- actualizar registro con errores
                    sb_actualizar_temp(v_temp(i).numero_orden, '1', 'El tecnico que tiene asociado la orden  '||v_temp(i).numero_orden||' no se encuentra asociado al contratista que tiene asignado el analista');
                    continue;
                end if;

            exception
                when no_data_found then
                    -- actualizar registro con errores
                    sb_actualizar_temp(v_temp(i).numero_orden, '1', 'La orden '||v_temp(i).numero_orden||' no existe');
                    continue;
                when others then
                    -- actualizar registro con errores
                    sb_actualizar_temp(v_temp(i).numero_orden, '1', 'Error al consultar la orden '||v_temp(i).numero_orden||': '|| sqlerrm);
                    continue;
			end;
          
			-- desasignamos la orden al tecnico y marcamos la orden como asignada contratista
			begin
				update aire.ord_ordenes
				   set id_estado_orden        = v_id_estado_orden_asignada_contratista
				   	 , id_contratista_persona = null
				 where id_orden = v_id_orden;
			exception
				when others then
                    -- actualizar registro con errores
                    sb_actualizar_temp(v_temp(i).numero_orden, '1', 'Error al desasignar la orden '||v_temp(i).numero_orden||' a tecnico '|| sqlerrm);
                    continue;					
			end;
			
			begin
				-- notificamos al usuario
				select id_dispositivo
			      into v_id_dispositivo
				  from aire.sgd_usuarios
				 where id_persona = (
										select id_persona
				  						  from aire.ctn_contratistas_persona
										 where id_contratista_persona = v_id_contratista_persona
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

            sb_actualizar_temp(v_temp(i).numero_orden, '0', 'Ok');
        end loop;
        
        -- se mandan las notificaciones en segundo plano
        if v_json_array_notificacion.get_size > 0 then
            aire.pkg_g_generales.prc_enviar_notificaciones_push_segundo_plano(
                e_notificaciones => v_json_array_notificacion.to_clob, 
                s_respuesta	     => v_respuesta
            );
        end if;

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
            from aire.ord_ordenes_temporal_desasignacion_tecnico  
           where id_soporte       = v_id_soporte 
             and usuario_registra = v_id_usuario_registra;
        
        -- eliminamos la tabla temporal
		delete aire.ord_ordenes_temporal_desasignacion_tecnico
         where id_soporte       = v_id_soporte 
           and usuario_registra = v_id_usuario_registra;
        
        sb_escribir_respuesta(0, 'Archivo procesado');
    exception
        when others then
            sb_escribir_respuesta(1, 'Error al procesar ordenes:'|| sqlerrm);
    end prc_desasignar_ordenes_masivo_tecnico;

    --/--OJO se migra el 31/03/2024
    procedure prc_gestionar_orden_asignar_brigada (
        e_json  in  clob,
        s_json 	out	clob
    ) is
        --e_json  	clob :=  '{"id_contratista":110,"id_ordenes":"1,150,220","contratista_asignar":"S"}';
        v_json_objeto           json_object_t;
        v_id_log		        aire.gnl_logs.id_log%type;
        v_respuesta		        aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_ordenes.prc_gestionar_orden_asignar_brigada');
        
        v_identificacion_contratista        varchar2(50 BYTE);
        v_id_contratista                    varchar2(100 BYTE);
        v_identificacion_brigada            varchar2(50 BYTE);
        v_id_brigada                        varchar2(100 BYTE);
        
        v_id_orden_string                   varchar2(4000 byte);
        asignar_brigada                     varchar2(1 BYTE);
        
        v_cnt                               NUMBER;
        
        -- Declarar la tabla temporal
        l_num_list              num_list_type := num_list_type();
        v_cnt_actualizar        number;
        v_cnt_actualizar2       number;
        v_cnt_actualizar3       number;
        v_cnt_actualizar4       number;
        v_id_estado_orden       number;
        
        v_notificacion                   json_object_t;
        v_respuesta_notificacion         aire.tip_respuesta;
        v_id_dispositivo                 aire.sgd_usuarios.id_dispositivo%type;
    BEGIN
    --LOG
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
    
    -- Deserializar JSON
    apex_json.free_output;
    apex_json.initialize_clob_output( p_preserve => false );
    
    v_json_objeto                       := json_object_t(e_json);
    v_identificacion_contratista        := v_json_objeto.get_string('identificacion_contratista');
    v_identificacion_brigada            := v_json_objeto.get_string('identificacion_brigada');
    
    v_id_orden_string                   := v_json_objeto.get_string('id_orden_string');
    
    asignar_brigada                     := v_json_objeto.get_string('brigada_asignar');
    
    apex_json.free_output;
/*
    SELECT
         COUNT(*) INTO v_cnt
    FROM aire.v_ctn_contratistas_persona cp
    where   cp.ind_activo = 'S'
        and cp.identificacion_contratista = v_identificacion_contratista
        and cp.identificacion_contratista_persona = v_identificacion_brigada;
    
    --Si no se encuentra registros, devolver el error
    IF (v_cnt = 0) THEN
        apex_json.open_object();
            apex_json.write('codigo', 50011);
            apex_json.write('mensaje', 'No se encontro el contratista o la brigada');
        apex_json.close_object();
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
        return;
    END IF;
*/
    begin
        select id_contratista
             , id_contratista_persona
          into v_id_contratista
            ,  v_id_brigada
          from aire.v_ctn_contratistas_persona
         where identificacion_contratista_persona = v_identificacion_brigada
           and ind_activo = 'S';
    exception
        when no_data_found then
            apex_json.open_object();
            apex_json.write('codigo', 50011);
            apex_json.write('mensaje', 'No se encontro el contratista o la brigada');
            apex_json.close_object();
            s_json := apex_json.get_clob_output;
            apex_json.free_output;
            return;
        when others then
            apex_json.open_object();
            apex_json.write('codigo', 50011);
            apex_json.write('mensaje', 'Error al consultar el contratista o la brigada: '|| sqlerrm);
            apex_json.close_object();
            s_json := apex_json.get_clob_output;
            apex_json.free_output;
            return;
    end;
/*
    SELECT
         nvl(cp.id_contratista,'-1')
        ,nvl(cp.id_contratista_persona,'-1')
        into
             v_id_contratista
            ,v_id_brigada
    FROM aire.v_ctn_contratistas_persona cp
    where   cp.ind_activo = 'S'
        and cp.identificacion_contratista = v_identificacion_contratista
        and cp.identificacion_contratista_persona = v_identificacion_brigada;
    
    --Si no se encuentra registros, devolver el error
    IF (v_id_contratista = '-1' or v_id_brigada = '-1') THEN
        apex_json.open_object();
            apex_json.write('codigo', 50012);
            apex_json.write('mensaje', 'No se encontro el contratista o la brigada');
        apex_json.close_object();
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
        return;
    END IF;
*/
    --Validar, si no llega ningun registro retornar provocar error.
    SELECT
       TO_NUMBER(REGEXP_SUBSTR(v_id_orden_string, '[^,]+', 1, level))
       BULK COLLECT INTO l_num_list
    FROM dual
    CONNECT BY REGEXP_SUBSTR(v_id_orden_string, '[^,]+', 1, level) IS NOT NULL;
    
    --/--INICIO contar si se encuentran ordenes con estado diferente a 23-SEAS-Asignada Tecnico
    select count(*) into v_cnt_actualizar2
    from aire.ord_ordenes
    where       id_orden in (select COLUMN_VALUE from TABLE(l_num_list))
            and id_contratista = v_id_contratista
            and id_contratista_persona is null
            and id_estado_orden not in (
                        select id_estado_orden
                        from aire.ord_estados_orden
                        where   codigo_estado in ('SEAS')
                            and id_actividad IN (select id_actividad from aire.gnl_actividades where prefijo = 'G_SCR' and ind_activo = 'S')
                        );
                        
    IF (v_cnt_actualizar2 = 0) THEN
        apex_json.open_object();
            apex_json.write('codigo', 50013);
            apex_json.write('mensaje', 'No se encontraron ordenes con estado diferente a 23-SEAS-Asignada Tecnico');
        apex_json.close_object();
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
        return;
    END IF;
    --/--FIN contar si se encuentran ordenes con estado diferente a 23-SEAS-Asignada Tecnico

    --/--27/03/2024
    --/--INICIO validar que las zonas de las ordenes esten en las zonas del tecnico y del contratista.
        ---Validar que la zona de la orden este contenida en la zona del contratista
        select count(*) 
            into v_cnt_actualizar3
        from aire.ord_ordenes x
        where       x.id_orden in (select COLUMN_VALUE from TABLE(l_num_list))
                and x.id_contratista = v_id_contratista                
                and x.id_estado_orden not in (
                            select id_estado_orden
                            from aire.ord_estados_orden
                            where   codigo_estado in ('SEAS')
                                and id_actividad IN (select id_actividad from aire.gnl_actividades where prefijo = 'G_SCR' and ind_activo = 'S')
                            )
                and x.id_zona not in (
                    select id_zona
                    from aire.ctn_contratos_zona
                    where id_contrato in (
                                select id_contrato
                                from aire.v_ctn_contratos
                                where prefijo_actividad = 'G_SCR'
                                and id_contratista = v_id_contratista
                                )
                );
                            
        IF (v_cnt_actualizar3 != 0) THEN
            apex_json.open_object();
                apex_json.write('codigo', 50016);
                apex_json.write('mensaje', 'Se encontraron ordenes con zonas que no pertenecen a las zonas del contrato para el contratista: ' || v_id_contratista);
            apex_json.close_object();
            s_json := apex_json.get_clob_output;
            apex_json.free_output;
            return;
        END IF;

        ---Validar que la zona de la orden este contenida en la zona de la brigada.
        select count(*) 
            into v_cnt_actualizar4
        from aire.ord_ordenes x
        where       x.id_orden in (select COLUMN_VALUE from TABLE(l_num_list))
                and x.id_contratista = v_id_contratista                
                and x.id_estado_orden not in (
                            select id_estado_orden
                            from aire.ord_estados_orden
                            where   codigo_estado in ('SEAS')
                                and id_actividad IN (select id_actividad from aire.gnl_actividades where prefijo = 'G_SCR' and ind_activo = 'S')
                            )
                and x.id_zona not in (
                    select
                        z.id_zona
                    from aire.sgd_usuarios_zona z
                    inner join aire.sgd_usuarios usr on usr.id_usuario = z.id_usuario
                    where usr.usuario = v_identificacion_brigada and z.ind_activo = 'S'
                );
                            
        IF (v_cnt_actualizar4 != 0) THEN
            apex_json.open_object();
                apex_json.write('codigo', 50017);
                apex_json.write('mensaje', 'Se encontraron ordenes con zonas que no pertenecen a las zonas del tecnico: ' || v_id_brigada);
            apex_json.close_object();
            s_json := apex_json.get_clob_output;
            apex_json.free_output;
            return;
        END IF;
    --/--FIN validar que las zonas de las ordenes esten en las zonas del tecnico y del contratista.
    
    --/--INICIO Solo permitir asignar tecnico si tiene el estado orden 1-SASI-Asignada Contratista
    select count(*) into v_cnt_actualizar
    from aire.ord_ordenes
    where       id_orden in (select COLUMN_VALUE from TABLE(l_num_list))
            and id_contratista = v_id_contratista
            and id_contratista_persona is null
            and id_estado_orden in (
                        select id_estado_orden
                        from aire.ord_estados_orden
                        where   codigo_estado in ('SASI')
                            and id_actividad IN (select id_actividad from aire.gnl_actividades where prefijo = 'G_SCR' and ind_activo = 'S')
                        );
    
    
    IF (v_cnt_actualizar = 0) THEN
        apex_json.open_object();
            apex_json.write('codigo', 50013);
            apex_json.write('mensaje', 'No se encontraron ordenes para actualizar, las ordenes deben estar en estado 1-SASI-Asignada Contratista');
        apex_json.close_object();
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
        return;
    END IF;
                        
    IF (v_cnt_actualizar > 0) THEN
        
        select id_estado_orden into v_id_estado_orden
        from aire.ord_estados_orden
        where   codigo_estado in ('SEAS')
            and id_actividad IN (select id_actividad from aire.gnl_actividades where prefijo = 'G_SCR' and ind_activo = 'S');
    
        update aire.ord_ordenes
        set   id_contratista_persona = v_id_brigada
            , id_estado_orden = v_id_estado_orden
            , observacion_rechazo = NULL
            , fecha_rechazo = NULL
        where   id_orden in (select COLUMN_VALUE from TABLE(l_num_list))
            and id_contratista = v_id_contratista
            and id_contratista_persona is null
            and id_estado_orden in (
                select id_estado_orden
                from aire.ord_estados_orden
                where   codigo_estado in ('SASI')
                    and id_actividad IN (select id_actividad from aire.gnl_actividades where prefijo = 'G_SCR' and ind_activo = 'S')
                );
        
        ---INICIO enviar notificacion a celular
            -- consultamos el id del dispositivo
            select id_dispositivo
              into v_id_dispositivo
              from aire.sgd_usuarios
             where id_persona = (select id_persona from aire.ctn_contratistas_persona where id_contratista_persona = v_id_brigada);
            
            v_notificacion := json_object_t();
            v_notificacion.put('id_dispositivo', v_id_dispositivo);
            v_notificacion.put('ind_android', true);
            v_notificacion.put('titulo', 'Asignación ordenes SCR');
            v_notificacion.put('cuerpo', 'Se asignaron '|| v_cnt_actualizar ||' ordenes a su gestión'); -- cuando es desasignación que diga Se desasignaron x ordenes a su gestión.
            v_notificacion.put('estado', 'asignada');
            v_notificacion.put('tipo', 'gestion_ordenes_scr');
            v_notificacion.put('fecha_envio', to_char(systimestamp, 'DD/MM/YYYY HH:MI:SS:FF PM'));
            -- Se envia notificacion en segundo plano
            aire.pkg_g_generales.prc_enviar_notificacion_push_segundo_plano( e_notificacion  => v_notificacion.to_clob, s_respuesta => v_respuesta_notificacion);
        ---FIN enviar notificacion a celular
        
        
        --Si todo sale bien devolver mensaje.
        apex_json.open_object();
            apex_json.write('codigo', 200);
            apex_json.write('mensaje', 'Proceso realizado con éxito. Se actualizaron: ' || v_cnt_actualizar || ' ordenes.');
        apex_json.close_object();
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
    ELSE
        --Si todo sale bien devolver mensaje.
        apex_json.open_object();
            apex_json.write('codigo', 400);
            apex_json.write('mensaje', 'no se actualizo ninguna orden');
        apex_json.close_object();
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
        return;
    END IF;
    --/--FIN Si no se encuentra registros, devolver el error
    
    --LOG
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Carga exitosa');
    --dbms_output.put_line(s_json);
    exception
    when others then
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al crear el json: '|| sqlerrm);
        apex_json.free_output;
        apex_json.open_object();
        apex_json.write('codigo', 400);
        apex_json.write('mensaje', 'Error N '||v_id_log ||' al consultar el listado de ordenes.' || sqlerrm);
        apex_json.close_object();
        --v_json := apex_json.get_clob_output;
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
        --s_json := v_json;
        --dbms_output.put_line('xxx: '|| sqlerrm);
    END prc_gestionar_orden_asignar_brigada;

    --/-- Se migro desde pkg_g_carlos_vargas_test7 el 31/03/2024 11:11 am
    procedure prc_asignar_ordenes_masivo_tecnico(
        e_json  in 	clob,
        s_json  out	clob
    ) is
        v_respuesta                 				aire.tip_respuesta := aire.tip_respuesta(mensaje => 'Registro exitoso.', nombre_up => 'pkg_g_ordenes.prc_asignar_ordenes_masivo_tecnico');
        v_id_soporte                				aire.gnl_soportes.id_soporte%type;
        v_id_usuario_registra       				aire.sgd_usuarios.id_usuario%type;
        v_nombre_archivo            				varchar2(250);
        v_id_archivo                				number;
        v_id_archivo_instancia      				number;
        v_fecha_inicio              				timestamp;
        v_id_orden          						aire.ord_ordenes.id_orden%type;
        v_id_zona          						    aire.ord_ordenes.id_zona%type;
        v_id_estado_orden_asignada_tecnico          aire.ord_ordenes.id_estado_orden%type;
        v_id_estado_orden_asignada_contratista      aire.ord_ordenes.id_estado_orden%type;
		v_id_estado_orden							aire.ord_ordenes.id_estado_orden%type;
        v_id_contratista_persona					aire.ctn_contratistas_persona.id_contratista_persona%type;
		v_id_contratista                            aire.ctn_contratistas.id_contratista%type;
		v_id_contratista_analista                   aire.ctn_contratistas.id_contratista%type;
		v_id_contratista_tecnico                    aire.ctn_contratistas.id_contratista%type;
		v_nic										aire.gnl_clientes.nic%type;
		v_ind_activo								varchar2(1);
        v_json_array_notificacion                   json_array_t  := json_array_t();
        v_notificacion                              json_object_t := json_object_t();
        v_id_dispositivo                            aire.sgd_usuarios.id_dispositivo%type;
        v_id_tipo_suspencion                        aire.ord_ordenes.id_tipo_suspencion%type;
        v_contador_exito                            number := 0;
        v_contador_error                            number := 0;
        v_contador_zona1                            number := 0;
        v_contador_zona2                            number := 0;
        v_ind_area_central                          varchar2(1);
        type 										t_temp is table of aire.ord_ordenes_temporal_asignacion_tecnico%rowtype;
        v_temp 										t_temp;

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
        is begin
            if codigo = '0' then
                v_contador_exito := v_contador_exito + 1;
            else
                v_contador_error := v_contador_error + 1;
            end if;

            update aire.ord_ordenes_temporal_asignacion_tecnico
              set con_errores       = codigo
                , desc_validacion   = mensaje
             where id_soporte       = v_id_soporte
               and usuario_registra = v_id_usuario_registra
               and numero_orden     = numero_orden_temp;
        end;
    begin
        -- validamos el json de entrada
        if e_json is not json then
            sb_escribir_respuesta(1, 'JSON invalido');
            return;
        end if;

        -- sacamos la informacion del JSON
        v_id_soporte            := json_value(e_json, '$.id_soporte');
        v_id_usuario_registra   := json_value(e_json, '$.usuario_registra');
        v_nombre_archivo        := json_value(e_json, '$.nombre_archivo');
        v_ind_area_central      := json_value(e_json, '$.ind_areacentral');

        if v_ind_area_central is null then
            sb_escribir_respuesta(1, 'Se debe definir el rol de la persona que realiza el cargue');
            return;
        end if;

        -- consultamos el estado cerrada
        begin
            select id_estado_orden
              into v_id_estado_orden_asignada_tecnico
              from aire.ord_estados_orden
             where codigo_estado = 'SEAS';
        exception
            when others then
                sb_escribir_respuesta(1, 'Error al consultar el estado de la orden: Asignada tecnico - ' || sqlerrm);
                return;
        end;
        
        -- consultamos el estado legalizacion fallida
        begin
            select id_estado_orden
              into v_id_estado_orden_asignada_contratista
              from aire.ord_estados_orden
             where codigo_estado = 'SASI';
        exception
            when others then
                sb_escribir_respuesta(1, 'Error al consultar el estado de la orden: Asignada contratista - ' || sqlerrm);
                return;
        end;

        -- consultamos el archivo o el cargue
        begin
            select id_archivo
              into v_id_archivo
              from aire.gnl_archivos
             where codigo = 'MASG';
        exception
            when others then
                sb_escribir_respuesta(1, 'Error al consultar el cargue: MASG');
                return;
        end;

        -- consultamos cuando inicio el cargue
        select min(fecha_registra)
          into v_fecha_inicio
          from aire.ord_ordenes_temporal_asignacion_tecnico
         where id_soporte       =  v_id_soporte
           and usuario_registra = v_id_usuario_registra;

        -- consultamos el contratista del analista que hace el cargue
        if v_ind_area_central = 'N' then
            begin
                select id_contratista
                  into v_id_contratista_analista
                  from aire.ctn_contratistas_persona a
                 where a.id_persona = (select id_persona from aire.sgd_usuarios where id_usuario = v_id_usuario_registra)
                   and a.codigo_rol = 'ANALISTA';
            exception
                when no_data_found then
                    sb_escribir_respuesta(1, 'No se pudo consultar el contratista asociado al analista');
                    return;
                when others then
                    sb_escribir_respuesta(1, 'Error al  consultar el contratista asociado al analista: '|| sqlerrm);
                    return;
            end;
        end if;

        select *
          bulk collect into v_temp
          from aire.ord_ordenes_temporal_asignacion_tecnico
         where id_soporte       = v_id_soporte
           and usuario_registra = v_id_usuario_registra;

        if sql%rowcount = 0 then
            sb_escribir_respuesta(1, 'No se encontraron registros para procesar');
            return;
        end if;
        -- recorremos los registros
        for i in 1..v_temp.count loop
            
            if trim(v_temp(i).numero_orden) is null then
                -- actualizar registro con errores
                sb_actualizar_temp(v_temp(i).numero_orden, '1', 'El numero de la orden es requerido');
                continue;
            elsif trim(v_temp(i).usuario) is null then
                -- actualizar registro con errores
                sb_actualizar_temp(v_temp(i).numero_orden, '1', 'El usuario es requerido');
                continue;
            elsif trim(v_temp(i).nic) is null then
                -- actualizar registro con errores
                sb_actualizar_temp(v_temp(i).numero_orden, '1', 'El nic es requerido');
                continue;
            end if;

            -- consultamos la orden
            begin
                select a.id_orden
				     , a.id_estado_orden
					 , b.nic
                     , a.id_contratista
                     , a.id_tipo_suspencion
                     , a.id_zona
                  into v_id_orden
				     , v_id_estado_orden
					 , v_nic
                     , v_id_contratista
                     , v_id_tipo_suspencion
                     , v_id_zona
                  from aire.ord_ordenes  a
				  join aire.gnl_clientes b on a.id_cliente = b.id_cliente
                 where a.numero_orden    = to_number(v_temp(i).numero_orden);

				-- validamos la zona de la orden contra la zona del contratista.
                select count(*)
                        into v_contador_zona1
                from aire.ctn_contratos_zona x
                where x.id_contrato in (
                            select id_contrato
                            from aire.v_ctn_contratos
                            where prefijo_actividad = 'G_SCR'
                            and id_contratista = v_id_contratista
                            )
                        and x.id_zona = v_id_zona;
                
                if v_contador_zona1 = 0 then
                    -- actualizar registro con errores
                    sb_actualizar_temp(v_temp(i).numero_orden, '1', 'La orden '||v_temp(i).numero_orden||' tiene una zona diferente a las del contratista.');
                    continue;
				end if;
                v_contador_zona1 := 0;

                -- validamos el estado de la orden
				if v_id_estado_orden <> v_id_estado_orden_asignada_contratista then
                    -- actualizar registro con errores
                    sb_actualizar_temp(v_temp(i).numero_orden, '1', 'La orden '||v_temp(i).numero_orden||' no tiene un estado valido');
                    continue;
				end if;

				-- validamos el nic
				if v_nic <> trim(v_temp(i).nic) then
                    -- actualizar registro con errores
                    sb_actualizar_temp(v_temp(i).numero_orden, '1', 'La orden '||v_temp(i).numero_orden||' no se encuentra asociada al nic '||v_temp(i).nic);
                    continue;
				end if;
                
            exception
                when no_data_found then
                    -- actualizar registro con errores
                    sb_actualizar_temp(v_temp(i).numero_orden, '1', 'La orden '||v_temp(i).numero_orden||' no existe');
                    continue;
                when others then
                    -- actualizar registro con errores
                    sb_actualizar_temp(v_temp(i).numero_orden, '1', 'Error al consultar la orden '||v_temp(i).numero_orden||': '|| sqlerrm);
                    continue;
			end;
          
		  	-- consultamos el tecnico
			begin
				select id_contratista_persona
				     , ind_activo
                     , id_contratista
				  into v_id_contratista_persona
				     , v_ind_activo
                     , v_id_contratista_tecnico
				  from aire.v_ctn_contratistas_persona
				 where identificacion_contratista_persona = trim(v_temp(i).usuario)
                   and ind_activo = 'S';
				
                -- validamos que el contratista deltecnico sea el contratista de la orden
				if v_id_contratista <> v_id_contratista_tecnico then
					-- actualizar registro con errores
					sb_actualizar_temp(v_temp(i).numero_orden, '1', 'El tecnico '||v_temp(i).usuario||' no se encuentra asociado al contratista que tiene asignada la orden');
					continue;
				end if;

                -- validamos que el contratista del analista sea el contratista del tecnico
                if v_id_contratista_analista <> v_id_contratista_tecnico and v_ind_area_central = 'N' then
					-- actualizar registro con errores
					sb_actualizar_temp(v_temp(i).numero_orden, '1', 'El tecnico '||v_temp(i).usuario||' no se encuentra asociado al contratista que tiene asignado el analista');
					continue;
				end if;
                
                -- validamos que el contratista de la orden sea el contratista del analista
                if v_id_contratista_analista <> v_id_contratista and v_ind_area_central = 'N' then
					-- actualizar registro con errores
					sb_actualizar_temp(v_temp(i).numero_orden, '1', 'La orden '||v_temp(i).numero_orden||' no se encuentra asociada al contratista que tiene asignado el analista');
					continue;
				end if;

                -- validamos que la zona de la orden este contenida en las zonas del tecnico 
                select
                    count(*)
                        into v_contador_zona2
                from aire.sgd_usuarios_zona z
                inner join aire.sgd_usuarios usr on usr.id_usuario = z.id_usuario
                where usr.usuario = trim(v_temp(i).usuario) and z.ind_activo = 'S' and z.id_zona = v_id_zona;

                if v_contador_zona2 = 0 then
                    -- actualizar registro con errores
                    sb_actualizar_temp(v_temp(i).numero_orden, '1', 'La orden '||v_temp(i).numero_orden||' tiene una zona diferente a las del tecnico.');
                    continue;
				end if;
                v_contador_zona2 := 0;

            exception
                when no_data_found then
                    -- actualizar registro con errores
                    sb_actualizar_temp(v_temp(i).numero_orden, '1', 'El tecnico '||v_temp(i).usuario||' no existe o se encuentra inactivo');
                    continue;
                when others then
                    -- actualizar registro con errores
                    sb_actualizar_temp(v_temp(i).numero_orden, '1', 'Error al consultar el tecnico '||v_temp(i).usuario||': '|| sqlerrm);
                    continue;
			end;
/*			
            -- validamos que el tipo de suspencion de la orden pueda ser atendido por el tipo de brigada asignado al tecnico
            declare
                v_id_tipo_brigada       aire.ctn_tipos_brigada.id_tipo_brigada%type;
                v_ids_tipos_brigadas    varchar2(4000);
            begin
                begin
                    select id_tipo_brigada
                      into v_id_tipo_brigada
                      from aire.v_ctn_contratistas_brigada
                     where identificacion_contratista_persona = v_temp(i).usuario
                       and ind_activo                         = 'S';
                exception
                    when others then
                    -- actualizar registro con errores
                        sb_actualizar_temp(v_temp(i).numero_orden, '1', 'Error al consultar el tipo de brigada asociado al tecnico '||v_temp(i).usuario||': '|| sqlerrm);
                        continue;
                end;
                
                select listagg(id_tipo_brigada, ',')
                  into v_ids_tipos_brigadas
                  from aire.scr_tipos_suspencion_tipo_brigada
                 where id_tipo_suspencion = v_id_tipo_suspencion;
                
                -- validamos si el tipo de brigada del tecnico esta en los tipo de brigradas que atienden el tipo de suspencion
                if not (instr(',' || v_ids_tipos_brigadas || ',', ',' || v_id_tipo_brigada || ',') > 0) then
                    -- actualizar registro con errores
                    sb_actualizar_temp(v_temp(i).numero_orden, '1', 'El tipo de suspencion de la orden '||v_temp(i).numero_orden||' no puede ser atentido por el tipo de brigada asociado al tecnico');
                    continue;
                end if;
            end;
*/            
			-- asignamos la orden al tecnico y marcamos la orden como asignada tecnico
			begin
				update aire.ord_ordenes
				   set id_estado_orden        = v_id_estado_orden_asignada_tecnico
				   	 , id_contratista_persona = v_id_contratista_persona
				 where id_orden        = v_id_orden;
			exception
				when others then
                    -- actualizar registro con errores
                    sb_actualizar_temp(v_temp(i).numero_orden, '1', 'Error al asignar la orden '||v_temp(i).numero_orden||' a tecnico '||v_temp(i).usuario||': '|| sqlerrm);
                    continue;
			end;
			
			begin
				-- notificamos al usuario
				select id_dispositivo
			      into v_id_dispositivo
				  from aire.sgd_usuarios
				 where id_persona = (
										select id_persona
				  						  from aire.ctn_contratistas_persona
										 where id_contratista_persona = v_id_contratista_persona
					   );

				v_notificacion := json_object_t();
				v_notificacion.put('id_dispositivo', v_id_dispositivo);
				v_notificacion.put('ind_android', true);
				v_notificacion.put('titulo', 'Asignación ordenes SCR');
				v_notificacion.put('cuerpo', 'Se asignaron ordenes de su gestión');
				v_notificacion.put('estado', 'asignada');
				v_notificacion.put('tipo', 'gestion_ordenes_scr');
				v_notificacion.put('fecha_envio', to_char(systimestamp, 'DD/MM/YYYY HH:MI:SS:FF PM'));

				v_json_array_notificacion.append(v_notificacion);

			exception
				when others then
					dbms_output.put_line('Error al armar la notificacion OP360: '|| sqlerrm);
					null;
			end;

            sb_actualizar_temp(v_temp(i).numero_orden, '0', 'Ok');
        end loop;
        
        -- se mandan las notificaciones en segundo plano
        if v_json_array_notificacion.get_size > 0 then
            aire.pkg_g_generales.prc_enviar_notificaciones_push_segundo_plano(
                e_notificaciones => v_json_array_notificacion.to_clob,
                s_respuesta	     => v_respuesta
            );
        end if;

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
            from aire.ord_ordenes_temporal_asignacion_tecnico
           where id_soporte       = v_id_soporte
             and usuario_registra = v_id_usuario_registra;
        
        -- eliminamos la tabla temporal
		delete aire.ord_ordenes_temporal_asignacion_tecnico
         where id_soporte       = v_id_soporte
           and usuario_registra = v_id_usuario_registra;
        
        commit;

        sb_escribir_respuesta(200, 'Archivo procesado');
    exception
        when others then
            sb_escribir_respuesta(1, 'Error al procesar ordenes:'|| sqlerrm);
    end prc_asignar_ordenes_masivo_tecnico;

    
end pkg_g_carlos_vargas_test7;