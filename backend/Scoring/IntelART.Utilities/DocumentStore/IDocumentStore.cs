using System.IO;
using System.Threading.Tasks;

namespace IntelART.Utilities.DocumentStore
{
    public interface IDocumentStore
    {
        Task StoreDocumentAsync(string documentId, Stream content);
        Task DeleteDocumentAsync(string documentId);
        Task<Stream> RetriveDocumentAsync(string documentId);
    }
}
