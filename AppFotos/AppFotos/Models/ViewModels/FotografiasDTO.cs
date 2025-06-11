namespace AppFotos.Models.ViewModels {

   /// <summary>
   /// dados de uma Fotografia, para serem usados na API
   /// </summary>
   public class FotografiasDTO {

      /// <summary>
      /// Título da Fotografia
      /// </summary>
      public string Titulo { get; set; } = string.Empty;

      /// <summary>
      /// Descrição da Fotografia
      /// </summary>
      public string? Descricao { get; set; }

      /// <summary>
      /// Nome do ficheiro com a imagem da fotografia
      /// </summary>
      public string Ficheiro { get; set; } = "";

      /// <summary>
      /// Data em que a fotografia foi tirada
      /// </summary>
      public DateTime Data { get; set; }

   }
}
