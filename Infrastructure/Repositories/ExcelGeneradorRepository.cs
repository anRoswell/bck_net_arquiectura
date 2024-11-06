namespace Infrastructure.Repositories
{
    using Core.DTOs;
    using Core.DTOs.FilesDto;
    using Core.Interfaces;
    using Core.QueryFilters;
    using Microsoft.EntityFrameworkCore;
    using OfficeOpenXml;
    using OfficeOpenXml.Style;
    using System;
    using System.Collections.Generic;
    using System.Data;    
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public class ExcelGeneradorRepository<T> : IExcelGeneradorRepository<T> where T : class
    {
        private string trazamanual { get; set; }

        public async Task<ResponseDto<(FileResponseOracleBase64, byte[])>> ExecuteExcelGenerador(IQueryable dataQuery, string format, QueryOp360ConfigExcel ConfigExcel)
        {
            try
            {
                var respbytes = GenerarExcel(dataQuery, ConfigExcel);
                ResponseDto<(FileResponseOracleBase64, byte[])> response = new ResponseDto<(FileResponseOracleBase64, byte[])>()
                {
                    Codigo = 200,
                    Mensaje = "Archivo generado correctamente",
                    TotalRecords = respbytes.Item1,
                    Datos = (
                            new FileResponseOracleBase64()
                                {
                                    Id_Soporte = 0,
                                    NombreArchivo = ConfigExcel.NombreArchivo,
                                    Extension = ".xlsx",
                                    ArchivoBase64 = format == "base64" ? Convert.ToBase64String(respbytes.Item2) : "",
                                    Size = respbytes.Item2.Length,
                                    TypeMime = ConfigExcel.TypeMime
                            }, 
                            respbytes.Item2
                            )
                };
                return response;
            }
            catch (Exception ex)
            {                
                ResponseDto<(FileResponseOracleBase64, byte[])> response = new ResponseDto<(FileResponseOracleBase64, byte[])>()
                {
                    Codigo = 400,
                    Mensaje = $"Error en Generador2: desc: {ex.Message}, - {ex.InnerException?.Message ?? ""}--{ex.StackTrace}--traza 2: {trazamanual}",
                    TotalRecords = 0,
                    Datos = (null, null)
                };
                return response;
            }
        }

        #region Private Methods

        /// <summary>
        /// Generar archivo de excel con EPPlus sdafadsfjpodfug9dfsg70adsdf8g67sfg5fg5h8fdghads6f98ewnkert
        /// </summary>
        /// <param name="dataQuery">datos a exportar</param>
        /// <param name="PropertyExcel">propiedades para el libro de excel</param>
        /// <param name="NoMapear">lista de campos que no se exportaran al libro de excel</param>
        public (int, byte[]) GenerarExcel(IQueryable dataQuery, QueryOp360ConfigExcel ConfigExcel)
        {
            //02)-- Datos que no se mapearan
            Dictionary<string, string[]> NoMapear = GetColumnsNotMaped();
            var fila = 2;
            //03)--Generar Excel combinando los dos diccionarios de datos.
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new())
            {
                //01)-Agregar Estilo al Libro --Add some formatting to the worksheet
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Report_{DateTime.Now.ToString("yyyyMMdd")}");
                worksheet.DefaultRowHeight = 14;
                worksheet.HeaderFooter.FirstFooter.LeftAlignedText = DateTime.Now.ToString();
                //03)-Detectar propiedades de la fila
                IEnumerable<PropertyInfo> tmpProp = GetPropiedadesFila(dataQuery, NoMapear);
                var columnatmp = 1;
                foreach (PropertyInfo property in tmpProp)
                {
                    worksheet.Cells[1, columnatmp].Value = property.Name;
                    columnatmp++;
                }
                //02)-Empezar a llenar el contenido                                
                foreach (var ifilas in dataQuery)
                {
                    //05)-Llenar los datos
                    var columna = 1;
                    foreach (PropertyInfo property in tmpProp)
                    {
                        object value = ifilas.GetType().GetProperty(property.Name).GetValue(ifilas, null);
                        worksheet.Cells[fila, columna].Value = value?.ToString() ?? "";
                        columna++;
                    }
                    fila++;
                }
                //06)- Auntoajustar columnas
                worksheet.Cells["A:XFD"].AutoFitColumns();

                //07)- Crear algunas propiedades para el archivo.
                package.Workbook.Properties.Title = "Reporte Automatico";
                package.Workbook.Properties.Author = "Syspotec S.A.S";
                package.Workbook.Properties.Company = "Syspotec S.A.S";
                package.Workbook.Properties.Created = DateTime.Now;
                
                var encabezados = worksheet.Cells[1, 1, 1, columnatmp - 1];
                encabezados.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                encabezados.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                encabezados.Style.Font.Bold = true;
                encabezados.Style.Font.Color.SetColor(0,255,255,255);
                encabezados.Style.Fill.PatternType = ExcelFillStyle.Solid;
                //encabezados.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#273580"));
                encabezados.Style.Fill.BackgroundColor.SetColor(0, 39, 53, 128);
                encabezados.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                encabezados.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Workbook.Worksheets.First().View.ShowGridLines = false;
                worksheet.Workbook.Worksheets.First().View.FreezePanes(2, 1);
                
                //---Agregamos titulo con columnas concatenadas
                // Colocamos la información de la empresa de la factura

                //worksheet.Cells["C2"].Value = ConfigExcel.TituloReporteExcel;
                //worksheet.Cells["C2"].Style.Font.Bold = true;
                //worksheet.Cells["C2"].Style.Font.Size = 26;
                //worksheet.Cells[2, 1, 5, 2].Merge = true;
                //worksheet.Cells[2, 3, 5, 6].Merge = true;
                //worksheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //worksheet.Cells[2, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                //worksheet.Cells[2, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //worksheet.Cells[2, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                
                // Agregamos Logo institucional                
                ////var handler = new HttpClientHandler();
                ////trazamanual = $"{trazamanual}, -14 ";
                //////////handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                ////var _httpClient2 = new HttpClient(handler);
                ////trazamanual = $"{trazamanual}, -15 ";
                ////HttpResponseMessage Response = _httpClient2.GetAsync(ConfigExcel.rootFileServer.RUTA_WEB).Result;
                ////trazamanual = $"{trazamanual}, -16 ";
                ////Stream Filex = null;
                ////if (Response.IsSuccessStatusCode)
                ////{
                ////    trazamanual = $"{trazamanual}, -17 ";
                ////    Filex = Response.Content.ReadAsStreamAsync().Result;
                ////    trazamanual = $"{trazamanual}, -18 ";
                ////}

                //MemoryStream streamlogo = new MemoryStream(ConfigExcel.ArchivoByte);
                //Image img = Image.FromStream(streamlogo);
                //var excelImage = worksheet.Drawings.AddPicture("Logo", img);
                //trazamanual = $"{trazamanual}, -19 ";
                //excelImage.SetSize(126, 52);
                //excelImage.SetPosition(1, 10, 1, -40);
                //trazamanual = $"{trazamanual}, -20 ";
                return (fila, package.GetAsByteArray());
            }
        }

        public Dictionary<string, string[]> GetColumnsNotMaped()
        {
            var ExcluirCampos = new string[] { "NombreArchivo" };
            string[] ExcluirTipos = new string[] { "ICollection`1", "List`1", "HttpPostedFileWrapper", "IEnumerable`1", "Byte[]"};
            Dictionary<string, string[]> ColumnsNotMaped = new Dictionary<string, string[]>() {
                 { "Campos", ExcluirCampos }
                ,{ "Types",ExcluirTipos }
            };
            return ColumnsNotMaped;
        }

        public IEnumerable<PropertyInfo> GetPropiedadesFila(IQueryable dataQuery, Dictionary<string, string[]> NoMapear)
        {
            IEnumerable<PropertyInfo> getPropiedadesFilax = null;
            //03)-Detectar propiedades de la fila            
            foreach (var ifilas in dataQuery)
            {
                //04)-Crear fila de encabezados
                getPropiedadesFilax = ifilas.GetType().GetProperties()
                           .Where(x => (!NoMapear["Campos"].Contains(x.Name) &&
                                        !NoMapear["Types"].Contains(x.PropertyType.Name)) &&
                                        !(x.Name == x.PropertyType.Name)
                                        );
                break;
            }
            return getPropiedadesFilax;
        }
        #endregion
    }
}

