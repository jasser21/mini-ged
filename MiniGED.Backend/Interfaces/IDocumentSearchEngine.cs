using System.Diagnostics.Eventing.Reader;
using Meilisearch;
using MiniGED.API.Dtos;
using Newtonsoft.Json.Linq;

namespace MiniGED.API.Interfaces
{
    public interface IDocumentSearchEngine
    {
        public Task<TaskInfoStatus> AddIndex(IEnumerable<FilePageSchema> Files);

        public Task<ISearchable<FilePageSchemaResponse>> SearchIndexAsync(String keyword,int userId);
    }
} 
