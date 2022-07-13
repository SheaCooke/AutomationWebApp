using Azure.Storage.Blobs;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DisplayKustoDiffToUsers.Models;
using Kusto.Data;
using Kusto.Data.Common;
using Kusto.Data.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace DisplayKustoDiffToUsers.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private string _SAConnectionString = Environment.GetEnvironmentVariable("SAConnectionString", EnvironmentVariableTarget.Process);

        private string _archiveSAConnectionString = Environment.GetEnvironmentVariable("archiveSAConnectionString", EnvironmentVariableTarget.Process);

        private string _containerName = "script";

        private string _archiveContainerName = "archive";

        private string _cluster1DiffFileName = "diff.txt";

        public HomeController()
        {

        }

        public IActionResult Index()
        {      
            ReadBlobIntoHashSet();

            return View(SchemaFromDiffFile.schemaDiffCommands);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void ReadBlobIntoHashSet()
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_SAConnectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
            BlobClient blobClient = containerClient.GetBlobClient(_cluster1DiffFileName);

            if (blobClient.Exists())
            {

                var response = blobClient.Download();

                StringBuilder element = new StringBuilder();

                bool firstElement = true;

                using (var streamReader = new StreamReader(response.Value.Content))
                {
                    while (!streamReader.EndOfStream)
                    {
                        var line = streamReader.ReadLine();

                        if (line.StartsWith("------") && !firstElement)
                        {
                            AddNewKustoCommandObjectToCollection(element.ToString());

                            element.Clear();
                        }

                        if (!String.IsNullOrWhiteSpace(line))
                        {
                            firstElement = false;
                        }

                        element.Append(line + "\n");
                    }
                    if (element.Length > 0)
                    {
                        AddNewKustoCommandObjectToCollection(element.ToString());
                    }
                }

            }

        }

        private (string, string, string) ParseDiffFile(string str)
        {

            string name = "";

            string previousSchema = "";

            string newSchema = "";

            string regexStr = @"------ (Function:|Table:) (?<objectName>[a-zA-Z0-9\.-_]+) ------\sPrevious Schema:\s(?<previousSchema>[\s\S]+)\sNew Schema:\s(?<newSchema>[\s\S]+)";
            Regex regex = new Regex(regexStr);
            Match regexMatch = regex.Match(str);

            if (regexMatch.Success)
            {
                name = regexMatch.Groups["objectName"].Value;
                previousSchema = regexMatch.Groups["previousSchema"].Value;
                newSchema = regexMatch.Groups["newSchema"].Value;
            }

            return (name, previousSchema, newSchema);
        }

        private string GetCurrentSchemaFromAGCCluster(string name, string objectType)
        {
            string AGCVersion = "";

            StringBuilder responseSB = new StringBuilder();

            var kcsb = new KustoConnectionStringBuilder("https://sync.eastus.kusto.windows.net/db2").WithAadSystemManagedIdentity();

            using (var client = KustoClientFactory.CreateCslAdminProvider(kcsb))
            {
                var query = $".show {objectType} {name}";//object type is either function or table 

                if (objectType.Equals("function"))
                {
                    using (var reader = client.ExecuteControlCommand(query))
                    {
                        while (reader.Read())
                        {
                            responseSB.Append(reader.GetString(0) + " ");
                            responseSB.Append(reader.GetString(1) + " ");
                            responseSB.Append(reader.GetString(2));
                        }
                    }
                }

                else if (objectType.Equals("table"))
                {
                    using (var reader = client.ExecuteControlCommand(query))
                    {
                        responseSB.Append(name);

                        while (reader.Read())
                        {
                            responseSB.Append(" ["+reader.GetString(0) + ":");
                            responseSB.Append(reader.GetString(1) + "] ");
                        }
                    }
                }
               
            }

            AGCVersion = responseSB.ToString();

            return AGCVersion;
        }


        [Route("Home/SendCommandToCluster/{KustoObjectName}")]//TODO: add optional parameter for using the translated version
        public IActionResult SendCommandToCluster([FromRoute]string KustoObjectName)
        {                      
            try
            {
                //get command from list with objectName
                var command = SchemaFromDiffFile.schemaDiffCommands.FirstOrDefault(x => x.KustoObjectName.Equals(KustoObjectName)).UpdatedPublicSchema;

                //connect to kusto cluster
                var kcsb = new KustoConnectionStringBuilder("https://sync.eastus.kusto.windows.net/db2").WithAadSystemManagedIdentity();

                //send command to cluster
                using (var client = KustoClientFactory.CreateCslAdminProvider(kcsb))
                {
                    client.ExecuteControlCommand(command);
                }

                SchemaFromDiffFile.schemaDiffCommands.FirstOrDefault(x => x.KustoObjectName.Equals(KustoObjectName)).CommandSentSuccesfully = true;
            }
            catch (Exception e)
            {
                return Redirect("/Home/Index");
            }

            return Redirect("/Home/Index");
        }


        [Route("Home/MoveFileToArchive/{fileName}")]
        public IActionResult MoveFileToArchive([FromRoute]string fileName)
        {

            //TODO: add switch to assing file names for multiple clusters

            //create blob in archive container
            string date = DateTime.Now.ToString("M_d_yyyy_HH:mm:ss");

            BlobServiceClient blobServiceClientArchive = new BlobServiceClient(_archiveSAConnectionString);
            BlobContainerClient containerClientArchive = blobServiceClientArchive.GetBlobContainerClient(_archiveContainerName);
            BlobClient diffBlob = containerClientArchive.GetBlobClient($"diff_{date}.txt");

            BlobServiceClient blobServiceClient = new BlobServiceClient(_SAConnectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
            BlobClient blobClient = containerClient.GetBlobClient(_cluster1DiffFileName);

            //upload content into new blob
            StringBuilder content = new StringBuilder();

            var response = blobClient.Download();

            using (var streamReader = new StreamReader(response.Value.Content))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    content.Append(line + "\n");
                }
            }

            diffBlob.Upload(GenerateStreamFromString(content.ToString())); //creates a new blob and uploads data to it 

            //delete old blob 
            DeleteTestCluster1DiffFile();

            SchemaFromDiffFile.schemaDiffCommands.Clear();

            return Redirect("/Home/Index");
        }



        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }


        
        private void DeleteTestCluster1DiffFile()
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_SAConnectionString);

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_containerName); 

            BlobClient blobClient = containerClient.GetBlobClient(_cluster1DiffFileName);

            blobClient.DeleteIfExists();
        }

        [Route("Home/DisplayLargeSchemaInNewTab/{objectName}/{value}")]
        public IActionResult DisplayLargeSchemaInNewTab([FromRoute] string objectName, [FromRoute]int value)
        {
            string schema = "";

            switch(value)
            {
                case 1:
                    schema = SchemaFromDiffFile.schemaDiffCommands.FirstOrDefault(x => x.KustoObjectName.Equals(objectName)).PreviousPublicSchema;
                    break;
                case 2:
                    schema = SchemaFromDiffFile.schemaDiffCommands.FirstOrDefault(x => x.KustoObjectName.Equals(objectName)).UpdatedPublicSchema;
                    break;
                case 3:
                    schema = SchemaFromDiffFile.schemaDiffCommands.FirstOrDefault(x => x.KustoObjectName.Equals(objectName)).CurrentAGCSchema;
                    break;
            }

            schema = TranslateFunctionBody(schema);

            (string, int) res = (schema, value);

            return View(res);
        }

        private string GetObjectType(string element)
        {
            string objectType = "";

            string normalizedELement = element.ToLower();

            if (normalizedELement.Contains(".create table") || normalizedELement.Contains(".create-merge table"))
            {
                objectType = "table";
            }
            else if (normalizedELement.Contains(".create function") || normalizedELement.Contains(".create-or-alter function"))
            {
                objectType = "function";
            }

            return objectType;
        }


        private string TranslateFunctionBody(string funcBody)
        {
            StringBuilder sb = new StringBuilder(funcBody);

            string yellowHtmlPrefix = "<mark>";//TODO: need to use @Html.Raw(variableName) in the view to get html tags to render 

            string htmpPostfix = "</mark>";

            foreach(KeyValuePair<string,string> kvp in TranslationDictionaries.USSecTranslationRules) 
            {
               if (funcBody.Contains(kvp.Key))
                {
                    sb.Replace(kvp.Key, yellowHtmlPrefix + kvp.Value + htmpPostfix);
                }
            }

            string sbString = sb.ToString();

            return sbString.Equals(funcBody) ? "No changes made" : sbString;
        }


        private void AddNewKustoCommandObjectToCollection(string element)
        {
            string name = "";

            string previousSchema = "";

            string newSchema = "";

            (name, previousSchema, newSchema) = ParseDiffFile(element);

            string objectType = GetObjectType(element);

            string currentAgcSchema = "";

            string suggestedTranslation = "";

            try
            {
                currentAgcSchema = GetCurrentSchemaFromAGCCluster(name, objectType);
            }
            catch (Exception e)
            {
                currentAgcSchema = "Function/Table is not present in database.";
            }

            //create suggested translation for functions
            if (objectType.Equals("function"))
            {
                suggestedTranslation = TranslateFunctionBody(newSchema);
            }
            else
            {
                suggestedTranslation = "N/A";
            }

            //make sure there are no duplicates 
            if (SchemaFromDiffFile.schemaDiffCommands.FirstOrDefault(x => x.KustoObjectName.Equals(name)) == null)
            {
                KustoCommandModel kcm = new KustoCommandModel(name, previousSchema, newSchema, suggestedTranslation, currentAgcSchema);

                SchemaFromDiffFile.schemaDiffCommands.Add(kcm);
            }
        }

        public IActionResult DisplayTranslationRules()
        {
            (Dictionary<string, string>, Dictionary<string, string>) translationDictionaries = (TranslationDictionaries.USSecTranslationRules, TranslationDictionaries.USNatTranslation);

            return View(translationDictionaries);
        }

        [Route("Home/ViewHilightedDifferences/{objectName}")]
        public IActionResult ViewHilightedDifferences([FromRoute] string objectName)
        {
            /*string oldText = "Some old text!!!";

            string newText = "Some new Test";*/

            string oldText = SchemaFromDiffFile.schemaDiffCommands.Where(x => x.KustoObjectName == objectName).Select(x => x.PreviousPublicSchema).FirstOrDefault();

            string newText = SchemaFromDiffFile.schemaDiffCommands.Where(x => x.KustoObjectName == objectName).Select(x => x.UpdatedPublicSchema).FirstOrDefault();

            var diffBuilder = new SideBySideDiffBuilder();

            var diff = diffBuilder.BuildDiffModel(oldText ?? string.Empty, newText ?? string.Empty);

            return View(diff);
        }
    }
}
