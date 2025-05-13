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

namespace AppFotos.Controllers.API {

   [Route("api/[controller]")]
   [ApiController]
   [Authorize(AuthenticationSchemes = "Bearer")]
   public class CategoriasAuthController:ControllerBase {

      private readonly ApplicationDbContext _context;

      public CategoriasAuthController(ApplicationDbContext context) {
         _context=context;
      }

      // GET: api/CategoriasAuth
      [HttpGet]
      public async Task<ActionResult<IEnumerable<Categorias>>> GetCategorias() {
         return await _context.Categorias
                              .Include(c => c.ListaFotografias)
                              .ToListAsync();
      }

      // GET: api/CategoriasAuth/5
      [HttpGet("{id}")]
      public async Task<ActionResult<Categorias>> GetCategorias(int id) {
         var categorias = await _context.Categorias
                                        .Include(c => c.ListaFotografias)
                                        .Where(c => c.Id==id)
                                        .FirstOrDefaultAsync();

         if (categorias==null) {
            return NotFound();
         }

         return categorias;
      }

      // PUT: api/CategoriasAuth/5
      // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      [HttpPut("{id}")]
      public async Task<IActionResult> PutCategorias(int id,Categorias categorias) {
         if (id!=categorias.Id) {
            return BadRequest();
         }

         _context.Entry(categorias).State=EntityState.Modified;

         try {
            await _context.SaveChangesAsync();
         }
         catch (DbUpdateConcurrencyException) {
            if (!CategoriasExists(id)) {
               return NotFound();
            }
            else {
               throw;
            }
         }

         return NoContent();
      }

      // POST: api/CategoriasAuth
      // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      [HttpPost]
      public async Task<ActionResult<Categorias>> PostCategorias(Categorias categorias) {
         _context.Categorias.Add(categorias);
         await _context.SaveChangesAsync();

         return CreatedAtAction("GetCategorias",new { id = categorias.Id },categorias);
      }

      // DELETE: api/CategoriasAuth/5
      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteCategorias(int id) {
         var categorias = await _context.Categorias.FindAsync(id);
         if (categorias==null) {
            return NotFound();
         }

         _context.Categorias.Remove(categorias);
         await _context.SaveChangesAsync();

         return NoContent();
      }

      private bool CategoriasExists(int id) {
         return _context.Categorias.Any(e => e.Id==id);
      }
   }
}
