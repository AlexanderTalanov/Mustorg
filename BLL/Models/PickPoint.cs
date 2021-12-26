using DAL.EF;

namespace BLL.Models
{
    public class PickPointModel
    {
        public PickPointModel() { }

        public PickPointModel(Pick_Point pick_Point)
        {
            Id = pick_Point.Id;
            Name = pick_Point.Name;
        }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}
