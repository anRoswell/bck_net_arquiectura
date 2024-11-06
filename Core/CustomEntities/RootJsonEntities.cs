using System.Collections.Generic;

namespace Core.CustomEntities
{
    public class RootJsonResponse<T> where T : class
    {
        public List<T> JsonResponse { get; set; }
    }
}
