using Application.Features.Orders.Queries.GetAll;

namespace Application.Common.Models.CrawlerService;



    public class CrawlerWorkerServiceOrderDto
    {
        public OrderGetAllDto Order { get; set; }
        public string AccessToken { get; set; }

        public CrawlerWorkerServiceOrderDto(OrderGetAllDto order, string accessToken)
        {
            Order = order;
            AccessToken = accessToken;
        }

    
}