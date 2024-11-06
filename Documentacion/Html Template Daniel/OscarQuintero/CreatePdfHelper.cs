using CasticketERP.Models.Parametros.configuracion;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;

namespace CasticketERP.Helpers.Pdf
{
    public class ITextEvents : PdfPageEventHelper
    {
        private string _titulo, _empresa, _nit, _ubicacion, _correo, _web;

        private Font _font = new Font(Font.HELVETICA, 8, Font.NORMAL);

        private PdfTemplate _totalPages;
        private PdfContentByte _cb;
        private BaseFont _bf = null;

        public ITextEvents(string titulo, string empresa, string nit, string ubicacion, string correo, string web)
        {
            _titulo = titulo;
            _empresa = empresa;
            _nit = nit;
            _ubicacion = ubicacion;
            _correo = correo;
            _web = web;
        }

        // write on top of document
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);

            _bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            _cb = writer.DirectContent;
            _totalPages = _cb.CreateTemplate(500, 50);
        }

        // write on start of each page
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);

            PdfPTable tabHeader = new PdfPTable(4);
            tabHeader.SpacingAfter = 5F;
            tabHeader.TotalWidth = 500;
            tabHeader.LockedWidth = true;
            tabHeader.DefaultCell.Border = 0;

            var pathLogoHeader = new ModelConfiguracion().logo_path;

            Image logoHeader = Image.GetInstance(pathLogoHeader);

            PdfPCell cellImage = new PdfPCell();
            cellImage.HorizontalAlignment = Element.ALIGN_CENTER;
            cellImage.VerticalAlignment = Element.ALIGN_MIDDLE;
            cellImage.AddElement(logoHeader);
            cellImage.Border = 0;

            tabHeader.AddCell(cellImage);

            Font fontTitle = new Font(Font.HELVETICA, 10, Font.NORMAL);

            PdfPCell cellTitle = new PdfPCell(new Phrase(_titulo, fontTitle));
            cellTitle.HorizontalAlignment = Element.ALIGN_CENTER;
            cellTitle.VerticalAlignment = Element.ALIGN_MIDDLE;
            cellTitle.Colspan = 2;
            cellTitle.Border = 0;
            tabHeader.AddCell(cellTitle);

            Font fontInfo = new Font(Font.HELVETICA, 6, Font.NORMAL);

            Paragraph parrafo = new Paragraph("Fecha: " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss"), fontInfo);
            parrafo.Alignment = Element.ALIGN_RIGHT;

            PdfPCell cellInfo = new PdfPCell();
            cellInfo.HorizontalAlignment = Element.ALIGN_RIGHT;
            cellInfo.VerticalAlignment = Element.ALIGN_MIDDLE;
            cellInfo.AddElement(parrafo);
            cellInfo.Border = 0;

            tabHeader.AddCell(cellInfo);

            tabHeader.WriteSelectedRows(0, -1, 56, document.PageSize.Height - 10, writer.DirectContent);
        }

        // write on end of each page
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            String text = "Página " + writer.PageNumber + " de ";

            _cb.BeginText();
            _cb.SetFontAndSize(_bf, 6);
            _cb.SetTextMatrix(document.PageSize.GetRight(89), document.PageSize.GetBottom(56));
            _cb.ShowText(text);
            _cb.EndText();
            float len = _bf.GetWidthPoint(text, 6);

            _cb.AddTemplate(_totalPages, document.PageSize.GetRight(89) + len, document.PageSize.GetBottom(56));

            Font fontInfo = new Font(Font.HELVETICA, 8, Font.NORMAL);

            string text_foot = _empresa + "\n" +
                               "Nit:" + _nit + "\n" +
                               _ubicacion + "\n" +
                               _correo + "\n" +
                               _web;

            PdfPTable tabFooter = new PdfPTable(1);
            tabFooter.TotalWidth = 500;
            tabFooter.DefaultCell.Border = 0;
            tabFooter.AddCell(new Phrase(text_foot, fontInfo));
            tabFooter.WriteSelectedRows(0, -1, 56, document.Bottom + 10, writer.DirectContent);
        }

        //write on close of document
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            _totalPages.BeginText();
            _totalPages.SetFontAndSize(_bf, 6);
            _totalPages.SetTextMatrix(0, 0);
            _totalPages.ShowText((writer.PageNumber - 1).ToString());
            _totalPages.EndText();
        }
    }
}