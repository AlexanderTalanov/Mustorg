using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Models;
using System.Collections.ObjectModel;
using iTextSharp;
using DAL.Interfaces;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;

namespace BLL.Services
{
    public class FileService : IFileService
    {
        IDbRepository dataBase;
        public FileService(IDbRepository dbRepository)
        {
            dataBase = dbRepository;
        }

        public void PrintExceptions(Exception ex)
        {
            string writePath = @"Exception.txt";

            try
            {
                using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(ex.Message);
                    sw.WriteLine(ex.StackTrace);
                    sw.WriteLine(ex.InnerException);
                    sw.WriteLine(ex.Source + "\n");
                }
            }
            catch (Exception e)
            {
                PrintExceptions(e);
            }
        }

        public void PrintCheque(int orderId)
        {
            try
            {
                string file = @"Check.pdf";
                var order = dataBase.Orders.GetItem(orderId);
                var customer = dataBase.Customers.GetItem((int)order.Customer_Id);
                var lines = dataBase.OrderLines.GetList().Where(i => i.Order_Id == orderId).Join(dataBase.Products.GetList(), i => i.Product_Id, pr => pr.Id, (i, pr) => new { i.Price, i.Amount, pr.Name, pr.Guarantee }).ToList();
                var coupons = dataBase.CustomerCoupons.GetList().Where(i => i.Order_Id == orderId).Join(dataBase.Coupons.GetList(), cc => cc.Coupon_Id, c => c.Id, (c,cc) => new { cc.Name, cc.Offer }).ToList();

                FileStream fs = new FileStream(file, FileMode.Create);
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);

                BaseFont baseFont = BaseFont.CreateFont("arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font font1 = new Font(baseFont, 14, Font.BOLD);
                Font font = new Font(baseFont, 12);

                document.Open();

                Paragraph header = new Paragraph("МУЗТОРГ", font1);
                Paragraph subHeader = new Paragraph("Кассовый чек", font);
                header.Alignment = Element.ALIGN_CENTER;
                subHeader.Alignment = Element.ALIGN_CENTER;
                Paragraph separator = new Paragraph("***********************************************************************************", font1);
                separator.Alignment = Element.ALIGN_CENTER;
                Paragraph productstart = new Paragraph("--------------------------- Список товаров в заказе ---------------------------", font1);
                productstart.Alignment = Element.ALIGN_CENTER;
                Paragraph productend = new Paragraph("---------------------------------------------------------------------------------------------", font1);
                productend.Alignment = Element.ALIGN_CENTER;
                Paragraph date = new Paragraph($"Дата оформления заказа: {((DateTime)order.Date).ToShortDateString()}", font);
                Paragraph name = new Paragraph($"Заказчик: {customer.Name}", font);
                Paragraph ord = new Paragraph($"Статус: {dataBase.OrderStatuses.GetItem((int)order.Order_Status_Id).Name}", font);
                Paragraph id = new Paragraph($"ID заказа: {order.Id}", font);

                document.Add(header);
                document.Add(subHeader);
                document.Add(separator);
                document.Add(new Paragraph("\n"));
                document.Add(date);
                document.Add(name);
                document.Add(ord);
                document.Add(id);
                document.Add(new Paragraph("\n"));

                document.Add(productstart);
                document.Add(new Paragraph("\n"));
                foreach (var i in lines)
                {
                    Chunk glue = new Chunk(new VerticalPositionMark());
                    Paragraph p = new Paragraph($"{i.Name}: {i.Amount} шт.", font);
                    p.Add(new Chunk(glue));
                    p.Add($"{i.Price:0.#} руб.");
                    document.Add(p);
                }
                document.Add(new Paragraph("\n"));
                document.Add(productend);
                                
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph($"Сумма без скидки: {order.Sum + order.Sale + (coupons.Count != 0 ? coupons[0].Offer : 0):0.##} руб.", font));
                document.Add(new Paragraph($"Скидка за статус: {order.Sale:0.##} руб.", font));
                if (coupons.Count != 0)
                {
                    document.Add(new Paragraph($"Купон: '{coupons[0].Name}' со скидкой {coupons[0].Offer:0} руб.",font));
                }
                document.Add(new Paragraph($"Итого: {order.Sum:0.##} руб.", font));
                document.Add(new Paragraph("\n"));
                document.Add(separator);

                document.Close();
                writer.Close();
                fs.Close();

                Process iStartProcess = new Process(); 
                iStartProcess.StartInfo.FileName = file; 
                iStartProcess.Start();
            }
            catch (Exception ex)
            {
                PrintExceptions(ex);
            }
        }

        public void PrintReport(int customerId, int statusId, DateTime dateStart, DateTime dateEnd)
        {
            try 
            {
                string file = @"Report.pdf";

                FileStream fs = new FileStream(file, FileMode.Create);
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);

                BaseFont baseFont = BaseFont.CreateFont("arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font font1 = new Font(baseFont, 14, Font.BOLD);
                Font font = new Font(baseFont, 12);

                Paragraph header = new Paragraph("МУЗТОРГ", font1);
                Paragraph subHeader = new Paragraph($"Отчёт за период: с {dateStart.ToShortDateString()} по {dateEnd.ToShortDateString()}", font);
                header.Alignment = Element.ALIGN_CENTER;
                subHeader.Alignment = Element.ALIGN_CENTER;
                Paragraph separator = new Paragraph("***********************************************************************************", font1);
                separator.Alignment = Element.ALIGN_CENTER;
                Paragraph orderStart = new Paragraph("--------------------------- Список заказов ---------------------------", font1);
                orderStart.Alignment = Element.ALIGN_CENTER;
                Paragraph orderEnd = new Paragraph("----------------------------------------------------------------------------------", font1);
                orderEnd.Alignment = Element.ALIGN_CENTER;

                document.Open();

                var orders = dataBase.Orders.GetList();
                var orderLines = dataBase.OrderLines.GetList();
                var customer = dataBase.Customers.GetItem(customerId);

                Paragraph name = new Paragraph($"Заказчик: {customer.Name}", font);

                decimal allSales = 0, Sum = 0;
                List<OrderModel> orderModels = orders.Where(i => customerId == i.Customer_Id && dateStart <= i.Date && dateEnd >= i.Date && ((statusId == -1 && (i.Order_Status_Id == 1 | i.Order_Status_Id == 2 || i.Order_Status_Id == 3)) | statusId == 0 | statusId == i.Order_Status_Id)).Select(i => new OrderModel { OrderLines = new ObservableCollection<Order_LineModel>(), Id = i.Id, Date = (DateTime)i.Date, Order_Status_Id = i.Order_Status_Id, Pick_Point_Id = i.Pick_Point_Id, Sum = i.Sum }).ToList();

                document.Add(header);
                document.Add(subHeader);
                document.Add(separator);
                document.Add(new Paragraph("\n"));
                document.Add(name);
                document.Add(new Paragraph("\n"));
                document.Add(orderStart);
                document.Add(new Paragraph("\n"));

                foreach (var ord in orderModels)
                {
                    var order = dataBase.Orders.GetItem(ord.Id);
                    var coupons = dataBase.CustomerCoupons.GetList().Where(i => i.Order_Id == ord.Id).Join(dataBase.Coupons.GetList(), cc => cc.Coupon_Id, c => c.Id, (c, cc) => new { cc.Name, cc.Offer }).ToList();
                    
                    Paragraph date = new Paragraph($"Дата оформления заказа: {((DateTime)order.Date).ToShortDateString()}", font);
                    Paragraph stat = new Paragraph($"Статус: {dataBase.OrderStatuses.GetItem((int)ord.Order_Status_Id).Name}", font);
                    Paragraph id = new Paragraph($"ID заказа: {order.Id}", font);
                    
                    document.Add(date);
                    document.Add(stat);
                    document.Add(id);
                    document.Add(new Paragraph($"Сумма без скидки: {order.Sum + order.Sale + (coupons.Count != 0 ? coupons[0].Offer : 0):0.##} руб.", font));
                    document.Add(new Paragraph($"Скидка за статус: {order.Sale:0.##} руб.", font));
                    if (coupons.Count != 0)
                    {
                        document.Add(new Paragraph($"Купон: '{coupons[0].Name}' со скидкой {coupons[0].Offer:0} руб.", font));
                    }
                    document.Add(new Paragraph($"Итого: {order.Sum:0.##} руб.", font));
                    document.Add(new Paragraph("\n"));

                    Sum += (decimal)order.Sum;
                    allSales += (decimal)(order.Sale + (coupons.Count != 0 ? coupons[0].Offer : 0));
                }
                document.Add(orderEnd);
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph($"Всего потрачено за период: {Sum:0.##} руб.", font));
                document.Add(new Paragraph($"Общая сумма скидок: {allSales:0.##} руб.", font));
                document.Add(new Paragraph("\n"));
                document.Add(separator);

                document.Close();
                writer.Close();
                fs.Close();

                Process iStartProcess = new Process();
                iStartProcess.StartInfo.FileName = file;
                iStartProcess.Start();
            }
            catch(Exception e) 
            {
                PrintExceptions(e);
            }

        }
    }
}
