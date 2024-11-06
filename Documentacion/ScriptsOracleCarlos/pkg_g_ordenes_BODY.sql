create or replace package body           pkg_g_ordenes as

    procedure prc_registrar_orden (
		e_json_orden  		in 	clob,
        s_json_respuesta 	out	clob
	) is
		v_json_orden    json_object_t;  
		v_id_log		aire.gnl_logs.id_log%type;
		v_respuesta     aire.tip_respuesta := aire.tip_respuesta(mensaje => 'Registro exitoso.', nombre_up => 'pkg_g_ordenes.prc_registrar_orden');
        v_orden      	aire.ord_ordenes%rowtype;
	begin
        -- validamos el Json de la orden y armamos el rowtype
        begin
            -- parseamos el json
            v_json_orden := json_object_t(e_json_orden);

            -- extraemos los datos de la orden
            v_orden.id_tipo_orden           := v_json_orden.get_string('id_tipo_orden');
            v_orden.numero_orden            := v_json_orden.get_string('numero_orden');
            v_orden.id_estado_orden         := v_json_orden.get_string('id_estado_orden');
            v_orden.id_contratista          := v_json_orden.get_string('id_contratista');
            v_orden.id_cliente              := v_json_orden.get_string('id_cliente');
            v_orden.id_territorial          := v_json_orden.get_string('id_territorial');
            v_orden.id_zona                 := v_json_orden.get_string('id_zona');
            v_orden.direcion                := v_json_orden.get_string('direcion');
            v_orden.fecha_creacion          := v_json_orden.get_string('fecha_creacion');
            v_orden.fecha_cierre            := v_json_orden.get_string('fecha_cierre');
            v_orden.id_usuario_cierre       := v_json_orden.get_string('id_usuario_cierre');
            v_orden.descripcion             := v_json_orden.get_string('descripcion');
            v_orden.comentarios             := v_json_orden.get_string('comentarios');
            v_orden.acta                    := v_json_orden.get_string('acta');
            v_orden.id_actividad            := v_json_orden.get_string('id_actividad');
            v_orden.id_contratista_persona  := v_json_orden.get_string('id_contratista_persona');
        exception 
            when others then 
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al procesar el json de la orden: '|| sqlerrm);
                apex_json.initialize_clob_output( p_preserve => false );
                apex_json.open_object();
                apex_json.write('codigo', 1);
                apex_json.write('mensaje', 'Error #'||v_id_log||' al registrar orden');
                apex_json.close_object();
                s_json_respuesta := apex_json.get_clob_output;
                apex_json.free_output;
                return;
        end;    
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
			apex_json.write('mensaje', 'Error #'||v_id_log||' al registrar orden');
			apex_json.close_object();
			s_json_respuesta := apex_json.get_clob_output;
			apex_json.free_output;
			return;
        end if;

		apex_json.initialize_clob_output( p_preserve => false );
		apex_json.open_object();
		apex_json.write('codigo', 0);
		apex_json.write('mensaje', 'Orden registrada exitosamente');
		apex_json.open_object('datos');
		apex_json.write('id_orden',v_orden.id_orden);
		apex_json.close_object();
		apex_json.close_object();
		s_json_respuesta := apex_json.get_clob_output;
		apex_json.free_output;
	exception
		when others then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al registrar orden: '|| sqlerrm);
            apex_json.initialize_clob_output( p_preserve => false );
            apex_json.open_object();
            apex_json.write('codigo', 1);
            apex_json.write('mensaje', 'Error #'||v_id_log||' al registrar orden');
            apex_json.close_object();
            s_json_respuesta := apex_json.get_clob_output;
            apex_json.free_output;
	end prc_registrar_orden;

    procedure prc_actualizar_orden (
        e_json_orden        in  clob,
        s_json_respuesta	out clob
    ) is
        v_json_orden    json_object_t;
		v_id_log	    aire.gnl_logs.id_log%type;
		v_respuesta     aire.tip_respuesta := aire.tip_respuesta(mensaje => 'Registro Actualizado.', nombre_up => 'pkg_g_ordenes.prc_actualizar_orden');
        v_orden         aire.ord_ordenes%rowtype;
	begin
        -- validamos el Json de la orden y armamos el rowtype
        begin
            -- parseamos el json
            v_json_orden := json_object_t(e_json_orden);

            -- extraemos los datos de la orden
            v_orden.id_orden                := v_json_orden.get_string('id_orden');
            v_orden.id_tipo_orden           := v_json_orden.get_string('id_tipo_orden');
            v_orden.numero_orden            := v_json_orden.get_string('numero_orden');
            v_orden.id_estado_orden         := v_json_orden.get_string('id_estado_orden');
            v_orden.id_contratista          := v_json_orden.get_string('id_contratista');
            v_orden.id_cliente              := v_json_orden.get_string('id_cliente');
            v_orden.id_territorial          := v_json_orden.get_string('id_territorial');
            v_orden.id_zona                 := v_json_orden.get_string('id_zona');
            v_orden.direcion                := v_json_orden.get_string('direcion');
            v_orden.fecha_creacion          := v_json_orden.get_string('fecha_creacion');
            v_orden.fecha_cierre            := v_json_orden.get_string('fecha_cierre');
            v_orden.id_usuario_cierre       := v_json_orden.get_string('id_usuario_cierre');
            v_orden.descripcion             := v_json_orden.get_string('descripcion');
            v_orden.comentarios             := v_json_orden.get_string('comentarios');
            v_orden.acta                    := v_json_orden.get_string('acta');
            v_orden.id_actividad            := v_json_orden.get_string('id_actividad');
            v_orden.id_contratista_persona  := v_json_orden.get_string('id_contratista_persona');
        exception 
            when others then 
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al procesar el json de la orden: '|| sqlerrm);
                apex_json.initialize_clob_output( p_preserve => false );
                apex_json.open_object();
                apex_json.write('codigo', 1);
                apex_json.write('mensaje', 'Error #'||v_id_log||' al actualizar orden');
                apex_json.close_object();
                s_json_respuesta := apex_json.get_clob_output;
                apex_json.free_output;
                return;
        end;

        -- registramos la gestion
        aire.pkg_p_ordenes.prc_actualizar_orden(
            e_orden	    => v_orden,
            s_respuesta	=> v_respuesta
        );

        -- validamos si hubo errores
        if v_respuesta.codigo <> 0 then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al actualizar orden: '|| v_respuesta.mensaje);
            apex_json.initialize_clob_output( p_preserve => false );
            apex_json.open_object();
            apex_json.write('codigo', 1);
            apex_json.write('mensaje', 'Error #'||v_id_log||' al actualizar orden');
            apex_json.close_object();
            s_json_respuesta := apex_json.get_clob_output;
            apex_json.free_output;
            return;
        end if;

        apex_json.initialize_clob_output( p_preserve => false );
        apex_json.open_object();
        apex_json.write('codigo', 0);
        apex_json.write('mensaje', 'Orden actualizada con exito');
        apex_json.close_object();
        s_json_respuesta := apex_json.get_clob_output;
        apex_json.free_output;
	exception
		when others then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al actualizar orden: '|| sqlerrm);
            apex_json.initialize_clob_output( p_preserve => false );
            apex_json.open_object();
            apex_json.write('codigo', 1);
            apex_json.write('mensaje', 'Error #'||v_id_log||' al actualizar orden');
            apex_json.close_object();
            s_json_respuesta := apex_json.get_clob_output;
            apex_json.free_output;
	end prc_actualizar_orden;

	procedure prc_consultar_ordenes_asignadas_tecnico(
		e_parametros 	 in 	clob,
		s_json_respuesta out 	clob
	) is
		v_parametros				json_object_t;
		v_json                      clob := '[]';
		v_ordenes                   apex_json.t_values;
		v_id_contratista_persona    aire.ctn_contratistas_persona.id_contratista_persona%type;

		v_id_log		aire.gnl_logs.id_log%type;
		v_respuesta     aire.tip_respuesta := aire.tip_respuesta(mensaje => 'Consulta exitosa.', nombre_up => 'pkg_g_ordenes.prc_consultar_ordenes_asignadas_tecnico');

		procedure sb_escribir_respuesta(codigo in number, mensaje in varchar2, ordenes apex_json.t_values)
		is begin
			apex_json.free_output; 
			apex_json.initialize_clob_output( p_preserve => false );
			apex_json.open_object();
			apex_json.write('codigo', codigo);
			apex_json.write('mensaje',mensaje);
			if ordenes is not null then
				apex_json.write('datos', ordenes);
			end if;
			apex_json.close_object();
			s_json_respuesta := apex_json.get_clob_output;
			apex_json.free_output;
		end;   	
	begin
		apex_json.parse(v_ordenes, v_json);
      	-- validamos el Json de parametros
        begin
            -- parseamos el json
            v_parametros := json_object_t(e_parametros);

            -- extraemos los datos de la orden
            v_id_contratista_persona := v_parametros.get_string('id_contratista_persona');

			if v_id_contratista_persona is null then
				v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al procesar el json de la orden: Faltan parametros requeridos');
				sb_escribir_respuesta(1, 'Error '||v_id_log||' faltan parametros requeridos', v_ordenes);
				return;
			end if;
        exception 
            when others then 
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al procesar el json de la orden: '|| sqlerrm);
                sb_escribir_respuesta(1, 'Error '||v_id_log|| ' al consultar orndes asignadas', v_ordenes);
                return;
        end;

		-- armamos el json con las ordenes asignadas
        select json_arrayagg(json_object(*) returning clob)
          into v_json
          from (
                    select c.descripcion                                            as "tipo"
                         , b.nic                                                    as "nic"
                         , a.id_orden 
                         , to_number(a.numero_orden)                                as "orden"
                         , b.direccion                                              as "direccion"
                         , d.descripcion                                            as "estado"
                         , json_object(
                                 key 'latitud'  value nvl(to_number(json_value(b.georreferencia, '$.latitud'), '9999.99999999'), 0),
                                 key 'longitud' value nvl(to_number(json_value(b.georreferencia, '$.longitud'), '9999.99999999'), 0) 
                         )                                                          as "georreferencia"
                         , a.numero_orden                                           as "numero_orden"
                         , nvl(a.acta, 'NA')                                        as "acta"
                         , nvl(a.descripcion, 'NA')                                 as "descripcion"
                         , b.nombre_cliente                                         as "nombre_cliente"
                         , e.nombre                                                 as "municipio"
                         , nvl(b.tarifa, 'NA')                                      as "tarifa"
                         , b.telefono_cliente                                       as "telefono_cliente"
                         , nvl(b.carga_contratada, 0)                               as "carga_contratada"
                         , b.numero_medidor                                         as "numero_medidor"
                         , b.marca_medidor                                          as "marca_medidor"
                         , nvl(to_char(b.fecha_ultima_factura, 'DD/MM/YYYY'), 'NA') as "fecha_ultima_factura"
                         , nvl(b.deuda, 0)                                          as "deuda"
                         , nvl(b.cantidad_facturas,0)                               as "cantidad_facturas"
                         , nvl(a.comentarios, 'NA')                                 as "comentarios"
                         , nvl(b.ultima_factura, 0)                                 as "ultima_factura"
                         , d.codigo_estado                                          as "codigo_estado"
                      from aire.ord_ordenes       a
                      join aire.gnl_clientes      b on a.id_cliente      = b.id_cliente
                      join aire.ord_tipos_orden   c on a.id_tipo_orden   = c.id_tipo_orden
                      join aire.ord_estados_orden d on a.id_estado_orden = d.id_estado_orden 
                      join aire.gnl_municipios    e on b.id_municipio    = e.id_municipio
                    where a.id_contratista_persona = v_id_contratista_persona
                      and a.id_estado_orden        = (select id_estado_orden from aire.ord_estados_orden where codigo_estado = 'SEAS')
               );

		-- validamos si se encontraron ordenes asignadas
		if v_json is null then
			v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'No se encontraron ordenes asignadas');
            sb_escribir_respuesta(0, 'No se encontraron ordenes asignadas', v_ordenes);
			return;
		end if;
		apex_json.parse(v_ordenes, v_json);
		sb_escribir_respuesta(0, 'Consulta exitosa', v_ordenes);
	exception
		when others then
			v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al consultar ordenes asignadas: '|| sqlerrm);
			sb_escribir_respuesta(1, 'Error '||v_id_log||' al consultar ordenes', v_ordenes);		
	end prc_consultar_ordenes_asignadas_tecnico;

    function fnc_obtener_valor_filtro(
        e_filtro  in  aire.ord_filtros%rowtype
    ) return aire.tip_respuesta is
        v_tipo_dato aire.ord_columnas_filtro.tipo_dato%type;
        v_nombre_columna aire.ord_columnas_filtro.nombre_columna%type;
        v_operador  aire.gnl_dominios_valor.valor%type;
        v_valor_1   varchar2(250);
        v_valor_2   varchar2(250);	

        v_id_log	aire.gnl_logs.id_log%type;
		v_respuesta aire.tip_respuesta := aire.tip_respuesta(mensaje => 'Valor obtenido con exito.', nombre_up => 'pkg_g_ordenes.fnc_obtener_valor_filtro');
    begin
        -- consultamos el tipo de dato
        select tipo_dato, nombre_columna
          into v_tipo_dato, v_nombre_columna
          from aire.ord_columnas_filtro 
         where id_columna_filtro = e_filtro.id_columna_filtro;

        -- consultamos el operador
        select valor
          into v_operador
          from aire.gnl_dominios_valor
         where id_dominio_valor = e_filtro.id_operador;

        -- concatenamos ' para todos los valores
        if v_operador in ('in', 'not in') then
            select listagg(chr(39) || trim(regexp_substr(valores, '[^,]+', 1, level)) || chr(39), ',') within group (order by 1)
                into v_respuesta.mensaje
                from (select e_filtro.valor as valores from dual)
            connect by level <= regexp_count(valores, ',') + 1;

        elsif v_operador in ('=', '<>', '>=', '<=', '<', '>') then
            if v_tipo_dato = 'fecha' then
                -- formateamos la fecha
                begin
                    v_respuesta.mensaje := to_char(to_date(e_filtro.valor, 'DD/MM/YYYY'), 'DD/MM/YYYY');
                exception
                    when others then
                        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al formatear la fecha: '|| sqlerrm);
                        v_respuesta.codigo  := 1;
                        v_respuesta.mensaje := 'Error N '||v_id_log||' al formatear la fecha';
                        return v_respuesta;
                end;

            end if;

            if lower(v_nombre_columna) = 'limit' then
                v_respuesta.mensaje := to_number(e_filtro.valor);
            end if;

            v_respuesta.mensaje := chr(39)|| e_filtro.valor ||chr(39);

        elsif v_operador = 'between' then
            v_valor_1 := substr(e_filtro.valor, 1, instr(e_filtro.valor, ':') - 1);
            v_valor_2 := substr(e_filtro.valor, instr(e_filtro.valor, ':') + 1);

            -- formateamos la fecha
            begin
                if v_tipo_dato = 'fecha' then
                    v_valor_1 := to_char(to_date(v_valor_1, 'DD/MM/YYYY'), 'DD/MM/YYYY');
                    v_valor_2 := to_char(to_date(v_valor_2, 'DD/MM/YYYY'), 'DD/MM/YYYY'); 
                end if;
            exception
                when others then
                    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al formatear las fechas: '|| sqlerrm);
                    v_respuesta.codigo  := 1;
                    v_respuesta.mensaje := 'Error N '||v_id_log||' al formatear las fechas';
                    return v_respuesta;
            end;

            v_respuesta.mensaje := chr(39)||v_valor_1||''':'''||v_valor_2||chr(39);
        elsif v_operador = 'like' then
            v_respuesta.mensaje := chr(39)||'%'||lower(e_filtro.valor)||'%'||chr(39);
        end if;

        return v_respuesta;
    exception
        when others then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al procesar valor: '|| sqlerrm);
            v_respuesta.codigo  := 1;
            v_respuesta.mensaje := 'Error N '||v_id_log||' al procesar valor.';
            return v_respuesta;
    end fnc_obtener_valor_filtro;

    procedure prc_registrar_filtro(
        e_json    in  clob,
        s_json    out clob
    ) is
        v_filtro    aire.ord_filtros%rowtype;

		v_id_log	aire.gnl_logs.id_log%type;
		v_respuesta aire.tip_respuesta := aire.tip_respuesta(mensaje => 'Filtro registrado con exito.', nombre_up => 'pkg_g_ordenes.prc_registrar_filtro');
		v_cantidad  number := 0;

        procedure sb_escribir_respuesta(codigo in number, mensaje in varchar2, id_filtro aire.ord_filtros.id_filtro%type default null)
		is begin
			apex_json.free_output; 
			apex_json.initialize_clob_output( p_preserve => false );
			apex_json.open_object;
			apex_json.write('codigo', codigo);
			apex_json.write('mensaje',mensaje);
			if id_filtro is not null then
                apex_json.open_object('datos');
				apex_json.write('id_filtro', id_filtro);
                apex_json.close_object;
			end if;
			apex_json.close_object;
			s_json := apex_json.get_clob_output;
			apex_json.free_output;
		end;   

    begin
        -- extraemos informacion del json
        begin   
            v_filtro.id_columna_filtro := json_value(e_json, '$.id_columna_filtro');
            v_filtro.id_operador       := json_value(e_json, '$.id_operador');
            v_filtro.valor             := json_value(e_json, '$.valor');
        exception
            when others then
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al procesar json de entrada: '|| sqlerrm);
                sb_escribir_respuesta(1, 'Error N '||v_id_log||' al registrar filtro');
                return;
        end;

        -- validamos si ya la columna esta parametrizada
        select count(*)
          into v_cantidad
          from aire.ord_filtros
         where id_columna_filtro = v_filtro.id_columna_filtro;

        if v_cantidad > 0 then 
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al procesar json de entrada: '|| sqlerrm);
            sb_escribir_respuesta(1, 'Error N '||v_id_log||' al registrar filtro: ya se encuentra un filtro registrado para la misma columna');
            return;
        end if;

        -- obtenemos el valor transformado
        v_respuesta := fnc_obtener_valor_filtro(v_filtro);

        if v_respuesta.codigo <> 0 then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al procesar valor: '|| v_respuesta.mensaje);
            sb_escribir_respuesta(1, v_respuesta.mensaje);
            return;
        end if;

        v_filtro.valor := v_respuesta.mensaje;

        -- registramos el filtro
        aire.pkg_p_ordenes.prc_registrar_filtro (
            e_filtro	 => v_filtro,
            s_respuesta	 => v_respuesta
        );

        sb_escribir_respuesta(v_respuesta.codigo, v_respuesta.mensaje);
    exception
        when others then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al registrar filtro: '|| sqlerrm);
            sb_escribir_respuesta(1, 'Error N '||v_id_log||' al registrar filtro');
    end prc_registrar_filtro;


    procedure prc_actualizar_filtro(
        e_json    in  clob,
        s_json    out clob
    ) is
        v_filtro    aire.ord_filtros%rowtype;

		v_id_log	aire.gnl_logs.id_log%type;
		v_respuesta aire.tip_respuesta := aire.tip_respuesta(mensaje => 'Filtro registrado con exito.', nombre_up => 'pkg_g_ordenes.prc_actualizar_filtro');
		v_cantidad  number := 0;

        procedure sb_escribir_respuesta(codigo in number, mensaje in varchar2)
		is begin
			apex_json.free_output; 
			apex_json.initialize_clob_output( p_preserve => false );
			apex_json.open_object;
			apex_json.write('codigo', codigo);
			apex_json.write('mensaje',mensaje);
			apex_json.close_object;
			s_json := apex_json.get_clob_output;
			apex_json.free_output;
		end;   

    begin
        -- extraemos informacion del json
        begin   
            v_filtro.id_filtro         := json_value(e_json, '$.id_filtro');
            v_filtro.id_columna_filtro := json_value(e_json, '$.id_columna_filtro');
            v_filtro.id_operador       := json_value(e_json, '$.id_operador');
            v_filtro.valor             := json_value(e_json, '$.valor');
        exception
            when others then
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al procesar json de entrada: '|| sqlerrm);
                sb_escribir_respuesta(1, 'Error N '||v_id_log||' al actualizar filtro');
                return;
        end;

        -- validamos si ya la columna esta parametrizada
        select count(*)
          into v_cantidad
          from aire.ord_filtros
         where id_filtro         <> v_filtro.id_filtro
           and id_columna_filtro = v_filtro.id_columna_filtro;

        if v_cantidad > 0 then 
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al procesar json de entrada: '|| sqlerrm);
            sb_escribir_respuesta(1, 'Error N '||v_id_log||' al actualizar filtro: ya se encuentra un filtro registrado para la misma columna');
            return;
        end if;

        -- obtenemos el valor transformado
        v_respuesta := fnc_obtener_valor_filtro(v_filtro);

        if v_respuesta.codigo <> 0 then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al procesar valor: '|| v_respuesta.mensaje);
            sb_escribir_respuesta(1, v_respuesta.mensaje);
            return;
        end if;

        v_filtro.valor := v_respuesta.mensaje;

        -- registramos el filtro
        aire.pkg_p_ordenes.prc_actualizar_filtro (
            e_filtro	 => v_filtro,
            s_respuesta	 => v_respuesta
        );

        sb_escribir_respuesta(v_respuesta.codigo, v_respuesta.mensaje);
    exception
        when others then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al actualizar filtro: '|| sqlerrm);
            sb_escribir_respuesta(1, 'Error N '||v_id_log||' al actualizar filtro');
    end prc_actualizar_filtro;


    procedure prc_eliminar_filtro(
        e_json    in  clob,
        s_json    out clob
    ) is
		v_id_log	    aire.gnl_logs.id_log%type;
		v_respuesta     aire.tip_respuesta := aire.tip_respuesta(mensaje => 'Filtro eliminado con exito.', nombre_up => 'pkg_g_ordenes.prc_eliminar_filtro');
        v_id_filtro     aire.ord_filtros.id_filtro%type;
        v_ind_requerido aire.ord_columnas_filtro.ind_requerido%type;

        procedure sb_escribir_respuesta(codigo in number, mensaje in varchar2)
		is begin
			apex_json.free_output; 
			apex_json.initialize_clob_output( p_preserve => false );
			apex_json.open_object;
			apex_json.write('codigo', codigo);
			apex_json.write('mensaje',mensaje);
			apex_json.close_object;
			s_json := apex_json.get_clob_output;
			apex_json.free_output;
		end;   

    begin
        -- extraemos informacion del json
        begin   
            v_id_filtro := json_value(e_json, '$.id_filtro');
        exception
            when others then
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al procesar json de entrada: '|| sqlerrm);
                sb_escribir_respuesta(1, 'Error N '||v_id_log||' al eliminar filtro');
                return;
        end;

        -- validamos si el filtro es requerido
        begin
            select b.ind_requerido
              into v_ind_requerido
              from aire.ord_filtros         a
              join aire.ord_columnas_filtro b on a.id_columna_filtro = b.id_columna_filtro
             where a.id_filtro = v_id_filtro;

            if v_ind_requerido = 'S' then
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al eliminar filtro: El filtro es requerido no puede ser eliminado');
                sb_escribir_respuesta(1, 'Error N '||v_id_log||' al eliminar filtro: El filtro es requerido no puede ser eliminado');
                return;
            end if;

        exception
            when others then
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al eliminar filtro: '|| sqlerrm);
                sb_escribir_respuesta(1, 'Error N '||v_id_log||' al eliminar filtro: El filtro no existe');
                return;
        end;

        -- registramos el filtro
        aire.pkg_p_ordenes.prc_eliminar_filtro (
            e_id_filtro	 => v_id_filtro,
            s_respuesta	 => v_respuesta
        );

        sb_escribir_respuesta(v_respuesta.codigo, v_respuesta.mensaje);
    exception
        when others then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al eliminar filtro: '|| sqlerrm);
            sb_escribir_respuesta(1, 'Error N '||v_id_log||' al eliminar  filtro');
    end prc_eliminar_filtro;

    procedure prc_consultar_filtros(
        s_json    out clob
    ) is
        v_json         json_object_t := json_object_t();
        v_filtros      clob;

        v_id_log       aire.gnl_logs.id_log%type;
        v_respuesta    aire.tip_respuesta := aire.tip_respuesta(mensaje => 'Consulta exitosa.', nombre_up => 'pkg_g_ordenes.prc_consultar_filtros');

        procedure sb_escribir_respuesta(codigo in number, mensaje in varchar2, v_filtros clob) is
        begin
            v_json := json_object_t();
            v_json.put('codigo', codigo);
            v_json.put('mensaje', mensaje);
            if v_filtros is not null then
                v_json.put('datos', json_array_t(v_filtros));
            end if;
            s_json := v_json.to_clob;
        end sb_escribir_respuesta;
    begin
        -- Parsear el JSON inicial
        v_json := json_object_t();

        -- Armamos el JSON con los filtros asignados
        select json_arrayagg(json_object(
                    'id_filtro'          value id_filtro,
                    'id_columna_filtro'  value id_columna_filtro,
                    'id_operador'        value id_operador,
                    'valor'              value valor
                ) returning clob format json)
        into v_filtros
        from aire.ord_filtros;

        -- Validamos si se encontraron filtros
        if v_filtros is null then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log(v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'No se encontraron filtros parametrizados');
            sb_escribir_respuesta(0, 'No se encontraron filtros parametrizados', v_filtros);
            return;
        end if;

        sb_escribir_respuesta(0, 'Consulta exitosa', v_filtros);

    exception
        when others then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log(v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al consultar filtros: ' || sqlerrm);
            sb_escribir_respuesta(1, 'Error ' || v_id_log || ' al consultar filtros', v_filtros);
    end prc_consultar_filtros;

procedure prc_registrar_orden_gestion (
                                            e_json_orden        in  clob,
                                            s_json_respuesta	out clob
                                        ) as

        v_json_orden                json_object_t;
		v_id_log	                aire.gnl_logs.id_log%type;
        v_nombre_up                 varchar2(50) := 'pkg_g_ordenes.prc_registrar_orden_gestion';
		v_respuesta                 aire.tip_respuesta := aire.tip_respuesta(mensaje => 'Registro Actualizado.', nombre_up => 'pkg_g_ordenes.prc_registrar_orden_gestion');
        v_orden_gestion             aire.ord_ordenes_gestion%rowtype;
        v_orden_gestion_accion      aire.ord_ordenes_gestion_acciones%rowtype;
        v_articulo_operacion        aire.mtr_articulos_operacion%rowtype;
        v_id_resultado              aire.ord_anomalias.id_resultado%type;
        v_id_contratista_brigada    aire.ctn_contratistas_brigada.id_contratista_brigada%type;
        v_soporte                   aire.gnl_soportes%rowtype;
        v_id_actividad              aire.gnl_actividades.id_actividad%type;
        v_id_soporte_tipo           aire.gnl_tipos_soporte.id_tipo_soporte%type;
        v_id_dominio_valor          aire.gnl_dominios_valor.id_dominio_valor%type;
        v_orden                     aire.ord_ordenes%rowtype;
        v_serie_historico           aire.mtr_series_historico%rowtype;
        v_estado_orden              aire.ord_estados_orden.id_estado_orden%type;
        procedure sb_escribir_respuesta(
            codigo                      in number,
            mensaje                     in varchar2
        )is
        begin
            apex_json.free_output;
            apex_json.initialize_clob_output( p_preserve => false );
            apex_json.open_object();
            apex_json.write('codigo', codigo);
            apex_json.write('mensaje',mensaje);
            apex_json.close_object();
            s_json_respuesta := apex_json.get_clob_output;
            apex_json.free_output;
        end;

	begin
        -- validamos el Json de la orden
        begin
            -- parseamos el json
            v_json_orden := json_object_t(e_json_orden);

        exception 
            when others then 
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al procesar el json de la orden: '|| sqlerrm);
                sb_escribir_respuesta(1, 'Error #'||v_id_log||' al gestionar la orden.');
                return;
        end;


        -- armamos el rowtype
        begin
            begin
                select id_resultado into v_id_resultado
                from aire.ord_anomalias
                where id_anomalia = json_value(e_json_orden, '$.anomalias.id_anomalia');

                select id_contratista_brigada into v_id_contratista_brigada
                from aire.ctn_contratistas_brigada
                where id_contratista_persona = json_value(e_json_orden, '$.info_acta.id_contratista_persona') 
                    and ind_activo = 'S';
            exception
                when others then 
                    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al consultar el resultado: '|| sqlerrm);
                    sb_escribir_respuesta(1, 'Error #'||v_id_log||' al consultar informacion.');
                    return;
            end;
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a gestionar orden: '||e_json_orden);
            -- extraemos los datos de la orden
            v_orden_gestion.id_orden                              := json_value(e_json_orden, '$.id_orden');
            v_orden_gestion.ind_anomalia                          := case when to_char(json_value(e_json_orden, '$.tiene_alguna_anomalia')) = 'true' then 'S' else 'N' end; 
            v_orden_gestion.id_resultado                          := v_id_resultado;
            v_orden_gestion.id_anomalia                           := json_value(e_json_orden, '$.anomalias.id_anomalia');
            v_orden_gestion.id_subanomalia                        := json_value(e_json_orden, '$.anomalias.id_subanomalia');
            v_orden_gestion.observacion                           := null;
            v_orden_gestion.fecha_cierre                          := to_date(json_value(e_json_orden, '$.fecha_fin_ejecucion'), 'DD/MM/YYYY HH24:MI:SS');
            v_orden_gestion.fecha_ejecucion                       := to_date(json_value(e_json_orden, '$.info_acta.fecha_ejecucion'), 'DD/MM/YYYY HH24:MI:SS');
            v_orden_gestion.fecha_inicio_ejecucion                := to_date(json_value(e_json_orden, '$.fecha_inicio_ejecucion'), 'DD/MM/YYYY HH24:MI:SS');
            v_orden_gestion.fecha_fin_ejecucion                   := to_date(json_value(e_json_orden, '$.fecha_fin_ejecucion'), 'DD/MM/YYYY HH24:MI:SS');
            v_orden_gestion.id_contratista_persona                := json_value(e_json_orden, '$.info_acta.id_contratista_persona');
            v_orden_gestion.id_contratista_brigada                := v_id_contratista_brigada;
            v_orden_gestion.ind_servicio_con_energia              := case when to_char(json_value(e_json_orden, '$.info_servicio.estado_servicio')) = 'true' then 'S' else 'N' end;
            v_orden_gestion.id_estado_predio                      := json_value(e_json_orden, '$.info_servicio.estado_predio');
            v_orden_gestion.id_observacion_anomalia               := json_value(e_json_orden, '$.anomalias.observaciones_subanomalia');
            v_orden_gestion.acta                                  := json_value(e_json_orden, '$.info_acta.acta');
            v_orden_gestion.id_uso_energia                        := json_value(e_json_orden, '$.info_servicio.uso_energia');
            v_orden_gestion.id_actividad_economica                := json_value(e_json_orden, '$.info_servicio.tipo_uso_energia');
            v_orden_gestion.numero_medidor                        := json_value(e_json_orden, '$.info_servicio.numero_medidor');
            v_orden_gestion.ind_lectura_visible                   := case when to_char(json_value(e_json_orden, '$.info_servicio.lectura_visible')) = 'true' then 'S' else 'N' end;
            v_orden_gestion.id_tipo_no_visibilidad                := case when v_orden_gestion.ind_lectura_visible = 'N' then json_value(e_json_orden, '$.info_servicio.tipo_no_visibilidad') else null end;
            v_orden_gestion.ct                                    := json_value(e_json_orden, '$.info_servicio.ct');
            v_orden_gestion.mt                                    := json_value(e_json_orden, '$.info_servicio.mt');
            v_orden_gestion.nombres_persona_atiende               := json_value(e_json_orden, '$.persona_atiende.nombre_completo');
            v_orden_gestion.identificacion_persona_atiende        := json_value(e_json_orden, '$.persona_atiende.identificacion');
            v_orden_gestion.telefono_persona_atiende              := json_value(e_json_orden, '$.persona_atiende.telefono');
            v_orden_gestion.relacion_titular_persona_atiende      := json_value(e_json_orden, '$.persona_atiende.relacion_con_titular');
            v_orden_gestion.ind_solicita_asesoria_persona_atiende := case when to_char(json_value(e_json_orden, '$.persona_atiende.solicita_asesoria')) = 'true' then 'S' else 'N' end;
            v_orden_gestion.nombres_testigo                       := json_value(e_json_orden, '$.testigo.nombre_completo');
            v_orden_gestion.identificacion_testigo                := json_value(e_json_orden, '$.testigo.identificacion');
            v_orden_gestion.lectura                               := json_value(e_json_orden, '$.info_servicio.lectura');
            v_orden_gestion.ind_procesado_ws                      := 'N';
        exception 
            when others then 
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al procesar el json de la orden: '|| sqlerrm||' --- '||e_json_orden);
                sb_escribir_respuesta(1, 'Error #'||v_id_log||' al gestionar la orden.');
                return;
        end;

        --Validamos que la orden no este en estado cerrada
        begin
            select id_estado_orden into v_estado_orden
            from aire.ord_estados_orden
            where codigo_estado = 'CERR'
            and ind_activo = 'S';

        exception
                when others then 
                    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al consultar el resultado: '|| sqlerrm);
                    sb_escribir_respuesta(1, 'Error #'||v_id_log||' al consultar informacion.');
                    return;
        end;

        begin
            select * into v_orden
            from aire.ord_ordenes
            where id_orden = v_orden_gestion.id_orden;

            if v_orden.id_estado_orden = v_estado_orden then
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error no se puede procesar la gestion ya que la orden tiene un estado invalido.');
                sb_escribir_respuesta(1, 'Error #'||v_id_log||' Error no se puede procesar la gestion ya que la orden tiene un estado invalido.');
                return;
            end if;

        exception
                when others then 
                    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al consultar el resultado: '|| sqlerrm);
                    sb_escribir_respuesta(1, 'Error #'||v_id_log||' al consultar informacion.');
                    return;
        end;

        -- registramos la gestion
        aire.pkg_p_ordenes.prc_registrar_orden_gestion(
            e_orden_gestion	=> v_orden_gestion,
            s_respuesta	    => v_respuesta
        );

        -- validamos si hubo errores
        if v_respuesta.codigo <> 0 then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al gestionar la orden: '|| v_respuesta.mensaje);
            sb_escribir_respuesta(1, 'Error #'||v_id_log||' al gestionar la orden.');
            rollback;
            return;
        end if;

        --Buscamos la actividad
        begin
            select id_actividad into v_id_actividad
            from aire.gnl_actividades
            where prefijo = 'G';
        exception
            when others then
                v_id_log			:= aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, v_tipo_mensaje_error, 'Error parametríca asociada a la actividad: '|| sqlerrm);
                sb_escribir_respuesta(1, 'Error #'||v_id_log||' error parametríca asociada a la actividad.');
                rollback;
                return;
        end;

        --Realizamos el procesamiento de las acciones
        if v_orden_gestion.ind_anomalia = 'N' then
            for c_acciones in (select
                                    jt.id_accion,
                                    jt.observacion,
                                    jt.subactividad,
                                    jt.fotos,
                                    jt.materiales
                            from
                            json_table(e_json_orden, '$.acciones[*]'
                                columns (
                                id_accion    varchar2(50) path '$.id_accion',
                                observacion  varchar2(255) path '$.observacion',
                                subactividad varchar2(255) path '$.subactividad',
                                fotos clob format json path '$.fotos',
                                materiales clob format json path '$.materiales')) jt
            ) loop

                v_orden_gestion_accion.id_orden_gestion := v_orden_gestion.id_orden_gestion;
                v_orden_gestion_accion.id_accion        := c_acciones.id_accion;
                v_orden_gestion_accion.observacion      := c_acciones.observacion;
                v_orden_gestion_accion.id_subaccion     := c_acciones.subactividad;

                -- registramos la gestion
                aire.pkg_p_ordenes.prc_registrar_orden_gestion_acciones(
                    e_orden_gestion_accion	=> v_orden_gestion_accion,
                    s_respuesta	            => v_respuesta
                );

                -- validamos si hubo errores
                if v_respuesta.codigo <> 0 then
                    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al gestionar las acciones de la orden: '|| v_respuesta.mensaje);
                    sb_escribir_respuesta(1, 'Error #'||v_id_log||' al gestionar las acciones de la orden.');
                    rollback;
                    return;
                end if;

                --Registramos los soportes
                for c_soportes in (select   jt.codigo_tipo_soporte,
                                            jt.nombre,
                                            jt.peso,
                                            jt.formato,
                                            jt.url
                                    from
                                    json_table(c_acciones.fotos, '$[*]'
                                        columns (
                                        codigo_tipo_soporte    varchar2(50) path '$.codigo_tipo_soporte',
                                        nombre                 varchar2(200) path '$.nombre',
                                        peso                   varchar2(200) path '$.peso',
                                        formato                varchar2(200) path '$.formato',
                                        url                    varchar2(200) path '$.url'
                                        )) jt
                ) loop
                    v_soporte := null;

                    begin
                        select id_tipo_soporte into v_id_soporte_tipo
                        from aire.gnl_tipos_soporte
                        where codigo = c_soportes.codigo_tipo_soporte;
                    exception
                        when others then
                            v_id_log			:= aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, v_tipo_mensaje_error, 'Error parametríca asociada al tipo de soporte: '|| sqlerrm);
                            sb_escribir_respuesta(1, 'Error #'||v_id_log||' error parametríca asociada al tipo de soporte.');
                            rollback;	            
                            return;
                    end;

                    v_soporte.id_actividad        := v_id_actividad;
                    v_soporte.id_tipo_soporte     := v_id_soporte_tipo;
                    v_soporte.nombre              := c_soportes.nombre;
                    v_soporte.peso                := c_soportes.peso;
                    v_soporte.id_usuario_registro := json_value(e_json_orden, '$.cuestionarios[0].id_usuario');
                    v_soporte.fecha_registro      := sysdate;
                    v_soporte.formato             := c_soportes.formato;
                    v_soporte.ind_archivo_externo := 'S';
                    v_soporte.url_externa         := c_soportes.url;

                    --Registramos el soporte
                    aire.pkg_p_generales.prc_registrar_soporte(
                                                                e_soporte 	=> v_soporte,
                                                                s_respuesta	=> v_respuesta
                                                            );

                    --Validamos la respuesta del procedimiento
                    if v_respuesta.codigo <> 0 then
                        v_id_log	        := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, v_tipo_mensaje_seguimiento, 'No se pudo realizar el proceso de registro de las evidencias: '|| v_respuesta.mensaje);
                        sb_escribir_respuesta(1, 'Error #'||v_id_log||' no se pudo realizar el proceso de registro de las evidencias de la incidencia.');
                        rollback;
                        return;         
                    end if;

                    --Asociamos el soporte a la orden gestion
                    declare
                        v_orden_gestion_soporte aire.ord_ordenes_gestion_soporte%rowtype;
                    begin
                        v_orden_gestion_soporte.id_orden_gestion := v_orden_gestion.id_orden_gestion;
                        v_orden_gestion_soporte.id_soporte       := v_soporte.id_soporte;
                        aire.pkg_p_ordenes.prc_registrar_orden_gestion_soporte (
                                                                                    e_orden_gestion_soporte	    => v_orden_gestion_soporte,
                                                                                    s_respuesta	                => v_respuesta
                                                                                );
                        if v_respuesta.codigo <> 0 then
                            v_id_log	        := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, v_tipo_mensaje_seguimiento, 'No se pudo realizar el proceso de registro de las evidencias - orden gestion: '|| v_respuesta.mensaje);
                            sb_escribir_respuesta(1, 'Error #'||v_id_log||' no se pudo realizar el proceso de registro de las evidencias - orden gestion');
                            rollback;
                            return;         
                        end if;
                    end;


                end loop;

                --Registramos los materiales retirados
                select id_dominio_valor into v_id_dominio_valor
                from aire.gnl_dominios_valor
                where codigo = 'RETT'
                and ind_activo = 'S';

                for c_materiales in (select jt.id_articulo,
                                            jt.cantidad        
                                       from json_table(c_acciones.materiales, '$[*]'
                                                columns (
                                                    id_articulo    number path '$.id_articulo',
                                                    cantidad       number path '$.cantidad'
                                                )
                                            ) jt
                ) loop

                    v_articulo_operacion.id_articulo            := c_materiales.id_articulo;
                    v_articulo_operacion.id_tipo_operacion      := v_id_dominio_valor;
                    v_articulo_operacion.cantidad               := c_materiales.cantidad;
                    v_articulo_operacion.id_orden               := v_orden.id_orden;
                    v_articulo_operacion.observacion            := c_acciones.observacion;
                    v_articulo_operacion.id_contratista_persona := v_orden_gestion.id_contratista_persona;
                    v_articulo_operacion.fecha                  := sysdate;

                    aire.pkg_p_materiales.prc_registrar_articulos_operacion(
                        e_articulo_operacion => v_articulo_operacion,
                        s_respuesta          => v_respuesta
                    );

                    --Validamos la respuesta del procedimiento
                    if v_respuesta.codigo <> 0 then
                        v_id_log	        := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, v_tipo_mensaje_seguimiento, 'No se pudo realizar el proceso de registro de las evidencias de la incidencia: '|| v_respuesta.mensaje);
                        sb_escribir_respuesta(1, 'Error #'||v_id_log||' no se pudo realizar el proceso de registro de los materiales retirados.');
                        rollback;
                        return;         
                    end if;

                end loop;

            end loop;
        end if;

        if v_orden_gestion.ind_anomalia = 'N' then
            --Registramos los materiales instalados
            select id_dominio_valor into v_id_dominio_valor
              from aire.gnl_dominios_valor
             where codigo     = 'INST'
               and ind_activo = 'S';


            for c_materiales in (select jt.id_articulo,
                                        jt.cantidad,
                                        jt.series
                                   from json_table(
                                            e_json_orden, '$.material_instalado.materiales[*]'
                                            columns (
                                                id_articulo   number path '$.id_articulo',
                                                cantidad      number path '$.cantidad',
                                                series clob format json path '$.series'
                                            )
                                        ) jt
            ) loop

                --Registramos la serie
                for c_series in ( select jt.id_serie,
                                         jt.serie
                                    from json_table(
                                            c_materiales.series, '$[*]'
                                            columns (
                                                id_serie number path '$.id_serie',
                                                serie    number path '$.serie'
                                            )
                                        ) jt
                ) loop
                    declare
                        v_codigo_articulo       aire.mtr_articulos.codigo_sap%type;
                        v_descripcion_articulo  aire.mtr_articulos.nombre%type;
                        v_id_marca              aire.mtr_series.id_marca%type;
                        v_descripcion_marca     aire.mtr_marcas.descripcion%type;
                        v_nombres_apellidos     varchar2(400);              
                    begin

                        select    b.nombre --as descripcion_articulo
                                , b.codigo_sap --as codigo_articulo
                                , a.id_marca  
                                , c.descripcion --as marca
                            into v_descripcion_articulo 
                                , v_codigo_articulo
                                , v_id_marca
                                , v_descripcion_marca
                        from aire.mtr_series a
                            join aire.mtr_articulos b on a.id_articulo = b.id_articulo
                            join aire.mtr_marcas c on a.id_marca = c.id_marca
                        where id_serie = c_series.id_serie;

                        select b.nombres||' '||b.apellidos into v_nombres_apellidos
                        from aire.ctn_contratistas_persona a 
                            join aire.gnl_personas b on a.id_persona = b.id_persona
                        where a.id_contratista_persona = v_orden_gestion.id_contratista_persona;

                        -- registramos historico
                        v_serie_historico.id_serie                      := c_series.id_serie; -- id de la serie
                        v_serie_historico.serie                         := c_series.serie;    -- serie
                        v_serie_historico.id_articulo                   := c_materiales.id_articulo; -- id del articulo
                        v_serie_historico.codigo_articulo               := v_codigo_articulo; -- codigo del articulo
                        v_serie_historico.descripcion_articulo          := v_descripcion_articulo; -- descripcion del articulo
                        v_serie_historico.id_marca                      := v_id_marca; -- marca de la serie
                        v_serie_historico.descripcion_marca             := v_descripcion_marca; -- descripcion de la marca
                        v_serie_historico.id_movimiento                 := null; -- esto va null
                        v_serie_historico.id_contratista_persona        := v_orden_gestion.id_contratista_persona; -- id del tecnico
                        v_serie_historico.nombre_contratista_persona    := v_nombres_apellidos; -- nombre del tecnico
                        v_serie_historico.decripcion_tipo_movimiento    := 'Instalación';-- instalacion / retiro
                        v_serie_historico.observacion                   := 'Instalación';-- se instala o retira 
                        v_serie_historico.fecha_registro                := sysdate;
                        v_serie_historico.fecha_instalacion             := sysdate;
                        v_serie_historico.id_orden                      := v_orden.id_orden;
                        v_serie_historico.acta                          := v_orden_gestion.acta ;
                        v_serie_historico.numero_orden                  := v_orden.numero_orden;

                        aire.pkg_p_materiales.prc_registrar_serie_historico(
                            e_serie_historico => v_serie_historico,
                            s_respuesta       => v_respuesta 
                        );

                        if v_respuesta.codigo <> 0 then
                            v_id_log	        := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, v_tipo_mensaje_seguimiento, 'No se pudo realizar el proceso de registro de las series: '|| v_respuesta.mensaje);
                            sb_escribir_respuesta(1, 'Error #'||v_id_log||' al gestionar la orden');
                            rollback;
                            return;         
                        end if;

                        v_articulo_operacion.id_serie               := c_series.id_serie;
                        v_articulo_operacion.id_articulo            := c_materiales.id_articulo;
                        v_articulo_operacion.id_tipo_operacion      := v_id_dominio_valor;
                        v_articulo_operacion.cantidad               := c_materiales.cantidad;
                        v_articulo_operacion.id_orden               := v_orden.id_orden;
                        v_articulo_operacion.observacion            := json_value(e_json_orden, '$.material_instalado.observacion');
                        v_articulo_operacion.id_contratista_persona := v_orden_gestion.id_contratista_persona;
                        v_articulo_operacion.fecha                  := sysdate;

                        aire.pkg_p_materiales.prc_registrar_articulos_operacion(
                            e_articulo_operacion => v_articulo_operacion,
                            s_respuesta          => v_respuesta
                        );

                        --Validamos la respuesta del procedimiento
                        if v_respuesta.codigo <> 0 then
                            v_id_log	        := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, v_tipo_mensaje_seguimiento, 'No se pudo realizar el proceso de registro de los materiales instalados: '|| v_respuesta.mensaje);
                            sb_escribir_respuesta(1, 'Error #'||v_id_log||' no se pudo realizar el proceso de registro de los materiales instalados');
                            rollback;
                            return;         
                        end if;

                        update aire.mtr_series set ind_instalado = 'S'
                        where id_serie = c_series.id_serie;

                    end;

                end loop;
            end loop;
        end if;
        --Registramos el cuestionario
        for c_cuestionario in (
            select cuestionario
              from json_table(
                        e_json_orden, '$.cuestionarios[*]'
                        columns ( cuestionario clob format json path '$')
                   ) jt
        ) loop

            aire.pkg_g_generales.prc_registrar_cuestionario_instancia(c_cuestionario.cuestionario, s_json_respuesta);

            --Validamos la respuesta del procedimiento
            if json_value(s_json_respuesta, '$.codigo') <> 0 then
                v_id_log	        := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, v_tipo_mensaje_seguimiento, 'No se pudo realizar el proceso de registro de los cuestionarios: '||s_json_respuesta);
                sb_escribir_respuesta(1, 'Error #'||v_id_log||' No se pudo realizar el proceso de registro de los cuestionarios.');
                rollback;
                return;         
            end if;
        end loop;

        aire.pkg_g_ordenes.prc_cerrar_orden(
            e_id_orden_gestion	=> v_orden_gestion.id_orden_gestion,
            s_respuesta			=> v_respuesta
        );

        --Validamos la respuesta del procedimiento
        if v_respuesta.codigo <> 0 then
            v_id_log	        := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, v_tipo_mensaje_seguimiento, 'No se pudo cerrar la orden: '|| v_respuesta.mensaje);
            sb_escribir_respuesta(1, 'Error #'||v_id_log||' No se pudo cerrar la orden.');
            rollback;
            return;         
        end if;

        v_orden.fecha_cierre        := sysdate;
        v_orden.acta                := v_orden_gestion.acta;
        v_orden.id_usuario_cierre   := json_value(e_json_orden, '$.cuestionarios[0].id_usuario');
        v_orden.id_estado_orden     := v_estado_orden;

        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, v_tipo_mensaje_seguimiento, 'v_orden_gestion.id_orden: '|| v_orden_gestion.id_orden
                                                                                                                ||' - v_orden.fecha_cierre: '||v_orden.fecha_cierre
                                                                                                                ||' - v_orden.id_estado_orden: '||v_orden.id_estado_orden
                                                                                                                ||' - v_orden.acta: '||v_orden.acta
                                                                                                                ||' - v_orden.id_usuario_cierre: '||v_orden.id_usuario_cierre);

        pkg_p_ordenes.prc_actualizar_orden(
            e_orden	    => v_orden,
            s_respuesta	=> v_respuesta
        );

        --Validamos la respuesta del procedimiento
        if v_respuesta.codigo <> 0 then
            v_id_log	        := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, v_tipo_mensaje_seguimiento, 'No se pudo cerrar la orden: '|| v_respuesta.mensaje);
            sb_escribir_respuesta(1, 'Error #'||v_id_log||' No se pudo cerrar la orden.');
            rollback;
            return;         
        end if;

        sb_escribir_respuesta(0, 'Orden gestionada con exito.');

	exception
		when others then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al gestionar orden: '|| sqlerrm);
            sb_escribir_respuesta(1, 'Error #'||v_id_log||' Error al gestionar orden.');
	end prc_registrar_orden_gestion;

    procedure prc_cerrar_orden_osf(
        e_xml_orden         in  clob,
        s_respuesta         out aire.tip_respuesta
    ) is
        -- variables
        v_solicitud             aire.gnl_solicitudes%rowtype;

        v_historico             aire.gnl_solicitudes_historico%rowtype;
        v_respuesta 		    aire.tip_respuesta := aire.tip_respuesta(mensaje => 'Peticion exitosa', nombre_up => 'pkg_g_ordenes.prc_cerrar_orden_osf');
        v_id_log		        aire.gnl_logs.id_log%type;

        v_respuesta_api         clob;
        v_codigo_respuesta_api  number;
		v_mensaje_respuesta_api	varchar2(250);
        v_xml_respuesta         clob;
        v_cuerpo                clob := '{"apiName":"OS_CLOSEORDERBYXML","parameters":[{"parameterName":"iclData","parameterValue":"'||e_xml_orden||'"}]}';
    begin
        -- consultamos la parametrizacion de la solicitud
        begin
            select *
              into v_solicitud
              from aire.gnl_solicitudes 
             where codigo_solicitud = 'DDCLI';

            v_historico.id_solicitud_historico := aire.sec_gnl_solicitudes_historico.nextval;
            v_historico.id_solicitud     := v_solicitud.id_solicitud;
            v_historico.cuerpo_solicitud := v_cuerpo;

            dbms_output.put_line('v_solicitud.codigo_solicitud: '|| v_solicitud.codigo_solicitud);
        exception
            when others then
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, v_tipo_mensaje_error, 'Error al consultar la parametrizacion del consumo de la API: '|| sqlerrm);
                v_respuesta.codigo := 1;
                v_respuesta.mensaje := 'Error No '||v_id_log||' al realizar consulta de la parametrizacion de la API.';
                s_respuesta := v_respuesta;
                return;
        end;

        v_historico.fecha_solicitud := systimestamp;

        -- realizamos la peticion
        v_respuesta_api := apex_web_service.make_rest_request(
              p_url         => v_solicitud.url
            , p_http_method => v_solicitud.metodo
            , p_body        => v_historico.cuerpo_solicitud
        );

        v_historico.fecha_respuesta     := systimestamp;
        v_historico.respuesta_solicitud := v_respuesta_api;  

        -- registramos historico de peticiones
        insert into aire.gnl_solicitudes_historico values v_historico;

        begin
        select parametervalue
          into v_codigo_respuesta_api
          from json_table(
                 v_respuesta_api,
                    '$'
                    columns (
                        nested path '$.parameters[*]'
                        columns (
                            parametername      path '$.parameterName',
                            parametervalue     path '$.parameterValue'
                        )
                    )
               )
         where parametername = 'ONUERRORCODE';

        dbms_output.put_line('v_codigo_respuesta_api: '|| v_codigo_respuesta_api);
        exception
            when others then
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, v_tipo_mensaje_error, 'Error al realizar la peticion, consulte el log de solicitudes con el numero: '||v_historico.id_solicitud_historico||' para mas informacion');
                v_respuesta.codigo := 1;
                v_respuesta.mensaje := 'Error No '||v_id_log|| ' al cerrar la orden';
                s_respuesta := v_respuesta;
                return;                
        end;
        -- validamos el codigo de respuesta de la api
        if v_codigo_respuesta_api <> 0 then
