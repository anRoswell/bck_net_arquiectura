-- INSERT INTO AIRE.ORD_ORDENES_HISTORICO
select
	aire.sec_ord_ordenes_historico.nextval,
	sysdate,
	'Creado',
	x.id_orden,
	x.id_tipo_orden,
	x.numero_orden,
	x.id_estado_orden,
	x.id_contratista,
	x.id_cliente,
	x.id_territorial,
	x.id_zona,
	x.direcion,
	x.fecha_creacion,
	x.fecha_cierre,
	x.id_usuario_cierre,
	x.descripcion,
	x.comentarios,
	x.acta,
	x.id_actividad,
	x.id_contratista_persona,
	x.id_tipo_trabajo,
	x.id_tipo_suspencion,
	x.actividad_orden,
	x.fecha_estimada_respuesta,
	x.numero_camp,
	x.comentario_orden_servicio_num1,
	x.comentario_orden_servicio_num2,
	x.observacion_rechazo,
	x.fecha_rechazo,
	x.origen,
	x.fecha_registro,
	x.id_barrio,
	x.fecha_asigna_contratista,
	x.fecha_asigna_tecnico,
	a.nic,
	b.expired_balance as deuda,
	d.nombres || ' ' || d.apellidos as tecnico,
	e.nombre as departamento,
	f.nombre as municipio,
	b.expired_periodos as numero_factura,
	h.codigo as tipo_brigada,
	trunc(sysdate - trunc(x.fecha_registro)) as antiguedad
from aire.ord_ordenes x
left join aire.gnl_clientes a on x.id_cliente = a.id_cliente
left join aire.ord_ordenes_dato_suministro b on x.id_orden = b.id_orden
left join aire.ctn_contratistas_persona c on x.id_contratista_persona = c.id_contratista_persona
left join aire.gnl_personas d on c.id_persona = d.id_persona
left join aire.gnl_departamentos e on a.id_departamento = e.id_departamento
left join aire.gnl_municipios f on a.id_municipio = f.id_municipio
left join aire.ctn_contratistas_brigada g on x.id_contratista_persona = g.id_contratista_persona and g.ind_activo = 'S'
left join aire.ctn_tipos_brigada h on g.id_tipo_brigada = h.id_tipo_brigada
where x.id_orden in (
    select 
        y.id_orden
    from (    
        select 
             x2.id_orden
            ,rownum rnum
        from aire.ord_ordenes x2
        order by x2.id_orden desc
    ) y
    --where y.rnum between 1 and 50000
    --where y.rnum between 1250001 and 1300000
    where y.rnum between 1300001 and 1338638    
)

commit;

/*
select count(*) from aire.ord_ordenes x2
--Agrupamientos
    select 
        z.Grupo
        ,min(z.id_orden) MinOrden
        ,max(z.id_orden) MaxOrden
        ,min(z.rnum) MinRnum
        ,max(z.rnum) MaxRnum
        ,max(z.rnum)-min(z.rnum) Registros
    from (
        select 
            y.id_orden,
            y.rnum,    
            (trunc((rnum - 1) / 50000)) Grupo
        from (    
            select 
                 x2.id_orden
                ,rownum rnum
            from aire.ord_ordenes x2
            order by x2.id_orden desc
        ) y
    ) z
    group by z.Grupo
    order by z.Grupo
    --where y.rnum between 50001 and 100000
*/



