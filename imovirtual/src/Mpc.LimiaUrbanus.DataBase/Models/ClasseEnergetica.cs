using System;
using System.Collections.Generic;

namespace Mpc.LimiaUrbanus.DataBase.Models
{
    public partial class ClasseEnergetica
    {
        public ClasseEnergetica()
        {
            Imovel = new HashSet<Imovel>();
        }

        public int ClasseEnergeticaId { get; set; }
        public string Nome { get; set; }
        public int Ordem { get; set; }

        public ICollection<Imovel> Imovel { get; set; }
    }
}
