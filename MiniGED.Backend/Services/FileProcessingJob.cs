using System.Threading.Tasks;
using Meilisearch;
using MiniGED.API.Data;
using MiniGED.API.Dtos;
using MiniGED.API.Interfaces;
using Meilisearch;
using MiniGED.API.Models;

namespace MiniGED.API.Services
{
    public class FileProcessingJob : IFileProcessingJob
    {
        public IDocumentSearchEngine _documentSearchEngine;
        public IDocumentTextExtractor _documentTextExtractor;
        public IFileService _fileService;

        public FileProcessingJob(IDocumentSearchEngine documentSearchEngine, IDocumentTextExtractor documentTextExtractor,IFileService fileService)
        {
            _documentSearchEngine = documentSearchEngine;
            _documentTextExtractor = documentTextExtractor;
            _fileService = fileService; 
            
        }


        public  async Task FileParsingAndIndexingAsync(FileProcessingPayload parsingPayload)
        {

            _fileService.ChangeFileStatus(parsingPayload.FileId, FileProcessingStatus.Processing);

            
            var parsingResponse = _documentTextExtractor.ParsePdfContent(parsingPayload);

            var resultStatus = await _documentSearchEngine.AddIndex(parsingResponse);

            if (resultStatus == TaskInfoStatus.Succeeded) { 
                _fileService.ChangeFileStatus(parsingPayload.FileId,FileProcessingStatus.Completed);
            }

            
             Console.WriteLine( resultStatus.ToString());
                    
            

        }
    }
}
