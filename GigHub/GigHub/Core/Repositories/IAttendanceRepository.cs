using GigHub.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GigHub.Core.Repositories
{
    public interface IAttendanceRepository
    {
        Task<Attendance> GetAttendance(int gigId, string attendeeId);

        Task<IEnumerable<Attendance>> GetFutureAttendances(string userId);
    }
}
