using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppFotos.Data;
using AppFotos.Models;
using Microsoft.AspNetCore.Authorization;
using AppFotos.Models.ViewModels;

namespace AppFotos.Controllers.API {

   [Route("api/[controller]")]
   [ApiController]
   [Authorize(AuthenticationSchemes = "Bearer")]
   public class FotografiasAuthController:ControllerBase {

      private readonly ApplicationDbContext _context;

      public FotografiasAuthController(ApplicationDbContext context) {
         _context=context;
      }

      // GET: api/FotografiasAuth
      [HttpGet]
      public async Task<ActionResult<IEnumerable<FotografiaDTObyUser>>> GetFotografias() {

         // Obter o UserName partir do token JWT
         string? username = User.Identity?.Name;

         if (string.IsNullOrEmpty(username))
            return Unauthorized();

         /*
               // obter os dados do utilizador, na tabela de Autenticação,
               // com base no 'UserName', caso seja necessário mais dados...
               var user = await _context.Users
                                        .FirstOrDefaultAsync(u => u.UserName==username);
         */

         var listaFotos = _context.Fotografias
                                  .OrderBy(f => f.Id)
                                  .Where(f => f.Dono.UserName==username)
                                  .Select(f => new FotografiaDTObyUser {
                                     Titulo=f.Titulo,
                                     Descricao=f.Descricao,
                                     Data=f.Data,
                                     Ficheiro=f.Ficheiro,
                                     NomeFotografo=f.Dono.Nome
                                  });


         return await listaFotos.ToListAsync();
      }

      // GET: api/FotografiasAuth/5
      [HttpGet("{id}")]
      public async Task<ActionResult<Fotografias>> GetFotografias(int id) {
         var fotografias = await _context.Fotografias.FindAsync(id);

         if (fotografias==null) {
            return NotFound();
         }

         return fotografias;
      }

      // PUT: api/FotografiasAuth/5
      // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      [HttpPut("{id}")]
      public async Task<IActionResult> PutFotografias(int id,Fotografias fotografias) {
         if (id!=fotografias.Id) {
            return BadRequest();
         }

         _context.Entry(fotografias).State=EntityState.Modified;

         try {
            await _context.SaveChangesAsync();
         }
         catch (DbUpdateConcurrencyException) {
            if (!FotografiasExists(id)) {
               return NotFound();
            }
            else {
               throw;
            }
         }

         return NoContent();
      }

      // POST: api/FotografiasAuth
      // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      [HttpPost]
      public async Task<ActionResult<Fotografias>> PostFotografias(Fotografias fotografias) {
         _context.Fotografias.Add(fotografias);
         await _context.SaveChangesAsync();

         return CreatedAtAction("GetFotografias",new { id = fotografias.Id },fotografias);
      }

      // DELETE: api/FotografiasAuth/5
      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteFotografias(int id) {
         var fotografias = await _context.Fotografias.FindAsync(id);
         if (fotografias==null) {
            return NotFound();
         }

         _context.Fotografias.Remove(fotografias);
         await _context.SaveChangesAsync();

         return NoContent();
      }

      private bool FotografiasExists(int id) {
         return _context.Fotografias.Any(e => e.Id==id);
      }
   }
}
