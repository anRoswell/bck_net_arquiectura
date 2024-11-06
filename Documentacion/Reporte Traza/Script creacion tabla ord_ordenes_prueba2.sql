SET SERVEROUTPUT ON;

--Creacion de la tabla ord_ordenes_prueba2 (Tabla para pruebas)

--Creacion de la secuencia
create sequence aire.sec_ord_ordenes_prueba2;
--Creacion de la tabla Actividades
create table aire.ord_ordenes_prueba2 (
    id_orden	                    number          default aire.sec_ord_ordenes_prueba2.nextval
                                                    constraint ord_ordenes_prueba2_id_orden_nn not null,
    id_tipo_orden	                number(3,0)     constraint ord_ordenes_prueba2_id_tipo_orden_nn not null,
    numero_orden	                varchar2(100)   constraint ord_ordenes_prueba2_numero_orden_nn not null,
    id_estado_orden	                number(3,0)     constraint ord_ordenes_prueba2_id_estado_orden_nn not null,
    id_contratista	                number(3,0),
    id_cliente	                    number(38,0),
    id_territorial	                number(3,0),
    id_zona	                        number(3,0),
    direcion	                    varchar2(200),
    fecha_creacion	                timestamp(6)    default systimestamp,
    fecha_cierre	                timestamp(6),
    id_usuario_cierre	            number,
    descripcion	                    varchar2(2000),
    comentarios	                    varchar2(2000),
    acta	                        varchar2(250),
    id_actividad	                number(3,0),
    id_contratista_persona	        number,
    id_tipo_trabajo	                number,
    id_tipo_suspencion	            number,
    actividad_orden	                varchar2(250),
    fecha_estimada_respuesta	    date,
    numero_camp	                    varchar2(50),
    comentario_orden_servicio_num1	varchar2(200),
    comentario_orden_servicio_num2	varchar2(200),
    observacion_rechazo	            varchar2(2000),
    fecha_rechazo	                date,
    origen	                        varchar2(250),
    fecha_registro	                timestamp(6)    default systimestamp,
    id_barrio	                    number,
    fecha_asigna_contratista	    timestamp(6),
    fecha_asigna_tecnico	        timestamp(6),
    constraint ord_ordenes_prueba2_id_orden_pk primary key (id_orden),
    constraint ord_ordenes_prueba2_numero_orden_un unique (numero_orden)
);