namespace Core.FileControl
{
    public class AwsSettings
    {
        public required string AwsBucketRegion { get; set; }
        public required string AwsBucketName { get; set; }
        public required string AwsAccessKeyId { get; set; }
        public required string AwsSecretKeyId { get; set; }
    }
}
