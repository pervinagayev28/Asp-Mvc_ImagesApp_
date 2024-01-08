using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouTube.ConnectionApi
{
    class VideoSearch
    {
        public List<VideoInforms> SearchVideos(string apiKey, string query)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = apiKey,
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = query;
            searchListRequest.MaxResults = 5;

            var searchListResponse = searchListRequest.Execute();
            var videos = new List<VideoInforms>();

            foreach (var searchResult in searchListResponse.Items)
            {
                if (searchResult.Id.Kind == "youtube#video")
                {
                    var video = new VideoInforms
                    {
                        VideoId = searchResult.Id.VideoId,
                        Title = searchResult.Snippet.Title,
                        ThumbnailUrl = searchResult.Snippet.Thumbnails.Default__.Url
                    };
                    videos.Add(video);
                }
            }

            return videos;
        }
    }
}
