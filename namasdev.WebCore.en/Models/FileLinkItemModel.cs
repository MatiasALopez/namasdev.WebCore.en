using namasdev.Core.IO;
using namasdev.Core.Types;

namespace namasdev.WebCore.Models
{
    public class FileLinkItemModel
    {
        private static readonly string[] VIEWABLE_EXTENSIONS = new[]
        {
            FileExtensions.Application.PDF,
            FileExtensions.Image.GIF,
            FileExtensions.Image.JPG,
            FileExtensions.Image.JPE,
            FileExtensions.Image.JPEG,
            FileExtensions.Image.PNG,
            FileExtensions.Image.TIF,
            FileExtensions.Image.TIFF
        };

        public string? URL { get; set; }
        public string? ClassCss { get; set; }

        public string? Name
        {
            get { return Formatter.FileNameFromUrl(URL); }
        }

        public bool IsViewable
        {
            get { return VIEWABLE_EXTENSIONS.Contains(Path.GetExtension(URL), StringComparer.CurrentCultureIgnoreCase); }
        }

        public string? DownloadURLBase { get; set; }

        public string? DownloadURL
        {
            get { return DownloadURLBase + $"&dl={(!IsViewable).ToString().ToLower()}"; }
        }
    }
}
