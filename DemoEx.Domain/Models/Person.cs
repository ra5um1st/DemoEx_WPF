using System;
using System.Collections.Generic;

#nullable disable

namespace DemoEx.Domain.Models
{
    public partial class Person : IEntity
    {
        public Person()
        {
            ServiceRecords = new HashSet<ServiceRecord>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime? RegistarionDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int? GenderId { get; set; }

        public virtual Gender Gender { get; set; }
        public virtual ICollection<ServiceRecord> ServiceRecords { get; set; }
    }
}
