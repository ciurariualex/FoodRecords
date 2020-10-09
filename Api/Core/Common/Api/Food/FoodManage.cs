using System.ComponentModel.DataAnnotations;

namespace Core.Common.Api.Food
{
    public class FoodManage
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(4001)]
        public string Ingredients { get; set; }

        [Required]
        public int Minutes { get; set; }

        [Required]
        public double Price { get; set; }
    }
}