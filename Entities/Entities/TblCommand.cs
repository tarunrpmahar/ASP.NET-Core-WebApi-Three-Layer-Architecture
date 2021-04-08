using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.Entities
{
    public partial class TblCommand
    {
        public int Id { get; set; }
        public string HowTo { get; set; }
        public string Line { get; set; }
        public string Platform { get; set; }
    }
}
