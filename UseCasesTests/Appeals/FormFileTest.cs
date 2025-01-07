using Microsoft.AspNetCore.Http;

namespace UseCasesTests.Appeals
{
    public class FormFileTest : IFormFile
    {
        public string ContentType => "";

        public string ContentDisposition => "";

        public IHeaderDictionary Headers => null;

        public long Length => 0;

        public string Name => "";

        public string FileName => "";

        public void CopyTo(Stream target)
        {

        }

        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Stream OpenReadStream()
        {
            return new MemoryStream();
        }
    }
}
