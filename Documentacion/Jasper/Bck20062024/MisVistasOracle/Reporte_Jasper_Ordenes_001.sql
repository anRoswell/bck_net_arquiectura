create or replace view aire.v_rptjasper_actaorden_01_DsInicial
AS 
select 
      nvl(a.id_orden,-1) as id_orden
     ,UPPER(c.localidad) as localidad
     ,nvl(q.acta,'0') as acta2
     ,(SELECT ruta_web FROM aire.gnl_rutas_archivo_servidor where id_ruta_archivo_servidor = 7) as Logo
--      ,(SELECT ruta_web FROM aire.gnl_rutas_archivo_servidor where id_ruta_archivo_servidor = 77) as Logo
     ,'NOTA: En caso de detectarse una(s) irregularidad(es), esta acta se constituye en una prueba documental de lo encontrado en sus instalaciones, por lo cual procede como tal ante el cliente o usuario del servicio de energia eléctrica.' || chr(13) || chr(13) || 'Señor suscriptor, usuario y/o propietario, con la firma del presente acta queda notificado que el uso indebido del servicio, la adulteración o manipulación y la reconexión sin autorización por fuera del equipo de medida, se constituye en el delito de "defraudación de fluidos" (articulo 256 del codigo penal).' || chr(13) || chr(13) || 'IMPORTANTE: favor acercarse a la oficina comercial de la empresa para gestionar su reconexión. Si la suspensión o corte fueron imputables a usted, deberá eliminar su causa y pagar la deuda, los intereses por mora, los cargos de reconexión e instalación y todos los gastos que demande el cobro prejudicial o judicial en el evento que sea necesario.' as Nota01
     , 'v_1.0.10 -- 05/06/2024' as version
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
-- where v.id_orden = $P{prIdOrden}
/*
select v.* from aire.v_rptjasper_actaorden_01_DsInicial v where v.id_orden = 
*/

create or replace view aire.v_rptjasper_actaorden_02_DsPrincipal
AS 
select 
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

-- where a.id_orden = $P{Ds01IdOrden}

create or replace view aire.v_rptjasper_actaorden_03_DsDatosMedidos
AS
select
      to_char(nvl(a.id_orden,-1)) as id_orden
     ,to_char(a.numero_orden) as numero_orden
     ,case when nvl(q.numero_medidor,'-1') = '-1' then ' ' else to_char(q.numero_medidor) end as numero_medidor
     ,case when nvl(q.lectura,'-1') = '-1' then ' ' else to_char(q.lectura) end as lectura
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
-- where a.id_orden = $P{Ds02IdOrden}

create or replace view aire.v_rptjasper_actaorden_04_DsSellos
AS
select
       nvl(to_char(op.id_orden),' ') as id_orden
      ,nvl(to_char(op.id_serie),' ') as id_serie
      ,nvl(to_char(op.id_tipo_operacion), ' ') as id_tipo_operacion
      ,dv.descripcion
      ,art.Nombre observacion
      ,art.Codigo_Sap
      ,op.Cantidad
from aire.mtr_articulos_operacion op
left join aire.mtr_articulos art on art.id_articulo = op.id_articulo
left join aire.gnl_dominios_valor dv on dv.id_dominio_valor = op.id_tipo_operacion
where art.ind_activo = 'S' AND art.Nombre like '%SELLO%'; 
--AND op.id_orden = 5202861
-- where op.id_orden = $P{Ds03IdOrden}

create or replace view aire.v_rptjasper_actaorden_09_DsMaterialInst
AS
select
       nvl(to_char(op.id_orden),' ') as id_orden
      ,nvl(to_char(op.id_serie),' ') as id_serie
      ,nvl(to_char(op.id_tipo_operacion), ' ') as id_tipo_operacion
      ,dv.descripcion
      ,art.Nombre observacion
      ,art.Codigo_Sap
      ,op.Cantidad
      ,ROWNUM as ITEM      
