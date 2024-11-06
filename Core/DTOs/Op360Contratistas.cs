using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class Op360Contratistas
    {
        public Contratista[] Contratista { get; set; }
    }

    public class Contratista
    {
        public int id_contratista { get; set; }
        public string identificacion { get; set; }
    }
}
