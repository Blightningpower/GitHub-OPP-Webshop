using Webshop_opp.Models;

namespace Webshop_opp.ViewModels
{
    public class AccountViewModel
    {
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public int phoneNumber { get; set; }
        public string postalCode { get; set; }
        public string street { get; set; }
        public string houseNumber { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string password { get; set; }
        public string province { get; set; }
        public bool isEmployee { get; set; }
        public List<Account> Accounts = new List<Account>();
    }
}
