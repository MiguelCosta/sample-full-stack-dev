using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GigHub.Persistence.Repositories
{
    public class FollowingRepository : IFollowingRepository
    {
        private ApplicationDbContext _context;

        public FollowingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Following> GetFollowing(string followerId, string followeeId)
        {
            return _context.Followings
                    .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FolloweeId == followeeId);
        }

        public async Task<IEnumerable<Following>> GetFollowings(string followerId)
        {
            return await _context.Followings
                .Include(f => f.Followee)
                .Where(f => f.FollowerId == followerId)
                .ToListAsync();
        }
    }
}
