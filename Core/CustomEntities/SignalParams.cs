using Core.HubConfig;
using Microsoft.AspNetCore.SignalR;

namespace Core.CustomEntities
{
    public class SignalParams<T,D>  where T : Hub where D : class
    {
        public IHubContext<T> HubContext { get; set; }
        public DataModelSignalResponse<D> DataModel { get; set; }
    }
}
