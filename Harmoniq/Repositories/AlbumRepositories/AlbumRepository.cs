using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Harmoniq.Models;
using Microsoft.EntityFrameworkCore;

namespace Harmoniq.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly ApplicationDbContext _context;

        public AlbumRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Album>> GetAllAlbumsAsync()
        {
            return await _context.Albums.Include(a => a.Artist).ToListAsync();
        }

        public async Task<Album> GetAlbumByIdAsync(int id)
        {
            return await _context.Albums.Include(a => a.Artist).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Albums.AnyAsync(a => a.Id == id);
        }

        public async Task AddAlbumAsync(Album album)
        {
            await _context.Albums.AddAsync(album);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAlbumAsync(Album album)
        {
            _context.Albums.Update(album);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAlbumAsync(int id)
        {
            var album = await _context.Albums.FindAsync(id);
            if (album != null)
            {
                _context.Albums.Remove(album);
                await _context.SaveChangesAsync();
            }
        }
    }
}
