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

namespace ExcelToDbfConvertor.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment _environment;

        public HomeController(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Selectie fisiere pentru convertire.";

            return View();
        }

        public IActionResult AboutAjax()
        {
            ViewData["Message"] = "Selectie fisiere pentru convertire.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Contact.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
        
        [HttpPost]
       // public async Task<IActionResult> Index(ICollection<IFormFile> files)
        public  IActionResult UploadAndExport(ICollection<IFormFile> files)
        {
            string uploads = Path.Combine(_environment.WebRootPath, "uploads");
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                    {
                        if (file.FileName.EndsWith(".xlsx") || file.FileName.EndsWith(".xls"))// Important for security if saving in webroot
                        {
                            //await file.CopyToAsync(fileStream);
                            file.CopyTo(fileStream);
                            string fileName = file.FileName;
                            ExportToDbf( Path.Combine(uploads, fileName));
                        }
                    }
                }
            }



            ViewData["Message"] = "S-au incarcat pe server " + files.Count + " fisiere.";

            return View("About");
        }

 
        private void ExportToDbf(string pathToFile)
        {
            string pathInConnectionString = string.Format("{0}/{1}", Path.GetDirectoryName(pathToFile), Path.GetFileName(pathToFile));

            try
            {
                    string extension = System.IO.Path.GetExtension(pathToFile).ToLower();

                    string connString = "";

                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };


                    if (validFileTypes.Contains(extension))
                    {

                        if (extension == ".csv")
                        {
                        DataTable dt = Convertors.ExcelToDbfConvertor.ConvertCSVtoDataTable(pathToFile);
                            ViewBag.Data = dt;
                        }
                        //Connection String to Excel Workbook  
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathInConnectionString + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                        DataTable dt = Convertors.ExcelToDbfConvertor.ConvertXSLXtoDataTable(pathToFile, connString);
                        Convertors.ExcelToDbfConvertor.DataTableToDBF(dt, Path.GetDirectoryName(pathToFile));
                            ViewBag.Data = dt;
                        }
                        else if (extension.Trim() == ".xlsx")
                        {
                            connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"", pathInConnectionString);
                        DataTable dt = Convertors.ExcelToDbfConvertor.ConvertXSLXtoDataTable(pathToFile, connString);
                        ViewBag.FileName = Convertors.ExcelToDbfConvertor.DataTableToDBF(dt, Path.GetDirectoryName(pathToFile));
                            ViewBag.Data = dt;
                        }

                    }
                    else
                    {
                        ViewBag.Error = "Incarcati fisiere in format .xls, .xlsx or .csv !";

                    }



            }
            catch (System.Exception ex)
            {

                ViewBag.Error = ex.Message;
            }

        }
    }
}
