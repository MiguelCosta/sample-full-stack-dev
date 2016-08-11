using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace GigHub.Persistence.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private IApplicationDbContext _context;

        public GenreRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Genre>> GetAll()
        {
            return await _context.Genres.ToListAsync();
        }
    }
}
