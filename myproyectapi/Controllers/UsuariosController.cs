using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.CompilerServices;
using myproyectapi.Models;

namespace myproyectapi.Controllers
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly myproyectdbContext _context;
        private readonly JWTSettings _jwtsettings;

        public UsuariosController(myproyectdbContext context, IOptions<JWTSettings> jwtsettings)
        {
            _context = context;
            this._jwtsettings = jwtsettings.Value;
        }
        //GET: api/usuarios/login
        [HttpPost("login")]
        public async Task<ActionResult<UserWithToken>> Login([FromBody] Usuarios usuario)
        {
            var firstValidate = await _context.Usuarios
                .Where(u => u.Email == usuario.Email)
                .FirstOrDefaultAsync();
            if (firstValidate == null)
            {
                var mensaje = "Este usuario no existe.";
                return NotFound(mensaje);
            }

            usuario = await _context.Usuarios
                .Where(u => u.Email==usuario.Email & u.Pass == usuario.Pass)
                .FirstOrDefaultAsync();

            if(usuario == null)
            {
                var mensaje = "Contraseña incorrecta.";
                return Unauthorized(mensaje);
            }

            UserWithToken userWithToken = new UserWithToken(usuario);

            if(userWithToken == null)
            {
                var mensaje = "Este usuario no existe.";
                return NotFound(mensaje);
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtsettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.Nombre),
                    new Claim(ClaimTypes.Email, usuario.Email),

                }),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            userWithToken.Token = tokenHandler.WriteToken(token);
            return userWithToken;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuarios>>> GetUsuarios()
        {
            var lista_usuarios = await _context.Usuarios.ToListAsync();
            return Ok(lista_usuarios);
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuarios>> GetUsuarios(int id)
        {
            var usuarios = await _context.Usuarios.FindAsync(id);

            if (usuarios == null)
            {
                return NotFound();
            }

            return Ok(usuarios);
        }
        // PUT: api/Usuarios/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuarios(int id, Usuarios usuarios)
        {
            if (id != usuarios.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuarios).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuariosExist(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Usuarios
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Usuarios>> PostUsuarios(Usuarios usuarios)
        {
            var userExist = await _context.Usuarios.AnyAsync(user => user.Email == usuarios.Email);

            if (userExist)
            {
                return BadRequest(userExist);
            }

            _context.Usuarios.Add(usuarios);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuarios", new { id = usuarios.Id }, usuarios);

        }
        // POST: api/usuarios/email
        [HttpPost("email")]
        public ActionResult<IEnumerable<Usuarios>> ValidarUsuario(string email)
        {
            var user = UsuarioEmailExists(email);
            if (!user)
            {
                return NotFound();
            }
            return Ok();
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Usuarios>> DeleteUsuarios(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }
        private bool UsuarioEmailExists(string email)
        {
            return _context.Usuarios.Any(e => e.Email == email);
        }
        private bool UsuariosExist(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
