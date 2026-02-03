using MiniGED.API.Dtos;

namespace MiniGED.API.Interfaces
{
    public interface IDocumentTextExtractor
    {

        public IEnumerable<FilePageSchema> ParsePdfContent(FileProcessingPayload dataparseobj);


    }
}
