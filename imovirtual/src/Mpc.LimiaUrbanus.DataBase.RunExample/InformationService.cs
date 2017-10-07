namespace Mpc.LimiaUrbanus.DataBase.RunExample
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Mpc.LimiaUrbanus.DataBase.Models;
    using Newtonsoft.Json;

    public class InformationService : IInformationService
    {
        private readonly LimiaUrbanusContext _dataBase;
        private readonly ILogger<InformationService> _logger;

        public InformationService(ILoggerFactory loggerFactory, LimiaUrbanusContext dataBase)
        {
            _logger = loggerFactory.CreateLogger<InformationService>();
            _dataBase = dataBase;
        }

        public void GetDistritos()
        {
            _logger.LogInformation("DISTRITOS");
            var distritos = _dataBase.Distrito
                .Include(d => d.Concelho).ThenInclude(c => c.Freguesia)
                .ToList();
            Print(distritos);
        }

        public void GetImoveis()
        {
            _logger.LogInformation("IMOVEIS");
            var imoveis = _dataBase.Imovel.ToList();
            Print(imoveis);
        }

        private void Print(object obj)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            _logger.LogInformation(json);
        }
    }
}
