using Serilog;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Options;

namespace Core.FileControl
{
    public class AwsUploader : FileManager, IFileManager
    {
        private AwsSettings Settings { get; set; }
        private readonly RegionEndpoint Region;
        private readonly IAmazonS3 S3Client;
        private readonly TransferUtility FileTransferUtility;

        public AwsUploader(ILogger logger, IOptions<AwsSettings> settings) : base(logger)
        {
            Settings = settings.Value;
            Region = RegionEndpoint.GetBySystemName(Settings.AwsBucketRegion);
            S3Client = new AmazonS3Client(Settings.AwsAccessKeyId, Settings.AwsSecretKeyId, Region);
            FileTransferUtility = new TransferUtility(S3Client);
            Logger.Information("AWS S3 Bucket був успішно ініціалізований.");
        }
        public async override Task<string> SaveFileAsync(Stream file, string relativePath)
        {
            Logger.Information("Запит на збереження файлу на сервісі AWS S3 Bucket.");
            string fileName = Guid.NewGuid().ToString();
            ChangeDailyPath();
            string fullPath = "/" + relativePath + dailyFolder;
            var result = await SaveToAsync(file, fullPath, fileName);
            if (result)
            {
                Logger.Information("Файл був збережений на сервісі AWS S3 Bucket.");
                return relativePath;
            }
            Logger.Error("Файл не був збережений на сервісі AWS S3 Bucket.");
            return string.Empty;
        }
        public async override Task<bool> SaveToAsync(Stream stream, string relativeFilePath, string fileName)
        {
            Logger.Information($"Запит на збереження файлу на сервісі AWS S3 Bucket за таким шляхом={relativeFilePath}.");
            try
            {
                var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {
                    BucketName = Settings.AwsBucketName,
                    InputStream = stream,
                    StorageClass = S3StorageClass.StandardInfrequentAccess,
                    PartSize = stream.Length,
                    Key = relativeFilePath,
                    CannedACL = S3CannedACL.PublicRead
                };
                fileTransferUtilityRequest.Metadata.Add("param1", "Value1");
                fileTransferUtilityRequest.Metadata.Add("param2", "Value2");
                await FileTransferUtility.UploadAsync(fileTransferUtilityRequest);
                Logger.Information($"Був збережен новий файл на сервісі AWS S3 Bucket за таким шляхом={relativeFilePath}.");
                return true;
            }
            catch (AmazonS3Exception e)
            {
                Logger.Error($"Не вдалося зберігти файл на сервисі AWS S3 Bucket, AmazonS3 виключення={e.Message}");
            }
            catch (Exception e)
            {
                Logger.Error($"Не вдалося зберігти файл на сервисі AWS S3 Bucket, виключення={e.Message}");
            }
            return false;
        }
    }
}