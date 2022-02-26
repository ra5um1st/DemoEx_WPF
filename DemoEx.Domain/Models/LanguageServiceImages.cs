using System;
using System.Collections.Generic;
using System.Text;

namespace DemoEx.Domain.Models
{
    public partial class LanguageServiceImages : IEntity
    {
        public int Id { get; set; }

        public virtual Image Image { get; set; }
        public virtual LanguageService LanguageService { get; set; }
    }
}
