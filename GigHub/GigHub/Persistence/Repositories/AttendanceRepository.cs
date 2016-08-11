using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GigHub.Persistence.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private IApplicationDbContext _context;

        public AttendanceRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Attendance attendance)
        {
            _context.Attendances.Add(attendance);
        }

        public async Task<Attendance> GetAttendance(int gigId, string attendeeId)
        {
            return await _context.Attendances
                    .FirstOrDefaultAsync(a => a.GigId == gigId && a.AttendeeId == attendeeId);
        }

        public async Task<IEnumerable<Attendance>> GetFutureAttendances(string userId)
        {
            return await _context.Attendances
                            .Where(a => a.AttendeeId == userId && a.Gig.DateTime > DateTime.Now)
                            .ToListAsync();
        }

        public void Remove(Attendance attendance)
        {
            _context.Attendances.Remove(attendance);
        }
    }
}
