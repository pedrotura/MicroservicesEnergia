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

        public ConsumoRepository(IOptions<MongoSettings> mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.Value.connectionString);
            var database = client.GetDatabase(mongoDbSettings.Value.databaseName);
            _consumoCollection = database.GetCollection<Consumo>(mongoDbSettings.Value.collectionName);
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
