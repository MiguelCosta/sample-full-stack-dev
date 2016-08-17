using GigHub.Core;
using GigHub.Core.Models;
using GigHub.Core.ViewModels;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public GigsController(IUnitOfWork unitOfwork)
        {
            _unitOfWork = unitOfwork;
        }

        [Authorize]
        public async Task<ActionResult> Attending()
        {
            var userId = User.Identity.GetUserId();

            var viewModel = new GigsViewModel
            {
                ShowActions = true,
                Gigs = await _unitOfWork.Gigs.GetGigsUserAttendingAsync(userId),
                Heading = "Gigs I'm attending",
                Attendances = (await _unitOfWork.Attendances.GetFutureAttendances(userId)).ToLookup(a => a.GigId)
            };

            return View("Gigs", viewModel);
        }

        [Authorize]
        public async Task<ActionResult> Create()
        {
            var viewModel = new GigFormViewModel
            {
                Genres = await _unitOfWork.Genres.GetAll(),
                Heading = "Add a Gig"
            };
            return View("GigForm", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(GigFormViewModel viewModel)
        {
            if(ModelState.IsValid == false)
            {
                viewModel.Genres = await _unitOfWork.Genres.GetAll();
                return View("GigForm", viewModel);
            }

            var gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };

            _unitOfWork.Gigs.Add(gig);
            await _unitOfWork.Complete();
            return RedirectToAction("Mine");
        }

        public async Task<ActionResult> Details(int id)
        {
            var gig = await _unitOfWork.Gigs.GetGigAsync(id);

            if(gig == null)
                return HttpNotFound();

            var viewModel = new GigDetailsViewModel
            {
                Gig = gig
            };

            if(User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();

                viewModel.IsFollowing =
                    await _unitOfWork.Followings.GetFollowing(userId, gig.ArtistId) != null;

                viewModel.IsGoing =
                    await _unitOfWork.Attendances.GetAttendance(gig.Id, userId) != null;
            }

            return View(viewModel);
        }

        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {
            var userId = User.Identity.GetUserId();

            var gig = await _unitOfWork.Gigs.GetGigAsync(id);

            if(gig.ArtistId != userId)
                return new HttpUnauthorizedResult();

            var viewModel = new GigFormViewModel
            {
                Heading = "Edit a Gig",
                Genres = await _unitOfWork.Genres.GetAll(),
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                Genre = gig.GenreId,
                Venue = gig.Venue,
                Id = gig.Id
            };
            return View("GigForm", viewModel);
        }

        [Authorize]
        public async Task<ViewResult> Mine()
        {
            var userId = User.Identity.GetUserId();
            var gigs = await _unitOfWork.Gigs.GetUpcomingGigsByArtistAsync(userId);

            return View(gigs);
        }

        [HttpPost]
        public ActionResult Search(GigsViewModel viewModel)
        {
            return RedirectToAction("Index", "Home", new { query = viewModel.SearchTerm });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(GigFormViewModel viewModel)
        {
            if(ModelState.IsValid == false)
            {
                viewModel.Genres = await _unitOfWork.Genres.GetAll();
                return View("GigForm", viewModel);
            }

            var gig = await _unitOfWork.Gigs.GetGigWithAttendancesAsync(viewModel.Id);

            if(gig == null)
                return HttpNotFound();

            if(gig.ArtistId != User.Identity.GetUserId())
                return new HttpUnauthorizedResult();

            gig.Modify(viewModel.GetDateTime(), viewModel.Venue, viewModel.Genre);

            await _unitOfWork.Complete();
            return RedirectToAction("Mine");
        }
    }
}
