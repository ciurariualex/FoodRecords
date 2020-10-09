using Core.Data.Entities.Base;
using System;

namespace Core.Data.Entities
{
    public class Food : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Ingredients { get; set; }
        public int Minutes { get; set; }
        public double Price { get; set; }
    }
}