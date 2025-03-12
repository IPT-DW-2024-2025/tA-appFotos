using AppFotos.Models;

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
