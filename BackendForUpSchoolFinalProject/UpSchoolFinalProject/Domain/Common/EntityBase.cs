using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public abstract class EntityBase<TKey> : IEntityBase<TKey>, IDeletedByEntity

    {
        public TKey Id { get; set; }
      
  
        public DateTimeOffset? DeletedOn { get; set; }
        public string? DeletedByUserId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
