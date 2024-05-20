namespace MVCAWSS3.Helpers
{
    public enum Folders
    {
        Images = 0,
        Documents = 1,
        Temporal = 2
    }

    public class PathProvider
    {
        IWebHostEnvironment Environment;

        public PathProvider(IWebHostEnvironment _environment)
        {
            Environment = _environment;
        }

        //Metodo que se encarga de devolver la ruta de los ficheros
        public string MapPath(string filename, Folders folder)
        {
            string folderName = string.Empty;
            switch (folder)
            {
                case Folders.Images:
                    folderName = "Images";
                    break;
                case Folders.Documents:
                    folderName = "Documents";
                    break;
                case Folders.Temporal:
                    folderName = "Temporal";
                    break;
            }
            return Path.Combine(Environment.WebRootPath, folderName, filename);
        }
    }
}
