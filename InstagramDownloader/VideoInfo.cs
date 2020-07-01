using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace InstagramDownloader
{
    public class VideoInfo : IMediaInfo
    {
        public MediaType Type => MediaType.Video;
        public string Url { get; }
        public string PreviewUrl { get; }

        public IEnumerable<IMediaInfo> Slides => throw new System.NotImplementedException();

        public VideoInfo(string url, string previewUrl)
        {
            Url = url;
            PreviewUrl = previewUrl;
        }

        private async Task<Stream> GetStream(string url)
        {
            using (var client = new HttpClient())
            {
                return await client.GetStreamAsync(url);
            }
        }

        public Task<Stream> GetPreviewStream()
        {
            return GetStream(PreviewUrl);
        }

        public Task<Stream> GetStream()
        {
            return GetStream(Url);
        }
    }
}
