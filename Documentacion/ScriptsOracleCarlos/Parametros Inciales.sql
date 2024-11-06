set serveroutput on;

Declare 
        e_json  	clob :=  '{"nic": "2076341","nis": ""}';
        v_json      clob;
        v_json_cliente    json_object_t; 
        v_id_log		aire.gnl_logs.id_log%type;
        v_respuesta		aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_carlos_vargas_test.prc_consultar_cliente_por_nic_nis');
        v_cliente      	aire.gnl_clientes%rowtype;
begin        
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
        apex_json.free_output; 
        apex_json.initialize_clob_output( p_preserve => false );
        
        -- validamos el Json de la orden y armamos el rowtype
        begin
            -- parseamos el json
            v_json_cliente := json_object_t(e_json);

            -- extraemos los datos de la orden
            v_cliente.nic           := v_json_cliente.get_string('nic');
            v_cliente.nis           := v_json_cliente.get_string('nis');
        exception 
            when others then 
                v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al procesar el json de la orden: '|| sqlerrm);
                apex_json.initialize_clob_output( p_preserve => false );
                apex_json.open_object();
                apex_json.write('codigo', 1);
                apex_json.write('mensaje', 'Error #'||v_id_log||' al registrar orden');
                apex_json.close_object();
                --s_json := apex_json.get_clob_output;
                apex_json.free_output;
                return;
        end; 
        
        apex_json.open_object();        
            apex_json.write('codigo', 0);
            apex_json.write('mensaje', 'Consulta exitosa');
            apex_json.open_object('datos');
            -- Tipos de orden
                apex_json.open_array('gnl_clientes');
                    for c_datos in (
                        select id_cliente
                             , id_territorial
                             , id_zona
                             , id_departamento
                             , id_municipio
                             , direccion
                             , georreferencia
                             , nic
                             , nis
                             , nombre_cliente
                          from aire.gnl_clientes
                         where nic = v_cliente.nic
                            or nis = v_cliente.nis
                    ) loop
                        apex_json.open_object();
                        apex_json.write('id_cliente', c_datos.id_cliente);
                        apex_json.write('id_territorial', c_datos.id_territorial);
                        apex_json.write('id_zona',c_datos.id_zona);
                        apex_json.write('id_departamento',c_datos.id_departamento);
                        apex_json.write('id_municipio',c_datos.id_municipio);
                        apex_json.write('direccion',c_datos.direccion);
                        apex_json.write('georreferencia',c_datos.georreferencia);
                        apex_json.write('nic',c_datos.nic);
                        apex_json.write('nis',c_datos.nis);
                        apex_json.write('nombre_cliente',c_datos.nombre_cliente);
                        apex_json.close_object();
                    end loop;
                apex_json.close_array();     
               
            apex_json.close_object();            
        apex_json.close_object();
        
        v_json := apex_json.get_clob_output;
        apex_json.free_output;         
        --s_json := v_json;
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Carga exitosa');        
        dbms_output.put_line(v_json);
    exception
        when others then
            v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_error, 'Error al crear el json: '|| sqlerrm);
            apex_json.free_output; 
            apex_json.open_object();
            apex_json.write('codigo', 1);
            apex_json.write('mensaje', 'Error N '||v_id_log ||' al crear los parametros iniciales');            
            apex_json.close_object();
            v_json := apex_json.get_clob_output;
            apex_json.free_output;
           -- s_json := v_json;
end;
/

SET SERVEROUTPUT ON;

DECLARE
  E_JSON CLOB;
  S_JSON CLOB;
BEGIN
  E_JSON := '{"nic":"2076341","nis":null}';

  AIRE.PKG_G_CARLOS_VARGAS_TEST.PRC_CONSULTAR_CLIENTE_POR_NIC_NIS(
    E_JSON => E_JSON,
    S_JSON => S_JSON
  );
  
dbms_output.put_line('--------------------');  
DBMS_OUTPUT.PUT_LINE('S_JSON = ' || S_JSON);
 
  --:S_JSON := S_JSON;
--rollback; 
END;

