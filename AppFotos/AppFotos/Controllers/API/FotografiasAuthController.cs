using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppFotos.Data;
using AppFotos.Models;
using AppFotos.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace AppFotos.Controllers.API {
   [Route("api/[controller]")]
   [ApiController]
   [Authorize(AuthenticationSchemes ="Bearer")] // esta autrenticação será por JWT
   public class FotografiasAuthController:ControllerBase {

      private readonly ApplicationDbContext _context;

      public FotografiasAuthController(ApplicationDbContext context) {
         _context=context;
      }

      // GET: api/Fotografias
      [HttpGet]
      public async Task<ActionResult<IEnumerable<FotografiasByUserDTO>>> GetFotografias() {

         // dados da pessoa autenticada
         string? nomePessoaAutenticada = User.Identity?.Name;


         // o que tínhamos
         // // SELECT *
         // // FROM Fotografias
         // return await _context.Fotografias.ToListAsync();

         // o que pretendemos...
         // SELECT Titulo, Descricao, Data, Ficheiro
         // FROM Fotografias

         var listagemFotos = await _context.Fotografias
                                           .Where(f=>f.Dono.UserName == nomePessoaAutenticada)
                                           .OrderByDescending(f => f.Data)
                                           .Select(f => new FotografiasByUserDTO {
                                              Titulo=f.Titulo,
                                              Descricao=f.Descricao,
                                              Ficheiro=f.Ficheiro,
                                              Data=f.Data,
                                              NomeFotografo=f.Dono.Nome
                                           })
                                           .ToListAsync();
         return listagemFotos;
      }

      // GET: api/Fotografias/5
      [HttpGet("{id}")]
      public async Task<ActionResult<FotografiasByUserDTO>> GetFotografia(int id) {
       
         // falta a filtragem pelos dados da pessoa autenticada
         
         var fotografia = await _context.Fotografias
                                        .Where(f => f.Id==id)
                                        .Select(f => new FotografiasByUserDTO {
                                           Titulo=f.Titulo,
                                           Ficheiro=f.Ficheiro,
                                           Data=f.Data,
                                           NomeFotografo=f.Dono.Nome
                                        })
                                        .FirstOrDefaultAsync();

         if (fotografia==null) {
            return NotFound();
         }

         return fotografia;
      }

      // PUT: api/Fotografias/5
      // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      [HttpPut("{id}")]
      public async Task<IActionResult> PutFotografia(int id,Fotografias fotografia) {
         if (id!=fotografia.Id) {
            return BadRequest();
         }

         _context.Entry(fotografia).State=EntityState.Modified;

         try {
            await _context.SaveChangesAsync();
         }
         catch (DbUpdateConcurrencyException) {
            if (!FotografiaExiste(id)) {
               return NotFound();
            }
            else {
               throw;
            }
         }

         return NoContent();
      }

      // POST: api/Fotografias
      // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      [HttpPost]
      public async Task<ActionResult<Fotografias>> PostFotografia(Fotografias fotografia) {
         _context.Fotografias.Add(fotografia);
         await _context.SaveChangesAsync();

         return CreatedAtAction("GetFotografias",new { id = fotografia.Id },fotografia);
      }

      // DELETE: api/Fotografias/5
      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteFotografia(int id) {
         var fotografia = await _context.Fotografias.FindAsync(id);
         if (fotografia==null) {
            return NotFound();
         }

         _context.Fotografias.Remove(fotografia);
         await _context.SaveChangesAsync();

         return NoContent();
      }

      private bool FotografiaExiste(int id) {
         return _context.Fotografias.Any(e => e.Id==id);
      }
   }
}
