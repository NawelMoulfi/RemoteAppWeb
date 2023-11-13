using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories.Contracts;
using Shared.Dto;

namespace RemoteAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleRepository _moduleRepository;

        public ModuleController(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetModules()
        {
            return Ok(await _moduleRepository.GetAllModuls());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetModuleById(int id)
        {
            return Ok(await _moduleRepository.GetModuleById(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateModule([FromBody] ModuleDto module)
        {
            if (module == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdModule = await _moduleRepository.AddModule(module);
            var jsonModule = System.Text.Json.JsonSerializer.Serialize(createdModule);
            Console.WriteLine($" Module response in json manuale  : {Content(jsonModule, "application/json")}");
            Console.WriteLine($" Module response in json   : {Created("Module", createdModule)}");

            // Return the JSON string as content with the appropriate content type
            ///return Content(jsonModule, "application/json");

            return Created("Module", createdModule);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateModule([FromBody] ModuleDto module)
        {
            if (module == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entryToUpdate = await _moduleRepository.GetModuleById(module.ModuleID);

            if (entryToUpdate == null)
                return NotFound();

            await _moduleRepository.UpdateModule(module);

            return NoContent(); //success
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            if (id == 0)
                return BadRequest();

            var moduleToDelete = await _moduleRepository.GetModuleById(id);
            if (moduleToDelete == null)
                return NotFound();

            await _moduleRepository.DeleteModule(id);

            return NoContent();//success
        }
    }
}
