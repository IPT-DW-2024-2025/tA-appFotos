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

namespace AppFotos.Controllers.API {
   [Route("api/[controller]")]
   [ApiController]
   public class FotografiasController: ControllerBase {
      private readonly ApplicationDbContext _context;

      public FotografiasController(ApplicationDbContext context) {
         _context = context;
      }

      // GET: api/Fotografias
      [HttpGet]
      public async Task<ActionResult<IEnumerable<FotografiaDTO>>> GetFotografias() {
         return await _context.Fotografias
                                 .OrderBy(f => f.Id)
                                 .Select(f => new FotografiaDTO {
                                    Titulo = f.Titulo,
                                    Descricao = f.Descricao,
                                    Data = f.Data,
                                    Ficheiro = f.Ficheiro
                                 })
                                 .ToListAsync();
      }

      /*
       *  public async Task<ActionResult<IEnumerable<DonosViewModel>>> GetDonos() {
         return await _context.Donos
                              .OrderBy(d => d.Nome)
                              .Select(d => new DonosViewModel {
                                 Id = d.Id,
                                 Nome = d.Nome + " (NIF: " + d.NIF + ")"
                              })
                              .ToListAsync();
      }

       */



      // GET: api/Fotografias/5
      [HttpGet("{id}")]
      public async Task<ActionResult<FotografiaDTO>> GetFotografias(int id) {
         var fotografia = await _context.Fotografias.FindAsync(id);

         if (fotografia == null) {
            return NotFound();
         }

         return new FotografiaDTO {
            Titulo = fotografia.Titulo,
            Data = fotografia.Data,
            Ficheiro = fotografia.Ficheiro
         };
      }




      // PUT: api/Fotografias/5
      // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      [HttpPut("{id}")]
      public async Task<IActionResult> PutFotografias(int id, Fotografias fotografia) {
         if (id != fotografia.Id) {
            return BadRequest();
         }

         _context.Entry(fotografia).State = EntityState.Modified;

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

      // POST: api/Fotografias
      // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      [HttpPost]
      public async Task<ActionResult<Fotografias>> PostFotografias(Fotografias fotografia) {
         _context.Fotografias.Add(fotografia);
         await _context.SaveChangesAsync();

         return CreatedAtAction("GetFotografias", new { id = fotografia.Id }, fotografia);
      }

      // DELETE: api/Fotografias/5
      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteFotografias(int id) {
         var fotografia = await _context.Fotografias.FindAsync(id);
         if (fotografia == null) {
            return NotFound();
         }

         _context.Fotografias.Remove(fotografia);
         await _context.SaveChangesAsync();

         return NoContent();
      }

      private bool FotografiasExists(int id) {
         return _context.Fotografias.Any(e => e.Id == id);
      }
   }
}
