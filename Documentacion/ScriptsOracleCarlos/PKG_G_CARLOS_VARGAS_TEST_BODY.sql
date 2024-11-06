CREATE OR REPLACE PACKAGE BODY "AIRE".PKG_G_CARLOS_VARGAS_TEST AS

    /*	Procedimiento que retorna json con los parametros iniciales para ORDENES PERFIL AREA CENTRAL PERSONAL DE AIRE
    
        @autor: Carlos Vargas
        @Fecha Creación: 10/01/2024
        @Fecha Ultima Actualización: 11/01/2024
    
        Parámetros
        s_json   : corresponde al json de respuesta con toda la informacion requerida para el modulo de SCR
    */
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
  
  /*	Procedimiento que retorna json con la info del cliente
    
        @autor: Carlos Vargas
        @Fecha Creación: 10/01/2024
        @Fecha Ultima Actualización: 10/01/2024
    
        Parámetros
        s_json   : corresponde al json de respuesta con toda la informacion requerida para el modulo de SCR
    */
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
    dbms_output.put_line(s_json);
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
  
  /*	Procedimiento que retorna json con el listado AGRUPADO de ordenes para procesar por parte de area central
    
        @autor: Carlos Vargas
        @Fecha Creación: 12/01/2024
        @Fecha Ultima Actualización: 12/01/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
  */
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
  
  /*	Procedimiento que retorna json con el listado AGRUPADO de ordenes para procesar por parte de area central
    
        @autor: Carlos Vargas
        @Fecha Creación: 12/01/2024
        @Fecha Ultima Actualización: 12/01/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
  */
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
  
  /*	Procedimiento que retorna json con los parametros iniciales para CONTRATISTAS
    
        @autor: Carlos Vargas
        @Fecha Creación: 19/01/2024
        @Fecha Ultima Actualización: 19/01/2024
    
        Parámetros
        s_json   : corresponde al json de respuesta
    */
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
  
  /*	Procedimiento de asignacion o desasignación invidual o masiva de brigadas o tecnicos
    
        @autor: Carlos Vargas
        @Fecha Creación: 21/01/2024
        @Fecha Ultima Actualización: 21/01/2024
    
        Parámetros
        e_json   : corresponde al json de consulta
        s_json   : corresponde al json de respuesta
    */
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
  
  
  procedure prc_registro_ordenes_masivo_temporal_Borreme (
        e_json  		in 	clob,
        s_json 	        out	clob
    ) is
        v_json_orden                json_object_t;  
        v_id_log		            aire.gnl_logs.id_log%type;
        v_respuesta                 aire.tip_respuesta := aire.tip_respuesta(mensaje => 'Registro exitoso.', nombre_up => 'pkg_g_daniel_gonzalez_test.prc_registro_ordenes_masivo_temporal_Borreme');
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
    end prc_registro_ordenes_masivo_temporal_Borreme;   
    
    procedure prc_consultar_ordenes_dashboard_contratistas (		
        e_json  in  clob,
        s_json 	out	clob
    ) is
        --e_json  	clob :=  '{"pageSize":1,"pageNumber":0,"sortColumn":"id_orden","sortDirection":"desc"}';
        --v_json      clob;
        v_json_objeto       json_object_t; 
        v_id_log		    aire.gnl_logs.id_log%type;
        v_respuesta		    aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_ordenes.prc_consultar_ordenes_dashboard_contratistas');
        v_objeto      	    aire.ord_ordenes%rowtype;
        v_id_orden          NUMBER;
        pageNumber          NUMBER; 
        pageSize            NUMBER;
        sortColumn          VARCHAR(100 BYTE);
        sortDirection       VARCHAR(100 BYTE);  
        v_RegistrosTotales  NUMBER;
        v_id_contratista    NUMBER;
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
    v_id_contratista        := v_json_objeto.get_number('id_contratista');
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

                             --, nvl(d.georreferencia,'{\"longitud\":0,\"latitud\":0}') as georeferencia_cliente
                             , to_number(json_value(nvl(d.georreferencia,'{"longitud":0,"latitud":0}'), '$.longitud'), '9999.99999999') longitud_cliente
                             , to_number(json_value(nvl(d.georreferencia,'{"longitud":0,"latitud":0}'), '$.latitud'), '9999.99999999') latitud_cliente

                             , nvl(m.fecha_ejecucion,to_timestamp('1900/01/01 12:01:01,000000000 am', 'yyyy/mm/dd hh:mi:ss,ff9 am')) as fecha_ejecucion
                             
                             --, '{\"longitud\":0,\"latitud\":0}' georeferencia_ejecucion
                             , to_number(json_value(nvl(m.georreferencia,'{"longitud":0,"latitud":0}'), '$.longitud'), '9999.99999999') longitud_ejecucion
                             , to_number(json_value(nvl(m.georreferencia,'{"longitud":0,"latitud":0}'), '$.latitud'), '9999.99999999') latitud_ejecucion
                             
                             , NVL(m.id_anomalia,-1) id_anomalia
                             , NVL(n.descripcion,'-') descripcion_anomalia

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
                        left join aire.ord_ordenes_gestion              m on m.id_orden                 = a.id_orden
                        left join aire.ord_anomalias                    n on n.id_anomalia              = m.id_anomalia
                        where nvl(a.id_contratista,-1) in (
                                select id_contratista
                                        from aire.v_ctn_contratos 
                                        where UPPER(prefijo_actividad) = 'G_SCR'
                                        and UPPER(ind_activo) = 'S'       
                                union all
                                select -1 from dual
                            ) AND a.id_contratista = v_id_contratista
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
                            apex_json.write('longitud', c_datos.longitud_cliente);
                            apex_json.write('latitud', c_datos.latitud_cliente);
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
    END prc_consultar_ordenes_dashboard_contratistas;
  
END PKG_G_CARLOS_VARGAS_TEST;