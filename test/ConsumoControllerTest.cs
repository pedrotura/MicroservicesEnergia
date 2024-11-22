using Moq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MicroservicesRepository.Interfaces;
using MicroservicesDomain;
using MicroservicesEnergia.Controllers;

namespace test
{
    public class ConsumoControllerTest
    {
        private readonly Mock<IConsumoRepository> _consumoRepositoryMock;
        private readonly ConsumoController _controller;

        public ConsumoControllerTest()
        {
            _consumoRepositoryMock = new Mock<IConsumoRepository>();
            _controller = new ConsumoController(_consumoRepositoryMock.Object);
        }

        [Fact]
        public async Task Get_ListarConsumosOk()
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
                }
            };
            _consumoRepositoryMock.Setup(c => c.ListarConsumos()).ReturnsAsync(consumos);

            //act
            var result = await _controller.GetConsumo();

            //assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(JsonConvert.SerializeObject(consumos), JsonConvert.SerializeObject(okResult.Value));
        }

        [Fact]
        public async Task Get_ListarConsumosNotFound()
        {
            //arrange
            _consumoRepositoryMock.Setup(c => c.ListarConsumos()).ReturnsAsync((IEnumerable<Consumo>)null);

            //act
            var result = await _controller.GetConsumo();

            //assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Post_SalvarConsumo()
        {
            //arrange
            var consumo = new Consumo()
            {
                Id = 1,
                Status = "Normal",
                ConsumoKwh = 1200.0,
                DataMedicao = "21/11/2024",
                LocalMedicao = "São Paulo, SP"
            };
            _consumoRepositoryMock.Setup(c => c.SalvarConsumo(It.IsAny<Consumo>())).Returns(Task.CompletedTask);

            //act
            var result = await _controller.PostConsumo(consumo);

            //assert
            Assert.IsType<OkObjectResult>(result);
            _consumoRepositoryMock.Verify(c => c.SalvarConsumo(It.IsAny<Consumo>()), Times.Once);

        }

    }
}
