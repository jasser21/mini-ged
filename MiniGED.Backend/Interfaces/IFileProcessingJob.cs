using MiniGED.API.Dtos;
using MiniGED.API.Models;

namespace MiniGED.API.Interfaces
{
    public interface IFileProcessingJob
    {
        public  Task FileParsingAndIndexingAsync(FileProcessingPayload payload);
    }
}
