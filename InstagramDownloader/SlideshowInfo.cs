using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace InstagramDownloader
{
    public class SlideshowInfo : IMediaInfo
    {
        public MediaType Type => MediaType.Slideshow;

        public string Url => throw new System.NotImplementedException();

        public string PreviewUrl => throw new System.NotImplementedException();

        public IEnumerable<IMediaInfo> Slides { get; }

        public SlideshowInfo(IEnumerable<IMediaInfo> slides)
        {
            Slides = slides;
        }

        public Task<Stream> GetPreviewStream()
        {
            throw new System.NotImplementedException();
        }

        public Task<Stream> GetStream()
        {
            throw new System.NotImplementedException();
        }
    }
}
