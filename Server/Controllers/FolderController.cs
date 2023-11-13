using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories;
using RemoteAppApi.Repositories.Contracts;
using Shared.Dto;

namespace RemoteAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FolderController : ControllerBase
    {
        private readonly IFolderRepository _folderRepository;

        public FolderController(IFolderRepository folderRepository)
        {
            _folderRepository = folderRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetFolders()
        {
            var folders=  await _folderRepository.GetAllFolders();
         

            Console.WriteLine($" User response in json : {Ok(folders)}");

            var jsonfolders = System.Text.Json.JsonSerializer.Serialize(folders);
            Console.WriteLine($" User response in json manuale  : {Content(jsonfolders, "application/json")}");

            // Return the JSON string as content with the appropriate content type
            return Content(jsonfolders, "application/json");
          
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFolderById(int id)
        {
            return Ok(await _folderRepository.GetFolderById(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateFolder([FromBody] FolderDto folder)
        {
            if (folder == null)
                return BadRequest();

            /*    if ((cl.LastName == string.Empty) || (client.FirstName == string.Empty))
                {
                    ModelState.AddModelError("FirstName and LastName", "FirstName et LastName shouldn't be empty");
                }*/

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdFolder = await _folderRepository.AddFolder(folder);

            return Created("Folder", createdFolder);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFolder([FromBody] FolderDto folder)
        {
            if (folder == null)
                return BadRequest();

            /*  if ((client.FirstName == string.Empty) || (client.LastName == string.Empty))
              {
                  ModelState.AddModelError("FirstName and LastName", " FirstName and LastName shouldn't be empty");
              }*/

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entryToUpdate = await _folderRepository.GetFolderById(folder.FolderId);

            if (entryToUpdate == null)
                return NotFound();

            await _folderRepository.UpdateFolder(folder);

            return NoContent(); //success
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFolder(int id)
        {
            if (id == 0)
                return BadRequest();

            var folderToDelete = await _folderRepository.GetFolderById(id);
            if (folderToDelete == null)
                return NotFound();

            await _folderRepository.DeleteFolder(id);

            return NoContent();//success
        }
    }
}
