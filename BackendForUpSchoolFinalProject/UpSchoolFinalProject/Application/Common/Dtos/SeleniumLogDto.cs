namespace Application.Common.Dtos
{
    public class SeleniumLogDto
    {
        public string Message { get; set; }
        public DateTimeOffset SentOn { get; set; }

        public Guid? Id { get; set; }

        public SeleniumLogDto(string message, Guid? id)
        {
            Message = message;
            SentOn = DateTimeOffset.Now;
            Id = id;


        }
    }
}
