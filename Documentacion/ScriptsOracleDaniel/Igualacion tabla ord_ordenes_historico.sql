SET SERVEROUTPUT ON;

--Borre la tabla primero para volverla a crear con los 3 primeros campos arriba
--DROP TABLE AIRE.ORD_ORDENES_HISTORICO;

--Creacion de la secuencia
CREATE SEQUENCE AIRE.SEC_ORD_ORDENES_HISTORICO;

CREATE TABLE AIRE.ORD_ORDENES_HISTORICO (
	ID_ORDEN_HISTORICO number default AIRE.SEC_ORD_ORDENES_HISTORICO.nextval,
	FECHA_REGISTRO_HISTORICO timestamp default SYSTIMESTAMP,
	ESTADO_HISTORICO varchar2(100),
	ANTIGUEDAD NUMBER,
	FECHA_INGRESO_OP360 DATE,
	ID_ORDEN NUMBER,
	NUMERO_ORDEN VARCHAR2(100),
	ID_CLIENTE NUMBER,
	NIC VARCHAR2(100),
	ID_TIPO_ORDEN NUMBER(3,0),
	CODIGO_TIPO_ORDEN VARCHAR2(50),
	DESCRIPCION_TIPO_ORDEN VARCHAR2(100),
	DEUDA NUMBER,
	ID_CONTRATISTA_PERSONA NUMBER,
	NOMBRE_CONTRATISTA_PERSONA VARCHAR2(500),
	ID_DEPARTAMENTO NUMBER,
	NOMBRE_DEPARTAMENTO VARCHAR2(250),
	ID_MUNICIPIO NUMBER,
	NOMBRE_MUNICIPIO VARCHAR2(250),
	ID_BARRIO NUMBER,
	NOMBRE_BARRIO VARCHAR2(250),
	ID_ESTADO_ORDEN NUMBER,
	DESCRIPCION_ESTADO_ORDEN VARCHAR2(250),
	FECHA_ASIGNACION DATE,
	FECHA_GENERACION DATE,
	ID_CONTRATISTA NUMBER,
	NOMBRE_CONTRATISTA VARCHAR2(500),
	ID_TERRITORIAL NUMBER,
	NOMBRE_TERRITORIAL VARCHAR2(250),
	NUMERO_FACTURAS NUMBER,
	FECHA_CONSULTA DATE,
	ID_TIPO_BRIGADA NUMBER,
	DESCRIPCION_TIPO_BRIGADA VARCHAR2(250),
	COMENTARIO_ORDEN VARCHAR2(1000),
	ID_TIPO_SUSPENCION NUMBER,
	DESCRICPION_TIPO_SUSPENCION VARCHAR2(250),
	CONSTRAINT ORD_ORDENES_HISTORICO_ID_ORDEN_NN CHECK ("ID_ORDEN" IS NOT NULL)
);

-- AIRE.ORD_ORDENES_HISTORICO foreign keys

ALTER TABLE AIRE.ORD_ORDENES_HISTORICO ADD CONSTRAINT ORD_ORDENES_HISTORICO_ID_ORDEN_FK FOREIGN KEY (ID_ORDEN) REFERENCES AIRE.ORD_ORDENES(ID_ORDEN);


--elimino las restricciones de la tabla ord_ordenes_historico
alter table aire.ord_ordenes_historico
drop constraint ord_ordenes_historico_id_orden_fk;

alter table aire.ord_ordenes_historico
drop constraint ord_ordenes_historico_id_orden_nn;


--igualo la tabla aire.ord_ordenes_historico con la tabla aire.ord_ordenes

--Elimino los campos que sobran en la tabla aire.ord_ordenes_historico y que no estan en la tabla aire.ord_ordenes:
alter table aire.ord_ordenes_historico
drop column antiguedad;

alter table aire.ord_ordenes_historico
drop column fecha_ingreso_op360;

alter table aire.ord_ordenes_historico
drop column nic;

