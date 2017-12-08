using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using Convertors;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
//using OfficeOpenXml;
using System;

namespace ExcelToDbfConvertor.Controllers.Controllers
{
    public class ImportExportController : Controller
    {
        private IHostingEnvironment _environment;

        public ImportExportController(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        /// <summary>
        /// Upload and export
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult UploadAndExport()
        {
            ViewData["Message"] = "Selectie fisier Excel pentru convertire in *.dbf .";

            return View();
        }

        [HttpGet]
        public IActionResult UploadAndExportToXML()
        {
            ViewData["Message"] = "Selectie fisier Excel pentru convertire in *.xml .";

            return View();
        }


        /// <summary>
        /// From //http://www.talkingdotnet.com/import-export-xlsx-asp-net-core/
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UploadAndExport(ICollection<IFormFile> files)
        {
            DataTable dtExcel;

            foreach (var file in files)
            {
                try
                {

                    string uploads = Path.Combine(_environment.WebRootPath, "uploads");

                    string pathToFile = Path.Combine(uploads, file.FileName);

                    using (var memoryStream = new MemoryStream())
                    {
                        file.CopyTo(memoryStream);

                        dtExcel = Convertors.ExcelToDbfConvertor.ExcelToDataTable(memoryStream);
                    }

                    ViewBag.FileName = Convertors.ExcelToDbfConvertor.DataTableToDBF(dtExcel, Path.GetDirectoryName(pathToFile));
                    ViewBag.Data = dtExcel;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            ViewData["Message"] = "S-a incarcat pe server " + files.Count + " fisier.";

            return View("UploadAndExport");
        }

        /// <summary>
        /// See https://code.msdn.microsoft.com/office/How-to-convert-excel-file-7a9bb404/view/SourceCode#content
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UploadAndExportToXML(ICollection<IFormFile> files)
        {
            DataTable dtExcel;

            foreach (var file in files)
            {
                try
                {

                    string uploads = Path.Combine(_environment.WebRootPath, "uploads");

                    string pathToFile = Path.Combine(uploads, file.FileName);

                    ExcelToXmlConvertor excelToXmlConvertor = new ExcelToXmlConvertor();
                    string xmlFormatString = excelToXmlConvertor.GetXML(pathToFile);

                    
                    ViewBag.FileName = excelToXmlConvertor.FileName;
                    ViewBag.Data = xmlFormatString;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            ViewData["Message"] = "S-a incarcat pe server " + files.Count + " fisier.";

            return View("UploadAndExportToXML");
        }



    }
    //public static class ExcelPackageExtensions
    //{
    //    public static DataTable ToDataTable(this ExcelPackage package)
    //    {
    //        ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
    //        DataTable table = new DataTable();
    //        foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
    //        {
    //            table.Columns.Add(firstRowCell.Text);
    //        }
    //        for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
    //        {
    //            var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
    //            var newRow = table.NewRow();
    //            foreach (var cell in row)
    //            {
    //                newRow[cell.Start.Column - 1] = cell.Text;
    //            }
    //            table.Rows.Add(newRow);
    //        }
    //        return table;
    //    }
    //}
}