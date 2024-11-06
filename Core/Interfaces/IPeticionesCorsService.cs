using Core.Entities;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IPeticionesCorsService
    {
        Task RegisterLog(PeticionesCors peticionesCors);
        Task RegisterLogOracle(gnl_peticiones_cors gnl_peticiones_cors);        
    }
}
