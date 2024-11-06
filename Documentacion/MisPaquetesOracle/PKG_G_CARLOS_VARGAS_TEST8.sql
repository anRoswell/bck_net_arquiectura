CREATE OR REPLACE package "AIRE".pkg_g_carlos_vargas_test8 as

    --  este sp sobreescribe pgk_g_ordenes
    procedure prc_asignar_ordenes_masivo_tecnico(
        e_json  in 	clob,
        s_json  out	clob
    );

    --  este sp sobreescribe pgk_g_ordenes
    procedure prc_gestionar_orden_des_asignar_brigada (
        e_json  in  clob,
        s_json 	out	clob
	);

    --  este sp sobreescribe pgk_g_ordenes
    procedure prc_gestionar_orden_asignar_brigada (
        e_json  in  clob,
        s_json 	out	clob
	);

    procedure prc_descomprometer_ordenes (
        e_json  in  clob,
        s_json 	out	clob
	);

    -- este sobreescriba pkg_g_ordenes
    -- 27/03/2024
    procedure prc_consultar_ordenes_area_central (		
        e_json  in  clob,
        s_json 	out	clob
	); 

    -- este sobreescriba pkg_g_ordenes
    -- 27/03/2024
    procedure prc_consultar_ordenes_contratistas (		
        e_json  in  clob,
        s_json 	out	clob
	);

    -- query nuevo
    -- 27/03/2024
    -- OJO MIGRADO
    procedure prc_consultar_reportes_ejecutados (		
        e_json  in  clob,
        s_json 	out	clob
	); 

