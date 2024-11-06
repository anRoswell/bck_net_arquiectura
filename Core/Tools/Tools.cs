namespace Core.Tools
{
    using Core.Exceptions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.StaticFiles;
    using OfficeOpenXml;
    using PdfSharp;
    using PdfSharp.Drawing;
    using PdfSharp.Pdf;
    using PdfSharp.Pdf.IO;
    using QRCoder;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using TheArtOfDev.HtmlRenderer.PdfSharp;

    public static class Funciones
    {
        public static string GetQueryNombreEmpresa_Sispo()
        {
            return $"CASE  " +
                $"WHEN t.EMPRESA = 'NSGS' THEN 'URBASER COLOMBIA S.A. E.S.P.' " +
                $"WHEN t.EMPRESA = 'NSTJ' THEN 'URBASER TUNJA S.A. E.S.P.'  " +
                $"WHEN t.EMPRESA = 'NCAF' THEN 'URBASER MONTENEGRO S.A. E.S.P.'  " +
                $"WHEN t.EMPRESA = 'NAIS' THEN 'URBASER SOACHA S.A. E.S.P.'  " +
                $"WHEN t.EMPRESA = 'NSAD' THEN 'URBASER DUITAMA S.A. E.S.P.'  " +
                $"WHEN t.EMPRESA = 'NSAT' THEN 'URBASER LA TEBAIDA S.A. E.S.P.'  " +
                $"WHEN t.EMPRESA = 'NSAP' THEN 'URBASER POPAYAN S.A. E.S.P.'  " +
                $"WHEN t.EMPRESA = 'NCAU' THEN 'URBASER ECOAMBIENTAL S.A. E.S.P.'  " +
                $"ELSE '' " +
                $"END AS NombreEmpresa, ";
        }

        public static string GetQueryNombreEmpresa_Apoteosys()
        {
            return $"CASE  " +
                $"WHEN MC_____CODIGO____CONTAB_B = 'NSGS' THEN 'URBASER COLOMBIA S.A. E.S.P.' " +
                $"WHEN MC_____CODIGO____CONTAB_B = 'NSTJ' THEN 'URBASER TUNJA S.A. E.S.P.'  " +
                $"WHEN MC_____CODIGO____CONTAB_B = 'NCAF' THEN 'URBASER MONTENEGRO S.A. E.S.P.'  " +
                $"WHEN MC_____CODIGO____CONTAB_B = 'NAIS' THEN 'URBASER SOACHA S.A. E.S.P.'  " +
                $"WHEN MC_____CODIGO____CONTAB_B = 'NSAD' THEN 'URBASER DUITAMA S.A. E.S.P.'  " +
                $"WHEN MC_____CODIGO____CONTAB_B = 'NSAT' THEN 'URBASER LA TEBAIDA S.A. E.S.P.'  " +
                $"WHEN MC_____CODIGO____CONTAB_B = 'NSAP' THEN 'URBASER POPAYAN S.A. E.S.P.'  " +
                $"WHEN MC_____CODIGO____CONTAB_B = 'NCAU' THEN 'URBASER ECOAMBIENTAL S.A. E.S.P.'  " +
                $"ELSE '' " +
                $"END AS NombreEmpresa, ";
        }

        public static string GetQueryBaseOrigen()
        {
            return $"LTRIM(TO_CHAR(sum(case when a.MC_____CREMONLOC_B > 0 " +
                $"then (a.MC_____BASE______B / 0.19) " +
                $"else ((a.MC_____BASE______B / 0.19) * -1) " +
                $"end), '$999,999,999,999,999' )) as Base_Origen, ";
        }

        public static string GetQueryBaseRetencion()
        {
            return $"LTRIM(TO_CHAR(sum(case when a.MC_____CREMONLOC_B > 0 " +
                $"then a.MC_____BASE______B " +
                $"else a.MC_____BASE______B * -1 " +
                $"end), '$999,999,999,999,999.00' )) as Base_Retencion, ";
        }

        public static string GetQueryBaseRetencionIva()
        {
            return $"LTRIM(TO_CHAR(sum(case when a.MC_____CREMONLOC_B > 0 " +
                $"then a.MC_____BASE______B " +
                $"else a.MC_____BASE______B * -1 " +
                $"end), '$999,999,999,999,999' )) as Base_Retencion, ";
        }

        public static string GetQueryPorcentajePracticado()
        {
            return $"LTRIM(TO_CHAR ( " +
                $"CASE  " +
                $"	WHEN (SUM(MC_____CREMONLOC_B) / SUM(a.MC_____BASE______B) * 100) - trunc((SUM(MC_____CREMONLOC_B) / SUM(a.MC_____BASE______B) * 100)) > 0.5 THEN ROUND((SUM(MC_____CREMONLOC_B) / SUM(a.MC_____BASE______B) * 100)) " +
                $"	ELSE (SUM(MC_____CREMONLOC_B) / SUM(a.MC_____BASE______B) * 100) " +
                $"END,'999.99') " +
                $") Porcentaje_Practicado,  ";
        }

        public static string GetCodigoUnico(string CodigoID)
        {
            try
            {
                string CodigoUnico = DateTime.Now.ToString("yyyyMMddHHmmss_fff") + "_" + CodigoID + "_" + Path.GetRandomFileName().PadLeft(11).Replace('.', '_');
                return CodigoUnico;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public static string GetSHA256(string cadena)
        {
            try
            {
                SHA256 sha256 = SHA256Managed.Create();
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] stream = null;
                StringBuilder sb = new StringBuilder();
                stream = sha256.ComputeHash(encoding.GetBytes(cadena));
                for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
                return sb.ToString();
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public static string PdfSharpConvert(string html, string pathFileServer, string id)
        {
            try
            {
                using MemoryStream ms = new MemoryStream();
                var pdf = PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4, 0);
                pdf.Save(ms);
                byte[] res = ms.ToArray();

                string nombre = GetCodigoUnico(id) + ".pdf";

                if (!Directory.Exists(pathFileServer))
                {
                    Directory.CreateDirectory(pathFileServer);
                }

                var testFile = Path.Combine(pathFileServer, nombre);
                string url = "Proveedores/" + nombre;

                bool isCreated = ByteArrayToFile(testFile, res);
                return nombre;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public static string PdfSharpConvertParticipacion(string html, string pathFileServer, string id, string pathRel)
        {
            try
            {
                using MemoryStream ms = new MemoryStream();
                var pdf = PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4, 0);
                pdf.Save(ms);
                byte[] res = ms.ToArray();

                string nombre = GetCodigoUnico(id) + ".pdf";

                if (!Directory.Exists(pathFileServer))
                {
                    Directory.CreateDirectory(pathFileServer);
                }

                var testFile = Path.Combine(pathFileServer, nombre);
                string url = LimpiarPaths(Path.Combine(pathRel, nombre));
                bool isCreated = ByteArrayToFile(testFile, res);
                return url;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public static string PdfSharpConvertReturnedName(string html, string pathFileServer, string id)
        {
            try
            {
                using MemoryStream ms = new MemoryStream();
                var pdf = PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4, 50);
                pdf.Save(ms);
                byte[] res = ms.ToArray();

                string nombre = GetCodigoUnico(id) + ".pdf";

                if (!Directory.Exists(pathFileServer))
                {
                    Directory.CreateDirectory(pathFileServer);
                }

                var testFile = Path.Combine(pathFileServer, nombre);
                bool isCreated = ByteArrayToFile(testFile, res);
                return nombre;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public static byte[] GenerateArchivoPdfCombinado(List<string> listadoArchivos, string pathFS)
        {
            try
            {
                // Open the output document
                PdfDocument outputDocument = new PdfDocument();

                // Iterate files
                foreach (string file in listadoArchivos)
                {
                    // Open the document to import pages from it.
                    string pathFile = Path.Combine(pathFS, file);
                    PdfDocument inputDocument = PdfReader.Open(pathFile, PdfDocumentOpenMode.Import);

                    // Iterate pages
                    int count = inputDocument.PageCount;
                    for (int idx = 0; idx < count; idx++)
                    {
                        // Get the page from the external document...
                        PdfPage page = inputDocument.Pages[idx];
                        // ...and add it to the output document.
                        outputDocument.AddPage(page);
                    }
                }

                // Save the document...
                using MemoryStream ms = new MemoryStream();
                outputDocument.Save(ms);
                return ms.ToArray();

                //const string filename = "ConcatenatedDocument1_tempfile.pdf";
                //outputDocument.Save(filename);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Método para generar archivo con firmas electronicas y guardarlo en el File Server.
        /// </summary>
        /// <param name="files"></param>
        /// <param name="pathFileTmp"></param>
        /// <param name="pathFileContrato"></param>
        /// <returns>bool: Si se creó el archivo con éxito; string: Mensaje de la operación</returns>
        public async static Task<(bool, string)> GenerateArchivoPdfFirmaElectronica(IFormFile files, string pathFileTmp, string pathFileContrato)
        {
            bool wasCreated;
            string msg = string.Empty;

            try
            {
                // Create the file, or overwrite if the file exists.
                using (FileStream fs = File.Create(pathFileTmp))
                {
                    await files.CopyToAsync(fs);
                }

                // Open the document to import pages from it.
                PdfDocument pdfDocument = PdfReader.Open(pathFileTmp, PdfDocumentOpenMode.Modify);

                string html = @"<div style=""displayfirmaContratista"">
                            <ul style=""margin-top:30px; margin-left:40px; list-style-type:none"">
                                <li style=""margin-bottom:20px"">Firma Contratista:</li>
                                <li>{{Sig_es_:signer1:signature:dimension(width=50mm, height=12mm)}}</li>
                            </ul>
                        </div>
                        <div style=""displayfirmaRteLegal"">
                            <ul style=""margin-top:30px; margin-left:40px; list-style-type:none"">
                                <li style=""margin-bottom:20px"">Firma Rte Legal:</li>
                                <li>{{Sig_es_:signer2:signature:dimension(width=50mm, height=12mm)}}</li>
                            </ul>
                        </div>";

                PdfGenerator.AddPdfPages(pdfDocument, html, PageSize.Letter);

                //When the file is saved dimentions change to - Width = 11 inches, Height - 11 inches
                // Save the document...
                pdfDocument.Save(pathFileContrato);

                // Eliminamos archivo temporal
                if (File.Exists(pathFileTmp))
                {
                    File.Delete(pathFileTmp);
                }

                wasCreated = true;
                msg = "Operación exitosa. El contrato ha sido enviado para firmar.";
            }
            catch (Exception e)
            {
                wasCreated = false;
                msg = $"Error: {e.Message}";
            }

            return (wasCreated, msg);
        }

        public static string GenerateArchivoPdfCombinadoProveddor(List<string> listadoArchivos, string pathFS, string id, string nombreant = "")
        {
            try
            {
                // Open the output document
                PdfDocument outputDocument = new PdfDocument();

                // Iterate files
                foreach (string file in listadoArchivos)
                {
                    // Open the document to import pages from it.
                    string pathFile = Path.Combine(pathFS, file);
                    PdfDocument inputDocument = PdfReader.Open(pathFile, PdfDocumentOpenMode.Import);

                    // Iterate pages
                    int count = inputDocument.PageCount;
                    for (int idx = 0; idx < count; idx++)
                    {
                        // Get the page from the external document...
                        PdfPage page = inputDocument.Pages[idx];
                        // ...and add it to the output document.
                        outputDocument.AddPage(page);
                    }
                }

                // Save the document...
                using MemoryStream ms = new MemoryStream();
                outputDocument.Save(ms);

                byte[] res = ms.ToArray();

                string nombre = nombreant == "" ? GetCodigoUnico(id) + ".pdf" : nombreant;

                if (!Directory.Exists(pathFS))
                {
                    Directory.CreateDirectory(pathFS);
                }

                var testFile = Path.Combine(pathFS, nombre);
                string url = "Proveedores/" + nombre;

                bool isCreated = ByteArrayToFile(testFile, res);
                return url;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public static bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                fs.Write(byteArray, 0, byteArray.Length);
                return true;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public static bool SaveStreamAsFileAsync(string filePath, byte[] res, string fileName)
        {
            try
            {
                DirectoryInfo info = new DirectoryInfo(filePath);
                if (!info.Exists)
                {
                    info.Create();
                }

                string path = Path.Combine(filePath, fileName);
                return ByteArrayToFile(path, res);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public static string ConvertStreamToBase64(Stream stream)
        {
            // Leer el contenido del flujo y convertirlo a una matriz de bytes
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);

            // Convertir la matriz de bytes a una cadena Base64
            string base64String = Convert.ToBase64String(buffer);

            return base64String;
        }

        public static string GetContentType(string fileName)
        {
            try
            {
                new FileExtensionContentTypeProvider().TryGetContentType(fileName, out string contentType);
                return contentType ?? "application/octet-stream";
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public static (string, string) GetTipoDato(Object valor)
        {
            /*             
            System.Int32
            System.Double
            System.DateTime
            System.Boolean
            System.String
             */
            string tipo;
            string tipo2;
            if (int.TryParse(valor.ToString(), out int result))
            {
                tipo = typeof(int).ToString();
                tipo2 = "numero";
            }
            else if (bool.TryParse(valor.ToString(), out bool result2))
            {
                tipo = typeof(bool).ToString();
                tipo2 = "bool";
            }
            else if (double.TryParse(valor.ToString(), out double result3))
            {
                tipo = typeof(double).ToString();
                tipo2 = "numero";
            }
            else if (DateTime.TryParse(valor.ToString(), out DateTime result4))
            {
                tipo = typeof(DateTime).ToString();
                tipo2 = "fecha";
            }
            else if (DateOnly.TryParse(valor.ToString(), out DateOnly result5))
            {
                tipo = typeof(DateOnly).ToString();
                tipo2 = "fecha";
            }
            else if (TimeOnly.TryParse(valor.ToString(), out TimeOnly result6))
            {
                tipo = typeof(DateOnly).ToString();
                tipo2 = "fecha";
            }
            else
            {
                tipo = valor.GetType().ToString();
                tipo2 = "texto";
            }
            return (tipo, tipo2);
        }

        public static Tuple<string, byte[]> PdfSharpConvertToBytes(string html, string pathFileServer, string id)
        {
            try
            {
                using MemoryStream ms = new MemoryStream();
                var pdf = PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4, 0);
                pdf.Save(ms);
                byte[] res = ms.ToArray();

                string nombre = GetCodigoUnico(id) + ".pdf";

                return new Tuple<string, byte[]>(nombre, res);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public static byte[] PdfSharpConvertWithoutCreateFile(string html)
        {
            try
            {
                using MemoryStream ms = new MemoryStream();
                var pdf = PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4, 50);
                pdf.Save(ms);
                byte[] res = ms.ToArray();
                return res;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public static Tuple<string, byte[]> PdfSharpConvertToBytes2(string html, string id)
        {
            try
            {
                using MemoryStream ms = new MemoryStream();
                var pdf = PdfGenerator.GeneratePdf(html, PageSize.Letter);
                pdf.Save(ms);
                byte[] res = ms.ToArray();

                string nombre = GetCodigoUnico(id) + ".pdf";

                return new Tuple<string, byte[]>(nombre, res);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// ESTO NO SE ESTA USANDO (BORRAR)
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static byte[] PdfSharpConvertWithoutCreateFilePrueba(string html)
        {
            try
            {
                PdfDocument document = new PdfDocument();

                // Create a font
                XFont font = new XFont("Times", 25, XFontStyle.Bold);

                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                gfx.DrawString("PAGINA 1", font, XBrushes.DarkRed, new XRect(0, 0, page.Width, page.Height), XStringFormats.TopLeft);

                page = document.AddPage();
                gfx = XGraphics.FromPdfPage(page);
                gfx.DrawString("PAGINA 2", font, XBrushes.DarkRed, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);

                //var pdf = PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4, 50);
                using MemoryStream ms = new MemoryStream();
                document.Save(ms);
                byte[] res = ms.ToArray();
                return res;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public static string NumeroALetras(decimal numberAsString)
        {
            try
            {
                string dec;

                if (numberAsString < 0)
                {
                    throw new BusinessException($"Error: existen valores negativos que no se pueden convertir a letras!");
                }

                var entero = Convert.ToInt64(Math.Truncate(numberAsString));
                var decimales = Convert.ToInt32(Math.Round((numberAsString - entero) * 100, 2));
                if (decimales > 0)
                {
                    //dec = " PESOS CON " + decimales.ToString() + "/100";
                    dec = $" PESOS {decimales:0,0} /100";
                }
                //Código agregado por mí
                else
                {
                    //dec = " PESOS CON " + decimales.ToString() + "/100";
                    dec = $" PESOS {decimales:0,0} /100";
                }
                var res = NumeroALetras(Convert.ToDouble(entero)) + dec;
                return res;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        private static string NumeroALetras(double value)
        {
            string num2Text; value = Math.Truncate(value);
            if (value == 0) num2Text = "CERO";
            else if (value == 1) num2Text = "UNO";
            else if (value == 2) num2Text = "DOS";
            else if (value == 3) num2Text = "TRES";
            else if (value == 4) num2Text = "CUATRO";
            else if (value == 5) num2Text = "CINCO";
            else if (value == 6) num2Text = "SEIS";
            else if (value == 7) num2Text = "SIETE";
            else if (value == 8) num2Text = "OCHO";
            else if (value == 9) num2Text = "NUEVE";
            else if (value == 10) num2Text = "DIEZ";
            else if (value == 11) num2Text = "ONCE";
            else if (value == 12) num2Text = "DOCE";
            else if (value == 13) num2Text = "TRECE";
            else if (value == 14) num2Text = "CATORCE";
            else if (value == 15) num2Text = "QUINCE";
            else if (value < 20) num2Text = "DIECI" + NumeroALetras(value - 10);
            else if (value == 20) num2Text = "VEINTE";
            else if (value < 30) num2Text = "VEINTI" + NumeroALetras(value - 20);
            else if (value == 30) num2Text = "TREINTA";
            else if (value == 40) num2Text = "CUARENTA";
            else if (value == 50) num2Text = "CINCUENTA";
            else if (value == 60) num2Text = "SESENTA";
            else if (value == 70) num2Text = "SETENTA";
            else if (value == 80) num2Text = "OCHENTA";
            else if (value == 90) num2Text = "NOVENTA";
            else if (value < 100) num2Text = NumeroALetras(Math.Truncate(value / 10) * 10) + " Y " + NumeroALetras(value % 10);
            else if (value == 100) num2Text = "CIEN";
            else if (value < 200) num2Text = "CIENTO " + NumeroALetras(value - 100);
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) num2Text = NumeroALetras(Math.Truncate(value / 100)) + "CIENTOS";
            else if (value == 500) num2Text = "QUINIENTOS";
            else if (value == 700) num2Text = "SETECIENTOS";
            else if (value == 900) num2Text = "NOVECIENTOS";
            else if (value < 1000) num2Text = NumeroALetras(Math.Truncate(value / 100) * 100) + " " + NumeroALetras(value % 100);
            else if (value == 1000) num2Text = "MIL";
            else if (value < 2000) num2Text = "MIL " + NumeroALetras(value % 1000);
            else if (value < 1000000)
            {
                num2Text = NumeroALetras(Math.Truncate(value / 1000)) + " MIL";
                if ((value % 1000) > 0)
                {
                    num2Text = num2Text + " " + NumeroALetras(value % 1000);
                }
            }
            else if (value == 1000000)
            {
                num2Text = "UN MILLON";
            }
            else if (value < 2000000)
            {
                num2Text = "UN MILLON " + NumeroALetras(value % 1000000);
            }
            else if (value < 1000000000000)
            {
                num2Text = NumeroALetras(Math.Truncate(value / 1000000)) + " MILLONES ";
                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0)
                {
                    num2Text = num2Text + " " + NumeroALetras(value - Math.Truncate(value / 1000000) * 1000000);
                }
            }
            else if (value == 1000000000000) num2Text = "UN BILLON";
            else if (value < 2000000000000) num2Text = "UN BILLON " + NumeroALetras(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            else
            {
                num2Text = NumeroALetras(Math.Truncate(value / 1000000000000)) + " BILLONES";
                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0)
                {
                    num2Text = num2Text + " " + NumeroALetras(value - Math.Truncate(value / 1000000000000) * 1000000000000);
                }
            }
            return num2Text;
        }

        public static string FormatearMes(int mes)
        {
            try
            {
                return (mes < 10 ? string.Concat("0", mes) : mes.ToString());
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public static string GetQrCodeHtml(string texto)
        {
            try
            {
                QRCodeGenerator qrGenerator = new();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(texto, QRCodeGenerator.ECCLevel.Q);
                Base64QRCode qrCode = new(qrCodeData);
                var imgType = Base64QRCode.ImageType.Png;
                string qrCodeImageAsBase64 = qrCode.GetGraphic(3, Color.FromArgb(16, 143, 230), Color.Transparent, false, imgType);
                string imgHtml = $"<img alt=\"Embedded QR Code\" src=\"data:image/{imgType.ToString().ToLower()};base64,{qrCodeImageAsBase64}\" />";
                return imgHtml;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public static int GetCantidadPropiedades<T>()
        {
            try
            {
                // Get the Type object corresponding to MyClass.
                Type myType = typeof(T);
                return myType.GetProperties().Length;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public static string LimpiarPaths(string texto)
        {
            try
            {
                string textoLimpio = texto.Replace("\\", "/");
                return textoLimpio;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        /*
        * AES admite claves de 128, 192 y 256 bits, lo que significa que debes proporcionar 
        * una cadena de 16, 24 o 32 caracteres ASCII para que sirva como clave.
        * Clave de 128 bits (16 caracteres)
        */
        public static string EncriptarAES128(string texto, string clave)
        {
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(clave);
                aesAlg.Mode = CipherMode.CBC;

                // Generar un IV aleatorio (vector de inicialización) para cada operación de encriptación
                aesAlg.IV = GenerateRandomIV();

                // Crear un encriptador para realizar la transformación
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(texto);
                        }
                    }

                    // Concatenar el IV al principio del resultado
                    byte[] ivAndCiphertext = new byte[aesAlg.IV.Length + msEncrypt.ToArray().Length];
                    Array.Copy(aesAlg.IV, ivAndCiphertext, aesAlg.IV.Length);
                    Array.Copy(msEncrypt.ToArray(), 0, ivAndCiphertext, aesAlg.IV.Length, msEncrypt.ToArray().Length);

                    return Convert.ToBase64String(ivAndCiphertext);
                }
            }
        }

        /*
        * AES admite claves de 128, 192 y 256 bits, lo que significa que debes proporcionar 
        * una cadena de 16, 24 o 32 caracteres ASCII para que sirva como clave.
        * Clave de 128 bits (16 caracteres)
        */
        public static string DesencriptarAES128(string textoEncriptado, string clave)
        {
            try
            {
                using (AesManaged aesAlg = new AesManaged())
                {
                    aesAlg.Key = Encoding.UTF8.GetBytes(clave);
                    aesAlg.Mode = CipherMode.CBC;

                    // Extraer el IV del texto cifrado
                    byte[] iv = new byte[aesAlg.IV.Length];
                    Array.Copy(Convert.FromBase64String(textoEncriptado), iv, aesAlg.IV.Length);
                    aesAlg.IV = iv;

                    // Crear un desencriptador para realizar la transformación
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(textoEncriptado).Skip(aesAlg.IV.Length).ToArray()))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch
            {
                return "0";
            }
        }

        /*
        * AES admite claves de 128, 192 y 256 bits, lo que significa que debes proporcionar 
        * una cadena de 16, 24 o 32 caracteres ASCII para que sirva como clave.
        * Clave de 128 bits (16 caracteres)
        */
        public static byte[] GenerateRandomIV()
        {
            // Generar un IV (vector de inicialización) aleatorio
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] iv = new byte[16]; // Tamaño del IV en bytes (16 bytes para AES)
                rng.GetBytes(iv);
                return iv;
            }
        }

        public static string Base64EncodeWithKey(string texto, string clave)
        {
            try
            {
                // Concatenar la clave al texto
                string textoConClave = texto + clave;

                // Codificar el texto resultante en Base64
                byte[] bytesToEncode = Encoding.UTF8.GetBytes(textoConClave);
                return Convert.ToBase64String(bytesToEncode);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string Base64DecodeWithKey(string textoCodificado, string clave)
        {
            try
            {
                // Decodificar el texto en Base64
                byte[] bytesDecoded = Convert.FromBase64String(textoCodificado);

                // Convertir los bytes decodificados a cadena UTF-8
                string textoDecodificado = Encoding.UTF8.GetString(bytesDecoded);

                // Eliminar la clave del texto decodificado
                int indiceClave = textoDecodificado.LastIndexOf(clave);
                if (indiceClave != -1)
                {
                    textoDecodificado = textoDecodificado.Substring(0, indiceClave);
                }

                return textoDecodificado;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// Valida el archivo excel y retorna la cantidad de registros a procesar y la cantidad de columnas.
        /// </summary>
        /// <param name="excelFileBytes"></param>
        /// <returns></returns>
        public static int fnValidarExcel(byte[] excelFileBytes, string[] listcolumnsesperadas)
        {
            int validaciones = 0;

            using (var excelPackage = new ExcelPackage(new MemoryStream(excelFileBytes)))
            {
                string exceptiontext = "";
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var worksheet = excelPackage.Workbook.Worksheets[0];
                int finRows = worksheet.Dimension.Rows;
                int finColumnas = worksheet.Dimension.Columns;
                int NoFilas = 0;
                int NoColumnas = 0;
                List<string> listcolumns = new List<string>();
                //--Se recorren para asegurar la cantidad de registros y columnas
                for (int row = 2; row <= finRows; row++)
                {
                    if (string.IsNullOrEmpty(worksheet.Cells[row, 1].Value?.ToString()))
                    {
                        break;
                    };
                    NoFilas++;
                }

                //----Si no se encontraron registros se lanza una excepcion
                if (NoFilas == 0)
                {
                    exceptiontext = $"No se encontraron registros para cargar, No Filas: {NoFilas}. ";
                }
                
                for (int clm = 1; clm <= finColumnas; clm++)
                {
                    if (string.IsNullOrEmpty(worksheet.Cells[1, clm].Value?.ToString()))
                    {
                        break;
                    };
                    listcolumns.Add(worksheet.Cells[1, clm].Value?.ToString());
                    NoColumnas++;
                }

                //----Si no se encontraron columnas o son diferentes a las oficiales se lanza una excepcion
                if (listcolumnsesperadas.Length != NoColumnas)
                {
                    exceptiontext = $"{exceptiontext}, las columnas no corresponden a las que se esperaban: No Columnas: {NoColumnas}. ,se detectaron estas columnas, [{string.Join(", ", listcolumns)}], se esperaban las siguientes: [{string.Join(", ", listcolumnsesperadas)}]";
                }

                if (listcolumnsesperadas.Length == NoColumnas && !fnSonIguales(listcolumnsesperadas, listcolumns.ToArray()))
                {
                    exceptiontext = $"{exceptiontext}, las columnas no tienen el mismo orden esperado. ";
                }

                if (!string.IsNullOrWhiteSpace(exceptiontext))
                {
                    throw new Exception(exceptiontext);
                }

            }

            return validaciones;
        }

        public static bool fnSonIguales(string[] arr1, string[] arr2)
        {
            if (arr1.Length != arr2.Length)
            {
                return false;
            }

            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr1[i] != arr2[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
