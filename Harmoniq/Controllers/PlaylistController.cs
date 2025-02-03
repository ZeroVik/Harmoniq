using System.Collections.Generic;
using System.Threading.Tasks;
using Harmoniq.Dtos.PlaylistDtos;
using Harmoniq.Dtos;
using Harmoniq.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Harmoniq.Controllers
{
    [Route("api/playlists")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private readonly PlaylistService _playlistService;

        public PlaylistController(PlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaylistDto>>> GetPlaylists()
        {
            return Ok(await _playlistService.GetAllPlaylistsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlaylistDto>> GetPlaylist(int id)
        {
            var playlist = await _playlistService.GetPlaylistByIdAsync(id);
            return playlist == null ? NotFound() : Ok(playlist);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlaylist([FromBody] CreatePlaylistDto dto)
        {
            return Created("", await _playlistService.CreatePlaylistAsync(dto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlaylist(int id, [FromBody] UpdatePlaylistDto dto)
        {
            await _playlistService.UpdatePlaylistAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaylist(int id)
        {
            await _playlistService.DeletePlaylistAsync(id);
            return NoContent();
        }
    }
}
