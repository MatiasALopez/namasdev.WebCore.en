namespace namasdev.WebCore.Models
{
    public class OrderLinkPOST
    {
        public string? Order { get; set; }
        public string? Text { get; set; }
        public string? Operation { get; set; }
        public bool ApplyOrderDescToFirstElementOnly { get; set; }
    }
}
