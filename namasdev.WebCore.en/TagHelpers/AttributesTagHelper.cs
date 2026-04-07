using Microsoft.AspNetCore.Razor.TagHelpers;

namespace namasdev.WebCore.TagHelpers
{
    [HtmlTargetElement("select", Attributes = "asp-attributes")]
    public class AttributesTagHelper : TagHelper
    {
        public IDictionary<string, string> AspAttributes { get; set; } = new Dictionary<string, string>();

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            foreach (var attr in AspAttributes)
            {
                output.Attributes.SetAttribute(attr.Key, attr.Value);
            }
        }
    }
}
