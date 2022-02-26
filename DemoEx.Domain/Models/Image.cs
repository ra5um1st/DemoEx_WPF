using System;
using System.Collections.Generic;
using System.Text;

namespace DemoEx.Domain.Models
{
    public partial class Image : IEntity
    {
        public Image()
        {
            LanguageServiceImages = new HashSet<LanguageServiceImages>();
        }

        public int Id { get; set; }
        public string Path { get; set; }

        public virtual ICollection<LanguageServiceImages> LanguageServiceImages { get; set; }
    }
}
