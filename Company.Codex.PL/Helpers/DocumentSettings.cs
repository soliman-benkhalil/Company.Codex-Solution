using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Xml.Serialization;

namespace Company.Codex.PL.Helpers
{
    public class DocumentSettings
    {
        // 1. Upload

        public static string UploadFile(IFormFile file, string folderName) // images or videos 
        {
            // 1. Get Folder Location Path
            string folderpath = Path.Combine( Directory.GetCurrentDirectory(),@"wwwroot\files",folderName); // The File Where i will put The image  
            // @ here to make the \ as \ which treats it as in link

            // 2. Get The File Name Make it Nunique

            string fileName = $"{Guid.NewGuid()}{file.FileName}";

            // 3. Get The File Paht -> Folder Path + File Name 
             
            string FilePath = Path.Combine(folderpath, fileName);

            // 4. Save File as stream : Data Per Time 

            using var fileStream = new FileStream(FilePath,FileMode.Create); // Using here to close the connection after open it 


            file.CopyTo(fileStream);

            return fileName ;   

        }



        // 2. Delete 

        public static void DeleteFile(string fileName , string folderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName,fileName);

            if (File.Exists(filePath) )
            {
                File.Delete(filePath);
            }

        }
    }
}
