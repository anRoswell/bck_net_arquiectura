using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Core.HubConfig
{
    public class TestHub : Hub
    {
        public async Task AskServer(string someTextFromClient)
        {
            string tempString;

            if (someTextFromClient == "prueba")
            {
                tempString = "Es 'prueba'";
            }
            else
            {
                tempString = $"No es 'prueba', fue '{someTextFromClient}'";
            }

            // Enviar respuesta a uno o mas clientes especifico, como ejemplo al mismo cliente que solicita
            await Clients.Client(this.Context.ConnectionId).SendAsync("AskServerResponse", tempString);

            // await Clients.Client(this.Context.ConnectionId, this.Context.ConnectionId).SendAsync("askServerResponse", tempString);

            // Enviar respuesta a todos menos al que ejecuta la solicitud
            //await Clients.Others.SendAsync("askServerResponse", tempString, arg2, arg3);

            // Enviar respuesta a todos incluyendo a quien ejecuta la accion
            //await Clients.All.SendAsync("askServerResponse", tempString, arg2, arg3);

            // Enviar respuesta al mismo cliente que solicita
            //await Clients.Caller.SendAsync("askServerResponse", tempString, arg2, arg3);
        }
    }
}
