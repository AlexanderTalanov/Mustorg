using System;
using DAL.EF;

namespace BLL.Models
{
    public class ManagerModel
    {
        public ManagerModel() { }

        public ManagerModel(Manager manager)
        {
            Id = manager.Id;
            Activity_Id = (int)manager.Activity_Id;
            Pick_Point_Id = manager.Pick_Point_Id == null ? 0 : (int)manager.Pick_Point_Id;
            Name = manager.Name;
            Login = manager.Login;
            Password = manager.Password;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int Activity_Id { get; set; }
        public int Pick_Point_Id { get; set; }
        public string Activity_Name { get; set; }
        public string Pick_Point_Name { get; set; }
    }
}
