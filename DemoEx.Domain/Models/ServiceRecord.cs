using System;
using System.Collections.Generic;

#nullable disable

namespace DemoEx.Domain.Models
{
    public partial class ServiceRecord : IEntity
    {
        public int Id { get; set; }
        public int? PersonId { get; set; }
        public int? ServiceId { get; set; }
        public DateTime? StartDate { get; set; }

        public virtual Person Person { get; set; }
        public virtual LaguageService Service { get; set; }
    }
}
