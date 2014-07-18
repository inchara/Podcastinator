using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using podcastinatorService.DataObjects;
using podcastinatorService.Models;

namespace podcastinatorService.Controllers
{
    public class TodoItemController : TableController<PodCastItem>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            podcastinatorContext context = new podcastinatorContext();
            DomainManager = new EntityDomainManager<PodCastItem>(context, Request, Services);
        }

        // GET tables/TodoItem
        public IQueryable<PodCastItem> GetAllTodoItems()
        {
            return Query();
        }

        // GET tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<PodCastItem> GetTodoItem(string id)
        {
            return Lookup(id);
            
        }

        // PATCH tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<PodCastItem> PatchTodoItem(string id, Delta<PodCastItem> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/TodoItem
        public async Task<IHttpActionResult> PostTodoItem(PodCastItem item)
        {
            item.Text = ExtractInformationFromUrl(item.Text);
            PodCastItem current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteTodoItem(string id)
        {
            return DeleteAsync(id);
        }
        private string ExtractInformationFromUrl(string url)
        {

            return url + "-I like to move it move it";
        }
    }
}