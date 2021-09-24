using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sgart.Net5.ConsoleApp.BO.DTO;
using Sgart.Net5.ConsoleApp.BO.InputDTO;
using Sgart.Net5.WebReactApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sgart.Net5.WebReactApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {

        private readonly ILogger<TodoController> _logger;
        private readonly SgartDIExampleService _service;

        public TodoController(ILogger<TodoController> logger, SgartDIExampleService service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// ritorna tutti gli elementi della tabella
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoDTO>>> Get()
        {
            _logger.LogTrace("Get TodoDTO");

            return Ok(await _service.GetTodoAllAsync());
        }

        /// <summary>
        /// aggiunge un record in tabella
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TodoAddDTO data)
        {
            _logger.LogTrace($"Post TodoDTO: {JsonSerializer.Serialize(data)}");

            await _service.AddTodoAsync(data);

            return NoContent();
        }

        /// <summary>
        /// modifica un record in tabella
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] TodoEditDTO data)
        {
            _logger.LogTrace($"Put TodoDTO: {JsonSerializer.Serialize(data)}");

            if (await _service.EditTodoAsync(data))
                return NoContent();

            return BadRequest();
        }

        /// <summary>
        /// elimina un record in tabella
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery] int todoId)
        {
            _logger.LogTrace($"Delete TodoDTO: {todoId}");

            if (await _service.DeleteTodoAsync(todoId))
                return NoContent();

            return BadRequest();
        }
    }
}
