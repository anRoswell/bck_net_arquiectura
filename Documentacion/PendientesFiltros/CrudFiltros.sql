-- creacion--PUT
set serveroutput on;
declare
    e_filtro            clob := '{"id_columna":4, "id_operador":611, "valor":""}';
    v_json_respuesta   clob;
begin
    aire.pkg_g_ordenes.prc_registrar_filtro(e_filtro, v_json_respuesta);
    dbms_output.put_line(v_json_respuesta);
end;
/

-- actualizacion--POST
set serveroutput on;
declare
    e_filtro    clob := '{"id_filtro":31, "id_columna":3, "id_operador":605, "valor":"20/01/2024:22/02/2024"}';
    v_json_respuesta   clob;
begin
    aire.pkg_g_ordenes.prc_actualizar_filtro(e_filtro, v_json_respuesta);
    dbms_output.put_line(v_json_respuesta);
end;
/

-- eliminacion--POST
set serveroutput on;
declare
    e_filtro            clob := '{"id_filtro":30}';
    v_json_respuesta   clob;
begin
    aire.pkg_g_ordenes.prc_eliminar_filtro(e_filtro, v_json_respuesta);
    dbms_output.put_line(v_json_respuesta);
end;
/

-- consulta--GET
set serveroutput on;
declare
    v_json_respuesta   clob;
begin
    aire.pkg_g_ordenes.prc_consultar_filtros(v_json_respuesta);
    dbms_output.put_line(v_json_respuesta);
end;
/