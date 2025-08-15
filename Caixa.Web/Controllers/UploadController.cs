using Caixa.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Caixa.Web.Controllers
{
    public class UploadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(UploadViewModel uploadModel)
        {
            var extensions = new[] {  ".txt", ".pdf"};
            var fileExtention = Path.GetExtension(uploadModel.Arquivo.FileName);
            if (!extensions.Contains(fileExtention))
            {
                ModelState.AddModelError(nameof(uploadModel.Arquivo), $"As únicas extensões permitidas são: {string.Join(", ", extensions)}");
                return View("Index", uploadModel);
            }

            return Ok();
        }
    }
}
