create or replace PACKAGE                            "AIRE".PKG_G_CARLOS_VARGAS_TEST12 AS

    procedure prc_cierre_ordenes_masivo(
        e_json  in 	clob,
        s_json  out	clob
    );

END PKG_G_CARLOS_VARGAS_TEST12;
/

create or replace PACKAGE BODY                        "AIRE".PKG_G_CARLOS_VARGAS_TEST12 AS

    procedure prc_cierre_ordenes_masivo(
        e_json  in 	clob,
        s_json  out	clob
    ) is
        v_id_soporte                number;
        v_id_usuario_registra       varchar2(100);
        v_nombre_archivo            varchar2(250);
        
        procedure sb_escribir_respuesta(codigo in varchar2, mensaje in varchar2)
        is begin
            s_json := '{"codigo":'|| codigo ||',"mensaje":"'|| mensaje ||'"}';
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
        

        /*
        -- recorremos los registros
        for i in 1..v_temp.count loop           
            sb_actualizar_temp(v_temp(i).numero_orden, '0', 'Ok');
        end loop;
        */

        update aire.ord_ordenes_cargue_temporal_cierre
        set DESC_VALIDACION = 'ok'
         where id_soporte       = v_id_soporte 
           and usuario_registra = v_id_usuario_registra;
        commit;

        update aire.ord_ordenes_cargue_temporal_cierre
        set con_errores = 1
         where id_soporte       = v_id_soporte 
           and usuario_registra = v_id_usuario_registra;
        commit;

        DECLARE  
            v_start_time DATE;
        BEGIN  v_start_time := SYSDATE;
            -- Bucle que espera hasta que hayan pasado 5 minutos  
            WHILE (SYSDATE - v_start_time) * 24 * 60 < 2 LOOP
            NULL; -- No hace nada
        END LOOP;
        
        -- Aquí puedes poner el resto de tu código
        END;

        
        /*delete aire.ord_ordenes_cargue_temporal_cierre
         where id_soporte       = v_id_soporte 
           and usuario_registra = v_id_usuario_registra;*/
        
        sb_escribir_respuesta(0, 'Archivo procesado');
    exception
        when others then            
            sb_escribir_respuesta(1, 'Error al procesar ordenes:'|| sqlerrm);
    end prc_cierre_ordenes_masivo;

END PKG_G_CARLOS_VARGAS_TEST12;