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
    public class FollowingsController : ApiController
    {
        private ApplicationDbContext _context;

        public FollowingsController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public async Task<IHttpActionResult> Follow(FollowingDto dto)
        {
            var userId = User.Identity.GetUserId();
            var exist = await _context.Followings.AnyAsync(f => f.FollowerId == userId && f.FolloweeId == dto.FolloweeId);

            if(exist)
            {
                return BadRequest("Followinf already exist");
            }

            var following = new Following
            {
                FolloweeId = userId,
                FollowerId = dto.FolloweeId
            };

            _context.Followings.Add(following);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Unfollow(string id)
        {
            var userId = User.Identity.GetUserId();

            var follow = await _context.Followings
                .SingleOrDefaultAsync(f => f.FollowerId == userId && f.FolloweeId == id);

            if(follow == null)
                return NotFound();

            _context.Followings.Remove(follow);
            await _context.SaveChangesAsync();

            return Ok(id);
        }
    }
}
