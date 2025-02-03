﻿using Harmoniq.Models;

namespace Harmoniq.Repositories.SongRepositories
{
    public interface ISongRepository
    {
        Task<IEnumerable<Song>> GetAllSongsAsync();
        Task<Song> GetSongByIdAsync(int id);
        Task AddSongAsync(Song song);
        Task UpdateSongAsync(Song song);
        Task DeleteSongAsync(int id);
    }
}
