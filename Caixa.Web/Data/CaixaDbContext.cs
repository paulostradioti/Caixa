using Caixa.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Caixa
{
    public class CaixaDbContext : DbContext
    {
        public DbSet<Turma> Turmas => Set<Turma>();
        public DbSet<Aluno> Alunos => Set<Aluno>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dbPath = Path.Combine(localAppData, "Caixa.db");

            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Turma>().HasIndex(turma => turma.Nome).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}