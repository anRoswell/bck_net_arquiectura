using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IPrvEmpresasSelectedService
    {
        Task<List<PrvEmpresasSelected>> GetEmpresasSelected(int id);
    }
}