alter table aire.ord_ordenes_historico
drop column codigo_tipo_orden;

alter table aire.ord_ordenes_historico
drop column descripcion_tipo_orden;

alter table aire.ord_ordenes_historico
drop column deuda;

alter table aire.ord_ordenes_historico
drop column nombre_contratista_persona;

alter table aire.ord_ordenes_historico
drop column id_departamento;

alter table aire.ord_ordenes_historico
drop column nombre_departamento;

alter table aire.ord_ordenes_historico
drop column id_municipio;

alter table aire.ord_ordenes_historico
drop column nombre_municipio;

alter table aire.ord_ordenes_historico
drop column nombre_barrio;

alter table aire.ord_ordenes_historico
drop column descripcion_estado_orden;

alter table aire.ord_ordenes_historico
drop column fecha_asignacion;

alter table aire.ord_ordenes_historico
drop column fecha_generacion;

alter table aire.ord_ordenes_historico
drop column nombre_contratista;

alter table aire.ord_ordenes_historico
drop column nombre_territorial;

alter table aire.ord_ordenes_historico
drop column numero_facturas;

alter table aire.ord_ordenes_historico
drop column fecha_consulta;

alter table aire.ord_ordenes_historico
drop column id_tipo_brigada;

alter table aire.ord_ordenes_historico
drop column descripcion_tipo_brigada;

alter table aire.ord_ordenes_historico
drop column comentario_orden;

alter table aire.ord_ordenes_historico
drop column descricpion_tipo_suspencion; --Elimino 21 columnas


--Agrego los campos que no existen en la tabla aire.ord_ordenes_historico y si estan en la tabla aire.ord_ordenes:
alter table aire.ord_ordenes_historico
add id_zona	NUMBER;

alter table aire.ord_ordenes_historico
add direcion VARCHAR2(200);

alter table aire.ord_ordenes_historico
add fecha_creacion TIMESTAMP(6);

alter table aire.ord_ordenes_historico
add fecha_cierre TIMESTAMP(6);

alter table aire.ord_ordenes_historico
add id_usuario_cierre NUMBER;

alter table aire.ord_ordenes_historico
add descripcion	VARCHAR2(2000);

alter table aire.ord_ordenes_historico
add comentarios	VARCHAR2(2000);

alter table aire.ord_ordenes_historico
add acta VARCHAR2(250);

alter table aire.ord_ordenes_historico
add id_actividad NUMBER;

alter table aire.ord_ordenes_historico
add id_tipo_trabajo NUMBER;

alter table aire.ord_ordenes_historico
add actividad_orden	VARCHAR2(250);

alter table aire.ord_ordenes_historico
add fecha_estimada_respuesta DATE;

alter table aire.ord_ordenes_historico
add numero_camp VARCHAR2(50);

alter table aire.ord_ordenes_historico
add comentario_orden_servicio_num1 VARCHAR2(200);

alter table aire.ord_ordenes_historico
add comentario_orden_servicio_num2 VARCHAR2(200);

alter table aire.ord_ordenes_historico
add observacion_rechazo VARCHAR2(2000);

alter table aire.ord_ordenes_historico
add fecha_rechazo DATE;

alter table aire.ord_ordenes_historico
add origen VARCHAR2(250);

alter table aire.ord_ordenes_historico
add fecha_registro TIMESTAMP(6);

alter table aire.ord_ordenes_historico
add fecha_asigna_contratista TIMESTAMP(6);

alter table aire.ord_ordenes_historico
add fecha_asigna_tecnico TIMESTAMP(6); --Agrego 21 columnas

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
add fecha_consulta timestamp default SYSTIMESTAMP; --Agrego 8 columnas

--Modifico la columna id_cliente, para evitar errores ya que el number daba problemas y por eso le especifique la longitud ya q es un numero grande
alter table aire.ord_ordenes_historico
modify id_cliente	NUMBER(38,0);

