using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories;
using RemoteAppApi.Repositories.Contracts;

namespace RemoteAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;

        public ClientController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            return Ok(await _clientRepository.GetAllClients());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(int id)
        {
            return Ok(await _clientRepository.GetClientById(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] Client client)
        {
            if (client == null)
                return BadRequest();

            if ((client.LastName == string.Empty) || (client.FirstName== string.Empty))
            {
                ModelState.AddModelError("FirstName and LastName", "FirstName et LastName shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdClient = await _clientRepository.AddClient(client);

            return Created("Client", createdClient);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateClient([FromBody] Client client)
        {
            if (client == null)
                return BadRequest();

            if ((client.FirstName == string.Empty) || (client.LastName== string.Empty))
            {
                ModelState.AddModelError("FirstName and LastName", " FirstName and LastName shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var clientToUpdate = await _clientRepository.GetClientById(client.ClientId);

            if (clientToUpdate == null)
                return NotFound();

            await _clientRepository.UpdateClient(client);

            return NoContent(); //success
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            if (id == 0)
                return BadRequest();

            var clientToDelete = await _clientRepository.GetClientById(id);
            if (clientToDelete == null)
                return NotFound();

            await _clientRepository.DeleteClient(id);

            return NoContent();//success
        }
    }
}
