using HtmlToPdfConvertService.Models;
using HtmlToPdfConvertService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HtmlToPdfConvertService.Controllers
{
    public class ConvertFileController : Controller
    {
        private readonly IFileConverter fileConverter;

        public ConvertFileController(IFileConverter fileConverter)
        {
            this.fileConverter = fileConverter;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(IFormFile file)
        {
            if (file == null || file.Length == 0 || !fileConverter.IsSupportedExtension(file))
            {
                return RedirectToAction(nameof(Error));
            }

            return File(
                await fileConverter.Convert(file),
                fileConverter.GetConvertedFileType(),
                fileConverter.GetConvertedFileName(file));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}