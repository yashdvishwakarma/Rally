using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RallyAPI.SharedKernel.Domain
{
    public interface IDomainEvent
    {
        Guid EventId { get; }
        DateTime OccurredAt { get; }
    }
}
