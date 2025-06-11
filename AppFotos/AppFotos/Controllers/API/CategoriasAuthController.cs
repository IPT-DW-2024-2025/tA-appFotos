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
   [Authorize(Roles = "admin")]

   public class CategoriasAuthController:ControllerBase {

      private readonly ApplicationDbContext _context;

      public CategoriasAuthController(ApplicationDbContext context) {
         _context=context;
      }


      // GET: api/Categorias
      /// <summary>
      /// devolve a lista com todas as Categorias
      /// </summary>
      /// <returns></returns>
      [HttpGet]
      [AllowAnonymous]
      public async Task<ActionResult<IEnumerable<Categorias>>> GetCategorias() {
         return await _context.Categorias
                              .Include(c=>c.ListaFotografias)
                              .ToListAsync();
      }

      /// <summary>
      /// GET: devolver uma Categorias, 
      /// quando a solicitação é feita através de HTTP GET
      /// </summary>
      /// <param name="id">idenrtificador da categoria pretendida</param>
      /// <returns></returns>
      [HttpGet("{id}")]
      public async Task<ActionResult<Categorias>> GetCategoria(int id) {
         var categoria = await _context.Categorias.FindAsync(id);

         if (categoria==null) {
            return NotFound();
         }

         return categoria;
      }

      // PUT: api/Categorias/5
      // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      /// <summary>
      /// Edição de uma Categoria
      /// </summary>
      /// <param name="id">idemtificação da Categoria a editar</param>
      /// <param name="categoria">Novos dados da Categoria</param>
      /// <returns></returns>
      [HttpPut("{id}")]
      public async Task<IActionResult> PutCategoria(int id,Categorias categoria) {
         if (id!=categoria.Id) {
            return BadRequest();
         }

         _context.Entry(categoria).State=EntityState.Modified;

         try {
            await _context.SaveChangesAsync();
         }
         catch (DbUpdateConcurrencyException) {

            if (!CategoriaExiste(id)) {
               return NotFound();
            }
            else {
               throw;
            }
         }

         return NoContent();
      }

      // POST: api/Categorias
      // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      /// <summary>
      /// Adição de uma Categoria
      /// </summary>
      /// <param name="categoria">dados da Categoria a adicionar</param>
      /// <returns></returns>      
      [HttpPost]
      public async Task<ActionResult<Categorias>> PostCategoria(Categorias categoria) {
         _context.Categorias.Add(categoria);
         await _context.SaveChangesAsync();

         return CreatedAtAction("GetCategorias",new { id = categoria.Id },categoria);
      }

      // DELETE: api/Categorias/5
      /// <summary>
      /// Apagar uma Categoria
      /// </summary>
      /// <param name="id">identificador da Categoria a apagar</param>
      /// <returns></returns>
      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteCategoria(int id) {
         var categoria = await _context.Categorias.FindAsync(id);
         if (categoria==null) {
            return NotFound();
         }

         _context.Categorias.Remove(categoria);
         await _context.SaveChangesAsync();

         return NoContent();
      }


      /// <summary>
      /// Determina se a Categoria existe
      /// </summary>
      /// <param name="id">identificador da Categoria a procurar</param>
      /// <returns>'true', se a categoria existe, senão 'false'</returns>
      private bool CategoriaExiste(int id) {
         return _context.Categorias.Any(e => e.Id==id);
      }
   }

}
