using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface INotTipoPlantilla
    {
        Task<List<TiposPlantilla>> GetTipoPlantillaSelected(int id);
    }
}
