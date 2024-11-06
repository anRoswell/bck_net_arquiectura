with ts as (
    select 'TECNICO' as Cargox, 'FIRMA_TECNICO' as Accion, 'SFT' as Codigo, 'Firma' Clasificacion from dual 	        union all
    select 'TESTIGO' as Cargox, 'FIRMA_TESTIGO' as Accion, 'SFTG' as Codigo, 'Firma' Clasificacion from dual 	    union all
    select 'USUARIO' as Cargox, 'FIRMA_USUARIO' as Accion, 'SFU' as Codigo, 'Firma' Clasificacion from dual 	        union all
    select 'PARTICULAR' as Cargox, 'FIRMA_PARTICULAR' as Accion, 'SFP' as Codigo, 'Firma' Clasificacion from dual       union all
    select 'na' as Cargox, 'CUESTIONARIO_FIRMA' as Accion, 'SFCUE' as Codigo, 'Firma' Clasificacion from dual
),
a as (
    select
         x.Id
        ,x.id_orden
        ,x.id_soporte
        ,x.Accion
        ,x.Codigo
        ,x.Clasificacion
        ,x.Identificacion
        ,x.Nombre
        ,x.Cargo
        ,x.PathImPar
        ,x.PathPar
        ,x.PathImPar || x.PathPar as Pathx
    from aire.fnc_consultar_firmas_acta_jasper($P{DsImg},$P{Ds08IdOrden}) x
),
t as (
    select
        t2.Id
        ,t2.id_orden
        ,t2.id_soporte
        ,t2.Accion
        ,t2.Codigo
        ,t2.Clasificacion
        ,t2.Identificacion
        ,t2.Nombre
        ,t2.Cargo
        ,t2.PathImPar
        ,t2.PathPar
        ,t2.Pathx
    from a t2
    where case
    when t2.PathImpar is not null and t2.Codigo = $P{CodigoPr} then 1
    when   t2.PathPar is not null and t2.Codigo = $P{CodigoPr} then 1
    --Paraq mostrar los SFCUE registro 1
    when t2.PathImpar is not null and $P{CodigoPr} = 'SFU' and t2.Codigo = 'SFCUE' and t2.Id = 1 then 1
    when   t2.PathPar is not null and $P{CodigoPr} = 'SFU' and t2.Codigo = 'SFCUE' and t2.Id = 1 then 1
    --Paraq mostrar los SFCUE registro 2
    when t2.PathImpar is not null and $P{CodigoPr} = 'SFT' and t2.Codigo = 'SFCUE' and t2.Id = 2 then 1
    when   t2.PathPar is not null and $P{CodigoPr} = 'SFT' and t2.Codigo = 'SFCUE' and t2.Id = 2 then 1
    --Paraq mostrar los SFCUE registro 3
    when t2.PathImpar is not null and $P{CodigoPr} = 'SFTG' and t2.Codigo = 'SFCUE' and t2.Id = 3 then 1
    when   t2.PathPar is not null and $P{CodigoPr} = 'SFTG' and t2.Codigo = 'SFCUE' and t2.Id = 3 then 1
    --Paraq mostrar los SFCUE registro 4
    when t2.PathImpar is not null and $P{CodigoPr} = 'SFP' and t2.Codigo = 'SFCUE' and t2.Id = 4 then 1
    when   t2.PathPar is not null and $P{CodigoPr} = 'SFP' and t2.Codigo = 'SFCUE' and t2.Id = 4 then 1
    else 0 end = 1
    order by t2.Id
),
t2x AS (
    SELECT 
        id,id_orden,id_soporte,Codigo,Clasificacion,Identificacion,Nombre
        ,(select case when (select count(1) from a where a.Codigo = 'SFCUE') > 0 THEN 'xx' else t.Cargo end from ts where Codigo = $P{CodigoPr}) as Cargo
        ,PathImPar,PathPar,Pathx 
    FROM t WHERE ROWNUM > 0
    UNION ALL
    SELECT 
         0 as id,0 as id_orden,0 as id_soporte,'' as Codigo,'' as Clasificacion,'na' as Identificacion,'na' as Nombre
        ,(select case when (select count(1) from a where a.Codigo = 'SFCUE') > 0 THEN 'xx' else Cargox end from ts where Codigo = $P{CodigoPr}) as Cargo
        ,'' as PathImPar,'' as PathPar,'' as Pathx 
    FROM DUAL 
    WHERE NOT EXISTS (SELECT 1 FROM t)
)
select * from t2x