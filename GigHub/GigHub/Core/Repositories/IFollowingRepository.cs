using GigHub.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GigHub.Core.Repositories
{
    public interface IFollowingRepository
    {
        void Add(Following following);

        Task<Following> GetFollowing(string followerId, string followeeId);

        Task<IEnumerable<Following>> GetFollowings(string followerId);

        void Remove(Following following);
    }
}
