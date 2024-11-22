using MicroservicesDomain;
using MicroservicesRepository.Interfaces;
using Moq;
using Xunit;

namespace test
{
    public class ConsumoRepositoryTest
    {
        [Fact]
        public async Task ListarConsumo()
        {
            //arrange
            var consumos = new List<Consumo>()
            {
                new Consumo()
                {
                    Id = 1,
                    Status = "Normal",
                    ConsumoKwh = 1200.0,
                    DataMedicao = "21/11/2024",
                    LocalMedicao = "São Paulo, SP"

                },
                new Consumo()
                {
                    Id = 2,
                    Status = "Excessivo",
                    ConsumoKwh = 8000.0,
                    DataMedicao = "20/11/2024",
                    LocalMedicao = "São Paulo, SP"
                }
            };
            var consumoRepositoryMock = new Mock<IConsumoRepository>();
            consumoRepositoryMock.Setup(c => c.ListarConsumos()).ReturnsAsync(consumos);
            var consumoRepository = consumoRepositoryMock.Object;

            //act
            var result = await consumoRepository.ListarConsumos();

            //assert
            Assert.Equal(consumos, result);
        }

        [Fact]
        public async Task SalvarConsumo()
        {
            //arrange
            Consumo consumo = new Consumo()
            {
                Id = 1,
                Status = "Normal",
                ConsumoKwh = 1200.0,
                DataMedicao = "21/11/2024",
                LocalMedicao = "São Paulo, SP"
            };

            var consumoRepositoryMock = new Mock<IConsumoRepository>();
            consumoRepositoryMock.Setup(c => c.SalvarConsumo(consumo)).Returns(Task.CompletedTask);
            var consumoRepository = consumoRepositoryMock.Object;

            //act
            await consumoRepository.SalvarConsumo(consumo);

            //assert
            consumoRepositoryMock.Verify(c => c.SalvarConsumo(consumo), Times.Once);
            Assert.True(true);


        }
    }
}
