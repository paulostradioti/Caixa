using Caixa.Web.Data;
using Microsoft.AspNetCore.Mvc;

namespace Caixa.Web.Controllers
{
    public class TurmasController : Controller
    {
        private readonly CaixaDbContext dbContext;

        public TurmasController(CaixaDbContext dbContext)
            => this.dbContext = dbContext;

        public IActionResult Index()
            => View(dbContext.Turmas);

        public IActionResult Create()
            => View();

        [HttpPost]
        public IActionResult Create(Turma turma)
        {
            if (!ModelState.IsValid)
                return View(turma);

            var isNameDuplicated = dbContext.Turmas.Any(t => t.Nome == turma.Nome);
            if (isNameDuplicated)
            {
                ModelState.AddModelError(nameof(Turma.Nome), "O Nome da Turma deve ser único.");
                return View(turma);
            }

            dbContext.Turmas.Add(turma);
            dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(Guid? id)
        {
            if (!id.HasValue)
                return NotFound();

            var turma = dbContext.Turmas.Find(id);
            if (turma == null)
                return NotFound();

            return View(turma);
        }

        [HttpPost]
        public IActionResult Edit(Turma turma)
        {
            dbContext.Update(turma);
            dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(Guid? id)
        {
            if (!id.HasValue)
                return NotFound();

            var turma = dbContext.Turmas.Find(id);
            if (turma == null)
                return NotFound();

            return View(turma);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirm(Guid? id)
        {
            if (!id.HasValue)
                return NotFound();

            var turma = dbContext.Turmas.Find(id);
            if (turma == null)
                return NotFound();

            dbContext.Turmas.Remove(turma);
            dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
