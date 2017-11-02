namespace Mpc.LimiaUrbanus.DataBase.RunExample
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Mpc.LimiaUrbanus.DataBase.Models;
    using Mpc.LimiaUrbanus.Services;
    using Newtonsoft.Json;

    public class InformationService : IInformationService
    {
        private readonly LimiaUrbanusContext _dataBase;
        private readonly ILogger<InformationService> _logger;
        private readonly IImovirtualXmlGenerator _xmlGenerator;

        public InformationService(
            ILoggerFactory loggerFactory,
            IImovirtualXmlGenerator xmlGenerator,
            LimiaUrbanusContext dataBase)
        {
            _logger = loggerFactory.CreateLogger<InformationService>();
            _xmlGenerator = xmlGenerator;
            _dataBase = dataBase;
        }

        public async Task GetDistritosAsync()
        {
            _logger.LogInformation("DISTRITOS");
            var distritos = await _dataBase.Distrito
                .Include(d => d.Concelho).ThenInclude(c => c.Freguesia)
                .ToListAsync();
            Print(distritos);
        }

        public async Task GetImoveisAsync()
        {
            _logger.LogInformation("IMOVEIS");
            var imoveis = await _dataBase.Imovel.ToListAsync();
            Print(imoveis);
        }

        public async Task<string> GetXmlFromImoveisAsync(IEnumerable<string> referencias)
        {
            var imoveisQuery = _dataBase.Imovel
                .Include(i => i.ClasseEnergetica)
                .Include(i => i.Estado)
                .Include(i => i.FilePath)
                .Include(i => i.Freguesia).ThenInclude(f => f.Concelho).ThenInclude(c => c.Distrito)
                .Include(i => i.Objetivo)
                .Include(i => i.Tipo)
                .Include(i => i.Tipologia)
                .AsQueryable();

            if (referencias != null && referencias.Any())
            {
                imoveisQuery = imoveisQuery.Where(i => referencias.Contains(i.Referencia));
            }

            var imoveis = await imoveisQuery.ToListAsync();
            return _xmlGenerator.Generate(imoveis);
        }

        public async Task SaveXmlTextToFileAsync(string xmlText)
        {
            await System.IO.File.WriteAllTextAsync("LimiaUrbanus.xml", xmlText);
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
