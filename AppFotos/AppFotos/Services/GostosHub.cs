using AppFotos.Data;
using AppFotos.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace AppFotos.Services {

   /// <summary>
   /// Gestor do Signal R, 
   /// para efetuar o tratamento dos 'Gostos' de uma fotografia
   /// </summary>
   public class GostosHub:Hub {

      private readonly ApplicationDbContext _context;
      private readonly ILogger<GostosHub> _logger;

      public GostosHub(ApplicationDbContext context,ILogger<GostosHub> logger) {
         _context=context;
         _logger=logger;
      }


      /// <summary>
      /// quando há uma ligação ao Signal R,
      /// este é o PRIMEIRO método a ser executado
      /// </summary>
      /// <returns></returns>
      public override Task OnConnectedAsync() {
         _logger.LogInformation("OnConnectedAsync: {0}",Context.ConnectionId);

         // escrever aqui as ações que devem ser executadas,
         // de cada vez que um utilizador acede à página que
         // vai consumir esta classe, pela primeira vez,
         // em cada sessão de trabalho

         return base.OnConnectedAsync();
      }


      /// <summary>
      /// marcar, ou desmarcar, o 'gosto' numa Fotografia
      /// </summary>
      /// <param name="idFoto">id da fotografia sobre a qual se quer alterar o 'gosto'</param>
      [Authorize]
      public void UpdateLike(string idFoto) {
         // var auxiliar
         bool gostei=false;

         // obter dados do utilizador
         var username = Context.User.Identity.Name;
         var userBd = _context.Utilizadores.First(u => u.UserName==username);


         // Procurar pela foto
         // Se não for fornecido um possível ID, nada se faz...
         if (int.TryParse(idFoto,out int idFotoInt)) {

            var foto = _context.Fotografias.Where(f => f.Id==idFotoInt);
            // se entrar é porque a foto não existe
            if (!foto.Any()) {
               return;
            }

            // se a foto existe, o utilizador já fez like?
            var gostoEntry = _context.Gostos
                                     .Where(g => g.FotografiaFK==idFotoInt&&
                                                 g.UtilizadorFK==userBd.Id);

            // se entra é porque já existe uma entrada
            if (gostoEntry.Any()) {
               _context.Remove(gostoEntry.First());
            }
            else {
               // ainda não 'gostei' => vou marcar o 'gosto' do Utilizador autenticado
               var novoGosto = new Gostos() {
                  FotografiaFK=idFotoInt,
                  UtilizadorFK=userBd.Id,
                  Data=DateTime.Now
               };
               _context.Add(novoGosto);
               gostei = true;
            }

            _context.SaveChanges();

            // o conteúdo desta variável vai ser divulgada por todos os browsers
            // que estiverem ligados ao Servidor (será o Signal R o responsável por este trabalho...)
            var numGostos = _context.Gostos.Where(g => g.FotografiaFK==idFotoInt).Count();

            // nome do método abaixo pode ser o que quisermos,
            //      normalmente dá-se o mesmo nome do método em que é enviada a resposta
            // o que devolvemos na resposta tem de ser coerente com o código JS abaixo que vai ler a resposta 
            // JS:
            // connection.on("UpdateLike", (idFoto, numGostos,gostei) => {
            // nomes das variáveis podem mudar, mas o nome do método e o número de variáveis tem de ser igual
            Clients.All.SendAsync("UpdateLike",idFoto,numGostos,gostei);
         }
      }
   }



}
