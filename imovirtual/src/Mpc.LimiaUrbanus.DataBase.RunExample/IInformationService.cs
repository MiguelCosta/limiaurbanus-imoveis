namespace Mpc.LimiaUrbanus.DataBase.RunExample
{
    using System.Threading.Tasks;

    public interface IInformationService
    {
        Task GetDistritosAsync();

        Task GetImoveisAsync();

        Task<string> GetXmlFromImoveisAsync();

        Task SaveXmlTextToFileAsync(string xmlText);
    }
}
