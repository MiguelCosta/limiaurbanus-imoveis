﻿namespace Mpc.LimiaUrbanus.Services
{
    using System.Collections.Generic;
    using Mpc.LimiaUrbanus.DataBase.Models;

    public interface IXmlGenerator
    {
        string Generate(IEnumerable<Imovel> imoveis);
    }
}
