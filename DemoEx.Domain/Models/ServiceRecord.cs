using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace DemoEx.Domain.Models
{
    public partial class ServiceRecord : IEntity
    {
        public int Id { get; set; }
        public int? PersonId { get; set; }
        public int? ServiceId { get; set; }
        public DateTime? StartDate { get; set; }

        [NotMapped]
        public TimeSpan RemainingTime
        {
            get
            {
                if(StartDate == null)
                {
                    return TimeSpan.MinValue;
                }
                return StartDate.Value - DateTime.Now;
            }
        }

        public virtual Person Person { get; set; }
        public virtual LanguageService Service { get; set; }
    }
}
