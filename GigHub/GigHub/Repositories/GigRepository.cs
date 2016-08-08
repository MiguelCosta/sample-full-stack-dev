using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GigHub.Repositories
{
    public class GigRepository
    {
        private ApplicationDbContext _context;

        public GigRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateGig(Gig gig)
        {
            _context.Gigs.Add(gig);
            await _context.SaveChangesAsync();
        }

        public async Task<Gig> GetGig(int id)
        {
            return await _context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .SingleOrDefaultAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Gig>> GetGigsUserAttending(string userId)
        {
            return await _context.Attendances.Where(a => a.AttendeeId == userId)
                .Select(a => a.Gig)
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .ToListAsync();
        }

        public async Task<Gig> GetGigWithAttendances(int gigId)
        {
            return await _context.Gigs
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .SingleOrDefaultAsync(g => g.Id == gigId);
        }

        public async Task<IEnumerable<Gig>> GetUpcommingGigsByArtist(string userId)
        {
            return await _context.Gigs
                .Where(g => g.ArtistId == userId
                        && g.DateTime > DateTime.Now
                        && g.IsCanceled == false)
                .Include(g => g.Genre)
                .ToListAsync();
        }
    }
}
