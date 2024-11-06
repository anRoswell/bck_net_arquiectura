using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class ParametrosNoticias
    {
        public List<Empresa> Empresas { get; set; }
        public List<Noticia> Noticias { get; set; }
        public List<TiposPlantilla> TiposPlantillas { get; set; }
        public List<TipoNoticia> TiposNoticia { get; set; }
        public List<Alcance> Alcance { get; set; }

    }
}
