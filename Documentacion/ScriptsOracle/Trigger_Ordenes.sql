SET SERVEROUTPUT ON;

clear screen;
Declare 
    v_orden_traza 				aire.ord_ordenes_trazabilidad%rowtype;
    v_descripcion_estado_orden	aire.ord_estados_orden.descripcion%type;
    v_respuesta 				aire.tip_respuesta;
    v_orden                     aire.ord_ordenes%rowtype;
    v_updating                  number;
begin
    v_updating := 0; --0 nuevo, 1 modificacion
    
    select 
        *
        into v_orden
    from aire.ord_ordenes where id_orden = 15044;
    
    
    
    select descripcion 
      into v_descripcion_estado_orden
      from aire.ord_estados_orden
    where id_estado_orden = v_orden.id_estado_orden; 
    
    if v_orden.id_contratista_persona is not null and v_orden.id_contratista is not null then
        begin
            select id_contratista_brigada
              into v_orden_traza.id_contratista_brigada
              from aire.ctn_contratistas_brigada 
             where id_contratista         = v_orden.id_contratista
               and id_contratista_persona = v_orden.id_contratista_persona 
               and ind_activo             = 'S';
        exception
            when others then
                v_orden_traza.id_contratista_brigada := null;
        end;
    end if;
     
    DBMS_OUTPUT.PUT_LINE('id_contratista_brigada = ' || v_orden_traza.id_contratista_brigada); 
    
    v_orden_traza.id_orden        			:= v_orden.id_orden;
    v_orden_traza.id_estado_orden 			:= v_orden.id_estado_orden;
    v_orden_traza.id_contratista  			:= v_orden.id_contratista;
    v_orden_traza.id_contratista_persona  	:= v_orden.id_contratista_persona;
    v_orden_traza.fecha  					:= v_orden.fecha_rechazo;
    
    if v_updating = 0  then
        v_orden_traza.observacion := 'Orden creada';
    end if;
    DBMS_OUTPUT.PUT_LINE('v_orden_traza.id_orden = ' || v_orden_traza.id_orden); 
    
    if v_updating = 1 then
        if v_orden.observacion_rechazo is not null then
            v_orden_traza.observacion := 'Orden rechazada por el tecnico y asignada a contratista observacion rechazo: ' || v_orden.observacion_rechazo;
        else
            v_orden_traza.observacion := 'Orden '|| lower(v_descripcion_estado_orden);
        end if;
    end if;
    
    aire.pkg_p_ordenes.prc_registrar_orden_trazabilidad(v_orden_traza, v_respuesta);
    
    DBMS_OUTPUT.PUT_LINE('v_respuesta = ' || v_respuesta.mensaje); 
    /*
    
    if updating then
        if :new.observacion_rechazo is not null then
            v_orden_traza.observacion := 'Orden rechazada por el tecnico y asignada a contratista observacion rechazo: '||:new.observacion_rechazo;
        else
            v_orden_traza.observacion := 'Orden '|| lower(v_descripcion_estado_orden);
        end if;
    end if;
    
    aire.pkg_p_ordenes.prc_registrar_orden_trazabilidad(v_orden_traza, v_respuesta);
    :new.observacion_rechazo := null;
    :new.fecha_rechazo       := null; 
    */
end;
 /