using Microsoft.WindowsAzure.Mobile.Service;

namespace podcastinatorService.DataObjects
{
    public class PodCastItem : EntityData
    {
        public string Url { get; set; }

        public EntityType Type { get; set; }

        public string Content { get; set; }

        public bool IsPlayed { get; set; }

        public string Icon { get; set; }

        public string Title { get; set; }
    }

    public enum EntityType
    {
        Audio,
        Video,
        Text
    }
}