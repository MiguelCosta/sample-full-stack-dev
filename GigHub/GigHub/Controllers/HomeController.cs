using GigHub.Core.Repositories;
using GigHub.Core.ViewModels;
using GigHub.Persistence;
using GigHub.Persistence.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
            _attendanceRepository = new AttendanceRepository(_context);
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
            var upcomingGigs = _context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .Where(g => g.DateTime > DateTime.Now && g.IsCanceled == false);

            if(string.IsNullOrWhiteSpace(query) == false)
            {
                upcomingGigs = upcomingGigs
                    .Where(g =>
                        g.Artist.Name.Contains(query)
                        || g.Venue.Contains(query)
                        || g.Genre.Name.Contains(query));
            }

            var userId = User.Identity.GetUserId();
            var attendances = (await _attendanceRepository.GetFutureAttendances(userId))
                .ToLookup(a => a.GigId);

            var viewModel = new GigsViewModel
            {
                ShowActions = User.Identity.IsAuthenticated,
                Gigs = await upcomingGigs.ToListAsync(),
                Heading = "Upcoming Gigs",
                SearchTerm = query,
                Attendances = attendances
            };
            return View("Gigs", viewModel);
        }
    }
}
