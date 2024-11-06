SET SERVEROUTPUT ON;

    MERGE INTO aire.gnl_peticiones_url_origen destino
    USING (
        select 1 as id_peticion_url_origen,'unknow' as encabezado_peticion_origen,'1' as permitir_acceso,'7777777' as usuario_registra,'04/01/24 04:51:24,216494000 PM' as fecha_registra,''  as usuario_ultima_modificacion,'' as fecha_ultima_modificacion from dual	union all
        select 2 as id_peticion_url_origen,'unknow2' as encabezado_peticion_origen,'1' as permitir_acceso,'7777777' as usuario_registra,'04/01/24 04:53:45,596885000 PM' as fecha_registra,''  as usuario_ultima_modificacion,'' as fecha_ultima_modificacion from dual	union all
        select 22 as id_peticion_url_origen,'http://localhost:8100' as encabezado_peticion_origen,'1' as permitir_acceso,'7777777' as usuario_registra,'05/01/24 05:56:02,864063000 PM' as fecha_registra,''  as usuario_ultima_modificacion,'' as fecha_ultima_modificacion from dual	union all
        select 42 as id_peticion_url_origen,'http://localhost:8083' as encabezado_peticion_origen,'1' as permitir_acceso,'7777777' as usuario_registra,'09/01/24 05:01:07,498279000 PM' as fecha_registra,''  as usuario_ultima_modificacion,'' as fecha_ultima_modificacion from dual	union all
        select 61 as id_peticion_url_origen,'http://localhost:4200' as encabezado_peticion_origen,'1' as permitir_acceso,'7777777' as usuario_registra,'31/01/24 07:46:35,715537000 PM' as fecha_registra,''  as usuario_ultima_modificacion,'' as fecha_ultima_modificacion from dual	union all
        select 104 as id_peticion_url_origen,'Postman' as encabezado_peticion_origen,'1' as permitir_acceso,'7777777' as usuario_registra,'02/02/24 09:36:59,510722000 PM' as fecha_registra,''  as usuario_ultima_modificacion,'' as fecha_ultima_modificacion from dual	union all
        select 41 as id_peticion_url_origen,'https://op360.air-e.com:8083' as encabezado_peticion_origen,'1' as permitir_acceso,'7777777' as usuario_registra,'09/01/24 04:55:53,852178000 PM' as fecha_registra,''  as usuario_ultima_modificacion,'' as fecha_ultima_modificacion from dual	union all
        select 81 as id_peticion_url_origen,'http://www.postman.com' as encabezado_peticion_origen,'1' as permitir_acceso,'7777777' as usuario_registra,'01/02/24 03:49:31,542769000 PM' as fecha_registra,''  as usuario_ultima_modificacion,'' as fecha_ultima_modificacion from dual	union all
        select 123 as id_peticion_url_origen,'https://localhost:44326/Swagger/index.html' as encabezado_peticion_origen,'1' as permitir_acceso,'7777777' as usuario_registra,'03/02/24 01:41:11,778960000 PM' as fecha_registra,''  as usuario_ultima_modificacion,'' as fecha_ultima_modificacion from dual	union all
        select 201 as id_peticion_url_origen,'https://op360dev.air-e.com:8082' as encabezado_peticion_origen,'1' as permitir_acceso,'7777777' as usuario_registra,'17/03/24 12:43:08,024091000 PM' as fecha_registra,''  as usuario_ultima_modificacion,'' as fecha_ultima_modificacion from dual	union all
        select 101 as id_peticion_url_origen,'https://op360dev.air-e.com:8083' as encabezado_peticion_origen,'1' as permitir_acceso,'7777777' as usuario_registra,'02/02/24 05:19:12,966288000 PM' as fecha_registra,''  as usuario_ultima_modificacion,'' as fecha_ultima_modificacion from dual	union all
        select 102 as id_peticion_url_origen,'https://localhost' as encabezado_peticion_origen,'1' as permitir_acceso,'7777777' as usuario_registra,'02/02/24 06:16:42,472061000 PM' as fecha_registra,''  as usuario_ultima_modificacion,'' as fecha_ultima_modificacion from dual	union all
        select 103 as id_peticion_url_origen,'http://10.20.16.43:8082' as encabezado_peticion_origen,'1' as permitir_acceso,'7777777' as usuario_registra,'02/02/24 06:20:27,717873000 PM' as fecha_registra,''  as usuario_ultima_modificacion,'' as fecha_ultima_modificacion from dual	union all
        select 141 as id_peticion_url_origen,'http://10.20.16.43:8083' as encabezado_peticion_origen,'1' as permitir_acceso,'7777777' as usuario_registra,'13/02/24 08:12:44,829095000 AM' as fecha_registra,''  as usuario_ultima_modificacion,'' as fecha_ultima_modificacion from dual	union all
        select 161 as id_peticion_url_origen,'https://e1d3-8-242-126-60.ngrok-free.app' as encabezado_peticion_origen,'1' as permitir_acceso,'7777777' as usuario_registra,'08/03/24 02:48:21,659493000 PM' as fecha_registra,''  as usuario_ultima_modificacion,'' as fecha_ultima_modificacion from dual	union all
        select 162 as id_peticion_url_origen,'https://48d8-8-242-126-60.ngrok-free.app' as encabezado_peticion_origen,'1' as permitir_acceso,'7777777' as usuario_registra,'08/03/24 03:04:12,700195000 PM' as fecha_registra,''  as usuario_ultima_modificacion,'' as fecha_ultima_modificacion from dual	union all
        select 163 as id_peticion_url_origen,'https://5df7-138-84-41-219.ngrok-free.app' as encabezado_peticion_origen,'1' as permitir_acceso,'7777777' as usuario_registra,'08/03/24 03:29:38,957347000 PM' as fecha_registra,''  as usuario_ultima_modificacion,'' as fecha_ultima_modificacion from dual	union all
        select 121 as id_peticion_url_origen,'Swagger' as encabezado_peticion_origen,'1' as permitir_acceso,'7777777' as usuario_registra,'03/02/24 11:29:23,655109000 AM' as fecha_registra,''  as usuario_ultima_modificacion,'' as fecha_ultima_modificacion from dual	union all
        select 122 as id_peticion_url_origen,'https://localhost:44326/index.html' as encabezado_peticion_origen,'1' as permitir_acceso,'7777777' as usuario_registra,'03/02/24 01:38:24,411551000 PM' as fecha_registra,''  as usuario_ultima_modificacion,'' as fecha_ultima_modificacion from dual	union all
        select 181 as id_peticion_url_origen,'http://localhost:8082' as encabezado_peticion_origen,'1' as permitir_acceso,'7777777' as usuario_registra,'15/03/24 07:43:07,824237000 AM' as fecha_registra,''  as usuario_ultima_modificacion,'' as fecha_ultima_modificacion from dual	union all
        select 21 as id_peticion_url_origen,'https://localhost:44326' as encabezado_peticion_origen,'1' as permitir_acceso,'7777777' as usuario_registra,'05/01/24 04:14:06,555511000 PM' as fecha_registra,''  as usuario_ultima_modificacion,'' as fecha_ultima_modificacion from dual	union all
        select 82 as id_peticion_url_origen,'http://www.excel.com' as encabezado_peticion_origen,'1' as permitir_acceso,'7777777' as usuario_registra,'01/02/24 05:41:23,863019000 PM' as fecha_registra,''  as usuario_ultima_modificacion,'' as fecha_ultima_modificacion from dual
    ) origen
    ON (
        destino.ENCABEZADO_PETICION_ORIGEN = origen.encabezado_peticion_origen
    )
    WHEN MATCHED THEN
        UPDATE SET      destino.PERMITIR_ACCESO = origen.permitir_acceso,
                        destino.USUARIO_REGISTRA = origen.usuario_registra,
                        destino.FECHA_REGISTRA = origen.fecha_registra,
                        destino.USUARIO_ULTIMA_MODIFICACION = origen.usuario_ultima_modificacion,
                        destino.FECHA_ULTIMA_MODIFICACION = origen.fecha_ultima_modificacion
                        
    WHERE               destino.PERMITIR_ACCESO != origen.permitir_acceso OR
                        destino.USUARIO_REGISTRA != origen.usuario_registra OR
                        destino.FECHA_REGISTRA != origen.fecha_registra OR
                        destino.USUARIO_ULTIMA_MODIFICACION != origen.usuario_ultima_modificacion OR
                        destino.FECHA_ULTIMA_MODIFICACION != origen.fecha_ultima_modificacion
                        
    WHEN NOT MATCHED THEN
    INSERT (ENCABEZADO_PETICION_ORIGEN, PERMITIR_ACCESO, USUARIO_REGISTRA, FECHA_REGISTRA, USUARIO_ULTIMA_MODIFICACION, FECHA_ULTIMA_MODIFICACION)
    VALUES (origen.encabezado_peticion_origen, origen.permitir_acceso, origen.usuario_registra, origen.fecha_registra, origen.usuario_ultima_modificacion, origen.fecha_ultima_modificacion);

