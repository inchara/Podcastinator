using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using podcastinatorService.DataObjects;
using podcastinatorService.Models;
using System;
using AudioPlaybackAgent1;

namespace podcastinatorService.Controllers
{
    public class PodCastController : TableController<PodCastItem>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            podcastinatorContext context = new podcastinatorContext();
            DomainManager = new EntityDomainManager<PodCastItem>(context, Request, Services);
        }

        // GET tables/PodCast
        public IQueryable<PodCastItem> GetAllPodCastItem()
        {
            return Query(); 
        }

        // GET tables/PodCast/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<PodCastItem> GetPodCastItem(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/PodCast/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<PodCastItem> PatchPodCastItem(string id, Delta<PodCastItem> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/PodCast/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public async Task<IHttpActionResult> PostPodCastItem(PodCastItem item)
        {
            PodCastItem urlProcessedItem = this.UrlProcessor(item);
            var id = Guid.NewGuid();
            urlProcessedItem.Id = id.ToString();
            PodCastItem current = await InsertAsync(urlProcessedItem);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/PodCast/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeletePodCastItem(string id)
        {
             return DeleteAsync(id);
        }

        private PodCastItem UrlProcessor(PodCastItem podcast)
        {
            UrlProcessor urlProc = new UrlProcessor(podcast.Url);
            //string htmlContent = urlProc.GetHtmlFromUrl();
            //podcast.Title = urlProc.GetTitleFromHtml(htmlContent);
            if(podcast.Type == EntityType.Audio)
            {
                urlProc.GetMp3FileLink();
                podcast.Content = urlProc.directFileLink;
                podcast.Title = urlProc.title;
            }
            else if (podcast.Type == EntityType.Video)
            {
                urlProc.GetMp4FileLink();
                podcast.Content = urlProc.directFileLink;
                podcast.Title = urlProc.title;
            }
            else if (podcast.Type == EntityType.Text)
            {
                urlProc.GetText();
                podcast.Content = urlProc.directFileLink;
                podcast.Title = urlProc.title;
            }
            return podcast;
        }
    }
}