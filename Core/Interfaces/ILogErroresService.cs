namespace Core.Interfaces
{
    using Core.Entities;
    using Core.ModelResponse;
    using System.Threading.Tasks;

    public interface ILogErroresService
    {
        Task<ResponseAction> SaveError(LogErrores error);
    }
}