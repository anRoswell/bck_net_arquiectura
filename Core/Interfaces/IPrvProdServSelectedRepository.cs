﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IPrvProdServSelectedRepository
    {
        Task<List<PrvProdServSelected>> GetPrvProdServSelected(int id);
    }
}
