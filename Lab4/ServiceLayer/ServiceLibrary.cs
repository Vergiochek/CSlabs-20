using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using DataAccessLayer;
using DatabaseModels.Models;

namespace ServiceLayer
{
    public class XmlPerson : IxmlGeneratorService
    {

        public void GenerateXmlFile(SearchRes<HumanData> data, string Pathtosave, string Filename)
        {
             List<HumanData> list=new List<HumanData>();
             foreach (var variable in data.Entities)
             {
                 list.Add(variable);
             }
             
             XmlSerializer formatter = new XmlSerializer(typeof(List<HumanData>));
             using (FileStream fs = new FileStream(Path.Combine(Pathtosave, Filename),FileMode.Append))
             {
                 formatter.Serialize(fs,list);
             }
        }

        public void GenerateXsdSchema(string Pathtosave, string Filename, string Shemaname)
        {
            XmlReader reader = XmlReader.Create(Path.Combine(Pathtosave, Filename));
            XmlSchemaSet schemaSet = new XmlSchemaSet();
            XmlSchemaInference schema = new XmlSchemaInference();
            schemaSet = schema.InferSchema(reader);
            reader.Dispose();

            XmlSerializer formatter = new XmlSerializer(typeof(XmlSchema));

            using (FileStream fs = new FileStream(Path.Combine(Pathtosave, Shemaname), FileMode.OpenOrCreate))
            {
                foreach (XmlSchema s in schemaSet.Schemas())
                {
                    formatter.Serialize(fs, s);
                  
                }
            }
        }
    }

    public class Transactions : IFileTransferService
    {

        public void TransferFiles(string filename, string xsdschema, string sourcefolder, string TargetFolderPath)
        {

             string str = DateTime.Now.ToString();
             str = str.Replace(' ', '_');
             str = str.Replace('.', '_');
             str = str.Replace(':', '_');
             var k = '.';
             var i = filename.Length-1;
             while (k != filename[i])
                 i--;
                
             string file = filename.Insert(i, str);
             FileInfo file1 = new FileInfo(Path.Combine(sourcefolder, filename));
             file1.MoveTo(Path.Combine(TargetFolderPath, file));
             FileInfo file2 = new FileInfo(Path.Combine(sourcefolder, xsdschema));
             file2.MoveTo(Path.Combine(TargetFolderPath, xsdschema));
               
        }
    }

}
