CREATE OR REPLACE PACKAGE BODY "AIRE".PKG_G_CARLOS_VARGAS_TEST2 AS 
    
  /*	Procedimiento que retorna json con el listado de ordenes para procesar por parte de area central
    
        @autor: Carlos Vargas
        @Fecha Creación: 11/01/2024
        @Fecha Ultima Actualización: 11/01/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
    */
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
    
  
  /*	Procedimiento que retorna json con el listado de la tabla aire.gnl_archivos_instancia
    
        @autor: Carlos Vargas
        @Fecha Creación: 16/01/2024
        @Fecha Ultima Actualización: 16/01/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
  */
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
  
  /*	Procedimiento que retorna json con el listado de la tabla aire.gnl_archivos_instancia
    
        @autor: Carlos Vargas
        @Fecha Creación: 16/01/2024
        @Fecha Ultima Actualización: 16/01/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
  */
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
  
  /*	Procedimiento de desasignación invidual o masiva de contratistas o aliados
    
        @autor: Carlos Vargas
        @Fecha Creación: 16/01/2024
        @Fecha Ultima Actualización: 16/01/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
    */
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
  
  /*	Procedimiento que retorna json con la orden consultada por el id
    
        @autor: Carlos Vargas
        @Fecha Creación: 17/01/2024
        @Fecha Ultima Actualización: 17/01/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
    */
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
  
  /*	Procedimiento que retorna json con el listado de ordenes para el dashboard de area central
    
        @autor: Carlos Vargas
        @Fecha Creación: 17/01/2024
        @Fecha Ultima Actualización: 17/01/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
    */
  procedure prc_consultar_ordenes_dashboard_area_central (		
        e_json  in  clob,
        s_json 	out	clob
    ) is
        --e_json  	clob :=  '{"pageSize":1,"pageNumber":0,"sortColumn":"id_orden","sortDirection":"desc"}';
        --v_json      clob;
        v_json_objeto       json_object_t; 
        v_id_log		    aire.gnl_logs.id_log%type;
        v_respuesta		    aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_ordenes.prc_consultar_ordenes_dashboard_area_central');
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
                             , a.id_contratista as id_contratista
                             , c.nombre_completo as contratista
                             , a.id_cliente
                             , d.nombre_cliente as cliente
                             , a.id_territorial
                             , e.nombre as territorial
                             , a.id_zona id_zona
                             , f.nombre as zona
                             , a.direcion
                             , a.fecha_creacion
                             , NVL(a.fecha_cierre,TO_TIMESTAMP('1900/01/01 12:01:01,000000000 AM', 'YYYY/MM/DD HH:MI:SS,FF9 AM')) as fecha_cierre
                             , a.id_usuario_cierre id_usuario_cierre
                             , g.nombres as usuario_cierre
                             , a.id_actividad
                             , i.nombre as actividad
                             , a.id_contratista_persona
                             , j.nombre_contratista_persona
                             , a.id_tipo_trabajo id_tipo_trabajo
                             , k.descripcion as tipo_trabajo
                             , a.id_tipo_suspencion id_tipo_suspencion 
                             , l.descripcion as tipo_suspencion
                             , SUM(1) OVER() RegistrosTotales

                             --, nvl(d.georreferencia,'{\"longitud\":0,\"latitud\":0}') as georeferencia_cliente
                             , to_number(json_value(nvl(d.georreferencia,'{"longitud":NULL,"latitud":NULL}'), '$.longitud'), '9999.99999999') longitud_cliente
                             , to_number(json_value(nvl(d.georreferencia,'{"longitud":NULL,"latitud":NULL}'), '$.latitud'), '9999.99999999') latitud_cliente

                             , nvl(m.fecha_ejecucion,to_timestamp('1900/01/01 12:01:01,000000000 am', 'yyyy/mm/dd hh:mi:ss,ff9 am')) as fecha_ejecucion
                             
                             --, '{\"longitud\":0,\"latitud\":0}' georeferencia_ejecucion
                             , to_number(json_value(nvl(m.georreferencia,'{"longitud":"","latitud":""}'), '$.longitud'), '9999.99999999') longitud_ejecucion
                             , to_number(json_value(nvl(m.georreferencia,'{"longitud":"","latitud":""}'), '$.latitud'), '9999.99999999') latitud_ejecucion
                             
                             , m.id_anomalia id_anomalia
                             , n.descripcion descripcion_anomalia
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
                        left join aire.ord_ordenes_gestion              m on m.id_orden                 = a.id_orden
                        left join aire.ord_anomalias                    n on n.id_anomalia              = m.id_anomalia
                        where nvl(a.id_contratista,-1) in (
                                select id_contratista
                                        from aire.v_ctn_contratos 
                                        where UPPER(prefijo_actividad) = 'G_SCR'
                                        and UPPER(ind_activo) = 'S'       
                                union all
                                select -1 from dual
                            )
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
                    
                        apex_json.open_object('georeferencia_cliente');
                            apex_json.write('longitud', c_datos.longitud_cliente,true);
                            apex_json.write('latitud', c_datos.latitud_cliente,true);
                        apex_json.close_object();
                    
                    apex_json.write('fecha_ejecucion', c_datos.fecha_ejecucion);                    

                        apex_json.open_object('georeferencia_ejecucion');
                            apex_json.write('longitud', c_datos.longitud_ejecucion);
                            apex_json.write('latitud', c_datos.latitud_ejecucion);
                        apex_json.close_object();

                    apex_json.write('id_anomalia', c_datos.id_anomalia);
                    apex_json.write('descripcion_anomalia', c_datos.descripcion_anomalia);

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
  
  /*	Procedimiento de desasignación invidual o masiva de brigadas
    
        @autor: Carlos Vargas
        @Fecha Creación: 21/01/2024
        @Fecha Ultima Actualización: 21/01/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
    */
  procedure prc_gestionar_orden_des_asignar_brigada (		
        e_json  in  clob,
        s_json 	out	clob
    ) is
        --e_json  	clob :=  '{"id_contratista":0,"id_ordenes":"1,150,220","contratista_asignar":"N"}';
        v_json_objeto           json_object_t; 
        v_id_log		        aire.gnl_logs.id_log%type;
        v_respuesta		        aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_ordenes.prc_gestionar_orden_des_asignar_brigada');        
        
        v_identificacion_contratista        varchar2(50 BYTE);
        v_id_contratista                    varchar2(100 BYTE);        
        
        v_id_orden_string         varchar2(4000 byte);
        v_cnt                     number;
        
        -- Declarar la tabla temporal
        l_num_list              num_list_type := num_list_type();        
        v_cnt_actualizar        number;
        v_id_estado_orden       number;
        v_orden_notificar                aire.ord_ordenes%rowtype;    
        v_notificacion                   json_object_t;    
        v_respuesta_notificacion         aire.tip_respuesta;
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
                            and id_actividad IN (select id_actividad from aire.gnl_actividades where prefijo = 'G_SCR' and ind_activo = 'S')
                        );
                        
    IF (v_cnt_actualizar > 0) THEN        
        select id_estado_orden into v_id_estado_orden
        from aire.ord_estados_orden 
        where   codigo_estado in ('SASI') 
            and id_actividad IN (select id_actividad from aire.gnl_actividades where prefijo = 'G_SCR' and ind_activo = 'S');
        
        ---Guardar Ordenes Para notificar.xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx                        
            select
                 a.*
                into v_orden_notificar
            from aire.ord_ordenes a
            where a.id_orden in (select COLUMN_VALUE from TABLE(l_num_list))
            and id_contratista = v_id_contratista
            and id_contratista_persona is not null  
            and id_estado_orden in (
                select id_estado_orden 
                from aire.ord_estados_orden 
                where codigo_estado in ('SEAS') 
                    and id_actividad IN (select id_actividad from aire.gnl_actividades where prefijo = 'G_SCR' and ind_activo = 'S')
                );
        ---Fin guardar ordenes

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

        ---INICIO enviar notificacion a celular xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            -- consultamos el id del dispositivo
           /* DECLARE
                CURSOR c_dispositivos IS (
                select DISTINCT a.id_dispositivo              
                from aire.sgd_usuarios a
                join aire.ctn_contratistas_persona b on b.id_persona = a.id_persona
                WHERE b.id_contratista_persona in (select id_contratista_persona from v_orden_notificar)
                );
                
                
                v_id_dispositivo2            aire.sgd_usuarios.id_dispositivo%TYPE;
                v_notificacion2             json_object_t;
            BEGIN
                FOR dispos IN c_dispositivos LOOP
                    v_id_dispositivo2 := dispos.id_dispositivo;
                    
                    --v_notificacion2 := json_object_t();
                    --v_notificacion2.put('id_dispositivo', v_id_dispositivo2);
                    --v_notificacion2.put('ind_android', true);        
                    --v_notificacion2.put('titulo', 'Des - asignación ordenes SCR');
                    v_notificacion2.put('cuerpo', 'Se des - asignaron ordenes de su gestión'); -- cuando es desasignación que diga Se desasignaron x ordenes a su gestión.
                    v_notificacion2.put('estado', 'des-asignada');
                    v_notificacion2.put('tipo', 'gestion_ordenes_scr');
                    --v_notificacion2.put('fecha_envio', to_char(systimestamp, 'DD/MM/YYYY HH:MI:SS:FF PM'));
                    -- Se envia notificacion en segundo plano
                    --aire.pkg_g_generales.prc_enviar_notificacion_push_segundo_plano( e_notificacion  => v_notificacion.to_clob, s_respuesta => v_respuesta_notificacion);
                    
                    
                    --DBMS_OUTPUT.PUT_LINE('ID Dispositivo: ' || v_id_dispositivo);
                END LOOP;
            END;*/
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
  
  /*	Procedimiento que retorna json con el listado de ordenes para procesar por parte de los contratistas
    
        @autor: Carlos Vargas
        @Fecha Creación: 21/01/2024
        @Fecha Ultima Actualización: 21/01/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
    */
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
  
  /*	Procedimiento que retorna json con el listado de ordenes para el dashboard de contratistas
    
        @autor: Carlos Vargas
        @Fecha Creación: 21/01/2024
        @Fecha Ultima Actualización: 21/01/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
    */
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
  
  /*	Procedimiento que retorna json con la orden consultada por el id
    
        @autor: Carlos Vargas
        @Fecha Creación: 17/01/2024
        @Fecha Ultima Actualización: 17/01/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
    */
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
  
  /*	Procedimiento que retorna json con un resumen general de la tabla de ordenes para control de los fronted
    
        @autor: Carlos Vargas
        @Fecha Creación: 29/01/2024
        @Fecha Ultima Actualización: 29/01/2024
    
        Parámetros        
        s_json   : corresponde al json de respuesta
    */
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
  
  
  procedure prc_consultar_trazabilidad_ordenes (		
        e_json  in  clob,
        s_json 	out	clob
	) is
		--e_json  	clob :=  '{"v_id_orden_string":"15043,15044,15048"}';
        v_json_objeto           json_object_t; 
        v_id_log		        aire.gnl_logs.id_log%type;
        v_respuesta		        aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_ordenes.prc_consultar_trazabilidad_ordenes');        
        
        v_id_orden_string         varchar2(4000 byte);
        v_asignar_contratista     varchar2(1 BYTE); 
        
        -- Declarar la tabla temporal
        l_num_list              num_list_type := num_list_type();        
        v_cnt_actualizar        number;
        v_cnt_ordenes           number;
        v_id_estado_orden       number;
        v_json                  varchar2(4000 byte);
  BEGIN  
    --LOG
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
    
    -- Deserializar JSON
    apex_json.free_output; 
    apex_json.initialize_clob_output( p_preserve => false );
    
    v_json_objeto               := json_object_t(e_json);        
    v_id_orden_string           := v_json_objeto.get_string('id_orden_string');    
    
    
    apex_json.free_output;
        
    --Validar, si no llega ningun registro retornar provocar error.
    SELECT
       TO_NUMBER(REGEXP_SUBSTR(v_id_orden_string, '[^,]+', 1, level))
       BULK COLLECT INTO l_num_list
    FROM dual
    CONNECT BY REGEXP_SUBSTR(v_id_orden_string, '[^,]+', 1, level) IS NOT NULL;
    
    --//----INICIO validar si hay ordenes 
    select count(*) into v_cnt_ordenes 
    from aire.ord_ordenes 
    where       id_orden in (select COLUMN_VALUE from TABLE(l_num_list));                     
                        
   IF (v_cnt_ordenes = 0) THEN
        --Si todo sale bien devolver mensaje.
        apex_json.open_object();
            apex_json.write('codigo', 50034);
            apex_json.write('mensaje', 'No se encontraron ordenes para consultar la traza');            
        apex_json.close_object();
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
        return;
    END IF;
    --//----FIN validar si hay ordenes 
    
    --//----INICIO validar si hay ordenes 
                        
    IF (v_cnt_ordenes > 0) THEN                
        SELECT
          JSON_OBJECT (
            'codigo' VALUE 200,
            'mensaje' VALUE 'Proceso realizado con éxito',
            'datos' VALUE JSON_ARRAYAGG (
              JSON_OBJECT (
                'id_orden' VALUE a.id_orden,
                'traza' VALUE JSON_ARRAYAGG (
                  JSON_OBJECT (
                    'id' VALUE a.id_orden_trazabilidad,
                    'id_estado_orden' VALUE a.id_estado_orden,
                    'icono_Esado' VALUE y.Icono_Esado,
                    'nombre_estado_orden' VALUE eo.Descripcion,
                    'id_contratista' VALUE a.id_contratista,
                    'nombre_contratista' VALUE c.Nombre_Completo,
                    'id_contratista_persona' VALUE a.id_contratista_persona,
                    'nombre_contratista_persona' VALUE d.Nombre_Contratista_persona,
                    'observacion' VALUE a.Observacion,
                    'fecha' VALUE to_char(a.fecha, 'DD/MM/YYYY HH:MI:SS AM')
                  ) ORDER BY a.id_orden_trazabilidad
                )
              )
            )
          ) AS json_resultado 
          into v_json
        FROM aire.ord_ordenes_trazabilidad a
        LEFT JOIN aire.ord_estados_orden eo on eo.id_estado_orden = a.id_estado_orden
        LEFT JOIN (
            select 
                 x.id_estado_orden
                ,case x.id_estado_orden
                    when 1 then 'Icono '  || rownum
                    when 22 then 'Icono ' || rownum
                    when 23 then 'Icono ' || rownum
                    when 24 then 'Icono ' || rownum
                    when 25 then 'Icono ' || rownum
                    when 26 then 'Icono ' || rownum
                    when 27 then 'Icono ' || rownum
                    when 41 then 'Icono ' || rownum
                    when 61 then 'Icono ' || rownum
                    else 'Ninguno'
                end Icono_Esado
            from aire.ord_estados_orden x
            order by x.id_estado_orden
        ) y on y.id_estado_orden = a.id_estado_orden
        LEFT JOIN aire.v_ctn_contratistas c on c.id_contratista = a.id_contratista
        LEFT JOIN aire.v_ctn_contratistas_persona d on d.id_contratista_persona = a.id_contratista_persona
        WHERE a.id_orden IN (select COLUMN_VALUE from TABLE(l_num_list))
        GROUP BY a.id_orden
        ORDER BY a.id_orden;
        --Si todo sale bien devolver mensaje.        
        s_json := v_json;        
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
        apex_json.write('codigo', 50040);
        apex_json.write('mensaje', 'Error N '||v_id_log ||' al consultar la traza de las ordenes.' || sqlerrm);            
        apex_json.close_object();
        --v_json := apex_json.get_clob_output;
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
        --s_json := v_json;
        --dbms_output.put_line('xxx: '|| sqlerrm);
  END prc_consultar_trazabilidad_ordenes;
  
  procedure prc_consultar_georreferencia_areacentral (		
        e_json  in  clob,
        s_json 	out	clob
	) is
		--e_json  	clob :=  '{"v_id_orden_string":"15043,15044,15048"}';
        v_json_objeto           json_object_t; 
        v_id_log		        aire.gnl_logs.id_log%type;
        v_respuesta		        aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_ordenes.prc_consultar_georreferencia_areacentral');        
        v_objeto      	        aire.ord_ordenes%rowtype;
        v_fecha                 aire.sgd_sesiones_georreferencia.fecha%type;
        v_json                  clob;
  BEGIN  
    --LOG
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
    
    -- Deserializar JSON
    apex_json.free_output; 
    apex_json.initialize_clob_output( p_preserve => false );
    
    v_json_objeto                           := json_object_t(e_json);        
    v_fecha                                 := v_json_objeto.get_string('fecha');
    v_objeto.id_contratista                 := v_json_objeto.get_string('id_contratista');
    v_objeto.id_contratista_persona         := v_json_objeto.get_string('id_contratista_persona');
    
    
    apex_json.free_output;        
   
    with tx as (
        select distinct
              y.id_contratista_persona    
            , x.id_usuario
            , y.id_persona
            , y.nombre_contratista_persona
            , y.identificacion_contratista_persona
            , y.ind_activo  
            , y.id_contratista  
            , y.identificacion_contratista
            , y.nombre_contratista		
            , x.fecha           
            , x.georreferencia
        from aire.v_sgd_sesiones_georreferencia x
        inner join (
            select 
                      u.id_usuario
                    , a.id_contratista_persona
                    , a.id_persona
                    , b.nombre_completo 	    as nombre_contratista_persona
                    , b.identificacion  	    as identificacion_contratista_persona
                    , a.ind_activo        
                    , a.id_contratista  
                    , c.identificacion  	    as identificacion_contratista
                    , REGEXP_REPLACE(c.nombre_completo, '[^a-zA-Z0-9\s\.,;:_\-\(\)\[\]\{\}"''&<>\*\+\?\|\$#@=!%\/]+', '') as nombre_contratista		
            from aire.ctn_contratistas_persona		a
            join aire.v_gnl_personas			 	b on a.id_persona     = b.id_persona
            join aire.v_ctn_contratistas       	    c on a.id_contratista = c.id_contratista
            join aire.sgd_usuarios                  u on a.id_persona = u.id_persona
            where nvl(a.id_contratista,-1) in (
                                select id_contratista
                                        from aire.v_ctn_contratos 
                                        where UPPER(prefijo_actividad) = 'G_SCR'
                                        and UPPER(ind_activo) = 'S'       
                                union all
                                select -1 from dual
                            ) and
                            case 
                                when v_objeto.id_contratista > 0 and a.id_contratista = v_objeto.id_contratista then 1                            
                                else 0
                            end = 1 and
                            case
                                
                                when v_objeto.id_contratista_persona = -2 and a.id_contratista_persona is not null then 1
                                when v_objeto.id_contratista_persona > 0 and a.id_contratista_persona = v_objeto.id_contratista_persona then 1                            
                                else 0
                            end = 1
        ) y on y.id_usuario = x.id_usuario
        where trunc(x.fecha) = v_fecha --and rownum <= 100 --y.id_contratista_persona = v_id_contratista_persona
    )
    select
        json_object ( 
              'codigo' value 200
            , 'mensaje' value 'proceso realizado con éxito'
            , 'datos' value json_arrayagg (
                json_object (
                      t.id_contratista_persona    
                    , t.id_usuario
                    , t.id_persona
                    , t.nombre_contratista_persona
                    , t.identificacion_contratista_persona
                    , t.ind_activo  
                    , t.id_contratista  
                    , t.identificacion_contratista
                    , t.nombre_contratista		
                    , t.fecha           
                    , t.georreferencia
                    --, 'latitud' value to_number(json_value(t.georreferencia, '$.latitud'), '9999.99999999')  
                    --, 'longitud' value to_number(json_value(t.georreferencia, '$.longitud'), '9999.99999999')  
                ) order by t.fecha RETURNING CLOB
            ) RETURNING CLOB
        ) as result
        into v_json
    from tx t;
    
    s_json := v_json; --.get_clob_output;
    
    --LOG
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Carga exitosa');        
    --dbms_output.put_line(s_json);
    exception
    when others then
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al crear el json: '|| sqlerrm);
        apex_json.free_output; 
        apex_json.open_object();
        apex_json.write('codigo', 50040);
        apex_json.write('mensaje', 'Error N '||v_id_log ||' al consultar la traza de las ordenes.' || sqlerrm);            
        apex_json.close_object();
        --v_json := apex_json.get_clob_output;
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
        --s_json := v_json;
        --dbms_output.put_line('xxx: '|| sqlerrm);
  END prc_consultar_georreferencia_areacentral;
  
  procedure prc_consultar_georreferencia_contratista_persona (		
        e_json  in  clob,
        s_json 	out	clob
	) is
		--e_json  	clob :=  '{"v_id_orden_string":"15043,15044,15048"}';
        v_json_objeto           json_object_t; 
        v_id_log		        aire.gnl_logs.id_log%type;
        v_respuesta		        aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_ordenes.prc_consultar_georreferencia_contratista_persona');        
        v_objeto      	        aire.ord_ordenes%rowtype;
        v_fecha                 aire.sgd_sesiones_georreferencia.fecha%type;
        v_json                  clob;
  BEGIN  
    --LOG
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
    
    -- Deserializar JSON
    apex_json.free_output; 
    apex_json.initialize_clob_output( p_preserve => false );
    
    v_json_objeto                           := json_object_t(e_json);        
    v_fecha                                 := v_json_objeto.get_string('fecha');
    v_objeto.id_contratista                 := v_json_objeto.get_string('id_contratista');
    v_objeto.id_contratista_persona         := v_json_objeto.get_string('id_contratista_persona');
    
    
    apex_json.free_output;        
   
    with tx as (
        select distinct
              y.id_contratista_persona    
            , x.id_usuario
            , y.id_persona
            , y.nombre_contratista_persona
            , y.identificacion_contratista_persona
            , y.ind_activo  
            , y.id_contratista  
            , y.identificacion_contratista
            , y.nombre_contratista		
            , x.fecha           
            , x.georreferencia
        from aire.v_sgd_sesiones_georreferencia x
        inner join (
            select 
                      u.id_usuario
                    , a.id_contratista_persona
                    , a.id_persona
                    , b.nombre_completo 	    as nombre_contratista_persona
                    , b.identificacion  	    as identificacion_contratista_persona
                    , a.ind_activo        
                    , a.id_contratista  
                    , c.identificacion  	    as identificacion_contratista
                    , REGEXP_REPLACE(c.nombre_completo, '[^a-zA-Z0-9\s\.,;:_\-\(\)\[\]\{\}"''&<>\*\+\?\|\$#@=!%\/]+', '') as nombre_contratista		
            from aire.ctn_contratistas_persona		a
            join aire.v_gnl_personas			 	b on a.id_persona     = b.id_persona
            join aire.v_ctn_contratistas       	    c on a.id_contratista = c.id_contratista
            join aire.sgd_usuarios                  u on a.id_persona = u.id_persona
            where nvl(a.id_contratista,-1) in (
                                select id_contratista
                                        from aire.v_ctn_contratos 
                                        where UPPER(prefijo_actividad) = 'G_SCR'
                                        and UPPER(ind_activo) = 'S'       
                                union all
                                select -1 from dual
                            ) and
                            case 
                                when v_objeto.id_contratista > 0 and a.id_contratista = v_objeto.id_contratista then 1                            
                                else 0
                            end = 1 and
                            case
                                
                                when v_objeto.id_contratista_persona = -2 and a.id_contratista_persona is not null then 1
                                when v_objeto.id_contratista_persona > 0 and a.id_contratista_persona = v_objeto.id_contratista_persona then 1                            
                                else 0
                            end = 1
        ) y on y.id_usuario = x.id_usuario
        where trunc(x.fecha) = v_fecha --and rownum <= 100 --y.id_contratista_persona = v_id_contratista_persona
    )
    select
        json_object ( 
              'codigo' value 200
            , 'mensaje' value 'proceso realizado con éxito'
            , 'datos' value json_arrayagg (
                json_object (
                      t.id_contratista_persona    
                    , t.id_usuario
                    , t.id_persona
                    , t.nombre_contratista_persona
                    , t.identificacion_contratista_persona
                    , t.ind_activo  
                    , t.id_contratista  
                    , t.identificacion_contratista
                    , t.nombre_contratista		
                    , t.fecha           
                    , t.georreferencia
                    --, 'latitud' value to_number(json_value(t.georreferencia, '$.latitud'), '9999.99999999')  
                    --, 'longitud' value to_number(json_value(t.georreferencia, '$.longitud'), '9999.99999999')  
                ) order by t.fecha RETURNING CLOB
            ) RETURNING CLOB
        ) as result
        into v_json
    from tx t;
    
    s_json := v_json; --.get_clob_output;
    
    --LOG
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Carga exitosa');        
    --dbms_output.put_line(s_json);
    exception
    when others then
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al crear el json: '|| sqlerrm);
        apex_json.free_output; 
        apex_json.open_object();
        apex_json.write('codigo', 50040);
        apex_json.write('mensaje', 'Error N '||v_id_log ||' al consultar la traza de las ordenes.' || sqlerrm);            
        apex_json.close_object();
        --v_json := apex_json.get_clob_output;
        s_json := apex_json.get_clob_output;
        apex_json.free_output;
        --s_json := v_json;
        --dbms_output.put_line('xxx: '|| sqlerrm);
  END prc_consultar_georreferencia_contratista_persona;
  
  procedure prc_data_reporte_ordenes (		
        e_json  in  clob,
        s_json 	out	clob
	) is
		--v_json      clob;
        v_json_objeto       json_object_t; 
        v_id_log		    aire.gnl_logs.id_log%type;
        v_respuesta		    aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_carlos_vargas_test.prc_data_reporte_ordenes');        
        v_objeto      	        aire.ord_ordenes%rowtype;        
  BEGIN
  
    v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
    apex_json.free_output; 
    apex_json.initialize_clob_output( p_preserve => false );   
    
    --e_json  	clob :=  '{"id_orden":"102"}';
    v_json_objeto              := json_object_t(e_json);      
    v_objeto.id_orden          := v_json_objeto.get_number('id_orden');
    
    apex_json.open_object();        
        apex_json.write('codigo', 0);
        apex_json.write('mensaje', 'Consulta exitosa');
        apex_json.open_array('datos');
         -- ordenes
             for c_datos in (
                    select 
                          NVL(a.id_orden,-1) as id_orden
                         ,NVL(b.codigo_tipo_orden,-1) as codigo_tipo_orden
                         ,NVL(b.descripcion,-1) as descripcion_tipo_orden
                         ,NVL(c.nic,-1) as nic
                         ,c.Localidad
                         ,c.Nombre_Cliente
                         ,c.Direccion
                         --,c.georreferencia
                         ,to_number(json_value(nvl(c.georreferencia,'{"longitud":NULL,"latitud":NULL}'), '$.longitud'), '9999.99999999') longitud_cliente
                         ,to_number(json_value(nvl(c.georreferencia,'{"longitud":NULL,"latitud":NULL}'), '$.latitud'), '9999.99999999') latitud_cliente
                         ,d.mt
                         ,d.ct
                         ,e.nombre_contratista
                         ,d.fecha_ejecucion
                         ,d.fecha_inicio_ejecucion
                         ,d.fecha_fin_ejecucion
                         ,NVL(d.Acta,'0') AS Acta
                    from aire.ord_ordenes a                    
                    left join aire.ord_tipos_orden              b on b.id_tipo_orden = a.id_tipo_orden
                    left join aire.gnl_clientes                 c on c.id_cliente = a.id_cliente
                    left join aire.ord_ordenes_gestion          d on d.id_orden = a.id_orden
                    left join aire.v_ctn_contratistas_brigada   e on e.id_contratista_persona = d.id_contratista_persona
                    where a.id_orden = v_objeto.id_orden
                ) loop
                    apex_json.open_object();
                        apex_json.write('key', 'Localidad');apex_json.write('valor', TO_CHAR(c_datos.Localidad));
                    apex_json.close_object();
                    apex_json.open_object();
                        apex_json.write('key', 'id_orden');apex_json.write('valor', TO_CHAR(c_datos.id_orden));
                    apex_json.close_object();
                    apex_json.open_object();
                        apex_json.write('key', 'codigo_tipo_orden');apex_json.write('valor', TO_CHAR(c_datos.codigo_tipo_orden));
                    apex_json.close_object();
                    apex_json.open_object();
                        apex_json.write('key', 'descripcion_tipo_orden');apex_json.write('valor', TO_CHAR(c_datos.descripcion_tipo_orden));
                    apex_json.close_object(); 
                    apex_json.open_object();
                        apex_json.write('key', 'nic');apex_json.write('valor', TO_CHAR(c_datos.nic));
                    apex_json.close_object();
                    apex_json.open_object();
                        apex_json.write('key', 'Nombre_Cliente');apex_json.write('valor', TO_CHAR(c_datos.Nombre_Cliente));
                    apex_json.close_object();
                    apex_json.open_object();
                        apex_json.write('key', 'Direccion');apex_json.write('valor', TO_CHAR(c_datos.Direccion));
                    apex_json.close_object();
                    apex_json.open_object();
                        apex_json.write('key', 'Georreferencia');apex_json.write('valor', TO_CHAR(c_datos.longitud_cliente || ',' || c_datos.latitud_cliente));
                    apex_json.close_object();
                    apex_json.open_object();
                        apex_json.write('key', 'mt');apex_json.write('valor', c_datos.mt);
                    apex_json.close_object();
                    apex_json.open_object();
                        apex_json.write('key', 'ct');apex_json.write('valor', c_datos.ct);
                    apex_json.close_object();
                    apex_json.open_object();
                        apex_json.write('key', 'nombre_contratista');apex_json.write('valor', c_datos.nombre_contratista);
                    apex_json.close_object();
                    apex_json.open_object();
                        apex_json.write('key', 'fecha_ejecucion');apex_json.write('valor', c_datos.fecha_ejecucion);
                    apex_json.close_object();
                    apex_json.open_object();
                        apex_json.write('key', 'fecha_inicio_ejecucion');apex_json.write('valor', c_datos.fecha_inicio_ejecucion);
                    apex_json.close_object();
                    apex_json.open_object();
                        apex_json.write('key', 'fecha_fin_ejecucion');apex_json.write('valor', c_datos.fecha_fin_ejecucion);
                    apex_json.close_object();  
                    apex_json.open_object();
                        apex_json.write('key', 'Acta');apex_json.write('valor', c_datos.Acta);
                    apex_json.close_object();
                         
                end loop;
        apex_json.close_array();
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
  END prc_data_reporte_ordenes;
  
END PKG_G_CARLOS_VARGAS_TEST2;