
namespace Core.Interfaces
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public interface IControllerInit<M, Q, F>
    {
        Task<IActionResult> SearchAll();
        Task<IActionResult> SearchById(Q par);
        Task<IActionResult> ParametrosIniciales();
        Task<IActionResult> Update([FromBody] M contrato);
        Task<IActionResult> Delete([FromForm] Q par);
        Task<IActionResult> Post([FromBody] M contrato);
        Task<IActionResult> UpdateEstado([FromBody] Q par);
        Task<IActionResult> FilterEstado([FromBody] F par);
    }
}
