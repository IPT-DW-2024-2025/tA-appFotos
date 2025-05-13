namespace AppFotos.Models.ViewModels {
   public class FotografiaDTObyUser {

      /// <summary>
      /// título da fotografia
      /// </summary>
      public string Titulo { get; set; } = "";

      /// <summary>
      /// Descrição da fotografia
      /// </summary>
      public string? Descricao { get; set; }

      /// <summary>
      /// Nome do ficheiro da fotografia no disco rígido
      /// do servidor
      /// </summary>
      public string Ficheiro { get; set; } = null!;

      /// <summary>
      /// Data em que a fotografia foi tirada
      /// </summary>
      public DateTime Data { get; set; }

      /// <summary>
      /// Nome do dono da fotografia
      /// </summary>
      public string NomeFotografo { get; set; } = "";


    }
}
