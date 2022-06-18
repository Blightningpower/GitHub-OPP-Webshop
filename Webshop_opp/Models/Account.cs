using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using Webshop_opp.ViewModels;

namespace Webshop_opp.Models
{
    public class Account
    {
        Singelton instance = Singelton.GetInstance();

        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string postalCode { get; set; }
        public string street { get; set; }
        public string houseNumber { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string password { get; set; }
        public string province { get; set; }
        public string isEmployee { get; set; }

        public void createAccount(string firstName, string middleName, string lastName, string email, int phoneNumber, string postalCode, string street, string houseNumber, string city, string country, string password, string province)
        {
            if (String.IsNullOrEmpty(middleName))
            {
                middleName = " ";
            }
            instance.OpenConnection();           
            string sql = "INSERT INTO Account (FirstName, MiddleName, LastName, Email, PhoneNumber, PostalCode, Street, HouseNumber, City, Country, Password, IsEmployee, Province, DateCreation) VALUES(@firstName, @middleName, @lastname, @email, @phoneNumber, @postalCode, @street, @houseNumber, @city, @country, @password, @isemployee, @province, CONVERT(datetime,GETDATE()))";
            SqlCommand command = new SqlCommand(sql, instance.GetConnection());
            command.Parameters.AddWithValue("@firstName", firstName);
            command.Parameters.AddWithValue("@middleName", middleName);
            command.Parameters.AddWithValue("@lastName", lastName);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
            command.Parameters.AddWithValue("@postalCode", postalCode);
            command.Parameters.AddWithValue("@street", street);
            command.Parameters.AddWithValue("@houseNumber", houseNumber);
            command.Parameters.AddWithValue("@city", city);
            command.Parameters.AddWithValue("@country", country);
            command.Parameters.AddWithValue("@password", encrypPassword(password));
            command.Parameters.AddWithValue("@province", province);
            command.Parameters.AddWithValue("@isemployee", 1);
            command.ExecuteNonQuery();
            instance.CloseConnection();
        }

        private string encrypPassword(string password)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(password);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    password = Convert.ToBase64String(ms.ToArray());
                }
            }
            return password;
        }

        private string decrypPassword(string password)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(password);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    password = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return password;
        }

        public bool checkEmailExist(string email)
        {
            instance.OpenConnection();
            bool returnValue;
            string sql = "select AccountId from Account where Email=@email";
            SqlCommand cmd = new SqlCommand(sql, instance.GetConnection());
            cmd.Parameters.AddWithValue("@email", email);
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                returnValue = true;
            }
            else
            {
                returnValue = false;
            }
            instance.CloseConnection();
            return returnValue;
        }

        public string login(string email, string password)
        {
            instance.OpenConnection();
            string returnValue;
            string sql = "select AccountId from Account where Email=@email and Password=@password";
            SqlCommand cmd = new SqlCommand(sql, instance.GetConnection());
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@password", encrypPassword(password));
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                returnValue = sdr["AccountId"].ToString();
            }
            else
            {
                returnValue = "no";
            }
            instance.CloseConnection();
            return returnValue;
        }

        //public string getAccountData(string email, string password)
        //{
        //    instance.OpenConnection();
        //    string returnValue;
        //    string sql = "select * from Account where Email=@email and Password=@password";
        //    SqlCommand cmd = new SqlCommand(sql, instance.GetConnection());
        //    cmd.Parameters.AddWithValue("@email", email);
        //    cmd.Parameters.AddWithValue("@password", encrypPassword(password));
        //    SqlDataReader sdr = cmd.ExecuteReader();
        //    if (sdr.Read())
        //    {
        //        returnValue = sdr["AccountId"].ToString();
        //    }
        //    else
        //    {
        //        returnValue = "no";
        //    }
        //    instance.CloseConnection();
        //    return returnValue;
        //}

        public List<Account> showAccount(int id)
        {
            instance.OpenConnection();
            string sql = "SELECT * FROM Account WHERE AccountId = " + id;
            DataTable dt = new DataTable();
            SqlDataAdapter ad = new SqlDataAdapter(sql, instance.GetConnection());
            ad.Fill(dt);
            List<Account> accounts = new List<Account> { };
            string status = "no";
            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToBoolean(dr["IsEmployee"]))
                {
                    status = "yes";
                }
                Account account = new Account()
                {                   
                    firstName = dr["FirstName"].ToString(),
                    middleName = dr["MiddleName"].ToString(),
                    lastName = dr["LastName"].ToString(),
                    email = dr["Email"].ToString(),
                    phoneNumber = dr["PhoneNumber"].ToString(),
                    postalCode = dr["PostalCode"].ToString(),
                    street = dr["Street"].ToString(),
                    houseNumber = dr["HouseNumber"].ToString(),
                    city = dr["City"].ToString(),
                    country = dr["Country"].ToString(),
                    isEmployee = status,
                    province = dr["Province"].ToString()
                };

                accounts.Add(account);
            }
            instance.CloseConnection();
            return accounts;
        }

        public void editAccount(string firstName, string middleName, string lastName, string email, int phoneNumber, string postalCode, string street, string houseNumber, string city, string country, string password, string province, string id)
        {
            Console.WriteLine(id);
            if (String.IsNullOrEmpty(middleName))
            {
                middleName = " ";
            }
            instance.OpenConnection();
            string sql = "UPDATE Account SET FirstName = @firstName, MiddleName = @middleName, LastName = @lastName, Email = @email, PhoneNumber = @phoneNumber, PostalCode = @postalCode, Street = @street, HouseNumber = @houseNumber, City = @city, Country = @country, Password = @password, Province = @province WHERE AccountId = @id";
            SqlCommand command = new SqlCommand(sql, instance.GetConnection());
            command.Parameters.AddWithValue("@firstName", firstName);
            command.Parameters.AddWithValue("@middleName", middleName);
            command.Parameters.AddWithValue("@lastName", lastName);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
            command.Parameters.AddWithValue("@postalCode", postalCode);
            command.Parameters.AddWithValue("@street", street);
            command.Parameters.AddWithValue("@houseNumber", houseNumber);
            command.Parameters.AddWithValue("@city", city);
            command.Parameters.AddWithValue("@country", country);
            command.Parameters.AddWithValue("@password", encrypPassword(password));
            command.Parameters.AddWithValue("@province", province);
            command.Parameters.AddWithValue("@id", Convert.ToInt32(id));
            command.ExecuteNonQuery();
            instance.CloseConnection();
        }
    }
}
