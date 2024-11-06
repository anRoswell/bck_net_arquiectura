/*
Aca van todos los cambios realizados en la base de op360 para realizarlos en produccion o en pruebas
*/
--001)---> Crear las siguientes tablas:
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
--002)---> Crear secuencia --- aire.sec_ord_ordenes_cargue_tmp
    ---CREATE SEQUENCE aire.sec_ord_ordenes_cargue_temporal INCREMENT BY 1 MAXVALUE 9999999999999999999999999999 MINVALUE 1 CACHE 20;
--- la secuencia ya no se utiliza
--003)---> Crear la tabla:
    create table aire.ord_ordenes_cargue_temporal(
        orden    					number          constraint  ord_ordenes_cargue_temporal_orden_nn     					not null,
        nic 						varchar2(100),
        nis 						varchar2(100),
        codigo_tipo_orden			varchar2(100),
        codigo_tipo_suspencion		varchar2(100),
        codigo_estado_servicio		varchar2(100),        
        id_soporte					number			constraint  ord_ordenes_cargue_temporal_id_soporte_nn					not null,
        con_errores					varchar2(1)     default     0
													constraint  ord_ordenes_cargue_temporal_con_errores_nn            		not null,
        usuario_registra			varchar2(50)	constraint  ord_ordenes_cargue_temporal_usuario_registra_nn			    not null,
        fecha_registra				timestamp		default     localtimestamp 
													constraint  ord_ordenes_cargue_temporal_fecha_registra_nn				not null,
        numero_orden				varchar2(100),
        desc_validacion             varchar2(4000)	default     'Sin Validacion'
    );
	ALTER TABLE aire.ord_ordenes_cargue_temporal ADD NUM_CAMP VARCHAR2(1000) NULL;
	ALTER TABLE aire.ord_ordenes_cargue_temporal ADD COMENT_OS VARCHAR2(2000) NULL;
	ALTER TABLE aire.ord_ordenes_cargue_temporal ADD COMENT_OS2 VARCHAR2(2000) NULL;
	ALTER TABLE aire.ord_ordenes_cargue_temporal ADD ID_CONTRATISTA VARCHAR2(100) NULL;
	ALTER TABLE aire.ord_ordenes_cargue_temporal ADD CODIGO_CONTRATISTA VARCHAR2(100) NULL;
	ALTER TABLE aire.ord_ordenes_cargue_temporal ADD ID_ZONA NUMBER NULL;
	ALTER TABLE aire.ord_ordenes_cargue_temporal ADD NIC_REPETIDO NUMBER NULL;
	
	--EL NIS no es obligatorio.
	--	constraint  ord_ordenes_cargue_temporal_nis_nn   						not null
		constraint  ord_ordenes_cargue_temporal_codigo_tipo_orden_nn 			not null
		constraint  ord_ordenes_cargue_temporal_codigo_tipo_suspencion_nn		not null
		constraint  ord_ordenes_cargue_temporal_codigo_estado_servicio_nn		not null
	
--004)--> a la tabla gnl_soportes se le corrige la secuencia. la correcta es AIRE.SEC_GNL_SOPORTES.NEXTVAL

--005)--> a la talba ord_ordenes se le permite nulos al campo id_contratista realizando la eliminacion del constrain:
    
    alter table aire.ord_ordenes drop constraint ord_ordenes_id_contratista_nn;

--006)--> a la tabla aire.scr_tipos_suspencion se le corrige la secuencia. la correcta es aire.sec_scr_tipos_suspencion.nextval  

--007)--> se inserto un nuevo registro en la tabla aire.gnl_archivos para utilizarlo en el cargue masivo.

Ejecutar merge archivo: "Merge gnl_archivos.sql"

--008)--> añadir campo id_soporte a la tabla aire.gnl_archivos_instancia 

alter table aire.gnl_archivos_instancia add id_soporte number;


--009)--> Crear el tipo de dato

create or replace TYPE num_list_type AS TABLE OF NUMBER;

--010)--> Se insertan datos de parametros en AIRE.GNL_DOMINIOS y AIRE.GNL_DOMINIOS_VALOR

INSERT INTO AIRE.GNL_DOMINIOS (ID_DOMINIO,NOMBRE,DESCRIPCION,ID_ACTIVIDAD,TIPO_DATO,IND_ACTIVO,CODIGO) 
SELECT 383 AS ID_DOMINIO,
           'Parametros Op360 Angular' AS NOMBRE,
           'Parametros Op360 Angular' AS DESCRIPCION,
           101 AS ID_ACTIVIDAD,
           'C' AS TIPO_DATO,
           'S' AS IND_ACTIVO,
           'PARAM' AS CODIGO
FROM dual a
WHERE NOT EXISTS (
    SELECT 1
    FROM AIRE.GNL_DOMINIOS
    WHERE CODIGO = 'PARAM'
);

INSERT INTO AIRE.GNL_DOMINIOS_VALOR (ID_DOMINIO_VALOR, ID_DOMINIO, DESCRIPCION, VALOR, IND_ACTIVO, CODIGO)
SELECT 641 AS ID_DOMINIO_VALOR,
       383 AS ID_DOMINIO,
       'estados_orden' AS DESCRIPCION,
       '25,81' AS VALOR,
       'S' AS IND_ACTIVO,
       'PRM01' AS CODIGO
FROM dual
WHERE NOT EXISTS (
    SELECT 1
    FROM AIRE.GNL_DOMINIOS_VALOR
    WHERE CODIGO = 'PRM01'
);

--011)--> Se insertan datos de path de archivos	(ya lo habia hecho)

ejecutar merge en archivo: "Merge gnl_rutas_archivo_servidor.sql"

--012)--> se cambia el tipo de dato de la columna DIRECION de varchar2(100) a varchar2(200)
alter table aire.ord_ordenes modify DIRECION VARCHAR2(200 BYTE)

--014)--> Insertar relacion tipo suspencion y tipo brigada para tabla "aire.scr_tipos_suspencion_tipo_brigada"
select * from aire.scr_tipos_suspencion
select * from aire.ctn_tipos_brigada
select * from aire.scr_tipos_suspencion_tipo_brigada
	
	Nota: Esta relacion se realiza de manera manual segun excel compartido "Archivo Mejora  Bloqueantes SCR.xlsx" hoja "Matriz Brigada"

--015)--> merge de la tabla gnl_plantillas, el codigo va a ser mi llave

	Ejecutar Merge archivo: "Excel gnl_plantillas.xlsx"
	
	Nota: Subir los archivos de plantilla de cargue masivo:	

--016)--> merge gnl_peticiones_url_origen

	Ejectuar Merge archivo: "Merge gnl_peticiones_url_origen.sql"

--017)--> validar que la tabla AIRE.SCR_TIPOS_SUSPENCION_TIPO_BRIGADA exista en destino (qa)




















