using System;
using System.Collections.Generic;

namespace Mpc.LimiaUrbanus.DataBase.Models
{
    public partial class FilePath
    {
        public int FilePathId { get; set; }
        public string FileName { get; set; }
        public int FileTye { get; set; }
        public int ImovelId { get; set; }
        public bool IsPrincipal { get; set; }
        public bool IsCapa { get; set; }

        public Imovel Imovel { get; set; }
    }
}
