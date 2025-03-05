using System.ComponentModel.DataAnnotations;

namespace AppFotos.Models {

   /// <summary>
   /// utilizadores não anónimos da aplicação
   /// </summary>
   public class Utilizadores {

      /// <summary>
      /// Identificador do utilizador
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Nome do utilizador
      /// </summary>
      public string Nome { get; set; }

      /// <summary>
      /// Morada do utilizador
      /// </summary>
      public string Morada { get; set; }

      /// <summary>
      /// Código Postal da  morada do utilizador
      /// </summary>
      public string CodPostal { get; set; }

      /// <summary>
      /// País da morada do utilizador
      /// </summary>
      public string Pais { get; set; }

      /// <summary>
      /// Número de identificação fiscal do Utilizador
      /// </summary>
      public string NIF { get; set; }

      /// <summary>
      /// número de telemóvel do utilizador
      /// </summary>
      public string Telemovel { get; set; }


      /* *************************
     * Definção dos relacionamentos
     * ************************** 
     */

      /// <summary>
      /// Lista das fotografias que são propriedade do 
      /// utilizador
      /// </summary>
      public ICollection<Fotografias> ListaFotos { get; set; }

      /// <summary>
      /// Lista dos 'gostos' de fotografias do utilizador
      /// </summary>
      public ICollection<Gostos> ListaGostos { get; set; }

      /// <summary>
      /// Lista das fotografias compradas pelo utilizador
      /// </summary>
      public ICollection<Compras> ListaCompras { get; set; }



   }
}
