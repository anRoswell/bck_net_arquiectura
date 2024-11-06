create sequence aire.sec_gnl_peticiones_url_origen;
create table aire.gnl_peticiones_url_origen(
	id_peticion_url_origen  number              default     aire.sec_gnl_peticiones_url_origen.nextval
                                                constraint  gnl_peticiones_url_origen_id_peticion_url_origen_pk     primary key
                                                constraint  gnl_peticiones_url_origen_id_peticion_url_origen_nn     not null,
    encabezado_peticion_origen  varchar2(1000)  constraint  gnl_peticiones_url_origen_encabezado_peticion_origen_nn not null,
    permitir_acceso             varchar2(1)     constraint  gnl_peticiones_url_origen_permitir_acceso_nn            not null
                                                constraint  gnl_peticiones_url_origen_permitir_acceso_ck            check(permitir_acceso in ('0', '1')),
    usuario_registra			varchar2(50)	constraint  gnl_peticiones_url_origen_usuario_registra_nn			not null,
    fecha_registra				timestamp		default     systimestamp		
                                                constraint  gnl_peticiones_url_origen_fecha_registra_nn				not null,
    usuario_ultima_modificacion	varchar2(50),
    fecha_ultima_modificacion	timestamp
);


create sequence aire.sec_gnl_rutas_archivo_servidor;
create table aire.gnl_rutas_archivo_servidor(
	id_ruta_archivo_servidor    number          default     aire.sec_gnl_rutas_archivo_servidor.nextval
                                                constraint  gnl_rutas_archivo_servidor_id_ruta_archivo_servidor_pk  primary key
                                                constraint  gnl_rutas_archivo_servidor_id_ruta_archivo_servidor_nn  not null,
    id_aplicacion               number,
    ruta_web                    varchar2(255)   constraint  gnl_rutas_archivo_servidor_ruta_web_nn                  not null,
    ruta_red                    varchar2(255)   constraint  gnl_rutas_archivo_servidor_ruta_red_nn                  not null,
    ruta_red_archivo            varchar2(255)   constraint  gnl_rutas_archivo_servidor_ruta_red_archivo_nn          not null,
    ruta_web_archivo            varchar2(255)   constraint  gnl_rutas_archivo_servidor_ruta_web_archivo_nn          not null,
    observacion                 varchar2(200),
    estado                      varchar2(1)     constraint  gnl_rutas_archivo_servidor_estado_nn            not null
                                                constraint  gnl_rutas_archivo_servidor_estado_ck            check(estado in ('0', '1'))
);


create sequence aire.sec_gnl_peticiones_cors;
create table aire.gnl_peticiones_cors(
    id_peticion_cors            number          default     aire.sec_gnl_peticiones_cors.nextval
                                                constraint  gnl_peticiones_cors_id_peticion_cors_pk             primary key
                                                constraint  gnl_peticiones_cors_id_peticion_cors_nn             not null,
    encabezado_peticion_origen  varchar2(1000)  constraint  gnl_peticiones_cors_encabezado_peticion_origen_nn   not null,
    token                       varchar2(1000),
    grupo                       varchar2(100),
    nombre_controlador          varchar2(100),
    metodo_accion               varchar2(100),
    typo_metodo                 varchar2(100),
    usuario_registra			varchar2(50)	constraint  gnl_peticiones_cors_usuario_registra_nn			    not null,
    fecha_registra				timestamp		default     systimestamp		
                                                constraint  gnl_peticiones_cors_fecha_registra_nn				not null
);


create or replace trigger aire.dsp_gnl_peticiones_cors_gst
	for insert on aire.gnl_peticiones_cors
	compound trigger
        v_cantidad  number;
    before each row is
        begin
          select count(*)
            into v_cantidad
            from aire.gnl_peticiones_url_origen
           where encabezado_peticion_origen = :new.encabezado_peticion_origen;

        if v_cantidad = 0 then
            insert into aire.gnl_peticiones_url_origen ( encabezado_peticion_origen, permitir_acceso, usuario_registra, fecha_registra)
                                                values ( :new.encabezado_peticion_origen, 0, '7777777', systimestamp);
        end if;
    end before each row;
end;
/

//-- secuencia --- aire.sec_ord_ordenes_cargue_tmp
CREATE SEQUENCE aire.sec_ord_ordenes_cargue_tmp INCREMENT BY 1 MAXVALUE 9999999999999999999999999999 MINVALUE 1 CACHE 20;

create table aire.ord_ordenes_cargue_tmp(
    id_ord_ordenes_cargue_tmp   number          	default     aire.sec_ord_ordenes_cargue_tmp.nextval
													constraint  gnl_ord_ordenes_cargue_tmp_id_ord_ordenes_cargue_tmp_pk             primary key
													constraint  gnl_ord_ordenes_cargue_tmp_id_ord_ordenes_cargue_tmp_nn             not null,
    nic 						varchar2(100 BYTE)	constraint  gnl_ord_ordenes_cargue_tmp_nic_nn   								not null,
	nis 						varchar2(100 BYTE)	constraint  gnl_ord_ordenes_cargue_tmp_nis_nn   								not null,
	id_tipo_orden				varchar2(100 BYTE)	constraint  gnl_ord_ordenes_cargue_tmp_id_tipo_orden_nn 						not null,
	id_tipo_suspencion			varchar2(100 BYTE)	constraint  gnl_ord_ordenes_cargue_tmp_id_tipo_suspencion_nn					not null,
	id_estado_servicio			varchar2(100 BYTE)	constraint  gnl_ord_ordenes_cargue_tmp_id_estado_servicio_nn					not null,
	
	id_soporte					number				constraint  gnl_ord_ordenes_cargue_tmp_id_soporte_nn							not null,
	con_errores					varchar2(1)     	constraint  gnl_ord_ordenes_cargue_tmp_con_errores_nn            				not null
													constraint  gnl_ord_ordenes_cargue_tmp_con_errores_ck            				check(estado in ('0', '1')),
	
	usuario_registra			varchar2(50)		constraint  gnl_ord_ordenes_cargue_usuario_registra_nn			    			not null,
    fecha_registra				timestamp			default     systimestamp		
													constraint  gnl_ord_ordenes_cargue_fecha_registra_nn							not null
);