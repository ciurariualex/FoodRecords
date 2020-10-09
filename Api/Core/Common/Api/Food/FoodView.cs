using Core.Utils.Automapper.Interfaces;
using System;

namespace Core.Common.Api.Food
{
    public class FoodView : IFoodViewable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Ingredients { get; set; }
        public int Minutes { get; set; }
        public double Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}
