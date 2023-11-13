using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories.Contracts;
using Shared.Dto;

namespace RemoteAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleActionRoleController : ControllerBase
    {
        private readonly IModuleActionRoleRepository _moduleActionRoleRepository;

        public ModuleActionRoleController(IModuleActionRoleRepository moduleActionRoleRepository)
        {
            _moduleActionRoleRepository = moduleActionRoleRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetModuleActionRoles()
        {
            return Ok(await _moduleActionRoleRepository.GetAllModulActionRoles());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetModuleById(int id)
        {
            return Ok(await _moduleActionRoleRepository.GetModuleActionRoleById(id));
        }
        [HttpGet("role/{RoleId}/module-action/{ModuleActionId}")]
        public async Task<IActionResult> GetModuleActionRoleByRoleAndModuleActionId( int RoleId, int ModuleActionId)
        {
           
            return Ok(await _moduleActionRoleRepository.GetModuleActionRoleByRoleAndModuleActionId(RoleId,ModuleActionId));
        }

        [HttpPost]
        public async Task<IActionResult> CreateModule([FromBody] ModuleActionRoleDto moduleActionRole)
        {
            if (moduleActionRole == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdModule = await _moduleActionRoleRepository.AddModuleActionRole(moduleActionRole);
            var jsonModule = System.Text.Json.JsonSerializer.Serialize(createdModule);
            Console.WriteLine($" Module Action Role  response in json manuale  : {Content(jsonModule, "application/json")}");
            Console.WriteLine($" Module Action Role response in json   : {Created("Module", createdModule)}");

            // Return the JSON string as content with the appropriate content type
            ///return Content(jsonModule, "application/json");

            return Created("Module", createdModule);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateModuleActionRole([FromBody] ModuleActionRoleDto moduleActionRole)
        {
            if (moduleActionRole == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entryToUpdate = await _moduleActionRoleRepository.GetModuleActionRoleById(moduleActionRole.ModuleActionRoleId);

            if (entryToUpdate == null)
                return NotFound();

            await _moduleActionRoleRepository.UpdateModuleActionRole(moduleActionRole);

            return NoContent(); //success
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModuleActionRole(int id)
        {
            if (id == 0)
                return BadRequest();

            var moduleToDelete = await _moduleActionRoleRepository.GetModuleActionRoleById(id);
            if (moduleToDelete == null)
                return NotFound();

            await _moduleActionRoleRepository.DeleteModuleActionRole(id);

            return NoContent();//success
        }
    }
}

