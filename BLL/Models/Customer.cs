using System;
using DAL.EF;

namespace BLL.Models
{
    public class CustomerModel
    {
        public CustomerModel() { }

        public CustomerModel(Customer customer)
        {
            Name = customer.Name;
            Login = customer.Login;
            Password = customer.Password;
            Email = customer.Email;
            Sum = customer.Sum;
            Customer_Status_Id = customer.Customer_Status_Id;
        }

        public double Progress { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public decimal? Sum { get; set; }
        public int Customer_Status_Id { get; set; }
    }
}
