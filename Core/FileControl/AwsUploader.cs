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
        }
        public override string SaveFile(Stream stream, string RelativePath)
        {
            string fileName = Guid.NewGuid().ToString();
            ChangeDailyPath();
            string relativePath = RelativePath + dailyFolder + fileName;
            if (SaveTo(stream, relativePath))
            {
                return relativePath;
            }
            return string.Empty;
        }
        public bool SaveTo(Stream stream, string relativeFilePath)
        {
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
                FileTransferUtility.Upload(fileTransferUtilityRequest);
                Logger.Information($"��� �������� ����� ���� �� ����� AWS S3 Bucket �� ����� ������={relativeFilePath}.");
                return true;
            }
            catch (AmazonS3Exception e)
            {
                Logger.Error($"�� ������� ������� ���� �� ������ AWS S3 Bucket, AmazonS3 ����������={e.Message}");
            }
            catch (Exception e)
            {
                Logger.Error($"�� ������� ������� ���� �� ������ AWS S3 Bucket, ����������={e.Message}");
            }
            return false;
        }

        public bool SaveTo(string fullPathFile, string relativeFilePath)
        {
            try
            {
                var stream = File.OpenRead(fullPathFile);
                SaveTo(stream, relativeFilePath);
            }
            catch (Exception e)
            {
                Logger.Error($"�� ������� ��������� ���� ��� ���������� ���� �� AWS S3 Bucket, ����������={e.Message}");
            }
            return false;
        }
    }
}