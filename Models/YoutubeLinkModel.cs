using YoutubeDLSharp;
using YoutubeDLSharp.Options;

namespace flashtube.Models;

public class YoutubeLinkModel
{
    public string? UrlId { get; set; }
    public string? Format { get; set; }
    public bool Success { get; set; }
    public string? Path { get; set; }
    public string? Error { get; set; }

    public async Task<YoutubeLinkModel> Download(YoutubeDL ytdl)
    {
        var options = new OptionSet()
        {
            RecodeVideo = VideoRecodeFormat.Mp4
        };

        if (this.Format.Equals("mp3", StringComparison.CurrentCultureIgnoreCase))
        {
            var info = await ytdl.RunAudioDownload(this.UrlId, YoutubeDLSharp.Options.AudioConversionFormat.Mp3);
            this.Success = info.Success;
            foreach(var error in info.ErrorOutput)
            {
                this.Error += error + " ";
            }
            this.Path = info.Data;
            return this;


        } else if (this.Format.Equals("mp4", StringComparison.CurrentCultureIgnoreCase))
        {

            var info = await ytdl.RunVideoDownload(this.UrlId, overrideOptions: options );
            this.Success = info.Success;
            this.Path = info.Data;
            foreach(var error in info.ErrorOutput)
            {
                this.Error += error + " ";
            }
            return this;

        }
        else
        {

            var info = await ytdl.RunVideoDownload(this.UrlId);
            this.Success = info.Success;
            this.Path = info.Data;
            foreach(var error in info.ErrorOutput)
            {
                this.Error += error + " ";
            }
            return this;

        }
    }
}
