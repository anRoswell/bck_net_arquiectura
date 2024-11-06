using Core.CustomEntities;
using Core.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace Core.ModelProcess
{
    public class RequerimientosProcess
    {
        public static string MapearParticipacionPdf(List<ParticipacionDataReporteDto> data)
        {
            string htmlCriterios = string.Empty;
            string html = string.Empty;

            if (data.Count > 0)
            {
                if(!(data.FirstOrDefault().RespuestaEvaluacionCriterios.FirstOrDefault()?.RcriIdReqCriteriosEvaluacion is null))
                    htmlCriterios = @"<table class=""table cabeceras"" style="" border-collapse:collapse; border-spacing:0;margin-bottom:10px; "">
                                        <tr style = ""padding: 0px; margin: 0px;"">
                                            <td class=""tdCabecera negrita"" style=""padding:10px;width: 100%; "" >
                                                CRITERIOS DE EVALUACIÓN
                                            </td>
                                        </tr>
                                    </table>

                                    <table class=""table"" style=""border-collapse:collapse; border-spacing:0; "">
                                        <tr style=""padding: 0px; margin: 0px;"" > 
                                            <td class=""textoCenter tdCabecera negrita"" style=""border: 1px solid black; width: 50%"">              
                                                Criterio
                                            </td>
                                            <td class=""textoCenter tdCabecera negrita"" style=""border: 1px solid black; width: 50%"">
                                                Respuesta
                                            </td>
                                        </tr>
                                    </table>

                                    <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:10px; "">
                                        " + MapearCriterios(data.FirstOrDefault().RespuestaEvaluacionCriterios) + @"
                                    </table>
                    ";

                ParticipacionDataReporteDto entity = data.FirstOrDefault();

                html = @"
                            <html>
                                <head>
                                    <style>
                                        .table {
                                            width: 100%;
                                        }
                                        .cabeceras {
                                            background-color: #6C757D;
                                            color: white;
                                            border-radius:5px;
                                        }
                                        .tdCabecera {
                                            font-family: Arial, sans-serif;
                                            font-size: 12px;
                                            padding: 2px 2px;
                                            word-break: normal;
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
                                        .negrita{
                                             font-weight: bold;
                                        }
                                    </style>
                                </head>

                                <body>
                                    <table class=""table cabeceras"" style="" border-collapse:collapse; border-spacing:0;margin-bottom:10px;"">
                                        <tr style=""padding: 0px; margin: 0px;"">
                                            <td class=""tdCabecera negrita"" style=""padding:10px;width: 100%; "">
                                                DATOS REQUERIMIENTO
                                            </td>
                                        </tr>
                                    </table>

                                    <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:10px;border:1px solid black;"">    
                                        <tr style=""padding: 0px; margin: 0px;"">
                                            <td class=""textoIzq negrita"" style=""padding-left: 15px;width: 20%;"">
                                                Empresa
                                            </td>
                                            <td class=""textoIzq"" style=""padding:5px; width: 30%;"" colspan=""3"">
                                                " + entity.NombreEmpresa + @"
                                            </td>
                                            <td class="""" style=""width: 25%;"">
                                                
                                            </td>
                                            <td class="""" style=""padding:5px; width: 25%; "">
                                                
                                            </td>
                                        </tr>
                                        <tr style=""padding: 0px; margin: 0px;"">
                                            <td class=""textoIzq negrita"" style=""padding-left: 15px;width: 20%;"">                                       
                                                Fecha Entrega
                                            </td>
                                            <td class=""textoIzq"" style=""padding:5px; width: 30%;"">
                                                " + entity.ReqFechaEntrega.ToString("dd/MM/yyyy") + @"
                                            </td>
                                            <td class=""textoIzq negrita"" style=""width: 25%; "">
                                                Compra Prevista
                                            </td>
                                            <td class=""textoIzq"" style=""padding:5px; width: 25%; "">
                                                " + entity.ReqCompraPrevista.ToString("dd/MM/yyyy") + @"
                                            </td>
                                        </tr>
                                        <tr style=""padding: 0px; margin: 0px;"">
                                            <td class=""textoIzq negrita"" style=""padding-left: 15px;width: 20%;"">
                                                Lugar de Entrega
                                            </td>
                                            <td class=""textoIzq"" style=""padding:5px; width: 30%; "">
                                                " + entity.ReqLugarEntrega + @"
                                            </td>
                                            <td class="""" style=""width: 25%; "">
                                        
                                            </td>
                                            <td class="""" style=""padding:5px; width: 25%; "">
                                        
                                            </td>
                                        </tr>
                                        <tr style=""padding: 0px; margin: 0px;"">
                                            <td class="""" style=""padding-left: 15px;width: 20%;"">
                                                
                                            </td>
                                            <td class="""" style=""padding:5px; width: 30%;"">
                                                
                                            </td>
                                            <td class="""" style=""width: 25%; "">
                                        
                                            </td>
                                            <td class="""" style=""padding:5px; width: 25%; "">
                                        
                                            </td>
                                        </tr>
                                    </table>
    
                                    <table class=""table cabeceras"" style="" border-collapse:collapse; border-spacing:0;margin-bottom:10px; "">
                                        <tr style=""padding: 0px; margin: 0px;"">
                                            <td class=""tdCabecera negrita"" style=""padding:10px;width: 100%; "">
                                                DATOS PROVEEDOR
                                            </td>
                                        </tr>
                                    </table>
    
                                    <table class=""table"" style=""border-collapse:collapse; border-spacing:0;margin-bottom:10px;border:1px solid black;"">
                                        <tr style=""padding: 0px; margin: 0px;"">
                                            <td class=""textoIzq negrita"" style=""padding-left: 15px;width: 25%;"">
                                                Fecha Oferta Presentada
                                            </td>
                                            <td class=""textoIzq"" style=""padding:5px; width: 75%;"">
                                                " + entity.Participante.FirstOrDefault()?.FecOfePre.ToString("dd/MM/yyyy") + @"
                                            </td>
                                        </tr>
                                        <tr style=""padding: 0px; margin: 0px;"">
                                            <td class=""textoIzq negrita"" style=""padding-left: 15px; width: 25%;"">
                                                Observación
                                            </td>
                                            <td class=""textoIzq"" style=""padding:5px; width: 75%;"">
                                                " + entity.Participante.FirstOrDefault()?.Observacion + @"
                                            </td>
                                        </tr>
                                    </table>

                                    " + htmlCriterios + @"

                                    <table class=""table cabeceras"" style="" border-collapse:collapse; border-spacing:0;margin-bottom:10px; "">
                                        <tr style=""padding: 0px; margin: 0px;"" >
                                            <td class=""tdCabecera negrita"" style=""padding:10px;width: 100%; "" >
                                                ARTÍCULOS Y/O SERVICIOS REQUERIDOS
                                            </td>
                                        </tr>
                                    </table>

                                    <table class=""table"" style=""border-collapse:collapse; border-spacing:0; "">
                                        <tr style = ""padding: 0px; margin: 0px;"" >
                                            <td class=""textoCenter tdCabecera negrita"" style=""border: 1px solid black;width: 20%"">
                                                Descripción
                                            </td>
                                            <td class=""textoCenter tdCabecera negrita"" style=""border: 1px solid black;width: 10%"" >              
                                                Cantidad
                                            </td>
                                            <td class=""textoCenter tdCabecera negrita"" style=""border: 1px solid black;width: 15%"">              
                                                Valor
                                            </td>
                                            <td class=""textoCenter tdCabecera negrita"" style=""border: 1px solid black;width: 5%"">              
                                                % Iva
                                            </td>
                                            <td class=""textoCenter tdCabecera negrita"" style=""border: 1px solid black;width: 5%"">              
                                                % Desc.
                                            </td>
                                            <td class=""textoCenter tdCabecera negrita"" style=""border: 1px solid black;width: 15%"">              
                                                Precio Total
                                            </td>
                                            <td class=""textoCenter tdCabecera negrita"" style=""border: 1px solid black;width: 10%"">              
                                                ¿Marca solicitada?
                                            </td>
                                            <td class=""textoCenter tdCabecera negrita"" style=""border: 1px solid black;width: 20%"">              
                                                Observación
                                            </td>
                                        </tr>
                                    </table>

                                    <table class=""table"" style=""border-collapse:collapse; border-spacing:0; "">
                                        " + MapearArticulosOfertados(data.FirstOrDefault().ArticulosOfrecidos) + @"
                                    </table>
                                </body>
                            </html>";
            }

            return html;
        }

        public static string MapearCriterios(List<RespuestaEvaluacionCriterio> criterios)
        {
            string htmlCriterios = string.Empty;

            foreach (var item in criterios)
            {
                htmlCriterios += @"<tr style = ""padding: 0px; margin: 0px;"">       
                                    <td class="" textoCenter td"" style=""border: 1px solid black;;width: 50%"">
                                        " + item.RcriTituloCriterio + @"
                                    </td>
                                    <td class=""textoCenter td"" style=""border: 1px solid black;;width: 50%"">              
                                        " + (!(item.RespuestaCriterio.ValorRtaSiNo is null) && item.RespuestaCriterio.ValorRtaSiNo != "0" ? item.RespuestaCriterio.ValorRtaSiNo :
                                             !(item.RespuestaCriterio.ValorRtaRango is null) && item.RespuestaCriterio.ValorRtaRango != "0" ? item.RespuestaCriterio.ValorRtaRango :
                                             !(item.RespuestaCriterio.ValorRtaUnica is null) && item.RespuestaCriterio.ValorRtaUnica != "0" ? item.RespuestaCriterio.ValorRtaUnica : ""
                                            ) + @"
                                    </td>
                                </tr>";
            }

            return htmlCriterios;
        }

        public static string MapearArticulosOfertados(List<ArticulosOfrecidosParticipanteReq> articulos)
        {
            string htmlArticulos = string.Empty;

            foreach (var item in articulos)
            {
                htmlArticulos += @"<tr style = ""padding: 0px; margin: 0px;"">
                                    <td class=""textoCenter td"" style=""border: 1px solid black;width: 20%"">
                                        " + string.Concat(item.RasrDescripcionDteApot, " - (", item.RasrUnidadMedidaApot, ")") + @"
                                    </td>
                                    <td class=""textoCenter td"" style=""border: 1px solid black;width: 10%"" >              
                                        " + item.CantidadTotal.ToString("0.00") + @"
                                    </td>
                                    <td class=""textoCenter td"" style=""border: 1px solid black;width: 15%"">              
                                        " + item.Valor.ToString("0,0.00") + @"
                                    </td>
                                    <td class=""textoCenter td"" style=""border: 1px solid black;width: 5%"">              
                                        " + item.Iva.ToString("0.00") + @"
                                    </td>
                                    <td class=""textoCenter td"" style=""border: 1px solid black;width: 5%"">              
                                        " + item.Descuento.ToString("0.00") + @"
                                    </td>
                                    <td class=""textoCenter td"" style=""border: 1px solid black;width: 15%"">              
                                        " + (item.Valor + (item.Valor * (item.Iva/100)) - (item.Valor * item.Descuento)).ToString("0,0.00") + @"
                                    </td>
                                    <td class=""textoCenter td"" style=""border: 1px solid black;width: 10%"">              
                                        " + (item.MarcaSolicitada ? "Si" : "No") + @"
                                    </td>
                                    <td class=""textoCenter td"" style=""border: 1px solid black;width: 20%"">              
                                        " + item.Observacion + @"
                                    </td>
                                </tr>";
            }

            return htmlArticulos;
        }
    }
}
