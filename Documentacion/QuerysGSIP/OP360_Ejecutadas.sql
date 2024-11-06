select
      case when nvl(m1.nombres,'-1') = '-1' then null else m1.nombres || ' ' || m1.apellidos || '_' || a.id_contratista end as contratista
    , e.nombre as territorial
    , f.nombre || '_' || a.id_zona as zona
    , q.acta
    , q.fecha_ejecucion as fechaejecucion
    , q.fecha_inicio_ejecucion as fechainicial
    , q.fecha_fin_ejecucion as fechafinal
    , trunc(q.fecha_cierre) as fecha_Sincronizacion
    , to_char(q.fecha_cierre, 'HH24:MI:SS') as hora_Sincronizacion
    , a.id_orden
    , a.numero_orden
    , d.nic
    , mnc.nombre as Ciudad
    , brr.nombre as barrio
    , a.direcion as direccion
    , h.codigo_tipo_orden as tipo_orden
    , i.nombre as tipo_proceso    
    , qa.descripcion as Accion
    , case when qa.codigo_anomalia IN (3481,3476) then qa.descripcion || ' - ' || qsa.descripcion else qsa.descripcion end as SubAccion
    , q3.ListagCaracterizacion caracterizacion
    , k.descripcion as tipo_trabajo
    , n.expired_periodos as num_factura
    , TRIM(TO_CHAR(n.expired_balance,'L999G999G999G999D99MI','NLS_NUMERIC_CHARACTERS = '',.'' NLS_CURRENCY = ''$ '''))  as deuda_ejec
    , d.tarifa        
    , ae.nombre tipo_actividad
    , gdvq.descripcion as actividad
    , l.descripcion as tipo_suspension
    , json_object(
            key 'latitud'  VALUE nvl(to_number(json_value(replace(replace(replace(q.georreferencia,',"','#x#'),',','.'),'#x#',',"'), '$.latitud'), '9999.9999999999999999'), 0),
            key 'longitud' VALUE nvl(to_number(json_value(replace(replace(replace(q.georreferencia,',"','#x#'),',','.'),'#x#',',"'), '$.longitud'), '9999.9999999999999999'), 0)
    ) as georreferencia
    , gdv.descripcion vehiculo
    , tb.descripcion tipo_operativa
    , m3.identificacion as id_contratista_persona
    , case when nvl(m3.nombres,'-1') = '-1' then null else m3.nombres || ' ' || m3.apellidos end as nombre_contratista_persona
    , '  Fecha: ' || q.fecha_ejecucion ||
      ', ACTA: ' || q.acta ||
      ', TECNICO: ' || (case when nvl(m3.nombres,'-1') = '-1' then null else m3.nombres || ' ' || m3.apellidos end) ||
      ', PREDIO: ' || gdv2.descripcion ||
      ', ATENDIO: ' || nvl(q.nombres_persona_atiende,'no') ||
      ', MEDIDOR: ' || q.Numero_Medidor ||
      ', LECTURA: ' || q.lectura ||
      ', CT: ' || q.ct ||
      ', MT: ' || q.mt ||
      ', Otros: ' || oga2.ListagFaltante ||
      ' ' as observacion
      , a.origen
