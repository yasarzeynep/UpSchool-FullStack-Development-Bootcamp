using System.Text;
using System.Collections.Generic;
using Domain.Entities;

namespace Application.Common.Helpers
{
    public static class MessagesHelper
    {
        public static class Email
        {
            public static class Confirmation
            {
                public static string Subject => "Order Confirmation";
                public static string Message => "We have received your order and it will be processed shortly.\n\nOrder Details:\n{{orderDetails}}\n\nBest regards,\nDataCrawler Team";

                public static string Name(string firstName) =>
                    $"Hi {firstName}";

                public static string ExcelProductFile(List<Product> products)
                {
                    // Create an HTML table from the list of products
                    var tableBuilder = new StringBuilder();

                    tableBuilder.AppendLine("<table border=\"1\">");
                    tableBuilder.AppendLine("<tr><th>Product ID</th><th>Order ID</th><th>Name</th><th>Is On Sale</th><th>Price</th><th>Sale Price</th><th>Image Path</th></tr>");

                    foreach (var product in products)
                    {
                        tableBuilder.AppendLine("<tr>");
                        tableBuilder.AppendLine($"<td>{product.Id}</td>");
                        tableBuilder.AppendLine($"<td>{product.OrderId}</td>");
                        tableBuilder.AppendLine($"<td>{product.Name}</td>");
                        tableBuilder.AppendLine($"<td>{product.IsOnSale}</td>");
                        tableBuilder.AppendLine($"<td>{product.Price}</td>");
                        tableBuilder.AppendLine($"<td>{product.SalePrice}</td>");
                        tableBuilder.AppendLine($"<td>{product.Picture}</td>");
                        tableBuilder.AppendLine("</tr>");
                    }

                    tableBuilder.AppendLine("</table>");

                    return tableBuilder.ToString();
                }
            }
        }
    }
}
