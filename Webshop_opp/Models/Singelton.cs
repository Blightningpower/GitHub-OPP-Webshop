using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Webshop_opp.Models
{
    public class Singelton : Controller
    {
        private static Singelton INSTANCE = new Singelton();

        private string ConnectionString = @"Server=192.168.45.134;Database=OPPServices;User Id=Admin;Password=Admin;";
        private SqlConnection conn;

        //private constructor
        private Singelton()
        {
            conn = new SqlConnection(ConnectionString);
        }

        public static Singelton GetInstance()
        {
            return INSTANCE;
        }


        public SqlConnection GetConnection()
        {
            return conn;
        }

        public void OpenConnection()
        {
            try
            {
                if (conn.State.ToString() == "Closed")
                {
                    conn.Open();
                }
            }
            catch (Exception error)
            {
                throw;
            }

        }

        public void CloseConnection()
        {
            try
            {
                if (conn.State.ToString() == "Open")
                {
                    conn.Close();
                }
            }
            catch (Exception error)
            {

                throw;
            }
        }
    }
}
