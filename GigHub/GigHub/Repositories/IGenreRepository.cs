using GigHub.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GigHub.Repositories
{
    public interface IGenreRepository
    {
        Task<List<Genre>> GetAll();
    }
}