from aire.mtr_articulos_operacion op
left join aire.mtr_articulos art on art.id_articulo = op.id_articulo
left join aire.gnl_dominios_valor dv on dv.id_dominio_valor = op.id_tipo_operacion
where art.ind_activo = 'S' AND art.Nombre not like '%SELLO%'; 
--AND op.id_orden = 5202861
-- where op.id_orden = $P{Ds03IdOrden}

select
       op.*
from aire.v_rptjasper_actaorden_09_DsMaterialInst op
where op.id_orden = $P{Ds09IdOrden}  ;

create or replace view aire.v_rptjasper_actaorden_05_DsControl
AS      
select
       gi.id_orden
      ,cp.id_cuestionario_pregunta
      ,cp.pregunta
      ,case when dbms_lob.substr(gir.respuesta, 4000, 1) = 'true' then 'SI' else 'NO' end as respuesta
      ,gi.id_cuestionario
from aire.gnl_cuestionarios_instancia gi
left join aire.gnl_cuestionarios_instancia_respuesta gir on gir.id_cuestionario_instancia = gi.id_cuestionario_instancia
left join aire.gnl_cuestionarios_pregunta cp on cp.id_cuestionario_pregunta = gir.id_cuestionario_pregunta
where gi.id_cuestionario = 22;


  create or replace view aire.v_rptjasper_actaorden_06_DsObservaciones
