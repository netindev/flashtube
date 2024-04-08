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
                    YoutubeDLPath = "/home/jose.neto/flashtube-stuff/yt-dlp_linux",
                    FFmpegPath = "/home/jose.neto/flashtube-stuff/ffmpeg"
                };
                var downloadResult = Format.Equals("mp3", StringComparison.CurrentCultureIgnoreCase) ?
                    await ytdl.RunAudioDownload(Url, YoutubeDLSharp.Options.AudioConversionFormat.Mp3) :
                    await ytdl.RunVideoDownload(Url);
                if (downloadResult.Success)
                {
                    var filePath = downloadResult.Data;
                    var fileName = Path.GetFileName(filePath);

                    var fileBytes = System.IO.File.ReadAllBytes(filePath);
                    var mimeTypes = MimeTypes.GetMimeType(filePath);

                    if (Format.Equals("mp3", StringComparison.CurrentCultureIgnoreCase))
                    {
                        fileName = Path.ChangeExtension(fileName, "mp3");
                    }

                    // Return file name along with file bytes
                    return Ok(new { FileName = fileName, FileBytes = fileBytes });
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, downloadResult.ErrorOutput);
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
