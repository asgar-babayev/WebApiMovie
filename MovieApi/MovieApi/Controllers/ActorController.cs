using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.DAL;
using MovieApi.DTOs;
using MovieApi.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorController : ControllerBase
    {
        readonly Context _context;

        public ActorController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Actors.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            Actor actor = await _context.Actors.FindAsync(id);
            if (actor is null) return StatusCode(StatusCodes.Status404NotFound);
            return Ok(actor);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(ActorCreateDto actorDto)
        {
            if (actorDto == null) return StatusCode(StatusCodes.Status400BadRequest);
            Actor actor = new Actor
            {
                FullName = actorDto.FullName,
                ImageUrl = actorDto.ImageUrl,
                IsDeleted = false
            };
            await _context.Actors.AddAsync(actor);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, ActorUpdateDto actorDto)
        {
            if (actorDto == null) return StatusCode(StatusCodes.Status400BadRequest);
            Actor actor = await _context.Actors.FindAsync(id);
            if(actor == null) return StatusCode(StatusCodes.Status404NotFound);
            actor.FullName = actorDto.FullName;
            actor.ImageUrl = actorDto.ImageUrl;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Actor actor = await _context.Actors.FindAsync(id);
            if (actor == null) return StatusCode(StatusCodes.Status404NotFound);
            _context.Actors.Remove(actor);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
