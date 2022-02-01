using System;
using System.Collections.Generic;

#nullable disable

namespace DemoEx.Domain.Models
{
    public partial class Gender : IEntity
    {
        public Gender()
        {
            People = new HashSet<Person>();
        }

        public int Id { get; set; }
        public string GenderName { get; set; }

        public virtual ICollection<Person> People { get; set; }
    }
}