end pkg_g_carlos_vargas_test8;
/
CREATE OR REPLACE package body "AIRE".pkg_g_carlos_vargas_test8 as
    
    --/-- se migro a pkg_g_ordenes el 22/03/2024 11:11 am
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
                  into v_id_orden
				     , v_id_estado_orden
					 , v_nic
                     , v_id_contratista
                     , v_id_tipo_suspencion
                  from aire.ord_ordenes  a
				  join aire.gnl_clientes b on a.id_cliente = b.id_cliente
                 where a.numero_orden    = to_number(v_temp(i).numero_orden);

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
        
        sb_escribir_respuesta(0, 'Archivo procesado');
    exception
        when others then
            sb_escribir_respuesta(1, 'Error al procesar ordenes:'|| sqlerrm);
    end prc_asignar_ordenes_masivo_tecnico;

    --/--Se migro a pkg_g_ordenes desde pkg_g_carlos_vargas_test8 el 22/03/2024 16:49 pm
    --/--se migra nuevamente desde pkg_g_carlos_vargas_test8 el 31/03/2023
    procedure prc_gestionar_orden_des_asignar_brigada (
        e_json  in  clob,
        s_json 	out	clob
    ) is
        --e_json  	clob :=  '{"identificacion_contratista":"900123456","id_orden_string":"2681,2700"}';
        v_json_objeto                       json_object_t;
        v_id_log		                    aire.gnl_logs.id_log%type;
        v_respuesta		                    aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_ordenes.prc_gestionar_orden_des_asignar_brigada');

        v_identificacion                    varchar2(50 BYTE);
        v_id_contratista                    varchar2(100 BYTE);

        v_id_orden_string                   varchar2(4000 byte);
        v_cnt                               number;

        -- Declarar la tabla temporal
        l_num_list                          num_list_type := num_list_type();
        v_cnt_actualizar                    number;
        v_id_estado_orden                   number;
         
        type t_dispositivos_id              is table of aire.sgd_usuarios.id_dispositivo%type;
        v_dispositivos                      t_dispositivos_id;

        v_notificacion                      json_object_t;
        v_respuesta_notificacion            aire.tip_respuesta;

        v_ind_areacentral                   varchar2(2);
    BEGIN
    --LOG
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');

    -- Deserializar JSON
    apex_json.free_output;
    apex_json.initialize_clob_output( p_preserve => false );

    v_json_objeto           := json_object_t(e_json);
    v_identificacion        := v_json_objeto.get_string('identificacion_brigada');
    v_id_orden_string       := v_json_objeto.get_string('id_orden_string');
    v_ind_areacentral       := v_json_objeto.get_string('ind_areacentral');

    /*
        Si la peticion viene desde area central
        no se realiza esta validacion, ya que no envian
        la identificacion de la brigada.
    */
    if v_ind_areacentral != 'S' then
        begin  
            select id_contratista
                into v_id_contratista
            from aire.v_ctn_contratistas_persona
            where identificacion_contratista_persona = v_identificacion
            and ind_activo = 'S';        
        exception
            when no_data_found then
                apex_json.open_object();
                apex_json.write('codigo', 50011);
                apex_json.write('mensaje', 'No se encontro el contratista');
                apex_json.close_object();
                s_json := apex_json.get_clob_output;
                apex_json.free_output;
                return;
            when others then
                apex_json.open_object();
                apex_json.write('codigo', 50011);
                apex_json.write('mensaje', 'Error al consultar el contratista: '|| sqlerrm);
                apex_json.close_object();
                s_json := apex_json.get_clob_output;
                apex_json.free_output;
                return;
        end;
    end if;

    apex_json.free_output;

    --Validar, si no llega ningun registro retornar provocar error.
    SELECT
       TO_NUMBER(REGEXP_SUBSTR(v_id_orden_string, '[^,]+', 1, level))
       BULK COLLECT INTO l_num_list
    FROM dual
    CONNECT BY REGEXP_SUBSTR(v_id_orden_string, '[^,]+', 1, level) IS NOT NULL;

    --/--Solo se puede desasignar una orden del tecnico si se encuentra en estado 23-SEAS-Asignada Tecnico
    select 
        count(*) 
            into v_cnt_actualizar
    from aire.ord_ordenes
    where       id_orden in (select COLUMN_VALUE from TABLE(l_num_list))
            and id_contratista = v_id_contratista
            and id_contratista_persona is not null
            and id_estado_orden in (
                        select id_estado_orden
                        from aire.ord_estados_orden
                        where   codigo_estado in ('SEAS')
                            and id_actividad IN (select id_actividad from aire.gnl_actividades where prefijo = 'G_SCR' and ind_activo = 'S')
                        );
    --> si el usuario esta en el modulo de area central se debe hallar el id contratista a partir de la tabla de ordenes.
    --> sobreescribo la variable v_cnt_actualizar
    if v_ind_areacentral = 'S' then
        select count(*) 
            into v_cnt_actualizar
        from aire.ord_ordenes
        where       id_orden in (select COLUMN_VALUE from TABLE(l_num_list))
                --and id_contratista = v_id_contratista
                and id_contratista_persona is not null
                and id_estado_orden in (
                            select id_estado_orden
                            from aire.ord_estados_orden
                            where   codigo_estado in ('SEAS')
                                and id_actividad IN (select id_actividad from aire.gnl_actividades where prefijo = 'G_SCR' and ind_activo = 'S')
                            );
    end if;
    --DBMS_OUTPUT.PUT_LINE('v_id_contratista = ' || v_id_contratista);
    --DBMS_OUTPUT.PUT_LINE('v_id_orden_string = ' || v_id_orden_string);
    --DBMS_OUTPUT.PUT_LINE('v_cnt_actualizar = ' || v_cnt_actualizar);
    
    IF (v_cnt_actualizar > 0) THEN
        select id_estado_orden into v_id_estado_orden
        from aire.ord_estados_orden
        where   codigo_estado in ('SASI')
            and id_actividad IN (select id_actividad from aire.gnl_actividades where prefijo = 'G_SCR' and ind_activo = 'S');

        --si la peticion viene del modulo de ctn_contratistas
        --este era el codigo original
        if v_ind_areacentral = 'N' then
            ---Inicio obtener dispositivos a notificar
            select
                distinct c.id_dispositivo
                bulk collect into v_dispositivos
            from aire.ord_ordenes a
            join aire.ctn_contratistas_persona b on a.id_contratista_persona = b.id_contratista_persona
            join aire.sgd_usuarios c on b.id_persona = c.id_persona
            where a.id_orden in (select COLUMN_VALUE from TABLE(l_num_list))
            and a.id_contratista = v_id_contratista
            and a.id_contratista_persona is not null
            and id_estado_orden in (
                select id_estado_orden
                from aire.ord_estados_orden
                where codigo_estado in ('SEAS')
                    and id_actividad IN (select id_actividad from aire.gnl_actividades where prefijo = 'G_SCR' and ind_activo = 'S')
                );
            ---Fin obtener dispositivos a notificar
        
            ----Actualizar ordenes
            update aire.ord_ordenes
            set id_contratista_persona = NULL, id_estado_orden = v_id_estado_orden
            where   id_orden in (select COLUMN_VALUE from TABLE(l_num_list))
                and id_contratista = v_id_contratista
                and id_contratista_persona is not null
                and id_estado_orden in (
                    select id_estado_orden
                    from aire.ord_estados_orden
                    where   codigo_estado in ('SEAS')
                        and id_actividad IN (select id_actividad from aire.gnl_actividades where prefijo = 'G_SCR' and ind_activo = 'S')
                    );
        
        -- si la peticion viene del modulo de area central
        else
            ---Inicio obtener dispositivos a notificar
            select
                distinct c.id_dispositivo
                bulk collect into v_dispositivos
            from aire.ord_ordenes a
            join aire.ctn_contratistas_persona b on a.id_contratista_persona = b.id_contratista_persona
            join aire.sgd_usuarios c on b.id_persona = c.id_persona
            where a.id_orden in (select COLUMN_VALUE from TABLE(l_num_list))
            ---and a.id_contratista = v_id_contratista
            and a.id_contratista_persona is not null
            and id_estado_orden in (
                select id_estado_orden
                from aire.ord_estados_orden
                where codigo_estado in ('SEAS')
                    and id_actividad IN (select id_actividad from aire.gnl_actividades where prefijo = 'G_SCR' and ind_activo = 'S')
                );
            ---Fin obtener dispositivos a notificar
        
            ----Actualizar ordenes
            update aire.ord_ordenes
            set id_contratista_persona = NULL, id_estado_orden = v_id_estado_orden
            where   id_orden in (select COLUMN_VALUE from TABLE(l_num_list))
                ---and id_contratista = v_id_contratista
                and id_contratista_persona is not null
                and id_estado_orden in (
                    select id_estado_orden
                    from aire.ord_estados_orden
                    where   codigo_estado in ('SEAS')
                        and id_actividad IN (select id_actividad from aire.gnl_actividades where prefijo = 'G_SCR' and ind_activo = 'S')
                    );
        end if;

        ---INICIO enviar notificacion a celular
            for i in 1 .. v_dispositivos.count loop
                    v_notificacion := json_object_t();
                    v_notificacion.put('id_dispositivo', v_dispositivos(i));
                    v_notificacion.put('ind_android', true);
                    v_notificacion.put('titulo', 'Desasignación ordenes SCR');
                    v_notificacion.put('cuerpo', 'Se desasignaron ordenes de su gestión'); -- cuando es desasignación que diga Se desasignaron x ordenes a su gestión.
                    v_notificacion.put('estado', 'desasignada');
                    v_notificacion.put('tipo', 'gestion_ordenes_scr');
                    v_notificacion.put('fecha_envio', to_char(systimestamp, 'DD/MM/YYYY HH:MI:SS:FF PM'));
                    -- Se envia notificacion en segundo plano
                    aire.pkg_g_generales.prc_enviar_notificacion_push_segundo_plano( e_notificacion  => v_notificacion.to_clob, s_respuesta => v_respuesta_notificacion);
            end loop;
        ---FIN enviar notificacion a celular

        --
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
            apex_json.write('codigo', 50023);
            apex_json.write('mensaje', 'No se encontraron registros para actualizar. Solo se desasignan ordenes en estado 23-SEAS-Asignada Tecnico');
        apex_json.close_object();
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
        return;
    END IF;

    --LOG
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Carga exitosa');
    --dbms_output.put_line(s_json);
    exception
    when others then
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al crear el json: '|| sqlerrm);
        apex_json.free_output;
        apex_json.open_object();
        apex_json.write('codigo', 50030);
        apex_json.write('mensaje', 'Error N '||v_id_log ||' al desasignar una tecnico o brigada. ' || sqlerrm);
        apex_json.close_object();
        --v_json := apex_json.get_clob_output;
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
        --s_json := v_json;
        --dbms_output.put_line('xxx: '|| sqlerrm);
    END prc_gestionar_orden_des_asignar_brigada;

    --/ se migro a pkg_g_ordenes el 22/03/2024 10:31 am. NO USAR!
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
    
    --//--INICIO contar si se encuentran ordenes con estado diferente a 23-SEAS-Asignada Tecnico
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
    --//--FIN contar si se encuentran ordenes con estado diferente a 23-SEAS-Asignada Tecnico
    
    --//--INICIO Solo permitir asignar tecnico si tiene el estado orden 1-SASI-Asignada Contratista
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
    --//--FIN Si no se encuentra registros, devolver el error
    
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

    --/-- migrado desde pkg_g_carlos_vargas_test8 el 31/03/2024
    PROCEDURE prc_descomprometer_ordenes (
        e_json  in  clob,
        s_json 	out	clob
    ) is
        v_orden                             aire.ord_ordenes%rowtype;
        v_id_estado_orden                   aire.ord_estados_orden.id_estado_orden%type;
        v_id_comprometer_orden              aire.ord_estados_orden.id_estado_orden%type;
        v_codigo_estado                     aire.ord_estados_orden.codigo_estado%type;
        v_id_orden_string                   varchar2(4000);
        v_respuesta		                    aire.tip_respuesta := aire.tip_respuesta( mensaje => 'Orden descomprometida con exito.', nombre_up => 'pkg_g_carlos_vargas_test8.prc_descomprometer_ordenes');    
        v_id_log	                        aire.gnl_logs.id_log%type;
        
        type t_orden_id                             is table of aire.ord_ordenes.id_orden%type;
        v_ordenes                                   t_orden_id;
        v_contador_error                    number;

        procedure sb_escribir_respuesta(codigo in number, mensaje in varchar2)
        is begin
            apex_json.free_output; 
            apex_json.initialize_clob_output( p_preserve => false );
            apex_json.open_object();
            apex_json.write('codigo', codigo);
            apex_json.write('mensaje',mensaje);
            apex_json.close_object();
            s_json := apex_json.get_clob_output;
            apex_json.free_output;
            --dbms_output.put_line('s_json: '||s_json);
        end;
    BEGIN
    
        v_id_orden_string               := json_value(e_json, '$.id_orden_string');
        
        --Split de Ordenes
        SELECT
            TO_NUMBER(REGEXP_SUBSTR(v_id_orden_string, '[^,]+', 1, level))
            BULK COLLECT INTO v_ordenes
        FROM dual
        CONNECT BY REGEXP_SUBSTR(v_id_orden_string, '[^,]+', 1, level) IS NOT NULL;
        
        -- consultamos el estado comprometida
        begin
            select id_estado_orden
              into v_id_estado_orden
              from aire.ord_estados_orden
             where codigo_estado = 'SCOM';
        exception
            when others then
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al consultar el estado comprometida: '||sqlerrm);
                sb_escribir_respuesta(1, 'Error N '||v_id_log||' al comprometer orden: no se encontro la parametrizacion del estado de la orden comprometida');
                return;
    
        --dbms_output.put_line('Estado orden comprometida = ' || v_id_estado_comprometida);
        end;
        
        --consultamos el estado con el que vamos a comprometer la orden
        begin
            select id_estado_orden
              into v_id_comprometer_orden
              from aire.ord_estados_orden
             where codigo_estado = 'SEAS';
        exception
            when others then
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al consultar el estado comprometida: '||sqlerrm);
                sb_escribir_respuesta(1, 'Error N '||v_id_log||' al comprometer orden: no se encontro la parametrizacion del estado de la orden comprometida');
                return;
    
        --dbms_output.put_line('Estado orden comprometida = ' || v_id_estado_comprometida);
        
        end;

        select count(*)
                into v_contador_error
        from aire.ord_ordenes a        
        where a.id_orden in (
             SELECT
            TO_NUMBER(REGEXP_SUBSTR(v_id_orden_string, '[^,]+', 1, level))                
            FROM dual
            CONNECT BY REGEXP_SUBSTR(v_id_orden_string, '[^,]+', 1, level) IS NOT NULL
        ) and a.id_estado_orden not in (
            select id_estado_orden              
              from aire.ord_estados_orden
             where codigo_estado = 'SCOM'
        );

        -- validamos que el codigo de estado sea igual a 'SCOM'
        if v_contador_error != 0 then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log ('pkg_g_carlos_vargas_test8.prc_descomprometer_ordenes', aire.pkg_g_seguridad.v_tipo_mensaje_error, 'El codigo del estado de la orden no corresponde');
            sb_escribir_respuesta('1', 'El codigo del estado de la orden no corresponde a comprometida');
            --raise_application_error(1, 'El código del estado de la orden no corresponde a comprometida');
            return;
        end if;
        
        for i in 1 .. v_ordenes.count loop
            -- descomprometemos la orden
            update aire.ord_ordenes 
               set id_estado_orden = v_id_comprometer_orden
             where id_orden = v_ordenes(i);
             
            commit;                
        end loop;
        sb_escribir_respuesta(0, v_respuesta.mensaje);
        
        exception
        when others then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al descomprometer orden: '||sqlerrm);
            sb_escribir_respuesta(1, 'Error N '||v_id_log||' al descomprometer orden');
            return;
        
    END prc_descomprometer_ordenes;

    --/--OJO! nuevamente migrado desde pkg_g_carlos_vargas_test8 el 31/03/2024
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
                            , n.expired_balance as deuda
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


                        inner join aire.ord_estados_orden   			b on a.id_estado_orden   		= b.id_estado_orden                        
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



                    --apex_json.write(''id_tipo_orden'', c_datos.id_tipo_orden);
                    -- apex_json.write(''id_estado_orden'', c_datos.id_estado_orden);
                    -- apex_json.write(''id_contratista'', c_datos.id_contratista);
                    -- apex_json.write(''id_cliente'', c_datos.id_cliente);
                    --apex_json.write(''cliente'', c_datos.cliente);
                    -- apex_json.write(''id_territorial'', c_datos.id_territorial);
                    -- apex_json.write(''id_zona'', c_datos.id_zona);
                    --apex_json.write(''fecha_creacion'', c_datos.fecha_creacion);
                    --apex_json.write(''fecha_cierre'', c_datos.fecha_cierre);
                    -- apex_json.write(''id_usuario_cierre'', c_datos.id_usuario_cierre);
                    --apex_json.write(''usuario_cierre'', c_datos.usuario_cierre);
                    -- apex_json.write(''id_actividad'', c_datos.id_actividad);
                    --apex_json.write(''actividad'', c_datos.actividad);
                    -- apex_json.write(''id_tipo_trabajo'', c_datos.id_tipo_trabajo);
                    --apex_json.write(''tipo_trabajo'', c_datos.tipo_trabajo);
                    -- apex_json.write(''id_tipo_suspencion'', c_datos.id_tipo_suspencion);
                    
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

    --/- se migra desde pkg_g_carlos_vargas_test8 el 31/03/2024
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
                            , d.deuda
                            , d.ultima_factura
                            , a.id_contratista_persona                            
                            , dpt.nombre as Departamento
                            , mnc.nombre as Municipio
                            , brr.nombre as Barrio
                            , tb.descripcion as tipo_brigada
                            , a.comentario_orden_servicio_num1 Comanterio_OS
                            , localtimestamp Fecha_Consulta
                            , a.fecha_asigna_tecnico
                        from aire.ord_ordenes              				a
                        inner join aire.ord_estados_orden   				b on a.id_estado_orden   		= b.id_estado_orden
                        left join aire.v_ctn_contratistas  				c on a.id_contratista    		= c.id_contratista
                        left join aire.gnl_clientes        				d on a.id_cliente        		= d.id_cliente
                        
                        left join aire.gnl_departamentos                dpt on dpt.id_departamento      = d.id_departamento
                        left join aire.gnl_municipios                   mnc on mnc.id_municipio         = d.id_municipio
                        left join aire.gnl_barrios                      brr on brr.id_barrio            = d.id_barrio

                        left join aire.gnl_territoriales   				e on a.id_territorial    		= e.id_territorial
                        left join aire.gnl_zonas           				f on a.id_zona           		= f.id_zona
                        inner join aire.sgd_usuarios_zona uz on uz.id_zona = f.id_zona and uz.id_usuario = '||v_id_usuario||'
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
                    --apex_json.write(''id_tipo_suspencion'', c_datos.id_tipo_suspencion);
                    
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

        v_list_contratistas VARCHAR(4000);
        v_list_zonas        VARCHAR(4000);
        v_ruta_web          VARCHAR(400);
    BEGIN
    
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
    apex_json.free_output;
    apex_json.initialize_clob_output( p_preserve => false );
    
    -- parseamos el json
    v_json_objeto := json_object_t(e_json);
    -- extraemos los datos de la orden
    v_objeto.id_contratista         := v_json_objeto.get_string('id_contratista');
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

    -- DBMS_OUTPUT.PUT_LINE('v_ServerSide: ' || v_ServerSide.to_string());
    
    --- Hallar los contratistas y zonas definidas por contrato en scr.
    -- select 
    --      LISTAGG(distinct x.id_contratista, ', ') WITHIN GROUP (ORDER BY x.id_contratista) as id_contratista
    --     ,LISTAGG(distinct c.id_zona, ', ') WITHIN GROUP (ORDER BY c.id_zona) as id_zona
    --         into v_list_contratistas, v_list_zonas
    -- from aire.v_ctn_contratistas x
    -- inner join (
    --     select id_contratista,id_contrato
    --     from aire.v_ctn_contratos
    --     where UPPER(prefijo_actividad) = 'G_SCR'
    --     and UPPER(ind_activo) = 'S'
    -- ) b on b.id_contratista = x.id_contratista
    -- inner join aire.ctn_contratos_zona c on c.id_contrato = b.id_contrato
    -- where UPPER(x.ind_activo) = 'S' and
    --     case 
    --         when v_objeto.id_contratista = -2 and (x.id_contratista is null or x.id_contratista is not null) then 1
    --         when v_objeto.id_contratista = -1 and x.id_contratista is null then 1
    --         when v_objeto.id_contratista > 0 and x.id_contratista = v_objeto.id_contratista then 1
    --         else 0 end = 1;


    -- DBMS_OUTPUT.PUT_LINE('v_list_zonas: ' || v_list_zonas);


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
    where id_ruta_archivo_servidor = 104;
    
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
                              case when nvl(m3.nombres,''-1'') = ''-1'' then null else m3.nombres || '' '' || m3.apellidos end as nombre_contratista_persona
                            , e.nombre as territorial
                            , f.nombre || ''_'' || a.id_zona as zona
                            , min(nvl(q.acta,''0'')) over() as acta
                            , min(q.fecha_ejecucion) over() as fechaejecucion
                            , min(q.fecha_inicio_ejecucion) over() as fechainicial
                            , min(q.fecha_fin_ejecucion) over() as fechafinal


                            , a.id_orden
                            , a.numero_orden
                            , d.nic
                            , brr.nombre as barrio
                            , a.direcion as direccion
                            , h.codigo_tipo_orden as tipo_orden
                            , k.descripcion as tipo_trabajo
                            , n.expired_periodos as num_factura
                            , n.expired_balance as deuda_act

                            , d.tarifa
                            , q2.nombre tipo_actividad
                            , REPLACE(:v0,''xnOrdnxn'',a.id_orden) as UrlDescargaActa


                            --, a.id_tipo_orden
                            --, b.descripcion  as estado_orden
                            --, case when nvl(m1.nombres,''-1'') = ''-1'' then null else m1.nombres || '' '' || m1.apellidos || ''_'' || a.id_contratista end as contratista
                            --, REPLACE(REGEXP_REPLACE(d.nombre_cliente, ''[[:cntrl:]]'', '' ''),''"'','' '') as cliente
                            
                            --, a.fecha_creacion
                            --, a.fecha_cierre as fecha_cierre
                            --, case when nvl(m2.nombres,''-1'') = ''-1'' then null else m2.nombres || '' '' || m2.apellidos end as usuario_cierre
                            --, i.nombre as actividad
                            --
                            --, l.descripcion as tipo_suspencion
                            --, a.origen
                            --, TRUNC(sysdate - TRUNC(a.fecha_registro)) as antiguedad
                            --, a.fecha_registro
                            --, a.id_contratista_persona
                            --, dpt.nombre as Departamento
                            --, mnc.nombre as Municipio
                            
                            --, tb.descripcion as tipo_brigada
                            --, a.comentario_orden_servicio_num1 Comanterio_OS
                            --, localtimestamp Fecha_Consulta
                            --, a.fecha_asigna_tecnico
                        from aire.ord_ordenes              				a
                        left join aire.ord_ordenes_gestion              q  on q.id_orden = a.id_orden
                        left join aire.gnl_actividades_economica        q2 on q2.id_actividad_economica = q.id_actividad_economica

                        left join aire.ord_ordenes_dato_suministro      n on n.id_orden = a.id_orden
                        inner join aire.ord_estados_orden   			b on a.id_estado_orden   		= b.id_estado_orden
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
                          q.nombre_contratista_persona
                        , q.territorial
                        , q.zona
                        , q.acta
                        , q.fechaejecucion
                        , q.fechainicial
                        , q.fechafinal
                        , q.id_orden
                        , q.numero_orden

                        , q.nic
                        , q.barrio

                        , q.direccion
                        , q.tipo_orden
                        , q.tipo_trabajo
                        , q.num_factura
                        , q.deuda_act
                        , q.tarifa
                        , q.tipo_actividad

                        , q.UrlDescargaActa

                        , acnt.MaxRows RegistrosTotales
                    from tx q, txcnt acnt
                    ' || myDictionary('ServerSide_WhereAdd').value || '
                    ' || myDictionary('ServerSide_SortAdd').value || '
                    OFFSET ' || pageNumber || ' ROWS FETCH NEXT ' || pageSize ||' ROWS ONLY
                ) loop
                apex_json.open_object();
                    apex_json.write(''nombre_contratista_persona'', c_datos.nombre_contratista_persona);
                    apex_json.write(''territorial'', c_datos.territorial);
                    apex_json.write(''zona'', c_datos.zona);
                    apex_json.write(''acta'', c_datos.acta);
                    apex_json.write(''fechaejecucion'', c_datos.fechaejecucion);
                    apex_json.write(''fechainicial'', c_datos.fechainicial);
                    apex_json.write(''fechafinal'', c_datos.fechafinal);
                    apex_json.write(''id_orden'', c_datos.id_orden);
                    apex_json.write(''numero_orden'', c_datos.numero_orden);
                    apex_json.write(''nic'', c_datos.nic);
                    apex_json.write(''barrio'', c_datos.barrio);
                    apex_json.write(''direccion'', c_datos.direccion);
                    apex_json.write(''tipo_orden'', c_datos.tipo_orden);
                    apex_json.write(''tipo_trabajo'', c_datos.tipo_trabajo);
                    apex_json.write(''num_factura'', c_datos.num_factura);
                    apex_json.write(''deuda_act'', c_datos.deuda_act);
                    apex_json.write(''tarifa'', c_datos.tarifa);
                    apex_json.write(''tipo_actividad'', c_datos.tipo_actividad);


                    apex_json.write(''UrlDescargaActa'', c_datos.UrlDescargaActa);
                    
                    
                    BEGIN :v0 := c_datos.RegistrosTotales; END;
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
    END prc_consultar_reportes_ejecutados;

end pkg_g_carlos_vargas_test8;