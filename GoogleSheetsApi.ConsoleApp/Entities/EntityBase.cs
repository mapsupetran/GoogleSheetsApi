using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheetsApi.Entities
{
    public abstract class EntityBase<T> : IEntity<T>
    {
        public T Id { get; set; }
    }

    public abstract class StringEntityBase : EntityBase<string> { }

    public abstract class IntegerEntityBase : EntityBase<int> { }
}
