using System.Collections;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace namasdev.WebCore.Models
{
    public class DropDownListMultipleModel
    {
        public string? Id { get; set; }
        public IEnumerable<SelectListItem>? Items { get; set; }
        public IEnumerable? SelectedValues { get; set; }
        public string? EmptyOptionText { get; set; }
        public string? CssClass { get; set; }
        public Dictionary<string, string>? HtmlAttributes { get; set; }
    }
}
