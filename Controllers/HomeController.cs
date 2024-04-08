using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using flashtube.Models;
using YoutubeDLSharp;
using System.Net;
using System.Net.Mime;

namespace flashtube.Controllers;

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

    [Route("DownloadVideo")]
    public async Task<IActionResult> DownloadVideo(string Url)
    {
        if (string.IsNullOrEmpty(Url))
        {
            return BadRequest("Please provide a valid video URL.");
        }

        try
        {
            var ytdl = new YoutubeDL();
            ytdl.YoutubeDLPath = "C:/Users/thiago.barbieri/Documents/Projects/flashtube/bin/yt-dlp.exe";
            ytdl.FFmpegPath = "C:/Users/thiago.barbieri/Documents/Projects/flashtube/bin/ffmpeg.exe";
            var downloadResult = await ytdl.RunVideoDownload(Url);

            if (downloadResult.Success)
            {
                var filePath = downloadResult.Data;
                var fileName = Path.GetFileName(filePath);

                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                var mimeTypes = MimeTypes.GetMimeType(filePath);
                return File(fileBytes, mimeTypes, fileName);
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
