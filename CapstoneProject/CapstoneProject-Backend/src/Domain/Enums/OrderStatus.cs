using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum OrderStatus
    {
        BotStarted = 1,

        CrawlingStarted = 2,

        CrawlingCompleted = 3,

        CrawlingFailed = 4,

        OrderCompleted = 5,

    }
}
