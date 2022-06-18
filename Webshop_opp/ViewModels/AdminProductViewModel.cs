using Webshop_opp.Models;

namespace Webshop_opp.ViewModels
{
    public class AdminProductViewModel
    {
        public List<Category> Categories = new List<Category>();
        public List<Product> Products = new List<Product>();
        public List<ShoppingCard> ShoppingCard = new List<ShoppingCard>();
        public string Category_name_input { get; set; }
        public string Product_name_input { get; set; }
        public string Product_description_input { get; set; }
        public decimal Product_price_input { get; set; }
        public decimal Product_height_input { get; set; }
        public decimal Product_width_input { get; set; }
        public decimal Product_weight_input { get; set; }
        public IFormFile Image { get; set; }
    }
}
