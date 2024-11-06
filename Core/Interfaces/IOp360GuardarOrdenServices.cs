using Core.Entities;
using Core.ModelResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IOp360GuardarOrdenServices
    {
        Task<List<ResponseAction>> CrearOrden(Op360GuardarOrden guardarOrden);
    }
}
