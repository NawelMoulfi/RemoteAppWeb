using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories.Contracts;

namespace RemoteAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterielController : ControllerBase
    {
        private readonly IMaterielRepository _materielRepository;

        public MaterielController(IMaterielRepository materielRepository)
        {
            _materielRepository = materielRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetMateriels()
        {
            return Ok(await _materielRepository.GetAllMateriels());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMaterielById(long id)
        {
            return Ok(await _materielRepository.GetMaterielById(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateFolder([FromBody] Materiel materiel)
        {
            if (materiel == null)
                return BadRequest();

            /*    if ((cl.LastName == string.Empty) || (client.FirstName == string.Empty))
                {
                    ModelState.AddModelError("FirstName and LastName", "FirstName et LastName shouldn't be empty");
                }*/

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdMateriel = await _materielRepository.AddMateriel(materiel);

            return Created("Materiel", createdMateriel);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMateriel([FromBody] Materiel materiel)
        {
            if (materiel == null)
                return BadRequest();

            /*  if ((client.FirstName == string.Empty) || (client.LastName == string.Empty))
              {
                  ModelState.AddModelError("FirstName and LastName", " FirstName and LastName shouldn't be empty");
              }*/

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entryToUpdate = await _materielRepository.GetMaterielById(materiel.MaterielId);

            if (entryToUpdate == null)
                return NotFound();

            await _materielRepository.UpdateMateriel(materiel);

            return NoContent(); //success
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMateriel(int id)
        {
            if (id == 0)
                return BadRequest();

            var materielToDelete = await _materielRepository.GetMaterielById(id);
            if (materielToDelete == null)
                return NotFound();

            await _materielRepository.DeleteMateriel(id);

            return NoContent();//success
        }

    }
}
