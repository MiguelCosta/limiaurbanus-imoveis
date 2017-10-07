using System;
using System.Collections.Generic;

namespace Mpc.LimiaUrbanus.DataBase.Models
{
    public partial class Freguesia
    {
        public Freguesia()
        {
            Imovel = new HashSet<Imovel>();
        }

        public int FreguesiaId { get; set; }
        public string Nome { get; set; }
        public int ConcelhoId { get; set; }

        public Concelho Concelho { get; set; }
        public ICollection<Imovel> Imovel { get; set; }
    }
}
