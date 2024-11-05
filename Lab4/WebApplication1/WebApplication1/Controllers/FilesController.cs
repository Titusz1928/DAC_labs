using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication1.Services;
using System.IO;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly CloudStorageService _cloudStorageService;

        public FilesController()
        {
            _cloudStorageService = new CloudStorageService();
        }


        // GET: api/files
        [HttpGet]
        public async Task<IActionResult> ListFiles()
        {
            // Assuming ListFilesAsync returns a list of file information (name, last updated, etc.)
            var files = await _cloudStorageService.ListFilesAsync(); // Implement this method to fetch file details
            return Ok(files); // Return the list of files as JSON
        }

        // GET: /home
        [HttpGet("home")]
        public IActionResult Home()
        {
            // Return the index.html file
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "index.html"), "text/html");
        }

        // POST: api/files/upload
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] string fileName, [FromForm] IFormFile file)
        {
            using var stream = file.OpenReadStream();
            await _cloudStorageService.UploadFileAsync(fileName, stream);
            return Ok($"Uploaded {fileName}.");
        }

        // GET: api/files/download/{fileName}
        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            var stream = await _cloudStorageService.DownloadFileAsync(fileName);
            return File(stream, "application/octet-stream", fileName);
        }

        // PUT: api/files/update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateFile([FromForm] string fileName, [FromForm] IFormFile file)
        {
            using var stream = file.OpenReadStream();
            await _cloudStorageService.UpdateFileAsync(fileName, stream);
            return Ok($"Updated {fileName}.");
        }

        // DELETE: api/files/{fileName}
        [HttpDelete("{fileName}")]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            await _cloudStorageService.DeleteFileAsync(fileName);
            return Ok($"Deleted {fileName}.");
        }
    }
}
