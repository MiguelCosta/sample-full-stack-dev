using GigHub.Core;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class FolloweesController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public FolloweesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Followees
        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            var artists = (await _unitOfWork.Followings.GetFollowings(userId))
                .Select(f => f.Followee);

            return View(artists);
        }
    }
}
