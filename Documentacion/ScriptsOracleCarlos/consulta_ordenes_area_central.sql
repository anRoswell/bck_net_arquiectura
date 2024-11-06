set serveroutput on;

Declare 
        --e_json  	clob :=  '{"id_contratista": "1","id_zona": "2"}';
        s_json      clob;
        v_json_objeto    json_object_t; 
        v_id_log		aire.gnl_logs.id_log%type;
        v_respuesta		aire.tip_respuesta :=   aire.tip_respuesta( mensaje     => 'Consulta realizada con exito.', nombre_up   => 'pkg_g_carlos_vargas_test.prc_consultar_ordenes_area_central');
        --v_objeto      	aire.ord_ordenes%rowtype;
begin        
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Entro a crear JSON');
        apex_json.free_output; 
        apex_json.initialize_clob_output( p_preserve => false );
        
        -- validamos el Json de la orden y armamos el rowtype        
        
        apex_json.open_object();        
            apex_json.write('codigo', 0);
            apex_json.write('mensaje', 'Consulta exitosa');
            apex_json.open_object('datos');
            -- ordenes
                apex_json.open_array('ordenes');
                    for c_datos in (
                        select a.ID_TIPO_ORDEN
                        from aire.ord_ordenes a
                        where rownum <=1900
                    ) loop
                        apex_json.open_object();
                        apex_json.write('ID_TIPO_ORDEN', c_datos.ID_TIPO_ORDEN);                        
                    apex_json.close_object();
                end loop;
                apex_json.close_array();               
            apex_json.close_object();            
        apex_json.close_object();
        
        s_json := apex_json.get_clob_output;
        apex_json.free_output;  
        dbms_lob.getlength(s_json);
        --dbms_output.put_line('res' || s_json);
          loop exit when l_offset > dbms_lob.getlength(s_json);
           DBMS_OUTPUT.PUT_LINE (dbms_lob.substr( s_json, 254, l_offset) || '~');
           l_offset := l_offset + 255;
          end loop;
  
        --s_json := v_json;
        v_id_log := aire.pkg_p_generales.fnc_registrar_log (v_respuesta.nombre_up, aire.pkg_g_seguridad.v_tipo_mensaje_seguimiento, 'Carga exitosa');        
        --PL/SQL: numeric or value error%s dbms_output.put_line(s_json);    
end;
/

SET SERVEROUTPUT ON;

DECLARE
  E_JSON CLOB;
  S_JSON CLOB;
BEGIN
  E_JSON := '{"id_contratista": 1,"id_zona": 1}';

  AIRE.PKG_G_CARLOS_VARGAS_TEST.PRC_CONSULTAR_ORDENES_AREA_CENTRAL(
    E_JSON => E_JSON,
    S_JSON => S_JSON
  );
  DBMS_OUTPUT.PUT_LINE('S_JSON = ' || S_JSON);
  --:S_JSON := S_JSON;
--rollback; 
END;
/
