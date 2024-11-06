
namespace Core.Services
{
    using Core.CustomEntities;
    using Core.Entities;
    using Core.Interfaces;
    using Core.ModelResponse;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class NoticiaService : INoticiaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NoticiaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Consultamos los parametros iniciales de la vista listNoticias
        /// </summary>
        /// <returns>Model ParametrosNoticias</returns>

        public async Task<ParametrosNoticias> ParametrosNoticias(string Operacion, string CodUser)
        {
            ParametrosNoticias parametrosNoticias = new ParametrosNoticias
            {
                Empresas = await _unitOfWork.NoticiaRepository.GetEmpresas(),
                Noticias = await _unitOfWork.NoticiaRepository.GetNoticias(Operacion, CodUser),
                TiposPlantillas = await _unitOfWork.NoticiaRepository.GetTiposPlantillas(),
                TiposNoticia = await _unitOfWork.NoticiaRepository.GetTipoNoticia(),
                Alcance = await _unitOfWork.NoticiaRepository.GetAlcance(),
            };

            return parametrosNoticias;
        }

        public async Task<List<Noticia>> GetNoticias(string Operacion, string CodUser)
        {
            return await _unitOfWork.NoticiaRepository.GetNoticias(Operacion, CodUser);
        }

        public async Task<List<TiposPlantilla>> GetTiposPlantillas()
        {
            return await _unitOfWork.NoticiaRepository.GetTiposPlantillas();
        }

        public async Task<List<Alcance>> GetAlcance()
        {
            return await _unitOfWork.NoticiaRepository.GetAlcance();
        }

        public async Task<ParametrosNoticias> GetParametros(int idNoticia, string CodUser, string Operacion)
        {
            ParametrosNoticias paramsNot = new ParametrosNoticias
            {
                Empresas = await _unitOfWork.NoticiaRepository.GetEmpresas(),
                Noticias = await _unitOfWork.NoticiaRepository.GetNoticias(Operacion, CodUser),
                TiposPlantillas = await _unitOfWork.NoticiaRepository.GetTiposPlantillas(),
                TiposNoticia = await _unitOfWork.NoticiaRepository.GetTipoNoticia(),
                Alcance = await _unitOfWork.NoticiaRepository.GetAlcance(),
            };
            return paramsNot;
        }

        public async Task<NoticiasDetalle> GetNoticiaPorID(int id)
        {
            NoticiasDetalle noticiasDetalle = new NoticiasDetalle()
            {
                Noticia = await _unitOfWork.NoticiaRepository.GetNoticiaPorID(id),
                NoticiasDocs = await _unitOfWork.NoticiasDocRepository.GetDocumentosNoticia(id)
            };
            return noticiasDetalle;
        }

        public async Task<List<ResponseAction>> PostNoticia(Noticia noticia, List<NoticiasDoc> listDocuments)
        {
            return await _unitOfWork.NoticiaRepository.PostNoticia(noticia, listDocuments);
        }

        public async Task<List<ResponseAction>> PutNoticia(Noticia noticia, List<NoticiasDoc> listDocuments)
        {
            return await _unitOfWork.NoticiaRepository.PutNoticia(noticia, listDocuments);
        }

        public async Task<List<ResponseAction>> DeleteNoticia(Noticia noticia)
        {
            return await _unitOfWork.NoticiaRepository.DeleteNoticia(noticia);
        }
    }
}
