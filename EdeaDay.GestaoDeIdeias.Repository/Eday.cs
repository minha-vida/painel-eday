using System.Data.Entity;
using EdeaDay.GestaoDeIdeias.Model;

namespace EdeaDay.GestaoDeIdeias.Repository
{
    public class Eday : DbContext
    {
        public DbSet<Ideia> Ideias { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Colaborador> Colaboradores { get; set; }

        public DbSet<Vaga> Vagas { get; set; }

        public DbSet<Brainstorm> Brainstorm { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ideia>()
            .HasKey(ideia => ideia.IdeiaID);

            modelBuilder.Entity<Usuario>()
                .HasKey(usuario => usuario.UsuarioID);

            modelBuilder.Entity<Colaborador>()
                .HasKey(colaborador => new { colaborador.ColaboradorID, colaborador.IdeiaID, colaborador.UsuarioID, colaborador.Cargo });

            modelBuilder.Entity<Ideia>()
                .HasMany(ideia => ideia.Envolvidos)
                .WithRequired(colaborador => colaborador.Ideia)
                .HasForeignKey(colaborador => colaborador.IdeiaID);

            modelBuilder.Entity<Usuario>()
                .HasMany(usuario => usuario.Ideias)
                .WithRequired(colaborador => colaborador.Usuario)
                .HasForeignKey(colaborador => colaborador.UsuarioID);
        }
    }
}
