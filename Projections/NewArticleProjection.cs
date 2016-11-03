using System.Threading.Tasks;
using WebApplication.Interfaces;
using WebApplication.Models;
namespace WebApplication.Projections
{
    public class NewArticleProjection : IProjection
    {
        public bool CanExecute(string type)
        {
            return type == "NewArticle";
        }

        public Task<bool> ExecuteAsync(Event evnt)
        {
            return Task.FromResult(true);
        }
    }
}