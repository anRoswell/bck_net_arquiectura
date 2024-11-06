using Core.Entities;
using Core.Tools;
using System.Collections.Generic;
using System.IO;

namespace Core.ModelProcess
{
    public static class ProveedoresProcess
    {
        public static string maperarSociosPdf(List<PrvSocio> socios)
        {
            string html = "";
            foreach (PrvSocio item in socios)
            {
                html += @"

                                <table class="""" style=""border-collapse:collapse; border-spacing:0; "">
                                                        <tr style = ""padding: 0px; margin: 0px;"" > 
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 43%"">              
                                                                 " + item.SocNombre + @"             
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 19%"">
                                                                " + item.IdentSocCompleta + @"
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 18.1%"" >              
                                                                " + item.NombreCiudad + @"
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black;width: 19.9%"">              
                                                                " + item.SocDireccion + @"              
                                                            </td>
                                                        </tr>
                                                </table>";
            }

            return html;
        }

        public static string maperarReferenciasPdf(List<PrvReferencia> referencias)
        {
            string html = "";
            foreach (PrvReferencia item in referencias)
            {
                html += @"                      <table  style=""border-collapse:collapse; border-spacing:0; "">
                                                    <tr style = ""padding: 0px; margin: 0px;"" > 
                                                        <td class="" textoCenter td"" style=""border: 1px solid black; width: 33%"">              
                                                                " + item.RefEmpresa + @"            
                                                        </td>
                                                        <td class="" textoCenter td"" style=""border: 1px solid black; width: 33%"">
                                                            " + item.RefTelefono + @"
                                                        </td>
                                                        <td class="" textoCenter td"" style=""border: 1px solid black; width: 33%"" >              
                                                            " + item.RefContacto + @"
                                                        </td>
                                                           
                                                    </tr>
                                            </table>";
            }

            return html;
        }

        public static string maperarEmpresasPdf(List<PrvEmpresasSelected> empresas)
        {
            string html = "";
            foreach (PrvEmpresasSelected item in empresas)
            {
                html += @"<div class=""td"">- " + item.NombreEmpresa + @" </div></br> ";
            }

            return html;
        }

        public static string maperarProdServPdf(List<PrvProdServSelected> prodservs)
        {
            string html = "";
            foreach (PrvProdServSelected item in prodservs)
            {
                html += @"<div  class=""td"">- " + item.NombreProducto + @" </div></br> ";
            }

            return html;
        }

        public static string generarPdfHojadeVida(Proveedores proveedores, List<PrvSocio> socios, List<PrvReferencia> referencias,
            List<PrvEmpresasSelected> empresas, List<PrvProdServSelected> prodservs,string pathFs, string id,  string nombreante="")
        {
            List<string> listadoArchivos = new List<string>();
            string html = ProveedoresProcess.MapearPdf(proveedores, socios, referencias, empresas, prodservs);
            string url = Funciones.PdfSharpConvert(html.Replace("displayfirma", nombreante==""? "display:none": "display:block"), pathFs,id);
            listadoArchivos.Add(url);
            //string htmldecla = ProveedoresProcess.MapearPdfDeclaracion(proveedores);
            //string urlDeclaracion = Funciones.PdfSharpConvert(htmldecla.Replace("displayfirma", string.IsNullOrWhiteSpace(nombreante) ? "display:none" : "display:block"), pathFs, id);
            //listadoArchivos.Add(urlDeclaracion);
            string urlFinal = Funciones.GenerateArchivoPdfCombinadoProveddor(listadoArchivos, pathFs, id, nombreante);
            foreach (string file in listadoArchivos)
            {
                string path = Path.Combine(pathFs, file);
                if (File.Exists(path))
                {
                    File.Delete(path); 
                }
            }
            return urlFinal;
        }

        
        public static string MapearPdf(
            Proveedores proveedores, List<PrvSocio> socios, List<PrvReferencia> referencias, 
            List<PrvEmpresasSelected> empresas, List<PrvProdServSelected> prodservs)
        {
            string html = @"
                            <html>
                                <head>
                                <style>

                            

                                    .colorBlue
                                        {
                                            background-color: #002844;
                                            color: white;
                                        }
                                    .colorGreen
                                        {
                                            background-color: #d6eefe;
                                            color: black;
                                        }
                                    .table
                                        {
                                            width: 100 %;
                                          
                                        }
                                    .td
                                        {

                                            font-family: Arial, sans-serif;
                                            font-size: 12px;
                                           
                                            padding: 2px 2px;
                                            word-break: normal;
                                        }
                                .textoCenter
                                        {
                                            text-align: center;
                                        }

                                .fecha
                                        {
                                            width: 15%;
                                        }
                                .nombre
                                        {
                                            width: 55%;
                                        }
                                .nit
                                        {
                                        width:30%;
                                        }
                                .pdfFormField {
                                        min-width: 370px !important
                                        }
                                </style>
                                </head>
                                <body>
                                            <div class=""table"">
                                              

                                                    <table class=""colorBlue"" style="" border-collapse:collapse; border-spacing:0; "">
                                                            <tr style = ""padding: 0px; margin: 0px;"" >
       
                                                               <td class=""textoCenter td"" style=""border: 1px solid black;width: 100%; "" >
                                                                I.DATOS DEL PROVEEDOR
                                                                </td>
                                                            </tr>
                                                    </table>

                                                    <table class=""colorGreen"" style=""border-collapse:collapse; border-spacing:0;"">
                                                            <tr style = ""padding: 0px; margin: 0px;"" >       
                                                               <td class="" fecha textoCenter td"" style=""border: 1px solid black;"">
                                                                    Fecha Envió
                                                               </td>
                                                               <td class=""nombre textoCenter td"" style=""border: 1px solid black;"">              
                                                                    Nombre o Razón Social de la empresa
                                                               </td>
                                                               <td class=""nit textoCenter td"" style=""border: 1px solid black;"">              
                                                                    No.documento regsitro de la emprea ante el gobierno del pais(NIT / CC)              
                                                               </td>
                                                            </tr>
                                                    </table>

                                             

                                            <table  style=""border-collapse:collapse; border-spacing:0; "">
                                                            <tr style = ""padding: 0px; margin: 0px;"" >
       
                                                               <td class="" fecha textoCenter td"" style=""border: 1px solid black; color: black;"">
                                                         " + proveedores.PrvFechaEnvio.ToString("yyyy/MM/dd") + @"
                                                        </td>
                                                        <td class=""nombre textoCenter td"" style=""border: 1px solid black; color: black;"">
              
                                                          " + proveedores.PrvNombreProveedor + @"

                                                        </td>
                                                        <td class=""nit textoCenter td"" style=""border: 1px solid black; color: black;"">
              
                                                           " + proveedores.IdentPrvCompleta + @"
              
                                                        </td>
                                                      </tr>
                                                </table>

                                              

                                                <table class=""colorGreen"" style=""border-collapse:collapse; border-spacing:0; "">
                                                        <tr style = ""padding: 0px; margin: 0px;"" > 
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 34%"">              
                                                                Dirección completa de la empresa              
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 24%"">
                                                                Departamento
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 22%"" >              
                                                                Ciudad
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black;width: 20%"">              
                                                                Teléfono              
                                                            </td>
                                                        </tr>
                                                </table>

                                                <table  style=""border-collapse:collapse; border-spacing:0; "">
                                                        <tr style = ""padding: 0px; margin: 0px;"" > 
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 34%; color: black;"">              
                                                               " + proveedores.PrvDireccion + @"             
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black;width: 24%; color: black;"">
                                                                " + proveedores.NombreDepartamento + @"
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 22%; color: black;"" >              
                                                                " + proveedores.NombreCiudad + @"
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 20%; color: black;"">              
                                                                " + proveedores.PrvTelefono + @"              
                                                            </td>
                                                        </tr>
                                                </table>

                                             

                                              

                                            <table  class=""colorGreen"" style="" border-collapse:collapse; border-spacing:0; "">
                                                        <tr style = "" padding: 0px; margin: 0px;"" > 
                                                            <td class=""  td"" style=""border: 1px solid black;width: 73.8%"">              
                                                                Persona contacto(Proveedor)              
                                                            </td>
                                                            <td class="" td"" style=""border: 1px solid black;width: 26.2%"">
                                                                Telefono Movil
                                                            </td>
                                                            
                                                        </tr>
                                                </table>

                                              

                                                <table  style=""border-collapse:collapse; border-spacing:0; "">
                                                        <tr style = ""padding: 0px; margin: 0px;"" > 
                                                            <td class=""  td"" style=""border: 1px solid black; width: 41.5%; color: black;"">              
                                                              " + proveedores.PrvContacto + @"            
                                                            </td>
                                                            <td class=""  td colorGreen"" style="" border: 1px solid black; width: 13%"">
                                                               Correo Electronico 1
                                                            </td>
                                                            <td class=""  td"" style=""border: 1px solid black; width: 19.3%; color: black;"" >              
                                                                " + proveedores.PrvMail + @"
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 26.2%"">              
                                                                            
                                                            </td>
                                                        </tr>
                                                </table>

                                                <table  style=""border-collapse:collapse; border-spacing:0; "">
                                                        <tr style = ""padding: 0px; margin: 0px;"" > 
                                                            <td class=""  td"" style=""border: 1px solid black; width: 41.5%"">              
                                                                        
                                                            </td>
                                                            <td class=""  td colorGreen"" style="" border: 1px solid black; width: 13%"">
                                                               Correo Electronico 2
                                                            </td>
                                                            <td class=""  td"" style=""border: 1px solid black; width: 19.3%"" >              
                                                              
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 26.2%"">              
                                                                            
                                                            </td>
                                                        </tr>
                                                </table>

                                              

                                            
                                                <table class=""colorBlue"" style="" border-collapse:collapse; border-spacing:0; "">
                                                            <tr style = ""padding: 0px; margin: 0px;"" >
       
                                                               <td class=""textoCenter td"" style=""border: 1px solid black;width: 100%; "" >
                                                                SOCCIOS / ACCIONISTAS
                                                                </td>
                                                            </tr>
                                                    </table>

                                              

                                            <table class=""colorGreen"" style=""border-collapse:collapse; border-spacing:0; "">
                                                        <tr style = ""padding: 0px; margin: 0px;"" > 
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 43%"">              
                                                                Nombre y Apellido             
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 19%"">
                                                                Identificación
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 18.1%"" >              
                                                                Ciudad
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black;width: 19.9%"">              
                                                                Dirección              
                                                            </td>
                                                        </tr>
                                                </table>

                                              
                                                    " + maperarSociosPdf(socios) + @"
                                              

                                             

                                                <table class=""colorBlue"" style="" border-collapse:collapse; border-spacing:0; "">
                                                        <tr style = ""padding: 0px; margin: 0px;"" >
       
                                                            <td class=""textoCenter td"" style=""border: 1px solid black;width: 100%; "" >
                                                            REPRESENTANTE LEGAL
                                                            </td>
                                                        </tr>
                                                </table>                                              

                                                <table class=""colorGreen"" style=""border-collapse:collapse; border-spacing:0; "">
                                                        <tr style = ""padding: 0px; margin: 0px;"" > 
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 30%"">              
                                                                Nombre y Apellido             
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 12%"">
                                                                Identificación
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 11.5%"" >              
                                                                Ciudad
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black;width: 17.5%"">              
                                                                Teléfono Movil              
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black;width: 29%"">              
                                                                Email             
                                                            </td>
                                                        </tr>
                                                </table>
                                             
                                                <table class="""" style=""border-collapse:collapse; border-spacing:0; "">
                                                        <tr style = ""padding: 0px; margin: 0px;"" > 
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 30%; color: black;"">              
                                                                " + proveedores.PrvRteLegalNombre + @"             
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 12%; color: black;"">
                                                                " + proveedores.IdentRteCompleta + @"
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 11.5%; color: black;"" >              
                                                                " + proveedores.RptNombreCiudad + @"
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black;width: 17.5%; color: black;"">              
                                                                " + proveedores.PrvRteLegalTelefonoMovil + @"            
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black;width: 29%; color: black;"">              
                                                                " + proveedores.PrvRteLegalEmail + @"             
                                                            </td>
                                                        </tr>
                                                </table>



                                                <table class=""colorBlue"" style="" border-collapse:collapse; border-spacing:0; "">
                                                        <tr style = ""padding: 0px; margin: 0px;"" >
       
                                                            <td class=""textoCenter td"" style=""border: 1px solid black;width: 100%; "" >
                                                            REVISOR FISCAL
                                                            </td>
                                                        </tr>
                                                </table>                                              

                                                <table class=""colorGreen"" style=""border-collapse:collapse; border-spacing:0; "">
                                                        <tr style = ""padding: 0px; margin: 0px;"" > 
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 30%"">              
                                                                Nombre y Apellido             
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 12%"">
                                                                Identificación
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 11.5%"" >              
                                                                Ciudad
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black;width: 17.5%"">              
                                                                Teléfono Movil              
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black;width: 29%"">              
                                                                Email             
                                                            </td>
                                                        </tr>
                                                </table>
                                             
                                                <table class="""" style=""border-collapse:collapse; border-spacing:0; "">
                                                        <tr style = ""padding: 0px; margin: 0px;"" > 
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 30%; color: black;"">              
                                                                " + proveedores.PrvRevFiscalNombre+ @"             
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 12%; color: black;"">
                                                                " + proveedores.IdentRevCompleta + @"
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 11.5%; color: black;"" >              
                                                                " + proveedores.RevNombreCiudad + @"
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black;width: 17.5%; color: black;"">              
                                                                " + proveedores.PrvRevFiscalTelefonoMovil + @"            
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black;width: 29%; color: black;"">              
                                                                " + proveedores.PrvRevFiscalEmail + @"             
                                                            </td>
                                                        </tr>
                                                </table>




                                                    <table class=""colorBlue"" style="" border-collapse:collapse; border-spacing:0; "">
                                                            <tr style = ""padding: 0px; margin: 0px;"" >
       
                                                               <td class=""textoCenter td"" style=""border: 1px solid black;width: 100%; "" >
                                                               II.DETALLES BANCARIOS
                                                                </td>
                                                            </tr>
                                                    </table>

                                              

                                             <table class=""colorGreen"" style=""border-collapse:collapse; border-spacing:0; "">
                                                        <tr style = ""padding: 0px; margin: 0px;"" > 
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 40%"">              
                                                                Banco            
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 15%"">
                                                                Número de Cuenta
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 45%"" >              
                                                                Tipo de Cuenta
                                                            </td>
                                                           
                                                        </tr>
                                                </table>

                                              
                                                <table class="""" style=""border-collapse:collapse; border-spacing:0; "">
                                                        <tr style = ""padding: 0px; margin: 0px;"" > 
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 40%; color: black;"">              
                                                                 " + proveedores.NombreBanco + @"            
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 15%; color: black;"">
                                                                " + proveedores.PrvDtllesBanNroCuenta + @"
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 45%; color: black;"" >              
                                                                " + proveedores.NombreTipoCuenta + @"
                                                            </td>
                                                           
                                                        </tr>
                                                </table>


                                             
                                                <table class=""colorBlue"" style="" border-collapse:collapse; border-spacing:0; "">
                                                            <tr style = ""padding: 0px; margin: 0px;"" >
       
                                                               <td class=""textoCenter td"" style=""border: 1px solid black;width: 100%; "" >
                                                               III.TIPO DE PROVEEDOR
                                                                </td>
                                                            </tr>
                                                    </table>

                                             

                                            <table class="""" style=""border-collapse:collapse; border-spacing:0; "">
                                                        <tr style = ""padding: 0px; margin: 0px;"" > 
                                                            <td class="" td"" style=""border-left: 1px solid black; width: 30%; color: black;"">              
                                                                " + (proveedores.PrvProveeedor == 1 ? "Natural" : "Juridica") + @"         
                                                            </td>
                                                            <td class="" td"" style="" width: 50%; color: black;"">
                                                                " + proveedores.NombreTipoProveedor + @"
                                                            </td>
                                                            <td class="" td"" style=""border-right: 1px solid #000;; width: 20%; color: black;"" >              
                                                                 " + proveedores.PrvTipoProveedorCual + @"
                                                            </td>
                                                           
                                                        </tr>
                                                </table>

                                          

                                                <table class=""colorBlue"" style="" border-collapse:collapse; border-spacing:0; "">
                                                            <tr style = ""padding: 0px; margin: 0px;"" >
       
                                                               <td class=""textoCenter td"" style=""border: 1px solid black;width: 100%; "" >
                                                               INFORMACIÓN DEL PRODUCTO O SERVICIO
                                                                </td>
                                                            </tr>
                                                    </table>
                                                

                                                <table style="" border-collapse:collapse; border-spacing:0; "">
                                                            <tr style = ""padding: 0px; margin: 0px;"" >
       
                                                               <td class="""" style=""border: 1px solid black;width: 100%; color: black;"" >
                                                                 " + maperarProdServPdf(prodservs) + @"
                                                                </td>
                                                            </tr>
                                                    </table>

                                              

                                                <table class=""colorBlue"" style="" border-collapse:collapse; border-spacing:0; "">
                                                            <tr style = ""padding: 0px; margin: 0px;"" >
       
                                                               <td class=""textoCenter td"" style=""border: 1px solid black;width: 100%; "" >
                                                              CONDICIONES DE PAGO
                                                                </td>
                                                            </tr>
                                                    </table>

                                              

                                                <table class="""" style=""border-collapse:collapse; border-spacing:0; "">
                                                        <tr style = ""padding: 0px; margin: 0px;"" > 
                                                            <td class="" td"" style=""border-left: 1px solid black; width: 20%; color: black;"">              
                                                                 " + proveedores.NombreCondiconesPago + @"        
                                                            </td>
                                                            <td class="" td"" style="" width: 30%; color: black;"">
                                                                " + proveedores.PrvCpaContadoCual + @"
                                                            </td>
                                                            <td class="" td"" style=""border-right: 1px solid #000;; width: 50%; color: black;"" >              
                                                                 " + proveedores.PrvCpaCual + @"
                                                            </td>
                                                           
                                                        </tr>
                                                </table>
           
        
                                           
                                                    <table style="" border-collapse:collapse; border-spacing:0; "">
                                                            <tr style = ""padding: 0px; margin: 0px;"" >
       
                                                               <td class="""" style=""border: 1px solid black;width: 100%; color: black; "" >
                                                                 " + maperarEmpresasPdf(empresas) + @"
                                                                </td>
                                                            </tr>
                                                    </table>

                                           

                                                    <table class=""colorBlue"" style="" border-collapse:collapse; border-spacing:0; "">
                                                            <tr style = ""padding: 0px; margin: 0px;"" >
       
                                                               <td class=""textoCenter td"" style=""border: 1px solid black;width: 100%; "" >
                                                              REFERENCIAS
                                                                </td>
                                                            </tr>
                                                    </table>

                                            <table style=""border-collapse:collapse; border-spacing:0; "">
                                                        <tr style = ""padding: 0px; margin: 0px;"" > 
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 20%; color: black;"">              
                                                                 Tiene usted experiencia en el sector?</br>
                                                    " + (proveedores.PrvExperienciaSector == true ? "SI" : "NO") + @"            
                                                            </td>
                                                            <td class="" textoCenter td"" style=""border: 1px solid black; width: 80%; padding: 0px; margin: 0px;"">
                                                              
                                                            <table class=""colorGreen"" style=""border-collapse:collapse; border-spacing:0; "">
                                                                    <tr style = ""padding: 0px; margin: 0px;"" > 
                                                                        <td class="" textoCenter td"" style=""border: 1px solid black; width: 33%"">              
                                                                            Empresa            
                                                                        </td>
                                                                        <td class="" textoCenter td"" style=""border: 1px solid black; width: 33%"">
                                                                            Teléfono
                                                                        </td>
                                                                        <td class="" textoCenter td"" style=""border: 1px solid black; width: 33%"" >              
                                                                            Contacto
                                                                        </td>
                                                           
                                                                    </tr>
                                                            </table>

                                                             

                                                            </td>
                                                           
                                                           
                                                        </tr>
                                                </table>
                                                    
                                                    
                                                    <div style=""displayfirma"">
                                                    <ul style=""margin-top:30px; margin-left:40px; list-style-type:none; width: 800px !important"">
                                                       <li style=""margin-bottom:20px"">Firma</li></br>
                                                        <li >{{Sig_es_:signer1:signature:dimension(width=80mm, height=12mm)}}</li> </br>
                                                        <li style=""width: 800px !important""><label>Email:</label> <span style=""min-width: 370px !important"">{{Em_es_:signer1:dimension(width=80mm):email}}</span></li>
                                                    </ul>
                                                    </div>
                                              
                                              
                                                  
                                                       
                                                 

        
                                                
                                    </body>
                            </html>";

            return html;
        }

        public static string MapearPdfDeclaracion(
           Proveedores proveedores)
        {
            return @"
                            <html>
                                <head>
                                <style>

                            

                                    .colorBlue
                                        {
                                            background-color: #273580;
                                            color: white;
                                        }
                                    .colorGreen
                                        {
                                            background-color: #46a13a;
                                            color: white;
                                        }
                                    .table
                                        {
                                            width: 100 %;
                                          
                                        }
                                    .td
                                        {

                                            font-family: Arial, sans-serif;
                                            font-size: 12px;
                                           
                                            padding: 2px 2px;
                                            word-break: normal;
                                        }
                                .textoCenter
                                        {
                                            text-align: center;
                                        }

                                .fecha
                                        {
                                            width: 15%;
                                        }
                                .nombre
                                        {
                                            width: 55%;
                                        }
                                .nit
                                        {
                                        width:30%;
                                        }
                                </style>
                                </head>
                                <body>"
                                                 + MapearDeclaracion(proveedores) + @"

        
                                                
                                    </body>
                            </html>";
        }
        public static string MapearDeclaracion(Proveedores proveedores)
        {
            return @"<div style=""margin-top:10px; padding:70px;"">" +
                  @"<div style=""text-align:center;"" ><img  style=""width:62px; height:62px"" src=""https://api_portal_proveedores.syspotec.co/PathFilePortalPrv/logo-urbaser.png"" /> </div>" +
                @"<h4 style=""text-align:center;"">Declaración Responsable del Código Ético de Proveedores de URBASER</h4>" +



            @"<p style=""text-align:justify; font-size:13.5px;"">URBASER extiende a todos sus proveedores, contratistas y colaboradores los valores y principios por" +
                "los que se rige la compañía y que permite reforzar las relaciones existentes y asegurar el " +
                "cumplimiento de sus políticas y compromisos.</p></br>" +
                MapearPersona(proveedores)+
                "<ul>" +

                    @"<li style=""text-align:justify; margin-top:10px;  font-size:13.5px;"">Que he recibido el Código Ético de Proveedores de URBASER donde se recogen todas las
                        relaciones a seguir por URBASER con todos sus proveedores, contratistas y colaboradores,
                        el cual conozco y acepto expresamente sin reservas, comprometiéndome a respetarlo,
                        cumplirlo, así como transmitírselo a mis empleados, colaboradores y subcontratistas que
                        presten servicios o realicen cualquier tipo de suministro dentro de URBASER.</li></br></br>" +

                    @"<li style=""text-align:justify; margin-top:10px;  font-size:13.5px;"">Que en caso de no poder dar cumplimiento a todos los requerimientos que se presentan en
                        el Código Ético para proveedores de URBASER, comunicaré este hecho a la Compañía para
                        que se establezca un plan de trabajo colaborativo que permita en el futuro evitar cualquier
                        tipo de riesgos para ambas partes.</li></br></br>" +

                    @"<li style=""text-align:justify; margin-top:10px;  font-size:13.5px;"">Que ni el que suscribe, ni en su caso a la empresa que represento en este caso, han sido
                        condenados mediante sentencia firme por delitos de apropiación indebida, estafa,
                        publicidad engañosa, falsedad documental o contable, corrupción pública o privada,
                        blanqueo de capitales, tráfico ilegal de mano de obra, constitución o integración de una
                        organización o grupo criminal, discriminación o violencia contra las personas, entre otros.</li></br></br>" +

                    @"<li style=""text-align:justify; margin-top:10px;  font-size:13.5px;"">Que el que suscribe o la empresa que represento se encuentra al corriente del cumplimiento
                        de las obligaciones tributarias y con la seguridad social impuestas por las disposiciones
                        legales vigentes que le resultan de aplicación.</li></br></br>" +
                 "</ul></br></br>" +
                 "</ul></br></br>" +

                 @"     <div style=""displayfirma"">
                                                    <ul style=""margin-top:10px; margin-left:40px; list-style-type:none; width: 800px !important"">
                                                       <li style=""margin-bottom:20px"">Firma</li></br>
                                                        <li >{{Sig_es_:signer1:signature:dimension(width=80mm, height=12mm)}}</li> </br>
                                                        <li style=""width: 800px !important""><label>Email:</label> <span style=""min-width: 370px !important"">{{Em_es_:signer1:dimension(width=80mm):email}}</span></li>
                                                    </ul>
                                                    </div>
                </div>";

        }

        public static string MapearPersona(Proveedores proveedor) 
        {
            /*Persona natural*/
            if (proveedor.PrvProveeedor == 1)
                return @"<p style=""text-align:justify;  font-size:13.5px;"">" + proveedor.PrvNombreProveedor + @", con documento de identidad No. " +
                 proveedor.IdentPrvCompleta + @", en nombre propio, DECLARO BAJO MI RESPONSABILIDAD:</p></br>";

            /*Persona juridica*/
            return @"<p style=""text-align:justify;  font-size:13.5px;"">" + proveedor.PrvRteLegalNombre + @", con documento de identidad No. " +
                 proveedor.PrvRteLegalIdentificacion + @", en nombre propio o en representación de la " +
                "sociedad "+ proveedor.PrvNombreProveedor+ ", con NIT "+ proveedor.IdentPrvCompleta + ", en calidad de representante " +
                "legal, DECLARO BAJO MI RESPONSABILIDAD:</p></br>";
        }
    }
}
