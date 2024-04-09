using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using flashtube.Models;
using YoutubeDLSharp;
using System.Net;
using System.Net.Mime;
using System.IO;
using System.Threading.Tasks;

namespace flashtube.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            YoutubeLinkModel video = new YoutubeLinkModel();
            return View(video);
        }

        public IActionResult Privacy()
        {
            return View();
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
                    YoutubeDLPath = "C:/Users/thiago.barbieri/Documents/Projects/flashtube/bin/yt-dlp.exe",
                    FFmpegPath = "C:/Users/thiago.barbieri/Documents/Projects/flashtube/bin/ffmpeg.exe",
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
                    } else if (Format.Equals("mp4", StringComparison.CurrentCultureIgnoreCase))
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
