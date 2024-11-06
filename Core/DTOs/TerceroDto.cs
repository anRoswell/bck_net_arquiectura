using System;

namespace Core.Entities;

public partial class TerceroDto
{
    public int Id { get; set; }
    public int PrvIdProveedores { get; set; }
    public string PrvNit { get; set; }
    public string PrvDigitoVerificacion { get; set; }
    public DateTime PrvFechaEnvio { get; set; }
    public string PrvNombreProveedor { get; set; }
    public string PrvDireccion { get; set; }
    public string PrvCodCiudad { get; set; }
    public string PrvTelefono { get; set; }
    public string PrvContacto { get; set; }
    public string PrvMail { get; set; }
    public string PrvMailAlterno { get; set; }
    public string PrvRteLegalNombre { get; set; }
    public string PrvRteLegalApellido { get; set; }
    public string PrvRteLegalIdentificacion { get; set; }
    public string PrvRteLegalCodCiudad { get; set; }
    public string PrvRteLegalTelefonoMovil { get; set; }
    public string PrvRteLegalEmail { get; set; }
    public string PrvRteLegalSuplenteNombre { get; set; }
    public string PrvRteLegalSuplenteApellido { get; set; }
    public string PrvRteLegalSuplenteIdentificacion { get; set; }
    public string PrvRteLegalSuplenteDigVerificacion { get; set; }
    public string PrvRteLegalSuplenteCodCiudad { get; set; }
    public string PrvRteLegalSuplenteTelefonoMovil { get; set; }
    public string PrvRteLegalSuplenteEmail { get; set; }
    public int PrvCodBanco { get; set; }
    public string PrvDtllesBanNroCuenta { get; set; }
    public int PrvCodTipoCuenta { get; set; }
    public int PrvProveeedor { get; set; }
    public string PrvCodTipoProveeedor { get; set; }
    public string PrvTipoProveedorCual { get; set; }
    public int PrvCpaCodCondicionesPago { get; set; }
    public string PrvCpaCual { get; set; }
    public string PrvCpaContadoCual { get; set; }
    public bool PrvExperienciaSector { get; set; }
    public bool PrvPoliticaTratamientoDatosPersonales { get; set; }
    public bool PrvDeclaramientoInhabilidadesInteres { get; set; }
    public string PrvUrlPdf { get; set; }
    public string PrvValidationNumber { get; set; }
    public string PrvRteLegalDigVerificacion { get; set; }
    public string PrvRevFiscalNombre { get; set; }
    public string PrvRevFiscalApellido { get; set; }
    public string PrvRevFiscalIdentificacion { get; set; }
    public string PrvRevFiscalDigVerificacion { get; set; }
    public string PrvRevFiscalCodCiudad { get; set; }
    public string PrvRevFiscalTelefonoMovil { get; set; }
    public string PrvRevFiscalEmail { get; set; }
    public bool PrvListaRestrictiva { get; set; }
    public int PrvCodEstado { get; set; }
    public string PrvJustificacionInspektor { get; set; }
    public int? PrvUsuarioApRzInspektor { get; set; }
    public string PrvJustificacionCorreccion { get; set; }
    public string CodUser { get; set; }
    public DateTime FechaRegistro { get; set; }
    public string CodUserUpdate { get; set; }
    public DateTime FechaRegistroUpdate { get; set; }
    public string Info { get; set; }
    public string InfoUpdate { get; set; }
    public bool PrvEsTercero { get; set; }
}

public partial class TerceroDto
{
    public int PrvCodTraza { get; set; }
    public string PrvUrlPdfRel { get; set; }
    public string RptNombreCiudad { get; set; }
    public string NombreBanco { get; set; }
    public string CodigoDepartamento { get; set; }
    public string NombreDepartamento { get; set; }
    public string NombreCiudad { get; set; }
    public string NombreTipoCuenta { get; set; }
    public string NombreTipoProveedor { get; set; }
    public string NombreCondiconesPago { get; set; }
    public int UsrEstado { get; set; }
    public string FechaTexto { get; set; }
    public bool ValGestor { get; set; }
    public int CodEstadoInspector { get; set; }
    public bool CodEstadoDocumentos { get; set; }
    public string RevNombreCiudad { get; set; }
    public string IdentPrvCompleta { get; set; }
    public string IdentRteCompleta { get; set; }
    public string IdentRevCompleta { get; set; }
    public string PrvNombreUsuarioApRzInspektor { get; set; }
}