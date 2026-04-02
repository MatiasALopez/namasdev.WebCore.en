using System.Collections;

namespace namasdev.WebCore.Models
{
    public class DropDownListDependentModel
    {
        public string? Id { get; set; }
        public IEnumerable? SelectedValues { get; set; }
        public string? EmptyOptionText { get; set; }
        public string? CssClass { get; set; }
        public string? ParentComboId { get; set; }
        public string? ItemsSearchUrl { get; set; }
        public string? ItemsAddedCallback { get; set; }
        public Dictionary<string, string>? HtmlAttributes { get; set; }
    }
}
