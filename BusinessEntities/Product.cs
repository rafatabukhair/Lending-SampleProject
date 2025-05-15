using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;

namespace BusinessEntities
{
    public class Product : IdObject
    {
        public string Name { get; set; } 
        public string Category { get; set; }
        public decimal Price { get; set; } = 0;
    }
}