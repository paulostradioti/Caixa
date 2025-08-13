﻿using Caixa.Web.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Caixa.Web.Controllers
{
    public class AlunosController : Controller
    {
        private readonly CaixaDbContext dbContext;

        public AlunosController(CaixaDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ActionResult Index()
        {
            var alunos = dbContext.Alunos.Include(x => x.Turma);

            return View(alunos);
        }

        public ActionResult Create()
        {
            ViewBag.Turmas = GetTurmas();

            return View();
        }

        [HttpPost]
        public ActionResult Create(Aluno aluno)
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

            var turma = dbContext.Alunos.Find(id);
            if (turma == null)
                return NotFound();

            ViewBag.Turmas = GetTurmas();
            return View(turma);
        }

        [HttpPost]
        public ActionResult Edit(Aluno aluno)
        {
            dbContext.Alunos.Update(aluno);
            dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: AlunosController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AlunosController/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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
