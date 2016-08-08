using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GigHub.Repositories
{
    public class FollowingRepository
    {
        private ApplicationDbContext _context;

        public FollowingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Genre>> GetAll()
        {
            return await _context.Genres.ToListAsync();
        }

        public Task<Following> GetFollowing(string followerId, string followeeId)
        {
            return _context.Followings
                    .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FolloweeId == followeeId);
        }
    }
}