--		
			select parametervalue
			  into v_mensaje_respuesta_api
			  from json_table(
					v_respuesta_api,
						'$'
						columns (
							nested path '$.parameters[*]'
							columns (
								parametername      path '$.parameterName',
								parametervalue     path '$.parameterValue'
							)
						)
				  )
			where parametername = 'OSBERRORMESSAGE';

            dbms_output.put_line('v_mensaje_respuesta_api: '|| v_mensaje_respuesta_api);

            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, v_tipo_mensaje_error, 'Error al realizar la peticion, consulte el log de solicitudes con el numero: '||v_historico.id_solicitud_historico||' para mas informacion');
            v_respuesta.codigo := 1;
            v_respuesta.mensaje := 'Error No '||v_id_log|| ' al cerrar la orden : '||v_mensaje_respuesta_api;
            s_respuesta := v_respuesta;
            return;
        end if;
/*
        -- extraemos el xml de la respuesta
        select parametervalue
          into v_xml_respuesta
          from json_table(
                 v_respuesta_api,
                    '$'
                    columns (
                        nested path '$.parameters[*]'
                        columns (
                            parametername      path '$.parameterName',
                            parametervalue     path '$.parameterValue'
                        )
                    )
               )
         where parametername = 'OCLRESPONSE';
*/       
        s_respuesta:= v_respuesta;
    exception
        when others then
            v_id_log            := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, v_tipo_mensaje_error, 'Error al realizar la peticion: '||sqlerrm);
            v_respuesta.codigo  := 1;
            v_respuesta.mensaje := 'Error No '||v_id_log|| ' al realizar la peticion';
            s_respuesta := v_respuesta;
    end prc_cerrar_orden_osf;

    procedure prc_cerrar_orden(
        e_id_orden_gestion	in 	aire.ord_ordenes_gestion.id_orden_gestion%type,
        s_respuesta			out	aire.tip_respuesta
    ) is
        v_xml_orden_type    xmltype;
        v_xml_orden_clob    clob;
        v_serie             varchar2(250);
        v_nis               aire.gnl_clientes.nis%type;
        v_id_log			aire.gnl_logs.id_log%type;
        v_respuesta			aire.tip_respuesta := aire.tip_respuesta(mensaje => 'Orden cerrada con exito.', nombre_up => 'pkg_g_ordenes.prc_cerrar_orden');
        v_id_persona_op360  aire.gnl_dominios_valor.valor%type;
    begin
        -- consultamos el identificador de op360 en OSF
        v_id_persona_op360 := aire.pkg_p_generales.fnc_consultar_dominio_valor(e_codigo_dominio => 'PERID',e_codigo_dominio_valor => 'PERID');

        begin
                select xmlelement(
                            "ORDERINFO",
                            xmlforest(
                                b.numero_orden as order_id,
                                f.codigo_anomalia as causal_id,
                                v_id_persona_op360 as person_id,                                
                                to_char(a.fecha_inicio_ejecucion, 'YYYY-MM-DD"T"HH24:MI:SS') as init_exec_date,
                                to_char(a.fecha_fin_ejecucion, 'YYYY-MM-DD"T"HH24:MI:SS') as fin_exec_date,
                                (
                                    select
                                        xmlagg(
                                            xmlelement(
                                                "ACTIVITY",
                                                xmlforest(
                                                    b.actividad_orden as order_activity,
                                                    decode(g.ind_ejecuta_actividad, 'S', 1,0) as executed,
                                                    case when g.ind_ejecuta_actividad = 'S' then
                                                    (
                                                        select
                                                            xmlconcat(
                                                                xmlelement(
                                                                    "attribute",
                                                                    xmlforest(
                                                                        'LECTURA_AIRE' as name,
                                                                        '#SERIE_MED#' as value,
                                                                        xmlelement(
                                                                            "READINGS",
                                                                            (
                                                                                select
                                                                                    xmlagg(
                                                                                        xmlelement(
                                                                                            "READING",
                                                                                            xmlforest(
                                                                                                '#SERIE_MED#' as med_serie,
                                                                                                case when d.codigo_tipo_orden in ('TO502') then
                                                                                                    nvl(a.lectura,0)
                                                                                                end as reading_value,
                                                                                                case when d.codigo_tipo_orden in ('TO501', 'TO504') then
                                                                                                    103
                                                                                                end as usage_type,
                                                                                                'T' as cause,
                                                                                                99 as observation1
                                                                                            )
                                                                                        )
                                                                                    )
                                                                                from dual
                                                                            )
                                                                        ) as component
                                                                    )
                                                                ),
                                                                case when b.id_tipo_suspencion is not null and d.codigo_tipo_orden in ('TO501', 'TO504') then
                                                                    xmlelement(
                                                                        "ATTRIBUTE",
                                                                        xmlforest(
                                                                            'TYPES_DISCONNECTION' as name,
                                                                            e.codigo as value
                                                                        )
                                                                    )
                                                                end
                                                            )
                                                        from dual
                                                    ) end as attributes
                                                )
                                            )
                                        )
                                    from dual
                                ) as activities,
                                (
                                    select
                                        xmlagg(
                                            xmlelement(
                                                "COMMENT",
                                                xmlforest(
                                                    4 as type_comment,
                                                    'Prueba cierre orden OP360 '||d.codigo_tipo_orden as order_comment
                                                ) as attributes
                                            )
                                        )
                                    from dual
                                ) as comments
                            )
                        ).getclobval() as xml
                       , c.nis 
                  into v_xml_orden_clob
                     , v_nis
                  from aire.ord_ordenes_gestion       a 
                  join aire.ord_ordenes               b on a.id_orden           = b.id_orden
                  join aire.gnl_clientes              c on b.id_cliente         = c.id_cliente
                  join aire.ord_tipos_orden           d on b.id_tipo_orden      = d.id_tipo_orden   
                  left join aire.scr_tipos_suspencion e on b.id_tipo_suspencion = e.id_tipo_suspencion
                  join aire.ord_anomalias             f on a.id_anomalia        = f.id_anomalia
                  join aire.ord_resultados            g on a.id_resultado       = g.id_resultado
                where a.id_orden_gestion = e_id_orden_gestion;
        exception 
            when others then
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, v_tipo_mensaje_error, 'Error al construir el XML: '||sqlerrm);
                v_respuesta.codigo  := 1;
                v_respuesta.mensaje := 'Error No '||v_id_log||' al cerrar la orden.';
                s_respuesta         := v_respuesta;
                return;
        end;
        -- consultamos la serie del medidor
        aire.pkg_g_generales.prc_consultar_serie_medidor(v_nis, v_serie, s_respuesta);

        if s_respuesta.codigo <> 0 then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, v_tipo_mensaje_error, 'Error al consultar la serie del medidor: '||s_respuesta.mensaje);
            v_respuesta.codigo  := 1;
            v_respuesta.mensaje := 'Error No '||v_id_log||' al cerrar la orden.';
            s_respuesta         := v_respuesta;
            return;
        end if;

        -- si el xml tiene saltos de linea lo volvemos lineal
        with xml_data as (select xmltype(v_xml_orden_clob) as xml_val from dual) select xmlserialize(document xml_val as clob no indent) into v_xml_orden_clob from xml_data;

        -- reemplazamos la serie del medidor
        v_xml_orden_clob := replace(v_xml_orden_clob,'#SERIE_MED#', v_serie);
        dbms_output.put_line(v_serie);
        dbms_output.put_line(v_xml_orden_clob);

        -- cerramos la orden en osf
        aire.pkg_g_ordenes.prc_cerrar_orden_osf(
            e_xml_orden         => v_xml_orden_clob, 
            s_respuesta         => s_respuesta
        );

        if s_respuesta.codigo <> 0 then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, v_tipo_mensaje_error, 'Error al legalizar la orden con OSF: '||s_respuesta.mensaje);
            v_respuesta.codigo  := 1;
            v_respuesta.mensaje := 'Error No '||v_id_log||' al cerrar la orden.';
            s_respuesta         := v_respuesta;
            return;
        end if;   

        update aire.ord_ordenes_gestion
           set ind_procesado_ws = 'S'
         where id_orden_gestion = e_id_orden_gestion;

        s_respuesta := v_respuesta;

    exception
        when others then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, v_tipo_mensaje_error, 'Error al cerar la orden: '||sqlerrm);
            v_respuesta.codigo  := 1;
            v_respuesta.mensaje := 'Error No '||v_id_log||' al cerrar la orden.';
            s_respuesta         := v_respuesta;
            return;	
    end prc_cerrar_orden;
    
    --//-->--------------------------- INICIO PKG_G_CARLOS_VARGAS_TEST Procedimientos Carlos Vargas
       
    --//--Migrado a pkg_g_ordenes 31/01/2024
    procedure prc_parametros_iniciales_areacentral (		
        s_json 	out	clob
	) is
		--v_json              clob;
        v_id_actividad  NUMBER(3,0);
        v_id_log		aire.gnl_logs.id_log%type;
        v_respuesta		aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Carga realizada con exito.', nombre_up   => 'pkg_g_carlos_vargas_test.prc_parametros_iniciales_areacentral');
    BEGIN
        -- consultamos el id de la actividad de SCR
        begin
            select id_actividad
              into v_id_actividad
              from aire.gnl_actividades
             where prefijo = 'G_SCR';
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
        
        apex_json.open_object();        
            apex_json.write('codigo', 0);
            apex_json.write('mensaje', 'Carga exitosa');
            apex_json.open_object('datos');
            -- Tipos de orden
                apex_json.open_array('tipos_orden');
                    for c_datos in (
                        select id_tipo_orden
                             , codigo_tipo_orden
                             , descripcion
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
                           and ind_activo = 'S'
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
                        select id_contratista
                             , id_persona
                             , identificacion
                             , nombre_completo
                             , email
                             , ind_activo
                             , descripcion_ind_activo
                          from aire.v_ctn_contratistas  
                          where id_contratista in (
                                select id_contratista
                                  from aire.v_ctn_contratos 
                                 where prefijo_actividad = 'G_SCR'
                                   and ind_activo        = 'S' 
                           )
                    ) loop
                        apex_json.open_object();
                        apex_json.write('id_contratista', c_datos.id_contratista);
                        apex_json.write('id_persona', c_datos.id_persona);
                        apex_json.write('identificacion', c_datos.identificacion);
                        apex_json.write('nombre_completo', c_datos.nombre_completo);
                        apex_json.write('email', c_datos.email);
                        apex_json.write('ind_activo', c_datos.ind_activo);
                        apex_json.write('descripcion_ind_activo', c_datos.descripcion_ind_activo);
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
                
            apex_json.close_object();            
        apex_json.close_object();
        
        ---v_json := apex_json.get_clob_output;
        s_json := apex_json.get_clob_output;
        apex_json.free_output;         
        --s_json := v_json;
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Carga exitosa');        
        dbms_output.put_line(s_json);
    exception
        when others then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al crear el json: '|| sqlerrm);
            apex_json.free_output; 
            apex_json.open_object();
            apex_json.write('codigo', 1);
            apex_json.write('mensaje', 'Error N '||v_id_log ||' al crear los parametros iniciales');            
            apex_json.close_object();
            --v_json := apex_json.get_clob_output;
            s_json := apex_json.get_clob_output;
            apex_json.free_output;
            --s_json := v_json;
    END prc_parametros_iniciales_areacentral;
    
    --//--Migrado a pkg_g_ordenes 31/01/2024
    procedure prc_consultar_cliente_por_nic_nis (		
        e_json  in  clob,
        s_json 	out	clob
    ) is
        --e_json  	clob :=  '{"nic": "2076341","nis": ""}';
        --v_json      clob;
        v_json_cliente    json_object_t; 
        v_id_log		aire.gnl_logs.id_log%type;
        v_respuesta		aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_carlos_vargas_test.prc_consultar_cliente_por_nic_nis');
        v_cliente      	aire.gnl_clientes%rowtype;
        BEGIN
        
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
        apex_json.free_output; 
        apex_json.initialize_clob_output( p_preserve => false );
        
        -- validamos el Json de la orden y armamos el rowtype
        begin
            -- parseamos el json
            v_json_cliente := json_object_t(e_json);
        
            -- extraemos los datos de la orden
            v_cliente.nic           := v_json_cliente.get_string('nic');
            v_cliente.nis           := v_json_cliente.get_string('nis');
        exception 
            when others then 
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al procesar el json de la orden: '|| sqlerrm);
                apex_json.initialize_clob_output( p_preserve => false );
                apex_json.open_object();
                apex_json.write('codigo', 1);
                apex_json.write('mensaje', 'Error #'||v_id_log||' al registrar orden');
                apex_json.close_object();
                s_json := apex_json.get_clob_output;
                apex_json.free_output;
                return;
        end; 
        
        apex_json.open_object();        
            apex_json.write('codigo', 0);
            apex_json.write('mensaje', 'Consulta exitosa');
            apex_json.open_object('datos');
            -- Tipos de orden
                apex_json.open_array('clientes');
                    for c_datos in (
                        select c.id_cliente
                             , c.id_territorial
                             , t.nombre as nombre_territorial
                             , c.id_zona
                             , z.nombre as nombre_zona
                             , c.id_departamento
                             , d.nombre as nombre_departamento
                             , c.id_municipio
                             , m.nombre as nombre_municipio
                             , c.direccion
                             , c.georreferencia
                             , c.nic
                             , c.nis
                             , c.nombre_cliente
                        from aire.gnl_clientes c
                        left join aire.gnl_territoriales t on t.id_territorial = c.id_territorial
                        left join aire.gnl_zonas z on z.id_zona = c.id_zona
                        left join aire.gnl_departamentos d on d.id_departamento = c.id_departamento
                        left join aire.gnl_municipios m on m.id_municipio = c.id_municipio
                         -- where rownum <= 5
                        where nic = v_cliente.nic
                            or nis = v_cliente.nis
                    ) loop
                        apex_json.open_object();
                        apex_json.write('id_cliente', c_datos.id_cliente);
                        apex_json.write('id_territorial', c_datos.id_territorial);
                        apex_json.write('nombre_territorial', c_datos.nombre_territorial);
                        apex_json.write('id_zona',c_datos.id_zona);
                        apex_json.write('nombre_zona',c_datos.nombre_zona);
                        apex_json.write('id_departamento',c_datos.id_departamento);
                        apex_json.write('nombre_departamento',c_datos.nombre_departamento);
                        apex_json.write('id_municipio',c_datos.id_municipio);
                        apex_json.write('nombre_municipio',c_datos.nombre_municipio);
                        apex_json.write('direccion',c_datos.direccion);
                        apex_json.write('georreferencia',c_datos.georreferencia);
                        apex_json.write('nic',c_datos.nic);
                        apex_json.write('nis',c_datos.nis);
                        apex_json.write('nombre_cliente',c_datos.nombre_cliente);
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
        apex_json.write('mensaje', 'Error N '||v_id_log ||' al consultar el cliente.');            
        apex_json.close_object();
        --v_json := apex_json.get_clob_output;
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
        --s_json := v_json;
    
    END prc_consultar_cliente_por_nic_nis;    
    
    --//--Migrado a pkg_g_ordenes 31/01/2024
    procedure prc_consulta_agrupada_ordenes_area_central (		
        e_json  in  clob,
        s_json 	out	clob
    ) is
        --e_json  	clob :=  '{"id_contratista": "2076341"}';        
        v_json_objeto    json_object_t; 
        v_id_log		aire.gnl_logs.id_log%type;
        v_respuesta		aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_carlos_vargas_test.prc_consulta_agrupada_ordenes_area_central');
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
                    where NVL(a.id_contratista,-1) in (
                                select id_contratista
                                      from aire.v_ctn_contratos 
                                     where prefijo_actividad = 'G_SCR'
                                       and ind_activo        = 'S'       
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
                                     where prefijo_actividad = 'G_SCR'
                                       and ind_activo        = 'S'       
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
      
    --//--Migrado a pkg_g_ordenes 31/01/2024
    procedure prc_gestionar_orden_asignar_contratista (		
        e_json  in  clob,
        s_json 	out	clob
    ) is
        --e_json  	clob :=  '{"id_contratista":110,"id_ordenes":"1,150,220","contratista_asignar":"S"}';
        v_json_objeto           json_object_t; 
        v_id_log		        aire.gnl_logs.id_log%type;
        v_respuesta		        aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_carlos_vargas_test.prc_gestionar_orden_asignar_contratista');        
        
        v_identificacion        varchar2(50 BYTE);
        v_id_contratista        varchar2(100 BYTE);
        v_id_orden_string       varchar2(4000 byte);
        asignar_contratista     varchar2(1 BYTE); 
        v_id_estado_orden       number;
        
        -- Declarar la tabla temporal
        l_num_list              num_list_type := num_list_type();        
        v_cnt_actualizar        number;
        v_cnt_actualizar2        number;
        v_cnt_contra            number;
    BEGIN  
    --LOG
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
    
    -- Deserializar JSON
    apex_json.free_output; 
    apex_json.initialize_clob_output( p_preserve => false );
    
    v_json_objeto := json_object_t(e_json);    
    v_identificacion         := v_json_objeto.get_string('identificacion');    
    v_id_orden_string        := v_json_objeto.get_string('id_orden_string');    
    asignar_contratista    := v_json_objeto.get_string('contratista_asignar');    
    
    apex_json.free_output;    
    
    
    SELECT
        count(*) into v_cnt_contra
    FROM aire.v_ctn_contratistas
    where identificacion = v_identificacion;
    
    --Si no se encuentra registros, devolver el error
    IF (v_cnt_contra = 0) THEN        
        apex_json.open_object();
            apex_json.write('codigo', 50001);
            apex_json.write('mensaje', 'No se encontro el contratista');            
        apex_json.close_object();
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
        return;
    ELSE
        SELECT
            nvl(id_contratista,'-1') into v_id_contratista
        FROM aire.v_ctn_contratistas
        where identificacion = v_identificacion;
    END IF;
        
    --Validar, si no llega ningun registro retornar provocar error.
    SELECT
       TO_NUMBER(REGEXP_SUBSTR(v_id_orden_string, '[^,]+', 1, level))
       BULK COLLECT INTO l_num_list
    FROM dual
    CONNECT BY REGEXP_SUBSTR(v_id_orden_string, '[^,]+', 1, level) IS NOT NULL;
    --//----INICIO validar si hay ordenes en estado diferente a pendiente
    
    select count(*) into v_cnt_actualizar2 
    from aire.ord_ordenes 
    where       id_orden in (select COLUMN_VALUE from TABLE(l_num_list))
            and id_contratista is null
            and id_estado_orden not in (
                        select id_estado_orden 
                        from aire.ord_estados_orden 
                        where   codigo_estado in ('SPEN') 
                            and id_actividad IN (select id_actividad from aire.gnl_actividades where nombre = 'SCR' and ind_activo = 'S')
                        );
    
    IF (v_cnt_actualizar2 > 0) THEN                
        --Si todo sale bien devolver mensaje.
        apex_json.open_object();
            apex_json.write('codigo', 50002);
            apex_json.write('mensaje', 'Se encontraron ordenes con estado diferente a 25-SPEN-Pendiente');            
        apex_json.close_object();
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
        return;
    END IF;
    --//----FIN validar si hay ordenes en estado diferente a pendiente
    
    --//----INICIO validar si hay ordenes en estado pendiente
    select count(*) into v_cnt_actualizar 
    from aire.ord_ordenes 
    where       id_orden in (select COLUMN_VALUE from TABLE(l_num_list))
            and id_contratista is null
            and id_estado_orden in (
                        select id_estado_orden 
                        from aire.ord_estados_orden 
                        where   codigo_estado in ('SPEN') 
                            and id_actividad IN (select id_actividad from aire.gnl_actividades where nombre = 'SCR' and ind_activo = 'S')
                        );
                        
    IF (v_cnt_actualizar > 0) THEN        
        select id_estado_orden into v_id_estado_orden
        from aire.ord_estados_orden 
        where   codigo_estado in ('SASI') 
            and id_actividad IN (select id_actividad from aire.gnl_actividades where nombre = 'SCR' and ind_activo = 'S');
        
        update aire.ord_ordenes
        set id_contratista = v_id_contratista,id_estado_orden = v_id_estado_orden
        where   id_orden in (select COLUMN_VALUE from TABLE(l_num_list))
            and id_contratista is null
            and id_estado_orden in (
                select id_estado_orden 
                from aire.ord_estados_orden 
                where   codigo_estado in ('SPEN') 
                    and id_actividad IN (select id_actividad from aire.gnl_actividades where nombre = 'SCR' and ind_activo = 'S')
                );
                
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
            apex_json.write('codigo', 50003);
            apex_json.write('mensaje', 'No se actualizo ninguna orden');            
        apex_json.close_object();
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
        return;
    END IF;
    --//----FIN validar si hay ordenes en estado pendiente
    
    
    --LOG
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Carga exitosa');        
    --dbms_output.put_line(s_json);
    exception
    when others then
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al crear el json: '|| sqlerrm);
        apex_json.free_output; 
        apex_json.open_object();
        apex_json.write('codigo', 50000);
        apex_json.write('mensaje', 'Error N '||v_id_log ||' al consultar el listado de ordenes.' || sqlerrm);            
        apex_json.close_object();
        --v_json := apex_json.get_clob_output;
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
        --s_json := v_json;
        --dbms_output.put_line('xxx: '|| sqlerrm);
    END prc_gestionar_orden_asignar_contratista;
      
    --//--Migrado a pkg_g_ordenes 31/01/2024
    procedure prc_parametros_iniciales_contratistas (		
        e_json 	in	clob,
        s_json 	out	clob
    ) is
        --v_json              clob;
        v_json_objeto           json_object_t; 
        v_id_contratista    VARCHAR2(100 BYTE);
        v_identificacion    VARCHAR2(50 BYTE);
        v_id_log		aire.gnl_logs.id_log%type;
        v_respuesta		aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Carga realizada con exito.', nombre_up   => 'pkg_g_carlos_vargas_test.prc_parametros_iniciales_contratistas');
    BEGIN
        --LOG
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
        
        -- Deserializar JSON
        apex_json.free_output; 
        apex_json.initialize_clob_output( p_preserve => false );
        
        v_json_objeto           := json_object_t(e_json);    
        v_identificacion        := v_json_objeto.get_string('identificacion');        
        apex_json.free_output;    
        
        -- consultamos el id de la actividad de SCR
        SELECT
            nvl(id_contratista,'-1') into v_id_contratista
        FROM aire.v_ctn_contratistas
        where identificacion = v_identificacion;
        dbms_output.put_line('v_id_contratista: ' || v_id_contratista);
        
        apex_json.open_object();        
            apex_json.write('codigo', 0);
            apex_json.write('mensaje', 'Carga exitosa');
            apex_json.open_object('datos');
            -- Tipos de orden                
                    for c_datos in (
                        SELECT distinct
                             cp.id_contratista
                            ,cp.identificacion_contratista
                            ,cp.nombre_contratista
                        FROM aire.v_ctn_contratistas_persona cp
                        where cp.ind_activo = 'S' and cp.id_contratista = v_id_contratista
                        order by cp.id_contratista
                    ) loop
                       
                        apex_json.write('id_contratista', c_datos.id_contratista);
                        apex_json.write('identificacion_contratista', c_datos.identificacion_contratista);
                        apex_json.write('nombre_contratista',c_datos.nombre_contratista);
                        apex_json.open_array('Brigadas');
                            -- Brigadas
                             for x_datos in (
                                SELECT
                                     cp.id_contratista_persona
                                    ,cp.identificacion_contratista_persona
                                    ,cp.nombre_contratista_persona        
                                FROM aire.v_ctn_contratistas_persona cp
                                where cp.ind_activo = 'S' and cp.id_contratista = c_datos.id_contratista
                                order by cp.id_contratista_persona
                            ) loop
                            apex_json.open_object();
                                apex_json.write('id_contratista_persona', x_datos.id_contratista_persona);
                                apex_json.write('identificacion_contratista_persona', x_datos.identificacion_contratista_persona);
                                apex_json.write('nombre_contratista_persona', x_datos.nombre_contratista_persona);
                            apex_json.close_object();
                            end loop;                                
                        apex_json.close_array();                        
                    end loop;
                    -- gnl_zonas
                    apex_json.open_array('Zonas');
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
                            where a.id_contratista = v_id_contratista                            
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
            apex_json.close_object();            
        apex_json.close_object();
        
        ---v_json := apex_json.get_clob_output;
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
            apex_json.write('codigo', 50000);
            apex_json.write('mensaje', 'Error N '||v_id_log ||' al crear los parametros iniciales');            
            apex_json.close_object();
            --v_json := apex_json.get_clob_output;
            s_json := apex_json.get_clob_output;
            apex_json.free_output;
            --s_json := v_json;
    END prc_parametros_iniciales_contratistas;
      
    --//--Migrado a pkg_g_ordenes 31/01/2024
    procedure prc_gestionar_orden_asignar_brigada (		
        e_json  in  clob,
        s_json 	out	clob
    ) is
        --e_json  	clob :=  '{"id_contratista":110,"id_ordenes":"1,150,220","contratista_asignar":"S"}';
        v_json_objeto           json_object_t; 
        v_id_log		        aire.gnl_logs.id_log%type;
        v_respuesta		        aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_carlos_vargas_test.prc_gestionar_orden_asignar_brigada');        
        
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
                            and id_actividad IN (select id_actividad from aire.gnl_actividades where nombre = 'SCR' and ind_activo = 'S')
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
                            and id_actividad IN (select id_actividad from aire.gnl_actividades where nombre = 'SCR' and ind_activo = 'S')
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
            and id_actividad IN (select id_actividad from aire.gnl_actividades where nombre = 'SCR' and ind_activo = 'S');
    
        update aire.ord_ordenes
        set id_contratista_persona = v_id_brigada, id_estado_orden = v_id_estado_orden
        where   id_orden in (select COLUMN_VALUE from TABLE(l_num_list))
            and id_contratista = v_id_contratista
            and id_contratista_persona is null
            and id_estado_orden in (
                select id_estado_orden 
                from aire.ord_estados_orden 
                where   codigo_estado in ('SASI') 
                    and id_actividad IN (select id_actividad from aire.gnl_actividades where nombre = 'SCR' and ind_activo = 'S')
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
    
    
    --//-->--------------------------- FIN PKG_G_CARLOS_VARGAS_TEST Procedimientos Carlos Vargas
    
    --//-->--------------------------- INICIO PKG_G_CARLOS_VARGAS_TEST2 Procedimientos Carlos Vargas
    
    --//--Migrado a pkg_g_ordenes 31/01/2024
    procedure prc_consultar_ordenes_area_central (		
        e_json  in  clob,
        s_json 	out	clob
	) is
		--e_json  	clob :=  '{"id_contratista": "2076341","id_zona": ""}';
        --v_json      clob;
        v_json_objeto       json_object_t; 
        v_id_log		    aire.gnl_logs.id_log%type;
        v_respuesta		    aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_carlos_vargas_test.prc_consultar_ordenes_area_central');
        v_objeto      	    aire.ord_ordenes%rowtype;
        v_id_orden          NUMBER;
        pageNumber          NUMBER; 
        pageSize            NUMBER;
        sortColumn          VARCHAR(100 BYTE);
        sortDirection       VARCHAR(100 BYTE);  
        v_RegistrosTotales  NUMBER;
    BEGIN
    
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
    apex_json.free_output; 
    apex_json.initialize_clob_output( p_preserve => false );   
    
    -- parseamos el json
    v_json_objeto := json_object_t(e_json);
    
    -- extraemos los datos de la orden
    v_objeto.id_contratista         := v_json_objeto.get_string('id_contratista');
    v_objeto.id_zona                := v_json_objeto.get_string('id_zona'); 
    v_objeto.id_estado_orden        := v_json_objeto.get_string('id_estado_orden'); 
    v_id_orden                      := v_json_objeto.get_number('id_orden');
    
    pageNumber                      := v_json_objeto.get_number('pageNumber');
    pageSize                        := v_json_objeto.get_number('pageSize');
    --sortColumn              := v_json_objeto.get_string('sortColumn');
    --sortDirection              := v_json_objeto.get_string('sortDirection');
        
    apex_json.open_object();        
        apex_json.write('codigo', 0);
        apex_json.write('mensaje', 'Consulta exitosa');
        apex_json.open_object('datos');
         -- ordenes
            apex_json.open_array('ordenes');
                for c_datos in (
                    with tx as (
                        select a.id_orden
                             , a.id_tipo_orden
                             , h.descripcion as tipo_orden
                             , a.numero_orden
                             , a.id_estado_orden 
                             , b.descripcion  as estado_orden
                             , NVL(a.id_contratista,-1) id_contratista
                             , NVL(c.nombre_completo,'-') as contratista
                             , a.id_cliente
                             , d.nombre_cliente as cliente
                             , a.id_territorial
                             , e.nombre as territorial
                             , NVL(a.id_zona,-1) id_zona
                             , NVL(f.nombre,'-') as zona
                             , a.direcion
                             , a.fecha_creacion
                             , NVL(a.fecha_cierre,TO_TIMESTAMP('1900/01/01 12:01:01,000000000 AM', 'YYYY/MM/DD HH:MI:SS,FF9 AM')) as fecha_cierre
                             , NVL(a.id_usuario_cierre,-1) as id_usuario_cierre
                             , NVL(g.nombres,'-') as usuario_cierre
                             , a.id_actividad
                             , i.nombre as actividad
                             , a.id_contratista_persona
                             , j.nombre_contratista_persona
                             , NVL(a.id_tipo_trabajo,-1) as id_tipo_trabajo
                             , NVL(k.descripcion,'-') as tipo_trabajo
                             , NVL(a.id_tipo_suspencion,-1) as id_tipo_suspencion 
                             , NVL(l.descripcion,'-') as tipo_suspencion
                             , SUM(1) OVER() RegistrosTotales
                        from aire.ord_ordenes              				a
                        left join aire.ord_estados_orden   				b on a.id_estado_orden   		= b.id_estado_orden
                        left join aire.v_ctn_contratistas  				c on a.id_contratista    		= c.id_contratista
                        left join aire.gnl_clientes        				d on a.id_cliente        		= d.id_cliente
                        left join aire.gnl_territoriales   				e on a.id_territorial    		= e.id_territorial
                        left join aire.gnl_zonas           				f on a.id_zona           		= f.id_zona
                        left join aire.v_sgd_usuarios      				g on a.id_usuario_cierre 		= g.id_usuario
                        left join aire.ord_tipos_orden     				h on a.id_tipo_orden     		= h.id_tipo_orden
                        left join aire.gnl_actividades     				i on a.id_actividad      		= i.id_actividad
                        left join aire.v_ctn_contratistas_persona  		j on a.id_contratista_persona 	= j.id_contratista_persona
                        left join aire.ord_tipos_trabajo 				k on a.id_tipo_trabajo 		 	= k.id_tipo_trabajo
                        left join aire.scr_tipos_suspencion 			l on a.id_tipo_suspencion 	 	= l.id_tipo_suspencion
                        where 
                            NVL(a.id_contratista,-1) in (
                                select id_contratista
                                      from aire.v_ctn_contratos 
                                     where prefijo_actividad = 'G_SCR'
                                       and ind_activo        = 'S'       
                                union all
                                select -1 from dual
                            ) and
                            case 
                                when v_id_orden = -1 and a.id_orden >= 0 then 1
                                when v_id_orden = -2 and a.id_orden >= 0 then 1
                                when v_id_orden > 0 and a.id_orden = v_id_orden then 1
                                else 0
                            end = 1 and
                            case 
                                when v_id_orden > 0 then 1
                                when v_objeto.id_contratista = -1 and a.id_contratista is null then 1
                                when v_objeto.id_contratista = -2 and (a.id_contratista is not null or a.id_contratista is null) then 1
                                when v_objeto.id_contratista > 0 and a.id_contratista = v_objeto.id_contratista then 1                            
                                else 0
                            end = 1 and
                            case 
                                when v_id_orden > 0 then 1
                                when v_objeto.id_zona = -1 and a.id_zona is null then 1
                                when v_objeto.id_zona = -2 and (a.id_zona is not null or a.id_zona is null) then 1
                                when v_objeto.id_zona > 0 and a.id_zona = v_objeto.id_zona then 1                            
                                else 0
                            end = 1 and
                            case 
                                when v_id_orden > 0 then 1
                                when v_objeto.id_estado_orden = -1 and a.id_estado_orden is null then 1
                                when v_objeto.id_estado_orden = -2 and (a.id_estado_orden is not null or a.id_estado_orden is null) then 1
                                when v_objeto.id_estado_orden > 0 and a.id_estado_orden = v_objeto.id_estado_orden then 1                            
                                else 0
                            end = 1 
                    )
                    select 
                        a.*
                    from tx a                   
                    OFFSET pageNumber ROWS FETCH NEXT pageSize ROWS ONLY
                ) loop
                apex_json.open_object();
                    apex_json.write('id_orden', c_datos.id_orden);
                    apex_json.write('id_tipo_orden', c_datos.id_tipo_orden);
                    apex_json.write('tipo_orden', c_datos.tipo_orden);
                    apex_json.write('numero_orden', c_datos.numero_orden);
                    apex_json.write('id_estado_orden', c_datos.id_estado_orden);
                    apex_json.write('estado_orden', c_datos.estado_orden);
                    apex_json.write('id_contratista', c_datos.id_contratista);
                    apex_json.write('contratista', c_datos.contratista);
                    apex_json.write('id_cliente', c_datos.id_cliente);
                    apex_json.write('cliente', c_datos.cliente);
                    apex_json.write('id_territorial', c_datos.id_territorial);
                    apex_json.write('territorial', c_datos.territorial);
                    apex_json.write('id_zona', c_datos.id_zona);
                    apex_json.write('zona', c_datos.zona);
                    apex_json.write('direcion', c_datos.direcion);
                    apex_json.write('fecha_creacion', c_datos.fecha_creacion);
                    apex_json.write('fecha_cierre', c_datos.fecha_cierre);
                    apex_json.write('id_usuario_cierre', c_datos.id_usuario_cierre);
                    apex_json.write('usuario_cierre', c_datos.usuario_cierre);
                    apex_json.write('id_actividad', c_datos.id_actividad);
                    apex_json.write('actividad', c_datos.actividad);
                    apex_json.write('id_contratista_persona', c_datos.id_contratista_persona);
                    apex_json.write('nombre_contratista_persona', c_datos.nombre_contratista_persona);
                    apex_json.write('id_tipo_trabajo', c_datos.id_tipo_trabajo);
                    apex_json.write('tipo_trabajo', c_datos.tipo_trabajo);
                    apex_json.write('id_tipo_suspencion', c_datos.id_tipo_suspencion);
                    apex_json.write('tipo_suspencion', c_datos.tipo_suspencion);
                    v_RegistrosTotales := c_datos.RegistrosTotales;
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
                                     where prefijo_actividad = 'G_SCR'
                                       and ind_activo        = 'S'       
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
      
    --//--Migrado a pkg_g_ordenes 31/01/2024
    procedure prc_consultar_archivos_instancia (		
        e_json  in  clob,
        s_json 	out	clob
    ) is
        --e_json  	clob :=  '{"codigo":"ASBO"}';
        v_json_objeto       json_object_t; 
        v_id_log		    aire.gnl_logs.id_log%type;
        v_respuesta		    aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_carlos_vargas_test.prc_consultar_archivos_instancia');
        v_codigo            varchar2(100 BYTE);
        v_rutaweb           VARCHAR2(255 BYTE);
    BEGIN  
    --LOG
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
    
    SELECT
        ruta_web INTO v_rutaweb
    FROM aire.gnl_rutas_archivo_servidor
    WHERE id_ruta_archivo_servidor = 5;
    
    -- Deserializar JSON
    apex_json.free_output; 
    apex_json.initialize_clob_output( p_preserve => false );
    v_json_objeto := json_object_t(e_json);    
    v_codigo         := v_json_objeto.get_string('codigo');    
    
    apex_json.open_object();        
        apex_json.write('codigo', 0);
        apex_json.write('mensaje', 'Consulta exitosa');
        apex_json.open_object('datos');
         -- gnl_archivos_instancia
            apex_json.open_array('archivos_instancia');
                for c_datos in (
                    SELECT
                          a.id_archivo_instancia
                        , a.id_archivo
                        , a.nombre_archivo
                        , a.numero_registros_archivo
                        , a.numero_registros_procesados
                        , a.numero_errores
                        , a.fecha_inicio_cargue
                        , a.fecha_fin_cargue
                        , a.duracion
                        , a.id_usuario_registro
                        , a.fecha_registro
                        , a.id_estado_intancia
                        , a.observaciones
                        , v_rutaweb || a.id_soporte as pathwebdescarga
                    FROM  aire.gnl_archivos_instancia a
                    join aire.gnl_archivos b on b.id_archivo = a.id_archivo                    
                    where b.codigo = v_codigo
                ) loop
                apex_json.open_object();
                    apex_json.write('id_archivo_instancia', c_datos.id_archivo_instancia);                  
                    apex_json.write('id_archivo', c_datos.id_archivo);
                    apex_json.write('nombre_archivo', c_datos.nombre_archivo);
                    apex_json.write('numero_registros_archivo', c_datos.numero_registros_archivo);
                    apex_json.write('numero_registros_procesados', c_datos.numero_registros_procesados);
                    apex_json.write('numero_errores', c_datos.numero_errores);
                    apex_json.write('fecha_inicio_cargue', c_datos.fecha_inicio_cargue);
                    apex_json.write('fecha_fin_cargue', c_datos.fecha_fin_cargue);
                    apex_json.write('duracion', c_datos.duracion);
                    apex_json.write('id_usuario_registro', c_datos.id_usuario_registro);
                    apex_json.write('fecha_registro', c_datos.fecha_registro);
                    apex_json.write('id_estado_intancia', c_datos.id_estado_intancia);
                    apex_json.write('observaciones', c_datos.observaciones);
                    apex_json.write('pathwebdescarga', c_datos.pathwebdescarga);
                apex_json.close_object();
                end loop;
            apex_json.close_array();            
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
    END prc_consultar_archivos_instancia;
  
    --//--Migrado a pkg_g_ordenes 31/01/2024
    procedure prc_consultar_archivos_instancia_detalle (		
        e_json  in  clob,
        s_json 	out	clob
    ) is
        --e_json  	clob :=  '{"id_archivo_instancia":0}';
        v_json_objeto           json_object_t; 
        v_id_log		        aire.gnl_logs.id_log%type;
        v_respuesta		        aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_carlos_vargas_test.prc_consultar_archivos_instancia_detalle');
        v_id_archivo_instancia    number;
    BEGIN  
    --LOG
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
    
    -- Deserializar JSON
    apex_json.free_output; 
    apex_json.initialize_clob_output( p_preserve => false );
    v_json_objeto := json_object_t(e_json);    
    v_id_archivo_instancia         := v_json_objeto.get_number('id_archivo_instancia');    
    
    apex_json.open_object();        
        apex_json.write('codigo', 0);
        apex_json.write('mensaje', 'Consulta exitosa');
        apex_json.open_object('datos');
         -- gnl_archivos_instancia
            apex_json.open_array('archivos_instancia_detalle');
                for c_datos in (
                    SELECT
                          a.id_archivo_instancia_detalle
                        , a.id_archivo_instancia
                        , a.numero_fila
                        , a.estado
                        , a.observaciones
                        , a.datos
                        , a.referencia
                    FROM aire.gnl_archivos_instancia_detalle a
                    where a.id_archivo_instancia = v_id_archivo_instancia
                ) loop
                apex_json.open_object();
                    apex_json.write('id_archivo_instancia_detalle', c_datos.id_archivo_instancia_detalle);                  
                    apex_json.write('id_archivo_instancia', c_datos.id_archivo_instancia);
                    apex_json.write('numero_fila', c_datos.numero_fila);
                    apex_json.write('estado', c_datos.estado);
                    apex_json.write('observaciones', c_datos.observaciones);
                    apex_json.write('datos', c_datos.datos);
                    apex_json.write('referencia', c_datos.referencia);
                apex_json.close_object();
                end loop;
            apex_json.close_array();            
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
    
    --//--Migrado a pkg_g_ordenes 31/01/2024
    procedure prc_gestionar_orden_des_asignar_contratista (		
        e_json  in  clob,
        s_json 	out	clob
    ) is
        --e_json  	clob :=  '{"id_contratista":0,"id_ordenes":"1,150,220","contratista_asignar":"N"}';
        v_json_objeto           json_object_t; 
        v_id_log		        aire.gnl_logs.id_log%type;
        v_respuesta		        aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_carlos_vargas_test.prc_gestionar_orden_des_asignar_contratista');        
        
        --v_id_contratista          varchar2(100 BYTE);
        v_id_orden_string         varchar2(4000 byte);
        v_asignar_contratista     varchar2(1 BYTE); 
        
        -- Declarar la tabla temporal
        l_num_list              num_list_type := num_list_type();        
        v_cnt_actualizar        number;
        v_cnt_actualizar2       number;
        v_id_estado_orden       number;
    BEGIN  
    --LOG
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
    
    -- Deserializar JSON
    apex_json.free_output; 
    apex_json.initialize_clob_output( p_preserve => false );
    
    v_json_objeto := json_object_t(e_json);    
    --v_id_contratista         := v_json_objeto.get_string('id_contratista');    
    v_id_orden_string        := v_json_objeto.get_string('id_orden_string');    
    v_asignar_contratista    := v_json_objeto.get_string('contratista_asignar');    
    
    apex_json.free_output;
        
    --Validar, si no llega ningun registro retornar provocar error.
    SELECT
       TO_NUMBER(REGEXP_SUBSTR(v_id_orden_string, '[^,]+', 1, level))
       BULK COLLECT INTO l_num_list
    FROM dual
    CONNECT BY REGEXP_SUBSTR(v_id_orden_string, '[^,]+', 1, level) IS NOT NULL;
    
    --//----INICIO validar si hay ordenes en estado diferente a asignado a contratista
    select count(*) into v_cnt_actualizar2 
    from aire.ord_ordenes 
    where       id_orden in (select COLUMN_VALUE from TABLE(l_num_list))
            and id_contratista is not null
            and id_estado_orden not in (
                        select id_estado_orden 
                        from aire.ord_estados_orden 
                        where   codigo_estado in ('SASI') 
                            and id_actividad IN (select id_actividad from aire.gnl_actividades where nombre = 'SCR' and ind_activo = 'S')
                        );
                        
    IF (v_cnt_actualizar2 > 0) THEN
        --Si todo sale bien devolver mensaje.
        apex_json.open_object();
            apex_json.write('codigo', 50004);
            apex_json.write('mensaje', 'Se encontraron ordenes en estado diferente a 1-SASI-Asignada Contratista');            
        apex_json.close_object();
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
        return;
    END IF;
    --//----FIN validar si hay ordenes en estado diferente a asignado a contratista
    
    --//----INICIO validar si hay ordenes en estado asignado a contratista
    select count(*) into v_cnt_actualizar 
    from aire.ord_ordenes 
    where       id_orden in (select COLUMN_VALUE from TABLE(l_num_list))
            and id_contratista is not null
            and id_estado_orden in (
                        select id_estado_orden 
                        from aire.ord_estados_orden 
                        where   codigo_estado in ('SASI') 
                            and id_actividad IN (select id_actividad from aire.gnl_actividades where nombre = 'SCR' and ind_activo = 'S')
                        );
                        
    IF (v_cnt_actualizar > 0) THEN        
        select id_estado_orden into v_id_estado_orden
        from aire.ord_estados_orden 
        where   codigo_estado in ('SPEN') 
            and id_actividad IN (select id_actividad from aire.gnl_actividades where nombre = 'SCR' and ind_activo = 'S');
            
        update aire.ord_ordenes
        set id_contratista = NULL, id_estado_orden = v_id_estado_orden
        where   id_orden in (select COLUMN_VALUE from TABLE(l_num_list))
            and id_contratista is not null
            and id_estado_orden in (
                select id_estado_orden 
                from aire.ord_estados_orden 
                where   codigo_estado in ('SASI') 
                    and id_actividad IN (select id_actividad from aire.gnl_actividades where nombre = 'SCR' and ind_activo = 'S')
                );
                
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
            apex_json.write('codigo', 50005);
            apex_json.write('mensaje', 'No se actualizo ninguna orden');            
        apex_json.close_object();
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
        return;
    END IF;
    --//----FIN validar si hay ordenes en estado asignado a contratista
    
    --LOG
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Carga exitosa');        
    --dbms_output.put_line(s_json);
    exception
    when others then
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al crear el json: '|| sqlerrm);
        apex_json.free_output; 
        apex_json.open_object();
        apex_json.write('codigo', 50010);
        apex_json.write('mensaje', 'Error N '||v_id_log ||' al consultar el listado de ordenes.' || sqlerrm);            
        apex_json.close_object();
        --v_json := apex_json.get_clob_output;
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
        --s_json := v_json;
        --dbms_output.put_line('xxx: '|| sqlerrm);
    END prc_gestionar_orden_des_asignar_contratista;
  
    --//--Migrado a pkg_g_ordenes 31/01/2024
    procedure prc_consultar_orden_por_id (		
        e_json  in  clob,
        s_json 	out	clob
    ) is
        --e_json  	clob :=  '{"id_orden": 57}';
        --v_json      clob;
        v_json_objeto       json_object_t; 
        v_id_log		    aire.gnl_logs.id_log%type;
        v_respuesta		    aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_carlos_vargas_test.prc_consultar_orden_por_id');
        v_objeto      	    aire.ord_ordenes%rowtype;
        v_id_orden          NUMBER;        
    BEGIN
    
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
    apex_json.free_output; 
    apex_json.initialize_clob_output( p_preserve => false );   
    
    -- parseamos el json
    v_json_objeto := json_object_t(e_json);
    
    -- extraemos los datos de la orden
    v_id_orden              := v_json_objeto.get_number('id_orden');
    
    apex_json.open_object();        
        apex_json.write('codigo', 0);
        apex_json.write('mensaje', 'Consulta exitosa');
        apex_json.open_object('datos');
         -- ordenes
            apex_json.open_array('orden');
                for c_datos in (
                    select a.id_orden
                             , a.id_tipo_orden
                             , h.descripcion as tipo_orden
                             , a.numero_orden
                             , a.id_estado_orden 
                             , b.descripcion  as estado_orden
                             , NVL(a.id_contratista,-1) id_contratista
                             , NVL(c.nombre_completo,'-') as contratista
                             , a.id_cliente
                             , d.nombre_cliente as cliente
                             , a.id_territorial
                             , e.nombre as territorial
                             , NVL(a.id_zona,-1) id_zona
                             , NVL(f.nombre,'-') as zona
                             , a.direcion
                             , a.fecha_creacion
                             , NVL(a.fecha_cierre,TO_TIMESTAMP('1900/01/01 12:01:01,000000000 AM', 'YYYY/MM/DD HH:MI:SS,FF9 AM')) as fecha_cierre
                             , NVL(a.id_usuario_cierre,-1) as id_usuario_cierre
                             , NVL(g.nombres,'-') as usuario_cierre
                             , a.id_actividad
                             , i.nombre as actividad
                             , NVL(a.id_contratista_persona,-1) id_contratista_persona
                             , NVL(j.nombre_contratista_persona,'-') nombre_contratista_persona
                             , NVL(a.id_tipo_trabajo,-1) as id_tipo_trabajo
                             , NVL(k.descripcion,'-') as tipo_trabajo
                             , NVL(a.id_tipo_suspencion,-1) as id_tipo_suspencion 
                             , NVL(l.descripcion,'-') as tipo_suspencion                             
                    from aire.ord_ordenes              				a
                    left join aire.ord_estados_orden   				b on a.id_estado_orden   		= b.id_estado_orden
                    left join aire.v_ctn_contratistas  				c on a.id_contratista    		= c.id_contratista
                    left join aire.gnl_clientes        				d on a.id_cliente        		= d.id_cliente
                    left join aire.gnl_territoriales   				e on a.id_territorial    		= e.id_territorial
                    left join aire.gnl_zonas           				f on a.id_zona           		= f.id_zona
                    left join aire.v_sgd_usuarios      				g on a.id_usuario_cierre 		= g.id_usuario
                    left join aire.ord_tipos_orden     				h on a.id_tipo_orden     		= h.id_tipo_orden
                    left join aire.gnl_actividades     				i on a.id_actividad      		= i.id_actividad
                    left join aire.v_ctn_contratistas_persona  		j on a.id_contratista_persona 	= j.id_contratista_persona
                    left join aire.ord_tipos_trabajo 				k on a.id_tipo_trabajo 		 	= k.id_tipo_trabajo
                    left join aire.scr_tipos_suspencion 			l on a.id_tipo_suspencion 	 	= l.id_tipo_suspencion
                    where a.id_orden = v_id_orden
                ) loop
                apex_json.open_object();
                    apex_json.write('id_orden', c_datos.id_orden);
                    apex_json.write('id_tipo_orden', c_datos.id_tipo_orden);
                    apex_json.write('tipo_orden', c_datos.tipo_orden);
                    apex_json.write('numero_orden', c_datos.numero_orden);
                    apex_json.write('id_estado_orden', c_datos.id_estado_orden);
                    apex_json.write('estado_orden', c_datos.estado_orden);
                    apex_json.write('id_contratista', c_datos.id_contratista);
                    apex_json.write('contratista', c_datos.contratista);
                    apex_json.write('id_cliente', c_datos.id_cliente);
                    apex_json.write('cliente', c_datos.cliente);
                    apex_json.write('id_territorial', c_datos.id_territorial);
                    apex_json.write('territorial', c_datos.territorial);
                    apex_json.write('id_zona', c_datos.id_zona);
                    apex_json.write('zona', c_datos.zona);
                    apex_json.write('direcion', c_datos.direcion);
                    apex_json.write('fecha_creacion', c_datos.fecha_creacion);
                    apex_json.write('fecha_cierre', c_datos.fecha_cierre);
                    apex_json.write('id_usuario_cierre', c_datos.id_usuario_cierre);
                    apex_json.write('usuario_cierre', c_datos.usuario_cierre);
                    apex_json.write('id_actividad', c_datos.id_actividad);
                    apex_json.write('actividad', c_datos.actividad);
                    apex_json.write('id_contratista_persona', c_datos.id_contratista_persona);
                    apex_json.write('nombre_contratista_persona', c_datos.nombre_contratista_persona);
                    apex_json.write('id_tipo_trabajo', c_datos.id_tipo_trabajo);
                    apex_json.write('tipo_trabajo', c_datos.tipo_trabajo);
                    apex_json.write('id_tipo_suspencion', c_datos.id_tipo_suspencion);
                    apex_json.write('tipo_suspencion', c_datos.tipo_suspencion);                    
                apex_json.close_object();
            end loop;
            apex_json.close_array(); 
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
        apex_json.write('mensaje', 'Error N '||v_id_log ||' al consultar la orden.' || sqlerrm);            
        apex_json.close_object();
        
        s_json := apex_json.get_clob_output;
        apex_json.free_output;        
    END prc_consultar_orden_por_id;
  
    --//--Migrado a pkg_g_ordenes 31/01/2024
    procedure prc_consultar_ordenes_dashboard_area_central (		
        e_json  in  clob,
        s_json 	out	clob
    ) is
        --e_json  	clob :=  '{"pageSize":1,"pageNumber":0,"sortColumn":"id_orden","sortDirection":"desc"}';
        --v_json      clob;
        v_json_objeto       json_object_t; 
        v_id_log		    aire.gnl_logs.id_log%type;
        v_respuesta		    aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_carlos_vargas_test.prc_consultar_ordenes_dashboard_area_central');
        v_objeto      	    aire.ord_ordenes%rowtype;
        v_id_orden          NUMBER;
        pageNumber          NUMBER; 
        pageSize            NUMBER;
        sortColumn          VARCHAR(100 BYTE);
        sortDirection       VARCHAR(100 BYTE);  
        v_RegistrosTotales  NUMBER;
    BEGIN
    
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
    apex_json.free_output; 
    apex_json.initialize_clob_output( p_preserve => false );   
    
    -- parseamos el json
    v_json_objeto := json_object_t(e_json);
    
    -- extraemos los datos de la orden
    --v_objeto.id_contratista := v_json_objeto.get_string('id_contratista');
    --v_objeto.id_zona        := v_json_objeto.get_string('id_zona'); 
    --v_id_orden              := v_json_objeto.get_number('id_orden');
    
    pageNumber              := v_json_objeto.get_number('pageNumber');
    pageSize              := v_json_objeto.get_number('pageSize');
    --sortColumn              := v_json_objeto.get_string('sortColumn');
    --sortDirection              := v_json_objeto.get_string('sortDirection');
        
    apex_json.open_object();        
        apex_json.write('codigo', 0);
        apex_json.write('mensaje', 'Consulta exitosa');
        apex_json.open_object('datos');
         -- ordenes
            apex_json.open_array('ordenes');
                for c_datos in (
                    with tx as (
                        select a.id_orden
                             , a.id_tipo_orden
                             , h.descripcion as tipo_orden
                             , a.numero_orden
                             , a.id_estado_orden 
                             , b.descripcion  as estado_orden
                             , NVL(a.id_contratista,-1) id_contratista
                             , NVL(c.nombre_completo,'-') as contratista
                             , a.id_cliente
                             , d.nombre_cliente as cliente
                             , a.id_territorial
                             , e.nombre as territorial
                             , NVL(a.id_zona,-1) id_zona
                             , NVL(f.nombre,'-') as zona
                             , a.direcion
                             , a.fecha_creacion
                             , NVL(a.fecha_cierre,TO_TIMESTAMP('1900/01/01 12:01:01,000000000 AM', 'YYYY/MM/DD HH:MI:SS,FF9 AM')) as fecha_cierre
                             , NVL(a.id_usuario_cierre,-1) as id_usuario_cierre
                             , NVL(g.nombres,'-') as usuario_cierre
                             , a.id_actividad
                             , i.nombre as actividad
                             , a.id_contratista_persona
                             , j.nombre_contratista_persona
                             , NVL(a.id_tipo_trabajo,-1) as id_tipo_trabajo
                             , NVL(k.descripcion,'-') as tipo_trabajo
                             , NVL(a.id_tipo_suspencion,-1) as id_tipo_suspencion 
                             , NVL(l.descripcion,'-') as tipo_suspencion
                             , SUM(1) OVER() RegistrosTotales
                        from aire.ord_ordenes              				a
                        left join aire.ord_estados_orden   				b on a.id_estado_orden   		= b.id_estado_orden
                        left join aire.v_ctn_contratistas  				c on a.id_contratista    		= c.id_contratista
                        left join aire.gnl_clientes        				d on a.id_cliente        		= d.id_cliente
                        left join aire.gnl_territoriales   				e on a.id_territorial    		= e.id_territorial
                        left join aire.gnl_zonas           				f on a.id_zona           		= f.id_zona
                        left join aire.v_sgd_usuarios      				g on a.id_usuario_cierre 		= g.id_usuario
                        left join aire.ord_tipos_orden     				h on a.id_tipo_orden     		= h.id_tipo_orden
                        left join aire.gnl_actividades     				i on a.id_actividad      		= i.id_actividad
                        left join aire.v_ctn_contratistas_persona  		j on a.id_contratista_persona 	= j.id_contratista_persona
                        left join aire.ord_tipos_trabajo 				k on a.id_tipo_trabajo 		 	= k.id_tipo_trabajo
                        left join aire.scr_tipos_suspencion 			l on a.id_tipo_suspencion 	 	= l.id_tipo_suspencion
                        /*where 
                            case 
                                when v_id_orden = -1 and a.id_orden >= 0 then 1
                                when v_id_orden = -2 and a.id_orden >= 0 then 1
                                when v_id_orden > 0 and a.id_orden = v_id_orden then 1
                                else 0
                            end = 1 and
                            case 
                                when v_id_orden > 0 then 1
                                when v_objeto.id_contratista = -1 and a.id_contratista is null then 1
                                when v_objeto.id_contratista = -2 and (a.id_contratista is not null or a.id_contratista is null) then 1
                                when v_objeto.id_contratista > 0 and a.id_contratista = v_objeto.id_contratista then 1                            
                                else 0
                            end = 1 and
                            case 
                                when v_id_orden > 0 then 1
                                when v_objeto.id_zona = -1 and a.id_zona is null then 1
                                when v_objeto.id_zona = -2 and (a.id_zona is not null or a.id_zona is null) then 1
                                when v_objeto.id_zona > 0 and a.id_zona = v_objeto.id_zona then 1                            
                                else 0
                            end = 1 */
                    )
                    select 
                        a.*
                    from tx a                   
                    OFFSET pageNumber ROWS FETCH NEXT pageSize ROWS ONLY
                ) loop
                apex_json.open_object();
                    apex_json.write('id_orden', c_datos.id_orden);
                    apex_json.write('id_tipo_orden', c_datos.id_tipo_orden);
                    apex_json.write('tipo_orden', c_datos.tipo_orden);
                    apex_json.write('numero_orden', c_datos.numero_orden);
                    apex_json.write('id_estado_orden', c_datos.id_estado_orden);
                    apex_json.write('estado_orden', c_datos.estado_orden);
                    apex_json.write('id_contratista', c_datos.id_contratista);
                    apex_json.write('contratista', c_datos.contratista);
                    apex_json.write('id_cliente', c_datos.id_cliente);
                    apex_json.write('cliente', c_datos.cliente);
                    apex_json.write('id_territorial', c_datos.id_territorial);
                    apex_json.write('territorial', c_datos.territorial);
                    apex_json.write('id_zona', c_datos.id_zona);
                    apex_json.write('zona', c_datos.zona);
                    apex_json.write('direcion', c_datos.direcion);
                    apex_json.write('fecha_creacion', c_datos.fecha_creacion);
                    apex_json.write('fecha_cierre', c_datos.fecha_cierre);
                    apex_json.write('id_usuario_cierre', c_datos.id_usuario_cierre);
                    apex_json.write('usuario_cierre', c_datos.usuario_cierre);
                    apex_json.write('id_actividad', c_datos.id_actividad);
                    apex_json.write('actividad', c_datos.actividad);
                    apex_json.write('id_contratista_persona', c_datos.id_contratista_persona);
                    apex_json.write('nombre_contratista_persona', c_datos.nombre_contratista_persona);
                    apex_json.write('id_tipo_trabajo', c_datos.id_tipo_trabajo);
                    apex_json.write('tipo_trabajo', c_datos.tipo_trabajo);
                    apex_json.write('id_tipo_suspencion', c_datos.id_tipo_suspencion);
                    apex_json.write('tipo_suspencion', c_datos.tipo_suspencion);
                    v_RegistrosTotales := c_datos.RegistrosTotales;
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
        apex_json.write('mensaje', 'Error N '||v_id_log ||' al consultar el listado de ordenes.' || sqlerrm);            
        apex_json.close_object();
        
        s_json := apex_json.get_clob_output;
        apex_json.free_output;        
    END prc_consultar_ordenes_dashboard_area_central;
  
    --//--Migrado a pkg_g_ordenes 31/01/2024
    procedure prc_gestionar_orden_des_asignar_brigada (		
        e_json  in  clob,
        s_json 	out	clob
    ) is
        --e_json  	clob :=  '{"id_contratista":0,"id_ordenes":"1,150,220","contratista_asignar":"N"}';
        v_json_objeto           json_object_t; 
        v_id_log		        aire.gnl_logs.id_log%type;
        v_respuesta		        aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_carlos_vargas_test.prc_gestionar_orden_des_asignar_brigada');        
        
        v_identificacion_contratista        varchar2(50 BYTE);
        v_id_contratista                    varchar2(100 BYTE);        
        
        v_id_orden_string         varchar2(4000 byte);
        v_cnt                     number;
        
        -- Declarar la tabla temporal
        l_num_list              num_list_type := num_list_type();        
        v_cnt_actualizar        number;
        v_id_estado_orden       number;
    BEGIN  
    --LOG
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
    
    -- Deserializar JSON
    apex_json.free_output; 
    apex_json.initialize_clob_output( p_preserve => false );
    
    v_json_objeto := json_object_t(e_json);    
    v_identificacion_contratista        := v_json_objeto.get_string('identificacion_contratista'); 
    v_id_orden_string                   := v_json_objeto.get_string('id_orden_string');
    
    SELECT 
         count(*)  into v_cnt
    FROM aire.v_ctn_contratistas_persona cp
    where   cp.ind_activo = 'S' 
        and cp.identificacion_contratista = v_identificacion_contratista;
    
    --Si no se encuentra registros, devolver el error
    IF (v_cnt = 0) THEN        
        apex_json.open_object();
            apex_json.write('codigo', 50021);
            apex_json.write('mensaje', 'No se encontro el contratista');            
        apex_json.close_object();
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
        return;
    END IF;
    
    SELECT 
         nvl(cp.id_contratista,'-1')         
         into 
             v_id_contratista         
    FROM aire.v_ctn_contratistas cp
    where   cp.ind_activo = 'S' 
        and cp.identificacion = v_identificacion_contratista;
    
    --Si no se encuentra registros, devolver el error
    IF (v_id_contratista = '-1') THEN        
        apex_json.open_object();
            apex_json.write('codigo', 50022);
            apex_json.write('mensaje', 'No se encontro el contratista, busqueda 2');            
        apex_json.close_object();
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
        return;
    END IF;
    
    apex_json.free_output;
        
    --Validar, si no llega ningun registro retornar provocar error.
    SELECT
       TO_NUMBER(REGEXP_SUBSTR(v_id_orden_string, '[^,]+', 1, level))
       BULK COLLECT INTO l_num_list
    FROM dual
    CONNECT BY REGEXP_SUBSTR(v_id_orden_string, '[^,]+', 1, level) IS NOT NULL;
    
    --//--Solo se puede desasignar una orden del tecnico si se encuentra en estado 23-SEAS-Asignada Tecnico
    select count(*) into v_cnt_actualizar 
    from aire.ord_ordenes 
    where       id_orden in (select COLUMN_VALUE from TABLE(l_num_list))
            and id_contratista = v_id_contratista
            and id_contratista_persona is not null            
            and id_estado_orden in (
                        select id_estado_orden 
                        from aire.ord_estados_orden 
                        where   codigo_estado in ('SEAS') 
                            and id_actividad IN (select id_actividad from aire.gnl_actividades where nombre = 'SCR' and ind_activo = 'S')
                        );
                        
    IF (v_cnt_actualizar > 0) THEN        
        select id_estado_orden into v_id_estado_orden
        from aire.ord_estados_orden 
        where   codigo_estado in ('SASI') 
            and id_actividad IN (select id_actividad from aire.gnl_actividades where nombre = 'SCR' and ind_activo = 'S');
        
        update aire.ord_ordenes
        set id_contratista_persona = NULL, id_estado_orden = v_id_estado_orden
        where   id_orden in (select COLUMN_VALUE from TABLE(l_num_list))
            and id_contratista = v_id_contratista
            and id_contratista_persona is not null  
            and id_estado_orden in (
                select id_estado_orden 
                from aire.ord_estados_orden 
                where   codigo_estado in ('SEAS') 
                    and id_actividad IN (select id_actividad from aire.gnl_actividades where nombre = 'SCR' and ind_activo = 'S')
                );
                
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
  
    --//--Migrado a pkg_g_ordenes 31/01/2024
    procedure prc_consultar_ordenes_contratistas (		
        e_json  in  clob,
        s_json 	out	clob
    ) is
        --e_json  	clob :=  '{"id_contratista": "2076341","id_zona": ""}';
        --v_json      clob;
        v_json_objeto       json_object_t; 
        v_id_log		    aire.gnl_logs.id_log%type;
        v_respuesta		    aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_carlos_vargas_test.prc_consultar_ordenes_contratistas');
        v_objeto      	    aire.ord_ordenes%rowtype;
        v_id_orden          NUMBER;
        pageNumber          NUMBER; 
        pageSize            NUMBER;
        sortColumn          VARCHAR(100 BYTE);
        sortDirection       VARCHAR(100 BYTE);  
        v_RegistrosTotales  NUMBER;
    BEGIN
    
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
    apex_json.free_output; 
    apex_json.initialize_clob_output( p_preserve => false );   
    
    -- parseamos el json
    v_json_objeto := json_object_t(e_json);
    
    -- extraemos los datos de la orden    
    v_objeto.id_contratista         := v_json_objeto.get_string('id_contratista');
    v_objeto.id_contratista_persona := v_json_objeto.get_string('id_contratista_persona');
    v_objeto.id_estado_orden        := v_json_objeto.get_string('id_estado_orden');
    v_objeto.id_zona                := v_json_objeto.get_string('id_zona'); 
    v_id_orden                      := v_json_objeto.get_number('id_orden');
    
    pageNumber                      := v_json_objeto.get_number('pageNumber');
    pageSize                        := v_json_objeto.get_number('pageSize');
    --sortColumn              := v_json_objeto.get_string('sortColumn');
    --sortDirection              := v_json_objeto.get_string('sortDirection');
        
    apex_json.open_object();        
        apex_json.write('codigo', 0);
        apex_json.write('mensaje', 'Consulta exitosa');
        apex_json.open_object('datos');
         -- ordenes
            apex_json.open_array('ordenes');
                for c_datos in (
                    with tx as (
                        select a.id_orden
                             , a.id_tipo_orden
                             , h.descripcion as tipo_orden
                             , a.numero_orden
                             , a.id_estado_orden 
                             , b.descripcion  as estado_orden
                             , NVL(a.id_contratista,-1) id_contratista
                             , NVL(c.nombre_completo,'-') as contratista
                             , a.id_cliente
                             , d.nombre_cliente as cliente
                             , a.id_territorial
                             , e.nombre as territorial
                             , NVL(a.id_zona,-1) id_zona
                             , NVL(f.nombre,'-') as zona
                             , a.direcion
                             , a.fecha_creacion
                             , NVL(a.fecha_cierre,TO_TIMESTAMP('1900/01/01 12:01:01,000000000 AM', 'YYYY/MM/DD HH:MI:SS,FF9 AM')) as fecha_cierre
                             , NVL(a.id_usuario_cierre,-1) as id_usuario_cierre
                             , NVL(g.nombres,'-') as usuario_cierre
                             , a.id_actividad
                             , i.nombre as actividad
                             , a.id_contratista_persona
                             , j.nombre_contratista_persona
                             , NVL(a.id_tipo_trabajo,-1) as id_tipo_trabajo
                             , NVL(k.descripcion,'-') as tipo_trabajo
                             , NVL(a.id_tipo_suspencion,-1) as id_tipo_suspencion 
                             , NVL(l.descripcion,'-') as tipo_suspencion
                             , SUM(1) OVER() RegistrosTotales
                        from aire.ord_ordenes              				a
                        left join aire.ord_estados_orden   				b on a.id_estado_orden   		= b.id_estado_orden
                        left join aire.v_ctn_contratistas  				c on a.id_contratista    		= c.id_contratista
                        left join aire.gnl_clientes        				d on a.id_cliente        		= d.id_cliente
                        left join aire.gnl_territoriales   				e on a.id_territorial    		= e.id_territorial
                        left join aire.gnl_zonas           				f on a.id_zona           		= f.id_zona
                        left join aire.v_sgd_usuarios      				g on a.id_usuario_cierre 		= g.id_usuario
                        left join aire.ord_tipos_orden     				h on a.id_tipo_orden     		= h.id_tipo_orden
                        left join aire.gnl_actividades     				i on a.id_actividad      		= i.id_actividad
                        left join aire.v_ctn_contratistas_persona  		j on a.id_contratista_persona 	= j.id_contratista_persona
                        left join aire.ord_tipos_trabajo 				k on a.id_tipo_trabajo 		 	= k.id_tipo_trabajo
                        left join aire.scr_tipos_suspencion 			l on a.id_tipo_suspencion 	 	= l.id_tipo_suspencion
                        where NVL(a.id_contratista,-1) in (
                                select id_contratista
                                      from aire.v_ctn_contratos 
                                     where prefijo_actividad = 'G_SCR'
                                       and ind_activo        = 'S'       
                                union all
                                select -1 from dual
                            ) and
                            case 
                                when v_id_orden = -1 and a.id_orden >= 0 then 1
                                when v_id_orden = -2 and a.id_orden >= 0 then 1
                                when v_id_orden > 0 and a.id_orden = v_id_orden then 1
                                else 0
                            end = 1 and
                            a.id_contratista = v_objeto.id_contratista and
                            case 
                                when v_id_orden > 0 then 1
                                when v_objeto.id_contratista_persona = -1 and a.id_contratista_persona is null then 1
                                when v_objeto.id_contratista_persona = -2 and (a.id_contratista_persona is not null or a.id_contratista_persona is null) then 1
                                when v_objeto.id_contratista_persona > 0 and a.id_contratista_persona = v_objeto.id_contratista_persona then 1                            
                                else 0
                            end = 1 and
                            case 
                                when v_id_orden > 0 then 1
                                when v_objeto.id_zona = -1 and a.id_zona is null then 1
                                when v_objeto.id_zona = -2 and (a.id_zona is not null or a.id_zona is null) then 1
                                when v_objeto.id_zona > 0 and a.id_zona = v_objeto.id_zona then 1                            
                                else 0
                            end = 1 and
                            case 
                                when v_id_orden > 0 then 1
                                when v_objeto.id_estado_orden = -1 and a.id_estado_orden is null then 1
                                when v_objeto.id_estado_orden = -2 and (a.id_estado_orden is not null or a.id_estado_orden is null) then 1
                                when v_objeto.id_estado_orden > 0 and a.id_estado_orden = v_objeto.id_estado_orden then 1                            
                                else 0
                            end = 1 
                    )
                    select 
                        a.*
                    from tx a                   
                    OFFSET pageNumber ROWS FETCH NEXT pageSize ROWS ONLY
                ) loop
                apex_json.open_object();
                    apex_json.write('id_orden', c_datos.id_orden);
                    apex_json.write('id_tipo_orden', c_datos.id_tipo_orden);
                    apex_json.write('tipo_orden', c_datos.tipo_orden);
                    apex_json.write('numero_orden', c_datos.numero_orden);
                    apex_json.write('id_estado_orden', c_datos.id_estado_orden);
                    apex_json.write('estado_orden', c_datos.estado_orden);
                    apex_json.write('id_contratista', c_datos.id_contratista);
                    apex_json.write('contratista', c_datos.contratista);
                    apex_json.write('id_cliente', c_datos.id_cliente);
                    apex_json.write('cliente', c_datos.cliente);
                    apex_json.write('id_territorial', c_datos.id_territorial);
                    apex_json.write('territorial', c_datos.territorial);
                    apex_json.write('id_zona', c_datos.id_zona);
                    apex_json.write('zona', c_datos.zona);
                    apex_json.write('direcion', c_datos.direcion);
                    apex_json.write('fecha_creacion', c_datos.fecha_creacion);
                    apex_json.write('fecha_cierre', c_datos.fecha_cierre);
                    apex_json.write('id_usuario_cierre', c_datos.id_usuario_cierre);
                    apex_json.write('usuario_cierre', c_datos.usuario_cierre);
                    apex_json.write('id_actividad', c_datos.id_actividad);
                    apex_json.write('actividad', c_datos.actividad);
                    apex_json.write('id_contratista_persona', c_datos.id_contratista_persona);
                    apex_json.write('nombre_contratista_persona', c_datos.nombre_contratista_persona);
                    apex_json.write('id_tipo_trabajo', c_datos.id_tipo_trabajo);
                    apex_json.write('tipo_trabajo', c_datos.tipo_trabajo);
                    apex_json.write('id_tipo_suspencion', c_datos.id_tipo_suspencion);
                    apex_json.write('tipo_suspencion', c_datos.tipo_suspencion);
                    v_RegistrosTotales := c_datos.RegistrosTotales;
                apex_json.close_object();
            end loop;
            apex_json.close_array(); 
            
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
        apex_json.write('mensaje', 'Error N '||v_id_log ||' al consultar el listado de ordenes.' || sqlerrm);            
        apex_json.close_object();
        
        s_json := apex_json.get_clob_output;
        apex_json.free_output;        
    END prc_consultar_ordenes_contratistas;
  
    --//--Migrado a pkg_g_ordenes 31/01/2024
    procedure prc_consultar_ordenes_dashboard_contratistas (		
        e_json  in  clob,
        s_json 	out	clob
    ) is
        --e_json  	clob :=  '{"pageSize":1,"pageNumber":0,"sortColumn":"id_orden","sortDirection":"desc"}';
        --v_json      clob;
        v_json_objeto       json_object_t; 
        v_id_log		    aire.gnl_logs.id_log%type;
        v_respuesta		    aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_carlos_vargas_test.prc_consultar_ordenes_dashboard_contratistas');
        v_objeto      	    aire.ord_ordenes%rowtype;
        v_id_orden          NUMBER;
        pageNumber          NUMBER; 
        pageSize            NUMBER;
        sortColumn          VARCHAR(100 BYTE);
        sortDirection       VARCHAR(100 BYTE);  
        v_RegistrosTotales  NUMBER;
    BEGIN
    
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
    apex_json.free_output; 
    apex_json.initialize_clob_output( p_preserve => false );   
    
    -- parseamos el json
    v_json_objeto := json_object_t(e_json);
    
    -- extraemos los datos de la orden
    --v_objeto.id_contratista := v_json_objeto.get_string('id_contratista');
    --v_objeto.id_zona        := v_json_objeto.get_string('id_zona'); 
    --v_id_orden              := v_json_objeto.get_number('id_orden');
    
    pageNumber              := v_json_objeto.get_number('pageNumber');
    pageSize              := v_json_objeto.get_number('pageSize');
    --sortColumn              := v_json_objeto.get_string('sortColumn');
    --sortDirection              := v_json_objeto.get_string('sortDirection');
        
    apex_json.open_object();        
        apex_json.write('codigo', 0);
        apex_json.write('mensaje', 'Consulta exitosa');
        apex_json.open_object('datos');
         -- ordenes
            apex_json.open_array('ordenes');
                for c_datos in (
                    with tx as (
                        select a.id_orden
                             , a.id_tipo_orden
                             , h.descripcion as tipo_orden
                             , a.numero_orden
                             , a.id_estado_orden 
                             , b.descripcion  as estado_orden
                             , NVL(a.id_contratista,-1) id_contratista
                             , NVL(c.nombre_completo,'-') as contratista
                             , a.id_cliente
                             , d.nombre_cliente as cliente
                             , a.id_territorial
                             , e.nombre as territorial
                             , NVL(a.id_zona,-1) id_zona
                             , NVL(f.nombre,'-') as zona
                             , a.direcion
                             , a.fecha_creacion
                             , NVL(a.fecha_cierre,TO_TIMESTAMP('1900/01/01 12:01:01,000000000 AM', 'YYYY/MM/DD HH:MI:SS,FF9 AM')) as fecha_cierre
                             , NVL(a.id_usuario_cierre,-1) as id_usuario_cierre
                             , NVL(g.nombres,'-') as usuario_cierre
                             , a.id_actividad
                             , i.nombre as actividad
                             , a.id_contratista_persona
                             , j.nombre_contratista_persona
                             , NVL(a.id_tipo_trabajo,-1) as id_tipo_trabajo
                             , NVL(k.descripcion,'-') as tipo_trabajo
                             , NVL(a.id_tipo_suspencion,-1) as id_tipo_suspencion 
                             , NVL(l.descripcion,'-') as tipo_suspencion
                             , SUM(1) OVER() RegistrosTotales
                        from aire.ord_ordenes              				a
                        left join aire.ord_estados_orden   				b on a.id_estado_orden   		= b.id_estado_orden
                        left join aire.v_ctn_contratistas  				c on a.id_contratista    		= c.id_contratista
                        left join aire.gnl_clientes        				d on a.id_cliente        		= d.id_cliente
                        left join aire.gnl_territoriales   				e on a.id_territorial    		= e.id_territorial
                        left join aire.gnl_zonas           				f on a.id_zona           		= f.id_zona
                        left join aire.v_sgd_usuarios      				g on a.id_usuario_cierre 		= g.id_usuario
                        left join aire.ord_tipos_orden     				h on a.id_tipo_orden     		= h.id_tipo_orden
                        left join aire.gnl_actividades     				i on a.id_actividad      		= i.id_actividad
                        left join aire.v_ctn_contratistas_persona  		j on a.id_contratista_persona 	= j.id_contratista_persona
                        left join aire.ord_tipos_trabajo 				k on a.id_tipo_trabajo 		 	= k.id_tipo_trabajo
                        left join aire.scr_tipos_suspencion 			l on a.id_tipo_suspencion 	 	= l.id_tipo_suspencion
                        /*where 
                            case 
                                when v_id_orden = -1 and a.id_orden >= 0 then 1
                                when v_id_orden = -2 and a.id_orden >= 0 then 1
                                when v_id_orden > 0 and a.id_orden = v_id_orden then 1
                                else 0
                            end = 1 and
                            case 
                                when v_id_orden > 0 then 1
                                when v_objeto.id_contratista = -1 and a.id_contratista is null then 1
                                when v_objeto.id_contratista = -2 and (a.id_contratista is not null or a.id_contratista is null) then 1
                                when v_objeto.id_contratista > 0 and a.id_contratista = v_objeto.id_contratista then 1                            
                                else 0
                            end = 1 and
                            case 
                                when v_id_orden > 0 then 1
                                when v_objeto.id_zona = -1 and a.id_zona is null then 1
                                when v_objeto.id_zona = -2 and (a.id_zona is not null or a.id_zona is null) then 1
                                when v_objeto.id_zona > 0 and a.id_zona = v_objeto.id_zona then 1                            
                                else 0
                            end = 1 */
                    )
                    select 
                        a.*
                    from tx a                   
                    OFFSET pageNumber ROWS FETCH NEXT pageSize ROWS ONLY
                ) loop
                apex_json.open_object();
                    apex_json.write('id_orden', c_datos.id_orden);
                    apex_json.write('id_tipo_orden', c_datos.id_tipo_orden);
                    apex_json.write('tipo_orden', c_datos.tipo_orden);
                    apex_json.write('numero_orden', c_datos.numero_orden);
                    apex_json.write('id_estado_orden', c_datos.id_estado_orden);
                    apex_json.write('estado_orden', c_datos.estado_orden);
                    apex_json.write('id_contratista', c_datos.id_contratista);
                    apex_json.write('contratista', c_datos.contratista);
                    apex_json.write('id_cliente', c_datos.id_cliente);
                    apex_json.write('cliente', c_datos.cliente);
                    apex_json.write('id_territorial', c_datos.id_territorial);
                    apex_json.write('territorial', c_datos.territorial);
                    apex_json.write('id_zona', c_datos.id_zona);
                    apex_json.write('zona', c_datos.zona);
                    apex_json.write('direcion', c_datos.direcion);
                    apex_json.write('fecha_creacion', c_datos.fecha_creacion);
                    apex_json.write('fecha_cierre', c_datos.fecha_cierre);
                    apex_json.write('id_usuario_cierre', c_datos.id_usuario_cierre);
                    apex_json.write('usuario_cierre', c_datos.usuario_cierre);
                    apex_json.write('id_actividad', c_datos.id_actividad);
                    apex_json.write('actividad', c_datos.actividad);
                    apex_json.write('id_contratista_persona', c_datos.id_contratista_persona);
                    apex_json.write('nombre_contratista_persona', c_datos.nombre_contratista_persona);
                    apex_json.write('id_tipo_trabajo', c_datos.id_tipo_trabajo);
                    apex_json.write('tipo_trabajo', c_datos.tipo_trabajo);
                    apex_json.write('id_tipo_suspencion', c_datos.id_tipo_suspencion);
                    apex_json.write('tipo_suspencion', c_datos.tipo_suspencion);
                    v_RegistrosTotales := c_datos.RegistrosTotales;
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
        apex_json.write('mensaje', 'Error N '||v_id_log ||' al consultar el listado de ordenes.' || sqlerrm);            
        apex_json.close_object();
        
        s_json := apex_json.get_clob_output;
        apex_json.free_output;        
    END prc_consultar_ordenes_dashboard_contratistas;
  
    --//--Migrado a pkg_g_ordenes 31/01/2024
    procedure prc_consultar_contratista_por_identificacion (		
        e_json  in  clob,
        s_json 	out	clob
    ) is
        --e_json  	clob :=  '{"id_orden": 57}';
        --v_json      clob;
        v_json_objeto       json_object_t; 
        v_id_log		    aire.gnl_logs.id_log%type;
        v_respuesta		    aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_carlos_vargas_test.prc_consultar_contratista_por_identificacion');
        v_objeto      	    aire.ord_ordenes%rowtype;
        v_identificacion          varchar2(100 BYTE);        
    BEGIN
    
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
    apex_json.free_output; 
    apex_json.initialize_clob_output( p_preserve => false );   
    
    -- parseamos el json
    v_json_objeto := json_object_t(e_json);
    
    -- extraemos los datos de la orden
    v_identificacion              := v_json_objeto.get_number('identificacion');
    
    apex_json.open_object();        
        apex_json.write('codigo', 0);
        apex_json.write('mensaje', 'Consulta exitosa');
        apex_json.open_object('datos');
         -- ordenes
            apex_json.open_array('Contratista');
                for c_datos in (
                    SELECT
                        id_contratista,
                        identificacion
                    FROM aire.v_ctn_contratistas
                    where identificacion = v_identificacion
                ) loop
                apex_json.open_object();
                    apex_json.write('id_contratista', c_datos.id_contratista);
                    apex_json.write('identificacion', c_datos.identificacion);                   
                apex_json.close_object();
            end loop;
            apex_json.close_array(); 
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
        apex_json.write('mensaje', 'Error N '||v_id_log ||' al consultar los datos del contratista.' || sqlerrm);            
        apex_json.close_object();
        
        s_json := apex_json.get_clob_output;
        apex_json.free_output;        
    END prc_consultar_contratista_por_identificacion;
  
    --//--Migrado a pkg_g_ordenes 31/01/2024
    procedure prc_resumen_global_ordenes (		
        s_json 	out	clob
    ) is
        --v_json      clob;
        v_json_objeto       json_object_t; 
        v_id_log		    aire.gnl_logs.id_log%type;
        v_respuesta		    aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_carlos_vargas_test.prc_resumen_global_ordenes');
        
        v_RegistrosTotales  NUMBER;
    BEGIN
    
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
    apex_json.free_output; 
    apex_json.initialize_clob_output( p_preserve => false );   
    
    apex_json.open_object();        
        apex_json.write('codigo', 0);
        apex_json.write('mensaje', 'Consulta exitosa');
        apex_json.open_object('datos');
         -- ordenes
            apex_json.open_array('resumen_ordenes');
                for c_datos in (
                    select 
                         NVL(x.id_contratista,-1) as id_contratista
                        ,NVL(c.Nombre_Completo,'-') as nombre_contratista
                        ,NVL(x.id_zona,-1) as id_zona
                        ,NVL(z.Nombre,'-') as nombre_zona
                        ,NVL(x.id_estado_orden,-1) as id_estado_orden
                        ,NVL(eo.Descripcion,'-') as nombre_estado_orden
                        ,count(*) NoRegistros
                    from aire.ord_ordenes x
                    left join aire.v_ctn_contratistas c on c.id_contratista = x.id_contratista
                    left join aire.gnl_zonas z on z.id_zona = x.id_zona
                    left join aire.ord_estados_orden eo on eo.id_estado_orden = x.id_estado_orden
                    group by NVL(x.id_contratista,-1)
                            ,NVL(c.Nombre_Completo,'-')
                            ,NVL(x.id_zona,-1)
                            ,NVL(z.Nombre,'-')
                            ,NVL(x.id_estado_orden,-1)
                            ,NVL(eo.Descripcion,'-')
                    order by NVL(x.id_contratista,-1)  desc      
                            ,NVL(x.id_zona,-1) desc        
                            ,NVL(x.id_estado_orden,-1)                                    
                ) loop
                    apex_json.open_object();
                        apex_json.write('id_contratista', c_datos.id_contratista);
                        apex_json.write('nombre_contratista', c_datos.nombre_contratista);
                        apex_json.write('id_zona', c_datos.id_zona);
                        apex_json.write('nombre_zona', c_datos.nombre_zona);
                        apex_json.write('id_estado_orden', c_datos.id_estado_orden);
                        apex_json.write('nombre_estado_orden', c_datos.nombre_estado_orden);
                        apex_json.write('NoRegistros', c_datos.NoRegistros);
                        --v_RegistrosTotales := c_datos.RegistrosTotales;
                    apex_json.close_object();
                end loop;
            apex_json.close_array(); 
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
        apex_json.write('mensaje', 'Error N '||v_id_log ||' al consultar el agrupamiento.' || sqlerrm);            
        apex_json.close_object();
        
        s_json := apex_json.get_clob_output;
        apex_json.free_output;        
    END prc_resumen_global_ordenes;
    
    
    --//-->--------------------------- FIN PKG_G_CARLOS_VARGAS_TEST2 Procedimientos Carlos Vargas
    
    --//-->--------------------------- INICIO PKG_G_DANIEL_GONZALEZ_TEST Procedimientos Daniel
    --//--Migrado a pkg_g_ordenes 31/01/2024
    procedure prc_obtener_datos_dummi (		
        e_json  in  clob,
        s_json 	out	clob
    ) IS        
        v_json_objeto   json_object_t; 
        v_id_log		aire.gnl_logs.id_log%type;
        v_respuesta		aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_daniel_gonzalez_test.prc_obtener_datos_dummi');
        v_orden      	aire.ord_ordenes%rowtype;
        --v_a number;
        --v_b number;
        --v_r number;
    BEGIN
    -- TAREA: Se necesita implantación para procedure PKG_G_DANIEL_GONZALEZ_TEST.prc_obtener_datos_dummi
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON dummi');
    apex_json.free_output; 
    apex_json.initialize_clob_output( p_preserve => false );
    --v_a := 1;
    --v_b := 0;
    --v_r := v_a / v_b;
    -- armamos o serializamos el json del parametro e_json
    begin
        -- parseamos el json
        v_json_objeto := json_object_t(e_json);
    
        -- extraemos los datos de la orden
        v_orden.id_orden           := v_json_objeto.get_string('id_orden');
        
    exception 
        when others then 
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al procesar el json del dummi: ' || sqlerrm);
            apex_json.initialize_clob_output( p_preserve => false );
            apex_json.open_object();
                apex_json.write('codigo', 1);
                apex_json.write('mensaje', 'Error #'||v_id_log||' al serializar los datos de entrada');
            apex_json.close_object();
            s_json := apex_json.get_clob_output;
            apex_json.free_output;
            return;
    end; 
    
    apex_json.open_object();        
        apex_json.write('codigo', v_orden.id_orden);
        apex_json.write('mensaje', 'Consulta exitosa');
        apex_json.open_object('datos');
        -- Tipos de orden
            apex_json.open_array('ordenes_dummi');
                for c_datos in (
                    select a.id_orden
                         , a.id_tipo_orden
                         , h.descripcion as tipo_orden
                    from aire.ord_ordenes a
                    left join aire.ord_tipos_orden     				h on a.id_tipo_orden     		= h.id_tipo_orden
                    where 
                    (
                    (v_orden.id_orden = -1 and a.id_orden != -1) or
                    (v_orden.id_orden != -1 and a.id_orden = v_orden.id_orden)
                    ) and
                    rownum <= 2
                ) loop
                    apex_json.open_object();
                        apex_json.write('id_orden', c_datos.id_orden);
                        apex_json.write('id_tipo_orden', c_datos.id_tipo_orden);
                        apex_json.write('tipo_orden', c_datos.tipo_orden);
                    apex_json.close_object();
                end loop;
            apex_json.close_array();     
           
        apex_json.close_object();            
    apex_json.close_object();
    
    s_json := apex_json.get_clob_output;
    apex_json.free_output;             
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'se genera json exitosamente');        
    --dbms_output.put_line(s_json);
    
    exception
    when others then
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al crear el json del dummi: '|| sqlerrm);
        apex_json.free_output; 
        apex_json.open_object();
            apex_json.write('codigo', 1);
            apex_json.write('mensaje', 'Error N '||v_id_log ||' al consultar el cliente.'|| sqlerrm);            
        apex_json.close_object();        
        s_json := apex_json.get_clob_output;
        apex_json.free_output;       
        
    END prc_obtener_datos_dummi;
    
    --//--Migrado a pkg_g_ordenes 31/01/2024
    procedure prc_registro_ordenes_masivo_temporal (
        e_json  		in 	clob,
        s_json 	        out	clob
    ) is
        v_json_orden                json_object_t;  
        v_id_log		            aire.gnl_logs.id_log%type;
        v_respuesta                 aire.tip_respuesta := aire.tip_respuesta(mensaje => 'Registro exitoso.', nombre_up => 'pkg_g_daniel_gonzalez_test.prc_registro_ordenes_masivo_temporal');
        v_orden      	            aire.ord_ordenes_cargue_temporal%rowtype;
        valido                      boolean;
        v_rownum                    number;
        v_errorms                   varchar2(4000 BYTE);
    begin
        -- validamos el Json de la orden y armamos el rowtype
        begin
            -- parseamos el json
            v_json_orden := json_object_t(e_json);
    
            -- extraemos los datos de la orden            
            v_orden.nic                     := v_json_orden.get_string('nic');
            v_orden.nis                     := v_json_orden.get_string('nis');
            v_orden.codigo_tipo_orden           := v_json_orden.get_string('id_tipo_orden');
            v_orden.codigo_tipo_suspencion      := v_json_orden.get_string('id_tipo_suspencion');
            v_orden.codigo_estado_servicio      := v_json_orden.get_string('id_estado_servicio');
            v_orden.id_soporte              := v_json_orden.get_number('id_soporte');
            v_orden.con_errores             := v_json_orden.get_string('con_errores');
            v_orden.usuario_registra        := v_json_orden.get_string('usuario_registra');
            --v_orden.fecha_registra        := systimestamp;
            v_orden.desc_validacion         := v_json_orden.get_string('desc_validacion');
            v_rownum                        := v_json_orden.get_number('Autonumerico');
            
            DBMS_OUTPUT.PUT_LINE('v_orden.codigo_tipo_suspencion = ' || v_orden.codigo_tipo_suspencion);
            
            INSERT INTO aire.ord_ordenes_cargue_temporal (
                  nic
                , nis
                , codigo_tipo_orden
                , codigo_tipo_suspencion
                , codigo_estado_servicio
                , id_soporte
                , con_errores
                , usuario_registra
                , fecha_registra
                , desc_validacion
                , numero_orden
            )
            VALUES (
                v_orden.nic
                , v_orden.nis
                , v_orden.codigo_tipo_orden
                , v_orden.codigo_tipo_suspencion
                , v_orden.codigo_estado_servicio
                , v_orden.id_soporte
                , v_orden.con_errores
                , v_orden.usuario_registra
                , SYSTIMESTAMP
                , v_orden.desc_validacion
                ,(to_char(systimestamp, 'DDMMYYYYHH24MISSFF') + v_rownum)
            );
            commit;
            
        exception
            when others then 
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al procesar el json de la orden: '|| sqlerrm);
                apex_json.initialize_clob_output( p_preserve => false );
                apex_json.open_object();
                apex_json.write('codigo', 400);
                v_errorms := replace(sqlerrm,'"','''');
                apex_json.write('mensaje', 'Error #'||v_id_log||' al alimentar la tabla temporal - '|| v_errorms);
                apex_json.close_object();
                s_json := apex_json.get_clob_output;
                apex_json.free_output;
                return;
        end;
        
        apex_json.initialize_clob_output( p_preserve => false );
        apex_json.open_object();
        apex_json.write('codigo', 200);--v_orden.id_ord_ordenes_cargue_tmp
        apex_json.write('mensaje', 'Registro añadido con exito');
        --apex_json.write('mensaje', 'Registro añadido con exito, con el numero de orden: ' || v_orden.id_cliente);
        apex_json.open_object('datos');
        apex_json.write('id_orden',0);
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
            v_errorms := replace(sqlerrm,'"','''');
            apex_json.write('mensaje', 'Error #'||v_id_log||' al registrar la tabla temporal - ' || v_errorms);
            apex_json.close_object();
            s_json := apex_json.get_clob_output;
            apex_json.free_output;
    end prc_registro_ordenes_masivo_temporal;
    --//-->--------------------------- FIN    PKG_G_DANIEL_GONZALEZ_TEST Procedimientos Daniel
    
    --//-->--------------------------- INICIO PKG_G_DANIEL_GONZALEZ_TEST2 Procedimientos Daniel
    --//--Migrado a pkg_g_ordenes 31/01/2024
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
    --//-->--------------------------- FIN    PKG_G_DANIEL_GONZALEZ_TEST2 Procedimientos Daniel
    
    --//-->--------------------------- INICIO PKG_G_DANIEL_GONZALEZ_TEST3 Procedimientos Daniel
    --//--Migrado a pkg_g_ordenes 31/01/2024
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
    
    --//--Migrado a pkg_g_ordenes 31/01/2024
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
    --//-->--------------------------- FIN    PKG_G_DANIEL_GONZALEZ_TEST3 Procedimientos Daniel

end pkg_g_ordenes;