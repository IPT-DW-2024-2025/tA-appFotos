namespace AppFotos.Models.ViewModels {

   /// <summary>
   /// dados de uma Fotografia, de um determinado fotógrafo, para serem usados na API
   /// </summary>
   public class FotografiasByUserDTO:FotografiasDTO {

      /// <summary>
      /// Nome do dono da fotografia: o fotógrafo
      /// </summary>
      public string NomeFotografo { get; set; } = "";

   }
}
