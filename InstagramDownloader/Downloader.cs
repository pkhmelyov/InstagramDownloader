using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Js;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace InstagramDownloader
{
    public class Downloader
    {
        private const string TYPE_NAME_IMAGE = "GraphImage";
        private const string TYPE_NAME_VIDEO = "GraphVideo";
        private const string TYPE_NAME_SLIDE_SHOW = "GraphSidecar";
        private const string GET_POST_TYPE = @"
(function(data){
    return data.entry_data.PostPage[0].graphql.shortcode_media.__typename;
})(window._sharedData);
";
        private const string GET_IMAGE_URL_FROM_SINGLE_PHOTO_POST = @"
(function(data){
    return data.entry_data.PostPage[0].graphql.shortcode_media.display_resources[2].src;
})(window._sharedData);
";
        private const string GET_VIDEO_URL_FROM_SINGLE_VIDEO_POST = @"
(function(data){
    return data.entry_data.PostPage[0].graphql.shortcode_media.video_url;
})(window._sharedData);
";
        private const string GET_SLIDES_COUNT_FROM_SLIDESHOW_POST = @"
(function(data){
    return data.entry_data.PostPage[0].graphql.shortcode_media.edge_sidecar_to_children.edges.length;
})(window._sharedData);
";
        private const string GET_SLIDE_TYPE = @"
(function(data){{
    return data.entry_data.PostPage[0].graphql.shortcode_media.edge_sidecar_to_children.edges[{0}].node.__typename;
}})(window._sharedData);
";
        private const string GET_SLIDE_IMAGE_URL_FROM_SLIDESHOW_POST = @"
(function(data){{
    return data.entry_data.PostPage[0].graphql.shortcode_media.edge_sidecar_to_children.edges[{0}].node.display_resources[2].src;
}})(window._sharedData);
";
        private const string GET_SLIDE_VIDEO_URL_FROM_SLIDESHOW_POST = @"
(function(data){{
    return data.entry_data.PostPage[0].graphql.shortcode_media.edge_sidecar_to_children.edges[{0}].node.video_url;
}})(window._sharedData);
";
        public async Task<IMediaInfo> Download(string url)
        {
            var config = Configuration.Default
                .WithDefaultLoader()
                .WithJs();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(url).WaitUntilAvailable();
            var postType = document.ExecuteScript(GET_POST_TYPE).ToString();
            switch(postType)
            {
                case TYPE_NAME_IMAGE:
                    var imageUrl = document.ExecuteScript(GET_IMAGE_URL_FROM_SINGLE_PHOTO_POST).ToString();
                    return new ImageInfo(imageUrl);
                case TYPE_NAME_VIDEO:
                    var previewUrl = document.ExecuteScript(GET_IMAGE_URL_FROM_SINGLE_PHOTO_POST).ToString();
                    var videoUrl = document.ExecuteScript(GET_VIDEO_URL_FROM_SINGLE_VIDEO_POST).ToString();
                    return new VideoInfo(videoUrl, previewUrl);
                case TYPE_NAME_SLIDE_SHOW:
                    var slidesCount = int.Parse(document.ExecuteScript(GET_SLIDES_COUNT_FROM_SLIDESHOW_POST).ToString());
                    var slides = Enumerable.Range(0, slidesCount).Select<int, IMediaInfo>(i =>
                    {
                        var slideType = document.ExecuteScript(string.Format(GET_SLIDE_TYPE, i)).ToString();
                        switch (slideType)
                        {
                            case TYPE_NAME_IMAGE:
                                var slideImageUrl = document.ExecuteScript(string.Format(GET_SLIDE_IMAGE_URL_FROM_SLIDESHOW_POST, i)).ToString();
                                return new ImageInfo(slideImageUrl);
                            case TYPE_NAME_VIDEO:
                                var slidePreviewUrl = document.ExecuteScript(string.Format(GET_SLIDE_IMAGE_URL_FROM_SLIDESHOW_POST, i)).ToString();
                                var slideVideoUrl = document.ExecuteScript(string.Format(GET_SLIDE_VIDEO_URL_FROM_SLIDESHOW_POST, i)).ToString();
                                return new VideoInfo(slideVideoUrl, slidePreviewUrl);
                            default:
                                return null;
                        }
                    });
                    return new SlideshowInfo(slides);
                default:
                    return null;
            }
        }
    }
}
