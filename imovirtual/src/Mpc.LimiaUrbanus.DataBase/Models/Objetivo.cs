using System;
using System.Collections.Generic;

namespace Mpc.LimiaUrbanus.DataBase.Models
{
    public partial class Objetivo
    {
        public Objetivo()
        {
            Imovel = new HashSet<Imovel>();
        }

        public int ObjetivoId { get; set; }
        public string Nome { get; set; }

        public ICollection<Imovel> Imovel { get; set; }
    }
}
