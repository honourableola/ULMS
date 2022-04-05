using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Paging
{
    public class PagedResult<TItemType>
    {
        public int ItemCount { get; set; }

        public int Size { get; set; }

        public int Page { get; set; }

        public int PageCount { get; set; }

        public IEnumerable<TItemType> Items { get; set; }
    }
}
