using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppFotos.Data;
using AppFotos.Models;
using System.Globalization;

namespace AppFotos.Controllers {
   public class FotografiasController: Controller {

      private readonly ApplicationDbContext _context;

      public FotografiasController(ApplicationDbContext context) {
         _context = context;
      }

      // GET: Fotografias
      public async Task<IActionResult> Index() {

         /* interrogação à BD feita em LINQ <=> SQL
          * SELECT *
          * FROM Fotografias f INNER JOIN Categorias c ON f.CategoriaFK = c.Id
          *                    INNER JOIN Utilizadores u ON f.DonoFK = u.Id
          */
         var listaFotografias = _context.Fotografias.Include(f => f.Categoria).Include(f => f.Dono);

         return View(await listaFotografias.ToListAsync());
      }

      // GET: Fotografias/Details/5
      public async Task<IActionResult> Details(int? id) {
         if (id == null) {
            return NotFound();
         }

         /* interrogação à BD feita em LINQ <=> SQL
          * SELECT *
          * FROM Fotografias f INNER JOIN Categorias c ON f.CategoriaFK = c.Id
          *                    INNER JOIN Utilizadores u ON f.DonoFK = u.Id
          * WHERE f.Id = id
          */
         var fotografia = await _context.Fotografias
             .Include(f => f.Categoria)
             .Include(f => f.Dono)
             .FirstOrDefaultAsync(m => m.Id == id);
         if (fotografia == null) {
            return NotFound();
         }

         return View(fotografia);
      }




      // GET: Fotografias/Create
      public IActionResult Create() {
         // este é o primeiro método a ser chamado qd se pretende adicionar uma Fotografia

         // estes contentores servem para levar os dados das 'dropdowns' para a View
         // SELECT *
         // FROM Categorias c
         // ORDER BY c.Categoria
         ViewData["CategoriaFK"] = new SelectList(_context.Categorias.OrderBy(c => c.Categoria), "Id", "Categoria");
         // SELECT *
         // FROM Utilizadores u
         // ORDER BY u.Nome
         ViewData["DonoFK"] = new SelectList(_context.Utilizadores.OrderBy(u => u.Nome), "Id", "Nome");


         return View();
      }




      // POST: Fotografias/Create
      // To protect from overposting attacks, enable the specific properties you want to bind to.
      // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create([Bind("Titulo,Descricao,Ficheiro,Data,PrecoAux,CategoriaFK,DonoFK")] Fotografias fotografia, IFormFile imagemFoto) {
         // vars. auxiliar
         bool haErro = false;

         // Avaliar se há Categoria
         if (fotografia.CategoriaFK <= 0) {
            // Erro. Não foi escolhida uma categoria
            haErro = true;
            // crio msg de erro
            ModelState.AddModelError("", "Tem de escolher uma Categoria");
         }

         // Avaliar se há Utilizador
         if (fotografia.DonoFK <= 0) {
            // Erro. Não foi escolhida um dono
            haErro = true;
            // construo a msg de erro
            ModelState.AddModelError("", "Tem de escolher um Dono");
         }

         /* Avaliar o ficheiro fornecido
          * - há ficheiro?
          *   - se não existir ficheiro, gerar msg erro e devolver à view o controlo
          *   - se há,
          *     - será uma imagem?
          *       - se não for, gerar msg erro e devolver à view o controlo
          *       - é,
          *         - determinar novo nome do ficheiro
          *         - guardar na BD o nome do ficheiro
          *         - guardar o ficheiro no disco rígido do servidor
          */
         if (imagemFoto == null) {
            // não há imagem
            haErro = true;
            // construo a msg de erro
            ModelState.AddModelError("", "Tem de submeter uma Fotografia");
         }
         else {
            // há ficheiro. Mas, é uma imagem?

         }



         // Avalia se os dados estão de acordo com o Model
         if (ModelState.IsValid && !haErro) {
            // transferir o valor do PrecoAux para o Preco
            // há necessidade de tratar a questão do . no meio da string
            // há necessidade de garantir que a conversão é feita segundo uma 'cultura' pre-definida
            fotografia.Preco = Convert.ToDecimal(fotografia.PrecoAux.Replace('.', ','),
                                                 new CultureInfo("pt-PT"));


            // adicionar os dados da nova fotografia na BD
            _context.Add(fotografia);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
         }

         ViewData["CategoriaFK"] = new SelectList(_context.Categorias.OrderBy(c => c.Categoria), "Id", "Categoria", fotografia.CategoriaFK);
         ViewData["DonoFK"] = new SelectList(_context.Utilizadores.OrderBy(u => u.Nome), "Id", "Nome", fotografia.DonoFK);

         // Se chego aqui é pq algo correu mal...
         return View(fotografia);
      }




      // GET: Fotografias/Edit/5
      public async Task<IActionResult> Edit(int? id) {
         if (id == null) {
            return NotFound();
         }

         var fotografia = await _context.Fotografias.FindAsync(id);
         if (fotografia == null) {
            return NotFound();
         }
         ViewData["CategoriaFK"] = new SelectList(_context.Categorias.OrderBy(c => c.Categoria), "Id", "Categoria", fotografia.CategoriaFK);
         ViewData["DonoFK"] = new SelectList(_context.Utilizadores.OrderBy(u => u.Nome), "Id", "Nome", fotografia.DonoFK);
         return View(fotografia);
      }

      // POST: Fotografias/Edit/5
      // To protect from overposting attacks, enable the specific properties you want to bind to.
      // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descricao,Ficheiro,Data,Preco,CategoriaFK,DonoFK")] Fotografias fotografia) {
         if (id != fotografia.Id) {
            return NotFound();
         }

         if (ModelState.IsValid) {
            try {
               _context.Update(fotografia);
               await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
               if (!FotografiasExists(fotografia.Id)) {
                  return NotFound();
               }
               else {
                  throw;
               }
            }
            return RedirectToAction(nameof(Index));
         }
         ViewData["CategoriaFK"] = new SelectList(_context.Categorias, "Id", "Categoria", fotografia.CategoriaFK);
         ViewData["DonoFK"] = new SelectList(_context.Utilizadores, "Id", "Id", fotografia.DonoFK);
         return View(fotografia);
      }

      // GET: Fotografias/Delete/5
      public async Task<IActionResult> Delete(int? id) {
         if (id == null) {
            return NotFound();
         }

         var fotografia = await _context.Fotografias
             .Include(f => f.Categoria)
             .Include(f => f.Dono)
             .FirstOrDefaultAsync(m => m.Id == id);
         if (fotografia == null) {
            return NotFound();
         }

         return View(fotografia);
      }

      // POST: Fotografias/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> DeleteConfirmed(int id) {
         var fotografia = await _context.Fotografias.FindAsync(id);
         if (fotografia != null) {
            _context.Fotografias.Remove(fotografia);
         }

         await _context.SaveChangesAsync();
         return RedirectToAction(nameof(Index));
      }

      private bool FotografiasExists(int id) {
         return _context.Fotografias.Any(e => e.Id == id);
      }
   }
}
