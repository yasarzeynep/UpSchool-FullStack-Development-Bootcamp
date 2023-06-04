using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Dtos
{
    public class SendLogNotificationApiDto
    {
        public SeleniumLogDto Log { get; set; }
        public string ConnectionId { get; set; }


        public SendLogNotificationApiDto(SeleniumLogDto log, string connectionId)
        {
            Log = log;

            ConnectionId = connectionId;
        }
    }
}
