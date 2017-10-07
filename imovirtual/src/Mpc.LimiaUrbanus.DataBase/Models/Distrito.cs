using System;
using System.Collections.Generic;

namespace Mpc.LimiaUrbanus.DataBase.Models
{
    public partial class Distrito
    {
        public Distrito()
        {
            Concelho = new HashSet<Concelho>();
        }

        public int DistritoId { get; set; }
        public string Nome { get; set; }

        public ICollection<Concelho> Concelho { get; set; }
    }
}
