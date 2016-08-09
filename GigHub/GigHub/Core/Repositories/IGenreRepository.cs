using GigHub.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GigHub.Core.Repositories
{
    public interface IGenreRepository
    {
        Task<List<Genre>> GetAll();
    }
}
