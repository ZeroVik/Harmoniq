using System.Collections.Generic;
using System.Threading.Tasks;
using Harmoniq.Dtos.ArtistDtos;
using Harmoniq.Dtos;
using Harmoniq.Services;
using Harmoniq.Services.ArtistService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Harmoniq.Controllers
{
    [Route("api/artists")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly ArtistService _artistService;

        public ArtistController(ArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtistDto>>> GetArtists()
        {
            return Ok(await _artistService.GetAllArtistsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArtistDto>> GetArtist(int id)
        {
            var artist = await _artistService.GetArtistByIdAsync(id);
            return artist == null ? NotFound() : Ok(artist);
        }

        [HttpPost]
        public async Task<IActionResult> CreateArtist([FromForm] CreateArtistDto dto)
        {
            return Created("", await _artistService.CreateArtistAsync(dto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArtist(int id, [FromForm] UpdateArtistDto dto)
        {
            await _artistService.UpdateArtistAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtist(int id)
        {
            await _artistService.DeleteArtistAsync(id);
            return NoContent();
        }
    }
}
