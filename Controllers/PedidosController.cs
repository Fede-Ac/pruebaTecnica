using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using pruebaTecnicaItalmundo.DTOs;
using System.Data;

namespace pruebaTecnicaItalmundo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public PedidosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("clientes")]
        public async Task<IActionResult> ObtenerClientes()
        {
            var lista = new List<ClienteDto>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (var command = new SqlCommand("sp_ObtenerClientes", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lista.Add(new ClienteDto
                            {
                                Id = reader.GetInt32(0),
                                Nombre = reader.GetString(1)
                            });
                        }
                    }
                }
            }

            return Ok(lista);
        }

        [HttpGet("pedidos/{clienteId}")]
        public async Task<IActionResult> ObtenerPedidos(int clienteId)
        {
            var lista = new List<PedidoDto>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (var command = new SqlCommand("sp_ObtenerPedidosPorCliente", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@ClienteId", clienteId));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lista.Add(new PedidoDto
                            {
                                Id = reader.GetInt32(0),
                                Cliente = reader.GetString(1),
                                Fecha = reader.GetDateTime(2),
                                Total = reader.GetDecimal(3)
                            });
                        }
                    }
                }
            }

            return Ok(lista);
        }

        [HttpGet("ultimo-pedido/{clienteId}")]
        public async Task<IActionResult> ObtenerUltimoPedido(int clienteId)
        {
            PedidoDto? pedido = null;

            using var connection = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")
            );

            using var command = new SqlCommand("sp_ObtenerUltimoPedido", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(
                new SqlParameter("@ClienteId", clienteId)
            );

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                pedido = new PedidoDto
                {
                    Id = reader.GetInt32(0),
                    Fecha = reader.GetDateTime(1),
                    Total = reader.GetDecimal(2)
                };
            }

            if (pedido == null)
                return NotFound("El cliente no tiene pedidos");

            return Ok(pedido);
        }

        /*
                // GET: api/<PedidosController>
                [HttpGet]
                public IEnumerable<string> Get()
                {
                    return new string[] { "value1", "value2" };
                }

                // GET api/<PedidosController>/5
                [HttpGet("{id}")]
                public string Get(int id)
                {
                    return "value";
                }

                // POST api/<PedidosController>
                [HttpPost]
                public void Post([FromBody] string value)
                {
                }

                // PUT api/<PedidosController>/5
                [HttpPut("{id}")]
                public void Put(int id, [FromBody] string value)
                {
                }

                // DELETE api/<PedidosController>/5
                [HttpDelete("{id}")]
                public void Delete(int id)
                {
                }
        */
    }
}
