using GigHub.Core.Repositories;
using System.Threading.Tasks;

namespace GigHub.Core
{
    public interface IUnitOfWork
    {
        IAttendanceRepository Attendances { get; }

        IFollowingRepository Followings { get; }

        IGenreRepository Genres { get; }

        IGigRepository Gigs { get; }

        INotificationRepository Notifications { get; }

        Task Complete();
    }
}
