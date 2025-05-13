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
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AppFotos.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using AppFotos.Services;

namespace AppFotos.Controllers.API {
   [Route("api/[controller]")]
   [ApiController]
   public class AuthController: ControllerBase {
      private readonly ApplicationDbContext _context;
      private readonly UserManager<IdentityUser> _userManager;
      private readonly SignInManager<IdentityUser> _signInManager;
      private readonly IConfiguration _config;

      public AuthController(ApplicationDbContext context,
         UserManager<IdentityUser> userManager,
         SignInManager<IdentityUser> signInManager,
         IConfiguration config) {
         _context = context;
         _userManager = userManager;
         _signInManager = signInManager;
         _config = config;
      }


      [AllowAnonymous]
      [HttpPost("login")]
      public async Task<IActionResult> Login([FromBody] LoginModel login) { 

         var user = await _userManager.FindByEmailAsync(login.Username);
         if (user == null) return Unauthorized();

         var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
         if (!result.Succeeded) return Unauthorized();

         var token = GenerateJwtToken(login.Username);
         
         return Ok(new { token });
         /*


         if (login.Username == "user" && login.Password == "1234") {

            var token = GenerateJwtToken(login.Username);
            return Ok(new { token });
         */
         }

       
      private string GenerateJwtToken(string username) {
         var claims = new[] {
            new Claim(ClaimTypes.Name, username)
        };

         var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(s: _config["Jwt:Key"]));
         var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

         var token = new JwtSecurityToken(
             issuer: _config["Jwt:Issuer"],
             audience: _config["Jwt:Audience"],
             claims: claims,
             expires: DateTime.Now.AddHours(2),
             signingCredentials: creds);

         return new JwtSecurityTokenHandler().WriteToken(token);
      }
   


/*







   // GET: api/Auth
   [HttpGet]
      public async Task<ActionResult<IEnumerable<Categorias>>> GetCategorias() {
         return await _context.Categorias.ToListAsync();
      }

      // GET: api/Auth/5
      [HttpGet("{id}")]
      public async Task<ActionResult<Categorias>> GetCategorias(int id) {
         var categorias = await _context.Categorias.FindAsync(id);

         if (categorias == null) {
            return NotFound();
         }

         return categorias;
      }

      // PUT: api/Auth/5
      // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      [HttpPut("{id}")]
      public async Task<IActionResult> PutCategorias(int id, Categorias categorias) {
         if (id != categorias.Id) {
            return BadRequest();
         }

         _context.Entry(categorias).State = EntityState.Modified;

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

      // POST: api/Auth
      // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      [HttpPost]
      public async Task<ActionResult<Categorias>> PostCategorias(Categorias categorias) {
         _context.Categorias.Add(categorias);
         await _context.SaveChangesAsync();

         return CreatedAtAction("GetCategorias", new { id = categorias.Id }, categorias);
      }

      // DELETE: api/Auth/5
      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteCategorias(int id) {
         var categorias = await _context.Categorias.FindAsync(id);
         if (categorias == null) {
            return NotFound();
         }

         _context.Categorias.Remove(categorias);
         await _context.SaveChangesAsync();

         return NoContent();
      }

      private bool CategoriasExists(int id) {
         return _context.Categorias.Any(e => e.Id == id);
      }

      */


   }
}
