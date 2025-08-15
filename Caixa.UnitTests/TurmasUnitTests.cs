using Caixa.Web.Controllers;
using Caixa.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Caixa.UnitTests
{
    public class TurmasUnitTests
    {
        private CaixaDbContext GetDbContext() 
        {
            //.UseSqlite("Data Source=:memory:")
            var options = new DbContextOptionsBuilder<CaixaDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;

            return new CaixaDbContext(options);
        }

        [Fact]
        public void Create_ShouldAddNewTurma_WhenTurmaIsValid()
        {
            // Arrange
            var context = GetDbContext();
            var controller = new TurmasController(context);
            var turma = new Turma() { Nome = "Turma 1", Professor = "Professor 1" };

            // Act
            var result = controller.Create(turma);

            // Assert
            var redirectionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectionResult.ActionName);
            Assert.Single(context.Turmas);
            Assert.Equal(turma.Nome, context.Turmas.First().Nome);
        }


        [Fact]
        public void Create_ShouldFail_WhenTurmaIsInvalid()
        {
            // Arrange
            var context = GetDbContext();
            var controller = new TurmasController(context);
            var turma = new Turma() { Nome = "T", Professor = "Professor 1" };

            var validationContext = new ValidationContext(turma, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(turma, validationContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError(
                    validationResult.MemberNames.FirstOrDefault() ?? string.Empty,
                    validationResult.ErrorMessage
                );
            }

            // Act
            var result = controller.Create(turma);

            Assert.False(controller.ModelState.IsValid);
        }

        [Fact]
        public void Index_ShouldReturnTurmas()
        {
            // Arrange
            var context = GetDbContext();
            var turma = new Turma() { Nome = "T", Professor = "Professor 1" };
            context.Turmas.Add(turma);
            context.SaveChanges();

            var controller = new TurmasController(context);

            // Act
            var result = controller.Index();

            //Assert
            var viewResult = (ViewResult)result;
            var model = (IQueryable<Turma>)viewResult.Model;
            Assert.Single(model);
        }
    }
}