using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RallyAPI.SharedKernel.Domain
{
    /// <summary>
    /// Aggregate root is an entity that owns other entities.
    /// External code should only reference aggregate roots, not child entities.
    /// Example: Order (aggregate root) owns OrderItems (child entities)
    /// </summary>
    public abstract class AggregateRoot : BaseEntity
    {
        // Aggregate roots can have additional behavior
        // For now, it's a marker class
        // In future: versioning for optimistic concurrency

        public int Version { get; protected set; }

        protected void IncrementVersion()
        {
            Version++;
            MarkAsUpdated();
        }
    }
}
