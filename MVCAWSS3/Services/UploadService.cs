using MVCAWSS3.Helpers;

namespace MVCAWSS3.Services
{
    public class UploadService
    {
        PathProvider PathProvider;

        public UploadService(PathProvider pathprovider)
        {
            PathProvider = pathprovider;
        }

        public async Task<string>
            UploadFileAsync(IFormFile formfile, Folders folder)
        {
            string filename = formfile.FileName;
            string path =
                PathProvider.MapPath(filename, folder);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await formfile.CopyToAsync(stream);
            }
            return path;
        }

        public async Task<string>
         UploadFileAsync(IFormFile formfile, string filename, Folders folder)
        {
            string path =
                PathProvider.MapPath(filename, folder);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await formfile.CopyToAsync(stream);
            }
            return path;
        }

        public async Task<string>
            UploadFileAsync(Stream stream, string fileName, Folders folder)
        {
            string path =
                PathProvider.MapPath(fileName, folder);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await stream.CopyToAsync(fileStream);
            }
            return path;
        }
        //método para borrar primero el fichero en nuestro //servidor a la hora de 
        //actualizer un fichero
        public async Task<bool> DeleteFileAsync(string fileName, Folders folder)
        {
            string path =
                PathProvider.MapPath(fileName, folder);
            File.Delete(path);

            return true;
        }
    }
}