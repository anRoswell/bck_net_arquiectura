CREATE OR REPLACE PACKAGE BODY "AIRE".PKG_G_DANIEL_GONZALEZ_TEST AS

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
    
    
    --nuevo
    procedure prc_resgistrar_ordenes_ws_osf (
        e_json  		in 	clob,
        s_json 	        out	clob
    ) is
        v_json_dato                 json_object_t;
        v_datos_sum                 json_object_t;
        v_actividades_array         json_array_t;
        v_actividades_json          json_object_t;
        v_precintos_array           json_array_t;
        v_precintos_json            json_object_t;
        v_recibos_array             json_array_t;
        v_recibos_json              json_object_t;
        v_apaconen_array            json_array_t;
        v_apaconen_json             json_object_t;
        v_id_log		            aire.gnl_logs.id_log%type;
        v_respuesta                 aire.tip_respuesta := aire.tip_respuesta(mensaje => 'Registro exitoso.', nombre_up => 'pkg_g_daniel_gonzalez_test.prc_resgistrar_ordenes_ws_osf');
        v_orden      	            aire.ord_ordenes%rowtype;
        v_datosum      	            aire.ord_ordenes_dato_suministro%rowtype;
        v_actividad      	        aire.ord_ordenes_actividad%rowtype;
        v_precinto      	        aire.ord_ordenes_precinto%rowtype;
        v_recibo      	            aire.ord_ordenes_recibo%rowtype;
        v_apaconen      	        aire.ord_ordenes_aparato_conexion%rowtype;
        v_errorms                   varchar2(4000);
        v_id_orden                  aire.ord_ordenes.id_orden%type;
        v_id_tipo_trabajo           aire.ord_tipos_trabajo.id_tipo_trabajo%type;
        v_id_tipo_orden             aire.ord_tipos_orden.id_tipo_orden%type;
        v_id_estado_orden           aire.ord_estados_orden.id_estado_orden%type;
        v_id_cliente                aire.gnl_clientes.id_cliente%type;
        v_id_actividad              aire.gnl_actividades.id_actividad%type;
        v_codigo                    aire.ord_tipos_trabajo.codigo%type;
        v_codigo_tipo_orden         aire.ord_tipos_orden.codigo_tipo_orden%type;
        v_nic                       aire.gnl_clientes.nic%type;
    begin
        -- validamos el Json de la orden y armamos el rowtype
        begin
            DBMS_OUTPUT.PUT_LINE('paso = 1');
            -- parseamos el json
            v_json_dato             := json_object_t(e_json);
            v_datos_sum             := v_json_dato.get_object('datosum');
            v_actividades_array     := v_json_dato.get_array('actividades');
            v_precintos_array       := v_json_dato.get_array('precintos');
            v_recibos_array         := v_json_dato.get_array('recibos');
            v_apaconen_array        := v_json_dato.get_array('apaconen');
            DBMS_OUTPUT.PUT_LINE('paso = 2');
            v_codigo                := v_json_dato.get_number('tipo_trabajo');
            v_codigo_tipo_orden     := v_json_dato.get_string('tip_os');
            v_nic                   := v_json_dato.get_number('nic');

            DBMS_OUTPUT.PUT_LINE('paso = 3');
            -- consultamos el tipo de trabajo
            select id_tipo_trabajo
            into v_id_tipo_trabajo
            from aire.ord_tipos_trabajo
            where codigo = v_codigo;
            
             -- consultamos el tipo orden
            select id_tipo_orden
            into v_id_tipo_orden
            from aire.ord_tipos_orden
            where codigo_tipo_orden = v_codigo_tipo_orden;
            
            -- consultamos el estado orden
            select id_estado_orden
            into v_id_estado_orden
            from aire.ord_estados_orden
            where codigo_estado = 'SPEN';
            
            -- consultamos el id cliente
            select id_cliente
            into v_id_cliente
            from aire.gnl_clientes
            where nic = v_nic;
            
            -- consultamos el id actividad
            select id_actividad
            into v_id_actividad
            from aire.gnl_actividades
            where nombre = 'SCR';
            
             
            -- extraemos los datos para la tabla ord_ordenes
            v_orden.id_orden                        := aire.sec_ord_ordenes.nextval;
            v_orden.id_tipo_orden                   := v_id_tipo_orden;
            v_orden.numero_orden                    := v_json_dato.get_number('num_os');
            v_orden.id_estado_orden                 := v_id_estado_orden;
            v_orden.id_cliente                      := v_id_cliente;
            v_orden.direcion                        := v_json_dato.get_string('direccion');
            v_orden.fecha_creacion                  := to_date(v_json_dato.get_string('f_gen'), 'YYYY-MM-DD');
            --v_orden.descripcion                     := v_json_dato.get_string('descripcion');
            --v_orden.comentarios                     := v_json_dato.get_string('comentarios');
            v_orden.id_actividad                    := v_id_actividad;
            v_orden.id_tipo_trabajo                 := v_id_tipo_trabajo;
            --v_orden.id_tipo_suspencion              := v_json_dato.get_number('TIPO_SUSPENSION'); no encontre esta tabla
            --v_orden.actividad_orden                 := v_json_dato.get_string('actividad_orden'); no me pasa este campo
            v_orden.fecha_estimada_respuesta        := to_date(v_json_dato.get_string('f_estm_rest'), 'YYYY-MM-DD');
            v_orden.numero_camp                     := v_json_dato.get_string('num_camp');
            v_orden.comentario_orden_servicio_num1  := v_json_dato.get_string('coment_os');
            v_orden.comentario_orden_servicio_num2  := v_json_dato.get_string('coment_os2');
            
            --Hacemos el insert en la tabla ord_ordenes
            aire.pkg_p_ordenes.prc_registrar_orden (
                e_orden	    => v_orden,
                s_respuesta	=> v_respuesta
            );
            
            if v_respuesta.codigo <> 0 then
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al registrar orden: '|| v_respuesta.mensaje);
                apex_json.initialize_clob_output( p_preserve => false );
                apex_json.open_object();
                apex_json.write('codigo', 1);
                apex_json.write('mensaje', 'Error #'||v_id_log||' al registrar la orden');
                apex_json.close_object();
                s_json := apex_json.get_clob_output;
                apex_json.free_output;
                return;
            end if;

            v_id_orden := v_orden.id_orden;

            -- extraemos los datos para la tabla ord_ordenes_dato_suministro
            v_datosum.id_orden_dato_suministro      := aire.sec_ord_ordenes_dato_suministro.nextval;
            v_datosum.id_orden                      := v_id_orden;
            v_datosum.tipo_servico                  := v_datos_sum.get_string('tip_serv');
            v_datosum.codigo_tarifa                 := v_datos_sum.get_string('cod_tar');
            v_datosum.tipo_conexion                 := v_datos_sum.get_number('tip_conexion');
            v_datosum.tipo_tension                  := v_datos_sum.get_string('tip_tension');
            v_datosum.potencia                      := v_datos_sum.get_number('pot');
            v_datosum.municipio                     := v_datos_sum.get_number('municipio');
            v_datosum.localidad                     := v_datos_sum.get_string('localidad');
            v_datosum.id_sector_operativo           := v_datos_sum.get_string('operating_sector_id');
            v_datosum.id_zona                       := v_datos_sum.get_number('zona_id');
            v_datosum.tipo_de_via                   := v_datos_sum.get_number('tip_via');
            v_datosum.calle                         := v_datos_sum.get_string('calle');
            v_datosum.numero_puerta                 := v_datos_sum.get_number('Num_Puerta');
            v_datosum.duplicador                    := v_datos_sum.get_string('duplicador');
            v_datosum.cgv_suministro                := v_datos_sum.get_string('cgv_sum');
            v_datosum.nombre_finca                  := v_datos_sum.get_string('nom_finca');
            v_datosum.referencia_dirreccion         := v_datos_sum.get_string('ref_dir');
            v_datosum.acceso_finca                  := v_datos_sum.get_string('acc_finca');
            v_datosum.primer_apellido_cliente       := v_datos_sum.get_string('ape1_cli');
            v_datosum.segundo_apellido_cliente      := v_datos_sum.get_string('ape2_cli');
            v_datosum.nombre_cliente                := v_datos_sum.get_string('nom_cli');
            v_datosum.telefono_cliente              := v_datos_sum.get_string('tfno_cli');
            v_datosum.tipo_cliente                  := v_datos_sum.get_number('tip_cli');
            v_datosum.tipo_medida                   := v_datos_sum.get_string('tipo_medida');
            v_datosum.unicon                        := v_datos_sum.get_number('unicon');
            v_datosum.nis                           := v_datos_sum.get_number('nis');
            v_datosum.circuito                      := v_datos_sum.get_string('circuito');
            v_datosum.trafo                         := v_datos_sum.get_string('trafo');
            v_datosum.factor_mult                   := v_datos_sum.get_number('factor_mult');
            v_datosum.estado_servicio               := v_datos_sum.get_string('estado_servicio');
            v_datosum.premise_id                    := v_datos_sum.get_number('premise_id');
            v_datosum.expired_balance               := v_datos_sum.get_number('expired_balance');
            v_datosum.expired_periodos              := v_datos_sum.get_number('expired_periodos');
            v_datosum.address_id                    := v_datos_sum.get_number('address_id');
            v_datosum.id_barrio                     := v_datos_sum.get_number('barrio_id');
            
            --Hacemos el insert en la tabla ord_ordenes_dato_suministro
            insert into aire.ord_ordenes_dato_suministro values v_datosum;
            
            --ciclo para recorrer array
            for i in 0 .. v_actividades_array.get_size - 1 loop
                begin
                    v_actividades_json := json_object_t(v_actividades_array.get(i));
                    
                    -- extraemos los datos para la tabla ord_ordenes_actividad
                    v_actividad.id_orden_actividad          := aire.sec_ord_ordenes_actividad.nextval;
                    v_actividad.id_orden                    := v_id_orden;
                    v_actividad.numero_actividad            := v_actividades_json.get_number('nro_actividad');
                    v_actividad.tipo_actividad              := v_actividades_json.get_number('tipo_actividad');
                    
                    --Hacemos el insert en la tabla ord_ordenes_actividad
                    insert into aire.ord_ordenes_actividad values v_actividad;
    
                    v_actividad := null;
                    
                exception
                    when others then 
                        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al procesar el json de la orden: '|| sqlerrm);
                        apex_json.initialize_clob_output( p_preserve => false );
                        apex_json.open_object();
                        apex_json.write('codigo', 400);
                        v_errorms := replace(sqlerrm,'"','''');
                        apex_json.write('mensaje', 'Error #'||v_id_log||' al alimentar la tabla ord_ordenes_actividad - '|| v_errorms);
                        apex_json.close_object();
                        s_json := apex_json.get_clob_output;
                        apex_json.free_output;
                        return;
                end;
            end loop;
            
            
            --ciclo para recorrer array
            for i in 0 .. v_precintos_array.get_size - 1 loop
                begin
                    v_precintos_json := json_object_t(v_precintos_array.get(i));
                    
                    -- extraemos los datos para la tabla ord_ordenes_precinto
                    v_precinto.id_orden_precinto            := aire.sec_ord_ordenes_precinto.nextval;
                    v_precinto.id_orden                     := v_id_orden;
                    v_precinto.codigo_marca                 := v_precintos_json.get_string('cod_marca');
                    v_precinto.numero_sello                 := v_precintos_json.get_string('num_precin');
                    
                    --Hacemos el insert en la tabla ord_ordenes_precinto
                    insert into aire.ord_ordenes_precinto values v_precinto;
    
                    v_precinto := null;
                exception
                    when others then 
                        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al procesar el json de la orden: '|| sqlerrm);
                        apex_json.initialize_clob_output( p_preserve => false );
                        apex_json.open_object();
                        apex_json.write('codigo', 400);
                        v_errorms := replace(sqlerrm,'"','''');
                        apex_json.write('mensaje', 'Error #'||v_id_log||' al alimentar la tabla ord_ordenes_precinto - '|| v_errorms);
                        apex_json.close_object();
                        s_json := apex_json.get_clob_output;
                        apex_json.free_output;
                        return;
                end;
            end loop;
            
            
            --ciclo para recorrer array
            for i in 0 .. v_recibos_array.get_size - 1 loop
                begin
                    v_recibos_json := json_object_t(v_recibos_array.get(i));
                    
                    -- extraemos los datos para la tabla ord_ordenes_recibo
                    v_recibo.id_orden_recibo                := aire.sec_ord_ordenes_recibo.nextval;
                    v_recibo.id_orden                       := v_id_orden;
                    v_recibo.simbolo_variable               := v_recibos_json.get_number('simbolo_var');
                    v_recibo.fecha_factura                  := to_date(v_recibos_json.get_string('f_fact'), 'DD-MM-YYYY');
                    v_recibo.fecha_vencimiento_factura      := to_date(v_recibos_json.get_string('f_vcto_fact'), 'DD-MM-YYYY');
                    v_recibo.importe_total_recibo           := v_recibos_json.get_number('imp_tot_rec');
                    
                    --Hacemos el insert en la tabla ord_ordenes_recibo
                    insert into aire.ord_ordenes_recibo values v_recibo;
    
                    v_recibo := null;
                exception
                    when others then 
                        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al procesar el json de la orden: '|| sqlerrm);
                        apex_json.initialize_clob_output( p_preserve => false );
                        apex_json.open_object();
                        apex_json.write('codigo', 400);
                        v_errorms := replace(sqlerrm,'"','''');
                        apex_json.write('mensaje', 'Error #'||v_id_log||' al alimentar la tabla ord_ordenes_recibo - '|| v_errorms);
                        apex_json.close_object();
                        s_json := apex_json.get_clob_output;
                        apex_json.free_output;
                        return;
                end;
            end loop;
            
            
            --ciclo para recorrer array
            for i in 0 .. v_apaconen_array.get_size - 1 loop
                begin
                    v_apaconen_json := json_object_t(v_apaconen_array.get(i));
                    
                    -- extraemos los datos para la tabla ord_ordenes_aparato_conexion
                    v_apaconen.id_orden_aparato_conexion    := aire.sec_ord_ordenes_aparato_conexion.nextval;
                    v_apaconen.id_orden                     := v_id_orden;
                    v_apaconen.codigo_marca                 := v_apaconen_json.get_string('cod_marca');
                    v_apaconen.numero_aparato               := v_apaconen_json.get_string('num_apa');
                    v_apaconen.tipo_consumo                 := v_apaconen_json.get_string('tip_csmo');
                    v_apaconen.numero_ruedas                := v_apaconen_json.get_string('num_rue');
                    v_apaconen.consumo                      := v_apaconen_json.get_string('csmo');
                    v_apaconen.lectura                      := v_apaconen_json.get_string('lect');
                    v_apaconen.fecha_lectura                := to_date(v_apaconen_json.get_string('f_lect'), 'YYYY-MM-DD');
                    v_apaconen.tipo_aparato                 := v_apaconen_json.get_string('tip_apa');
                    v_apaconen.tipo_intensidad              := v_apaconen_json.get_string('tip_intensidad');
                    v_apaconen.tipo_fase                    := v_apaconen_json.get_string('tip_fase');
                    v_apaconen.nivel_tension                := v_apaconen_json.get_string('nivel_tension');
                    v_apaconen.propiedad_activo             := v_apaconen_json.get_string('propiedad_activo');
                    
                    --Hacemos el insert en la tabla ord_ordenes_aparato_conexion
                    insert into aire.ord_ordenes_aparato_conexion values  v_apaconen;
    
                    v_apaconen := null;
                exception
                    when others then 
                        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al procesar el json de la orden: '|| sqlerrm);
                        apex_json.initialize_clob_output( p_preserve => false );
                        apex_json.open_object();
                        apex_json.write('codigo', 400);
                        v_errorms := replace(sqlerrm,'"','''');
                        apex_json.write('mensaje', 'Error #'||v_id_log||' al alimentar la tabla ord_ordenes_aparato_conexion - '|| v_errorms);
                        apex_json.close_object();
                        s_json := apex_json.get_clob_output;
                        apex_json.free_output;
                        return;
                end;
            end loop;

  
            
        exception
            when others then 
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al procesar el json de la orden: '|| sqlerrm);
                apex_json.initialize_clob_output( p_preserve => false );
                apex_json.open_object();
                apex_json.write('codigo', 400);
                v_errorms := replace(sqlerrm,'"','''');
                apex_json.write('mensaje', 'Error #'||v_id_log||' al alimentar la tabla - '|| v_errorms);
                apex_json.close_object();
                s_json := apex_json.get_clob_output;
                apex_json.free_output;
                return;
        end;
        
            apex_json.initialize_clob_output( p_preserve => false );
            apex_json.open_object();
            apex_json.write('codigo', 200);
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
                apex_json.write('mensaje', 'Error #'||v_id_log||' al ingresar datos en la tabla - ' || v_errorms);
                apex_json.close_object();
                s_json := apex_json.get_clob_output;
                apex_json.free_output;
    
        end prc_resgistrar_ordenes_ws_osf;

END PKG_G_DANIEL_GONZALEZ_TEST;