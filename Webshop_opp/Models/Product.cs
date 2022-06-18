using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;

namespace Webshop_opp.Models
{
    public class Product
    {
        Singelton instance = Singelton.GetInstance();

        public int ProductId { get; private set; }
        public string ProductName { get; private set; }
        public string Product_description { get; private set; }
        public decimal Product_price { get; private set; }
        public decimal Product_height { get; private set; }
        public decimal Product_width { get; private set; }
        public decimal Product_weight { get; private set; }
        public string Product_image { get; private set; }

        public void createProduct(string file, string name, string description, decimal price, decimal height, decimal width, decimal weight, int categorie)
        {
            insertPrice(price);
            int priceId = getLastProductId();
            instance.OpenConnection();
            string sql = "INSERT INTO Product (ProductName, ProductDescription, Height, Width, Weight, CategoryId, Picture, PriceId) VALUES(@name, @description, @height, @width, @weight, @categorie, @image, @lastPrice)";
            SqlCommand command = new SqlCommand(sql, instance.GetConnection());
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@description", description);
            command.Parameters.AddWithValue("@height", height);
            command.Parameters.AddWithValue("@width", width);
            command.Parameters.AddWithValue("@weight", weight);
            command.Parameters.AddWithValue("@categorie", categorie);
            command.Parameters.AddWithValue("@image", file);
            command.Parameters.AddWithValue("@lastPrice", priceId);
            command.ExecuteNonQuery();
            instance.CloseConnection();          
        }

        private void insertPrice(decimal price)
        {
            instance.OpenConnection();         
            string sql = "INSERT INTO Price (Price, DatePrice) VALUES(@price, CONVERT(datetime,GETDATE()))";
            SqlCommand command = new SqlCommand(sql, instance.GetConnection());
            command.Parameters.AddWithValue("@price", price);
            command.ExecuteNonQuery();
            instance.CloseConnection();
        }

        private int getLastProductId()
        {
            instance.OpenConnection();
            string sql = "SELECT TOP(1) PriceId FROM Price";
            DataTable dt = new DataTable();
            SqlDataAdapter ad = new SqlDataAdapter(sql, instance.GetConnection());
            ad.Fill(dt);
            int PriceId = 0;
            foreach (DataRow dr in dt.Rows)
            {
                PriceId = Convert.ToInt32(dr["PriceId"]);
            }
            instance.CloseConnection();
            return PriceId;
        }

        public List<Product> showProduct()
        {
            instance.OpenConnection();
            string sql = "SELECT * FROM Product";
            DataTable dt = new DataTable();
            SqlDataAdapter ad = new SqlDataAdapter(sql, instance.GetConnection());
            ad.Fill(dt);
            List<Product> products = new List<Product> { };

            foreach (DataRow dr in dt.Rows)
            {
                Product product = new Product()
                {
                    ProductId = Convert.ToInt32(dr["ProductId"]),
                    ProductName = dr["ProductName"].ToString(),
                    Product_description = dr["ProductDescription"].ToString(),
                    Product_height = Convert.ToDecimal(dr["Height"]),
                    Product_width = Convert.ToDecimal(dr["Width"]),
                    Product_weight = Convert.ToDecimal(dr["Weight"]),
                    Product_price = Convert.ToDecimal(dr["PriceId"]),
                    Product_image = dr["Picture"].ToString()
                };
                products.Add(product);
            }

            instance.CloseConnection();
            return products;
        }

        public void deleteProduct(int product_id)
        {
            instance.OpenConnection();
            Console.WriteLine(product_id);
            string sql = "DELETE FROM Product WHERE ProductId = @id";
            SqlCommand command = new SqlCommand(sql, instance.GetConnection());
            command.Parameters.AddWithValue("@id", product_id);
            Console.WriteLine(sql);
            command.ExecuteNonQuery();
            instance.CloseConnection();
        }
    }
}
