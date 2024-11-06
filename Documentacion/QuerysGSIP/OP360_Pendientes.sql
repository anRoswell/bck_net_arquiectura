select
      a.id_orden
    , a.id_tipo_orden
    , h.descripcion as tipo_orden
    , a.numero_orden
    , b.descripcion  as estado_orden
    , case when nvl(m1.nombres,'-1') = '-1' then null else m1.nombres || ' ' || m1.apellidos || '_' || a.id_contratista end as contratista
    , REPLACE(REGEXP_REPLACE(d.nombre_cliente, '[[:cntrl:]]', ' '),'"',' ') as cliente
    , e.nombre as territorial
    , f.nombre || '_' || a.id_zona as zona
    , a.direcion
    , a.fecha_creacion
    , a.fecha_cierre as fecha_cierre
    , case when nvl(m2.nombres,'-1') = '-1' then null else m2.nombres || ' ' || m2.apellidos end as usuario_cierre
    , i.nombre as actividad
    , case when nvl(m3.nombres,'-1') = '-1' then null else m3.nombres || ' ' || m3.apellidos end as nombre_contratista_persona
    , k.descripcion as tipo_trabajo
    , l.descripcion as tipo_suspencion
    , a.origen    
    , TRUNC(sysdate - TRUNC(a.fecha_registro)) as antiguedad
    , a.fecha_registro
    , d.nic
    , tc.descripcion || ' - ' || ts.descripcion  tarifa
    , h.codigo_tipo_orden
    , trim(to_char(n.expired_balance, '999G999G999G999G999G999G990')) deuda    
    , n.expired_periodos as ultima_factura
    , a.id_contratista_persona
    , dpt.nombre as Departamento
    , mnc.nombre as Municipio
    , brr.nombre as Barrio
    , tb.descripcion as tipo_brigada
    , a.comentario_orden_servicio_num1 Comanterio_OS
    , localtimestamp Fecha_Consulta
    , a.fecha_asigna_tecnico
from aire.ord_ordenes              				a
left join aire.ord_ordenes_dato_suministro      n on n.id_orden                     = a.id_orden
left join aire.gnl_tarifas_subcategoria         ts on ts.codigo_osf                 = n.codigo_tarifa
left join aire.gnl_tarifas_categoria            tc on tc.codigo_tarifa_categoria    = ts.codigo_tarifa_categoria
left join aire.ord_estados_orden   			    b on a.id_estado_orden   		    = b.id_estado_orden
left join aire.ctn_contratistas  				c on a.id_contratista    		    = c.id_contratista
left join aire.gnl_personas                     m1 on c.id_persona                  = m1.id_persona
left join aire.gnl_clientes        				d on a.id_cliente        		    = d.id_cliente
left join aire.gnl_departamentos                dpt on dpt.id_departamento          = d.id_departamento
left join aire.gnl_municipios                   mnc on mnc.id_municipio             = d.id_municipio
left join aire.gnl_barrios                      brr on brr.id_barrio                = a.id_barrio
left join aire.gnl_territoriales   				e on a.id_territorial    		    = e.id_territorial
left join aire.gnl_zonas           				f on a.id_zona           		    = f.id_zona
left join aire.sgd_usuarios      				g on a.id_usuario_cierre 		    = g.id_usuario
left join aire.gnl_personas             		m2 on g.id_persona                  = m2.id_persona
left join aire.ord_tipos_orden     				h on a.id_tipo_orden     		    = h.id_tipo_orden
left join aire.gnl_actividades     				i on a.id_actividad      		    = i.id_actividad
left join aire.ctn_contratistas_persona  		j on  a.id_contratista_persona 	    = j.id_contratista_persona
left join aire.ctn_contratistas_brigada         j2 on a.id_contratista_persona      = j2.id_contratista_persona and j2.ind_activo = 'S'
left join aire.ctn_tipos_brigada                tb on tb.id_tipo_brigada            = j2.id_tipo_brigada
left join aire.v_gnl_personas			 	    m3 on j.id_persona                  = m3.id_persona
left join aire.ord_tipos_trabajo 				k on a.id_tipo_trabajo 		 	    = k.id_tipo_trabajo
left join aire.scr_tipos_suspencion 			l on a.id_tipo_suspencion 	 	    = l.id_tipo_suspencion
where
    b.codigo_estado != 'CERR' and
    NVL(a.id_contratista,-1) in (
            select id_contratista
            from aire.v_ctn_contratos
            where UPPER(prefijo_actividad) = 'G_SCR'
            and UPPER(ind_activo) = 'S'
            union all
            select -1 from dual
     )     
     and b.codigo_estado IN ('SASI','SCOM','SEAS','SPEN','LFAL','SBAN')