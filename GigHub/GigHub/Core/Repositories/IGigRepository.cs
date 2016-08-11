using GigHub.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GigHub.Core.Repositories
{
    public interface IGigRepository
    {
        void Add(Gig gig);

        Task<Gig> GetGigAsync(int id);

        Task<IEnumerable<Gig>> GetGigsUserAttendingAsync(string userId);

        Task<Gig> GetGigWithAttendancesAsync(int gigId);

        Task<IEnumerable<Gig>> GetUpcomingGigsByArtistAsync(string userId);

        Task<IEnumerable<Gig>> GetUpcommingGigsAsync(string query);
    }
}