AS
with t as (
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


   create or replace view aire.v_rptjasper_actaorden_07_DsObservaciones
AS
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
    x.*
from t x;

  create or replace view aire.v_rptjasper_actaorden_08_DsTrabajos
AS
      select 
            a.id_orden as id_orden
      ,q.id_anomalia
      ,q.id_subanomalia
      ,q.descripcion
      ,q.item
      from aire.ord_ordenes a
      left join (
      select 
            qs.id_orden                  
            , qs.id_anomalia
            , qs.id_subanomalia
            , a.descripcion
            , to_char(qs.id_anomalia) as item
      from aire.ord_ordenes_gestion qs
      left join aire.ord_anomalias a on a.id_anomalia = qs.id_anomalia
      union all
      select 
            qs.id_orden                  
            , qs.id_anomalia
            , qs.id_subanomalia
            , b.descripcion
            , qs.id_anomalia || '-' || nvl(to_char(qs.id_subanomalia),' ') || ' ' as item
      from aire.ord_ordenes_gestion qs
      left join aire.ord_subanomalias b on b.id_anomalia = qs.id_anomalia and b.id_subanomalia = qs.id_subanomalia         
      )                            q  on q.id_orden = a.id_orden
      left join aire.gnl_clientes  c  on c.id_cliente = a.id_cliente;


select 
      v.id_orden
     ,v.id_anomalia
     ,v.id_subanomalia
     ,v.descripcion
     ,v.item
from aire.v_rptjasper_actaorden_08_DsTrabajos v
where v.id_orden = 5202842;


/*
Funcion que retorna las imagenes
*/
-- Crear un tipo de registro
/*
      CREATE OR REPLACE TYPE aire.record_imagenes_jasper_type AS OBJECT (
            id NUMBER,
            id_orden NUMBER,
            id_soporte NUMBER,
            Accion VARCHAR2(30),
            Codigo VARCHAR2(30),
            Clasificacion VARCHAR2(30),
            PathImPar VARCHAR2(300),
            PathPar VARCHAR2(300)
      );
*/
-- Crear un tipo de tabla basado en el tipo de registro
--- DROP TYPE aire.tbl_acta_imagenes_jasper_type
-- CREATE OR REPLACE TYPE aire.tbl_acta_imagenes_jasper_type AS TABLE OF aire.record_imagenes_jasper_type;

-- Crear la función pipelinable
  CREATE OR REPLACE FUNCTION aire.fnc_consultar_imagenes_acta_jasper(
    prm_id_ruta_archivo_servidor NUMBER,
    prm_IdOrden NUMBER
      )
      RETURN aire.tbl_acta_imagenes_jasper_type PIPELINED IS
BEGIN
    FOR r IN 
    (
            with ts as 
            (
                  select 'ACCION_VS' as Accion, 'SAVS' as Codigo, 'Imagen' Clasificacion from dual 	        union all
                  select 'ACCION_VM' as Accion, 'SAVM' as Codigo, 'Imagen' Clasificacion from dual 	        union all
                  select 'ACCION_RCS' as Accion, 'SARCS' as Codigo, 'Imagen' Clasificacion from dual 	        union all
                  select 'ACCION_RI' as Accion, 'SARI' as Codigo, 'Imagen' Clasificacion from dual 	        union all
                  select 'ACCION_MDV' as Accion, 'SMDV' as Codigo, 'Imagen' Clasificacion from dual 	        union all
                  select 'FIRMA_TECNICO' as Accion, 'SFT' as Codigo, 'Firma' Clasificacion from dual 	        union all
                  select 'FIRMA_TESTIGO' as Accion, 'SFTG' as Codigo, 'Firma' Clasificacion from dual 	    union all
                  select 'FIRMA_USUARIO' as Accion, 'SFU' as Codigo, 'Firma' Clasificacion from dual 	        union all
                  select 'FIRMA_PARTICULAR' as Accion, 'SFP' as Codigo, 'Firma' Clasificacion from dual       union all
                  select 'CUESTIONARIO_FIRMA' as Accion, 'SFCUE' as Codigo, 'Firma' Clasificacion from dual 	union all
                  select 'CUESTIONARIO_FOTO' as Accion, 'SSCUE' as Codigo, 'Imagen' Clasificacion from dual union all
                  select 'GESTION_FOTO' as Accion, 'SOAN' as Codigo, 'Imagen' Clasificacion from dual 
            )
            ,t as 
            (
                  select 
                        n.id_orden
                        ,n.id_soporte
                        ,n.Accion
                        ,n.Codigo
                        ,n.Clasificacion
                        ,rownum as numrow
                  from 
                  (
                        select
                              gci.id_orden
                              ,a.id_soporte
                              ,ts.Accion
                              ,ts.Codigo
                              ,ts.Clasificacion
                        from aire.gnl_cuestionarios_instancia_soporte a
                        left join aire.gnl_cuestionarios_instancia gci on gci.id_cuestionario_instancia = a.id_cuestionario_instancia
                        left join aire.gnl_soportes b on a.id_soporte = b.id_soporte
                        left join aire.gnl_tipos_soporte c on b.id_tipo_soporte = c.id_tipo_soporte
                        left join ts on ts.Codigo = c.codigo
                        where ts.Clasificacion = 'Imagen' and gci.id_orden = prm_IdOrden --5193701
                        union all
                        SELECT
                              ges.Id_Orden
                              ,a.id_soporte
                              ,ts.Accion
                              ,ts.Codigo
                              ,ts.Clasificacion
                        FROM aire.ord_ordenes_gestion_soporte a
                        left join aire.ord_ordenes_gestion ges on ges.id_orden_gestion = a.id_orden_gestion
                        left join aire.gnl_soportes b on a.id_soporte = b.id_soporte
                        left join aire.gnl_tipos_soporte c on b.id_tipo_soporte = c.id_tipo_soporte
                        left join ts on ts.Codigo = c.codigo
                        where ts.Clasificacion = 'Imagen' and ges.id_orden = prm_IdOrden
                  ) n
            ),
            t2 as 
            (
                  select
                        t.numrow as Id        
                        ,t.id_orden
                        ,t.id_soporte
                        ,t.Accion
                        ,t.Codigo
                        ,t.Clasificacion
                        ,case
                              when mod(t.numrow,2) = 1 then (SELECT ruta_web FROM aire.gnl_rutas_archivo_servidor where id_ruta_archivo_servidor = prm_id_ruta_archivo_servidor) || t.id_soporte
                              else ''
                         end PathImPar
                        ,case
                              when mod(t.numrow,2) = 0 then (SELECT ruta_web FROM aire.gnl_rutas_archivo_servidor where id_ruta_archivo_servidor = prm_id_ruta_archivo_servidor) || t.id_soporte
                              else ''
                         end PathPar
                  from t
            )
            select t2.* from t2
            --where t2.PathImpar is not null
            order by t2.Id     
    ) 
    LOOP
      PIPE ROW (aire.record_imagenes_jasper_type(r.id,r.id_orden,r.id_soporte,r.Accion,r.Codigo,r.Clasificacion,r.PathImPar,r.PathPar));
    END LOOP;
    RETURN;
END fnc_consultar_imagenes_acta_jasper;
/

select * 
from aire.fnc_consultar_imagenes_acta_jasper(88,5193701) t2
where t2.PathImpar is not null
order by t2.Id;


/*
Funcion que retorna las firmas
*/
-- Crear un tipo de registro
/*
      CREATE OR REPLACE TYPE aire.record_firmas_jasper_type AS OBJECT (
            id NUMBER,
            id_orden NUMBER,
            id_soporte NUMBER,
            Accion VARCHAR2(30),
            Codigo VARCHAR2(30),
            Clasificacion VARCHAR2(30),

            Identificacion VARCHAR2(30),
            Nombre VARCHAR2(30),
            Cargo VARCHAR2(30),
            
            PathImPar VARCHAR2(300),
            PathPar VARCHAR2(300)
      );

*/
-- Crear un tipo de tabla basado en el tipo de registro
--- DROP TYPE aire.tbl_acta_firmas_jasper_type
-- CREATE OR REPLACE TYPE aire.tbl_acta_firmas_jasper_type AS TABLE OF aire.record_firmas_jasper_type;

-- Crear la función pipelinable
  CREATE OR REPLACE FUNCTION aire.fnc_consultar_firmas_acta_jasper(
    prm_id_ruta_archivo_servidor NUMBER,
    prm_IdOrden NUMBER
      )
      RETURN aire.tbl_acta_firmas_jasper_type PIPELINED IS
BEGIN
    FOR r IN 
    (
      with ts as 
      (
      select 'ACCION_VS' as Accion, 'SAVS' as Codigo, 'Imagen' Clasificacion from dual 	        union all
      select 'ACCION_VM' as Accion, 'SAVM' as Codigo, 'Imagen' Clasificacion from dual 	        union all
      select 'ACCION_RCS' as Accion, 'SARCS' as Codigo, 'Imagen' Clasificacion from dual 	    union all
      select 'ACCION_RI' as Accion, 'SARI' as Codigo, 'Imagen' Clasificacion from dual 	        union all
      select 'ACCION_MDV' as Accion, 'SMDV' as Codigo, 'Imagen' Clasificacion from dual 	    union all
      select 'FIRMA_TECNICO' as Accion, 'SFT' as Codigo, 'Firma' Clasificacion from dual 	    union all
      select 'FIRMA_TESTIGO' as Accion, 'SFTG' as Codigo, 'Firma' Clasificacion from dual 	    union all
      select 'FIRMA_USUARIO' as Accion, 'SFU' as Codigo, 'Firma' Clasificacion from dual 	    union all
      select 'FIRMA_PARTICULAR' as Accion, 'SFP' as Codigo, 'Firma' Clasificacion from dual     union all
      select 'CUESTIONARIO_FIRMA' as Accion, 'SFCUE' as Codigo, 'Firma' Clasificacion from dual union all
      select 'CUESTIONARIO_FOTO' as Accion, 'SSCUE' as Codigo, 'Imagen' Clasificacion from dual union all
      select 'GESTION_FOTO' as Accion, 'SOAN' as Codigo, 'Imagen' Clasificacion from dual 
      )
      ,t as 
      (
            select 
                  n.id_orden
                  ,n.id_soporte
                  ,n.Accion
                  ,n.Codigo
                  ,n.Clasificacion
                  ,case 
                        when n.codigo = 'SFT' then n.identificacion_contratista_persona 
                        when n.codigo = 'SFTG' then TO_CHAR(n.identificacion_testigo)
                        when n.codigo = 'SFU' or n.codigo = 'SFP' then TO_CHAR(n.identificacion_persona_atiende)
                  else '0' end as Identificacion
                  ,case 
                        when n.codigo = 'SFT' then n.nombre_contratista_persona 
                        when n.codigo = 'SFTG' then n.nombres_testigo
                        when n.codigo = 'SFU' or n.codigo = 'SFP' then n.nombres_persona_atiende
                  else '' end as Nombre   
                  ,case 
                        when n.codigo = 'SFT' then 'TECNICO'
                        when n.codigo = 'SFTG' then 'TESTIGO'
                        when n.codigo = 'SFU' then 'USUARIO'
                        when n.codigo = 'SFP' then 'PARTICULAR'
                  else '' end as Cargo    
                  ,rownum as numrow
            from 
            (
                  select
                        gci.id_orden
                        ,a.id_soporte
                        ,ts.Accion
                        ,ts.Codigo
                        ,ts.Clasificacion
                        --//--- usuario
                        ,q.identificacion_persona_atiende
                        ,q.nombres_persona_atiende
                        --//--- zona tecnico
                        ,o.id_contratista_persona
                        ,vc.identificacion_contratista_persona
                        ,vc.nombre_contratista_persona
                        --//--- testigo
                        ,q.identificacion_testigo
                        ,q.nombres_testigo
                  from aire.gnl_cuestionarios_instancia_soporte a
                  left join aire.gnl_cuestionarios_instancia gci on gci.id_cuestionario_instancia = a.id_cuestionario_instancia
                  left join aire.gnl_soportes b on a.id_soporte = b.id_soporte
                  left join aire.gnl_tipos_soporte c on b.id_tipo_soporte = c.id_tipo_soporte
                  left join aire.ord_ordenes o on o.id_orden = gci.id_orden
                  left join aire.v_ctn_contratistas_persona vc on vc.id_contratista_persona = o.id_contratista_persona
                  left join (
                        select 
                              subq.*
                        from (
                        select 
                              qs.*
                              ,row_number() over (partition by qs.id_orden order by qs.id_orden_gestion) as rn
                        from aire.ord_ordenes_gestion qs
                        where id_orden = prm_IdOrden
                        ) subq
                        where subq.rn = 1
                  ) q on q.id_orden = gci.id_orden
                  left join ts on ts.Codigo = c.codigo
                  where ts.Clasificacion = 'Firma' and gci.id_orden = prm_IdOrden
            ) n
      ),
      t2 as 
      (
      select
            t.numrow as Id        
            ,t.id_orden
            ,t.id_soporte
            ,t.Accion
            ,t.Codigo
            ,t.Clasificacion
            ,NVL(UPPER(t.Identificacion),'Sin Identificacion') AS Identificacion
            ,NVL(UPPER(t.Nombre),'Sin Nombre') AS Nombre
            ,NVL(UPPER(t.Cargo),' ') AS Cargo
            ,case
                  when mod(t.numrow,2) = 1 then (SELECT ruta_web FROM aire.gnl_rutas_archivo_servidor where id_ruta_archivo_servidor = prm_id_ruta_archivo_servidor) || t.id_soporte
                  else ''
            end PathImPar
            ,case
                  when mod(t.numrow,2) = 0 then (SELECT ruta_web FROM aire.gnl_rutas_archivo_servidor where id_ruta_archivo_servidor = prm_id_ruta_archivo_servidor) || t.id_soporte
                  else ''
            end PathPar
      from t
      )
      select * from t2    
    ) 
    LOOP
      PIPE ROW (aire.record_firmas_jasper_type(r.id,r.id_orden,r.id_soporte,r.Accion,r.Codigo,r.Clasificacion,r.Identificacion,r.Nombre,r.Cargo,r.PathImPar,r.PathPar));
    END LOOP;
    RETURN;
END fnc_consultar_firmas_acta_jasper;
/

select * 
from aire.fnc_consultar_firmas_acta_jasper(88,5193691) t2
where t2.PathImpar is not null
order by t2.Id;

/*
--- Ultima modificacion: CFV 04/06/2024 04:42 PM
*/