---/---------------------------------------------------
Backup codigo acta scr antes del 17/07/2024
---/---------------------------------------------------
CREATE VIEW AIRE.V_RPTJASPER_ACTAORDEN_01_DSINICIAL
AS select
      nvl(a.id_orden,-1) as id_orden
     ,UPPER(c.localidad) as localidad
     ,nvl(q.acta,'0') as acta2
     ,(SELECT ruta_web FROM aire.gnl_rutas_archivo_servidor where id_ruta_archivo_servidor = 7) as Logo
--      ,(SELECT ruta_web FROM aire.gnl_rutas_archivo_servidor where id_ruta_archivo_servidor = 77) as Logo
     ,'NOTA: En caso de detectarse una(s) irregularidad(es), esta acta se constituye en una prueba documental de lo encontrado en sus instalaciones, por lo cual procede como tal ante el cliente o usuario del servicio de energia eléctrica.' || chr(13) || chr(13) || 'Señor suscriptor, usuario y/o propietario, con la firma del presente acta queda notificado que el uso indebido del servicio, la adulteración o manipulación y la reconexión sin autorización por fuera del equipo de medida, se constituye en el delito de "defraudación de fluidos" (articulo 256 del codigo penal).' || chr(13) || chr(13) || 'IMPORTANTE: favor acercarse a la oficina comercial de la empresa para gestionar su reconexión. Si la suspensión o corte fueron imputables a usted, deberá eliminar su causa y pagar la deuda, los intereses por mora, los cargos de reconexión e instalación y todos los gastos que demande el cobro prejudicial o judicial en el evento que sea necesario.' as Nota01
     , 'v_1.0.5 -- 11/04/2024' as version
from aire.ord_ordenes a
left join aire.gnl_clientes        c  on c.id_cliente = a.id_cliente
left join (
      select
              subq.id_orden
            , subq.acta
      from (
            select
                    qs.id_orden
                  , qs.acta
                  , row_number() over (partition by qs.id_orden order by qs.id_orden_gestion) as rn
            from aire.ord_ordenes_gestion qs
      ) subq
      where subq.rn = 1
)                                                     q       on q.id_orden = a.id_orden;

---/---------------------------------------------------
CREATE VIEW AIRE.V_RPTJASPER_ACTAORDEN_02_DSPRINCIPAL
( ID_ORDEN
, NUMERO_ORDEN
, CODIGO_TIPO_ORDEN
, DESCRIPCION_TIPO_ORDEN
, NIC
, TIPO_CLIENTE
, NOMBRE_CLIENTE
, DIRECCION
, NOMBRE_BARRIO
, GEORREFERENCIA
, MT
, CT
, CARGA_CONTRATADA
, TECNICO
, NOMBRE_CONTRATISTA_PERSONA
, FECHAEJECUCION
, FECHAINICIAL
, FECHAFINAL
)
AS select
      a.id_orden as id_orden
     ,a.numero_orden
     ,b.codigo_tipo_orden as codigo_tipo_orden
     ,b.descripcion as descripcion_tipo_orden
     ,c.nic
     ,upper(gdvq.descripcion) as tipo_cliente
     ,upper(c.nombre_cliente) as nombre_cliente
     ,upper(c.direccion) as direccion
     ,upper(nvl(f.nombre,' ')) as nombre_barrio
     , json_value(nvl(q.georreferencia,'{"latitud":"","longitud":""}'), '$.latitud') || ',' || json_value(nvl(q.georreferencia,'{"latitud":"","longitud":""}'), '$.longitud') as georreferencia
     ,upper(to_char(nvl(q.mt,' '))) as mt
     ,upper(to_char(nvl(q.ct,' '))) as ct
     ,upper(nvl(c.carga_contratada,' ')) as carga_contratada
--      ,upper(nvl(q.id_contratista_persona,'')) as tecnico
     ,upper(nvl(a.id_contratista_persona,'')) as tecnico
     ,upper(nvl(e1.nombre_contratista_persona,' ')) as nombre_contratista_persona
     ,nvl(q.fecha_ejecucion,to_date('1900/01/01','yyyy/mm/dd')) as fechaejecucion
     ,nvl(q.fecha_inicio_ejecucion,to_date('1900/01/01','yyyy/mm/dd')) as fechainicial
     ,nvl(q.fecha_fin_ejecucion,to_date('1900/01/01','yyyy/mm/dd')) as fechafinal
