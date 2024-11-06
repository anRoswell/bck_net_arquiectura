using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.CustomEntitiesOracle
{
    /// <summary>
    /// Atributo Personalizado aire.tip_respuesta
    /// </summary>
    public class CustomTIP_RESPUESTA : IOracleCustomType
    {
        [OracleObjectMappingAttribute("CODIGO")]
        public Int32 CODIGO { get; set; }
        [OracleObjectMappingAttribute("MENSAJE")]
        public string MENSAJE { get; set; }
        [OracleObjectMappingAttribute("NOMBRE_UP")]
        public string NOMBRE_UP { get; set; }
        [OracleObjectMappingAttribute("ESTADO")]
        public Int32 ESTADO { get; set; }

        public virtual void FromCustomObject(OracleConnection conn, object objUdt)
        {
            OracleUdt.SetValue(conn, objUdt, "MENSAJE", this.MENSAJE);
            OracleUdt.SetValue(conn, objUdt, "CODIGO", this.CODIGO);
            OracleUdt.SetValue(conn, objUdt, "NOMBRE_UP", this.NOMBRE_UP);
            OracleUdt.SetValue(conn, objUdt, "ESTADO", this.ESTADO);
        }
        public virtual void ToCustomObject(OracleConnection conn, object objUdt)
        {
            this.MENSAJE = ((string)(OracleUdt.GetValue(conn, objUdt, "MENSAJE")));
            this.CODIGO = ((Int32)(OracleUdt.GetValue(conn, objUdt, "CODIGO")));
            this.NOMBRE_UP = ((string)(OracleUdt.GetValue(conn, objUdt, "NOMBRE_UP")));
            this.ESTADO = ((Int32)(OracleUdt.GetValue(conn, objUdt, "ESTADO")));
        }
    }

    // Definir la clase MiTipoCustom para mapear al tipo personalizado
    [OracleCustomTypeMappingAttribute("AIRE.TIP_RESPUESTA")]
    public class TIP_RESPUESTA : IOracleCustomTypeFactory
    {
        public virtual IOracleCustomType CreateObject()
        {
            CustomTIP_RESPUESTA obj = new CustomTIP_RESPUESTA();
            return obj;
        }
    }
}
