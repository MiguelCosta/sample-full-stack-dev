using System;
using System.Linq;
using System.Web.Mvc;
using GigHub.Models;
using System.Data.Entity;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using GigHub.Repositories;
using System.Threading.Tasks;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly AttendanceRepository _attendanceRepository;

        public HomeController()
        {
            _context = new ApplicationDbContext();
            _attendanceRepository = new AttendanceRepository(_context);
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
    }
}