using Domain.Entities;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace Application.Common.Models.Excel
{
    public class ProductExcelExporter
    {
        public void ExportToExcel(List<Product> productList, string fileName)
        {
            // Create a new Excel package
            using (var package = new ExcelPackage())
            {
                // Create a new worksheet
                var worksheet = package.Workbook.Worksheets.Add("ProductsSheet");

                // Add header line
                worksheet.Cells[1, 1].Value = "OrderId";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Price";
                worksheet.Cells[1, 4].Value = "Sale Price";
                worksheet.Cells[1, 5].Value = "IsOnSale";
                worksheet.Cells[1, 6].Value = "Picture";

                // Fill products into Excel table
                for (int i = 0; i < productList.Count; i++)
                {
                    Product product = productList[i];

                    worksheet.Cells[i + 2, 1].Value = product.OrderId; // Guid to strin; .ToString()
                    worksheet.Cells[i + 2, 2].Value = product.Name;
                    worksheet.Cells[i + 2, 3].Value = product.Price;
                    worksheet.Cells[i + 2, 4].Value = product.SalePrice;
                    worksheet.Cells[i + 2, 5].Value = product.IsOnSale;
                    worksheet.Cells[i + 2, 6].Value = product.Picture;
                }

                // Save the file
                var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var filePath = Path.Combine(desktopPath, fileName);

                FileInfo fileInfo = new FileInfo(filePath);
                package.SaveAs(fileInfo);
            }
        }
    }
}
