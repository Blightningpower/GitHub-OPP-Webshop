using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;

namespace Webshop_opp.Models
{
    public class Category
    {
        Singelton instance = Singelton.GetInstance();

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public List<Category> GetCategories()
        {
            instance.OpenConnection();
            string sql = "SELECT * FROM Category";
            DataTable dt = new DataTable();
            SqlDataAdapter ad = new SqlDataAdapter(sql, instance.GetConnection());
            ad.Fill(dt);
            List<Category> categories = new List<Category> { };

            foreach (DataRow dr in dt.Rows)
            {
                Category category = new Category()
                {
                    CategoryId = Convert.ToInt32(dr["CategoryId"]),
                    CategoryName = dr["CategoryName"].ToString()
                };

                categories.Add(category);
            }

            instance.CloseConnection();
            return categories;
        }

        public void create_caterory(string category_name)
        {
            instance.OpenConnection();
            string sql = "INSERT INTO Category (CategoryName, CategoryDate) VALUES(@name, CONVERT(datetime,GETDATE()))";
            SqlCommand command = new SqlCommand(sql, instance.GetConnection());
            command.Parameters.AddWithValue("@name", category_name);
            command.ExecuteNonQuery();
            instance.CloseConnection();
        }

        public void delete_caterory(int category_id)
        {
            instance.OpenConnection();
            Console.WriteLine(category_id);
            string sql = "DELETE FROM Category WHERE CategoryId = @id";
            SqlCommand command = new SqlCommand(sql, instance.GetConnection());
            command.Parameters.AddWithValue("@id", category_id);
            Console.WriteLine(sql);
            command.ExecuteNonQuery();
            instance.CloseConnection();
        }


    }
}