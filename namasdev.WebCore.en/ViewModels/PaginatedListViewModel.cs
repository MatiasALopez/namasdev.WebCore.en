using namasdev.Core.Linq;
using namasdev.WebCore.Models;

namespace namasdev.WebCore.ViewModels
{
    public class PaginatedListViewModel<TItem> : ListViewModel<TItem>
        where TItem : class
    {
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }

        public Pagination? Pagination { get; set; }

        public void SetPagination(int itemsTotalCount)
        {
            if (ItemsAvailable)
            {
                Pagination = new Pagination(Page, Items!.Count, itemsTotalCount, (int)Math.Ceiling((decimal)itemsTotalCount / ItemsPerPage));
            }
        }

        public void SetPagination(OrderAndPagingParameters op)
        {
            SetPagination(op.ItemsTotalCount);
        }

        public void OrderAndPageItems()
        {
            if (Items == null)
            {
                return;
            }

            int itemsTotalCount = Items.Count;

            Items = Items.AsQueryable()
                .Order(Order)
                .Page(Page, ItemsPerPage)
                .ToList();

            SetPagination(itemsTotalCount);
        }

        public OrderAndPagingParameters CreateOrderAndPagingParameters()
        {
            return new OrderAndPagingParameters
            {
                Order = Order,
                Page = Page,
                ItemsPerPage = ItemsPerPage
            };
        }
    }
}
