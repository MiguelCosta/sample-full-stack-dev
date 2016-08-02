using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class AttendancesController : ApiController
    {
        private ApplicationDbContext _context;

        public AttendancesController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public async Task<IHttpActionResult> Attend([FromBody] AttendanceDto dto)
        {
            var userId = User.Identity.GetUserId();

            var exist = await _context.Attendances
                            .AnyAsync(a => a.AttendeeId == userId && a.GigId == dto.GigId);

            if(exist)
            {
                return BadRequest("The attendance already exists.");
            }

            var attendance = new Attendance
            {
                GigId = dto.GigId,
                AttendeeId = userId
            };
            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAttendance(int id)
        {
            var userId = User.Identity.GetUserId();

            var attendance = await _context.Attendances
                .SingleOrDefaultAsync(a => a.AttendeeId == userId && a.GigId == id);

            if(attendance == null)
                return NotFound();

            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();

            return Ok(id);
        }
    }
}
