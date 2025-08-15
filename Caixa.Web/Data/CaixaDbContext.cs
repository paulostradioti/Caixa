using Caixa.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Caixa
{
    public class CaixaDbContext : DbContext
    {
        public CaixaDbContext(DbContextOptions<CaixaDbContext> options) : base(options) { }

        public DbSet<Turma> Turmas => Set<Turma>();
        public DbSet<Aluno> Alunos => Set<Aluno>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Turma>().HasIndex(turma => turma.Nome).IsUnique();

            base.OnModelCreating(modelBuilder);
        }

        internal void SeedDatabase()
        {
            if (Turmas.Count() == 0)
            {
                var turmas = new Turma[]
                {
                    new() { Nome = "Turma 1", Professor = "Professor 1" },
                    new() { Nome = "Turma 2", Professor = "Professor 2" }
                };

                Turmas.AddRange(turmas);
                SaveChanges();
            }

            if (Alunos.Count() == 0)
            {
                var turmas = Turmas.Take(2);
                foreach (var turma in turmas)
                {
                    var alunos = new Aluno[]
                    {
                        new() { Name = "Aluno 1", TurmaId = turma.Id },
                        new() { Name = "Aluno 2", TurmaId = turma.Id }
                    };
                    Alunos.AddRange(alunos);
                }
                SaveChanges();
            }
        }   
    }
}