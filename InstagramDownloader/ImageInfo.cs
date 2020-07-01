using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace InstagramDownloader
{
    public class ImageInfo : IMediaInfo
    {
        public MediaType Type => MediaType.Image;

        public string Url { get; }

        public string PreviewUrl => throw new System.NotImplementedException();

        public IEnumerable<IMediaInfo> Slides => throw new System.NotImplementedException();

        public ImageInfo(string url)
        {
            Url = url;
        }

        public async Task<Stream> GetStream()
        {
            using (var client = new HttpClient())
            {
                return await client.GetStreamAsync(Url);
            }
        }

        public Task<Stream> GetPreviewStream()
        {
            throw new System.NotImplementedException();
        }
    }
}
