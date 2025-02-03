using System.Collections.Generic;
using System.Threading.Tasks;
using Harmoniq.Dtos.AlbumDtos;
using Harmoniq.Dtos;
using Harmoniq.Services;
using Harmoniq.Services.AlbumServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Harmoniq.Controllers
{
    [Route("api/albums")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly AlbumService _albumService;

        public AlbumController(AlbumService albumService)
        {
            _albumService = albumService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlbumDto>>> GetAlbums()
        {
            return Ok(await _albumService.GetAllAlbumsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AlbumDto>> GetAlbum(int id)
        {
            var album = await _albumService.GetAlbumByIdAsync(id);
            return album == null ? NotFound() : Ok(album);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAlbum([FromForm] CreateAlbumDto dto)
        {
            return Created("", await _albumService.CreateAlbumAsync(dto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAlbum(int id, [FromForm] UpdateAlbumDto dto)
        {
            await _albumService.UpdateAlbumAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            await _albumService.DeleteAlbumAsync(id);
            return NoContent();
        }
    }
}
