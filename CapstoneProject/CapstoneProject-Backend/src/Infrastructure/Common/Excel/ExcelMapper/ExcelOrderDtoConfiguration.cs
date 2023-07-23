using Application.Common.Models.Excel;
using ExcelMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common.Excel.ExcelMapper
{
    public class ExcelOrderDtoConfiguration : ExcelClassMap<ExcelOrderDto>
    {
        public ExcelOrderDtoConfiguration()
        {
            //Excelden ne gelecek sütunlara?
            //Map(c => c.) 
            //    .WithColumnIndex(0);

            //Map(c => c.)
            //    .WithColumnIndex(1);

            //Map(c => c.)
            //    .WithColumnIndex(2);
            //Map(c => c.)
            //    .WithColumnIndex(3)
            //    .WithInvalidFallback(null);
            //Map(c => c.)
            //    .WithColumnIndex(4)
            //    .WithInvalidFallback(null);

        }
    }
}
