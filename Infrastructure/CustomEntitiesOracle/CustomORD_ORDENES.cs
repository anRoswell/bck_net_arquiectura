using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.CustomEntitiesOracle
{
    ///// <summary>
    ///// Atributo Personalizado aire.tip_respuesta
    ///// </summary>
    //public class CustomORD_ORDENES : IOracleCustomType
    //{
    //    [OracleObjectMappingAttribute("ID_ORDEN")]
    //    public Int32 ID_ORDEN { get; set; }
    //    [OracleObjectMappingAttribute("DESCRIPCION")]
    //    public string DESCRIPCION { get; set; }
    //    [OracleObjectMappingAttribute("COMENTARIOS")]
    //    public string COMENTARIOS { get; set; }

    //    public virtual void FromCustomObject(OracleConnection conn, object objUdt)
    //    {
    //        OracleUdt.SetValue(conn, objUdt, "ID_ORDEN", this.ID_ORDEN);
    //        OracleUdt.SetValue(conn, objUdt, "DESCRIPCION", this.DESCRIPCION);
    //        OracleUdt.SetValue(conn, objUdt, "COMENTARIOS", this.COMENTARIOS);
    //    }
    //    public virtual void ToCustomObject(OracleConnection conn, object objUdt)
    //    {
    //        this.ID_ORDEN = 57;// ((Int32)(OracleUdt.GetValue(conn, objUdt, "ID_ORDEN")));
    //        this.DESCRIPCION = "prueba prueba prueba";//(string)(OracleUdt.GetValue(conn, objUdt, "DESCRIPCION")));
    //        this.COMENTARIOS = "prueba prueba prueba";//((string)(OracleUdt.GetValue(conn, objUdt, "COMENTARIOS")));
    //    }
    //}

    //// Definir la clase MiTipoCustom para mapear al tipo personalizado
    //[OracleCustomTypeMappingAttribute("AIRE.ORD_ORDENES")]
    //public class ORD_ORDENES : IOracleCustomTypeFactory
    //{
    //    public virtual IOracleCustomType CreateObject()
    //    {
    //        CustomORD_ORDENES obj = new CustomORD_ORDENES();
    //        return obj;
    //    }
    //}
}
