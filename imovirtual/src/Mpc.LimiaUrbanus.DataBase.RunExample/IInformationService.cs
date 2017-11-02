namespace Mpc.LimiaUrbanus.DataBase.RunExample
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IInformationService
    {
        Task GetDistritosAsync();

        Task GetImoveisAsync();

        Task<string> GetXmlFromImoveisAsync(IEnumerable<string> referencias);

        Task SaveXmlTextToFileAsync(string xmlText);
    }
}
