using FullStackTechTest.Interfaces;
using Models;
using DAL;
using Newtonsoft.Json;

namespace FullStackTechTest.Services
{
    public class ParseJsonService : IFileUpload
    {
        public async Task<List<JsonWrapper>> UploadFile(IFormFile formFile)
        {
            //Parsing data.json
            List<JsonWrapper> jsonList = new List<JsonWrapper>();
            try
            {
                string fileContent = null;
                using (var reader = new StreamReader(formFile.OpenReadStream()))
                {
                    fileContent = reader.ReadToEnd();
                }
                if (!string.IsNullOrEmpty(fileContent)) {
                    //parse the json here
                    jsonList = JsonConvert.DeserializeObject<List<JsonWrapper>>(fileContent);
                }
                return jsonList;
            }
            catch (Exception ex) 
            {
                throw new Exception("Could not parse the JSON", ex);
            }

        }
    }
}
