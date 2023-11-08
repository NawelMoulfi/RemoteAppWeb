using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories;
using RemoteAppApi.Repositories.Contracts;

namespace RemoteAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RapportInterventionController : ControllerBase
    {
        private readonly IRapportInterventionRepository _RapportInterventionRepository;

        public RapportInterventionController(IRapportInterventionRepository RapportInterventionRepository)
        {
            _RapportInterventionRepository = RapportInterventionRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetRapportInterventions()
        {
            return Ok(await _RapportInterventionRepository.GetAllRapportInterventions());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRapportInterventionById(long id)
        {
            return Ok(await _RapportInterventionRepository.GetRapportInterventionrById(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] RapportIntervention RapportIntervention)
        {
            if (RapportIntervention == null)
                return BadRequest();

          /*  if ((client.LastName == string.Empty) || (client.FirstName == string.Empty))
            {
                ModelState.AddModelError("FirstName and LastName", "FirstName et LastName shouldn't be empty");
            }+*/

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdRapportIntervention = await _RapportInterventionRepository.AddRapportIntervention(RapportIntervention);

            return Created("RapportIntervention", createdRapportIntervention);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateClient([FromBody] RapportIntervention RapportIntervention)
        {
            if (RapportIntervention == null)
                return BadRequest();

            /*if ((client.FirstName == string.Empty) || (client.LastName == string.Empty))
            {
                ModelState.AddModelError("FirstName and LastName", " FirstName and LastName shouldn't be empty");
            }*/

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var RapportInterventionToUpdate = await _RapportInterventionRepository.GetRapportInterventionrById(RapportIntervention.RapportId);

            if (RapportInterventionToUpdate == null)
                return NotFound();

            await _RapportInterventionRepository.UpdateRapportIntervention(RapportIntervention);

            return NoContent(); //success
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRapportIntervention(long id)
        {
            if (id == 0)
                return BadRequest();

            var RapportInterventionToDelete = await _RapportInterventionRepository.GetRapportInterventionrById(id);
            if (RapportInterventionToDelete == null)
                return NotFound();

            await _RapportInterventionRepository.DeleteRapportIntervention(id);

            return NoContent();//success
        }
    }
}
