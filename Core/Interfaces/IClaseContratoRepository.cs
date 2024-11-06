using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IClaseContratoRepository 
    {
        Task<List<ClaseContrato>> SearchAll();
    }
}
