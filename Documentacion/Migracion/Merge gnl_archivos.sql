SET SERVEROUTPUT ON;

/*
Nota: Para el codigo de actividad, para quemarlo en el excel, primero correr el siguiente query en el la bd de destino para hallar el codigo de SCR
        SELECT
            id_actividad
        FROM aire.gnl_actividades where prefijo = 'G_SCR' and ind_activo = 'S' 
*/
    SET DEFINE OFF --Ejecutar  primero este comando para evitar q tome el & como variable de sustitucion
    merge into (
        SELECT *
        FROM aire.gnl_archivos x
        where x.id_actividad in (
            SELECT
                id_actividad
            FROM aire.gnl_actividades where prefijo = 'G_SCR' and ind_activo = 'S'    
        )
    ) destino
    using (
        select 321 as id_archivo,101 as id_actividad,'Cargue reasignación de ordenes ' as nombre,'Cargue reasignación de ordenes' as descripcion,'N' as ind_longitud_fija,'' as caracter_separador,0  as maximo_errores,'ord_ordenes_cargue_temporal_cierre' as tabla_destino, 'S' as ind_activo,'PRSO' as codigo, 0 as filas_encabezado from dual	union all
        select 322 as id_archivo,101 as id_actividad,'Cargue cierre de ordenes ' as nombre,'Cargue cierre de ordenes ' as descripcion,'N' as ind_longitud_fija,'' as caracter_separador,0  as maximo_errores,'ord_ordenes_cargue_temporal_cierre' as tabla_destino, 'S' as ind_activo,'MCIO' as codigo, 0 as filas_encabezado from dual	union all
        select 323 as id_archivo,101 as id_actividad,'Cargue Asignacion Tecnicos' as nombre,'Cargue Asignacion Tecnicos' as descripcion,'N' as ind_longitud_fija,'' as caracter_separador,0  as maximo_errores,'ord_ordenes_temporal_asignacion_tecnico' as tabla_destino, 'S' as ind_activo,'MASG' as codigo, 0 as filas_encabezado from dual	union all
        select 324 as id_archivo,101 as id_actividad,'Cargue DesAsignacion Tecnicos' as nombre,'Cargue DesAsignacion Tecnicos' as descripcion,'N' as ind_longitud_fija,'' as caracter_separador,0  as maximo_errores,'ord_ordenes_temporal_desasignacion_tecnico' as tabla_destino, 'S' as ind_activo,'MDSG' as codigo, 0 as filas_encabezado from dual	union all
        select 281 as id_archivo,101 as id_actividad,'Masivo Ordenes Area Central' as nombre,'Cargue masivo de ordenes en area central' as descripcion,'N' as ind_longitud_fija,'' as caracter_separador,0  as maximo_errores,'ord_ordenes' as tabla_destino, 'S' as ind_activo,'OTAC' as codigo, 0 as filas_encabezado from dual 
    ) origen
    on (
        origen.codigo = destino.codigo AND destino.id_actividad = 101
    )
    WHEN MATCHED THEN
        UPDATE SET      
                destino.nombre = origen.nombre,
                destino.descripcion = origen.descripcion,
                destino.ind_longitud_fija = origen.ind_longitud_fija,
                destino.caracter_separador = origen.caracter_separador,
                destino.maximo_errores = origen.maximo_errores,
                destino.tabla_destino = origen.tabla_destino,
                destino.ind_activo = origen.ind_activo,
                destino.filas_encabezado = origen.filas_encabezado
                        
    WHERE     
              origen.nombre != destino.nombre OR
              origen.descripcion != destino.descripcion OR
              origen.ind_longitud_fija != destino.ind_longitud_fija OR
              origen.caracter_separador != destino.caracter_separador OR
              origen.maximo_errores != destino.maximo_errores OR
              origen.tabla_destino != destino.tabla_destino OR
              origen.ind_activo != destino.ind_activo OR
              origen.filas_encabezado != destino.filas_encabezado               
    WHEN NOT MATCHED THEN
    INSERT (ID_ARCHIVO,CODIGO,ID_ACTIVIDAD, NOMBRE, DESCRIPCION, IND_LONGITUD_FIJA, CARACTER_SEPARADOR, MAXIMO_ERRORES, TABLA_DESTINO, IND_ACTIVO, FILAS_ENCABEZADO)
    VALUES (origen.id_archivo,origen.codigo,origen.id_actividad, origen.nombre, origen.descripcion, origen.ind_longitud_fija, origen.caracter_separador, origen.maximo_errores, origen.tabla_destino, origen.ind_activo, origen.filas_encabezado);
    /


