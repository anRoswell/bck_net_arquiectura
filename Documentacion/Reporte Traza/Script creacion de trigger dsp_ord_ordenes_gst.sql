SET SERVEROUTPUT ON;

create or replace trigger aire.dsp_ord_ordenes_gst2
    for insert or update or delete on aire.ord_ordenes
	compound trigger

		v_ordenes_historico			aire.ord_ordenes_historico%rowtype;
        v_id_log		            aire.gnl_logs.id_log%type;
        v_respuesta		aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Trigger ejecuado con exito.', nombre_up   => 'dsp_ord_ordenes_prueba_gst2');

    after each row is
        begin
        
            if inserting or updating then
            
                v_ordenes_historico.id_orden                            := :new.id_orden;
                v_ordenes_historico.numero_orden                        := :new.numero_orden;
                v_ordenes_historico.id_cliente                          := :new.id_cliente;
                v_ordenes_historico.id_tipo_orden                       := :new.id_tipo_orden;
                v_ordenes_historico.id_contratista_persona              := :new.id_contratista_persona;
                v_ordenes_historico.id_barrio                           := :new.id_barrio;
                v_ordenes_historico.id_estado_orden                     := :new.id_estado_orden;
                v_ordenes_historico.id_estado_orden                     := :new.id_estado_orden;
                v_ordenes_historico.id_contratista                      := :new.id_contratista;
                v_ordenes_historico.id_territorial                      := :new.id_territorial;
                v_ordenes_historico.id_tipo_suspencion                  := :new.id_tipo_suspencion;
                v_ordenes_historico.id_zona                             := :new.id_zona;
                v_ordenes_historico.direcion                            := :new.direcion;
                v_ordenes_historico.fecha_creacion                      := :new.fecha_creacion;
                v_ordenes_historico.fecha_cierre                        := :new.fecha_cierre;
                v_ordenes_historico.id_usuario_cierre                   := :new.id_usuario_cierre;
                v_ordenes_historico.descripcion                         := :new.descripcion;
                v_ordenes_historico.comentarios                         := :new.comentarios;
                v_ordenes_historico.acta                                := :new.acta;
                v_ordenes_historico.id_actividad                        := :new.id_actividad;
                v_ordenes_historico.id_tipo_trabajo                     := :new.id_tipo_trabajo;
                v_ordenes_historico.actividad_orden                     := :new.actividad_orden;
                v_ordenes_historico.fecha_estimada_respuesta            := :new.fecha_estimada_respuesta;
                v_ordenes_historico.numero_camp                         := :new.numero_camp;
                v_ordenes_historico.comentario_orden_servicio_num1      := :new.comentario_orden_servicio_num1;
                v_ordenes_historico.comentario_orden_servicio_num2      := :new.comentario_orden_servicio_num2;
                v_ordenes_historico.observacion_rechazo                 := :new.observacion_rechazo;
                v_ordenes_historico.fecha_rechazo                       := :new.fecha_rechazo;
                v_ordenes_historico.origen                              := :new.origen;
                v_ordenes_historico.fecha_registro                      := :new.fecha_registro;
                v_ordenes_historico.fecha_asigna_contratista            := :new.fecha_asigna_contratista;
                v_ordenes_historico.fecha_asigna_tecnico                := :new.fecha_asigna_tecnico;
                select aire.sec_ord_ordenes_historico.nextval into v_ordenes_historico.id_orden_historico from dual;
                v_ordenes_historico.fecha_registro_historico            := sysdate;
                
                --NIC
                begin
                    select nic into v_ordenes_historico.nic from aire.gnl_clientes where id_cliente = :new.id_cliente;
                    
                    exception
                        when others then
                            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al consultar el nic ' || sqlerrm);
                            v_ordenes_historico.nic := null;
                end;
                
                --Deuda   
                begin
                    select expired_balance into v_ordenes_historico.deuda from aire.ord_ordenes_dato_suministro where id_orden = :new.id_orden;
                    
                    exception
                        when others then
                            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al consultar la deuda ' || sqlerrm);
                            v_ordenes_historico.deuda := null;
                end;
                
                --Tecnico
                begin
                    select b.nombres || ' ' || b.apellidos as tecnico into v_ordenes_historico.tecnico from aire.ctn_contratistas_persona a
                    left join aire.gnl_personas b on a.id_persona = b.id_persona where a.id_contratista_persona = :new.id_contratista_persona;
                    
                    exception
                        when others then
                            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al consultar el tecnico ' || sqlerrm);
                            v_ordenes_historico.tecnico := null;
                end;
                
                --Departamento
                begin
                    select nombre into v_ordenes_historico.departamento from aire.gnl_clientes a left join aire.gnl_departamentos b on a.id_departamento = b.id_departamento
                    where id_cliente = :new.id_cliente;
                    
                    exception
                        when others then
                            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al consultar el departamento ' || sqlerrm);
                            v_ordenes_historico.departamento := null;
                end;
                
                --Municipio
                begin
                    select nombre into v_ordenes_historico.municipio from aire.gnl_clientes a left join aire.gnl_municipios b on a.id_municipio = b.id_municipio
                    where id_cliente = :new.id_cliente;
                    
                    exception
                        when others then
                            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al consultar el municipio ' || sqlerrm);
                            v_ordenes_historico.municipio := null;
                end;
                
                --Numero Factura
                begin
                    select expired_periodos into v_ordenes_historico.numero_factura from aire.ord_ordenes_dato_suministro where id_orden = :new.id_orden;
                    
                    exception
                        when others then
                            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al consultar el numero factura ' || sqlerrm);
                            v_ordenes_historico.numero_factura := null;
                end;
                
                --Tipo Brigada
                begin
                    select codigo into v_ordenes_historico.tipo_brigada from aire.ctn_contratistas_brigada a left join aire.ctn_tipos_brigada b
                    on a.id_tipo_brigada = b.id_tipo_brigada where id_contratista_persona = :new.id_contratista_persona;
                    
                    exception
                        when others then
                            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al consultar el tipo brigada ' || sqlerrm);
                            v_ordenes_historico.tipo_brigada := null;
                end;
                
                --v_ordenes_historico.fecha_consulta                      := localtimestamp;
                v_ordenes_historico.antiguedad                          := TRUNC(sysdate - TRUNC(:new.fecha_registro));
                
                if inserting then
                    v_ordenes_historico.estado_historico := 'Creado';
                    
                elsif updating then
                    v_ordenes_historico.estado_historico := 'Actualizado';
                end if;
                
                --Incersion de datos en la tabla aire.ord_ordenes_historico
                insert into aire.ord_ordenes_historico values v_ordenes_historico;

        end if;
    end after each row;
    
    before each row is
        begin
            if deleting then
                v_ordenes_historico.id_orden                            := :old.id_orden;
                v_ordenes_historico.numero_orden                        := :old.numero_orden;
                v_ordenes_historico.id_cliente                          := :old.id_cliente;
                v_ordenes_historico.id_tipo_orden                       := :old.id_tipo_orden;
                v_ordenes_historico.id_contratista_persona              := :old.id_contratista_persona;
                v_ordenes_historico.id_barrio                           := :old.id_barrio;
                v_ordenes_historico.id_estado_orden                     := :old.id_estado_orden;
                v_ordenes_historico.id_estado_orden                     := :old.id_estado_orden;
                v_ordenes_historico.id_contratista                      := :old.id_contratista;
                v_ordenes_historico.id_territorial                      := :old.id_territorial;
                v_ordenes_historico.id_tipo_suspencion                  := :old.id_tipo_suspencion;
                v_ordenes_historico.id_zona                             := :old.id_zona;
                v_ordenes_historico.direcion                            := :old.direcion;
                v_ordenes_historico.fecha_creacion                      := :old.fecha_creacion;
                v_ordenes_historico.fecha_cierre                        := :old.fecha_cierre;
                v_ordenes_historico.id_usuario_cierre                   := :old.id_usuario_cierre;
                v_ordenes_historico.descripcion                         := :old.descripcion;
                v_ordenes_historico.comentarios                         := :old.comentarios;
                v_ordenes_historico.acta                                := :old.acta;
                v_ordenes_historico.id_actividad                        := :old.id_actividad;
                v_ordenes_historico.id_tipo_trabajo                     := :old.id_tipo_trabajo;
                v_ordenes_historico.actividad_orden                     := :old.actividad_orden;
                v_ordenes_historico.fecha_estimada_respuesta            := :old.fecha_estimada_respuesta;
                v_ordenes_historico.numero_camp                         := :old.numero_camp;
                v_ordenes_historico.comentario_orden_servicio_num1      := :old.comentario_orden_servicio_num1;
                v_ordenes_historico.comentario_orden_servicio_num2      := :old.comentario_orden_servicio_num2;
                v_ordenes_historico.observacion_rechazo                 := :old.observacion_rechazo;
                v_ordenes_historico.fecha_rechazo                       := :old.fecha_rechazo;
                v_ordenes_historico.origen                              := :old.origen;
                v_ordenes_historico.fecha_registro                      := :old.fecha_registro;
                v_ordenes_historico.fecha_asigna_contratista            := :old.fecha_asigna_contratista;
                v_ordenes_historico.fecha_asigna_tecnico                := :old.fecha_asigna_tecnico;
                select aire.sec_ord_ordenes_historico.nextval into v_ordenes_historico.id_orden_historico from dual;
                v_ordenes_historico.fecha_registro_historico            := sysdate;
                
                --NIC
                begin
                
                    select nic into v_ordenes_historico.nic from aire.gnl_clientes where id_cliente = :old.id_cliente;
                    
                    exception
                        when others then
                            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al consultar el nic ' || sqlerrm);
                            v_ordenes_historico.nic := null;
                end;
                
                --Deuda
                begin
                    select expired_balance into v_ordenes_historico.deuda from aire.ord_ordenes_dato_suministro where id_orden = :old.id_orden;
                    
                    exception
                        when others then
                            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al consultar la deuda ' || sqlerrm);
                            v_ordenes_historico.deuda := null;
                end;
                
                --Tecnico
                begin
                    select b.nombres || ' ' || b.apellidos as tecnico into v_ordenes_historico.tecnico from aire.ctn_contratistas_persona a
                    left join aire.gnl_personas b on a.id_persona = b.id_persona where a.id_contratista_persona = :old.id_contratista_persona;
                    
                    exception
                        when others then
                            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al consultar el tecnico ' || sqlerrm);
                            v_ordenes_historico.tecnico := null;
                end;
                
                --Departamento
                begin
                    select nombre into v_ordenes_historico.departamento from aire.gnl_clientes a left join aire.gnl_departamentos b on a.id_departamento = b.id_departamento
                    where id_cliente = :old.id_cliente;
                    
                    exception
                        when others then
                            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al consultar el departamento ' || sqlerrm);
                            v_ordenes_historico.departamento := null;
                end;
                
                --Municipio
                begin
                    select nombre into v_ordenes_historico.municipio from aire.gnl_clientes a left join aire.gnl_municipios b on a.id_municipio = b.id_municipio
                    where id_cliente = :old.id_cliente;
                    
                    exception
                        when others then
                            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al consultar el municipio ' || sqlerrm);
                            v_ordenes_historico.municipio := null;
                end;
                
                --Numero Factura
                begin
                    select expired_periodos into v_ordenes_historico.numero_factura from aire.ord_ordenes_dato_suministro where id_orden = :old.id_orden;
                    
                    exception
                        when others then
                            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al consultar el numero factura ' || sqlerrm);
                            v_ordenes_historico.numero_factura := null;
                end;
                
                --Tipo Brigada
                begin
                    select codigo into v_ordenes_historico.tipo_brigada from aire.ctn_contratistas_brigada a left join aire.ctn_tipos_brigada b
                    on a.id_tipo_brigada = b.id_tipo_brigada where id_contratista_persona = :old.id_contratista_persona;
                    
                    exception
                        when others then
                            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al consultar el tipo brigada ' || sqlerrm);
                            v_ordenes_historico.tipo_brigada := null;
                end;
                
                --v_ordenes_historico.fecha_consulta                      := localtimestamp;
                v_ordenes_historico.antiguedad                          := TRUNC(sysdate - TRUNC(:old.fecha_registro));
                
                
                if deleting then
                    v_ordenes_historico.estado_historico := 'Eliminado';
                end if;
                
                --Incersion de datos en la tabla aire.ord_ordenes_historico
                insert into aire.ord_ordenes_historico values v_ordenes_historico;

        end if;
    end before each row;
    
end;
