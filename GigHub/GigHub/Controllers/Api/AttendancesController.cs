using GigHub.Core;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Persistence;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class AttendancesController : ApiController
    {
        private IUnitOfWork _unitOfWork;

        public AttendancesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Attend([FromBody] AttendanceDto dto)
        {
            var userId = User.Identity.GetUserId();

            var exist = await _unitOfWork.Attendances.GetAttendance(dto.GigId, userId) != null;

            if(exist)
            {
                return BadRequest("The attendance already exists.");
            }

            var attendance = new Attendance
            {
                GigId = dto.GigId,
                AttendeeId = userId
            };

            _unitOfWork.Attendances.Add(attendance);
            await _unitOfWork.Complete();
            return Ok();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAttendance(int id)
        {
            var userId = User.Identity.GetUserId();

            var attendance = await _unitOfWork.Attendances.GetAttendance(id, userId);

            if(attendance == null)
                return NotFound();

            _unitOfWork.Attendances.Remove(attendance);
            await _unitOfWork.Complete();

            return Ok(id);
        }
    }
}
