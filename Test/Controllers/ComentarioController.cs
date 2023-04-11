using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test.models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentarioController : ControllerBase
    {
        private readonly AplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public ComentarioController(AplicationDbContext context, IMemoryCache memorycache)
        {

            _context = context;
            _cache = memorycache;
        }

        // GET: api/<ComentarioController>
        [HttpGet]
        public async Task<IActionResult> GetComentarios()
        {
            try
            {


                var listComentarios = await _context.Comentario.ToListAsync();


                return Ok(listComentarios);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }

        // GET api/<ComentarioController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById(int id)
        {
            try
            {

                // Creamos una clave para la caché usando la URL de la solicitud GET.
                var cacheKey = "Comentario_GetCommentById_" + id;

                MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions();
                cacheExpirationOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(30);
                cacheExpirationOptions.Priority = CacheItemPriority.Normal;




                // Intenta obtener la respuesta de la caché.
                if (_cache.TryGetValue(cacheKey, out var response))
                {
                    return Ok(response);
                }

                // Si no existe en caché, obtenemos los datos de la base de datos.
                var comentario = await _context.Comentario.FindAsync(id);

                if (comentario == null)
                {
                    return NotFound();
                }

                // Guardamos los datos en la caché.
                _cache.Set(cacheKey, comentario, cacheExpirationOptions);

                return Ok(comentario);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ComentarioController>
        [HttpPost]
        public async Task<IActionResult> PostComment([FromBody] Comentario comentario)
        {

            try
            {

                _context.Add(comentario);
                await _context.SaveChangesAsync();

                return Ok(comentario);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // PUT api/<ComentarioController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Comentario comentario)
        {
            try
            {
                if (id != comentario.Id)
                {
                    return BadRequest();
                }

                _context.Update(comentario);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Comentario actualizado con exito!" });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // DELETE api/<ComentarioController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            try
            {

                var comentario = await _context.Comentario.FindAsync(id);

                if (comentario == null)
                {
                    return NotFound();
                }

                _context.Comentario.Remove(comentario);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Comentario eliminado con exito!" });

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
    }
}
