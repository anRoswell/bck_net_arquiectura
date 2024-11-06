    set SERVEROUTPUT on;
    declare
        v_notificacion                   json_object_t;
        v_respuesta_notificacion         aire.tip_respuesta;
        v_id_dispositivo                 aire.sgd_usuarios.id_dispositivo%type;
    begin
     
        -- consultamos el id del dispositivo
        select id_dispositivo
          into v_id_dispositivo
          from aire.sgd_usuarios 
         where id_persona = (select id_persona from aire.ctn_contratistas_persona where id_contratista_persona = 2554);
        
        v_notificacion := json_object_t();
        v_notificacion.put('id_dispositivo', v_id_dispositivo);
        v_notificacion.put('ind_android', true);        
        v_notificacion.put('titulo', 'Asignación ordenes SCR');
        v_notificacion.put('cuerpo', 'Se asignaron '||sql%rowcount||' ordenes a su gestión'); -- cuando es desasignación que diga Se desasignaron x ordenes a su gestión.
        v_notificacion.put('estado', 'asignada');
        v_notificacion.put('tipo', 'gestion_ordenes_scr');
        v_notificacion.put('fecha_envio', to_char(systimestamp, 'DD/MM/YYYY HH:MI:SS:FF PM'));
        -- Se envia notificacion en segundo plano
        aire.pkg_g_generales.prc_enviar_notificacion_push_segundo_plano( e_notificacion  => v_notificacion.to_clob, s_respuesta => v_respuesta_notificacion);
        dbms_output.put_line('v_respuesta.mensaje: ' || v_respuesta_notificacion.mensaje);
        dbms_output.put_line('v_id_dispositivo: ' || v_id_dispositivo);
    end;
    /