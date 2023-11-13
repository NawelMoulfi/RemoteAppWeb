using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories.Contracts;
using Shared.Dto;

namespace RemoteAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _RoleRepository;

        public RoleController(IRoleRepository RoleRepository)
        {
            _RoleRepository = RoleRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(await _RoleRepository.GetAllRoles());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            return Ok(await _RoleRepository.GetRoleById(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleDto Role)
        {
            if (Role == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdRole = await _RoleRepository.AddRole(Role);

            return Created("Role", createdRole);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRole([FromBody] RoleDto Role)
        {
            if (Role == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entryToUpdate = await _RoleRepository.GetRoleById(Role.RoleId);

            if (entryToUpdate == null)
                return NotFound();

            await _RoleRepository.UpdateRole(Role);

            return NoContent(); //success
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            if (id == 0)
                return BadRequest();

            var roleToDelete = await _RoleRepository.GetRoleById(id);
            if (roleToDelete == null)
                return NotFound();

            await _RoleRepository.DeleteRole(id);

            return NoContent();//success
        }
    }
}
