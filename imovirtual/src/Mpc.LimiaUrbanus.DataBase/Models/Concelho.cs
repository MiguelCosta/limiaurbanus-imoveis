using System;
using System.Collections.Generic;

namespace Mpc.LimiaUrbanus.DataBase.Models
{
    public partial class Concelho
    {
        public Concelho()
        {
            Freguesia = new HashSet<Freguesia>();
        }

        public int ConcelhoId { get; set; }
        public string Nome { get; set; }
        public int DistritoId { get; set; }

        public Distrito Distrito { get; set; }
        public ICollection<Freguesia> Freguesia { get; set; }
    }
}
