CREATE OR REPLACE PACKAGE "AIRE".PKG_G_CARLOS_VARGAS_TEST6_Casa AS    
    /*
    este sp sobreescribe pgk_g_ordenes
    */
    procedure prc_registrar_orden2 (		
        e_json  in 	clob,
        s_json 	out	clob
	);

    /*
    este NO sobreescribe sp en pgk_g_ordenes
    debido a que se crea uno nuevo, la versión V2
    */
    procedure prc_registro_ordenes_masivo_final_V2 (		
        e_json      in clob,
        s_json      out clob
	);

    /*
    este sp sobreescribe pgk_g_ordenes
    */
    procedure prc_parametros_iniciales_contratistas (		        
        e_json 	in	clob,
        s_json 	out	clob
	);

END PKG_G_CARLOS_VARGAS_TEST6_Casa;
/
CREATE OR REPLACE PACKAGE BODY "AIRE".PKG_G_CARLOS_VARGAS_TEST6_Casa AS

    --/-- se migro a pkg_g_ordenes el 22/03/2024
    procedure prc_registrar_orden2 (
        e_json  		in 	clob,
        s_json 	out	clob
    ) is
        v_json_orden                json_object_t;
        v_id_log		            aire.gnl_logs.id_log%type;
        v_respuesta                 aire.tip_respuesta := aire.tip_respuesta(mensaje => 'Registro exitoso.', nombre_up => 'pkg_g_ordenes.prc_registrar_orden2');
        v_orden      	            aire.ord_ordenes%rowtype;
        v_id_contratista            aire.ord_ordenes.id_contratista%type;
        v_estado_orden              aire.ord_estados_orden.descripcion%type;
        v_cliente                   aire.gnl_clientes%rowtype;
        valor_nic                   number;
        valor_nis                   number;
        valor_zona                  number;
        valor_id_estado_servicio    number;
        v_id_actividad              aire.gnl_actividades.id_actividad%type;
        v_mje                       varchar2(4000) := 'Orden creada con éxito.';
        v_exist_contratista         number;
        v_exist_zona                number;
    begin
        -- validamos el Json de la orden y armamos el rowtype
        begin
            -- parseamos el json
            v_json_orden := json_object_t(e_json);
    
            -- extraemos los datos de la orden
            v_cliente.nic                               := v_json_orden.get_number('nic');
            v_cliente.nis                               := v_json_orden.get_number('nis');
            valor_nic                                   := v_cliente.nic;
            valor_nis                                   := v_cliente.nis;

            v_orden.id_tipo_orden                       := v_json_orden.get_number('id_tipo_orden');
            valor_id_estado_servicio                    := v_json_orden.get_number('id_estado_servicio');
            v_orden.id_tipo_suspencion                  := v_json_orden.get_number('id_tipo_suspencion');
            v_id_contratista                            := v_json_orden.get_number('contratista');
            v_orden.comentario_orden_servicio_num1      := v_json_orden.get_string('comentario1');
            v_orden.comentario_orden_servicio_num2      := v_json_orden.get_string('comentario2');
            v_orden.numero_camp                         := v_json_orden.get_string('num_camp');
                        
            --se valida que exista el contratista y que sea de SCR.
            if v_id_contratista > 0 then
                select
                     count(*)
                        into v_exist_contratista
                from aire.v_ctn_contratistas  x
                where UPPER(x.ind_activo) = 'S' and
                      x.id_contratista in (
                        select y.id_contratista
                            from aire.v_ctn_contratos y
                            where UPPER(y.prefijo_actividad) = 'G_SCR'
                            and UPPER(y.ind_activo) = 'S'
                ) and x.id_contratista = v_id_contratista;
                
                -- Retornar error
                if v_exist_contratista = 0 then
                    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_p_generales.v_tipo_mensaje_error, 'Error el contratista no existe: ');
                    apex_json.initialize_clob_output( p_preserve => false );
                    apex_json.open_object();
                    apex_json.write('codigo', 1);
                    apex_json.write('mensaje', 'Error #'||v_id_log||' el contratista no existe por cuanto no se crea ninguna orden');
                    apex_json.close_object();
                    s_json := apex_json.get_clob_output;
                    apex_json.free_output;
                    return;
                end if;
                -- validar que la zona del cliente este en la zona del contratista
                if v_exist_contratista != 0 then
                    --obtener la zona del cliente
                    select
                        id_zona
                            into valor_zona
                    from aire.gnl_clientes
                    where nic = valor_nic;
                    
                    -- obtener zonas asignadas al contratista por contrato y cruzarla con la zona del cliente
                    select count(*)
                            into v_exist_zona
                    from aire.gnl_zonas z
                    where z.id_zona in (
                            select id_zona
                            from aire.ctn_contratos_zona x
                            where x.id_contrato in (
                                        select y.id_contrato
                                        from aire.v_ctn_contratos y
                                        where UPPER(y.prefijo_actividad) = 'G_SCR'
                                              and UPPER(y.ind_activo) = 'S'
                                              and y.id_contratista    = v_id_contratista
                                        )
                        ) and z.id_zona = valor_zona;
                    
                    -- Retornar error
                    if v_exist_zona = 0 then
                        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_p_generales.v_tipo_mensaje_error, 'Error el contratista no existe: ');
                        apex_json.initialize_clob_output( p_preserve => false );
                        apex_json.open_object();
                        apex_json.write('codigo', 1);
                        apex_json.write('mensaje', 'Error #'||v_id_log||' la zona del cliente no existe en las zonas asignadas al contratista.');
                        apex_json.close_object();
                        s_json := apex_json.get_clob_output;
                        apex_json.free_output;
                        return;
                    end if;
                end if;
            end if;
            
            select
                aire.sec__ord_ordenes_cargue_temporal__numero_orden.NEXTVAL * -1
                    into v_orden.numero_orden
            from dual;
            
            select id_actividad
                    into v_id_actividad
            from aire.gnl_actividades where prefijo = 'G_SCR' and ind_activo = 'S';
            
            /*
            24-01-2024
            Se obtiene el valor del estado asignar
            12-03-2024
            ahora se añade el id contratista, si viene en el json entonces
            se utiliza en estado: Asignada a Contratista
            */
            select
                id_estado_orden,descripcion
                    into v_orden.id_estado_orden, v_estado_orden
            from aire.ord_estados_orden
            where   codigo_estado = case when v_id_contratista = 0 then 'SPEN' else 'SASI' end
            and     id_actividad  = v_id_actividad;
            
            
            select
                 id_cliente
                ,id_territorial
                ,id_zona
                ,direccion
                ,id_barrio
                into
                     v_orden.id_cliente
                    ,v_orden.id_territorial
                    ,v_orden.id_zona
                    ,v_orden.direcion
                    ,v_orden.id_barrio
            from aire.gnl_clientes
            where nic = valor_nic;
            
            
            --Se realiza insert a tabla aire.ord_ordenes
            insert into aire.ord_ordenes(
                id_tipo_orden
                ,id_cliente
                ,id_estado_orden
                ,numero_orden
                ,id_actividad
                ,id_tipo_suspencion
                ,id_territorial
                ,id_zona
                ,direcion
                ,origen
                ,id_contratista
                ,numero_camp
                ,comentario_orden_servicio_num1
                ,comentario_orden_servicio_num2
                ,id_barrio
            )
            values(
                 v_orden.id_tipo_orden
                ,v_orden.id_cliente
                ,v_orden.id_estado_orden
                ,v_orden.numero_orden
                ,v_id_actividad
                ,v_orden.id_tipo_suspencion
                ,v_orden.id_territorial
                ,v_orden.id_zona
                ,v_orden.direcion
                ,'OP360'
                ,case when v_id_contratista = 0 then null else v_id_contratista end
                ,v_orden.numero_camp
                ,v_orden.comentario_orden_servicio_num1
                ,v_orden.comentario_orden_servicio_num2
                ,v_orden.id_barrio
            );
            commit;
            
            select
                id_orden
                into v_orden.id_orden
            from aire.ord_ordenes
            where numero_orden = v_orden.numero_orden;
        exception
            when others then

                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_p_generales.v_tipo_mensaje_error, 'Error al procesar el json de la orden: '|| sqlerrm);
                apex_json.initialize_clob_output( p_preserve => false );
                apex_json.open_object();
                apex_json.write('codigo', 1);
                apex_json.write('mensaje', 'Error #'||v_id_log||' al registrar orden'|| sqlerrm);
                apex_json.close_object();
                s_json := apex_json.get_clob_output;
                apex_json.free_output;
                commit;
                return;
        end;
        
        if v_id_contratista > 0 then
                v_mje := 'Registro creado correctamente con el id_orden: ' || v_orden.id_orden || ', Se asigno al contratista: ' || v_id_contratista || ', y la orden quedo en estado: ' || v_estado_orden;
            else
                v_mje := 'Registro creado correctamente con el id_orden: ' || v_orden.id_orden || ', no se asigno a ningun contratista! , y la orden quedo en estado: ' || v_estado_orden;
        end if;

        apex_json.initialize_clob_output( p_preserve => false );
        apex_json.open_object();
        apex_json.write('codigo', v_orden.id_orden);
        apex_json.write('mensaje', v_mje);
        apex_json.open_object('datos');
        apex_json.write('id_orden',v_orden.id_orden);
        apex_json.close_object();
        apex_json.close_object();
        s_json := apex_json.get_clob_output;
        apex_json.free_output;

    exception
        when others then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_p_generales.v_tipo_mensaje_error, 'Error al registrar orden: '|| sqlerrm);
            apex_json.initialize_clob_output( p_preserve => false );
            apex_json.open_object();
            apex_json.write('codigo', 1);
            apex_json.write('mensaje', 'Error #'||v_id_log||' al registrar orden' || sqlerrm);
            apex_json.close_object();
            s_json := apex_json.get_clob_output;
            apex_json.free_output;
            commit;
    end prc_registrar_orden2;

    --/- se migro a pkg_p_ordenes el 22/03/2024
    procedure prc_registro_ordenes_masivo_final_V2 (
        e_json  		in 	clob,
        s_json 	        out	clob
    ) is
        v_json_orden                json_object_t;
        v_id_log		            aire.gnl_logs.id_log%type;
        v_respuesta                 aire.tip_respuesta := aire.tip_respuesta(mensaje => 'Registro exitoso.', nombre_up => 'pkg_g_ordenes.prc_registro_ordenes_masivo_final');
        
        v_id_soporte                NUMBER;
        v_usuario_registra          VARCHAR2(50 BYTE);
        v_nombre_archivo            VARCHAR2(100 BYTE);
        
        --v_numero_orden              NUMBER;
        v_id_archivo                NUMBER;
        v_id_actividad              NUMBER;
        v_id_archivo_instancia      NUMBER;
        v_fechainicio               timestamp;
        v_id_estado_orden_spen      NUMBER;
        v_id_estado_orden_sasi      NUMBER;
        -- Declarar la tabla temporal
        l_num_list                  num_list_type := num_list_type();
        v_id_contratista            aire.ord_ordenes.id_contratista%type;
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
            
            --actualizar tabla temporal  aire.ord_ordenes_cargue_temporal
            UPDATE aire.ord_ordenes_cargue_temporal
            SET Numero_Orden = aire.sec__ord_ordenes_cargue_temporal__numero_orden.NEXTVAL * -1
            where Numero_Orden is null and id_soporte = v_id_soporte and usuario_registra = v_usuario_registra;

            --identificar los nic repetidos.
            UPDATE aire.ord_ordenes_cargue_temporal
            SET nic_repetido = 1
            WHERE ROWID IN (
                SELECT ROWID
                FROM (
                    SELECT nic, ROW_NUMBER() OVER (PARTITION BY id_soporte,usuario_registra, nic ORDER BY id_soporte,usuario_registra, orden) AS rn
                    FROM aire.ord_ordenes_cargue_temporal
                    where id_soporte = v_id_soporte and usuario_registra = v_usuario_registra
                ) WHERE rn > 1
            ) and id_soporte = v_id_soporte and usuario_registra = v_usuario_registra;

            --se actualiza el campo de id_contratista a partir del codigo_contratista
            UPDATE aire.ord_ordenes_cargue_temporal x
            SET x.id_contratista = (SELECT y.id_contratista
                                    FROM aire.ctn_contratistas y
                                    WHERE y.codigo = x.codigo_contratista)
            WHERE x.id_contratista IS NULL
                AND x.id_soporte = v_id_soporte
                AND x.usuario_registra = v_usuario_registra;

            --se actualiza el campo de id_zona a aprtir del nic
            UPDATE aire.ord_ordenes_cargue_temporal x
            SET x.id_zona = (SELECT y.id_zona
                                    FROM aire.gnl_clientes y
                                    WHERE y.nic = x.nic)
            WHERE x.id_zona IS NULL
                AND x.id_soporte = v_id_soporte
                AND x.usuario_registra = v_usuario_registra;

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
            where id_soporte = v_id_soporte and usuario_registra = v_usuario_registra;
            
            -- se estraen los contratistas que tienen contrato vigente
            SELECT
                x.id_contratista
                    BULK COLLECT INTO l_num_list
            FROM aire.v_ctn_contratistas x
            WHERE
                UPPER(x.ind_activo) = 'S' and
                x.id_contratista in
                (
                    select y.id_contratista
                    from aire.v_ctn_contratos y
                    where UPPER(y.prefijo_actividad) = 'G_SCR'
                    and UPPER(y.ind_activo) = 'S'
               );
        exception
            when others then
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_p_generales.v_tipo_mensaje_error, 'Error al procesar el json de la orden: '|| sqlerrm);
                apex_json.initialize_clob_output( p_preserve => false );
                apex_json.open_object();
                apex_json.write('codigo', 400);
                apex_json.write('mensaje', 'Error #'||v_id_log||' al registrar las ordenes masivas'|| sqlerrm);
                apex_json.close_object();
                s_json := apex_json.get_clob_output;
                apex_json.free_output;
                return;
        end;
        --Se obtine el listado de contratistas con contrato.
        --ACA EL CODIGO PRINCIPAL
            MERGE
            INTO    aire.ord_ordenes_cargue_temporal trg
            USING   (
                    SELECT
                         x.Numero_Orden
                         
                        ,x.nic
                        --//--validar si el nic es numerico
                        ,case when REGEXP_LIKE(x.nic, '^[[:digit:]]+$') then 0 else 1 end                                       vnic
                        ,(select nvl(sum(0),1) from aire.gnl_clientes where nic = x.nic)                                        enic
                        
                        ,x.codigo_tipo_orden
                        --//--validar si el tipo orden es texto
                        ,case when REGEXP_LIKE(x.codigo_tipo_orden, '^[[:digit:]]+$') then 1 else 0 end                         vtor
                        ,(select nvl(sum(0),1) from aire.ord_tipos_orden where codigo_tipo_orden = x.codigo_tipo_orden)         etor
                        
                        ,x.codigo_tipo_suspencion
                        --//--validar si el tipo suspensión es texto
                        ,case when REGEXP_LIKE(x.codigo_tipo_suspencion, '^[[:digit:]]+$') then 1 else 0 end                    vtsu
                        ,(select nvl(sum(0),1) from aire.scr_tipos_suspencion where codigo = x.codigo_tipo_suspencion)          etsu
                        
                        ,x.codigo_estado_servicio
                        --//--validar si el tipo suspensión es texto
                        ,case when REGEXP_LIKE(x.codigo_estado_servicio, '^[[:digit:]]+$') then 1 else 0 end                    vtes
                        ,(select nvl(sum(0),1) from aire.gnl_estados_servicio where codigo = x.codigo_estado_servicio)          etes

                        --//--validar si existe el contratista.
                        ,case
                            when x.codigo_contratista is null then 0
                            when (
                                    select case when nvl(x.id_contratista,-1) = -1 then -1 else 1 end
                                    from aire.v_ctn_contratistas y
                                    where y.id_contratista = x.id_contratista and
                                        y.id_contratista in (select COLUMN_VALUE from TABLE(l_num_list))
                                ) = -1 then 1
                            else 0
                         end                                                                                                   eCnt
                        ,case
                            when x.codigo_contratista is null then 0
                            when nvl((
                                select case when nvl(x.id_contratista,-1) = -1 then -1 else 1 end
                                from aire.gnl_zonas z
                                where z.id_zona in (
                                        select id_zona
                                        from aire.ctn_contratos_zona n
                                        where n.id_contrato in (
                                                        select y.id_contrato
                                                        from aire.v_ctn_contratos y
                                                        where UPPER(y.prefijo_actividad) = 'G_SCR'
                                                        and UPPER(y.ind_activo) = 'S'
                                                        and y.id_contratista = x.id_contratista and y.id_contratista in (select COLUMN_VALUE from TABLE(l_num_list))
                                                    )
                                    ) and z.id_zona = x.id_zona
                                ),-1) = -1 then 1
                            else 0
                        end                                                                                                    eZna
                        ,case when x.nic_repetido = 1 then 1 else 0 end                                                        nRpt
                    FROM aire.ord_ordenes_cargue_temporal x
                    WHERE x.id_soporte = v_id_soporte and x.usuario_registra = v_usuario_registra
                    --WHERE x.id_soporte = 1720 and x.usuario_registra = '362'
                    order by 1
                    ) src
            ON      (trg.Numero_Orden = src.Numero_Orden)
            WHEN MATCHED THEN UPDATE
                SET
                    trg.con_errores = case when ((vnic + vtor) + (enic + etor) + (vtsu + etsu) + (vtes + etes) + eCnt + eZna + nRpt) = 0 then 0 else 1 end,
                    trg.desc_validacion =
                                case when ((vnic + vtor) + (enic + etor) + (vtsu + etsu) + (vtes + etes) + eCnt + eZna + nRpt) <= 0 then 'Validación Exitosa' end ||
                                case when (vnic + vtor + vtsu + vtes) > 0 then '-Tipo de dato incorrecto para: ' end    || case when src.vnic = 1 then '*-nic' end
                                                                                                                        || case when src.vtor = 1 then ' *-tipo_orden' end
                                                                                                                        || case when src.vtsu = 1 then ' *-tipo_suspencion' end
                                                                                                                        || case when src.vtes = 1 then ' *-estado_servicio' end
                                || case when (enic + etor + etsu + etes + eCnt + eZna + nRpt) > 0 then ' -otras validaciones: ' end
                                                                                                                                    || case when src.enic = 1 then ' *-No se encontraron registros para nic ' end
                                                                                                                                    || case when src.etor = 1 then ' *-No se encontraron registros para tipo_orden ' end
                                                                                                                                    || case when src.etsu = 1 then ' *-No se encontraron registros para tipo_suspencion ' end
                                                                                                                                    || case when src.etes = 1 then ' *-No se encontraron registros para estado_servicio ' end
                                                                                                                                    || case when src.eCnt = 1 then ' *-No se encontraron registros para codigo contratista ' end
                                                                                                                                    || case when src.nRpt = 1 then ' *-El nic se encuentra repetido ' end
                                                                                                                                    || case when src.eZna = 1 then ' *-la zona del cliente no esta soportada por las zonas del contratista ' end;
                                
        
        /*
        16-03-2024
        Se obtiene el valor del estado Pendiente
        */
        SELECT
            id_estado_orden into v_id_estado_orden_spen
        FROM aire.ord_estados_orden
        where   codigo_estado = 'SPEN'
        and     id_actividad IN (select id_actividad from aire.gnl_actividades where prefijo = 'G_SCR' and ind_activo = 'S');

        /*
        16-03-2024
        Se obtiene el valor del estado Asignada Contratista
        */
        SELECT
            id_estado_orden into v_id_estado_orden_sasi
        FROM aire.ord_estados_orden
        where   codigo_estado = 'SASI'
        and     id_actividad IN (select id_actividad from aire.gnl_actividades where prefijo = 'G_SCR' and ind_activo = 'S');
        
        --Crear registros de ordenes masivamente.
            insert into aire.ord_ordenes
            (
                id_tipo_orden
                ,id_cliente
                ,id_estado_orden
                ,numero_orden
                ,id_actividad
                ,id_tipo_suspencion
                ,origen
                ,id_territorial
                ,id_zona
                ,direcion
                ,id_contratista
                ,numero_camp
                ,comentario_orden_servicio_num1
                ,comentario_orden_servicio_num2
            )
            select
                 --x.id_tipo_orden
                 t.id_tipo_orden
                ,c.id_cliente
                ,case when x.id_contratista is null then v_id_estado_orden_spen else v_id_estado_orden_sasi end as id_estado_orden
                ,x.numero_orden as numero_orden
                ,v_id_actividad
                --,x.id_tipo_suspencion
                ,s.id_tipo_suspencion
                ,'OP360'
                ,c.id_territorial
                ,c.id_zona
                ,c.direccion
                ,x.id_contratista
                ,x.num_camp
                ,x.coment_os
                ,x.coment_os2
            from aire.ord_ordenes_cargue_temporal x
            left join aire.v_ctn_contratistas d on d.CODIGO = x.ID_CONTRATISTA
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
                ,localtimestamp fecha_fin_cargue
                ,'0' duracion
                ,v_usuario_registra id_usuario_registro
                ,localtimestamp fecha_registro
                ,163 id_estado_intancia --Finalizado
                ,'se cargaron ' || SUM(case when con_errores = 0 then 1 else 0 end) || ' ordenes.' observaciones
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
        
        --se eliminan los registros de la tabla temporal aire.ord_ordenes_cargue_temporal
            -- DELETE aire.ord_ordenes_cargue_temporal
            -- WHERE id_soporte = v_id_soporte and usuario_registra = v_usuario_registra;
        
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
    end prc_registro_ordenes_masivo_final_V2;

    --/-- se migro desde PKG_G_CARLOS_VARGAS_TEST6_Casa el 22/03/2024 11:20
    procedure prc_parametros_iniciales_contratistas (
        e_json 	in	clob,
        s_json 	out	clob
    ) is
        
        v_json_objeto       json_object_t;
        v_id_actividad      NUMBER(3,0);
        v_id_contratista    VARCHAR2(100 BYTE);
        v_id_persona    VARCHAR2(50 BYTE);
        v_id_usuario    VARCHAR2(50 BYTE);
        v_id_log		aire.gnl_logs.id_log%type;
        v_respuesta		aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Se cargaron los parametros iniciales del contratista con exito.', nombre_up   => 'pkg_g_ordenes.prc_parametros_iniciales_contratistas');
    BEGIN
        -- consultamos el id de la actividad de SCR
        begin
            select id_actividad
              into v_id_actividad
              from aire.gnl_actividades
             where prefijo = 'G_SCR' 
               and ind_activo = 'S';
        exception
            when others then
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Error al consultar la actividad de SCR: '||sqlerrm);
                apex_json.free_output;
                apex_json.open_object();
                apex_json.write('codigo', 1);
                apex_json.write('mensaje', 'Error N '||v_id_log ||' al registrar cuestionario: no se encuentra parametrizda la actividad SCR');
                apex_json.close_object();
                s_json := apex_json.get_clob_output;
                apex_json.free_output;
        end;
        
        --LOG
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
        
        -- Deserializar JSON
        apex_json.free_output;
        apex_json.initialize_clob_output( p_preserve => false );
        
        v_json_objeto           := json_object_t(e_json);
        -- Ahora llega el id persona del analista del contratista y se halla a partir del mismo el contratista.
        v_id_persona        := v_json_objeto.get_string('id_persona');
        v_id_usuario        := v_json_objeto.get_string('id_usuario');
        apex_json.free_output;

        begin
            select nvl(a.id_contratista,'-1') 
              into v_id_contratista
              from aire.ctn_contratistas_persona		a
             where a.id_contratista in (select id_contratista
                                          from aire.v_ctn_contratos
                                         where prefijo_actividad        = 'G_SCR'
                                           and ind_activo               = 'S')
               and a.id_persona     = v_id_persona 
               and a.codigo_rol     = 'ANALISTA'
               and a.ind_activo     = 'S';
        exception
            when no_data_found then
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'No se encontro un analista para el contratista ' || sqlerrm);
                apex_json.free_output;
                apex_json.open_object();
                apex_json.write('codigo', 1);
                apex_json.write('mensaje', 'Error No. '|| v_id_log || '. No se encontro un analista para el contratista');
                apex_json.close_object();
                s_json := apex_json.get_clob_output;
                apex_json.free_output;
            when others then
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Error al consultar el analista del contratista: '||sqlerrm);
                apex_json.free_output;
                apex_json.open_object();
                apex_json.write('codigo', 1);
                apex_json.write('mensaje', 'Error No. '|| v_id_log || '. Error al consultar el analista del contratista');
                apex_json.close_object();
                s_json := apex_json.get_clob_output;
                apex_json.free_output;
        end;
        
        
        apex_json.open_object();
        apex_json.write('codigo', 0);
        apex_json.write('mensaje', 'Carga exitosa');
        apex_json.open_object('datos');
        
        -- Tipos de orden
        for c_datos in ( select id_contratista 
                              , identificacion identificacion_contratista
                              , nombre_completo nombre_contratista 
                           from aire.v_ctn_contratistas 
                          where id_contratista = v_id_contratista
                            and ind_activo = 'S') loop
            apex_json.write('id_contratista', c_datos.id_contratista);
            apex_json.write('identificacion_contratista', c_datos.identificacion_contratista);
            apex_json.write('nombre_contratista',c_datos.nombre_contratista);
            
            -- Brigadas            
            apex_json.open_array('Brigadas');
            for x_datos in (select a.id_contratista_brigada
                                 , a.id_contratista_persona
                                 , a.identificacion_contratista_persona
                                 , a.nombre_contratista_persona
                                 , a.id_contratista_vehiculo
                                 , a.placa
                                 , a.id_tipo_brigada
                                 , a.codigo_tipo_brigada
                                 , a.tipo_brigada
                              from aire.v_ctn_contratistas_brigada a
                             where a.id_contratista = c_datos.id_contratista
                               and a.ind_activo     = 'S'
                               and codigo_rol       = 'TECNICO'
            ) loop
                apex_json.open_object();
                apex_json.write('id_contratista_brigada', x_datos.id_contratista_brigada);
                apex_json.write('id_contratista_persona', x_datos.id_contratista_persona);
                apex_json.write('identificacion_contratista_persona', x_datos.identificacion_contratista_persona);
                apex_json.write('nombre_contratista_persona', x_datos.nombre_contratista_persona);
                apex_json.write('id_contratista_vehiculo', x_datos.id_contratista_vehiculo);
                apex_json.write('placa', x_datos.placa);
                apex_json.write('id_tipo_brigada', x_datos.id_tipo_brigada);
                apex_json.write('codigo_tipo_brigada', x_datos.codigo_tipo_brigada);
                apex_json.write('tipo_brigada', x_datos.tipo_brigada);
                apex_json.close_object();
            end loop;
            apex_json.close_array();

            -- gnl_zonas
            apex_json.open_array('Zonas');
                for c_zonas in (
                    select z.id_zona
                        , z.id_territorial
                        , z.codigo
                        , z.nombre
                    from aire.gnl_zonas z
                    inner join aire.sgd_usuarios_zona uz on uz.id_zona = z.id_zona and uz.id_usuario = v_id_usuario
                    where z.ind_activo = 'S'
                        and z.id_zona in (
                            select id_zona
                            from aire.ctn_contratos_zona
                            where id_contrato in (
                                        select id_contrato
                                          from aire.v_ctn_contratos
                                         where prefijo_actividad = 'G_SCR'
                                           and id_contratista    = c_datos.id_contratista
                                        )
                        )
                ) loop
                    apex_json.open_object();
                    apex_json.write('id_zona', c_zonas.id_zona);
                    apex_json.write('id_territorial', c_zonas.id_territorial);
                    apex_json.write('codigo', c_zonas.codigo);
                    apex_json.write('nombre', c_zonas.nombre);
                    apex_json.close_object();
                end loop;
            apex_json.close_array();
        end loop;


                    -- tipos_suspencion
                    apex_json.open_array('tipos_suspencion');
                        for c_datos in (
                            SELECT
                                  x.id_tipo_suspencion
                                , x.codigo
                                , x.descripcion
                            FROM aire.scr_tipos_suspencion x
                            WHERE       x.ind_activo = 'S'
                                    and x.id_actividad = v_id_actividad
                        ) loop
                            apex_json.open_object();
                                apex_json.write('id_tipo_suspencion', c_datos.id_tipo_suspencion);
                                apex_json.write('codigo', c_datos.codigo);
                                apex_json.write('descripcion', c_datos.descripcion);
                                    apex_json.open_array('tipos_brigada');
                                        for c_datos2 in (
                                            SELECT
                                                  xy.id_tipo_brigada
                                                , y.codigo
                                                , y.descripcion
                                            from aire.scr_tipos_suspencion_tipo_brigada xy
                                            inner join aire.ctn_tipos_brigada y on y.id_tipo_brigada = xy.id_tipo_brigada
                                            WHERE       y.ind_activo = 'S'
                                                    and xy.id_tipo_suspencion = c_datos.id_tipo_suspencion
                                        ) loop
                                        apex_json.open_object();
                                            apex_json.write('id_tipo_brigada', c_datos2.id_tipo_brigada);
                                            apex_json.write('codigo', c_datos2.codigo);
                                            apex_json.write('descripcion', c_datos2.descripcion);
                                        apex_json.close_object();
                                        end loop;
                                    apex_json.close_array();
                            apex_json.close_object();
                        end loop;
                    apex_json.close_array();
                    
                    -- Estados orden
                    apex_json.open_array('estados_orden');
                        for c_datos2 in (
                            select id_estado_orden
                                , codigo_estado
                                , descripcion
                            from aire.ord_estados_orden
                            where id_actividad     = v_id_actividad
                            and ind_activo = 'S' and codigo_estado not in ('CERR')
                        ) loop
                            apex_json.open_object();
                            apex_json.write('id_estado_orden', c_datos2.id_estado_orden);
                            apex_json.write('codigo_estado',c_datos2.codigo_estado);
                            apex_json.write('descripcion',c_datos2.descripcion);
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
            apex_json.write('mensaje', 'Error N '||v_id_log ||' al crear los parametros iniciales, '|| sqlerrm);
            apex_json.close_object();
            --v_json := apex_json.get_clob_output;
            s_json := apex_json.get_clob_output;
            apex_json.free_output;
            --s_json := v_json;
    END prc_parametros_iniciales_contratistas;

    --ultima modificacion 19/03/2024 08 pm

END PKG_G_CARLOS_VARGAS_TEST6_Casa;