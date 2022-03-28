using System.IO;
using System.Threading.Tasks;

namespace IntelART.Utilities.DocumentStore
{
    public class FileSystemDocumentStore : IDocumentStore
    {
        private string fileStroagePath;

        public FileSystemDocumentStore(string fileStroagePath)
        {
            this.fileStroagePath = fileStroagePath;
            if (!string.IsNullOrEmpty(this.fileStroagePath))
            {
                this.fileStroagePath = this.fileStroagePath.Trim();
            }
            if (!string.IsNullOrEmpty(this.fileStroagePath))
            {
                if (this.fileStroagePath.EndsWith("/") 
                    && this.fileStroagePath.EndsWith("\\"))
                {
                    this.fileStroagePath = this.fileStroagePath.Substring(0, this.fileStroagePath.Length - 1);
                }
                this.fileStroagePath = string.Format("{0}\\", this.fileStroagePath);
            }
        }

        private string GetFullPath(string filename)
        {
            return string.Format("{0}{1}", this.fileStroagePath, filename);
        }

        public async Task<Stream> RetriveDocumentAsync(string documentId)
        {
            return new FileStream(this.GetFullPath(documentId), FileMode.Open);
        }

        public async Task StoreDocumentAsync(string documentId, Stream content)
        {
            try
            {
                using (Stream outputStream = new FileStream(this.GetFullPath(documentId), FileMode.Create))
                {
                    await content.CopyToAsync(outputStream);
                }
            }
            catch (IOException ex)
            {
                throw new ApplicationException("E-1001", string.Format("Filed to save document in the local file {0}", documentId), ex);
            }
        }

        public async Task DeleteDocumentAsync(string documentId)
        {
            File.Delete(this.GetFullPath(documentId));
        }
    }
}
