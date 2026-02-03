using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniGED.API.Data;
using MiniGED.API.Services;
using MiniGED.API.Dtos;
using Hangfire;
using MiniGED.API.Models;
using Microsoft.EntityFrameworkCore;
using UglyToad.PdfPig.Exceptions;
using MiniGED.API.Interfaces;

namespace MiniGED.API.Controllers   
{
    [Authorize]
    [ApiController]
    [Route("api/document")]
    public class DocumentFileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;
        private readonly IDocumentSearchEngine _documentSearchEngine;
        private readonly IFileProcessingJob _fileProcessingJob;

        public DocumentFileController(ApplicationDbContext context,IFileService fileService, IFileProcessingJob fileProcessingJob, IDocumentSearchEngine documentSearchEngine)
        {
            _context = context;
            _fileService = fileService;
            _fileProcessingJob = fileProcessingJob;
            _documentSearchEngine = documentSearchEngine;
        }

        [HttpGet("{id}/files")]
        public async Task<IActionResult> GetDocumentFiles(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            int userId = int.Parse(userIdClaim.Value);
            var document = await _context.Documents.FindAsync(id);

            if (document == null || document.UserId != userId)
            {
                return Unauthorized();
            }
            var documentFiles = _context.DocumentFiles
                .Where(x => x.DocumentId == id)
                  .Select(d => new DocFileResponse
                  {
                      Id = d.Id,
                      FileName = d.FileName,
                      FileSize = d.FileSize,
                      FilePath = d.FilePath
                  })
                .ToList();
            if (documentFiles.Count == 0) // Check if the list is empty using Count
            {
                return NotFound("No Files found for the given DocumentId");
            }

            return Ok(documentFiles);
        }

        //[HttpPost("ParseText")]
        //public async Task<IActionResult> parsePdfText(int fileId) {
        //    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        //    if (userIdClaim == null)
        //    {
        //        return Unauthorized();
        //    }
        //    int userId = int.Parse(userIdClaim.Value);

        //    var documentFile = _context.DocumentFiles
        //        .Where(X => X.Id == fileId)
        //        .First();
        //    if(documentFile == null)
        //    {
        //        return NotFound("File Doesn't exist");
        //    }
        //    DataParseObject parseRequestData = new DataParseObject
        //    {
        //        UserId = userId,
        //        DocumentId = documentFile.DocumentId,
        //        FileId = fileId,

        //    };

        //    var ExtractedResult = _textExtractionService.ParsePdfContent(parseRequestData);
        //    var resultTask = await _meiliSearchService.AddIndex(ExtractedResult);


        //    return Ok(new
        //    {
        //        success = resultTask,
        //        result = _textExtractionService.ParsePdfContent(parseRequestData)
        //    });
        //}

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] FileUploadModel model)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized();
                }

                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    return BadRequest("Invalid user");

                if (model.Files == null || model.Files.Count == 0)
                    return BadRequest("No files selected");

                var document = await _context.Documents.FindAsync(model.DocumentId);
                if (document == null)
                    return BadRequest("Document doesn't exist");

                if (user.Id != document.UserId)
                    return Unauthorized("Not authorized to update this document");

                // Validate each file
                foreach (var file in model.Files)
                {
                    if (file.Length == 0) continue;

                    // Validate size and extension...

                    var sanitizedFileName = Path.GetFileNameWithoutExtension(file.FileName) + Path.GetExtension(file.FileName).ToLower();

                    var newDocFile = new DocumentFile
                    {
                        FileName = sanitizedFileName,
                        FileSize = file.Length,
                        Status = FileProcessingStatus.Pending,
                        FilePath = "", // will update after upload
                        DocumentId = model.DocumentId
                    };

                    _context.DocumentFiles.Add(newDocFile);

                    // Save now to get the Id
                    await _context.SaveChangesAsync();

                    var result = await _fileService.UploadFile(newDocFile, file);

                    if (result == "file not selected" || result == "file Already Exists")
                    {
                        _context.DocumentFiles.Remove(newDocFile);
                        await _context.SaveChangesAsync();
                        return BadRequest($"Failed to upload {file.FileName}, {result}");
                    }

                    newDocFile.FilePath = result;

                    // Save updated filepath
                    await _context.SaveChangesAsync();
                    try
                    {

                    var jobId = BackgroundJob.Enqueue<IFileProcessingJob>(job => job.FileParsingAndIndexingAsync(new FileProcessingPayload{
                        DocumentId = newDocFile.DocumentId,
                        FileId = newDocFile.Id,
                        UserId = user.Id,
                        }));
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Inner Exception: " + ex.InnerException?.Message);
                        Console.WriteLine("Stack Trace: " + ex.StackTrace);
                        Console.WriteLine("Inner Stack Trace: " + ex.InnerException?.StackTrace);
                        return StatusCode(500,ex.Message);
                    }
                }
               
                return Ok(new
                {
                    success = true,

                    message = "All files uploaded successfully.",
                    
                });
            }
            catch (Exception e)
            {
                return   StatusCode(500, new { message = e.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("pdf/{id}")]
        public IActionResult Get(int id)
        {
            var folderName = Path.Combine("Resource", "AllFiles");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            // Use a regular expression to remove any existing version number from the filename
            var fileName = id.ToString();

            // Get all files that start with the same id
            var files = Directory.GetFiles(pathToSave, $"{fileName}");

            // Extract version numbers from filenames
           

            // Determine the highest version number
          

            // Append the version number to the filename
            var fullPath = Path.Combine(pathToSave, $"{fileName}.pdf");

            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound($"The requested version of the file does not exist. {fullPath}");
            }

            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            return File(stream, "application/pdf");
        }


        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _context.DocumentFiles.Find(id);
            if (result == null)
            {
                return NotFound("File Not Found");
            }
            _context.DocumentFiles.Remove(result);
            
            Boolean isdeleted = _fileService.DeleteFile(id);
            if (isdeleted)
            {
                _context.SaveChanges();
                return Ok(new
                {
                    success = true,
                    message = "File Deleted Successfully !"
                });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to delete");
            }
        }

        [HttpPost("search/{keyword}")]
        public async Task<IActionResult> Search(string keyword)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return BadRequest("Invalid user");

            var results = await _documentSearchEngine.SearchIndexAsync(keyword,userId);
            var resultsGrouped = results.Hits.GroupBy(r => r.DocumentIdIndexed)
                        .Select(group => new
                        {
                            DocumentId = group.Key,
                            Files = group.GroupBy(x => x.FileIdIndexed).ToList()
                        });



            return Ok(new
            {
                success = true,
                results,
            });
        }
    }
}
