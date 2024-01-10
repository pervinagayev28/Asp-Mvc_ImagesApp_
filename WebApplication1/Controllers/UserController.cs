using Microsoft.AspNetCore.Mvc;
using WebApplication1.ConnectionApi;
using System.IO;
using System.Text.Json;
using System;
using WebApplication1.Models;
using WebApplication1.DbContexts;
using Microsoft.EntityFrameworkCore;
using Google.Apis.YouTube.v3.Data;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        public MyImageDb context { get; }

        public UserController(MyImageDb context)
        {
            this.context = context;
        }

        public async Task<IActionResult> GetMainPageAsync(string LikedImageUrl)
        {
            //if (!string.IsNullOrEmpty(LikedImageUrl))
            //    await SeacrhImages(LikedImageUrl);
            return View();
        }
        [HttpPost]
        public async Task<string> SeacrhImages([FromBody] LikedImageUrlClass LikedImageUrl)
        {
            var data = await GetImages.GetImageFromUnsplash(LikedImageUrl.LikedImageUrl);
            foreach (var item in await context.FavoritesTb.ToListAsync())
            {
                foreach (var item1 in data)
                {
                    if (ExtractUniquePart(item.Url) == ExtractUniquePart(item1.Url))
                    {

                        item1.State = item.State;
                    }
                }

            }
            await addHistoriesAsync(data);
            return JsonSerializer.Serialize(data);
        }
        static string ExtractUniquePart(string url)
        {
            int startIndex = url.IndexOf('-', StringComparison.Ordinal) + 1;
            int endIndex = url.IndexOf('?', startIndex);

            if (startIndex > 0 && endIndex > startIndex)
            {
                if ("1579505754415-b2b7e0f3f7d8" == url.Substring(startIndex, endIndex - startIndex))
                {
                    var d = url.Substring(startIndex, endIndex - startIndex);
                }
                return url.Substring(startIndex, endIndex - startIndex);
            }

            return string.Empty;
        }
    
        private async Task addHistoriesAsync(List<IImage>data)
        {
            data.ForEach(async i =>
            {
                var temp = new Histories() { Url = i.Url, State = i.State };
                await context.HistoriesTb.AddAsync(temp);

            });
            await context.SaveChangesAsync();
        }
        public async Task<IActionResult> GetHistoriesAsync()
        {
            var data = await context.FavoritesTb.ToListAsync();
            TempData["Histories"] = await context.HistoriesTb.Cast<IImage>().ToListAsync();
            foreach (var item in TempData["Histories"] as List<IImage>)
            {
                foreach (var image in data)
                {
                    if (image.Url == item.Url)
                        item.State = image.State;
                }
            }
            return View();
        }

        public async Task<IActionResult> GetFavoritesAsync()
        {
            TempData["Favorites"] = await context.FavoritesTb.Cast<IImage>().ToListAsync();
            return View();
        }

        public IActionResult GetShorts()
        {

            return View();
        }
        public async Task<IActionResult> LikedImage([FromBody] LikedImageUrlClass LikedImageUrl)
        {

            await context.FavoritesTb.AddAsync(new Favorites()
            {
                Url = LikedImageUrl.LikedImageUrl,
                State = false
            });
            await context.SaveChangesAsync();
            return RedirectToAction(controllerName: "User", actionName: "GetMainPage");
        }
        public async Task<IActionResult> DislikedImage([FromBody] LikedImageUrlClass LikedImageUrl)
        {
            var data = context.FavoritesTb
                .AsEnumerable()
                .Where(f => ExtractUniquePart(f.Url) == ExtractUniquePart(LikedImageUrl.LikedImageUrl))
                .FirstOrDefault();
            context.FavoritesTb.Remove(data!);   
            await context.SaveChangesAsync();
            return RedirectToAction(controllerName: "User", actionName: "GetFavorites");
        }

    }
}
