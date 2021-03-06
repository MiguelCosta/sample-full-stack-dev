﻿using GigHub.Core;
using GigHub.Core.Repositories;
using GigHub.Persistence.Repositories;
using System.Threading.Tasks;
using System;

namespace GigHub.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Attendances = new AttendanceRepository(_context);
            Followings = new FollowingRepository(_context);
            Genres = new GenreRepository(_context);
            Gigs = new GigRepository(_context);
        }

        public IAttendanceRepository Attendances { get; private set; }

        public IFollowingRepository Followings { get; private set; }

        public IGenreRepository Genres { get; private set; }

        public IGigRepository Gigs { get; private set; }

        public INotificationRepository Notifications { get; private set; }

        public async Task Complete()
        {
            await _context.SaveChangesAsync();
        }
    }
}
