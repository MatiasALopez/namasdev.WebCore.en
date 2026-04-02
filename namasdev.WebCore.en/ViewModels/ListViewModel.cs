using namasdev.Core.Linq;

namespace namasdev.WebCore.ViewModels
{
    public class ListViewModel<TItem>
        where TItem : class
    {
        public string? Order { get; set; }
        public List<TItem>? Items { get; set; }

        public bool ItemsAvailable
        {
            get { return Items != null && Items.Any(); }
        }

        public void OrderItems()
        {
            if (ItemsAvailable
                && !string.IsNullOrWhiteSpace(Order))
            {
                Items = Items!.AsQueryable()
                    .Order(Order)
                    .ToList();
            }
        }
    }
}
