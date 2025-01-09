namespace UseCases.AutoPosts.AutoPostFiles
{
    public class FileMover : IFileMover
    {
        public FileStream OpenRead(string path)
        {
            return File.OpenRead(path);
        }
        public void Delete(string path)
        {
            File.Delete(path);
        }
        public bool Exists(string path)
        {
            return File.Exists(path);
        }
    }
}
