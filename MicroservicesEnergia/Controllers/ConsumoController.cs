using MicroservicesDomain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StackExchange.Redis;
using MicroservicesRepository.Interfaces;

namespace MicroservicesEnergia.Controllers
{
    [Route("api")]
    [ApiController]
    public class ConsumoController : ControllerBase
    {
        private static ConnectionMultiplexer redis;
        private readonly IConsumoRepository _repository;

        public ConsumoController(IConsumoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("health")]
        public async Task<IActionResult> GetConsumo()
        {
            string key = "getconsumo";
            redis = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase db = redis.GetDatabase();

            try
            {
                await db.KeyExpireAsync(key, TimeSpan.FromSeconds(10));
                string user = await db.StringGetAsync(key);

                if (!string.IsNullOrEmpty(user))
                {
                    return Ok(user);
                }

                var consumos = await _repository.ListarConsumos();

                if (consumos == null)
                {
                    return NotFound();
                }

                string consumosJson = JsonConvert.SerializeObject(consumos);
                await db.StringSetAsync(key, consumosJson);

                return Ok(consumos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro no servidor." });
            }
        }

        [HttpPost("consumo")]
        public async Task<IActionResult> PostConsumo([FromBody] Consumo consumo)
        {
            if (consumo == null)
            {
                return BadRequest(new { mensagem = "Dados inválidos. " });
            }

            try
            {
                await _repository.SalvarConsumo(consumo);

                string key = "getconsumo";
                redis = ConnectionMultiplexer.Connect("localhost:6379");
                IDatabase db = redis.GetDatabase();
                await db.KeyDeleteAsync(key);

                return CreatedAtAction(nameof(GetConsumo), new { id = consumo.Id }, new { mensagem = "Consumo criado com sucesso!" });
            } catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = "Erro no servidor." });
            }

        }
    }
}
