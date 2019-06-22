using System;
using System.Collections.Generic;

namespace EDennis.Samples.ColorsRepo.EfCore.Models
{
    public partial class Color
    {
        public string Name { get; set; }
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
