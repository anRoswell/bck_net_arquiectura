using Core.CustomEntities;
using Core.Entities;
using Core.QueryFilters;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IReportingService
    {
        Task<MemoryStream> CertificadoExperiencia(QuerySearchCertificados parameters);
        Task<MemoryStream> CertificadoEspecialPdf(int id);
        Task<MemoryStream> FacturaPorPagarPdf(QuerySearchEstadoCuentasXPagarDetalle parameters);
        Task<MemoryStream> FacturaPorPagarXlsx(QuerySearchEstadoCuentasXPagarDetalle parameters);
        Task<MemoryStream> FacturaPagadaPdf(GenerateFactPag param);
        Task<MemoryStream> FacturaPagadaXlsx(GenerateFactPag param);
        Task<MemoryStream> Prueba();
        

        Task<string> CertificadoRetencionFuentePdf(QuerySearchCertificados parameters);
        Task<string> CertificadoRetencionIVAPdf(QuerySearchCertificados parameters);
        Task<string> CertificadoRetencionIcaPdf(QuerySearchCertificados parameters);
        Task<(string, byte[])> CertificadoRetencionIca2Pdf(QuerySearchCertificados parameters);
        Task<string> CertificadoRetencionEstampillaBoyacaPdf(QuerySearchCertificados parameters);
    }
}
