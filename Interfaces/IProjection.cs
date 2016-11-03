using System.Threading.Tasks;
using WebApplication.Models;
namespace WebApplication.Interfaces
{
    public interface IProjection
    {
        bool CanExecute(string type);
        Task<bool> ExecuteAsync(Event evnt);
    }
}