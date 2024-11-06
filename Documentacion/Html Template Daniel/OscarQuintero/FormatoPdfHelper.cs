using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Linq;

namespace CasticketERP.Helpers.Pdf
{
    public class FormatoPdfHelper
    {
        public static byte[] crear(string html, string titulo, string empresa, string nit, string ubicacion, string correo, string web, string qr, string observaciones)
        {
            // Configura tamaño de la hoja
            Rectangle letter = new Rectangle(612, 792);

            // Fija tamaño de la hoja y margenes
            var doc = new Document(PageSize.LETTER, 56, 56, 100, 56);

            MemoryStream memoryStream = new MemoryStream();

            PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream);

            writer.PageEvent = new ITextEvents(titulo, empresa, nit, ubicacion, correo, web);

            doc.Open();

            // Renderiza el documento
            StyleSheet styles = new StyleSheet();
            styles.LoadTagStyle("p", "style", "font-size:11pt;font-weight:normal;line-height:11pt;text-align:justify;");

            var parsedHtmlElements = HTMLWorker.ParseToList(new StringReader(html), styles);

            foreach (var element in parsedHtmlElements)
            {
                doc.Add((IElement)element);
            }

            writer.CloseStream = false;

            // Tabla para mostrar QR
            PdfPTable tabQr = new PdfPTable(4);
            tabQr.SpacingBefore = 10F;
            tabQr.SpacingAfter = 5F;
            tabQr.TotalWidth = 500;
            tabQr.LockedWidth = true; 

            // Captura QR
            Image imagenQr = Image.GetInstance(Convert.FromBase64String(qr));

            PdfPCell cellImage = new PdfPCell();
            cellImage.HorizontalAlignment = Element.ALIGN_CENTER;
            cellImage.VerticalAlignment = Element.ALIGN_MIDDLE;
            cellImage.AddElement(imagenQr);
            cellImage.BorderColor = Color.LIGHT_GRAY;
            cellImage.BorderWidthBottom = 2;
            cellImage.BorderWidthTop = 2;
            cellImage.BorderWidthLeft = 2;
            cellImage.BorderWidthRight = 2;
            cellImage.PaddingBottom = 5;
            cellImage.PaddingTop = 5;
            cellImage.PaddingLeft = 5;
            cellImage.PaddingRight = 5;

            Font fontObservaciones = new Font(Font.HELVETICA, 8, Font.NORMAL);

            Paragraph parrafo = new Paragraph("Observaciones:\n" + observaciones, fontObservaciones);
            parrafo.Alignment = Element.ALIGN_JUSTIFIED;

            PdfPCell cellObservaciones = new PdfPCell();
            cellObservaciones.HorizontalAlignment = Element.ALIGN_JUSTIFIED_ALL;
            cellObservaciones.VerticalAlignment = Element.ALIGN_TOP;
            cellObservaciones.Colspan = 3;
            cellObservaciones.AddElement(parrafo);
            cellObservaciones.Border = 0;
            cellObservaciones.PaddingRight = 10;

            tabQr.AddCell(cellObservaciones);
            tabQr.AddCell(cellImage);

            doc.Add(tabQr);

            doc.Close();

            memoryStream.Position = 0;

            return memoryStream.ToArray();
        }
    }
}