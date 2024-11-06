using Core.CustomEntities;
using Core.ModelResponse;
using Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAdobeSignService
    {
        public string Tipo_Agreement { get; set; }
        /// <summary>
        /// Método para subir archivo a Adobe Sign
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="idProveedor"></param>
        /// <returns></returns>
        Task<TransientDocument> TransientDocumentsAsync(string filePath, int id = 0);
        /// <summary>
        /// Método para enviar documento a ciertos correos para recolectar firmas
        /// </summary>
        /// <param name="transientDocumentId"></param>
        /// <param name="NombreArchivo"></param>
        /// <param name="emailProveedor"></param>
        /// <param name="idProveedor"></param>
        /// <returns></returns>
        Task<AgreementsResponse> AgreementsAsync(string transientDocumentId, string nombreAcuerdo, string emailProveedor, int id = 0, string emailRteLegal = "");
        /// <summary>
        /// Método para descargar documento de Adobe Sign
        /// </summary>
        /// <param name="queryDownload"></param>
        /// <param name="pathFs"></param>
        /// <returns></returns>
        Task<bool> DownloadDocumentAsync(QueryDownloadPdfAdb queryDownload, string pathFs, int IdPathFS);
        /// <summary>
        /// Método para validar algún documento en Adobe Sign
        /// </summary>
        /// <param name="idDocument"></param>
        /// <returns></returns>
        Task<ValidateDocument> ValidateDocumentAsync(string idDocument);
    }
}
