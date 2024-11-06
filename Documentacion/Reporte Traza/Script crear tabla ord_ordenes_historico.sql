SET SERVEROUTPUT ON;

--Borre la tabla primero para volverla a crear con los 3 primeros campos arriba
--DROP TABLE AIRE.ORD_ORDENES_HISTORICO;

--Creacion de la secuencia
CREATE SEQUENCE AIRE.SEC_ORD_ORDENES_HISTORICO;

CREATE TABLE AIRE.ORD_ORDENES_HISTORICO (
	ID_ORDEN_HISTORICO number default AIRE.SEC_ORD_ORDENES_HISTORICO.nextval,
	FECHA_REGISTRO_HISTORICO timestamp default sysdate,
	ESTADO_HISTORICO varchar2(100),
	ID_ORDEN NUMBER,
	ID_TIPO_ORDEN NUMBER(3,0),
	NUMERO_ORDEN VARCHAR2(100),
	ID_ESTADO_ORDEN NUMBER(3,0),
	ID_CONTRATISTA NUMBER(3,0),
	ID_CLIENTE NUMBER(38,0),
	ID_TERRITORIAL NUMBER(3,0),
	ID_ZONA NUMBER(3,0),
	DIRECION VARCHAR2(200),
	FECHA_CREACION TIMESTAMP,
	FECHA_CIERRE TIMESTAMP,
	ID_USUARIO_CIERRE NUMBER,
	DESCRIPCION VARCHAR2(2000),
	COMENTARIOS VARCHAR2(2000),
	ACTA VARCHAR2(250),
	ID_ACTIVIDAD NUMBER(3,0),
	ID_CONTRATISTA_PERSONA NUMBER,
	ID_TIPO_TRABAJO NUMBER,
	ID_TIPO_SUSPENCION NUMBER,
	ACTIVIDAD_ORDEN VARCHAR2(250),
	FECHA_ESTIMADA_RESPUESTA DATE,
	NUMERO_CAMP VARCHAR2(50),
	COMENTARIO_ORDEN_SERVICIO_NUM1 VARCHAR2(200),
	COMENTARIO_ORDEN_SERVICIO_NUM2 VARCHAR2(200),
	OBSERVACION_RECHAZO VARCHAR2(2000),
	FECHA_RECHAZO DATE,
	ORIGEN VARCHAR2(250),
	FECHA_REGISTRO TIMESTAMP,
	ID_BARRIO NUMBER,
	FECHA_ASIGNA_CONTRATISTA TIMESTAMP,
	FECHA_ASIGNA_TECNICO TIMESTAMP
);

--Agrego 8 campos extras:

alter table aire.ord_ordenes_historico
add nic NUMBER;

alter table aire.ord_ordenes_historico
add deuda NUMBER(38,2);

alter table aire.ord_ordenes_historico
add tecnico VARCHAR2(250);

alter table aire.ord_ordenes_historico
add departamento VARCHAR2(250);

alter table aire.ord_ordenes_historico
add municipio VARCHAR2(250);

alter table aire.ord_ordenes_historico
add numero_factura NUMBER;

alter table aire.ord_ordenes_historico
add tipo_brigada VARCHAR2(100);

alter table aire.ord_ordenes_historico
add antiguedad NUMBER; --Agrego 8 columnas

/*alter table aire.ord_ordenes_historico
add fecha_consulta timestamp default SYSTIMESTAMP;

alter table aire.ord_ordenes_historico
drop column fecha_consulta;*/

