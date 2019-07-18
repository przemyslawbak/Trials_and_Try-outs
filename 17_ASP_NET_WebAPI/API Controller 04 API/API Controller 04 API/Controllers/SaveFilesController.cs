using API_Controller_04_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace API_Controller_04_API.Controllers
{
    //https://code-maze.com/aspnetcore-webapi-best-practices/
    //encoding in UTF8
    public class Utf8StringWriter : StringWriter //zamienić na ApiController
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
    //saving DB records in the folder
    [Route("api/jobs/[controller]")]
    public class SaveFilesController : Controller
    {
        //repository
        private ISaveFilesRepository _repository;
        public SaveFilesController(ISaveFilesRepository repo)
        {
            //instance repo
            _repository = repo;
        }
        /// <summary>
        /// Gets some models from the DB
        /// </summary>
        /// <description>
        /// selecting the database records and then serializes each table record as an XML file
        /// </description>
        [HttpGet]
        public void Get()
        {
            //clear App_Data/xml/ files and folders
            string saveFolder = Path.Combine("App_Data", "xml");
            DirectoryInfo di = new DirectoryInfo(saveFolder);
            //delete files
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            //delete folders
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
            //foreach item in DB table
            IEnumerable<RequestJSONModel> getModels = _repository.GetRequests;
            foreach (var item in getModels)
            {
                //parse with XML model for the output
                RequestXMLModel xmlModel = new RequestXMLModel
                {
                    Index = item.Index,
                    Content = new ContentXMLModel
                    {
                        Name = item.Name,
                        Visits = item.Visits,
                        dateRequested = item.Date.ToString("yyyy-MM-dd")
                    }
                };
                //serialize
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(RequestXMLModel));
                //serialization string
                var serializedItem = "";
                //clear namespaces
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                //use writer
                using (StringWriter writer = new Utf8StringWriter())
                {
                    //serializes it
                    xmlSerializer.Serialize(writer, xmlModel, ns);
                    serializedItem = writer.ToString();
                }
                //clear \r\n
                serializedItem = serializedItem.Replace("\r\n", string.Empty);
                //creates new XML document
                XmlDocument xmlFile = new XmlDocument();
                //loads string into the document
                xmlFile.LoadXml(serializedItem);
                //save path
                string savePath = Path.Combine(saveFolder, xmlModel.Content.dateRequested);
                //saves the file
                xmlFile.Save(savePath);
            }
        }
    }
}
