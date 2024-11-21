using MicroservicesDomain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StackExchange.Redis;
using MicroservicesRepository.Interfaces;

namespace MicroservicesEnergia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumoController : ControllerBase
    {
        private readonly IConsumoRepository _repository;

        public ConsumoController(IConsumoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetConsumo()
        {
            /*string key = "getconsumo";
            redis = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase db = redis.GetDatabase();
            await db.KeyExpireAsync(key, TimeSpan.FromSeconds(10));
            string user = await db.StringGetAsync(key);

            if (!string.IsNullOrEmpty(user))
            {
                return Ok(user);
            }*/

            var consumos = await _repository.ListarConsumos();

            if (consumos == null)
            {
                return NotFound();
            }

            string consumosJson = JsonConvert.SerializeObject(consumos);
            // await db.StringSetAsync(key, consumosJson);

            return Ok(consumos);
        }

        [HttpPost]
        public async Task<IActionResult> PostConsumo([FromBody] Consumo consumo)
        {
            await _repository.SalvarConsumo(consumo);

            //apaga o cache
            /*string key = "getconsumo";
            redis = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase db = redis.GetDatabase();
            await db.KeyDeleteAsync(key);*/

            return Ok(new { mensagem = "Criado com sucesso!" });

        }
    }
}
