using System;
using System.Collections.Generic;

namespace Mpc.LimiaUrbanus.DataBase.Models
{
    public partial class Imovel
    {
        public Imovel()
        {
            FilePath = new HashSet<FilePath>();
        }

        public int ImovelId { get; set; }
        public string Nome { get; set; }
        public string Referencia { get; set; }
        public string Descricao { get; set; }
        public int TipoId { get; set; }
        public int EstadoId { get; set; }
        public int FreguesiaId { get; set; }
        public string Morada { get; set; }
        public string CoordenadasGps { get; set; }
        public int ObjetivoId { get; set; }
        public int? ClasseEnergeticaId { get; set; }
        public double Area { get; set; }
        public int? Wc { get; set; }
        public int? TipologiaId { get; set; }
        public double Preco { get; set; }
        public string ContactoResponsavel { get; set; }
        public bool IsDestaque { get; set; }
        public bool? IsOportunidade { get; set; }
        public bool? IsAtivo { get; set; }

        public ClasseEnergetica ClasseEnergetica { get; set; }
        public Estado Estado { get; set; }
        public Freguesia Freguesia { get; set; }
        public Objetivo Objetivo { get; set; }
        public Tipo Tipo { get; set; }
        public Tipologia Tipologia { get; set; }
        public ICollection<FilePath> FilePath { get; set; }
    }
}
