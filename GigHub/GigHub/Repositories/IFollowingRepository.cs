using GigHub.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GigHub.Repositories
{
    public interface IFollowingRepository
    {
        Task<Following> GetFollowing(string followerId, string followeeId);
    }
}
