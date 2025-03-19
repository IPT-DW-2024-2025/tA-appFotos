using System.ComponentModel.DataAnnotations;

namespace AppFotos.Models {

   /// <summary>
   /// utilizadores não anónimos da aplicação
   /// </summary>
   public class Utilizadores {

      /// <summary>
      /// Identificador do utilizador
      /// </summary>
      [Key]
      public int Id { get; set; }

      /// <summary>
      /// Nome do utilizador
      /// </summary>
      [Display(Name ="Nome")]
      [StringLength(50)]
      [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
      public string Nome { get; set; }

      /// <summary>
      /// Morada do utilizador
      /// </summary>
      [Display(Name ="Morada")]
      [StringLength(50)]
      public string Morada { get; set; }

      /// <summary>
      /// Código Postal da  morada do utilizador
      /// </summary>
      [Display(Name ="Código Postal")]
      [StringLength(50)]
      [RegularExpression("[1-9][0-9]{3}-[0-9]{3} [A-Za-z ]+",
                         ErrorMessage ="No {0} só são aceites algarismos e letras inglesas.")]
      public string CodPostal { get; set; }
      /* exemplo de exp. regulares sobre o Código Postal
       * [1-9][0-9]{3}-[0-9]{3} [A-Za-z ]+  --> Portugal
       * [1-9][0-9]{3,4}-[0-9]{3,4}( [A-Za-z ]*)?  --> fora de Portugal
       */

      /// <summary>
      /// País da morada do utilizador
      /// </summary>
      [Display(Name ="País")]
      [StringLength(50)]
      public string Pais { get; set; }

      /// <summary>
      /// Número de identificação fiscal do Utilizador
      /// </summary>
      [Display(Name ="NIF")]
      [StringLength(9)]
      [RegularExpression("[1-9][0-9]{8}",ErrorMessage ="Deve escrever apenas 9 digitos no {0}")]
      [Required (ErrorMessage ="O {0} é de preenchimento obrigatório")]
      public string NIF { get; set; }

      /// <summary>
      /// número de telemóvel do utilizador
      /// </summary>
      [Display(Name ="Telemóvel")]
      [StringLength(18)]
      [RegularExpression("(([+]|00)[0-9]{1,5})?[1-9][0-9]{5,10}", ErrorMessage ="Escreva um nº de telefone. Pode adicionar indicativo do país.")]
      public string Telemovel { get; set; }
      /*  9[1236][0-9]{7}  --> nºs telemóvel nacional
       *  (([+]|00)[0-9]{1,5})?[1-9][0-9]{5,10}  -->  nºs telefone internacionais
       */


      /* *************************
     * Definição dos relacionamentos
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
