using CRM.Data.Migrations;
using CRM.Models.Aluno_Cliente;
using CRM.Models.Atendimento_;
using CRM.Models.Curso;
using CRM.Models.CursoAluno_;
using CRM.Models.Modalidade_;
using CRM.Models.Pagamento_;
using CRM.Models.Polo_;
using CRM.Models.Usuario_;
using Microsoft.EntityFrameworkCore;
using CRM.Models.Empresa_;

namespace CRM.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Atendimento> Atendimentos { get; set; }
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Curso> Cursos { get; set; }

        public DbSet<CursoAluno> CursoAlunos { get; set; }

        public DbSet<Modalidade> Modalidades { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }

        public DbSet<FormaPagamento> FormaPagamentos { get; set; }

        public DbSet<Empresa> Empresas { get; set; }

        public DbSet<Polo> Polos { get; set; }
        public DbSet<SessaoUsuario> SessoesUsuarios { get; set; }



    }
}