using Amazon.S3;
using Amazon.S3.Model;

namespace MVCAWSS3.Helpers
{
    public class AWSS3BucketHelper
    {
        private IAmazonS3 AmazonS3Client;
        private string bucketName;
        public AWSS3BucketHelper(IAmazonS3 s3Client
            , IConfiguration configuration)
        {
            AmazonS3Client = s3Client;
            bucketName = configuration["AWSS3:BucketName"];
        }

        public async Task<bool> UploadFile(System.IO.Stream inputStream
            , string fileName)
        {
            try
            {
                PutObjectRequest request = new PutObjectRequest()
                {
                    InputStream = inputStream,
                    BucketName = bucketName,
                    Key = fileName
                };
                PutObjectResponse response = await AmazonS3Client
                    .PutObjectAsync(request);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ListVersionsResponse> FilesList()
        {
            return await AmazonS3Client.ListVersionsAsync(bucketName);
        }

        public async Task<Stream> GetFile(string fileName)
        {

            GetObjectResponse response = await AmazonS3Client
                .GetObjectAsync(bucketName, fileName);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                return response.ResponseStream;
            else
                return null;
        }

        public async Task<bool> DeleteFile(string fileName)
        {
            try
            {
                DeleteObjectResponse response = await
            AmazonS3Client.DeleteObjectAsync(bucketName, fileName);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.NoContent)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}