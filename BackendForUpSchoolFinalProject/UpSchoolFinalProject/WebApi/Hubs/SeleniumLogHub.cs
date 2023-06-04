
using Application.Common.Dtos;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs
{
    public class SeleniumLogHub : Hub
    {
        //Buraya yazılan methodlar clientlerı calıstırır;
        //server dısındaki her sey client olabilir
        //blazor, react uygulaması, selenium gibi
        //crawlerden buraya istek yaparak calıstırabiliriz
        public async Task SendLogNotificationAsync(SeleniumLogDto log)
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("NewSeleniumLogAdded", log);
        }
    }
}