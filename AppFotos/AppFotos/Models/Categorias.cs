using System.ComponentModel.DataAnnotations;

namespace AppFotos.Models {

   /// <summary>
   /// categorias a que as fotografias podem ser associadas
   /// </summary>
   public class Categorias {

      /// <summary>
      /// Identificador da categoria
      /// </summary>
      [Key]
      public int Id { get; set; }

      /// <summary>
      /// Nome da categoria que será associada às fotografias
      /// </summary>
      [Required(ErrorMessage = "A {0} é de preenchimento obrigatório")]
      [StringLength(20)]
      [Display(Name = "Categoria")]
      public string Categoria { get; set; }

      /* *************************
      * Definção dos relacionamentos
      * ************************** 
      */

      /// <summary>
      /// Lista das fotografias associadas a uma categoria
      /// </summary>
      public ICollection<Fotografias> ListaFotografias { get; set; }

   }
}
