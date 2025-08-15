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
            var extensions = new[] { ".exe", ".txt", ".pdf", ".mp4", ".png" };
            var fileExtention = Path.GetExtension(uploadModel.Arquivo.FileName);
            if (!extensions.Contains(fileExtention))
            {
                ModelState.AddModelError(nameof(uploadModel.Arquivo), $"As únicas extensões permitidas são: {string.Join(", ", extensions)}");
                return View("Index", uploadModel);
            }

            var name = Path.GetFileNameWithoutExtension(uploadModel.Arquivo.FileName);
            var safeName = $"{name}-{Guid.NewGuid():N}{fileExtention}";
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", safeName);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            using var stream = System.IO.File.Create(fullPath);
            uploadModel.Arquivo.CopyTo(stream);


            TempData["mensagem"] = "Sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}
