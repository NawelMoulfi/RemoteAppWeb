using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories.Contracts;

namespace RemoteAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleActionController : ControllerBase
    {
        private readonly IModuleActionRepository _moduleactionRepository;

        public ModuleActionController(IModuleActionRepository moduleactionRepository)
        {
            _moduleactionRepository = moduleactionRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetModuleActions()
        {
            return Ok(await _moduleactionRepository.GetAllModulActions());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMaterielById(int id)
        {
            return Ok(await _moduleactionRepository.GetModuleActionAById(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateModuleAction([FromBody] ModuleAction moduleaction)
        {
            if (moduleaction == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdModuleAction = await _moduleactionRepository.AddModuleAction(moduleaction);

            return Created("ModuleAction", createdModuleAction);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateModuleAction([FromBody] ModuleAction moduleaction)
        {
            if (moduleaction == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entryToUpdate = await _moduleactionRepository.GetModuleActionAById(moduleaction.ModuleActionID);

            if (entryToUpdate == null)
                return NotFound();

            await _moduleactionRepository.UpdateModuleAction(moduleaction);

            return NoContent(); //success
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModuleAction(int id)
        {
            if (id == 0)
                return BadRequest();

            var moduleactionToDelete = await _moduleactionRepository.GetModuleActionAById(id);
            if (moduleactionToDelete == null)
                return NotFound();

            await _moduleactionRepository.DeleteModuleAction(id);

            return NoContent();//success
        }
        //[HttpGet("resource/{resource}/action/{action}")]
        [HttpGet("resource")]
        public async Task<IActionResult> GetResourceAction([FromQuery] ResourceActionParameters parameters)
        {
            Resource resource = parameters.Resource;
            RemoteApp.Data.Models.Action action = parameters.Action;
            Console.WriteLine($"Resource: {resource}");
            Console.WriteLine($"Action: {action}");

            var moduleAction = await _moduleactionRepository.GetResourceAction(resource, action);



            if (moduleAction == null)
            {
                return NotFound(); // Return a 404 Not Found response if the ModuleAction doesn't exist
            }

            // You can return the ModuleAction in the response
            return Ok(moduleAction);

        }

        [HttpGet("resource/{resource}/actions")]
        public async Task<IActionResult> GetModuleActionsByResource(Resource resource)
        {
            // Call your repository method to get the list of ModuleActions by resource
            var moduleActions = await _moduleactionRepository.GetListModuleActionsByResource(resource);

            if (moduleActions == null || !moduleActions.Any())
            {
                return NotFound(); // Return a 404 Not Found response if no ModuleActions are found
            }

            // You can return the list of ModuleActions in the response
            return Ok(moduleActions);
        }
        [HttpGet("resource/{resource}/resource-actions")]
        public async Task<IActionResult> GetActionsByResource(Resource resource)
        {
            var actions = await _moduleactionRepository.GetActionsByResource(resource);
            if (actions == null || !actions.Any())
            {
                return NotFound(); // Return a 404 Not Found response if no ModuleActions are found
            }
            return Ok(actions);
        }
        [HttpGet("resource/actions/excluding-first")]
        public async Task<IActionResult> GetActionsByResourceExcludingFirst()
        {
            var actions = await _moduleactionRepository.GetActionsByResourceExcludingFirst();
            if (actions == null || !actions.Any())
            {
                return NotFound(); // Return a 404 Not Found response if no ModuleActions are found
            }
            return Ok(actions);
        }

        [HttpGet("resource/used")]
        public async Task<IActionResult> GetUsedResources()
        {
            var resources = await _moduleactionRepository.GetUsedResources();
            if (resources == null || !resources.Any())
            {
                return NotFound(); // Return a 404 Not Found response if no ModuleActions are found
            }
            return Ok(resources);
        }


    }
}
