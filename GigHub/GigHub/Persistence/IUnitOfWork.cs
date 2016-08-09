using GigHub.Repositories;
using System.Threading.Tasks;

namespace GigHub.Persistence
{
    public interface IUnitOfWork
    {
        IAttendanceRepository Attendances { get; }

        IFollowingRepository Followings { get; }

        IGenreRepository Genres { get; }

        IGigRepository Gigs { get; }

        Task Complete();
    }
}
