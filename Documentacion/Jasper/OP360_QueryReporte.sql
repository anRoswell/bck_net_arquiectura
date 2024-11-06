-- select distinct 
--       nvl(a.id_orden,-1) as id_orden
--      ,c.localidad
--      ,min(nvl(d.acta,'0')) over() as acta
--      ,a.numero_orden
--      ,nvl(b.codigo_tipo_orden,-1) as codigo_tipo_orden
--      ,nvl(b.descripcion,-1) as descripcion_tipo_orden
--      ,nvl(c.nic,-1) as nic    
--      ,'residencial estrato' as tipo_cliente 
--      ,c.nombre_cliente
--      ,c.direccion
--      ,nvl(f.nombre,'na') as nombre_barrio
--       -- -- --,c.georreferencia
--      ,to_number(json_value(nvl(c.georreferencia,'{"longitud":null,"latitud":null}'), '$.longitud'), '9999.99999999') longitud_cliente
--      ,to_number(json_value(nvl(c.georreferencia,'{"longitud":null,"latitud":null}'), '$.latitud'), '9999.99999999') latitud_cliente
--      ,sum(d.mt) over() as mt
--      ,sum(d.ct) over() as ct
--      ,nvl(c.carga_contratada,'na') as carga_contratada
--      ,d.id_contratista_persona as tecnico
--      ,e1.nombre_contratista_persona
--      ,min(d.fecha_ejecucion) over() as fechaejecucion
--      ,min(d.fecha_inicio_ejecucion) over() as fechainicial
--      ,min(d.fecha_fin_ejecucion) over() as fechafinal
--      --,'http://localhost:3186/api/Op360Files/GetPlantillaReporte?codigo=LOG01&usuario=rNWyHxiGfuJN1LWFv90y2hDjqpfIkfHc/TmW7QxQ7mU=&contrasena=V1tqKGIzQ0NcjyfuvrA62tDZkuEI4VvDLlVg52E326zlIoCLZDOZLvrRUBIgfpSE' as Logo
-- from aire.ord_ordenes a                    
-- left join aire.gnl_clientes                     c  on c.id_cliente = a.id_cliente
-- left join aire.ord_ordenes_gestion              d  on d.id_orden = a.id_orden
-- left join aire.v_ctn_contratistas_persona       e1 on e1.id_contratista_persona = d.id_contratista_persona
-- left join aire.ord_tipos_orden                  b  on b.id_tipo_orden = a.id_tipo_orden
-- left join aire.gnl_barrios                      f  on f.id_barrio = c.id_barrio
-- where a.id_orden = 2177

with t as (
      select distinct 
             nvl(a.id_orden,-1) as id_orden
            ,min(d.fecha_ejecucion) over() as fechaejecucion
            ,min(nvl(d.acta,'0')) over() as acta
            ,e1.nombre_contratista_persona
            ,min(d.NOMBRES_PERSONA_ATIENDE) over() as Persona_Atiende
            ,min(d.NUMERO_MEDIDOR) over() as Medidor
            ,min(d.LECTURA) over() as Lectura
            ,sum(d.mt) over() as mt
            ,sum(d.ct) over() as ct
            ,h.ListagFaltante
      from aire.ord_ordenes a                    
      left join aire.gnl_clientes                     c  on c.id_cliente = a.id_cliente
      left join aire.ord_ordenes_gestion              d  on d.id_orden = a.id_orden
      left join aire.v_ctn_contratistas_persona       e1 on e1.id_contratista_persona = d.id_contratista_persona
      left join (
            select 
                  og.ID_ORDEN
                  ,LISTAGG(ac.CODIGO || ': ' || oga.OBSERVACION, '; ') as ListagFaltante
            from aire.ord_ordenes_gestion_acciones oga
            inner join aire.ORD_ORDENES_GESTION og on og.ID_ORDEN_GESTION = oga.ID_ORDEN_GESTION
            inner join aire.scr_acciones ac on ac.ID_ACCION = oga.ID_ACCION
            where og.ID_ORDEN = 93 and ac.IND_ACTIVO = 'S'
            group by og.ID_ORDEN
      ) h on h.ID_ORDEN = a.ID_ORDEN
      where a.id_orden = 2177
)
select 
      'Orden: ' || t.ID_ORDEN
      || ', fecha: ' || t.fechaejecucion
      || ', acta: ' || t.ACTA
      || ', tecnico: ' || t.nombre_contratista_persona
      || ', atendio: ' || t.Persona_Atiende
      || ', medidor: ' || t.Medidor
      || ', lectura: ' || t.lectura
      || ', ct: ' || t.mt
      || ', mt: ' || t.ct
      || ', mt: ' || t.ct
      || ', otros: ' || t.ListagFaltante
      as OBSERVACIONES
FROM t

-- select * from aire.ord_ordenes_gestion where ind_anomalia = 'N';

-- select a.id_orden,a.numero_orden,a.direcion
-- from aire.ord_ordenes a                    
-- where a.id_orden = 2177



---seccion datos del medidor encontrado en el inmueble
-- select  
--       nvl(a.id_orden,-1) as id_orden
--      ,a.numero_orden
--      ,d.numero_medidor
--      ,d.lectura
-- from aire.ord_ordenes a                    
-- left join aire.ord_ordenes_gestion              d  on d.id_orden = a.id_orden
-- where a.id_orden = 2177

-- select 
--        op.id_orden
--       ,op.id_serie
--       ,op.id_tipo_operacion
--       ,dv.descripcion
--       ,op.observacion
--       ,'sad fsad fsda \n asdf sdaf sda f\t asdf sadf asd' as Nota01
-- from aire.mtr_articulos_operacion op
-- left join aire.gnl_dominios_valor dv on dv.id_dominio_valor = op.id_tipo_operacion
-- where op.id_orden = 2177

