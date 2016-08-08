using GigHub.Models;
using GigHub.Repositories;
using System.Threading.Tasks;

namespace GigHub.Persistence
{
    public class UnitOfWork
    {
        private ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Attendances = new AttendanceRepository(_context);
            Followings = new FollowingRepository(_context);
            Genres = new GenreRepository(_context);
            Gigs = new GigRepository(_context);
        }

        public AttendanceRepository Attendances { get; set; }

        public FollowingRepository Followings { get; private set; }

        public GenreRepository Genres { get; set; }

        public GigRepository Gigs { get; private set; }

        public async Task Complete()
        {
            await _context.SaveChangesAsync();
        }
    }
}
