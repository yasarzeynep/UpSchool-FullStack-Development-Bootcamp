using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order : EntityBase<Guid>
    {
        public Guid Id { get; set; }
        public int RequestedAmount { get; set; }
        public int TotalFoundAmount { get; set; }
        public ProductCrawlType ProductCrawlType { get; set; }
        public ICollection<OrderEvent> OrderEvents { get; set; } // Bir Order'ın birden fazla orderevent'si olabilir
        public ICollection<Product> Products { get; set; } // Bir Order'ın birden fazla Products'sı olabilir
        DateTime CreatedOn { get; set; }

    }
}
