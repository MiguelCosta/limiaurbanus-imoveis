using System;
using System.Collections.Generic;

namespace Mpc.LimiaUrbanus.DataBase.Models
{
    public partial class Estado
    {
        public Estado()
        {
            Imovel = new HashSet<Imovel>();
        }

        public int EstadoId { get; set; }
        public string Nome { get; set; }

        public ICollection<Imovel> Imovel { get; set; }
    }
}
