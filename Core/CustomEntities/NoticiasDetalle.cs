using Core.Entities;
using System.Collections.Generic;

namespace Core.CustomEntities
{
    public class NoticiasDetalle
    {
        public List<Noticia> Noticia { get; set; }
        public List<NoticiasDoc> NoticiasDocs { get; set; }
    }
}
