using Core.CustomEntities;
using Core.Entities;
using System.Collections.Generic;

namespace Core.Services
{
    public class PerfilPaginador : ModeloPaginador
    {
        public List<Perfil> perfiles { get; set; }
    }
}
