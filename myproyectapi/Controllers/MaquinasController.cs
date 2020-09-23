using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myproyectapi.Models;

namespace myproyectapi.Controllers
{
    [Authorize]
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class MaquinasController : ControllerBase
    {
        private readonly myproyectdbContext _context;

        public MaquinasController(myproyectdbContext context)
        {
            _context = context;
        }

        // GET: api/Maquinas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Maquinas>>> GetMaquinas()
        {
            var lista_maquinas = await _context.Maquinas.ToListAsync();
            return Ok(lista_maquinas);
        }

        // GET: api/Maquinas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Maquinas>> GetMaquinas(int id)
        {
            var maquinas = await _context.Maquinas.FindAsync(id);

            if (maquinas == null)
            {
                return NotFound();
            }

            return Ok(maquinas);
        }
        // GET: api/Maquinas/usuario/{usuarioId}
        [HttpGet("usuario/{usuarioId}")]
        public ActionResult<IEnumerable<Maquinas>> GetMaquinasUsuario(int usuarioId)
        {
            var collection = _context.Maquinas as IQueryable<Maquinas>;

            collection = collection.Where(vm => vm.UsuarioId == usuarioId);

            if (collection == null){
                return NotFound();
            }
            return Ok(collection.ToList());
        }

        // PUT: api/Maquinas/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaquinas(int id, Maquinas maquinas)
        {
            if (maquinas == null)
            {
                return NoContent();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("El modelo no es valido.");
            }
            if (id != maquinas.Id)
            {
                return BadRequest("Está maquina no existe");
            }

            var originalMachine = await _context.Maquinas.FindAsync(id);

            if (originalMachine == maquinas)
            {
                return BadRequest("Sin modificaciones.");
            }

            _context.Entry(maquinas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaquinasExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }

            return Ok();
        }

        // POST: api/Maquinas
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Maquinas>> PostMaquinas(Maquinas maquinas)
        {
            _context.Maquinas.Add(maquinas);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMaquinas", new { id = maquinas.Id }, maquinas);
        }

        // DELETE: api/Maquinas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Maquinas>> DeleteMaquinas(int id)
        {
            var maquinas = await _context.Maquinas.FindAsync(id);
            if (maquinas == null)
            {
                return NotFound();
            }

            _context.Maquinas.Remove(maquinas);
            await _context.SaveChangesAsync();

            return maquinas;
        }

        private bool MaquinasExists(int id)
        {
            return _context.Maquinas.Any(e => e.Id == id);
        }
    }
}
