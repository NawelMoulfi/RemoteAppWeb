using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories;
using RemoteAppApi.Repositories.Contracts;

namespace RemoteAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterielRapportController : ControllerBase
    {
        private readonly IMaterielRapportRepository _MaterielRapportRepository;

        public MaterielRapportController(IMaterielRapportRepository MaterielRapportRepository)
        {
            _MaterielRapportRepository = MaterielRapportRepository;
        }

        [HttpGet("{RapportId}")]
        public async Task<IActionResult> GetMaterielRapports(long RapportId)
        {
            return Ok(await  _MaterielRapportRepository.GetMateriels(RapportId));
        }

        [HttpPost]
        public async Task<IActionResult> CreateMaterielRepport([FromBody] MaterielRapport materielrapport)
        {
            if (materielrapport == null)
                return BadRequest();

            /*    if ((cl.LastName == string.Empty) || (client.FirstName == string.Empty))
                {
                    ModelState.AddModelError("FirstName and LastName", "FirstName et LastName shouldn't be empty");
                }*/

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdMateriel = await _MaterielRapportRepository.AddMaterielRapport(materielrapport);

            return Created("Materiel", createdMateriel);
        }

    }
}
