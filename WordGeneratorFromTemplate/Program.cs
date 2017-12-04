using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;

namespace WordGeneratorFromTemplate
{
    class Program
    {
        static void Main(string[] args)
        {
            //initialize files and  folder to generate document
            Initialize(out DateTime currentDate, out DirectoryInfo temporaryFolder, out FileInfo templateDocument, out FileInfo xmlDataFile);

            WmlDocument openXmlPowerToolsWmlDocument = new WmlDocument(templateDocument.FullName);
            XElement data = XElement.Load(xmlDataFile.FullName);
            bool errors = false;

            errors = Generate(currentDate, temporaryFolder, templateDocument, openXmlPowerToolsWmlDocument, data);
        }

        /// <summary>
        /// Generate final word document starting from a Template and xml data informations.
        /// </summary>
        /// <param name="currentDate">current Date used in genetayed file name.</param>
        /// <param name="temporaryFolder">use a folder to keep files</param>
        /// <param name="templateDocument">the template document</param>
        /// <param name="openXmlPowerToolsWmlDocument">an object from powerTools</param>
        /// <param name="data">xml data file</param>
        /// <returns>true or false</returns>
        private static bool Generate(DateTime currentDate, DirectoryInfo temporaryFolder, FileInfo templateDocument, WmlDocument openXmlPowerToolsWmlDocument, XElement data)
        {
            bool errors;
            WmlDocument wmlAssembledDoc = DocumentAssembler.AssembleDocument(openXmlPowerToolsWmlDocument, data, out errors);

            if (errors)
            {
                Console.WriteLine("There are some errors in template " + templateDocument + " .");
                Console.WriteLine("Watch generated document AssembledDoc.docx to see errors.");
            }

            FileInfo generatedDocument = new FileInfo(Path.Combine(temporaryFolder.FullName, string.Format("GeneratedFromTemplate{0:00}-{1:00}-{2:00}-{3:00}{4:00}{5:00}.docx", currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour, currentDate.Minute, currentDate.Second)));
            wmlAssembledDoc.SaveAs(generatedDocument.FullName);

            return errors;
        }

        /// <summary>
        /// Initialize files and  folder to generate document.
        /// This can be done from app.config
        /// </summary>
        /// <param name="currentDate"></param>
        /// <param name="temporaryFolder"></param>
        /// <param name="templateDocument"></param>
        /// <param name="xmlDataFile"></param>
        private static void Initialize(out DateTime currentDate, out DirectoryInfo temporaryFolder, out FileInfo templateDocument, out FileInfo xmlDataFile)
        {
            currentDate = DateTime.Now;
            temporaryFolder = new DirectoryInfo("Docs");
            temporaryFolder.Create();

            templateDocument = new FileInfo("Template.docx");
            xmlDataFile = new FileInfo("XMLData.xml");
        }
    }
}
