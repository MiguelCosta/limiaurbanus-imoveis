using System;
using System.Collections.Generic;

namespace Mpc.LimiaUrbanus.DataBase.Models
{
    public partial class Tipo
    {
        public Tipo()
        {
            Imovel = new HashSet<Imovel>();
        }

        public int TipoId { get; set; }
        public string Nome { get; set; }

        public ICollection<Imovel> Imovel { get; set; }
    }
}