from aire.ord_ordenes a
left join (
      select
              subq.id_orden
            , subq.acta
            , subq.id_contratista_persona
            , subq.fecha_ejecucion
            , subq.fecha_inicio_ejecucion
            , subq.fecha_fin_ejecucion
            , subq.mt
            , subq.ct
            , subq.id_uso_energia
            , subq.georreferencia
      from (
            select
                  qs.id_orden
                , qs.acta
                , qs.id_contratista_persona
                , qs.fecha_ejecucion
                , qs.fecha_inicio_ejecucion
                , qs.fecha_fin_ejecucion
                , qs.mt
                , qs.ct
                , qs.id_uso_energia
                , qs.georreferencia
                , row_number() over (partition by qs.id_orden order by qs.id_orden_gestion) as rn
            from aire.ord_ordenes_gestion qs
      ) subq
      where subq.rn = 1
)                                               q  on q.id_orden = a.id_orden
left join aire.gnl_clientes                     c  on c.id_cliente = a.id_cliente
-- left join aire.v_ctn_contratistas_persona       e1 on e1.id_contratista_persona = q.id_contratista_persona
left join aire.v_ctn_contratistas_persona       e1 on e1.id_contratista_persona = a.id_contratista_persona
left join aire.ord_tipos_orden                  b  on b.id_tipo_orden = a.id_tipo_orden
left join aire.gnl_dominios_valor               gdvq    on gdvq.id_dominio_valor = q.id_uso_energia
left join aire.gnl_barrios                      f  on f.id_barrio = a.id_barrio;

---/---------------------------------------------------
CREATE VIEW AIRE.V_RPTJASPER_ACTAORDEN_03_DSDATOSMEDIDOS
AS select
      to_char(nvl(a.id_orden,-1)) as id_orden
     ,to_char(a.numero_orden) as numero_orden
     ,to_char(q.numero_medidor) as numero_medidor
     ,to_char(q.lectura) as lectura
from aire.ord_ordenes a
left join (
      select
              subq.id_orden
            , subq.numero_medidor
            , subq.lectura
      from (
            select
                  qs.id_orden
                , qs.numero_medidor
                , qs.lectura
                , row_number() over (partition by qs.id_orden order by qs.id_orden_gestion) as rn
            from aire.ord_ordenes_gestion qs
      ) subq
      where subq.rn = 1
)                                               q  on q.id_orden = a.id_orden;

---/---------------------------------------------------
CREATE VIEW AIRE.V_RPTJASPER_ACTAORDEN_04_DSSELLOS
( ID_ORDEN
, ID_SERIE
, ID_TIPO_OPERACION
, DESCRIPCION
, OBSERVACION
)
AS select
       nvl(to_char(op.id_orden),' ') as id_orden
      ,nvl(to_char(op.id_serie),' ') as id_serie
      ,nvl(to_char(op.id_tipo_operacion), ' ') as id_tipo_operacion
      ,dv.descripcion
      ,op.observacion
from aire.mtr_articulos_operacion op
left join aire.gnl_dominios_valor dv on dv.id_dominio_valor = op.id_tipo_operacion;

---/---------------------------------------------------
CREATE VIEW AIRE.V_RPTJASPER_ACTAORDEN_05_DSCONTROL
AS select
       gi.id_orden
      ,cp.id_cuestionario_pregunta
      ,cp.pregunta
      ,case when dbms_lob.substr(gir.respuesta, 4000, 1) = 'true' then 'SI' else 'NO' end as respuesta
      ,gi.id_cuestionario
from aire.gnl_cuestionarios_instancia gi
left join aire.gnl_cuestionarios_instancia_respuesta gir on gir.id_cuestionario_instancia = gi.id_cuestionario_instancia
left join aire.gnl_cuestionarios_pregunta cp on cp.id_cuestionario_pregunta = gir.id_cuestionario_pregunta
where gi.id_cuestionario = 22;

