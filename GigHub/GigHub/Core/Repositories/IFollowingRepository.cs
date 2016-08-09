using GigHub.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GigHub.Core.Repositories
{
    public interface IFollowingRepository
    {
        Task<Following> GetFollowing(string followerId, string followeeId);

        Task<IEnumerable<Following>> GetFollowings(string followerId);
    }
}
