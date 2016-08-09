using GigHub.Persistence;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class GigsController : ApiController
    {
        private ApplicationDbContext _context;

        public GigsController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Cancel(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = await _context.Gigs
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .SingleAsync(g => g.Id == id && g.ArtistId == userId);

            if(gig.IsCanceled)
            {
                return NotFound();
            }

            gig.Cancel();

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
