using GigHub.Models;
using GigHub.Repositories;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly AttendanceRepository _attendanceRepository;

        private readonly GigRepository _gigRepository;

        public GigsController()
        {
            _context = new ApplicationDbContext();
            _attendanceRepository = new AttendanceRepository(_context);
            _gigRepository = new GigRepository(_context);
        }

        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var gigs = _context.Gigs
                .Where(g => g.ArtistId == userId
                        && g.DateTime > DateTime.Now
                        && g.IsCanceled == false)
                .Include(g => g.Genre)
                .ToList();

            return View(gigs);
        }

        [Authorize]
        public async Task<ActionResult> Attending()
        {
            var userId = User.Identity.GetUserId();

            var viewModel = new GigsViewModel
            {
                ShowActions = true,
                Gigs = await _gigRepository.GetGigsUserAttending(userId),
                Heading = "Gigs I'm attending",
                Attendances = (await _attendanceRepository.GetFutureAttendances(userId)).ToLookup(a => a.GigId)
            };

            return View("Gigs", viewModel);
        }

        [HttpPost]
        public ActionResult Search(GigsViewModel viewModel)
        {
            return RedirectToAction("Index", "Home", new { query = viewModel.SearchTerm });
        }

        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new ViewModels.GigFormViewModel
            {
                Genres = _context.Genres.ToList(),
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
                viewModel.Genres = await _context.Genres.ToListAsync();
                return View("GigForm", viewModel);
            }

            var gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };

            _context.Gigs.Add(gig);
            await _context.SaveChangesAsync();

            return RedirectToAction("Mine");
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();

            var gig = _context.Gigs.Single(g => g.Id == id && g.ArtistId == userId);

            var viewModel = new GigFormViewModel
            {
                Heading = "Edit a Gig",
                Genres = _context.Genres.ToList(),
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                Genre = gig.GenreId,
                Venue = gig.Venue,
                Id = gig.Id
            };
            return View("GigForm", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(GigFormViewModel viewModel)
        {
            if(ModelState.IsValid == false)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
            }

            var gig = await _gigRepository.GetGigWithAttendances(viewModel.Id);

            if(gig == null)
                return HttpNotFound();

            if(gig.ArtistId != User.Identity.GetUserId())
                return new HttpUnauthorizedResult();

            gig.Modify(viewModel.GetDateTime(), viewModel.Venue, viewModel.Genre);

            await _context.SaveChangesAsync();
            return RedirectToAction("Mine");
        }

        public async Task<ActionResult> Details(int id)
        {
            var gig = await _context.Gigs
                .Include(g => g.Artist)
                .SingleOrDefaultAsync(g => g.Id == id);

            if(gig == null)
                return HttpNotFound();

            var viewModel = new GigDetailsViewModel
            {
                Gig = gig
            };

            if(User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();

                viewModel.IsFollowing = await _context.Followings
                    .AnyAsync(f => f.FollowerId == userId && f.FolloweeId == gig.ArtistId);

                viewModel.IsGoing = await _context.Attendances
                    .AnyAsync(a => a.GigId == gig.Id && a.AttendeeId == userId);

            }

            return View(viewModel);
        }

    }


}
