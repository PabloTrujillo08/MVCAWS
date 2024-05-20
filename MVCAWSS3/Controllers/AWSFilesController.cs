using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MVCAWSS3.Helpers;
using MVCAWSS3.Services;

namespace MVCAWSS3.Controllers
{
    public class AWSFilesController : Controller
    {

        private UploadService UploadService;
        private AWSS3Service AWSS3Service;

        public AWSFilesController(UploadService uploadService, AWSS3Service aWSS3Service)
        {
            UploadService = uploadService;
            AWSS3Service = aWSS3Service;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> FilesListAWS()
        {
            List<string> files = await AWSS3Service.GetFilesList();
            return View(files);
        }

        //Vista para subir ficheros al bucket de AWS S3
        public IActionResult UploadFileAWS()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFileAWS(IFormFile file)
        {
            var tipo = file.ContentType.ToString();
            bool respuesta = false;
            int posicion = tipo.IndexOf("/");
            var tipofichero = tipo.Substring(0, posicion);

            if (tipofichero == "image")
            {
                await UploadService
                .UploadFileAsync(file, Folders.Images);
                respuesta = await AWSS3Service.UploadFile
                (file.FileName, Folders.Images);
            }
            else if (tipofichero == "text")
            {
                await UploadService
                .UploadFileAsync(file, Folders.Documents);
                respuesta = await AWSS3Service.UploadFile
                (file.FileName, Folders.Documents);
            }
            else
            {
                await UploadService
                .UploadFileAsync(file, Folders.Temporal);
                respuesta = await AWSS3Service.UploadFile
                (file.FileName, Folders.Temporal);
            }

            ViewData["MENSAJE"] ="Fichero subido a AWS" + respuesta;
            return View();
        }


        public async Task<IActionResult> FileAWS(string filename)
        {
            var posicion = filename.LastIndexOf(".");
            string extension = filename.Substring(posicion + 1);
            string[] extensiones_imagenes = { "jpg", "jpeg", "gif", "png", "tiff", "tif", "RAW", "bmp", "psd", "pdf", "eps", "pic" };
            bool esImagen = extensiones_imagenes.Contains(extension);
            string formato = "";
            if (esImagen)
            {
                formato = "image/png";
            }
            else
            {
                switch (extension)
                {
                    case "txt":
                        formato = "text/plain";
                        break;
                    case "docx":
                        formato = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;
                    case "pptx":
                        formato = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                        break;
                    default:
                        break;
                }
            }


            if (filename == null)
            {
                return View();
            }
            else
            {
                var file = await AWSS3Service.GetFile(filename);
                return File(file, formato);

            }
        }

        public IActionResult EditFileAWS(string filename)
        {

            ViewData["FILENAME"] = filename;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditFileAWS
            (IFormFile file, string filename)
        {
            if (file == null)
            {
                return View();
            }
            else
            {
                var tipo = file.ContentType.ToString();
                bool respuesta = false;
                int posicion = tipo.IndexOf("/");
                var tipofichero = tipo.Substring(0, posicion);
                Folders carpeta = Folders.Images;
                if (tipofichero == "image")
                {
                    carpeta = Folders.Images;
                }
                else if (tipofichero == "text")
                {
                    carpeta = Folders.Documents;
                }
                else
                {
                    carpeta = Folders.Temporal;
                }
                //PRIMERO SUSTITUIMOS EL FICHERO EN NUESTRO SERVIDOR

                await UploadService.UploadFileAsync(file, carpeta);
                //BORRAMOS EL FICHERO EN AWS

                await AWSS3Service.DeleteFile(filename,carpeta);

                //BORRAMOS EL FICHERO EN NUESTRO SERVIDOR
                //SUBIMOS EL ARCHIVO NUEVO A AWS

                respuesta =
                    await UploadService.DeleteFileAsync(filename, carpeta);
                await AWSS3Service.UpdateFile(file.FileName, carpeta);
                ViewData["FILENAME"] = file.FileName;
                ViewData["MENSAJE"] = "Fichero actualizado AWS: " + respuesta;
                return View();
            }
        }

        public async Task<IActionResult> DeleteFileAWS(string filename)
        {
            var posicion = filename.LastIndexOf(".");
            string extension = filename.Substring(posicion + 1);
            string[] extensiones_imagenes = { "jpg", "jpeg", "gif", "png", "tiff", "tif", "RAW", "bmp", "psd", "pdf", "eps", "pic" };
            bool esImagen = extensiones_imagenes.Contains(extension);
            Folders carpeta = Folders.Images;
            if (esImagen)
            {
                carpeta = Folders.Images;
            }
            else
            {
                switch (extension)
                {
                    case "txt":
                        carpeta = Folders.Documents;
                        break;

                    default:
                        carpeta = Folders.Documents;
                        break;
                }
            }

            await AWSS3Service.DeleteFile(filename, carpeta);
            return RedirectToAction("FilesListAWS");
        }


    }
}
