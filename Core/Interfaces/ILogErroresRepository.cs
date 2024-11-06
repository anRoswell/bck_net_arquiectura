using Core.Entities;
using Core.ModelResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ILogErroresRepository
    {
        Task<List<ResponseAction>> SaveError(LogErrores error);
    }
}
