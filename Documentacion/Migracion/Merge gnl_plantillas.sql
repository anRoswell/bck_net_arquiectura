SET SERVEROUTPUT ON;

    merge into (
        select *
        from aire.gnl_plantillas
        where id_plantilla >= 62
    ) destino
    using (
        select 6 as id_plantilla,'ACDE' as codigo,'Plantilla de Cargue para la actualización de las deudas de clientes en campañas de cobro persuasivo' as nombre,'Plantilla de Cargue para la actualización de las deudas de clientes en campañas de cobro persuasivo' as descripcion,'application/vnd.ms-excel' as extension,'Plantilla actualización de deuda.xlsx'  as nombre_archivo,82 as id_usuario_registro,'23/11/23 08:19:47,254209000 AM' as fecha_registro from dual	union all
        select 63 as id_plantilla,'LOG01' as codigo,'Logo Ejemplo 01' as nombre,'Logo Ejemplo 01' as descripcion,'image/png' as extension,'logo-aire.png'  as nombre_archivo,61 as id_usuario_registro,'09/02/24 11:32:54,309698000 AM' as fecha_registro from dual	union all
        select 64 as id_plantilla,'LOG02' as codigo,'Logo Ejemplo 01' as nombre,'Logo Ejemplo 02' as descripcion,'image/png' as extension,'logo2.png'  as nombre_archivo,61 as id_usuario_registro,'09/02/24 11:32:54,309698000 AM' as fecha_registro from dual	union all
        select 41 as id_plantilla,'CNTP' as codigo,'Plantilla cargue personas contratistas' as nombre,'Plantilla cargue personas contratistas' as descripcion,'application/vnd.ms-excel' as extension,'Plantilla Contratistas Persona.xlsx'  as nombre_archivo,82 as id_usuario_registro,'10/01/24 02:55:19,271426000 PM' as fecha_registro from dual	union all
        select 42 as id_plantilla,'CNTV' as codigo,'Plantilla cargue vehiculos contratistas' as nombre,'Plantilla cargue vehiculos contratistas' as descripcion,'application/vnd.ms-excel' as extension,'Plantilla Contratistas Vehículos.xlsx'  as nombre_archivo,82 as id_usuario_registro,'10/01/24 03:01:08,220385000 PM' as fecha_registro from dual	union all
        select 43 as id_plantilla,'CNTT' as codigo,'Plantilla cargue turnos contratistas' as nombre,'Plantilla cargue turnos contratistas' as descripcion,'application/vnd.ms-excel' as extension,'Plantilla Contratista Turnos.xlsx'  as nombre_archivo,82 as id_usuario_registro,'10/01/24 03:01:08,323030000 PM' as fecha_registro from dual	union all
        select 61 as id_plantilla,'ACSMS' as codigo,'Plantilla cargue series a movimiento' as nombre,'Plantilla cargue series a movimiento' as descripcion,'application/vnd.ms-excel' as extension,'Plantilla_cargue_series_a_movimientos.xlsx'  as nombre_archivo,61 as id_usuario_registro,'18/01/24 03:01:08,524794000 PM' as fecha_registro from dual	union all
        select 103 as id_plantilla,'PRSOR' as codigo,'Plantilla reasignación de ordenes SCR' as nombre,'Plantilla reasignación de ordenes SCR' as descripcion,'application/vnd.ms-excel' as extension,'Plantilla_reasignación de ordenes.xlsx'  as nombre_archivo,61 as id_usuario_registro,'28/09/23 07:47:08,878582000 AM' as fecha_registro from dual	union all
        select 104 as id_plantilla,'PCORD' as codigo,'Plantilla cierre de ordenes SCR' as nombre,'Plantilla cierre de ordenes SCR' as descripcion,'application/vnd.ms-excel' as extension,'Plantilla_cierre.xlsx'  as nombre_archivo,61 as id_usuario_registro,'28/09/23 07:47:08,878582000 AM' as fecha_registro from dual	union all
        select 105 as id_plantilla,'PASGT' as codigo,'Plantilla Asignacion a Tecnicos SCR' as nombre,'Plantilla Asignacion a Tecnicos SCR' as descripcion,'application/vnd.ms-excel' as extension,'Plantilla_Asignacion_Tecnicos.xlsx'  as nombre_archivo,61 as id_usuario_registro,'28/09/23 07:47:08,878582000 AM' as fecha_registro from dual	union all
        select 106 as id_plantilla,'PDSGT' as codigo,'Plantilla DesAsignacion a Tecnicos SCR' as nombre,'Plantilla DesAsignacion a Tecnicos SCR' as descripcion,'application/vnd.ms-excel' as extension,'Plantilla_DesAsignacion_Tecnicos.xlsx'  as nombre_archivo,61 as id_usuario_registro,'28/09/23 07:47:08,878582000 AM' as fecha_registro from dual	union all
        select 2 as id_plantilla,'RECA' as codigo,'Plantilla cargue masivo gestion campaña' as nombre,'Plantilla cargue masivo gestion campaña' as descripcion,'application/vnd.ms-excel' as extension,'Plantilla cargue masivo gestion campaña.csv'  as nombre_archivo,82 as id_usuario_registro,'22/11/23 03:53:18,132816000 PM' as fecha_registro from dual	union all
        select 3 as id_plantilla,'CGCM' as codigo,'Plantilla cargue de campaña' as nombre,'Plantilla cargue de campaña' as descripcion,'application/vnd.ms-excel' as extension,'Plantilla cargue de Campaña.xlsx'  as nombre_archivo,82 as id_usuario_registro,'22/11/23 03:53:18,132816000 PM' as fecha_registro from dual	union all
        select 4 as id_plantilla,'ELCL' as codigo,'Plantilla eliminar clientes de una campaña' as nombre,'Plantilla eliminar clientes de una campaña' as descripcion,'application/vnd.ms-excel' as extension,'Plantilla eliminar clientes de una campaña.xlsx'  as nombre_archivo,82 as id_usuario_registro,'22/11/23 04:06:44,314402000 PM' as fecha_registro from dual	union all
        select 5 as id_plantilla,'ASIG' as codigo,'Plantilla asignacion de BackOffice-Gestores' as nombre,'Plantilla asignacion de BackOffice-Gestores' as descripcion,'application/vnd.ms-excel' as extension,'Plantilla asignacion de BackOffice-Gestores.xlsx'  as nombre_archivo,82 as id_usuario_registro,'22/11/23 04:05:16,638322000 PM' as fecha_registro from dual	union all
        select 62 as id_plantilla,'CRMAO' as codigo,'Plantilla Cargue Manual Masivo de Ordenes SCR' as nombre,'Plantilla para cargar masivamente ordenes en el submódulo Area Central de SCR' as descripcion,'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' as extension,'Plantilla Orden Manual Masivo.xlsx'  as nombre_archivo,82 as id_usuario_registro,'09/02/24 11:32:54,309698000 AM' as fecha_registro from dual	union all
        select 81 as id_plantilla,'GPCM' as codigo,'Plantilla cargue masivo GOS' as nombre,'Plantilla cargue masivo GOS' as descripcion,'application/vnd.ms-excel' as extension,'Plantilla cargue masivo GOS.xlsx'  as nombre_archivo,82 as id_usuario_registro,'10/01/24 02:55:19,271426000 PM' as fecha_registro from dual	union all
        select 1 as id_plantilla,'ACMMS' as codigo,'Plantilla cargue materiales a movimiento' as nombre,'Plantilla cargue materiales a movimiento' as descripcion,'application/vnd.ms-excel' as extension,'Plantilla_reasignacion.xlsx'  as nombre_archivo,61 as id_usuario_registro,'28/09/23 07:47:08,878582000 AM' as fecha_registro from dual
    ) origen
    on (
        origen.codigo = destino.codigo AND origen.id_plantilla >= 62
    )
    WHEN MATCHED THEN
        UPDATE SET      
                destino.nombre = origen.nombre,
                destino.descripcion = origen.descripcion,
                destino.extension = origen.extension,
                destino.nombre_archivo = origen.nombre_archivo,
                destino.id_usuario_registro = origen.id_usuario_registro,
                destino.fecha_registro = origen.fecha_registro     
    WHERE     
                origen.id_plantilla >= 62 AND
                origen.nombre != destino.nombre OR
                origen.descripcion != destino.descripcion OR
                origen.extension != destino.extension OR
                origen.nombre_archivo != destino.nombre_archivo OR
                origen.id_usuario_registro != destino.id_usuario_registro OR
                origen.fecha_registro != destino.fecha_registro               
    WHEN NOT MATCHED THEN
    INSERT (ID_PLANTILLA, CODIGO, NOMBRE, DESCRIPCION, ARCHIVO, EXTENSION, NOMBRE_ARCHIVO, ID_USUARIO_REGISTRO, FECHA_REGISTRO)
    VALUES (origen.id_plantilla, origen.codigo, origen.nombre, origen.descripcion, EMPTY_BLOB(), origen.extension, origen.nombre_archivo, origen.id_usuario_registro, origen.fecha_registro)
    WHERE origen.id_plantilla >= 62
    /




