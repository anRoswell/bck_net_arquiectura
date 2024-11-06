CREATE OR REPLACE PACKAGE BODY "AIRE".PKG_G_DANIEL_GONZALEZ_TEST2 AS
  
    procedure prc_registrar_orden (
		e_json  		in 	clob,
        s_json 	out	clob
	) is
        v_json_orden                json_object_t;  
		v_id_log		            aire.gnl_logs.id_log%type;
		v_respuesta                 aire.tip_respuesta := aire.tip_respuesta(mensaje => 'Registro exitoso.', nombre_up => 'pkg_g_daniel_gonzalez_test.prc_registrar_orden');
        v_orden      	            aire.ord_ordenes%rowtype;
        c_orden                     aire.gnl_clientes%rowtype;
        valor_nic                   number;
        valor_nis                   number;
        valor_id_estado_servicio    number;
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
            valor_id_estado_servicio           := v_json_orden.get_number('id_estado_servicio');
           -- DBMS_OUTPUT.PUT_LINE('valor_id_estado_servicio = ' || valor_id_estado_servicio);
            
            v_orden.id_tipo_suspencion           := v_json_orden.get_number('id_tipo_suspencion');
            
            --v_orden.numero_orden            := (select max(numero_orden) from aire.ord_ordenes);
            
            select (max(numero_orden) + 1) into v_orden.numero_orden from aire.ord_ordenes;
            DBMS_OUTPUT.PUT_LINE('numero_orden = ' || v_orden.numero_orden);
            
            /*
            24-01-2024
            Se obtiene el valor del estado asignar 
            */
            SELECT
                id_estado_orden into v_orden.id_estado_orden
            FROM aire.ord_estados_orden 
            where   codigo_estado = 'SPEN' 
            and     id_actividad IN (select id_actividad from aire.gnl_actividades where nombre = 'SCR' and ind_activo = 'S');
            
            
            select id_cliente into v_orden.id_cliente
            from aire.gnl_clientes
            where nic = valor_nic;
            
            
            --Se realiza insert a tabla aire.ord_ordenes
            insert into aire.ord_ordenes(
            id_tipo_orden,id_cliente,id_estado_orden,numero_orden,id_actividad,id_tipo_suspencion
            )
            values(
            v_orden.id_tipo_orden,v_orden.id_cliente,v_orden.id_estado_orden,v_orden.numero_orden,101,v_orden.id_tipo_suspencion
            );
            commit;
            
            select id_orden into v_orden.id_orden from aire.ord_ordenes where numero_orden = v_orden.numero_orden;
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
                return;
        end;
        /*
        -- registramos la gestion
        aire.pkg_p_ordenes.prc_registrar_orden(
            e_orden	    => v_orden,
            s_respuesta	=> v_respuesta
        );

        -- validamos si hubo errores
        if v_respuesta.codigo <> 0 then
			v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al registrar orden: '|| v_respuesta.mensaje);
			apex_json.initialize_clob_output( p_preserve => false );
			apex_json.open_object();
			apex_json.write('codigo', 1);
			apex_json.write('mensaje', 'Error #'||v_id_log||' al registrar orden' || sqlerrm);
			apex_json.close_object();
			s_json := apex_json.get_clob_output;
			apex_json.free_output;
			return;
        end if;
        */
        
        apex_json.initialize_clob_output( p_preserve => false );
		apex_json.open_object();
		apex_json.write('codigo', v_orden.id_orden);
        apex_json.write('mensaje', 'Registro añadido con exito');
		--apex_json.write('mensaje', 'Registro añadido con exito, con el numero de orden: ' || v_orden.id_cliente);
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
	end prc_registrar_orden;

END PKG_G_DANIEL_GONZALEZ_TEST2;