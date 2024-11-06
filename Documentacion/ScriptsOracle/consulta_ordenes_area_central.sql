SET SERVEROUTPUT ON;

DECLARE
  E_JSON CLOB;
  S_JSON CLOB;
BEGIN
  E_JSON := '{"id_contratista": 1,"id_zona": 64}';

  AIRE.PKG_G_CARLOS_VARGAS_TEST.PRC_CONSULTAR_ORDENES_AREA_CENTRAL(
    E_JSON => E_JSON,
    S_JSON => S_JSON
  );
  DBMS_OUTPUT.PUT_LINE('S_JSON = ' || S_JSON);
  --:S_JSON := S_JSON;
--rollback; 
END;
/


select * from aire.gnl_clientes
select * from aire.scr_tipos_suspencion

select * from aire.ord_ordenes where id_orden = 14616


