using Microsoft.AspNetCore.Http;

namespace Core.QueryFilters
{
    public class TipoMinutaCreateCommand
    {
        public string Nombre {  get; set; }
        public IFormFile File { get; set; }
    }
}
