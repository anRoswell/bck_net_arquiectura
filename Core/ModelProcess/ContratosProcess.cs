namespace Core.ModelProcess
{
    using Core.Entities;
    using Core.Enumerations;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ContratosProcess
    {
        public static string MapearActividadesPendientesPdf(List<ActividadesPendientesContrato> data, int opcion)
        {
            string html = string.Empty;
            string detalle = string.Empty;

            switch (opcion)
            {
                case (int)Email_OpcionEjecutar.TodosLosEstados_ExceptoFirmaElectronica:
                    detalle = MapearActividades(data);
                    break;
                case (int)Email_OpcionEjecutar.FirmaElectronica:
                    detalle = MapearActividadesPendFirma(data);
                    break;
                default:
                    break;
            }

            if (data.Count > 0)
            {
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
                                    border-radius: 5px;
                                }

                                .tdCabecera {
                                    font-family: Arial, sans-serif;
                                    font-size: 12px;
                                    padding: 2px 2px;
                                    word-break: normal;
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

                                .negrita {
                                    font-weight: bold;
                                }
                            </style>
                        </head>

                        <body>
                            <table class=""table cabeceras"" style="" border-collapse:collapse; border-spacing:0;margin-bottom:10px;"">
                                <tr style=""padding: 0px; margin: 0px;"">
                                    <td class=""tdCabecera negrita"" style=""padding:10px;width: 100%; "">
                                        Actividades Pendientes de Contrato
                                    </td>
                                </tr>
                            </table>

                            <table class=""table"" style=""border-collapse:collapse; border-spacing:0; "">
                                <tr style="""" padding: 0px; margin: 0px; """">
                                    <td class=""textoCenter td negrita"" style=""border: 1px solid black;padding: 10px 0; width: 20%"">
                                        CONTRATO
                                    </td>
                                    <td class=""textoCenter td negrita"" style=""border: 1px solid black;padding: 10px 0; width: 80% "">
                                        DESCRIPCION ACTIVIDAD
                                    </td>
                                </tr>
                            </table>

                            <table class=""table"" style=""border-collapse:collapse; border-spacing:0;border: 1px solid black; "">
                                " + detalle + @"
                            </table>
                        </body>
                    </html>";
            }

            return html;
        }

        public static string MapearActividades(List<ActividadesPendientesContrato> data)
        {
            string html = string.Empty;

            foreach (ActividadesPendientesContrato item in data)
            {
                html += @"<tr style=""padding: 0px; margin: 0px;"">
                            <td class=""textoCenter td"" style=""border: 1px solid black; border-top: none; padding:8px; width: 20%"">
                                " + item.IdContrato + @"
                            </td>
                            <td class=""textoCenter td"" style=""border: 1px solid black; border-top: none; padding:8px; width: 80%"">
                                " + item.Pendiente + @"
                            </td>
                        </tr>";
            }

            return html;
        }

        public static string MapearActividadesPendFirma(List<ActividadesPendientesContrato> data)
        {
            string html = string.Empty;
            int i = 1;

            foreach (ActividadesPendientesContrato item in data)
            {
                if (i == 1) // Si es el primer ciclo
                {
                    html += @"<tr style=""padding: 0px; margin: 0px;"">
                            <td class=""textoCenter td"" style=""border: 1px solid black; border-top: none; padding:8px; width: 20%"">
                                " + item.IdContrato + @"
                            </td>
                            <td class=""textoCenter td"" style=""border: 1px solid black; border-top: none; border-bottom: none; padding:8px; width: 80%"" rowspan=""" + data.Count + @""">
                                Firma Electrónica Contratista
                            </td>
                        </tr>";
                }
                else
                {
                    html += @"<tr style=""padding: 0px; margin: 0px;"">
                            <td class=""textoCenter td"" style=""border: 1px solid black; border-top: none; padding:8px; width: 20%"">
                                " + item.IdContrato + @"
                            </td>
                        </tr>";
                }

                i++;
            }

            return html;
        }
    }
}
