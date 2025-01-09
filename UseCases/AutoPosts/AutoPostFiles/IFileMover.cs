namespace UseCases.AutoPosts.AutoPostFiles
{
    public interface IFileMover
    {
        public FileStream OpenRead(string path);
        public bool Exists(string path);
        public void Delete(string path);
    }
}
