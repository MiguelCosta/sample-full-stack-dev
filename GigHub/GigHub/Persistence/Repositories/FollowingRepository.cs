using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace GigHub.Persistence.Repositories
{
    public class FollowingRepository : IFollowingRepository
    {
        private IApplicationDbContext _context;

        public FollowingRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Following following)
        {
            _context.Followings.Add(following);
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

        public void Remove(Following following)
        {
            _context.Followings.Remove(following);
        }
    }
}
