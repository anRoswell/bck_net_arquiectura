set serveroutput on;
declare
    v_json          clob;  
    v_cursor        sys_refcursor;
    v_id            number;
    v_descripcion   varchar2(1000);
begin
    apex_json.free_output; 
    apex_json.initialize_clob_output( p_preserve => false );
    apex_json.open_object;
    apex_json.open_object('datos');
    apex_json.open_array('columnas');
    
    -- consultamos las columnas
    for c_columnas in (
        select id_columna_filtro
             , descripcion_columna
             , tipo_dato
             , tipo_elemento
             , ind_requerido
             , operadores
             , origen
          from aire.ord_columnas_filtro
         where ind_activo = 'S' 
    ) loop
        apex_json.open_object;
        apex_json.write('id_columna', c_columnas.id_columna_filtro);
        apex_json.write('descripcion',c_columnas.descripcion_columna);
        apex_json.write('tipo_dato',c_columnas.tipo_dato);
        apex_json.write('tipo_elemento',c_columnas.tipo_elemento);
        apex_json.write('ind_requerido',c_columnas.ind_requerido);
        
        -- recorremos los operadores
        apex_json.open_array('operadores');
        for c_operadores in (
            select id_dominio_valor
                 , descripcion
                 , valor
              from aire.gnl_dominios_valor
             where id_dominio_valor in ( 
                        select regexp_substr(c_columnas.operadores, '[^:]+', 1, level)
                          from dual
                        connect by regexp_substr(c_columnas.operadores, '[^:]+', 1, level) is not null
                   )        
        ) loop
            apex_json.open_object;
            apex_json.write('id_operador',c_operadores.id_dominio_valor);
            apex_json.write('descripcion',c_operadores.descripcion);
            apex_json.write('operador',c_operadores.valor);
            apex_json.close_object;
        end loop;
        apex_json.close_array;        
        
        
        if c_columnas.origen is not null then
            begin
                execute immediate 'begin :cursor := ' || c_columnas.origen || '; end;' using out v_cursor;
            exception
                when others then
                    dbms_output.put_line('Error al consultar el origen de la columna: '|| c_columnas.descripcion_columna);
                    return;
            end;
            
            -- Recorrer el cursor y agregar cada fila al array JSON
            apex_json.open_array('lista');
            loop
                fetch v_cursor into v_id, v_descripcion;
                exit when v_cursor%notfound;
        
                -- Agregar cada fila como un objeto JSON al array
                apex_json.open_object;
                apex_json.write('id', v_id);
                apex_json.write('descripcion', v_descripcion);
                apex_json.close_object;
            end loop;            
            apex_json.close_array;

        end if;
        apex_json.close_object;
    end loop;
    apex_json.close_array;
    apex_json.close_object;

    apex_json.write('codigo', 0);
    apex_json.write('mensaje', 'Carga exitosa');
    apex_json.close_object;
    
    v_json := apex_json.get_clob_output;
    apex_json.free_output;
    
    dbms_output.put_line(v_json);
end;
/



    set serveroutput on;
    DECLARE
      S_JSON CLOB;
    BEGIN
    
      AIRE.PKG_G_DANIEL_GONZALEZ_TEST3.PRC_FILTROS_PARAMETROS_INICIALES(
        S_JSON => S_JSON
      );
      DBMS_OUTPUT.PUT_LINE('S_JSON = ' || S_JSON);
    --rollback; 
    END;