-- SELECT 
--        GI.ID_ORDEN
--       ,CP.ID_CUESTIONARIO_PREGUNTA
--       ,CP.PREGUNTA
--       ,GIR.RESPUESTA      
-- FROM AIRE.GNL_CUESTIONARIOS_INSTANCIA GI
-- LEFT JOIN AIRE.GNL_CUESTIONARIOS_INSTANCIA_RESPUESTA GIR ON GIR.ID_CUESTIONARIO_INSTANCIA = GI.ID_CUESTIONARIO_INSTANCIA
-- LEFT JOIN AIRE.GNL_CUESTIONARIOS_PREGUNTA CP ON CP.ID_CUESTIONARIO_PREGUNTA = GIR.ID_CUESTIONARIO_PREGUNTA
-- WHERE GI.ID_ORDEN = 2177 AND GI.ID_CUESTIONARIO = 22

-- select * from aire.ord_ordenes_gestion
-- select * from aire.ord_ordenes_gestion_soporte
-- select * from aire.gnl_soportes where id_soporte=2065
-- select * from aire.gnl_tipos_soporte
select 
      s.*
      ,t.Nombre
from aire.gnl_soportes s
left join aire.gnl_tipos_soporte t on t.id_tipo_soporte = s.id_tipo_soporte
where id_soporte=2065

select 
       d.id_orden 
      ,ogs.id_soporte
from aire.ord_ordenes_gestion  d
left join aire.ord_ordenes_gestion_soporte ogs on ogs.ID_ORDEN_GESTION = d.ID_ORDEN_GESTION 
where d.id_orden = 373


-- select * 
-- from aire.gnl_soportes 
-- where id_soporte in (2065,2066,2067,2068,2069)



-- select * from aire.ord_ordenes_gestion
-- select * from aire.ord_ordenes_gestion_acciones
-- select * from aire.scr_acciones


-- select * from aire.mtr_articulos_operacion
-- select * from aire.gnl_dominios_valor dv where dv.id_dominio_valor = 581



-- select * from aire.gnl_cuestionarios
-- select * from aire.gnl_cuestionarios_pregunta where id_cuestionario = 22

-- select * from aire.gnl_cuestionarios_instancia where id_orden = 93
-- select * from aire.gnl_cuestionarios_instancia order by 1
-- select * from aire.gnl_cuestionarios_instancia_respuesta

-- select * from aire.ord_ordenes_gestion_soporte
-- select * from aire.gnl_soportes where id_soporte=2065


-- select * from aire.ord_ordenes
-- select * from aire.gnl_clientes


with t as (
    select
         RowNum as Id
        ,gci.id_orden
        ,a.id_cuestionario_instancia_soporte
        ,a.id_cuestionario_instancia
        ,a.id_soporte
        ,b.id_tipo_soporte
        ,c.codigo as Codigo_Tipo_Soporte
        ,b.Nombre
        ,b.peso
        ,b.id_usuario_registro
        ,case
            when mod(ROWNUM,2) = 0 then (SELECT ruta_web FROM aire.gnl_rutas_archivo_servidor where id_ruta_archivo_servidor = 8) || a.id_soporte
            else ''
        end PathPar
        ,case
            when mod(ROWNUM,2) = 1 then (SELECT ruta_web FROM aire.gnl_rutas_archivo_servidor where id_ruta_archivo_servidor = 8) || a.id_soporte
            else ''
        end PathImPar
      from aire.gnl_cuestionarios_instancia_soporte a
      left join aire.gnl_cuestionarios_instancia gci on gci.id_cuestionario_instancia = a.id_cuestionario_instancia
      left join aire.gnl_soportes b on a.id_soporte = b.id_soporte
      left join aire.gnl_tipos_soporte c on b.id_tipo_soporte = c.id_tipo_soporte ---and c.codigo = 'SFCUE'
    where gci.id_orden = 2177 and c.codigo = 'SSCUE' --SFCUE,SSCUE
)
select 
    x.*
from t x   
where x.PathImpar is not null
order by x.id




select a.* 
  from aire.gnl_cuestionarios_instancia_soporte a
where a.id_cuestionario_instancia in ( 
            select id_cuestionario_instancia
              from aire.gnl_cuestionarios_instancia
             where id_orden in (
                     select id_orden
                       from aire.ord_ordenes_gestion
                      where trunc(fecha_ejecucion) between trunc(to_date('2024/02/15')) and trunc(to_date('2024/02/16'))
                   ) 
       )
       

select * 
  from aire.gnl_cuestionarios_instancia_soporte a
  join aire.gnl_soportes b on a.id_soporte = b.id_soporte
  join aire.gnl_tipos_soporte c on b.id_tipo_soporte = c.id_tipo_soporte and c.codigo = 'SFCUE'
where id_cuestionario_instancia in ( 
            select id_cuestionario_instancia
              from aire.gnl_cuestionarios_instancia
             where id_orden in (
                     select id_orden
                       from aire.ord_ordenes_gestion
                      where trunc(fecha_ejecucion) between  '15/02/24' and '16/02/24'
                   ) 
       );       




select 
     gci.id_orden
    ,a.* 
    ,b.*
  from aire.gnl_cuestionarios_instancia_soporte a
  left join aire.gnl_cuestionarios_instancia gci on gci.id_cuestionario_instancia = a.id_cuestionario_instancia
  join aire.gnl_soportes b on a.id_soporte = b.id_soporte
  join aire.gnl_tipos_soporte c on b.id_tipo_soporte = c.id_tipo_soporte and c.codigo = 'SFCUE'
where gci.id_orden = 592676
order by a.id_cuestionario_instancia_soporte    