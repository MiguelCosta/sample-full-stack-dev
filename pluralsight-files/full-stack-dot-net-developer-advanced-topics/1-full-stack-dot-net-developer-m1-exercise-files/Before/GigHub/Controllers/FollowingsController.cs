using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers
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
        public IHttpActionResult Follow(FollowingDto dto)
        {
            var userId = User.Identity.GetUserId();

            if (_context.Followings.Any(f => f.FolloweeId == userId && f.FolloweeId == dto.FolloweeId))
                return BadRequest("Following already exists.");

            var following = new Following
            {
                FollowerId = userId,
                FolloweeId = dto.FolloweeId
            };
            _context.Followings.Add(following);
            _context.SaveChanges();

            return Ok();
        }
    }
}
