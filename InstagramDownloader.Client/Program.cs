using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InstagramDownloader.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var photoUrl = "https://www.instagram.com/p/CCDGJWVChDB/";
            var videoUrl = "https://www.instagram.com/p/CCD-3sZDmzY/";
            var longVideoUrl = "https://www.instagram.com/tv/CB_HPwgI_yp/";
            var slideShowUrl = "https://www.instagram.com/p/CCApZM1IE3Y/";
            var downloader = new Downloader();

            var media = downloader.Download(photoUrl).Result;
            SaveTo(() => media.GetStream(), "D:\\photo.jpg");

            media = downloader.Download(videoUrl).Result;
            SaveTo(() => media.GetPreviewStream(), "D:\\videoPreview.jpg");
            SaveTo(() => media.GetStream(), "D:\\video.mp4");

            media = downloader.Download(longVideoUrl).Result;
            SaveTo(() => media.GetPreviewStream(), "D:\\longVideoPreview.jpg");
            SaveTo(() => media.GetStream(), "D:\\longVideo.mp4");

            media = downloader.Download(slideShowUrl).Result;
            int number = 0;
            foreach(var slide in media.Slides)
            {
                switch(slide.Type)
                {
                    case MediaType.Image:
                        SaveTo(() => slide.GetStream(), $"D:\\photo{number}.jpg");
                        break;
                    case MediaType.Video:
                        SaveTo(() => slide.GetPreviewStream(), $"D:\\videoPreview{number}.jpg");
                        SaveTo(() => slide.GetStream(), $"D:\\video{number}.mp4");
                        break;
                }
                number++;
            }
        }

        static void SaveTo(Func<Task<Stream>> content, string filePath)
        {
            using (var stream = content().Result)
            using (var fileStream = File.OpenWrite(filePath))
                stream.CopyTo(fileStream);
        }
    }
}
