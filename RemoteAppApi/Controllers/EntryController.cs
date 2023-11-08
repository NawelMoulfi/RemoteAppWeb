using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories.Contracts;

namespace RemoteAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntryController : ControllerBase
    {
        private readonly IEntryRepository _entryRepository;

        public EntryController(IEntryRepository entryRepository)
        {
            _entryRepository = entryRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetEntries()
        {
            return Ok(await _entryRepository.GetAllEntrys());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEntryById(int id)
        {
            return Ok(await _entryRepository.GetEntryById(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateEntry([FromBody] Entry entry)
        {
            if (entry == null)
                return BadRequest();

        /*    if ((cl.LastName == string.Empty) || (client.FirstName == string.Empty))
            {
                ModelState.AddModelError("FirstName and LastName", "FirstName et LastName shouldn't be empty");
            }*/

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdEntry = await _entryRepository.AddEntry(entry);

            return Created("Entry", createdEntry);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEntry([FromBody] Entry entry)
        {
            if (entry == null)
                return BadRequest();

          /*  if ((client.FirstName == string.Empty) || (client.LastName == string.Empty))
            {
                ModelState.AddModelError("FirstName and LastName", " FirstName and LastName shouldn't be empty");
            }*/

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entryToUpdate = await _entryRepository.GetEntryById(entry.EntryId);

            if (entryToUpdate == null)
                return NotFound();

            await _entryRepository.UpdateEntry(entry);

            return NoContent(); //success
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntry(int id)
        {
            if (id == 0)
                return BadRequest();

            var entryToDelete = await _entryRepository.GetEntryById(id);
            if (entryToDelete == null)
                return NotFound();

            await _entryRepository.DeleteEntry(id);

            return NoContent();//success
        }
    }
}
