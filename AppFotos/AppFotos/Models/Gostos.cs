using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations.Schema;

namespace AppFotos.Models {

   /// <summary>
   /// regista os 'gostos' que um utilizador da app tem
   /// pelas fotografias existentes
   /// </summary>
   [PrimaryKey(nameof(UtilizadorFK),nameof(FotografiaFK))]
   public class Gostos {

      /// <summary>
      /// data em que o utilizador marcou que 
      /// 'gosta' de uma fotografia
      /// </summary>
      public DateTime Data { get; set; }

      /* *************************
      * Definção dos relacionamentos
      * ************************** 
      */


      // Relacionamentos 1-N

      /// <summary>
      /// FK para referenciar o utilizador que gosta da fotografia
      /// </summary>
      [ForeignKey(nameof(Utilizador))]
      public int UtilizadorFK { get; set; }
      /// <summary>
      ///  FK para referenciar o utilizador que gosta da fotografia
      /// </summary>
      public Utilizadores Utilizador { get; set; }

      /// <summary>
      /// FK para a fotografia que o utilizador gostou
      /// </summary>
      [ForeignKey(nameof(Fotografia))]
      public int FotografiaFK { get; set; }
      /// <summary>
      /// FK para a fotografia que o utilizador gostou
      /// </summary>
      public Fotografias Fotografia { get; set; }

   }
}