---/---------------------------------------------------
CREATE VIEW AIRE.V_RPTJASPER_ACTAORDEN_06_DSOBSERVACIONES
AS with t as (
      select
             a.id_orden
            ,q.fecha_ejecucion as fechaejecucion
            ,nvl(q.acta,'0') as acta
            ,e1.nombre_contratista_persona
            ,q.NOMBRES_PERSONA_ATIENDE as Persona_Atiende
            ,q.NUMERO_MEDIDOR as Medidor
            ,q.LECTURA as Lectura
            ,q.mt as mt
            ,q.ct as ct
            ,h.ListagFaltante
      from aire.ord_ordenes a
      left join aire.gnl_clientes                     c  on c.id_cliente = a.id_cliente
      left join (
              select
                      subq.id_orden
                    , subq.fecha_ejecucion
                    , subq.acta
                    , subq.NOMBRES_PERSONA_ATIENDE
                    , subq.NUMERO_MEDIDOR
                    , subq.LECTURA
                    , subq.mt
                    , subq.ct
                    , subq.id_contratista_persona
              from (
                    select
                          qs.id_orden
                        , qs.fecha_ejecucion
                        , qs.acta
                        , qs.NOMBRES_PERSONA_ATIENDE
                        , qs.NUMERO_MEDIDOR
                        , qs.LECTURA
                        , qs.mt
                        , qs.ct
                        , qs.id_contratista_persona
                        , row_number() over (partition by qs.id_orden order by qs.id_orden_gestion) as rn
                    from aire.ord_ordenes_gestion qs
              ) subq
              where subq.rn = 1
      )                                               q  on q.id_orden = a.id_orden
      left join aire.v_ctn_contratistas_persona       e1 on e1.id_contratista_persona = q.id_contratista_persona
      left join (
            select
                  og.ID_ORDEN
                  ,LISTAGG(ac.CODIGO || ': ' || oga.OBSERVACION, '; ') as ListagFaltante
            from aire.ord_ordenes_gestion_acciones oga
            inner join aire.ord_ordenes_gestion og on og.ID_ORDEN_GESTION = oga.ID_ORDEN_GESTION
            inner join aire.scr_acciones ac on ac.ID_ACCION = oga.ID_ACCION
            where
                --og.ID_ORDEN = $P{Ds05IdOrden} and
                ac.IND_ACTIVO = 'S'
            group by og.ID_ORDEN
      ) h on h.ID_ORDEN = a.ID_ORDEN
      --where a.id_orden = $P{Ds05IdOrden}
)
select
      t.id_orden
      ,'Orden: ' || t.ID_ORDEN
      || ', fecha: ' || t.fechaejecucion
      || ', acta: ' || t.ACTA
      || ', tecnico: ' || t.nombre_contratista_persona
      || ', atendio: ' || t.Persona_Atiende
      || ', medidor: ' || t.Medidor
      || ', lectura: ' || t.lectura
      || ', ct: ' || t.ct
      || ', mt: ' || t.mt
      || ', otros: ' || t.ListagFaltante
      as OBSERVACIONES
FROM t;

---/---------------------------------------------------
CREATE VIEW AIRE.V_RPTJASPER_ACTAORDEN_07_DSOBSERVACIONES
AS with t as (
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
        /*,case
            when mod(ROWNUM,2) = 0 then (SELECT ruta_web FROM aire.gnl_rutas_archivo_servidor where id_ruta_archivo_servidor = $P{DsImg}) || a.id_soporte
            else ''
        end PathPar
        ,case
            when mod(ROWNUM,2) = 1 then (SELECT ruta_web FROM aire.gnl_rutas_archivo_servidor where id_ruta_archivo_servidor = $P{DsImg}) || a.id_soporte
            else ''
        end PathImPar*/
      from aire.gnl_cuestionarios_instancia_soporte a
      left join aire.gnl_cuestionarios_instancia gci on gci.id_cuestionario_instancia = a.id_cuestionario_instancia
      left join aire.gnl_soportes b on a.id_soporte = b.id_soporte
      left join aire.gnl_tipos_soporte c on b.id_tipo_soporte = c.id_tipo_soporte ---and c.codigo = 'SFCUE'
    where c.codigo = 'SSCUE'
    ---gci.id_orden = $P{Ds06IdOrden}
)
select
    x."ID",x."ID_ORDEN",x."ID_CUESTIONARIO_INSTANCIA_SOPORTE",x."ID_CUESTIONARIO_INSTANCIA",x."ID_SOPORTE",x."ID_TIPO_SOPORTE",x."CODIGO_TIPO_SOPORTE",x."NOMBRE",x."PESO",x."ID_USUARIO_REGISTRO"
from t x;

---/---------------------------------------------------
---/---------------------------------------------------
---/---------------------------------------------------
---/---------------------------------------------------