namespace Core.FileControl
{
    public interface IFileManager
    {
        Task<string> SaveFileAsync(Stream file, string relativePath);
        Task<bool> SaveToAsync(Stream file, string fullPath, string fileName);
    }
}
