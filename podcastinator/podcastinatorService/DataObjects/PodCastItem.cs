using Microsoft.WindowsAzure.Mobile.Service;

namespace podcastinatorService.DataObjects
{
    public class PodCastItem : EntityData
    {
        public string Text { get; set; }//Can be a url also in the case of media

        public bool Complete { get; set; }
    }
}