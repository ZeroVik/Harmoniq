using System.Collections.Generic;
using System.Threading.Tasks;
using Harmoniq.Dtos.PlaylistSongDtos;
using Harmoniq.Dtos;
using Harmoniq.Models;
using Harmoniq.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Harmoniq.Controllers
{
    [Route("api/playlistsongs")]
    [ApiController]
    public class PlaylistSongController : ControllerBase
    {
        private readonly PlaylistSongService _playlistSongService;

        public PlaylistSongController(PlaylistSongService playlistSongService)
        {
            _playlistSongService = playlistSongService;
        }

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddSongToPlaylist([FromBody] PlaylistSongDto dto)
        {
            var result = await _playlistSongService.AddSongToPlaylistAsync(dto);
            if (!result)
                return BadRequest(new { message = "Invalid Playlist or Song ID" });

            return Ok(new { message = "Song added to playlist successfully" });
        }

        [HttpDelete("remove/{playlistId}/{songId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveSongFromPlaylist(int playlistId, int songId)
        {
            await _playlistSongService.RemoveSongFromPlaylistAsync(playlistId, songId);
            return Ok(new { message = "Song removed from playlist" });
        }

        [HttpGet("{playlistId}/songs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Song>>> GetSongsByPlaylist(int playlistId)
        {
            var songs = await _playlistSongService.GetSongsByPlaylistIdAsync(playlistId);
            if (songs == null || songs.Count == 0)
                return NotFound(new { message = "No songs found in this playlist" });

            return Ok(songs);
        }
    }
}