from aire.ord_ordenes              				        a
left join (
    select
        og.ID_ORDEN
        ,LISTAGG(ac.CODIGO || ': ' || oga.OBSERVACION, '; ') as ListagFaltante
    from aire.ord_ordenes_gestion_acciones oga
    inner join aire.ORD_ORDENES_GESTION og on og.ID_ORDEN_GESTION = oga.ID_ORDEN_GESTION
    inner join aire.scr_acciones ac on ac.ID_ACCION = oga.ID_ACCION
    where ac.IND_ACTIVO = 'S'
    group by og.ID_ORDEN
)                                                       oga2    on oga2.id_orden = a.id_orden
left join (
    select
          subq.id_orden_gestion, subq.id_orden
        , subq.acta
        , subq.id_anomalia
        , subq.id_uso_energia
        , subq.id_actividad_economica
        , subq.id_estado_predio
        , subq.id_subanomalia
        , subq.georreferencia
        , subq.nombres_persona_atiende
        , subq.Numero_Medidor
        , subq.lectura
        , subq.ct
        , subq.mt
        , subq.fecha_ejecucion
        , subq.fecha_inicio_ejecucion
        , subq.fecha_fin_ejecucion
        , subq.fecha_cierre
        , subq.id_contratista_brigada
    from (
        select qs.id_orden_gestion, qs.id_orden
            , qs.acta
            , qs.id_anomalia
            , qs.id_uso_energia
            , qs.id_actividad_economica
            , qs.id_estado_predio
            , qs.id_subanomalia
            , qs.georreferencia
            , qs.nombres_persona_atiende
            , qs.Numero_Medidor
            , qs.lectura
            , qs.ct
            , qs.mt
            , qs.fecha_ejecucion
            , qs.fecha_inicio_ejecucion
            , qs.fecha_fin_ejecucion
            , qs.fecha_cierre
            , qs.id_contratista_brigada
            ,row_number() over (partition by qs.id_orden order by qs.id_orden_gestion) as rn
        from aire.ord_ordenes_gestion qs
    ) subq
    where subq.rn = 1
)                                                       q       on q.id_orden = a.id_orden
left join (
    select
        z.id_orden
        ,LISTAGG(y.Descripcion,', ') as ListagCaracterizacion
    from aire.ord_comentarios_subanomalia y
    join (
        select
            t.id_orden
            ,regexp_substr(t.id_observacion_anomalia,'[^,]+',1,rn) as id_comentario_subanomalia
        from aire.ord_ordenes_gestion t
        cross join lateral (
        select level rn from dual
        connect by level <=
            length (t.id_observacion_anomalia) - length (replace(t.id_observacion_anomalia, ',' )) + 1
        )
    -- where t.id_observacion_anomalia is not null --and t.id_orden_gestion = 683
    ) z on z.id_comentario_subanomalia = y.id_comentario_subanomalia
    group by z.ID_ORDEN
)                                                       q3      on q3.id_orden                  = a.id_orden
left join aire.ord_anomalias                            qa      on q.id_anomalia                = qa.id_anomalia
left join aire.ord_subanomalias                         qsa     on q.id_anomalia                = qsa.id_anomalia and q.id_subanomalia = qsa.id_subanomalia
left join aire.gnl_dominios_valor                       gdvq    on gdvq.id_dominio_valor        = q.id_uso_energia
left join aire.gnl_actividades_economica                ae      on ae.id_actividad_economica    = q.id_actividad_economica
left join aire.gnl_dominios_valor                       gdv2    on q.id_estado_predio           = gdv2.id_dominio_valor
left join (
    select
        oga.id_orden_gestion
        ,LISTAGG(acc.descripcion || ', ') as ListagAcciones
        ,LISTAGG(distinct ascx.descripcion || ', ') as ListagSubAcciones
    from aire.ord_ordenes_gestion_acciones oga
    left join aire.scr_acciones                             acc on acc.id_accion    = oga.id_accion
    left join aire.scr_subacciones                          ascx on ascx.id_accion  = oga.id_accion and ascx.id_subaccion = oga.id_subaccion
    group by oga.id_orden_gestion
)                                                           oga22 on oga22.id_orden_gestion     = q.id_orden_gestion
left join aire.ord_ordenes_dato_suministro              n on n.id_orden                         = a.id_orden
left join aire.gnl_tarifas_subcategoria                 ts on ts.codigo_osf                     = n.codigo_tarifa
left join aire.gnl_tarifas_categoria                    tc on tc.codigo_tarifa_categoria        = ts.codigo_tarifa_categoria
inner join aire.ord_estados_orden   			        b on a.id_estado_orden   		        = b.id_estado_orden
left join aire.ctn_contratistas  				        c on a.id_contratista    		        = c.id_contratista
left join aire.gnl_personas                             m1 on c.id_persona                      = m1.id_persona
left join aire.gnl_clientes        				        d on a.id_cliente        		        = d.id_cliente
left join aire.gnl_municipios                           mnc on mnc.id_municipio                 = d.id_municipio
left join aire.gnl_barrios                              brr on brr.id_barrio                    = a.id_barrio
left join aire.gnl_territoriales   				        e on a.id_territorial    		        = e.id_territorial
left join aire.gnl_zonas           				        f on a.id_zona           		        = f.id_zona
left join aire.ord_tipos_orden     				        h on a.id_tipo_orden     		        = h.id_tipo_orden
left join aire.gnl_actividades     				        i on a.id_actividad      		        = i.id_actividad
left join aire.ctn_contratistas_persona  		        j on  a.id_contratista_persona 	        = j.id_contratista_persona
left join aire.v_gnl_personas			 	            m3 on j.id_persona                      = m3.id_persona
left join aire.ctn_contratistas_brigada                 j2 on q.id_contratista_brigada          = j2.id_contratista_brigada
left join aire.CTN_CONTRATISTAS_VEHICULO                cv on cv.id_contratista_vehiculo        = j2.id_contratista_vehiculo and cv.ind_activo = 'S'
left join aire.gnl_dominios_valor                       gdv on gdv.id_dominio_valor             = cv.id_tipo_vehiculo
left join aire.ctn_tipos_brigada                        tb on tb.id_tipo_brigada        = j2.id_tipo_brigada
left join aire.ord_tipos_trabajo 				        k on a.id_tipo_trabajo 		 	= k.id_tipo_trabajo
left join aire.scr_tipos_suspencion 			        l on a.id_tipo_suspencion 	 	= l.id_tipo_suspencion
where
    b.codigo_estado = 'CERR' and
    NVL(a.id_contratista,-1) in (
            select id_contratista
            from aire.v_ctn_contratos
            where UPPER(prefijo_actividad) = 'G_SCR'
            and UPPER(ind_activo) = 'S'
            union all
            select -1 from dual
    )     
    and trunc(q.fecha_cierre) between '01/04/24' and '24/04/24'