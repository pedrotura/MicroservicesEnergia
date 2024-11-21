using MicroservicesDomain;

namespace MicroservicesRepository.Interfaces
{
    public interface IConsumoRepository
    {
        Task<IEnumerable<Consumo>> ListarConsumos();
        Task SalvarConsumo(Consumo consumo);
    }
}