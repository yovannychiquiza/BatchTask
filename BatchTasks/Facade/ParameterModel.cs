using System;
using System.Collections.Generic;
using System.Text;
using Amazon.S3;

namespace BatchTask.Facade
{
    public class ParameterModel
    {
        public string ServerOrigin { get; set; }
        public string FolderOrigin { get; set; }
        public string ServerDestination { get; set; }
    }

    public class ParameterMailModel
    {
        public string IntervalMinutes { get; set; }
        public string MailServer { get; set; }
        public string MailFrom { get; set; }
        public string MailTo { get; set; }
        public string MailSubject { get; set; }
        public string MailMessage { get; set; }
        public string NumberImagesLimit { get; set; }
        public string Servers { get; set; }
    }

    public class ParameterAWSDto
    {
        public string ServiceURL { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string BucketName { get; set; }
        public string ExpireImage { get; set; }
        public string ServerDownload { get; set; }
        public string ServerSource { get; set; }

    }

    public class AWSConection
    {
        public ParameterAWSDto ParameterAWSDto { get; set; }
        public AmazonS3Client amazonS3Client { get; set; }
    }
}

