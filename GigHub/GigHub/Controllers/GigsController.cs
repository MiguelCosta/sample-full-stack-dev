using GigHub.Models;
using GigHub.Repositories;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {

        private readonly AttendanceRepository _attendanceRepository;
        private readonly ApplicationDbContext _context;
        private readonly FollowingRepository _followingRepository;
        private readonly GenreRepository _genreRepository;
        private readonly GigRepository _gigRepository;

        public GigsController()
        {
            _context = new ApplicationDbContext();
            _attendanceRepository = new AttendanceRepository(_context);
            _gigRepository = new GigRepository(_context);
            _genreRepository = new GenreRepository(_context);
            _followingRepository = new FollowingRepository(_context);
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

        [Authorize]
        public async Task<ActionResult> Create()
        {
            var viewModel = new ViewModels.GigFormViewModel
            {
                Genres = await _genreRepository.GetAll(),
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
                viewModel.Genres = await _genreRepository.GetAll();
                return View("GigForm", viewModel);
            }

            var gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };

            await _gigRepository.CreateGig(gig);

            return RedirectToAction("Mine");
        }

        public async Task<ActionResult> Details(int id)
        {
            var gig = await _gigRepository.GetGig(id);

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
                    await _followingRepository.GetFollowing(userId, gig.ArtistId) != null;

                viewModel.IsGoing = 
                    await _attendanceRepository.GetAttendance(gig.Id, userId) != null;

            }

            return View(viewModel);
        }

        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {
            var userId = User.Identity.GetUserId();

            var gig = await _gigRepository.GetGig(id);

            if(gig.ArtistId != userId)
                return new HttpUnauthorizedResult();

            var viewModel = new GigFormViewModel
            {
                Heading = "Edit a Gig",
                Genres = await _genreRepository.GetAll(),
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                Genre = gig.GenreId,
                Venue = gig.Venue,
                Id = gig.Id
            };
            return View("GigForm", viewModel);
        }

        [Authorize]
        public async Task<ActionResult> Mine()
        {
            var userId = User.Identity.GetUserId();
            var gigs = await _gigRepository.GetUpcommingGigsByArtist(userId);

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
                viewModel.Genres = await _genreRepository.GetAll();
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
    }


}
