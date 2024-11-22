using Dapper;
using MicroservicesDomain;
using MicroservicesRepository.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MicroservicesRepository
{
    public class ConsumoRepository : IConsumoRepository
    {
        private readonly IMongoCollection<Consumo> _consumoCollection;

        public ConsumoRepository(IOptions<MongoSettings> mongoSettings)
        {
            var client = new MongoClient(mongoSettings.Value.connectionString);
            var database = client.GetDatabase(mongoSettings.Value.databaseName);
            _consumoCollection = database.GetCollection<Consumo>(mongoSettings.Value.collectionName);
        }

        public async Task<IEnumerable<Consumo>> ListarConsumos()
        {
            var consumos = await _consumoCollection.Find(consumo => true).ToListAsync();
            return consumos;
        }

        public async Task SalvarConsumo(Consumo consumo)
        {
            await _consumoCollection.InsertOneAsync(consumo);
        }
    }
}
