namespace UseCases.Base
{
    public class FileDto
    {
        //
        // Summary:
        //     Gets the raw Content-Type header of the uploaded file.
        public required string ContentType { get; set; }
        //
        // Summary:
        //     Gets the raw Content-Disposition header of the uploaded file.
        public required string ContentDisposition { get; set; }
        //
        // Summary:
        //     Gets the header dictionary of the uploaded file.
        public required Dictionary<string, string> Headers { get; set; }
        //
        // Summary:
        //     Gets the file length in bytes.
        public long Length { get; set; }
        //
        // Summary:
        //     Gets the form field name from the Content-Disposition header.
        public required string Name { get; set; }
        //
        // Summary:
        //     Gets the file name from the Content-Disposition header.
        public required string FileName { get; set; }
        //
        // Summary:
        //     Copies the contents of the uploaded file to the target stream.
        //
        //
        // Summary:
        //     Opens the request stream for reading the uploaded file.
        public Stream OpenReadStream()
        {
            return Stream;
        }
        public required Stream Stream { get; set; }
    }
}
