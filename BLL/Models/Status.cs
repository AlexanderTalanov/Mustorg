using DAL.EF;


namespace BLL.Models
{
    public class StatusModel
    {
        public StatusModel() { }

        public StatusModel(Order_Status status)
        {
            Id = status.Id;
            Name = status.Name;
        }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}
