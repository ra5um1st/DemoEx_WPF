﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace DemoEx.Domain.Models
{
    public partial class LanguageService : IEntity
    {
        public LanguageService()
        {
            ServiceRecords = new HashSet<ServiceRecord>();
        }

        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string ImagePath { get; set; }
        public int? Duration { get; set; }
        public decimal? Cost { get; set; }
        public int? Discount { get; set; }

        [NotMapped]
        public int DiscountCost { get; set; }

        public virtual ICollection<ServiceRecord> ServiceRecords { get; set; }
    }
}
