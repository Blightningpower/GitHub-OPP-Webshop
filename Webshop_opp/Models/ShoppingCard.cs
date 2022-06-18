using System.Data;
using System.Data.SqlClient;

namespace Webshop_opp.Models
{
    public class ShoppingCard
    {
        Singelton instance = Singelton.GetInstance();

        public string ProductName { get; private set; }
        public string Product_image { get; private set; }
        public string Products {get; set;}
        private string[] arrayProducts { get; set; }
        public ShoppingCard(string Products)
        {
            this.Products = Products;
        }

        private void makeArray()
        {
            arrayProducts = this.Products.Split(" ");
        }

        public List<ShoppingCard> showProduct()
        {
            makeArray();
            instance.OpenConnection();
            string whereString = "";
            for (int i = 1; i < arrayProducts.Count(); i++)
            {
                if(i == 1)
                {
                    whereString += arrayProducts[i];
                }
                else
                {
                    whereString += " OR ProductId = " + arrayProducts[i];
                }             
            }
            string sql = "SELECT * FROM Product WHERE ProductId = " + whereString;
            Console.WriteLine(sql);
            DataTable dt = new DataTable();
            SqlDataAdapter ad = new SqlDataAdapter(sql, instance.GetConnection());
            ad.Fill(dt);
            List<ShoppingCard> shoppingCards = new List<ShoppingCard> { };

            foreach (DataRow dr in dt.Rows)
            {
                ShoppingCard shoppingCard = new ShoppingCard(this.Products)
                {
                    ProductName = dr["ProductName"].ToString(),
                    Product_image = dr["Picture"].ToString()
                };
                Console.WriteLine(dr["ProductName"].ToString());
                shoppingCards.Add(shoppingCard);
            }
            instance.CloseConnection();
            return shoppingCards;
        }

        public bool checkLogin(string userId)
        {
            bool Return = false;
            if (userId != null)
            {
                Return = true;
            }
            return Return;
        }

        public void make_order(string postalCode, string street, string houseNumber, string city, string country, string province, string userId)
        {
            makeArray();
            createOrderDeliveryStatus();
            createOrderReview();
            createOrderDeleveryAdres(postalCode, street, houseNumber, city, country, province);
            createOrder(userId);
            createOrderProduct();
        }

        private void createOrderProduct()
        {
            int orderId = getLastId("[dbo].[Order]", "OrderId");
            instance.OpenConnection();
            for (int i = 1; i < arrayProducts.Count(); i++)
            {
                string sql = "INSERT INTO OrderProduct (OrderId, ProductId, TimeStamp) VALUES(@orderId, @productId, CONVERT(datetime,GETDATE()))";
                SqlCommand command = new SqlCommand(sql, instance.GetConnection());
                command.Parameters.AddWithValue("@productId", arrayProducts[i]);
                command.Parameters.AddWithValue("@orderId", orderId);
                command.ExecuteNonQuery();                
            }
            instance.CloseConnection();
        }

        private void createOrderDeleveryAdres(string postalCode, string street, string houseNumber, string city, string country, string province)
        {
            instance.OpenConnection();
            string sql = "INSERT INTO DeliveryAddress (PostalCode, StreetName, HouseNumber, City, Country, Province) VALUES(@postalCode, @street, @houseNumber, @city, @country, @province)";
            SqlCommand command = new SqlCommand(sql, instance.GetConnection());
            command.Parameters.AddWithValue("@postalCode", postalCode);
            command.Parameters.AddWithValue("@street", street);
            command.Parameters.AddWithValue("@houseNumber", houseNumber);
            command.Parameters.AddWithValue("@city", city);
            command.Parameters.AddWithValue("@country", country);
            command.Parameters.AddWithValue("@province", province);
            command.ExecuteNonQuery();
            instance.CloseConnection();
        }

        private void createOrderDeliveryStatus()
        {
            instance.OpenConnection();
            string sql = "INSERT INTO DeliveryStatus (LocationId, DeliveryStatus) VALUES(1, 'Created')";
            SqlCommand command = new SqlCommand(sql, instance.GetConnection());
            command.ExecuteNonQuery();
            instance.CloseConnection();
        }

        private void createOrderReview()
        {
            instance.OpenConnection();
            string sql = "INSERT INTO Reviews DEFAULT VALUES";
            SqlCommand command = new SqlCommand(sql, instance.GetConnection());
            command.ExecuteNonQuery();
            instance.CloseConnection();
        }

        private void createOrder(string userId)
        {
            int revieuwId = getLastId("Reviews", "ReviewId");
            int adressId = getLastId("DeliveryAddress", "AddressId");
            int deliveryStutusId = getLastId("DeliveryStatus", "DeliveryStatusId");
            instance.OpenConnection();
            string sql = "INSERT INTO [dbo].[Order] ([AccountId] ,[OrderName] ,[Height] ,[Width] ,[Weight] ,[DateOrder] ,[AddressId], [ReviewId], [DeliveryStatusId]) VALUES(@userId, 'Order', 100, 80, 50, CONVERT(datetime,GETDATE()), @adressId, @revieuwId, @deliveryStutusId)";
            SqlCommand command = new SqlCommand(sql, instance.GetConnection());
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@revieuwId", revieuwId);
            command.Parameters.AddWithValue("@adressId", adressId);
            command.Parameters.AddWithValue("@deliveryStutusId", deliveryStutusId);
            Console.WriteLine(sql);
            command.ExecuteNonQuery();
            instance.CloseConnection();
        }

        private int getLastId(string tableName, string tableId)
        {
            instance.OpenConnection();
            string sql = "SELECT TOP(1) " + tableId + " FROM " + tableName + " ORDER BY " + tableId + " DESC";
            Console.WriteLine(sql);
            DataTable dt = new DataTable();
            SqlDataAdapter ad = new SqlDataAdapter(sql, instance.GetConnection());
            ad.Fill(dt);
            int Id = 0;
            foreach (DataRow dr in dt.Rows)
            {
                Id = Convert.ToInt32(dr[tableId]);
            }
            instance.CloseConnection();
            return Id;
        }
    }
}
