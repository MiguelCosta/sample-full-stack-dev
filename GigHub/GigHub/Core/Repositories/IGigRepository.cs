using GigHub.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GigHub.Core.Repositories
{
    public interface IGigRepository
    {
        void Add(Gig gig);

        Task<Gig> GetGig(int id);

        Task<IEnumerable<Gig>> GetGigsUserAttending(string userId);

        Task<Gig> GetGigWithAttendances(int gigId);

        Task<IEnumerable<Gig>> GetUpcommingGigsByArtist(string userId);

        Task<IEnumerable<Gig>> GetUpcommingGigs(string query);
    }
}
