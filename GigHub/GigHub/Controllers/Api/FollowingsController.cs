using GigHub.Core;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class FollowingsController : ApiController
    {
        private IUnitOfWork _unitOfWork;

        public FollowingsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Follow(FollowingDto dto)
        {
            var userId = User.Identity.GetUserId();
            var exist = await _unitOfWork.Followings.GetFollowing(userId, dto.FolloweeId) != null;

            if(exist)
            {
                return BadRequest("Following already exist");
            }

            var following = new Following
            {
                FolloweeId = userId,
                FollowerId = dto.FolloweeId
            };

            _unitOfWork.Followings.Add(following);
            await _unitOfWork.Complete();
            return Ok();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Unfollow(string id)
        {
            var userId = User.Identity.GetUserId();

            var follow = await _unitOfWork.Followings.GetFollowing(userId, id);

            if(follow == null)
                return NotFound();

            _unitOfWork.Followings.Remove(follow);
            await _unitOfWork.Complete();

            return Ok(id);
        }
    }
}
