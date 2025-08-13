using Caixa.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Caixa.Web.Controllers
{
    public class AlunosController : Controller
    {
        private readonly CaixaDbContext dbContext;

        public AlunosController(CaixaDbContext dbContext)
            => this.dbContext = dbContext;

        public IActionResult Index()
        {
            var alunos = dbContext.Alunos.Include(x => x.Turma);
            return View(alunos);
        }

        public IActionResult Create()
        {
            ViewBag.Turmas = GetTurmas();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Aluno aluno)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Turmas = GetTurmas();
                return View(aluno);
            }

            dbContext.Alunos.Add(aluno);
            dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Edit(Guid? id)
        {
            if (!id.HasValue)
                return NotFound();

            var aluno = dbContext.Alunos.Find(id);
            if (aluno == null)
                return NotFound();

            ViewBag.Turmas = GetTurmas();
            return View(aluno);
        }

        [HttpPost]
        public IActionResult Edit(Aluno aluno)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Turmas = GetTurmas();
                return View(aluno);
            }

            dbContext.Alunos.Update(aluno);
            dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(Guid? id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult DeleteConfirm(Guid? id)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<SelectListItem> GetTurmas()
        {
            var turmas = dbContext.Turmas.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Nome
            }).ToList();

            turmas.Insert(0, new SelectListItem { Value = "", Text = "Selecione uma Turma" });

            return turmas;
        }
    }
}
