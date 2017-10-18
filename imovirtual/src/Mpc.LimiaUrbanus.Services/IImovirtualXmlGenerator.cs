namespace Mpc.LimiaUrbanus.Services
{
    using System.Collections.Generic;
    using Mpc.LimiaUrbanus.DataBase.Models;

    public interface IImovirtualXmlGenerator
    {
        string Generate(IEnumerable<Imovel> imoveis);
    }
}
