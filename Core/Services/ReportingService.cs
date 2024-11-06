namespace Core.Services
{
    using Core.CustomEntities;
    using Core.CustomEntities.ReportExcel;
    using Core.Entities;
    using Core.Exceptions;
    using Core.Interfaces;
    using Core.Options;
    using Core.QueryFilters;
    using Core.Tools;
    using Microsoft.Extensions.Options;
    using OfficeOpenXml.Style;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Drawing.Printing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class ReportingService : IReportingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmpresasService _empresasService;
        private readonly IFilesProcess _filesProcess;
        private readonly PathOptions _pathOptions;
        private List<string> ListadoArchivosTemporales = new List<string>();

        public ReportingService(
            IUnitOfWork unitOfWork,
            IEmpresasService empresasService,
            IFilesProcess filesProcess,
            IOptions<PathOptions> pathOptions
        )
        {
            _unitOfWork = unitOfWork;
            _empresasService = empresasService;
            _filesProcess = filesProcess;
            _pathOptions = pathOptions.Value;
        }

        #region NoSeUtiliza
        public async Task<MemoryStream> CertificadoExperiencia(QuerySearchCertificados parameters)
        {
            try
            {
                List<CertificadoExperiencia> listEntity = await _unitOfWork.ApoteosysRepository.CertificadosExperienciaApoteosys(parameters);

                string html;

                if (listEntity.Count > 0)
                {
                    List<PathsPortal> imgPaths = await _unitOfWork.ParametrosInicialesRepository.GetPathsImagenesCertificados();

                    string logoPath = imgPaths.Find(x => x.Id == 9).Path;
                    string firmaPath = imgPaths.Find(x => x.Id == 10).Path;
                    string superServicioPath = imgPaths.Find(x => x.Id == 11).Path;
                    string fecha = DateTime.Now.ToString("dd \\de MMMM \\de yyyy", new CultureInfo("es-CO"));

                    html =
                        @"<html>
                        <head>
                            <style>
                                .colorBlue {
                                    background-color: #273580;
                                    color: white;
                                }

                                .colorGreen {
                                    background-color: #46a13a;
                                    color: white;
                                }
        
                                .textoBlue{
                                    color: #103588;
                                }

                                .table {
                                    width: 100%;
                                }

                                .td {
                                    font-family: Arial, sans-serif;
                                    font-size: 12px;
                                    padding: 2px 2px;
                                    word-break: normal;
                                }

                                .negrita{
                                     font-weight: bold;
                                }

                                .textoCenter {
                                    text-align: center;
                                }

                                .textoDerecha {
                                    text-align: right;
                                }

                                .textoIzq {
                                    text-align: left;
                                }

                                .textoJustificado {
                                    text-align: justify;
                                }
                            </style>
                        </head>

                        <body>
                            <table class=""table"" style="" border-collapse:collapse; border-spacing:0; "">
                                <tr style="" padding: 0px; margin: 0px; "">
                                    <td class=""textoCenter"" style=""padding:10px;width: 100%;"">
                                        <img src=""" + logoPath + @""" width=""70px"">
                                    </td>
                                </tr>
                            </table>

                            <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-top:40px"">
                                <tr style=""padding: 0px; margin: 0px;"">
                                    <td class=""negrita"" style=""width: 100%; "">
                                        Bogotá, " + fecha + @"
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoCenter"" style=""padding:40px 0 0 0;width: 100%; "">
                                    <p>LA COORDINACIÓN NACIONAL DE COMPRAS DE LA EMPRESA URBASER COLOMBIA S.A E.S.P.</p>
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoCenter negrita"" style=""padding:20px 0 0 0;width: 100%; "">
                                    CERTIFICA
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoJustificado"" style=""padding:40px 0 0 0;width: 100%; "">
                                    Que la sociedad de razón social <b>" + listEntity[0].Nombre_Tercero + @"</b> e identificada con <b>NIT " + listEntity[0].Nit_Tercero + @"</b> ha tenido relaciones de carácter comercial con esta empresa. En virtud de la suscripción de ordenes de servicio desde el año " + listEntity[0].Anio + @", cumpliendo satisfactoriamente con sus compromisos, calidad y servicios prestados a nuestra empresa, desde el inicio de actividades.
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class="""" style=""padding:40px 0 0 0;width: 100%; "">
                                    La presente certificación se expide a solicitud de parte interesada.
                                </td>
                            </tr>
                        </table>

                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-top:40px;"">
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td>
                                    <img src=""" + firmaPath + @""" width=""200px"">
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td>
                                    ________________________________________
                                </td>
                            </tr>
                            <tr style="" padding: 0px; margin: 0px; "">
                                <td class=""textoJustificado negrita"">
                                    RICARDO GOMEZ <br> COORDINADOR NACIONAL DE COMPRAS <br> Ricardo.gomez@urbaser.co <br> Tel: 580 2145 Ext. 3031 <br> Cel: 3208595504 <br> www.urbaser.co
                                </td>
                            </tr>
                        </table>

                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-top:40px;"">
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoCenter"" style=""width: 20%;"">
                                    
                                </td>
                                <td class=""textoCenter textoBlue"" style=""width: 60%;font-size:10px;"">
                                    Calle 100 No. 19ª-10, Edificio Torre Azul, Pisos 2 y 9 - Bogotá (Colombia)<br>
                                    PBX: 779 93 77
                                </td>
                                <td class=""textoCenter"" style=""width: 20%;"">
                                    <img src=""" + superServicioPath + @""" width=""70px"">
                                </td>
                            </tr>
                        </table>
                    </body>

                    </html>";
                }
                else
                {
                    html =
                        @"<html>
                        <head> 
                            <style>
                                .contenedorOne {
                                    text-align: center;
                                    width: 60%;
                                    padding: 20px;
                                    margin: 40px;
                                    border-radius: 20px;
                                    border-width: 1px;
                                    border-style: solid;
                                    border-color: red;
                                    
                                  }
                            </style>
                        </head>
                         <body>
                            <div class=""contenedorOne"" >
                               <div>
                                    <h1>No se encuentraron datos para la consulta realizada</h1>
                                </div
                             </div>
                         </body>
                        <html>";
                }

                // Creamos el stream del pdf
                MemoryStream ms = new MemoryStream(Funciones.PdfSharpConvertWithoutCreateFile(html));
                return ms;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<MemoryStream> CertificadoEspecialPdf(int id)
        {
            try
            {
                List<CertificadosEspeciale> listEntity = await _unitOfWork.CertificadosEspecialesRepository.GetCertificadoPorID(id);

                string html;

                if (listEntity.Count > 0)
                {
                    List<PathsPortal> imgPaths = await _unitOfWork.ParametrosInicialesRepository.GetPathsImagenesCertificados();

                    string logoPath = imgPaths.Find(x => x.Id == 9).Path;
                    string firmaPath = imgPaths.Find(x => x.Id == 10).Path;
                    string superServicioPath = imgPaths.Find(x => x.Id == 11).Path;
                    string fecha = DateTime.Now.ToString("dd \\de MMMM \\de yyyy", new CultureInfo("es-CO"));

                    // Armamos el HTML
                    html =
                    @"<html>
                    <head>
                        <style>
                            .colorBlue {
                                background-color: #273580;
                                color: white;
                            }

                            .colorGreen {
                                background-color: #46a13a;
                                color: white;
                            }
        
                            .textoBlue{
                                color: #103588;
                            }

                            .table {
                                width: 100%;
                            }

                            .td {
                                font-family: Arial, sans-serif;
                                font-size: 12px;
                                padding: 2px 2px;
                                word-break: normal;
                            }

                            .negrita{
                                 font-weight: bold;
                            }

                            .textoCenter {
                                text-align: center;
                            }

                            .textoDerecha {
                                text-align: right;
                            }

                            .textoIzq {
                                text-align: left;
                            }

                            .textoJustificado {
                                text-align: justify;
                            }
                        </style>
                    </head>

                    <body>
                        <table class=""table"" style="" border-collapse:collapse; border-spacing:0; "">
                            <tr style="" padding: 0px; margin: 0px; "">
                                <td class=""textoCenter"" style=""padding:10px;width: 100%;"">
                                    <img src=""" + logoPath + @""" width=""70px"">
                                </td>
                            </tr>
                        </table>

                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-top:40px"">
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""negrita"" style=""width: 100%; "">
                                    Bogotá, " + fecha + @"
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoCenter"" style=""padding:40px 0 0 0;width: 100%; "">
                                    <p>LA COORDINACIÓN NACIONAL DE COMPRAS DE LA EMPRESA URBASER COLOMBIA S.A E.S.P.</p>
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoCenter negrita"" style=""padding:20px 0 0 0;width: 100%; "">
                                    CERTIFICA
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoJustificado"" style=""padding:40px 0 0 0;width: 100%; "">
                                    " + listEntity[0].CerHtmlPdf + @"
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class="""" style=""padding:40px 0 0 0;width: 100%; "">
                                    La presente certificación se expide a solicitud de parte interesada.
                                </td>
                            </tr>
                        </table>

                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-top:40px;"">
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td>
                                    <img src=""" + firmaPath + @""" width=""200px"">
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td>
                                    ________________________________________
                                </td>
                            </tr>
                            <tr style="" padding: 0px; margin: 0px; "">
                                <td class=""textoJustificado negrita"">
                                    RICARDO GOMEZ <br> COORDINADOR NACIONAL DE COMPRAS <br> Ricardo.gomez@urbaser.co <br> Tel: 580 2145 Ext. 3031 <br> Cel: 3208595504 <br> www.urbaser.co
                                </td>
                            </tr>
                        </table>

                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-top:40px;"">
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoCenter"" style=""width: 20%;"">
                                    
                                </td>
                                <td class=""textoCenter textoBlue"" style=""width: 60%;font-size:10px;"">
                                    Calle 100 No. 19ª-10, Edificio Torre Azul, Pisos 2 y 9 - Bogotá (Colombia)<br>
                                    PBX: 779 93 77
                                </td>
                                <td class=""textoCenter"" style=""width: 20%;"">
                                    <img src=""" + superServicioPath + @""" width=""70px"">
                                </td>
                            </tr>
                        </table>
                    </body>

                    </html>";
                }
                else
                {
                    // Armamos el HTML
                    html = @"<html>No se encontró data para este certificado, consulte con el administrador</html>";
                }

                // Creamos el stream del pdf
                MemoryStream ms = new MemoryStream(Funciones.PdfSharpConvertWithoutCreateFile(html));
                return ms;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<MemoryStream> FacturaPorPagarPdf(QuerySearchEstadoCuentasXPagarDetalle parameters)
        {
            try
            {
                QuerySearchEstadoCuentasXPagarDetalle query = new QuerySearchEstadoCuentasXPagarDetalle()
                {
                    Anio = parameters.Anio,
                    Codigo_Td = parameters.Codigo_Td,
                    Empresa = parameters.Empresa,
                    NitProveedor = parameters.NitProveedor,
                    Numero_B = parameters.Numero_B,
                    Tipo_Documento = parameters.Tipo_Documento
                };

                QuerySearchEstadoCuentasXPagarDetalle queryDte = new QuerySearchEstadoCuentasXPagarDetalle()
                {
                    Empresa = parameters.Empresa,
                    NitProveedor = parameters.NitProveedor,
                    Anio = parameters.Anio,
                    Numero_B = parameters.Numero_B,
                    Codigo_Td = parameters.Codigo_Td,
                    Periodo = parameters.Periodo,
                    Numero_Documento = parameters.Numero_Documento,
                    Tipo_Documento = parameters.Tipo_Documento
                };

                // Consultamos datos de la factura
                List<EstadoCuentasXPorPagar> facturas = await _unitOfWork.SispoRepository.EstadoCuentasXPorPagarApoteosysDte(query);
                List<EstadoCuentasXPagarDetalle> facturasDetalle = await _unitOfWork.ApoteosysRepository.EstadoCuentasXPorPagarDetalleApoteosys_Reporte(queryDte);
                string html = string.Empty;

                // Consultamos data de la empresa
                List<Empresa> emp = _empresasService.EmpresasActivas;
                Empresa empresaGestion = new Empresa();

                if (parameters.Empresa == "NSGS")
                {
                    string tipoDocAlt = facturas[0].Tipo_Documento[2..];
                    empresaGestion = emp.Find(x => x.EmpAbreviatura == parameters.Empresa && x.Emp_TipoDocAlternoApot == tipoDocAlt);
                }
                else
                {
                    empresaGestion = emp.Find(x => x.EmpAbreviatura == parameters.Empresa);
                }

                if (facturas.Count > 0 && facturasDetalle.Count > 0)
                {

                    // Armamos el HTML
                    html =
                        @"<html>
                        <head>
                            <style>
                                .colorBlue {
                                    background-color: #034F8A;
                                    color: white;
                                }

                                .colorGreen {
                                    background-color: #46a13a;
                                    color: white;
                                }

                                .table {
                                    width: 100%;
                                }

                                .td {
                                    font-family: Arial, sans-serif;
                                    font-size: 12px;
                                    padding: 2px 2px;
                                    word-break: normal;
                                }

                                .textoCenter {
                                    text-align: center;
                                }

                                .textoDerecha {
                                    text-align: right;
                                }

                                .textoIzq {
                                    text-align: left;
                                }

                                .negrita {
                                    font-weight: bold;
                                }

                                .twoContenedor {
                                    padding-left: 10px;
                                    padding-top: 0px;
                                    padding-right: 5px;
                                    padding-bottom: 0px;
                                }
                            </style>
                        </head>

                        <body>
                            <table class=""table"" style=""border-collapse:collapse; border-spacing:0;"">
                                <tr style=""padding: 0px; margin: 0px;"">
                                    <td class=""negrita"" style=""padding-left: 10px; width: 57%;"">
                                        " + empresaGestion.Emp_NombreEmpresaGnr + @"
                                </td>
                                <td class=""textoDerecha"" style=""width: 43%;"">
                                    <img src=""" + empresaGestion.CodArchivo + @""" width=""200px"" height=""50px"" >
                                </td>
                            </tr>
                        </table>
                          
                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:10px;"">
                            <tr>
                                <td class=""twoContenedor"" style=""padding-bottom:7px; padding-top-10px;width: 40%;"">                                       
                                    " + empresaGestion.EmpDireccion + @"
                                </td>
                                <td class=""twoContenedor textoIzq"" style=""width: 60%;"">
                                    
                                </td>
                            </tr>
                            <tr>
                                <td class=""twoContenedor"" style=""padding-bottom:7px; padding-top-10px;width: 40%;"">
                                    <b>NIT:</b> " + empresaGestion.EmpNit + @"
                                </td>
                            </tr>
                        </table>

                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:10px;border:1px solid black;"">    
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoIzq negrita"" style=""width: 30%; padding-left: 15px;"">                                       
                                    Contabilidad:
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 30%; "">
                                    " + empresaGestion.EmpNombreEmpresa + @"
                                </td>
                                <td class=""textoDerecha negrita"" style=""width: 20%; "">
                                    Fecha de Emisión
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 20%; "">
                                    " + facturas[0].Fecha_Emision + @"
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoIzq negrita"" style=""width: 30%; padding-left: 15px;"">                                       
                                    Concepto comprobante:
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 30%; ""  colspan=""3"">
                                    FACTURA DE COMPRA " + empresaGestion.Emp_Ciudad + @"
                                </td>
                                <td class=""textoDerecha negrita"" style=""width: 20%; "">
                                    
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 20%; "">
                                    
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoIzq negrita"" style=""width: 30%; padding-left: 15px;"">
                                    Factura N°:
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 30%; "" colspan=""3"">
                                    " + parameters.Numero_Documento + @"
                                </td>
                                <td class=""textoDerecha negrita"" style=""width: 20%; "">

                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 20%; "">

                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoIzq negrita"" style=""width: 30%; padding-left: 15px;"">
                                    Proveedor:
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 30%; "" colspan=""3"">
                                    " + string.Concat(facturas[0].Nombre_Tercero, " - NIT. ", parameters.NitProveedor) + @"
                                </td>
                                <td class=""textoDerecha negrita"" style=""width: 20%; "">
                                    
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 20%; "">
                                    
                                </td>
                            </tr>
                        </table>

                        <table class=""table colorBlue"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:0px;"">
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class="" textoCenter td negrita"" style=""padding: 10px 0; width: 60%"">
                                    DESCRIPCIÓN
                                </td>
                                <td class="" textoCenter td negrita"" style=""padding: 10px 0; width: 40%"">
                                    RETENCIÓN
                                </td>
                            </tr>
                        </table>

                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;"">
                            " + MaperarFacturaDetallePdf(facturasDetalle) + @"
                        </table>
                    </body>
                </html>";
                }
                else
                {
                    html =
                        @"<html>
                        <head>
                            <style>
                                .colorBlue {
                                    background-color: #034F8A;
                                    color: white;
                                }

                                .colorGreen {
                                    background-color: #46a13a;
                                    color: white;
                                }

                                .table {
                                    width: 100%;
                                }

                                .td {
                                    font-family: Arial, sans-serif;
                                    font-size: 12px;
                                    padding: 2px 2px;
                                    word-break: normal;
                                }

                                .textoCenter {
                                    text-align: center;
                                }

                                .textoDerecha {
                                    text-align: right;
                                }

                                .textoIzq {
                                    text-align: left;
                                }

                                .negrita {
                                    font-weight: bold;
                                }

                                .twoContenedor {
                                    padding-left: 10px;
                                    padding-top: 0px;
                                    padding-right: 5px;
                                    padding-bottom: 0px;
                                }
        
                                .SinDescuentos {
                                    border:1px solid black;
                                }
                            </style>
                        </head>

                        <body>
                            <table class=""table"" style=""border-collapse:collapse; border-spacing:0;"">
                                <tr style=""padding: 0px; margin: 0px;"">
                                    <td class=""negrita"" style=""padding-left: 10px; width: 57%;"">
                                        
                                    </td>
                                    <td class=""textoDerecha"" style=""width: 43%;"">
                                        <img src=""" + empresaGestion.CodArchivo + @""" width=""200px"" height=""50px"" >
                                    </td>
                                </tr>
                            </table>

                            <table class=""table SinDescuentos"" style=""border-collapse:collapse; border-spacing:0;margin-top:20px;"">
                                <tr style=""padding: 0px; margin: 0px;"">
                                    <td class="" textoCenter td negrita"" style=""padding: 10px 0; width: 60%;"">
                                        LA FACTURA O DOCUMENTO '" + facturas[0].Numero_Documento + @"' CONSULTADO NO SE LE PRACTICÓ NINGÚN TIPO DE RETENCIÓN
                                    </td>
                                </tr>
                            </table>
                        </body>

                        </html>";
                }

                // Creamos el stream del pdf
                MemoryStream ms = new MemoryStream(Funciones.PdfSharpConvertWithoutCreateFile(html));
                return ms;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<MemoryStream> FacturaPorPagarXlsx(QuerySearchEstadoCuentasXPagarDetalle parameters)
        {
            try
            {
                QuerySearchEstadoCuentasXPagarDetalle query = new QuerySearchEstadoCuentasXPagarDetalle()
                {
                    Anio = parameters.Anio,
                    Codigo_Td = parameters.Codigo_Td,
                    Empresa = parameters.Empresa,
                    NitProveedor = parameters.NitProveedor,
                    Numero_B = parameters.Numero_B,
                    Tipo_Documento = parameters.Tipo_Documento
                };

                QuerySearchEstadoCuentasXPagarDetalle queryDte = new QuerySearchEstadoCuentasXPagarDetalle()
                {
                    Empresa = parameters.Empresa,
                    NitProveedor = parameters.NitProveedor,
                    Anio = parameters.Anio,
                    Numero_B = parameters.Numero_B,
                    Codigo_Td = parameters.Codigo_Td,
                    Periodo = parameters.Periodo,
                    Numero_Documento = parameters.Numero_Documento,
                    Tipo_Documento = parameters.Tipo_Documento
                };

                // Consultamos datos de la factura
                List<EstadoCuentasXPorPagar> facturas = await _unitOfWork.SispoRepository.EstadoCuentasXPorPagarApoteosysDte(query);

                ReportExcel report = new ReportExcel();
                if (facturas is null || facturas.Count == 0)
                {
                    var hoja1 = report.AgregarHoja("Hoja 1");

                    hoja1.Cells["A1"].Value = "HUBO INCONVENIENTES PARA OBTENER LA FACTURA, INTENTE NUEVAMENTE Ó COMUNIQUESE CON EL ADMINISTRADOR";
                    hoja1.Cells["A1"].Style.Font.Bold = true;

                    // Ajustamos la columna al tamñano máximo
                    hoja1.Column(1).AutoFit();
                }
                else
                {
                    List<EstadoCuentasXPagarDetalle> facturasDetalle = await _unitOfWork.ApoteosysRepository.EstadoCuentasXPorPagarDetalleApoteosys_Reporte(queryDte);
                    List<EstadoCuentasXPagarDteExcel> listDteFact = (from a in facturasDetalle
                                                                     select new EstadoCuentasXPagarDteExcel
                                                                     {
                                                                         BASE_RETENCIÓN = a.Base_Retencion,
                                                                         VALOR_RETENIDO = a.Credito,
                                                                         DESCRIPCIÓN = a.Nombre_Cuenta_Contable,
                                                                         FACTURA = parameters.Numero_Documento
                                                                     }).ToList();

                    if (listDteFact.Count > 0)
                    {
                        var hoja1 = report.AgregarHoja("Detalle factura");

                        int filaInicioTabla = 5;

                        // Cargamos la data en el excel
                        report.CargarData(hoja1, listDteFact, $"A{filaInicioTabla}");

                        int lastRow = hoja1.Dimension.End.Row;
                        int lastColumn = hoja1.Dimension.End.Column;

                        // Colocamos la información de la empresa de la factura
                        hoja1.Cells["A1"].Value = facturas[0].NombreEmpresa;
                        hoja1.Cells["A1"].Style.Font.Bold = true;
                        hoja1.Cells[1, 1, filaInicioTabla - 1, 2].Merge = true;
                        hoja1.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja1.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        // Agregamos Logo institucional
                        string pathLogo = Path.Combine(_pathOptions.Path_FileServer_root, _pathOptions.Logo);
                        hoja1.Cells[1, 3, filaInicioTabla - 1, 4].Merge = true;
                        report.AgregarImagen(hoja1, "Logo", pathLogo, 0, 0, 2, 0, 235, 60);

                        // Ajustamos las columnas al tamñano máximo
                        report.AjustarColumnas(hoja1, lastColumn);

                        // Configuramos el encabezado
                        report.PreparaEncabezado(hoja1, filaInicioTabla, 1, filaInicioTabla, lastColumn, Color.White, "#273580");

                        // Combinamos las celdas del campo factura
                        var cellsRange = hoja1.Cells[6, 1, lastRow, 1];
                        cellsRange.Merge = true;

                        // Centramos el valor del campo factura
                        hoja1.Cells[6, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja1.Cells[6, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        // Estilo de tabla
                        report.SimulaTabla(hoja1, filaInicioTabla, 1, lastRow, lastColumn, ExcelBorderStyle.Thin, Color.Black);
                    }
                    else
                    {
                        var hoja1 = report.AgregarHoja("Hoja 1");

                        hoja1.Cells["A1"].Value = "LA FACTURA O DOCUMENTO '" + facturas[0].Numero_Documento + @"' CONSULTADO NO SE LE PRACTICÓ NINGÚN TIPO DE RETENCIÓN";
                        hoja1.Cells["A1"].Style.Font.Bold = true;

                        // Ajustamos la columna al tamñano máximo
                        hoja1.Column(1).AutoFit();
                    }
                }

                // Creamos el stream del excel
                MemoryStream ms = new MemoryStream(report.Libro.GetAsByteArray());
                return ms;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<MemoryStream> FacturaPagadaPdf(GenerateFactPag param)
        {
            try
            {
                QuerySearchEstadoCuentasPagadasDetalle queryDtePag = new QuerySearchEstadoCuentasPagadasDetalle()
                {
                    Empresa = param.parameters.Empresa,
                    NitProveedor = param.parameters.NitProveedor,
                    Tipo_Documento_Alterno = param.parameters.Tipo_Documento_Alterno,
                    Numero_Documento_Alterno = param.parameters.Numero_Documento_Alterno,
                    Tipo_Documento_Causacion = param.parameters.Tipo_Documento_Causacion,
                    Numero_Documento_Causacion = param.parameters.Numero_Documento_Causacion
                };

                // Consultamos datos de la factura
                List<EstadoCuentasPagadasDetalle_Reporte> facturasPagadasDte = await param.apoteosysService.EstadoCuentasPagadasDetalleApoteosys_Reporte(queryDtePag);
                EstadoCuentasPagadasDetalle_Reporte factura = facturasPagadasDte?.Find(x => !(x.Numero_Documento_Alterno is null));

                // Consultamos pagos y retenciones del comprobante de egreso
                List<FacturasPagas> entity = await _unitOfWork.ParametrosInicialesRepository.GetFacturasPagas(param.parameters);
                List<FacturasPagas> pagos = entity?.Where(x => x.Tipo_Movimiento == "P").ToList();
                List<FacturasPagas> retenciones = entity?.Where(x => x.Tipo_Movimiento == "R").ToList();

                string html = string.Empty;

                // Consultamos data de la empresa
                List<Empresa> emp = _empresasService.EmpresasActivas;
                Empresa empresaGestion = new Empresa();

                if (param.parameters.Empresa == "NSGS")
                {
                    string tipoDocAlt = factura?.Tipo_Documento_Alterno[2..];
                    empresaGestion = emp.Find(x => x.EmpAbreviatura == param.parameters.Empresa && x.Emp_TipoDocAlternoApot == tipoDocAlt);
                }
                else
                {
                    empresaGestion = emp.Find(x => x.EmpAbreviatura == param.parameters.Empresa);
                }

                if (!(factura is null))
                {
                    string htmlDescuentos = string.Empty;

                    if (retenciones.Count > 0)
                    {
                        htmlDescuentos = @"
                        <table class=""table colorBlue"" style=""border-collapse:collapse; border-spacing:0; margin: 50px 0 10px 0;border:1px solid black;"">
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoCenter negrita"" style=""padding:5px;"">
                                    DETALLE DESCUENTO(S) APLICADO(S)
                                </td>
                            </tr>
                        </table>

                        <table class=""table colorBlue"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:0px;"">
                            <tr padding: 0px; margin: 0px; """">
                                <td class=""textoCenter td negrita"" style=""width: 30%;padding: 10px 0;"">
                                    No. FACTURA
                                </td>
                                <td class=""textoCenter td negrita"" style=""width: 40%;padding: 10px 0;"">
                                    DESCRIPCIÓN
                                </td>
                                <td class=""textoCenter td negrita"" style=""width: 30%;padding: 10px 0;"">
                                    VALOR RETENIDO
                                </td>
                            </tr>
                        </table>

                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;border:1px solid black;"">
                            " + MaperarFacturaPagadasDetallePdf(2, retencionesFactPagas: retenciones) + @"
                        </table>
                    ";
                    }
                    else
                    {
                        htmlDescuentos = @"
                        <table class=""table SinDescuentos"" style=""border-collapse:collapse; border-spacing:0;margin-top:20px;"">
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class="" textoCenter td negrita"" style=""padding: 10px 0; width: 60%;"">
                                    LA FACTURA O DOCUMENTO CONSULTADO NO SE LE PRACTICÓ NINGÚN TIPO DE RETENCIÓN
                                </td>
                            </tr>
                        </table>
                    ";
                    }

                    // Armamos el HTML
                    html =
                        @"<html>
                    <head>
                        <style>
                            .colorBlue {
                                background-color: #034F8A;
                                color: white;
                            }
                            .colorGreen {
                                background-color: #46a13a;
                                color: white;
                            }
                            .table {
                                width: 100%;
                            }
                            .td {
                                font-family: Arial, sans-serif;
                                font-size: 10px;
                                padding: 2px 2px;
                                word-break: normal;
                            }
                            .textoCenter {
                                text-align: center;
                            }
                            .textoDerecha {
                                text-align: right;
                            }
                            .textoIzq {
                                text-align: left;
                            }
                            .total {
                                padding-left: 10px;
                                padding-top: 10px;
                                padding-right: 15px;
                                padding-bottom: 10px;
                                //border-top: 1px solid black;
                                width: 100%;
                                font-size: 12px;
                                background-color: #D2D5E3;
                            }
                            .negrita {
                                font-weight: bold;
                            }
                            .textFinal {
                                width: 100%;
                                margin-top: 20px;
                            }
                            .twoContenedor {
                                padding-left: 10px;
                                padding-top: 0px;
                                padding-right: 5px;
                                padding-bottom: 0px;
                            }
                            .SinDescuentos {
                                border:1px solid black;
                            }
                        </style>
                    </head>

                    <body>
                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;"">
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""negrita"" style=""padding-left: 10px; width: 57%;"">
                                    " + empresaGestion.Emp_NombreEmpresaGnr + @"
                                </td>
                                <td class=""textoDerecha"" style=""width: 43%;"">
                                    <img src=""" + empresaGestion.CodArchivo + @""" width=""200px"" height=""50px"" >
                                </td>
                            </tr>
                        </table>
                          
                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:10px;"">
                            <tr>
                                <td class=""twoContenedor"" style=""padding-bottom:7px; padding-top-10px;width: 40%;"">                                       
                                    " + empresaGestion.EmpDireccion + @"
                                </td>
                                <td class=""twoContenedor textoIzq"" style=""width: 60%;"">
                                    
                                </td>
                            </tr>
                            <tr>
                                <td class=""twoContenedor"" style=""padding-bottom:7px; padding-top-10px;width: 40%;"">
                                    <b>NIT:</b> " + empresaGestion.EmpNit + @"
                                </td>
                            </tr>
                        </table>

                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:10px;border:1px solid black;"">    
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoIzq negrita"" style=""width: 20%; padding-left: 15px;"">                                       
                                    Contabilidad:
                                </td>
                                <td class=""textoIzq"" style=""padding: 5px; width: 45%;"" colspan=""3"">
                                    " + empresaGestion.EmpNombreEmpresa + @"
                                </td>
                                <td style=""width: 15%;"">
                                    
                                </td>
                                <td style=""width: 20%;"">
                                    
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoIzq negrita"" style=""width: 20%; padding-left: 15px;"">                                       
                                    Concepto comprobante:
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 45%;"" colspan=""3"">
                                    FACTURA DE COMPRA " + empresaGestion.Emp_Ciudad + @"
                                </td>
                                <td class=""textoDerecha negrita"" style=""width: 15%; "">
                                    
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 20%; "">
                                    
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoIzq negrita"" style=""width: 20%; padding-left: 15px;"">
                                    Proveedor:
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 45%; "" colspan=""3"">
                                    " + string.Concat(factura.Nombre_Tercero, " - NIT. ", param.parameters.NitProveedor) + @"
                                </td>
                                <td class=""textoDerecha negrita"" style=""width: 15%; "">
                                    
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 20%; "">
                                    
                                </td>
                            </tr>
                        </table>

                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:10px;border:1px solid black;"">    
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoIzq negrita"" style=""width: 30%; padding-left: 15px;"">                                       
                                    Deposito Realizado a:
                                </td>
                                <td class=""textoIzq"" style=""padding: 5px; width: 70%;"">
                                    La cuenta " + factura.Tipo_Cuenta_Banco + " " + factura.Cuenta_Consignacion + @" de " + factura.Nombre_Banco + @"
                                </td>
                            </tr>
                        </table>

                        <table class=""table colorBlue"" style=""border-collapse:collapse; border-spacing:0;border:1px solid black;margin-bottom:10px;"">
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoCenter negrita"" style=""padding:5px;"">
                                    DETALLE PAGO(S) REALIZADO(S)
                                </td>
                            </tr>
                        </table>

                        <table class=""table colorBlue"" style=""border-collapse:collapse; border-spacing:0; margin-bottom:0px; "">
                            <tr style=""padding: 0px; margin: 0px; "" >         
                                <td class=""textoCenter td negrita"" style=""width: 20%;padding: 10px 0;"">
                                    FECHA DE PAGO
                                </td>
                                <td class=""textoCenter td negrita"" style=""width: 20%;padding: 10px 0;"">
                                    No. FACTURA
                                </td>
                                <td class=""textoCenter td negrita"" style=""width: 30%;padding: 10px 0;"">
                                    VALOR PAGADO
                                </td>
                                <td class=""textoCenter td negrita"" style=""width: 30%;padding: 10px 0;"">
                                    TOTAL DEPOSITADO
                                </td>
                            </tr>
                        </table>

                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;border:1px solid black;"">
                            " + MaperarFacturaPagadasDetallePdf(1, facturasPagadasDte: pagos) + @"
                        </table>                        

                        " + htmlDescuentos + @"
                    </body>
                </html>";
                }
                else
                {
                    html =
                        @"<html>
                        <head>
                            <style>
                                .colorBlue {
                                    background-color: #034F8A;
                                    color: white;
                                }

                                .colorGreen {
                                    background-color: #46a13a;
                                    color: white;
                                }

                                .table {
                                    width: 100%;
                                }

                                .td {
                                    font-family: Arial, sans-serif;
                                    font-size: 12px;
                                    padding: 2px 2px;
                                    word-break: normal;
                                }

                                .textoCenter {
                                    text-align: center;
                                }

                                .textoDerecha {
                                    text-align: right;
                                }

                                .textoIzq {
                                    text-align: left;
                                }

                                .negrita {
                                    font-weight: bold;
                                }

                                .twoContenedor {
                                    padding-left: 10px;
                                    padding-top: 0px;
                                    padding-right: 5px;
                                    padding-bottom: 0px;
                                }
        
                                .SinDescuentos {
                                    border:1px solid black;
                                }
                            </style>
                        </head>

                        <body>
                            <table class=""table"" style=""border-collapse:collapse; border-spacing:0;"">
                                <tr style=""padding: 0px; margin: 0px;"">
                                    <td class=""negrita"" style=""padding-left: 10px; width: 57%;"">
                                        
                                    </td>
                                    <td class=""textoDerecha"" style=""width: 43%;"">
                                        <img src=""" + empresaGestion.CodArchivo + @""" width=""200px"" height=""50px"" >
                                    </td>
                                </tr>
                            </table>

                            <table class=""table SinDescuentos"" style=""border-collapse:collapse; border-spacing:0;margin-top:20px;"">
                                <tr style=""padding: 0px; margin: 0px;"">
                                    <td class="" textoCenter td negrita"" style=""padding: 10px 0; width: 60%;"">
                                        NO SE OBTUVIERON DETALLES DEL PAGO
                                    </td>
                                </tr>
                            </table>
                        </body>

                        </html>";
                }

                // Creamos el stream del pdf
                MemoryStream ms = new MemoryStream(Funciones.PdfSharpConvertWithoutCreateFile(html));
                return ms;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<MemoryStream> FacturaPagadaXlsx(GenerateFactPag param)
        {
            try
            {
                QuerySearchEstadoCuentasPagadasDetalle queryDtePag = new QuerySearchEstadoCuentasPagadasDetalle()
                {
                    Empresa = param.parameters.Empresa,
                    NitProveedor = param.parameters.NitProveedor,
                    Tipo_Documento_Alterno = param.parameters.Tipo_Documento_Alterno,
                    Numero_Documento_Alterno = param.parameters.Numero_Documento_Alterno,
                    Tipo_Documento_Causacion = param.parameters.Tipo_Documento_Causacion,
                    Numero_Documento_Causacion = param.parameters.Numero_Documento_Causacion
                };

                // Consultamos datos de la factura
                List<EstadoCuentasPagadasDetalle_Reporte> facturasPagadasDte = await param.apoteosysService.EstadoCuentasPagadasDetalleApoteosys_Reporte(queryDtePag);
                EstadoCuentasPagadasDetalle_Reporte factura = facturasPagadasDte?.Find(x => !(x.Numero_Documento_Alterno is null));

                // Consultamos pagos y retenciones del comprobante de egreso
                List<FacturasPagas> entity = await _unitOfWork.ParametrosInicialesRepository.GetFacturasPagas(param.parameters);
                List<FacturasPagas> pagos = entity?.Where(x => x.Tipo_Movimiento == "P").ToList();
                List<FacturasPagas> retenciones = entity?.Where(x => x.Tipo_Movimiento == "R").ToList();
                ReportExcel report = new ReportExcel();
                byte[] resp = Array.Empty<byte>();

                if (!(factura is null))
                {
                    // Sección Pagos
                    List<PagosDteExcel> pagosDteExcels = (from a in pagos
                                                          select new PagosDteExcel
                                                          {
                                                              FACTURA = a.Nombre_Cuenta,
                                                              FECHA = a.Fecha,
                                                              VALOR_PAGADO = a.Debito
                                                          }).ToList();

                    var hoja1 = report.AgregarHoja("Detalle factura");

                    int filaInicioTabla = 5;

                    // Colocamos la información de la empresa de la factura
                    hoja1.Cells["A1"].Value = facturasPagadasDte.Find(x => x.Numero_Documento_Alterno is null).NombreEmpresa;
                    hoja1.Cells["A1"].Style.Font.Bold = true;
                    hoja1.Cells[1, 1, filaInicioTabla - 1, 2].Merge = true;
                    hoja1.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    // Agregamos Logo institucional
                    string pathLogo = Path.Combine(_pathOptions.Path_FileServer_root, _pathOptions.Logo);
                    hoja1.Cells[1, 3, filaInicioTabla - 1, 4].Merge = true;
                    report.AgregarImagen(hoja1, "Logo", pathLogo, 0, 0, 2, 0, 235, 60);

                    hoja1.Cells[$"A{filaInicioTabla}"].Value = "PAGOS";
                    hoja1.Cells[$"A{filaInicioTabla}"].Style.Font.Bold = true;

                    // Cargamos la data en el excel
                    report.CargarData(hoja1, pagosDteExcels, $"A{filaInicioTabla + 1}");

                    // Determinamos la ultima fila y la ultima columna en la hoja, hasta el momento.
                    (int, int) ultimosDatos = report.DeterminarUltimaFila_Y_Columna(hoja1);
                    int lastRow = ultimosDatos.Item1;
                    int lastColumn = ultimosDatos.Item2;

                    // Ajustamos las columnas al tamñano máximo
                    report.AjustarColumnas(hoja1, lastColumn);

                    // Configuramos el encabezado
                    report.PreparaEncabezado(hoja1, filaInicioTabla + 1, 1, filaInicioTabla + 1, lastColumn, Color.White, "#273580");

                    // Estilo de tabla
                    report.SimulaTabla(hoja1, filaInicioTabla + 1, 1, lastRow, lastColumn, ExcelBorderStyle.Thin, Color.Black);

                    // Alinear a la derecha valores de Pagos
                    string cad1 = $"C7:C{6 + pagosDteExcels.Count}";
                    hoja1.Cells[cad1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    // Sección Descuentos Aplicados
                    if (retenciones.Count > 0)
                    {
                        List<EstadoCuentasXPagarDteExcel> listDteFact = (from a in retenciones
                                                                         select new EstadoCuentasXPagarDteExcel
                                                                         {
                                                                             FACTURA = a.Nombre_Cuenta,
                                                                             BASE_RETENCIÓN = a.Base_Retencion,
                                                                             VALOR_RETENIDO = a.Credito,
                                                                             DESCRIPCIÓN = a.Nombre_Cuenta_Con
                                                                         }).ToList();

                        int inicioRowRetenciones = pagosDteExcels.Count + 10;
                        string inicioRowRet = $"A{inicioRowRetenciones}";

                        hoja1.Cells[inicioRowRet].Value = "DESCUENTOS APLICADOS";
                        hoja1.Cells[inicioRowRet].Style.Font.Bold = true;

                        int celdaInicialRetenciones = inicioRowRetenciones + 1;

                        // Cargamos la data en el excel
                        report.CargarData(hoja1, listDteFact, $"A{celdaInicialRetenciones}");

                        // Determinamos la ultima fila y la ultima columna en la hoja, hasta el momento.
                        ultimosDatos = report.DeterminarUltimaFila_Y_Columna(hoja1);
                        lastRow = ultimosDatos.Item1;
                        lastColumn = ultimosDatos.Item2;

                        // Ajustamos las columnas al tamñano máximo
                        report.AjustarColumnas(hoja1, lastColumn);

                        // Configuramos el encabezado
                        report.PreparaEncabezado(hoja1, celdaInicialRetenciones, 1, celdaInicialRetenciones, lastColumn, Color.White, "#273580");

                        // Estilo de tabla
                        report.SimulaTabla(hoja1, celdaInicialRetenciones, 1, lastRow, lastColumn, ExcelBorderStyle.Thin, Color.Black);

                        // Alinear a la derecha valores de Retenciones
                        string cad2 = $"C{2 + inicioRowRetenciones}:D{(inicioRowRetenciones + 1) + listDteFact.Count}";
                        hoja1.Cells[cad2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }
                    else
                    {
                        // Determinamos la ultima fila y la ultima columna en la hoja, hasta el momento.
                        ultimosDatos = report.DeterminarUltimaFila_Y_Columna(hoja1);
                        lastRow = ultimosDatos.Item1 + 4;
                        lastColumn = ultimosDatos.Item2;

                        var filaInicialRetenciones = hoja1.Cells[$"A{lastRow}"];
                        filaInicialRetenciones.Value = "DESCUENTOS APLICADOS";
                        filaInicialRetenciones.Style.Font.Bold = true;

                        var filaDataRetenciones = hoja1.Cells[$"A{lastRow + 1}"];
                        filaDataRetenciones.Value = "LA FACTURA O DOCUMENTO CONSULTADO NO SE LE PRACTICÓ NINGÚN TIPO DE RETENCIÓN";
                        filaDataRetenciones.Style.Font.Bold = true;

                        // Ajustamos la columna al tamñano máximo
                        hoja1.Column(1).AutoFit();
                    }
                }
                else
                {
                    var hoja1 = report.AgregarHoja("Hoja 1");

                    hoja1.Cells["A1"].Value = "NO SE OBTUVIERON DETALLES DEL PAGO";
                    hoja1.Cells["A1"].Style.Font.Bold = true;

                    // Ajustamos la columna al tamñano máximo
                    hoja1.Column(1).AutoFit();
                }

                // Creamos el stream del pdf
                MemoryStream ms = new MemoryStream(report.Libro.GetAsByteArray());
                return ms;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<string> CertificadoRetencionFuentePdf(QuerySearchCertificados parameters)
        {
            try
            {
                string periodoCertificado = parameters.Month == 0 ? $"01/{parameters.Year} - 12/{parameters.Year}" : $"{Funciones.FormatearMes(parameters.Month)}/{parameters.Year}";

                // Consultamos datos de la factura
                List<CertificadoRetencionMaestro> retencionMaestro = await _unitOfWork.ApoteosysRepository.CertificadosRetencionApoteosys(parameters);
                List<CertificadoRetencionFuenteDte> retencionDetalle = await _unitOfWork.ApoteosysRepository.CertificadosRetencionFuenteApoteosys(parameters);

                // Consultamos data de la empresa
                List<Empresa> emp = _empresasService.EmpresasActivas;
                var empresaGestion = emp.Find(x => x.EmpAbreviatura == parameters.Empresa);

                string html = string.Empty;

                if (retencionDetalle.Count > 0)
                {
                    // Creamos el QR de validación y le anexamos la información con la Url y el token
                    string qrCodeHtml = Funciones.GetQrCodeHtml(parameters.UrlToken);

                    string totalConFormato = retencionDetalle.Find(x => x.Total == "1").Credito;
                    decimal total = decimal.Parse(retencionDetalle.Find(x => x.Total == "1").CreditoSinFormato);
                    retencionDetalle = retencionDetalle.FindAll(x => x.Total == "0");

                    // Armamos el HTML
                    html =
                        @"<html>
                    <head>
                        <style>
                            .colorBlue {
                                background-color: #034F8A;
                                color: white;
                            }
                            .colorGreen {
                                background-color: #46a13a;
                                color: white;
                            }
                            .table {
                                width: 100%;
                            }
                            .td {
                                font-family: Arial, sans-serif;
                                font-size: 12px;
                                padding: 2px 2px;
                                word-break: normal;
                                text-align: center;     
                            }
                            .textoCenter {
                                text-align: center;
                            }
                            .textoDerecha {
                                text-align: right;
                            }
                            .textoIzq {
                                text-align: left;
                            }
                            .valorTotal {
                                width: 15%;
                                font-weight: bold;
                                font-size: 13px;
                                padding:10px;
                                border-top:1px solid black;
                                background-color: #D2D5E3;
                            }
                            .total {
                                padding-left: 10px;
                                padding-top: 10px;
                                padding-right: 15px;
                                padding-bottom: 10px;
                                border-top:1px solid black; 
                                width: 85%; 
                                font-size:10px; 
                                background-color: #D2D5E3;
                            }
                            .negrita{
                                 font-weight: bold;
                            }
                            .titlePrincipal{
                                font-weight: bold;
                                font-family: Arial, sans-serif;
                                font-size: 17px;
                                word-break: normal;
                                text-align: center;  
                            }
                            .textFinal{
                                width: 100%;
                                margin-top:20px;    
                            }
                            .twoContenedor{
                                padding-left: 10px;
                                padding-top: 0px;
                                padding-right: 5px;
                                padding-bottom: 0px;
                            }
                        </style>
                    </head>

                    <body>
                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;"">
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""negrita"" style=""padding-left: 10px; width: 57%;"">
                                        CERTIFICADO DE RETENCIÓN FUENTE
                                </td>
                                <td class=""textoDerecha"" style=""width: 43%;"" rowspan=""2"">
                                    <img src=""" + empresaGestion.CodArchivo + @""" width=""200px"" height=""50px"">
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""negrita"" style=""padding-left: 10px; width: 57%;"">
                                        " + empresaGestion.Emp_NombreEmpresaGnr + @"
                                </td>
                                <td class="""" style=""width: 43%;"">
                                          
                                </td>
                            </tr>
                        </table>
                          
                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:10px;"">
                            <tr>
                                <td class=""twoContenedor"" style=""padding-bottom:7px; padding-top-10px;width: 40%;"">                                       
                                    " + empresaGestion.EmpDireccion + @"
                                </td>
                                <td class=""twoContenedor textoIzq"" style=""width: 60%;"">
                                    
                                </td>
                            </tr>
                            <tr>
                                <td class=""twoContenedor"" style=""padding-bottom:7px; padding-top-10px;width: 40%;"">
                                    <b>NIT:</b> " + empresaGestion.EmpNit + @"
                                </td>
                            </tr>
                        </table>
                           
                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:10px;border:1px solid black;"">    
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoIzq negrita"" style=""width: 30%; padding-left: 15px;"">                                       
                                    Año gravable
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 30%; "">
                                    " + parameters.Year + @"
                                </td>
                                <td class=""textoDerecha negrita"" style=""width: 25%; "">
                                    Fecha de Expedición
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 15%; "">
                                    " + DateTime.Now.ToString("dd/MM/yyyy") + @"
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoIzq negrita"" style=""width: 30%; padding-left: 15px;"">                                       
                                    Periodo
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 30%; "">
                                    " + periodoCertificado + @"
                                </td>
                                <td class=""textoIzq negrita"" style=""width: 25%; "">
                                        
                                </td>
                                <td class=""textoIzq negrita"" style=""padding:5px; width: 15%; "">
                                        
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoIzq negrita"" style=""width: 30%; padding-left: 15px;"">                                       
                                    Retención efectuada a
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 30%; "">
                                    " + retencionMaestro[0].Razon_Social + @"
                                </td>
                                <td class=""textoIzq negrita"" style=""width: 25%; "">
                                        
                                </td>
                                <td class=""textoIzq negrita"" style=""padding:5px; width: 15%; "">
                                        
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoIzq negrita"" style=""width: 30%; padding-left: 15px;"">                                       
                                    NIT
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 30%;"">
                                    " + retencionMaestro[0].Nit + @"
                                </td>
                                <td class=""textoDerecha negrita"" style=""width: 25%; "">
                                        
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 15%; "">
                                        
                                </td>
                            </tr>
                        </table>

                        <table class=""table colorBlue"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:0px;"">
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoCenter td negrita"" style=""width: 40%;padding: 10px 0;"">
                                    CONCEPTO
                                </td>
                                <td class=""textoCenter td negrita"" style=""width: 30%;padding: 10px 0;"">
                                    BASE RETENCIÓN
                                </td>
                                <td class=""textoCenter td negrita"" style=""width: 30%;padding: 10px 0;"">
                                    VALOR RETENIDO
                                </td>
                            </tr>
                        </table>

                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;"">
                            " + MaperarCertificadoFuenteDetallePdf(retencionDetalle) + @"
                        </table>      
                             
                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;width: 100%"">    
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""total textoIzq"">                                       
                                    <b>TOTAL SON: </b>" + Funciones.NumeroALetras(total) + @" MCTE
                                </td>
                                <td class=""valorTotal textoCenter textoIzq"" style="""">
                                    " + totalConFormato + @"
                                </td>
                            </tr>
                        </table>                           
                             
                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:40px;"">    
                            <tr style=""border-top:none;"" >
                                <td class=""textoIzq "" style=""width: 100%; padding-top: 5px; font-size:10px"">
                                    Este certificado se expide de conformidad con lo dispuesto en el artículo 1.6.1.12.12 del D.U. 1625 de 2016.
                                </td>
                            </tr>
                        </table>

                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;"">    
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td style=""text-align: justify; font-size:10px"">
                                    <b>Certificados de retención en la fuente</b>, las personas jurídicas podrán entregar los certificados de retención en la fuente, en forma continua impresa por computador, sin necesidad de firma autógrafa. 
                                    (Art. 10, D.R. 836 de 1991)
                                </td>
                            </tr>
                        </table>

                        <table class=""textFinal"" style=""border-collapse:collapse; border-spacing:0;"">    
                            <tr style=""border-top:none;"" >
                                <td class=""textoCenter "" style=""width: 100%;"">
                                    " + qrCodeHtml + @"
                                </td>
                            </tr>
                        </table>
                    </body>
                </html>";
                }
                else
                {

                    html =
                        @"<html>
                        <head> 
                            <style>
                                .contenedorOne {
                                    text-align: center;
                                    width: 60%;
                                    padding: 20px;
                                    margin: 40px;
                                    border-radius: 20px;
                                    border-width: 1px;
                                    border-style: solid;
                                    border-color: red;
                                    
                                  }
                            </style>
                        </head>
                         <body>
                            <div class=""contenedorOne"" >
                               <div>
                                    <h1>No se encuentraron datos para la consulta realizada</h1>
                                </div
                             </div>
                         </body>
                        <html>";
                }

                return html;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<string> CertificadoRetencionIVAPdf(QuerySearchCertificados parameters)
        {
            try
            {
                string periodoCertificado = $"{Funciones.FormatearMes(parameters.Month)}/{parameters.Year} - {Funciones.FormatearMes(parameters.Month + 1)}/{parameters.Year}";

                // Consultamos datos de la factura
                List<CertificadoRetencionMaestro> retencionMaestro = await _unitOfWork.ApoteosysRepository.CertificadosRetencionApoteosys(parameters);
                List<CertificadoRetencionIvaDte> retencionDetalle = await _unitOfWork.ApoteosysRepository.CertificadosRetencionIvaApoteosys(parameters);

                // Consultamos data de la empresa
                List<Empresa> emp = _empresasService.EmpresasActivas;
                var empresaGestion = emp.Find(x => x.EmpAbreviatura == parameters.Empresa);

                string html = string.Empty;

                if (retencionDetalle.Count > 0)
                {
                    // Creamos el QR de validación y le anexamos la información con la Url y el token
                    string qrCodeHtml = Funciones.GetQrCodeHtml(parameters.UrlToken);

                    string totalConFormato = retencionDetalle.Find(x => x.Total == "1").Credito;
                    decimal total = decimal.Parse(retencionDetalle.Find(x => x.Total == "1").CreditoSinFormato);
                    retencionDetalle = retencionDetalle.FindAll(x => x.Total == "0");

                    // Armamos el HTML
                    html =
                        @"<html>
                    <head>
                        <style>
                            .colorBlue {
                                background-color: #034F8A;
                                color: white;
                            }
                            .colorGreen {
                                background-color: #46a13a;
                                color: white;
                            }
                            .table {
                                width: 100%;
                            }
                            .td {
                                font-family: Arial, sans-serif;
                                font-size: 12px;
                                padding: 2px 2px;
                                word-break: normal;
                                text-align: center;     
                            }
                            .textoCenter {
                                text-align: center;
                            }
                            .textoDerecha {
                                text-align: right;
                            }
                            .textoIzq {
                                text-align: left;
                            }
                            .valorTotal {
                                width: 15%;
                                font-weight: bold;
                                font-size: 13px;
                                padding:10px;
                                border-top:1px solid black;
                                background-color: #D2D5E3;
                            }
                            .total {
                                padding-left: 10px;
                                padding-top: 10px;
                                padding-right: 15px;
                                padding-bottom: 10px;
                                border-top:1px solid black; 
                                width: 85%; 
                                font-size:10px; 
                                background-color: #D2D5E3;
                            }
                            .negrita{
                                 font-weight: bold;
                            }
                            .titlePrincipal{
                                font-weight: bold;
                                font-family: Arial, sans-serif;
                                font-size: 17px;
                                word-break: normal;
                                text-align: center;  
                            }
                            .textFinal{
                                width: 100%;
                                margin-top:20px;    
                            }
                            .twoContenedor{
                                padding-left: 10px;
                                padding-top: 0px;
                                padding-right: 5px;
                                padding-bottom: 0px;
                            }
                        </style>
                    </head>

                    <body>
                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;"">
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""negrita"" style=""padding-left: 10px; width: 57%;"">
                                        CERTIFICADO DE RETENCIÓN IVA
                                </td>
                                <td class=""textoDerecha"" style=""width: 43%;"" rowspan=""2"">
                                        <img src=""" + empresaGestion.CodArchivo + @""" width=""200px"" height=""50px"" >
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""negrita"" style=""padding-left: 10px; width: 57%;"">
                                        " + empresaGestion.Emp_NombreEmpresaGnr + @"
                                </td>
                                <td class="""" style=""width: 43%;"">
                                          
                                </td>
                            </tr>
                        </table>
                          
                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:10px;"">
                            <tr>
                                <td class=""twoContenedor"" style=""padding-bottom:7px; padding-top-10px;width: 40%;"">                                       
                                    " + empresaGestion.EmpDireccion + @"
                                </td>
                                <td class=""twoContenedor textoIzq"" style=""width: 60%;"">
                                    
                                </td>
                            </tr>
                            <tr>
                                <td class=""twoContenedor"" style=""padding-bottom:7px; padding-top-10px;width: 40%;"">
                                    <b>NIT:</b> " + empresaGestion.EmpNit + @"
                                </td>
                            </tr>
                        </table>
                           
                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:10px;border:1px solid black;"">    
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoIzq negrita"" style=""width: 30%; padding-left: 15px;"">                                       
                                    Año gravable
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 30%; "">
                                    " + parameters.Year + @"
                                </td>
                                <td class=""textoDerecha negrita"" style=""width: 25%; "">
                                    Fecha de Expedición
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 15%; "">
                                    " + DateTime.Now.ToString("dd/MM/yyyy") + @"
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoIzq negrita"" style=""width: 30%; padding-left: 15px;"">                                       
                                    Periodo
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 30%; "">
                                    " + periodoCertificado + @"
                                </td>
                                <td class=""textoIzq negrita"" style=""width: 25%; "">
                                        
                                </td>
                                <td class=""textoIzq negrita"" style=""padding:5px; width: 15%; "">
                                        
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoIzq negrita"" style=""width: 30%; padding-left: 15px;"">                                       
                                    Retención efectuada a
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 30%; "">
                                    " + retencionMaestro[0].Razon_Social + @"
                                </td>
                                <td class=""textoIzq negrita"" style=""width: 25%; "">
                                        
                                </td>
                                <td class=""textoIzq negrita"" style=""padding:5px; width: 15%; "">
                                        
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoIzq negrita"" style=""width: 30%; padding-left: 15px;"">                                       
                                    NIT
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 30%;"">
                                    " + retencionMaestro[0].Nit + @"
                                </td>
                                <td class=""textoDerecha negrita"" style=""width: 25%; "">
                                        
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 15%; "">
                                        
                                </td>
                            </tr>
                        </table>

                        <table class=""colorBlue"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:0px;width:100%;"">
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class="" textoCenter td negrita"" style=""width: 35%;padding: 10px 0;"">
                                    CONCEPTO
                                </td>
                                <td class="" textoCenter td negrita"" style=""width: 15%;padding: 10px 0;"">
                                    %PRACTICADO
                                </td>
                                <td class="" textoCenter td negrita"" style=""width: 15%;padding: 10px 0;"">
                                    MONTO ORIGEN
                                </td>
                                <td class="" textoCenter td negrita"" style=""width: 15%;padding: 10px 0;"">
                                    BASE RETENCIÓN IVA
                                </td>
                                <td class="" textoCenter td negrita"" style=""width: 20%;padding: 10px 0;"">
                                    VALOR RETENIDO
                                </td>
                            </tr>
                        </table>

                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;"">
                            " + MaperarCertificadoIvaDetallePdf(retencionDetalle) + @"
                        </table>      
                             
                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;"">    
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""total textoIzq"">                                       
                                    <b>TOTAL SON: </b>" + Funciones.NumeroALetras(total) + @" MCTE
                                </td>
                                <td class=""valorTotal textoCenter textoIzq"" style="""">
                                    " + totalConFormato + @"
                                </td>
                            </tr>
                        </table>                           
                             
                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:40px;"">    
                            <tr style=""border-top:none;"" >
                                <td class=""textoIzq "" style=""width: 100%; padding-top: 5px; font-size:10px"">
                                    Este certificado se expide de conformidad con lo dispuesto en el artículo 1.6.1.12.12 y 1.6.1.12.13 del D.U. 1625 de 2016.
                                </td>
                            </tr>
                        </table>

                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;"">    
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td style=""text-align: justify; font-size:10px"">
                                    <b>Certificados de retención en la fuente</b>, las personas jurídicas podrán entregar los certificados de retención en la fuente, en forma continua impresa por computador, sin necesidad de firma autógrafa. 
                                    (Art. 10, D.R. 836 de 1991)
                                </td>
                            </tr>
                        </table>

                        <table class=""textFinal"" style=""border-collapse:collapse; border-spacing:0;"">    
                            <tr style=""border-top:none;"" >
                                <td class=""textoCenter "" style=""width: 100%;"">
                                    " + qrCodeHtml + @"
                                </td>
                            </tr>
                        </table>
                    </body>
                </html>";
                }
                else
                {
                    html = "<html>No se encuentraron datos para la consulta realizada<html>";
                }

                return html;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<(string, byte[])> CertificadoRetencionIca2Pdf(QuerySearchCertificados parameters)
        {
            try
            {
                // Consultamos data de la empresa
                List<Empresa> empresas = await _unitOfWork.ParametrosInicialesRepository.GetEmpresasByUser(int.Parse(parameters.CodUser), parameters.TipoUsuario);
                List<Empresa> empresasActivas = empresas.Where(x => (bool)x.EmpEstado).ToList();
                empresas = empresasActivas.GroupBy(p => new { p.EmpAbreviatura, p.Emp_CuentaApot }).Select(g => g.First()).OrderBy(x => x.Emp_CuentaApot).ToList();

                string periodoCertificado = string.Empty;
                string html = string.Empty;
                string htmlConcatenado = string.Empty;

                foreach (var empresaGestion in empresas)
                {
                    if (parameters.Periodicidad == 1) // Anual
                    {
                        periodoCertificado = $"{parameters.Year}";
                    }
                    else if (parameters.Periodicidad == 2) // Mensual
                    {
                        periodoCertificado = $"{Funciones.FormatearMes(parameters.Month)}/{parameters.Year}";
                    }
                    else if (parameters.Periodicidad == 3) // Bimestral
                    {
                        periodoCertificado = $"{Funciones.FormatearMes(parameters.Month)}/{parameters.Year} - {Funciones.FormatearMes(parameters.Month + 1)}/{parameters.Year}";
                    }

                    QuerySearchCertificadosICA query = new QuerySearchCertificadosICA()
                    {
                        Empresa = empresaGestion.EmpAbreviatura,
                        Month = parameters.Month,
                        NitProveedor = parameters.NitProveedor,
                        Year = parameters.Year,
                        IsBimensual = empresaGestion.Emp_IsBimestralApot,
                        Cuenta = empresaGestion.Emp_CuentaApot,
                        Periodicidad = parameters.Periodicidad
                    };

                    // Consultamos datos de la factura
                    List<CertificadoRetencionMaestro> retencionMaestro = await _unitOfWork.ApoteosysRepository.CertificadosRetencionApoteosys(parameters);
                    List<CertificadoRetencionIcaDte> retencionDetalle = await _unitOfWork.ApoteosysRepository.CertificadosRetencionIcaApoteosys(query);

                    if (retencionDetalle.Count > 0)
                    {
                        // Creamos el QR de validación y le anexamos la información con la Url y el token
                        string qrCodeHtml = Funciones.GetQrCodeHtml(parameters.UrlToken);

                        string totalConFormato = retencionDetalle.Find(x => x.Total == "1").Credito;
                        decimal total = decimal.Parse(retencionDetalle.Find(x => x.Total == "1").CreditoSinFormato);
                        retencionDetalle = retencionDetalle.FindAll(x => x.Total == "0");

                        // Armamos el HTML
                        html =
                            @"<html>
                            <head>
                                <style>
                                    .colorBlue {
                                        background-color: #034F8A;
                                        color: white;
                                    }
                                    .colorGreen {
                                        background-color: #46a13a;
                                        color: white;
                                    }
                                    .table {
                                        width: 100%;
                                    }
                                    .td {
                                        font-family: Arial, sans-serif;
                                        font-size: 12px;
                                        padding: 2px 2px;
                                        word-break: normal;
                                        text-align: center;     
                                    }
                                    .textoCenter {
                                        text-align: center;
                                    }
                                    .textoDerecha {
                                        text-align: right;
                                    }
                                    .textoIzq {
                                        text-align: left;
                                    }
                                    .valorTotal {
                                        width: 15%;
                                        font-weight: bold;
                                        font-size: 13px;
                                        padding:10px;
                                        border-top:1px solid black;
                                        background-color: #D2D5E3;
                                    }
                                    .total {
                                        padding-left: 10px;
                                        padding-top: 10px;
                                        padding-right: 15px;
                                        padding-bottom: 10px;
                                        border-top:1px solid black; 
                                        width: 85%; 
                                        font-size:10px; 
                                        background-color: #D2D5E3;
                                    }
                                    .negrita{
                                         font-weight: bold;
                                    }
                                    .titlePrincipal{
                                        font-weight: bold;
                                        font-family: Arial, sans-serif;
                                        font-size: 17px;
                                        word-break: normal;
                                        text-align: center;  
                                    }
                                    .textFinal{
                                        width: 100%;
                                        margin-top:20px;    
                                    }
                                    .twoContenedor{
                                        padding-left: 10px;
                                        padding-top: 0px;
                                        padding-right: 5px;
                                        padding-bottom: 0px;
                                    }
                                </style>
                            </head>

                            <body>
                                <table class=""table"" style=""border-collapse:collapse; border-spacing:0;"">
                                    <tr style=""padding: 0px; margin: 0px;"">
                                        <td class=""negrita"" style=""padding-left: 10px; width: 57%;"">
                                                CERTIFICADO DE RETENCIÓN ICA
                                        </td>
                                        <td class=""textoDerecha"" style=""width: 43%;"" rowspan=""2"">
                                                <img src=""" + empresaGestion.CodArchivo + @""" width=""200px"" height=""50px"" >
                                        </td>
                                    </tr>
                                    <tr style=""padding: 0px; margin: 0px;"">
                                        <td class=""negrita"" style=""padding-left: 10px; width: 57%;"">
                                                " + empresaGestion.Emp_NombreEmpresaGnr + @"
                                        </td>
                                        <td class="""" style=""width: 43%;"">
                                          
                                        </td>
                                    </tr>
                                </table>
                          
                                <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:10px;"">
                                    <tr>
                                        <td class=""twoContenedor"" style=""padding-bottom:7px; padding-top-10px;width: 40%;"">                                       
                                            " + empresaGestion.EmpDireccion + @"
                                        </td>
                                        <td class=""twoContenedor textoIzq"" style=""width: 60%;"">
                                    
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class=""twoContenedor"" style=""padding-bottom:7px; padding-top-10px;width: 40%;"">
                                            <b>NIT:</b> " + empresaGestion.EmpNit + @"
                                        </td>
                                    </tr>
                                </table>
                           
                                <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:10px;border:1px solid black;"">    
                                    <tr style=""padding: 0px; margin: 0px;"">
                                        <td class=""textoIzq negrita"" style=""width: 30%; padding-left: 15px;"">                                       
                                            Año gravable
                                        </td>
                                        <td class=""textoIzq"" style=""padding:5px; width: 30%; "">
                                            " + parameters.Year + @"
                                        </td>
                                        <td class=""textoDerecha negrita"" style=""width: 25%; "">
                                            Fecha de Expedición
                                        </td>
                                        <td class=""textoIzq"" style=""padding:5px; width: 15%; "">
                                            " + DateTime.Now.ToString("dd/MM/yyyy") + @"
                                        </td>
                                    </tr>
                                    <tr style=""padding: 0px; margin: 0px;"">
                                        <td class=""textoIzq negrita"" style=""width: 30%; padding-left: 15px;"">                                       
                                            Periodo
                                        </td>
                                        <td class=""textoIzq"" style=""padding:5px; width: 30%; "">
                                            " + periodoCertificado + @"
                                        </td>
                                        <td class=""textoIzq negrita"" style=""width: 25%; "">
                                        
                                        </td>
                                        <td class=""textoIzq negrita"" style=""padding:5px; width: 15%; "">
                                        
                                        </td>
                                    </tr>
                                    <tr style=""padding: 0px; margin: 0px;"">
                                        <td class=""textoIzq negrita"" style=""width: 30%; padding-left: 15px;"">                                       
                                            Retención efectuada a
                                        </td>
                                        <td class=""textoIzq"" style=""padding:5px; width: 30%; "">
                                            " + retencionMaestro[0].Razon_Social + @"
                                        </td>
                                        <td class=""textoIzq negrita"" style=""width: 25%; "">
                                        
                                        </td>
                                        <td class=""textoIzq negrita"" style=""padding:5px; width: 15%; "">
                                        
                                        </td>
                                    </tr>
                                    <tr style=""padding: 0px; margin: 0px;"">
                                        <td class=""textoIzq negrita"" style=""width: 30%; padding-left: 15px;"">                                       
                                            NIT
                                        </td>
                                        <td class=""textoIzq"" style=""padding:5px; width: 30%;"">
                                            " + retencionMaestro[0].Nit + @"
                                        </td>
                                        <td class=""textoDerecha negrita"" style=""width: 25%; "">
                                        
                                        </td>
                                        <td class=""textoIzq"" style=""padding:5px; width: 15%; "">
                                        
                                        </td>
                                    </tr>
                                </table>

                                <table class=""colorBlue"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:0px;width:100%;"">
                                    <tr style=""padding: 0px; margin: 0px;"">
                                        <td class="" textoCenter td negrita"" style=""width: 45%;padding: 10px 0;"">
                                            CONCEPTO
                                        </td>
                                        <td class="" textoCenter td negrita"" style=""width: 30%;padding: 10px 0;"">
                                            BASE RETENCIÓN
                                        </td>
                                        <td class="" textoCenter td negrita"" style=""width: 25%;padding: 10px 0;"">
                                            VALOR RETENIDO
                                        </td>
                                    </tr>
                                </table>

                                <table class=""table"" style=""border-collapse:collapse; border-spacing:0;"">
                                    " + MaperarCertificadoIcaDetallePdf(retencionDetalle) + @"
                                </table>      
                             
                                <table class=""table"" style=""border-collapse:collapse; border-spacing:0;"">    
                                    <tr style=""padding: 0px; margin: 0px;"">
                                        <td class=""total textoIzq"">                                       
                                            <b>TOTAL SON: </b>" + Funciones.NumeroALetras(total) + @" MCTE
                                        </td>
                                        <td class=""valorTotal textoCenter textoIzq"" style="""">
                                            " + totalConFormato + @"
                                        </td>
                                    </tr>
                                </table>                           
                             
                                <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:40px;"">    
                                    <tr style=""border-top:none;"" >
                                        <td class=""textoIzq "" style=""width: 100%; padding-top: 5px; font-size:10px"">
                                            Este certificado se expide sin firma autografa de conformidad con lo dispuesto en el articulo 10 del D.R 836 de 1991
                                        </td>
                                    </tr>
                                </table>

                                <table class=""table"" style=""border-collapse:collapse; border-spacing:0;"">    
                                    <tr style=""padding: 0px; margin: 0px;"">
                                        <td style=""text-align: justify; font-size:10px"">
                                            Según el decreto 836 de 1991, capítulo IV artículo 10; las personas jurídicas podrán entregar los certificados de retención en la fuente, en forma
                                            continua impreso por computador, sin necesidad de firma autógrafa y el decreto 380 de 1996, artículo 7 parágrafo; para efectos de la expedición
                                            del certificado al que se refiere este artículo, los agentes de retención podrán elaborarlo en formas continuas impresas por computador, sin
                                            necesidad de firma autógrafa.
                                        </td>
                                    </tr>
                                </table>

                                <table class=""textFinal"" style=""border-collapse:collapse; border-spacing:0;"">    
                                    <tr style=""border-top:none;"" >
                                        <td class=""textoCenter "" style=""width: 100%;"">
                                            " + qrCodeHtml + @"
                                        </td>
                                    </tr>
                                </table>
                            </body>
                        </html>";

                        htmlConcatenado += html;

                        string nombreArchivo = Funciones.PdfSharpConvertReturnedName(html, _pathOptions.Path_FileServer_TempFiles, empresaGestion.Id.ToString());
                        ListadoArchivosTemporales.Add(nombreArchivo);
                    }
                }

                byte[] res = Array.Empty<byte>();

                if (ListadoArchivosTemporales.Count > 1)
                {
                    // Combinamos los archivos
                    res = Funciones.GenerateArchivoPdfCombinado(ListadoArchivosTemporales, _pathOptions.Path_FileServer_TempFiles);
                }
                else
                {
                    res = Funciones.PdfSharpConvertWithoutCreateFile(html);
                }

                // Borramos archivos temporales
                BorrarArchivosTemp();

                return (htmlConcatenado, res);
            }
            catch (Exception e)
            {
                // Borramos archivos temporales
                BorrarArchivosTemp();
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<string> CertificadoRetencionIcaPdf(QuerySearchCertificados parameters)
        {
            try
            {
                // Consultamos data de la empresa
                List<Empresa> emp = _empresasService.EmpresasActivas;
                var empresaGestion = emp.Find(x => x.Id == parameters.IdEmpresa);
                parameters.Empresa = empresaGestion.EmpAbreviatura;

                QuerySearchCertificadosICA query = new QuerySearchCertificadosICA()
                {
                    Empresa = parameters.Empresa,
                    Month = parameters.Month,
                    NitProveedor = parameters.NitProveedor,
                    Year = parameters.Year,
                    IsBimensual = empresaGestion.Emp_IsBimestralApot,
                    Cuenta = empresaGestion.Emp_CuentaApot
                };

                string periodoCertificado = string.Empty;

                if (empresaGestion.Emp_IsBimestralApot)
                {
                    periodoCertificado = $"{Funciones.FormatearMes(parameters.Month)}/{parameters.Year} - {Funciones.FormatearMes(parameters.Month + 1)}/{parameters.Year}";
                }
                else
                {
                    periodoCertificado = $"{Funciones.FormatearMes(parameters.Month)}/{parameters.Year}";
                }

                // Consultamos datos de la factura
                List<CertificadoRetencionMaestro> retencionMaestro = await _unitOfWork.ApoteosysRepository.CertificadosRetencionApoteosys(parameters);
                List<CertificadoRetencionIcaDte> retencionDetalle = await _unitOfWork.ApoteosysRepository.CertificadosRetencionIcaApoteosys(query);

                string html = string.Empty;

                if (retencionDetalle.Count > 0)
                {
                    // Creamos el QR de validación y le anexamos la información con la Url y el token
                    string qrCodeHtml = Funciones.GetQrCodeHtml(parameters.UrlToken);

                    string totalConFormato = retencionDetalle.Find(x => x.Total == "1").Credito;
                    decimal total = decimal.Parse(retencionDetalle.Find(x => x.Total == "1").CreditoSinFormato);
                    retencionDetalle = retencionDetalle.FindAll(x => x.Total == "0");

                    // Armamos el HTML
                    html =
                        @"<html>
                    <head>
                        <style>
                            .colorBlue {
                                background-color: #034F8A;
                                color: white;
                            }
                            .colorGreen {
                                background-color: #46a13a;
                                color: white;
                            }
                            .table {
                                width: 100%;
                            }
                            .td {
                                font-family: Arial, sans-serif;
                                font-size: 12px;
                                padding: 2px 2px;
                                word-break: normal;
                                text-align: center;     
                            }
                            .textoCenter {
                                text-align: center;
                            }
                            .textoDerecha {
                                text-align: right;
                            }
                            .textoIzq {
                                text-align: left;
                            }
                            .valorTotal {
                                width: 15%;
                                font-weight: bold;
                                font-size: 13px;
                                padding:10px;
                                border-top:1px solid black;
                                background-color: #D2D5E3;
                            }
                            .total {
                                padding-left: 10px;
                                padding-top: 10px;
                                padding-right: 15px;
                                padding-bottom: 10px;
                                border-top:1px solid black; 
                                width: 85%; 
                                font-size:10px; 
                                background-color: #D2D5E3;
                            }
                            .negrita{
                                 font-weight: bold;
                            }
                            .titlePrincipal{
                                font-weight: bold;
                                font-family: Arial, sans-serif;
                                font-size: 17px;
                                word-break: normal;
                                text-align: center;  
                            }
                            .textFinal{
                                width: 100%;
                                margin-top:20px;    
                            }
                            .twoContenedor{
                                padding-left: 10px;
                                padding-top: 0px;
                                padding-right: 5px;
                                padding-bottom: 0px;
                            }
                        </style>
                    </head>

                    <body>
                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;"">
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""negrita"" style=""padding-left: 10px; width: 57%;"">
                                        CERTIFICADO DE RETENCIÓN ICA
                                </td>
                                <td class=""textoDerecha"" style=""width: 43%;"" rowspan=""2"">
                                        <img src=""" + empresaGestion.CodArchivo + @""" width=""200px"" height=""50px"" >
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""negrita"" style=""padding-left: 10px; width: 57%;"">
                                        " + empresaGestion.Emp_NombreEmpresaGnr + @"
                                </td>
                                <td class="""" style=""width: 43%;"">
                                          
                                </td>
                            </tr>
                        </table>
                          
                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:10px;"">
                            <tr>
                                <td class=""twoContenedor"" style=""padding-bottom:7px; padding-top-10px;width: 40%;"">                                       
                                    " + empresaGestion.EmpDireccion + @"
                                </td>
                                <td class=""twoContenedor textoIzq"" style=""width: 60%;"">
                                    
                                </td>
                            </tr>
                            <tr>
                                <td class=""twoContenedor"" style=""padding-bottom:7px; padding-top-10px;width: 40%;"">
                                    <b>NIT:</b> " + empresaGestion.EmpNit + @"
                                </td>
                            </tr>
                        </table>
                           
                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:10px;border:1px solid black;"">    
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoIzq negrita"" style=""width: 30%; padding-left: 15px;"">                                       
                                    Año gravable
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 30%; "">
                                    " + parameters.Year + @"
                                </td>
                                <td class=""textoDerecha negrita"" style=""width: 25%; "">
                                    Fecha de Expedición
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 15%; "">
                                    " + DateTime.Now.ToString("dd/MM/yyyy") + @"
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoIzq negrita"" style=""width: 30%; padding-left: 15px;"">                                       
                                    Periodo
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 30%; "">
                                    " + periodoCertificado + @"
                                </td>
                                <td class=""textoIzq negrita"" style=""width: 25%; "">
                                        
                                </td>
                                <td class=""textoIzq negrita"" style=""padding:5px; width: 15%; "">
                                        
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoIzq negrita"" style=""width: 30%; padding-left: 15px;"">                                       
                                    Retención efectuada a
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 30%; "">
                                    " + retencionMaestro[0].Razon_Social + @"
                                </td>
                                <td class=""textoIzq negrita"" style=""width: 25%; "">
                                        
                                </td>
                                <td class=""textoIzq negrita"" style=""padding:5px; width: 15%; "">
                                        
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoIzq negrita"" style=""width: 30%; padding-left: 15px;"">                                       
                                    NIT
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 30%;"">
                                    " + retencionMaestro[0].Nit + @"
                                </td>
                                <td class=""textoDerecha negrita"" style=""width: 25%; "">
                                        
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 15%; "">
                                        
                                </td>
                            </tr>
                        </table>

                        <table class=""colorBlue"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:0px;width:100%;"">
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class="" textoCenter td negrita"" style=""width: 35%;padding: 10px 0;"">
                                    CONCEPTO
                                </td>
                                <td class="" textoCenter td negrita"" style=""width: 20%;padding: 10px 0;"">
                                    TARIFA POR MIL
                                </td>
                                <td class="" textoCenter td negrita"" style=""width: 25%;padding: 10px 0;"">
                                    BASE RETENCIÓN
                                </td>
                                <td class="" textoCenter td negrita"" style=""width: 20%;padding: 10px 0;"">
                                    VALOR RETENIDO
                                </td>
                            </tr>
                        </table>

                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;"">
                            " + MaperarCertificadoIcaDetallePdf(retencionDetalle) + @"
                        </table>      
                             
                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;"">    
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""total textoIzq"">                                       
                                    <b>TOTAL SON: </b>" + Funciones.NumeroALetras(total) + @" MCTE
                                </td>
                                <td class=""valorTotal textoCenter textoIzq"" style="""">
                                    " + totalConFormato + @"
                                </td>
                            </tr>
                        </table>                           
                             
                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:40px;"">    
                            <tr style=""border-top:none;"" >
                                <td class=""textoIzq "" style=""width: 100%; padding-top: 5px; font-size:10px"">
                                    Este certificado se expide sin firma autografa de conformidad con lo dispuesto en el articulo 10 del D.R 836 de 1991
                                </td>
                            </tr>
                        </table>

                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;"">    
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td style=""text-align: justify; font-size:10px"">
                                    Según el decreto 836 de 1991, capítulo IV artículo 10; las personas jurídicas podrán entregar los certificados de retención en la fuente, en forma
                                    continua impreso por computador, sin necesidad de firma autógrafa y el decreto 380 de 1996, artículo 7 parágrafo; para efectos de la expedición
                                    del certificado al que se refiere este artículo, los agentes de retención podrán elaborarlo en formas continuas impresas por computador, sin
                                    necesidad de firma autógrafa.
                                </td>
                            </tr>
                        </table>

                        <table class=""textFinal"" style=""border-collapse:collapse; border-spacing:0;"">    
                            <tr style=""border-top:none;"" >
                                <td class=""textoCenter "" style=""width: 100%;"">
                                    " + qrCodeHtml + @"
                                </td>
                            </tr>
                        </table>
                    </body>
                </html>";
                }
                else
                {
                    html = "<html>No se encuentraron datos para la consulta realizada<html>";
                }

                return html;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<string> CertificadoRetencionEstampillaBoyacaPdf(QuerySearchCertificados parameters)
        {
            try
            {
                string periodoCertificado = parameters.Month == 0 ? $"01/{parameters.Year} - 12/{parameters.Year}" : $"{Funciones.FormatearMes(parameters.Month)}/{parameters.Year}";

                // Consultamos datos de la factura
                List<CertificadoRetencionMaestro> retencionMaestro = await _unitOfWork.ApoteosysRepository.CertificadosRetencionApoteosys(parameters);
                List<CertificadoRetencionEstampillaBoyacaDte> retencionDetalle = await _unitOfWork.ApoteosysRepository.CertificadosRetencionEstampillaBoyacaApoteosys(parameters);

                // Consultamos data de la empresa
                List<Empresa> emp = _empresasService.EmpresasActivas;
                var empresaGestion = emp.Find(x => x.EmpAbreviatura == parameters.Empresa);

                string html = string.Empty;

                if (retencionDetalle.Count > 0)
                {
                    // Creamos el QR de validación y le anexamos la información con la Url y el token
                    string qrCodeHtml = Funciones.GetQrCodeHtml(parameters.UrlToken);

                    string totalConFormato = retencionDetalle.Find(x => x.Total == "1").Credito;
                    decimal total = decimal.Parse(retencionDetalle.Find(x => x.Total == "1").CreditoSinFormato);
                    retencionDetalle = retencionDetalle.FindAll(x => x.Total == "0");

                    // Armamos el HTML
                    html =
                        @"<html>
                    <head>
                        <style>
                            .colorBlue {
                                background-color: #034F8A;
                                color: white;
                            }
                            .colorGreen {
                                background-color: #46a13a;
                                color: white;
                            }
                            .table {
                                width: 100%;
                            }
                            .td {
                                font-family: Arial, sans-serif;
                                font-size: 12px;
                                padding: 2px 2px;
                                word-break: normal;
                                text-align: center;     
                            }
                            .textoCenter {
                                text-align: center;
                            }
                            .textoDerecha {
                                text-align: right;
                            }
                            .textoIzq {
                                text-align: left;
                            }
                            .valorTotal {
                                width: 15%;
                                font-weight: bold;
                                font-size: 13px;
                                padding:10px;
                                border-top:1px solid black;
                                background-color: #D2D5E3;
                            }
                            .total {
                                padding-left: 10px;
                                padding-top: 10px;
                                padding-right: 15px;
                                padding-bottom: 10px;
                                border-top:1px solid black; 
                                width: 85%; 
                                font-size:10px; 
                                background-color: #D2D5E3;
                            }
                            .negrita{
                                 font-weight: bold;
                            }
                            .titlePrincipal{
                                font-weight: bold;
                                font-family: Arial, sans-serif;
                                font-size: 17px;
                                word-break: normal;
                                text-align: center;  
                            }
                            .textFinal{
                                width: 100%;
                                margin-top:20px;    
                            }
                            .twoContenedor{
                                padding-left: 10px;
                                padding-top: 0px;
                                padding-right: 5px;
                                padding-bottom: 0px;
                            }
                        </style>
                    </head>

                    <body>
                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;"">
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""negrita"" style=""padding-left: 10px; width: 57%;"">
                                        CERTIFICADO DE RETENCIÓN ESTAMPILLA BOYACÁ
                                </td>
                                <td class=""textoDerecha"" style=""width: 43%;"" rowspan=""2"">
                                        <img src=""" + empresaGestion.CodArchivo + @""" width=""200px"" height=""50px"" >
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""negrita"" style=""padding-left: 10px; width: 57%;"">
                                        " + empresaGestion.Emp_NombreEmpresaGnr + @"
                                </td>
                                <td class="""" style=""width: 43%;"">
                                          
                                </td>
                            </tr>
                        </table>
                          
                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:10px;"">
                            <tr>
                                <td class=""twoContenedor"" style=""padding-bottom:7px; padding-top-10px;width: 40%;"">                                       
                                    " + empresaGestion.EmpDireccion + @"
                                </td>
                                <td class=""twoContenedor textoIzq"" style=""width: 60%;"">
                                    
                                </td>
                            </tr>
                            <tr>
                                <td class=""twoContenedor"" style=""padding-bottom:7px; padding-top-10px;width: 40%;"">
                                    <b>NIT:</b> " + empresaGestion.EmpNit + @"
                                </td>
                            </tr>
                        </table>
                           
                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:10px;border:1px solid black;"">    
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoIzq negrita"" style=""width: 30%; padding-left: 15px;"">                                       
                                    Año gravable
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 30%; "">
                                    " + parameters.Year + @"
                                </td>
                                <td class=""textoDerecha negrita"" style=""width: 25%; "">
                                    Fecha de Expedición
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 15%; "">
                                    " + DateTime.Now.ToString("dd/MM/yyyy") + @"
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoIzq negrita"" style=""width: 30%; padding-left: 15px;"">                                       
                                    Periodo
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 30%; "">
                                    " + periodoCertificado + @"
                                </td>
                                <td class=""textoIzq negrita"" style=""width: 25%; "">
                                        
                                </td>
                                <td class=""textoIzq negrita"" style=""padding:5px; width: 15%; "">
                                        
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoIzq negrita"" style=""width: 30%; padding-left: 15px;"">                                       
                                    Retención efectuada a
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 30%; "">
                                    " + retencionMaestro[0].Razon_Social + @"
                                </td>
                                <td class=""textoIzq negrita"" style=""width: 25%; "">
                                        
                                </td>
                                <td class=""textoIzq negrita"" style=""padding:5px; width: 15%; "">
                                        
                                </td>
                            </tr>
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""textoIzq negrita"" style=""width: 30%; padding-left: 15px;"">                                       
                                    NIT
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 30%;"">
                                    " + retencionMaestro[0].Nit + @"
                                </td>
                                <td class=""textoDerecha negrita"" style=""width: 25%; "">
                                        
                                </td>
                                <td class=""textoIzq"" style=""padding:5px; width: 15%; "">
                                        
                                </td>
                            </tr>
                        </table>

                        <table class=""colorBlue"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:0px;width:100%;"">
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class="" textoCenter td negrita"" style=""width: 40%;padding: 10px 0;"">
                                    CONCEPTO
                                </td>
                                <td class="" textoCenter td negrita"" style=""width: 30%;padding: 10px 0;"">
                                    BASE RETENCIÓN
                                </td>
                                <td class="" textoCenter td negrita"" style=""width: 30%;padding: 10px 0;"">
                                    VALOR RETENIDO
                                </td>
                            </tr>
                        </table>

                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;"">
                            " + MaperarCertificadoEstampillaBoyacaDetallePdf(retencionDetalle) + @"
                        </table>      
                             
                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;"">    
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td class=""total textoIzq"">                                       
                                    <b>TOTAL SON: </b>" + Funciones.NumeroALetras(total) + @" MCTE
                                </td>
                                <td class=""valorTotal textoCenter textoIzq"" style="""">
                                    " + totalConFormato + @"
                                </td>
                            </tr>
                        </table>                           
                             
                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:40px;"">    
                            <tr style=""border-top:none;"" >
                                <td class=""textoIzq "" style=""width: 100%; padding-top: 5px; font-size:10px"">
                                    Este certificado se expide sin firma autografa de conformidad con lo dispuesto en el articulo 10 del D.R 836 de 1991
                                </td>
                            </tr>
                        </table>

                        <table class=""table"" style=""border-collapse:collapse; border-spacing:0;"">    
                            <tr style=""padding: 0px; margin: 0px;"">
                                <td style=""text-align: justify; font-size:10px"">
                                    Según el decreto 836 de 1991, capítulo IV artículo 10; las personas jurídicas podrán entregar los certificados de retención en la fuente, en forma
                                    continua impreso por computador, sin necesidad de firma autógrafa y el decreto 380 de 1996, artículo 7 parágrafo; para efectos de la expedición
                                    del certificado al que se refiere este artículo, los agentes de retención podrán elaborarlo en formas continuas impresas por computador, sin
                                    necesidad de firma autógrafa.
                                </td>
                            </tr>
                        </table>

                        <table class=""textFinal"" style=""border-collapse:collapse; border-spacing:0;"">    
                            <tr style=""border-top:none;"" >
                                <td class=""textoCenter "" style=""width: 100%;"">
                                    " + qrCodeHtml + @"
                                </td>
                            </tr>
                        </table>
                    </body>
                </html>";
                }
                else
                {
                    html = "<html>No se encuentraron datos para la consulta realizada<html>";
                }

                return html;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public static string MaperarFacturaDetallePdf(List<EstadoCuentasXPagarDetalle> detalles)
        {
            try
            {
                string html = "";

                foreach (EstadoCuentasXPagarDetalle item in detalles)
                {
                    string obsIva = string.Empty;
                    string obs = string.Empty;

                    // Obs
                    if (string.IsNullOrEmpty(item.Base_Retencion))
                    {
                        obs = @"" + item.Nombre_Cuenta_Contable +
                            //@" <br>" + 
                            //item.Observaciones + 
                            @"";

                    }
                    else if (!item.Base_Retencion.Equals("$.00"))
                    {
                        obs = @"" + item.Nombre_Cuenta_Contable + @" <br> Base retención: " + item.Base_Retencion +
                            //@" <br>" + 
                            //item.Observaciones + 
                            @"";
                    }
                    else
                    {
                        obs = @"" + item.Nombre_Cuenta_Contable +
                            //@" <br>" + 
                            //item.Observaciones + 
                            @"";
                    }

                    // ObsIva
                    /*if (!item.Iva_Mayor_Valor.Equals("$.00"))
                    {
                        obsIva = @"" + item.Nombre_Cuenta_Contable + @" <br> IVA mayor valor: " + item.Iva_Mayor_Valor + 
                            //@" <br>" + 
                            //item.Observaciones + 
                            @"";
                    }
                    else //if(string.IsNullOrEmpty(item.Iva_Mayor_Valor))
                    {
                        obsIva = @"" + item.Nombre_Cuenta_Contable + 
                            //@" <br>" + 
                            //item.Observaciones + 
                            @"";
                    }*/

                    html += @"<tr style=""padding: 0px; margin: 0px;"">
                            <td class=""textoIzq td"" style=""border: 1px solid black; border-top: none; padding: 20px 10px; width: 60%"">
                                " + obs + @"
                                " + obsIva + @"
                            </td>
                            <td class="" textoDerecha td"" style=""border: 1px solid black; border-top: none; padding: 20px 10px; width: 40%"">
                                " + item.Credito + @"
                            </td>
                        </tr>";
                }

                return html;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public static string MaperarFacturaPagadasDetallePdf(int tipo, List<FacturasPagas> facturasPagadasDte = null, List<FacturasPagas> retencionesFactPagas = null)
        {
            try
            {
                string html = "";

                switch (tipo)
                {
                    case 1: // Pagos
                        if (facturasPagadasDte.Count > 0)
                        {
                            int i = 1;
                            foreach (FacturasPagas item in facturasPagadasDte)
                            {
                                if (i == 1)
                                {
                                    if (facturasPagadasDte.Count >= 2)
                                    {
                                        html += @"<tr style=""padding: 0px; margin: 0px;"">
                                            <td class=""textoCenter td"" style=""border: 1px solid black; width: 20%; border-bottom: none; border-top:none; padding:8px;"" rowspan=""" + facturasPagadasDte.Count + @""">
                                                " + item.Fecha + @"
                                            </td>
                                            <td class=""textoCenter td"" style=""border: 1px solid black; width: 20%; border-bottom: none; border-top:none; padding:8px;"">
                                                " + item.Nombre_Cuenta + @"
                                            </td>
                                            <td class=""textoDerecha td"" style=""border: 1px solid black; width: 30%; border-bottom: none; border-top:none; padding:8px;"">
                                                " + item.Debito + @"
                                            </td>
                                            <td class=""textoDerecha td"" style=""border: 1px solid black; width: 30%; border-bottom: none; border-top:none; padding:8px;"" rowspan=""" + facturasPagadasDte.Count + @""">
                                                " + item.Total_Movimiento + @"
                                            </td>
                                        </tr>";
                                    }
                                    else
                                    {
                                        html += @"<tr style=""padding: 0px; margin: 0px;"">
                                            <td class=""textoCenter td"" style=""border: 1px solid black; width: 20%; border-bottom: none; border-top:none; padding:8px;"">
                                                " + item.Fecha + @"
                                            </td>
                                            <td class=""textoCenter td"" style=""border: 1px solid black; width: 20%; border-bottom: none; border-top:none; padding:8px;"">
                                                " + item.Nombre_Cuenta + @"
                                            </td>
                                            <td class=""textoDerecha td"" style=""border: 1px solid black; width: 30%; border-bottom: none; border-top:none; padding:8px;"">
                                                " + item.Debito + @"
                                            </td>
                                            <td class=""textoDerecha td"" style=""border: 1px solid black; width: 30%; border-bottom: none; border-top:none; padding:8px;"">
                                                " + item.Total_Movimiento + @"
                                            </td>
                                        </tr>";
                                    }

                                    i++;
                                }
                                else
                                {
                                    html += @"<tr style=""padding: 0px; margin: 0px;"">
                                            <td class=""textoCenter td"" style=""border: 1px solid black; width: 20%; border-bottom: none; border-top:none; padding:8px;"">
                                                " + item.Nombre_Cuenta + @"
                                            </td>
                                            <td class=""textoDerecha td"" style=""border: 1px solid black; width: 30%; border-bottom: none; border-top:none; padding:8px;"">
                                                " + item.Debito + @"
                                            </td>
                                        </tr>";
                                }
                            }
                        }

                        break;
                    case 2: // Retenciones
                        foreach (FacturasPagas item in retencionesFactPagas)
                        {
                            string obs = string.Empty;

                            // Obs
                            if (string.IsNullOrEmpty(item.Base_Retencion))
                            {
                                obs = @"" + item.Nombre_Cuenta_Con +
                                    //@" <br>" + 
                                    //item.Observaciones + 
                                    @"";

                            }
                            else if (!item.Base_Retencion.Equals("$.00"))
                            {
                                obs = @"" + item.Nombre_Cuenta_Con + @" <br> Base retención: " + item.Base_Retencion +
                                    //@" <br>" + 
                                    //item.Observaciones + 
                                    @"";
                            }
                            else
                            {
                                obs = @"" + item.Nombre_Cuenta_Con +
                                    //@" <br>" + 
                                    //item.Observaciones + 
                                    @"";
                            }

                            html += @"<tr style=""padding: 0px; margin: 0px;"">
                            <td class=""textoCenter td"" style=""border: 1px solid black; width: 30%; border-bottom: none; border-top:none; padding:8px;"">
                                " + item.Nombre_Cuenta + @"
                            </td>
                            <td class=""textoCenter td"" style=""border: 1px solid black; width: 40%; border-bottom: none; border-top:none; padding:8px;"">
                                " + obs + @"
                            </td>
                            <td class=""textoDerecha td"" style=""border: 1px solid black; width: 30%; border-bottom: none; border-top:none; padding:8px;"">
                                " + item.Credito + @"
                            </td>
                        </tr>";
                        }
                        break;
                    default:
                        break;
                }

                return html;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public static string MaperarCertificadoFuenteDetallePdf(List<CertificadoRetencionFuenteDte> detalles)
        {
            try
            {
                string html = "";

                /*for (int i = 0; i < 50; i++)
                {
                    string obsIva = string.Empty;
                    string obs = string.Empty;

                    html += @"<tr style=""padding: 0px; margin: 0px;"">
                                <td class="" textoCenter td"" style=""border: 1px solid black; width: 35%; border-bottom: none; border-top:none; padding:8px;"">
                                    prueba
                                </td>
                                <td class="" textoCenter td"" style=""border: 1px solid black; width: 20%; border-bottom: none; border-top:none; padding:8px; "">
                                    fsdfsdfsd
                                </td>
                                <td class="" td"" style=""border: 1px solid black; width: 20%; border-bottom: none; border-top:none; text-align: right; padding:8px; "">
                                    sdfdsfsdf 
                                </td>
                                <td class="" td"" style=""border: 1px solid black; width: 25%; border-bottom: none; border-top:none; text-align: right; padding:8px; "">
                                    sdfsdfsdfsd
                                </td>
                            </tr>";
                }*/

                foreach (CertificadoRetencionFuenteDte item in detalles)
                {
                    string obsIva = string.Empty;
                    string obs = string.Empty;

                    html += @"<tr style=""padding: 0px; margin: 0px;"">
                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 40%; border-bottom: none; border-top:none; padding:8px;"">
                                " + item.Nombre_Cuenta + @"
                            </td>
                            <td class="" td"" style=""border: 1px solid black; width: 30%; border-bottom: none; border-top:none; text-align: right; padding:8px; "">
                                " + item.Base_Retencion + @" 
                            </td>
                            <td class="" td"" style=""border: 1px solid black; width: 30%; border-bottom: none; border-top:none; text-align: right; padding:8px; "">
                                " + item.Credito + @"
                            </td>
                        </tr>";
                }

                return html;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public static string MaperarCertificadoIvaDetallePdf(List<CertificadoRetencionIvaDte> detalles)
        {
            try
            {
                string html = "";

                foreach (CertificadoRetencionIvaDte item in detalles)
                {
                    string obsIva = string.Empty;
                    string obs = string.Empty;

                    html += @"<tr style=""padding: 0px; margin: 0px;"">
                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 35%; border-bottom: none; border-top:none; padding:8px;"">
                                " + item.Nombre_Cuenta + @"
                            </td>
                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 15%; border-bottom: none; border-top:none; padding:8px; "">
                                " + item.Porcentaje_Practicado + @"
                            </td>
                            <td class="" td"" style=""border: 1px solid black; width: 15%; border-bottom: none; border-top:none; text-align: right; padding:8px; "">
                                " + item.Base_Origen + @" 
                            </td>
                            <td class="" td"" style=""border: 1px solid black; width: 15%; border-bottom: none; border-top:none; text-align: right; padding:8px; "">
                                " + item.Base_Retencion + @" 
                            </td>
                            <td class="" td"" style=""border: 1px solid black; width: 20%; border-bottom: none; border-top:none; text-align: right; padding:8px; "">
                                " + item.Credito + @"
                            </td>
                        </tr>";
                }

                return html;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public static string MaperarCertificadoIcaDetallePdf(List<CertificadoRetencionIcaDte> detalles)
        {
            try
            {
                string html = "";

                foreach (CertificadoRetencionIcaDte item in detalles)
                {
                    string obsIva = string.Empty;
                    string obs = string.Empty;

                    html += @"<tr style=""padding: 0px; margin: 0px;"">
                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 45%; border-bottom: none; border-top:none; padding:8px;"">
                                " + item.Nombre_Cuenta + @"
                            </td>
                            <td class="" td"" style=""border: 1px solid black; width: 30%; border-bottom: none; border-top:none; text-align: right; padding:8px; "">
                                " + item.Base_Retencion + @" 
                            </td>
                            <td class="" td"" style=""border: 1px solid black; width: 25%; border-bottom: none; border-top:none; text-align: right; padding:8px; "">
                                " + item.Credito + @"
                            </td>
                        </tr>";
                }

                return html;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public static string MaperarCertificadoEstampillaBoyacaDetallePdf(List<CertificadoRetencionEstampillaBoyacaDte> detalles)
        {
            try
            {
                string html = "";

                foreach (CertificadoRetencionEstampillaBoyacaDte item in detalles)
                {
                    string obsIva = string.Empty;
                    string obs = string.Empty;

                    html += @"<tr style=""padding: 0px; margin: 0px;"">
                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 40%; border-bottom: none; border-top:none; padding:8px;"">
                                " + item.Nombre_Cuenta + @"
                            </td>
                            <td class="" td"" style=""border: 1px solid black; width: 30%; border-bottom: none; border-top:none; text-align: right; padding:8px; "">
                                " + item.Base_Retencion + @" 
                            </td>
                            <td class="" td"" style=""border: 1px solid black; width: 30%; border-bottom: none; border-top:none; text-align: right; padding:8px; "">
                                " + item.Credito + @"
                            </td>
                        </tr>";
                }

                return html;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        #endregion
        public async Task<MemoryStream> Prueba()
        {
            try
            {
                string html = @"<html>
                    <head>
                        <style>
                            .colorBlue {
                                background-color: #273580;
                                color: white;
                            }

                            .colorGreen {
                                background-color: #46a13a;
                                color: white;
                            }
        
                            .textoBlue{
                                color: #103588;
                            }

                            .table {
                                width: 100%;
                            }

                            .td {
                                font-family: Arial, sans-serif;
                                font-size: 12px;
                                padding: 2px 2px;
                                word-break: normal;
                            }

                            .negrita{
                                 font-weight: bold;
                            }

                            .textoCenter {
                                text-align: center;
                            }

                            .textoDerecha {
                                text-align: right;
                            }

                            .textoIzq {
                                text-align: left;
                            }

                            .textoJustificado {
                                text-align: justify;
                            }
                        </style>
                    </head>

                    <body>
                        <table class=""table"" style="" border-collapse:collapse; border-spacing:0; "">
                            <tr style="" padding: 0px; margin: 0px; "">
                                <td class=""textoCenter"" style=""padding:10px;width: 100%;"">
                                    <h1>PRUEBA PAG 1</h1>
                                </td>
                            </tr>
                        </table>
                    </body>

                    </html>";
                // Creamos el stream del pdf
                MemoryStream ms = new MemoryStream(Funciones.PdfSharpConvertWithoutCreateFilePrueba(html));
                return ms;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        //public async Task<byte[]> ActaOsPdf()
        //{
        //    try
        //    {
        //        // HTML para el PDF (personaliza según tus necesidades)
        //        string html = @"
        //        <html>
        //            <head>
        //                <title>Acta OS PDF</title>
        //            </head>
        //            <body>
        //                <h1>Acta OS</h1>
        //                <p>Contenido del acta OS...</p>
        //            </body>
        //        </html>";

        //        // Convierte HTML a PDF
        //        var doc = new HtmlToPdfDocument()
        //        {
        //            GlobalSettings = {
        //            ColorMode = ColorMode.Color,
        //            Orientation = Orientation.Portrait,
        //            PaperSize = PaperKind.A4,
        //        },
        //            Objects = {
        //            new ObjectSettings() {
        //                HtmlContent = html
        //            }
        //        }
        //        };

        //        // Genera el PDF
        //        byte[] pdfBytes = _pdfConverter.Convert(doc);

        //        return pdfBytes;
        //    }
        //    catch (Exception e)
        //    {
        //        // Manejar errores (personaliza según tus necesidades)
        //        throw new BusinessException($"Error al generar el PDF: {e.Message}");
        //    }
        //}

        public void BorrarArchivosTemp()
        {
            try
            {
                // Borramos archivos temporales
                foreach (string file in ListadoArchivosTemporales)
                {
                    string path = Path.Combine(_pathOptions.Path_FileServer_TempFiles, file);
                    //////_filesProcess.RemoveFile(path);
                }
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

    }
}
