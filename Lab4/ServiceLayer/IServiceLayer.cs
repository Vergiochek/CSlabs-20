using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DatabaseModels.Models;

namespace ServiceLayer
{
    public interface IxmlGeneratorService
    {
        public void GenerateXmlFile(SearchRes<HumanData> data, string Pathtosave, string Filename);

        public void GenerateXsdSchema(string Pathtosave, string Filename, string Shemaname);
    }

    public interface IFileTransferService
    {
        public void TransferFiles(string filename, string xsdschema, string sourcefolder, string TargetFolderPath);
    }
}
