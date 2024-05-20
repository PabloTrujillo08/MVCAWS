using Amazon.S3.Model;
using MVCAWSS3.Helpers;

namespace MVCAWSS3.Services
{
    public class AWSS3Service
    {
        private AWSS3BucketHelper AWSS3BucketHelper;
        private PathProvider PathProvider;
        private UploadService UploadService;

       

        public AWSS3Service(AWSS3BucketHelper aWSS3BucketHelper
            , PathProvider pathprovider, UploadService uploadService)
        {
            AWSS3BucketHelper = aWSS3BucketHelper;
            PathProvider = pathprovider;
            UploadService = uploadService;
        }

        public async Task<bool> UploadFile(string fileName, Folders folder)
        {
            try
            {
                string path = PathProvider.MapPath(fileName, folder);
                using (FileStream stream = new FileStream(path, FileMode.Open
                    , FileAccess.Read))
                {
                    return await AWSS3BucketHelper.UploadFile(stream, fileName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<string>> GetFilesList()
        {
            try
            {
                ListVersionsResponse listVersions = await
                    AWSS3BucketHelper.FilesList();
                return listVersions.Versions.Select(c => c.Key).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<Stream> GetFile(string fileName)
        {
            try
            {
                Stream fileStream = await AWSS3BucketHelper.GetFile(fileName);
                if (fileStream == null)
                {
                    Exception ex = new Exception("File Not Found");
                    throw ex;
                }
                else
                {
                    return fileStream;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> UpdateFile
            (string filename, Folders folder)
        {
            try
            {
                string path = PathProvider.MapPath(filename, folder);
                using (FileStream stream = new FileStream(path, FileMode.Open
                    , FileAccess.Read))
                {
                    return await AWSS3BucketHelper.UploadFile(stream, filename);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> DeleteFile(string fileName, Folders folder)
        {
            try
            {

                await UploadService.DeleteFileAsync(fileName, folder);
                return await AWSS3BucketHelper.DeleteFile(fileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}