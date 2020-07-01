using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace InstagramDownloader
{
    public interface IMediaInfo
    {
        MediaType Type { get; }
        string Url { get; }
        string PreviewUrl { get; }
        Task<Stream> GetStream();
        Task<Stream> GetPreviewStream();
        IEnumerable<IMediaInfo> Slides { get; }
    }

    public enum MediaType
    {
        Image,
        Video,
        Slideshow
    }
}
