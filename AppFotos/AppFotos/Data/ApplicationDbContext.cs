using AppFotos.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppFotos.Data {


   /// <summary>
   /// Esta classe representa a Base de Dados associada ao projeto
   /// Se houver mais bases de dados, irão haver tantas classes
   /// deste tipo, quantas as necessárias
   /// 
   /// esta classe é equivalente a CREATE SCHEMA no SQL
   /// </summary>
   public class ApplicationDbContext: IdentityDbContext {

      /// <summary>
      /// Construtor
      /// </summary>
      /// <param name="options"></param>
      public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options) {
      }


      // este código deve ser colocado dentro da classe 'ApplicationDbContext'
      // serve para adicionar às Migrações um conjunto de registos que devem estar sempre presentes na 
      // base de dados do projeto, desde o seu início.
      // Esta técnica NÃO É ADEQUADA para a criação de dados de teste!!!

      /// <summary>
      /// este método é executado imediatamente antes 
      /// da criação da base de dados. <br />
      /// É utilizado para adicionar as últimas instruções
      /// à criação do modelo
      /// </summary>
      protected override void OnModelCreating(ModelBuilder modelBuilder) {
         // 'importa' todo o comportamento do método, 
         // aquando da sua definição na SuperClasse
         base.OnModelCreating(modelBuilder);

         // criar os perfis de utilizador da nossa app
         modelBuilder.Entity<IdentityRole>().HasData(
             new IdentityRole { Id = "a", Name = "admin", NormalizedName = "ADMIN" });

         // criar um utilizador para funcionar como ADMIN
         // função para codificar a password
         var hasher = new PasswordHasher<IdentityUser>();
         // criação do utilizador
         modelBuilder.Entity<IdentityUser>().HasData(
             new IdentityUser {
                Id = "admin",
                UserName = "admin@mail.pt",
                NormalizedUserName = "ADMIN@MAIL.PT",
                Email = "admin@mail.pt",
                NormalizedEmail = "ADMIN@MAIL.PT",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                PasswordHash = hasher.HashPassword(null, "Aa0_aa")
             }
         );
         // Associar este utilizador à role ADMIN
         modelBuilder.Entity<IdentityUserRole<string>>().HasData(
             new IdentityUserRole<string> { UserId = "admin", RoleId = "a" });


         /* ------------------------------------------------------------------ */

         // eventualmente, adicionar mais dados à BD
         // SÓ DEVERÁ SER ADICIONADA DESTA FORMA DADOS QUE SE DESEJAM MANTER
         // APÓS O FIM DO DESENVOLVIMENTO
         // dados de teste, NUNCA DEVEM aqui ser colocados
         // neste exercício, podemos adicionar algumas Categorias
         modelBuilder.Entity<Categorias>().HasData(
             new Categorias { Id = 1, Categoria = "Flores" },
             new Categorias { Id = 2, Categoria = "Animais" },
             new Categorias { Id = 3, Categoria = "Mar" },
             new Categorias { Id = 4, Categoria = "Pessoas" },
             new Categorias { Id = 5, Categoria = "Casas" }
         );
         /* ------------------------------------------------------------------ */
      }





      // especificar as tabelas associadas à BD
      /// <summary>
      /// tabela Categorias na BD
      /// </summary>
      public DbSet<Categorias> Categorias { get; set; }
      /// <summary>
      /// tabela Utilizadores
      /// </summary>
      public DbSet<Utilizadores> Utilizadores { get; set; }
      /// <summary>
      /// tabela Fotografias
      /// </summary>
      public DbSet<Fotografias> Fotografias { get; set; }
      /// <summary>
      /// tabela Compras
      /// </summary>
      public DbSet<Compras> Compras { get; set; }
      /// <summary>
      /// tabela Gostos
      /// </summary>
      public DbSet<Gostos> Gostos { get; set; }


   }
}
