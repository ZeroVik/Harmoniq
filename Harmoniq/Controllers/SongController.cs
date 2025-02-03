using System.Collections.Generic;
using System.Threading.Tasks;
using Harmoniq.Dtos.SongDtos;
using Harmoniq.Dtos;
using Harmoniq.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Harmoniq.Controllers
{
    [Route("api/songs")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private readonly SongService _songService;

        public SongController(SongService songService)
        {
            _songService = songService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SongDto>>> GetSongs()
        {
            return Ok(await _songService.GetAllSongsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SongDto>> GetSong(int id)
        {
            var song = await _songService.GetSongByIdAsync(id);
            return song == null ? NotFound() : Ok(song);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSong([FromForm] CreateSongDto dto)
        {
            return Created("", await _songService.CreateSongAsync(dto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSong(int id, [FromBody] UpdateSongDto dto)
        {
            await _songService.UpdateSongAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSong(int id)
        {
            await _songService.DeleteSongAsync(id);
            return NoContent();
        }
    }
}
