using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IFileService
    {
        void PrintExceptions(Exception ex);
        void PrintCheque(int orderId);
        void PrintReport(int customerId, int statusId, DateTime dateStart, DateTime dateEnd);
    }
}
