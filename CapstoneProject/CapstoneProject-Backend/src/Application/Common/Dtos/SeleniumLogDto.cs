namespace Application.Common.Dtos
{
    public class SeleniumLogDto
    {
        public string Message { get; set; }
        public DateTimeOffset SentOn { get; set; }
        public string ProductName { get; set; }
        //Property for product price
        //public decimal Price { get; set; }

        //Property for product stock status
        //public string StockStatus { get; set; }
        public Guid? Id { get; set; }
        public SeleniumLogDto(string message, Guid? id)
        {
            Message = message;
            SentOn = DateTimeOffset.Now;
            Id = id;


        }
    }
}
