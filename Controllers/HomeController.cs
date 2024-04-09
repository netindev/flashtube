using Microsoft.AspNetCore.Mvc;
using flashtube.Models;
using YoutubeDLSharp;
using System.Net;
using YoutubeDLSharp.Metadata;

namespace flashtube.Controllers
{
    public class HomeController : Controller
    {
        public string YoutubeDLPath = "/home/jose.neto/flashtube-stuff/yt-dlp_linux",
            FFmpegPath = "/home/jose.neto/flashtube-stuff/ffmpeg";

        public IActionResult Index()
        {
            YoutubeLinkModel video = new YoutubeLinkModel();
            return View(video);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost("VideoData")]
        public async Task<IActionResult> VideoData(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return BadRequest("Failed to fetch video metadata");
            }
            try
            {
                var ytdl = new YoutubeDL
                {
                    YoutubeDLPath = YoutubeDLPath,
                    FFmpegPath = FFmpegPath,
                };
                var res = await ytdl.RunVideoDataFetch(url);
                VideoData video = res.Data;
                string title = video.Title;
                var thumb = video.Thumbnail;
                return Ok(new { title = video.Title, thumb = video.Thumbnail });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("DownloadVideo")]
        public async Task<IActionResult> DownloadVideo(string Url, string Format)
        {
            if (string.IsNullOrEmpty(Url))
            {
                return BadRequest("Please provide a valid video URL.");
            }
            try
            {
                var ytdl = new YoutubeDL
                {
                    YoutubeDLPath = YoutubeDLPath,
                    FFmpegPath = FFmpegPath,
                };

                var ytModel = new YoutubeLinkModel
                {
                    UrlId = Url,
                    Format = Format
                };

                await ytModel.Download(ytdl);

                if (ytModel.Success)
                {
                    var filePath = ytModel.Path;
                    var fileName = Path.GetFileName(filePath);
                    var fileBytes = System.IO.File.ReadAllBytes(filePath);
                    var mimeTypes = MimeTypes.GetMimeType(filePath);
                    if (Format.Equals("mp3", StringComparison.CurrentCultureIgnoreCase))
                    {
                        fileName = Path.ChangeExtension(fileName, "mp3");
                    }
                    else if (Format.Equals("mp4", StringComparison.CurrentCultureIgnoreCase))
                    {
                        fileName = Path.ChangeExtension(fileName, "mp4");
                    }
                    return Ok(new { FileName = fileName, FileBytes = fileBytes });
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, ytModel.Error);
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}