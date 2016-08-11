using GigHub.Core;
using GigHub.Core.ViewModels;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public async Task<ActionResult> Index(string query = null)
        {
            var upcomingGigs = await _unitOfWork.Gigs.GetUpcommingGigsAsync(query);

            var userId = User.Identity.GetUserId();
            var attendances = (await _unitOfWork.Attendances.GetFutureAttendances(userId))
                .ToLookup(a => a.GigId);

            var viewModel = new GigsViewModel
            {
                ShowActions = User.Identity.IsAuthenticated,
                Gigs = upcomingGigs,
                Heading = "Upcoming Gigs",
                SearchTerm = query,
                Attendances = attendances
            };
            return View("Gigs", viewModel);
        }
    }
}
