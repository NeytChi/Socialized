using Serilog;

namespace Core.FileControl
{
    public class FileManager(ILogger logger) : IFileManager
    {
        private readonly string currentDirectory = Directory.GetCurrentDirectory();
        public readonly ILogger Logger = logger;
        public DateTime currentTime = DateTime.Now;
        public string dailyFolder = "/" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "/";

        public async virtual Task<string> SaveFileAsync(Stream file, string relativePath)
        {
            Logger.Information("Запит на збереження файлу в файловій системі.");
            string fileName = Guid.NewGuid().ToString();
            ChangeDailyPath();
            string fileRelativePath = "/" + relativePath + dailyFolder;
            var fullPath = CheckDirectory(fileRelativePath);
            var result = await SaveToAsync(file, fullPath, fileName);
            if (result)
            {
                Logger.Information("Файл був збережений в файловій системі.");
                return fileRelativePath + fileName;
            }
            Logger.Error("Файл не був збережений в файловій системі.");
            return string.Empty;
        }
        public void ChangeDailyPath()
        {
            if (currentTime.Day != DateTime.Now.Day)
            {
                currentTime = DateTime.Now;
                dailyFolder = "/" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "/";
                Logger.Information("Шлях до файлів був змінений на новий шлях={dailyFolder}");
            }
        }
        public string CheckDirectory(string fileRelativePath)
        {
            var fullPath = currentDirectory + fileRelativePath;
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
                Logger.Information("Була створена нова папка. Шлях до папки ->/" + fileRelativePath);
            }
            return fullPath;
        }
        public void DeleteFile(string relativePath)
        {
            if (File.Exists(currentDirectory + relativePath))
            {
                File.Delete(currentDirectory + relativePath);
                Logger.Information("Файл був видалений з файлової системи.");
            }
        }
        public async virtual Task<bool> SaveToAsync(Stream file, string fullPath, string fileName)
        {
            var fileFullPathToSave = fullPath + fileName;
            if (File.Exists(fileFullPathToSave))
            {
                Logger.Error("Сервер не може зберегти в файловій системі файл з такою самою назвою.");
                return false;
            }
            try
            {
                using (var stream = new FileStream(fileFullPathToSave, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                Logger.Information($"Був створений новий файл={fileName}.");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Помилка при збереженні файлу: {ex.Message}");
                return false;
            }
        }

    }
}