using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using WebApplication.Interfaces;
using WebApplication.Models;
namespace WebApplication.Projections.Article
{
    public class NewArticleProjection : IProjection
    {
        public IMongoClient Client { get; set; }
        public IMongoDatabase Database { get; set; }
        public IMongoCollection<Article> ArticleCollection { get; set; }
        
        public NewArticleProjection (IMongoClient mongoClient)
        {
          Database = mongoClient.GetDatabase("ArticleAggregate");
          ArticleCollection = Database.GetCollection<Article>("Article");
        }

        public bool CanExecute(string type)
        {
            return type == "NewArticle";
        }

        public async Task<bool> ExecuteAsync(Event evnt)
        {
            try
            {
                dynamic content = evnt.Data;
                await ArticleCollection.InsertOneAsync(
                    new Article
                    {
                        Name = content.Name,
                        IsPhysical = content.IsPhysical
                    });
                    
                return true;
            } 
            catch (Exception)
            {
                throw;
            }
        }
    }
}